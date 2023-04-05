using Degano_API.Models.Entities;
using Degano_API.Services.Interfaces;
using Newtonsoft.Json;
using System;
using Stripe.Terminal;
using FireSharp.Interfaces;
using FireSharp.Config;
using FireSharp;
using FireSharp.Response;
using Domain.Interfaces.Repositories;
using System.Globalization;
using IronPython.Hosting;
using System.Text;
using System.Diagnostics;
using Microsoft.Scripting.Hosting;
using Stripe;

namespace Degano_API.Services.Implementations
{
    public class DataCollectionService : IDataCollectionService
    {

        private readonly IGasStationRepository _gasStationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        

        public DataCollectionService(IGasStationRepository gasStationRepository,
            IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _gasStationRepository = gasStationRepository;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }

        public static double ToDouble(string arg)
        {
            double res = 0;
            if (double.TryParse(arg, NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out res))
                return res;
            return -1;
        }


        public async Task UpdateGasStationData()
        {
            DateTime now = DateTime.Now;

            Console.WriteLine("updating gas station data at " + now.ToString("yyyy-MM-dd HH:mm:ss"));

            try
            {



                var fileName = _webHostEnvironment.ContentRootPath + "scripts\\dist\\webscraper.exe";

                

                var process = new ProcessStartInfo();

                //process.FileName = @"C:\Users\justa\AppData\Local\Programs\Python\Python311\python.exe";

                //var script = @"scripts/webscraper.py";
                //var script = @"C:\Users\justa\Desktop\Personal Projects\Degano\Degano API\Degano API\Scripts\webscraper.py";



                process.FileName = fileName;

                process.UseShellExecute = false;
                process.CreateNoWindow = true;
                process.WorkingDirectory = _webHostEnvironment.ContentRootPath + "scripts\\dist\\";
                process.RedirectStandardError = true;
                process.RedirectStandardOutput = true;

                var errors = "";
                var output = "";



                using(var run = Process.Start(process))
                {
                    errors = run.StandardError.ReadToEnd();
                    output = run.StandardOutput.ReadToEnd();
                }

                Console.WriteLine("ERRORS:");
                Console.WriteLine(errors);
                Console.WriteLine();
                Console.WriteLine("OUTPUT:");
                Console.WriteLine(output);

                //Console.WriteLine(output);
                //process.redire

                /*
                var engine = Python.CreateEngine();

                //Console.WriteLine(Runtime.PythonLibraryPath);

                //engine.Runtime.LoadAssembly();

                engine.SetSearchPaths(new[] { @"C:\Program Files\IronPython 3.4\Lib\site-packages", @"C:\Users\justa\AppData\Local\Programs\Python\Python311\Lib" });

                var argv = new List<string>();
                argv.Add("");

                engine.GetSysModule().SetVariable("argv", argv);

                var script = "scripts/webscraper.py";
                var source = engine.CreateScriptSourceFromFile(script);

                var eIO = engine.Runtime.IO;
                var errors = new MemoryStream();
                eIO.SetErrorOutput(errors, Encoding.Default);

                var scope = engine.CreateScope();
                source.Execute(scope);

                string str(byte[] x) => Encoding.Default.GetString(x);

                Console.WriteLine(str(errors.ToArray()));*/
            }
            catch(Exception e)
            {

            }
            //return;

            IFirebaseConfig config = new FirebaseConfig
            {
                BasePath = "https://degano-70426-default-rtdb.europe-west1.firebasedatabase.app/"
            };
            try
            {
                IFirebaseClient client = new FirebaseClient(config);
                FirebaseResponse response = await client.GetAsync("Degano/");
                Dictionary<string, DatabaseEntry> data = JsonConvert.DeserializeObject<Dictionary<string, DatabaseEntry>>(response.Body.ToString());
                Func<string, double> parser = ToDouble;

                List<GasStation> gasStations = new List<GasStation>();

                foreach (var item in data)
                {
                    Lazy<GasStation> g;
                    double dieselPrice = parser(item.Value.diesel);
                    double? lpgPrice;
                    if (item.Value.lpg != "-")
                    {
                        lpgPrice = parser(item.Value.lpg);
                    }
                    else
                    {
                        lpgPrice = null;
                    }
                    double lat = parser(item.Value.lat);
                    double lng = parser(item.Value.lng);
                    double petrol95Price = parser(item.Value.petrol95);
                    double? petrol98Price;
                    if (item.Value.petrol98 != "-")
                    {
                        petrol98Price = parser(item.Value.petrol98);
                    }
                    else
                    {
                        petrol98Price = null;
                    }
                    g = new Lazy<GasStation>(() => new GasStation(item.Value.name, item.Value.address, lat, lng,
                        petrol95Price, petrol98Price, dieselPrice, lpgPrice,
                        petrol95Price, petrol98Price, dieselPrice, lpgPrice,
                        item.Value.brand, now, now));

                    gasStations.Add(g.Value);
                    //g.Value.GetDistanceToUser();
                    //g.Value.GetDrivingDistanceToUser();
                    //GasStation.gasStationList.Add(g.Value);
                }

                foreach(var gasStation in gasStations)
                {
                    var gasStationInDb =
                        await _gasStationRepository.GetGasStationAsync(gs => gs.Address == gasStation.Address);

                    

                    if(gasStationInDb == null)
                    {
                        if(now.Hour != 0)
                        {
                            gasStation.DailyUpdateTimestamp = null;
                            gasStation.FuelPriceDaily.priceDiesel = null;
                            gasStation.FuelPriceDaily.price95 = null;
                            gasStation.FuelPriceDaily.price98 = null;
                            gasStation.FuelPriceDaily.priceLPG = null;
                        }
                        Console.WriteLine("naujas");
                        gasStation.Id = Guid.NewGuid();
                        await _gasStationRepository.AddGasStationAsync(gasStation);
                    }
                    else
                    {
                        if (now.Hour != 0)
                        {
                            gasStation.DailyUpdateTimestamp = gasStationInDb.DailyUpdateTimestamp;
                            gasStation.FuelPriceDaily.priceDiesel = gasStationInDb.FuelPriceDaily.priceDiesel;
                            gasStation.FuelPriceDaily.price95 = gasStationInDb.FuelPriceDaily.price95;
                            gasStation.FuelPriceDaily.price98 = gasStationInDb.FuelPriceDaily.price98;
                            gasStation.FuelPriceDaily.priceLPG = gasStationInDb.FuelPriceDaily.priceLPG;
                        }
                        Console.WriteLine("update");
                        //gasStation.Id = Guid.NewGuid();
                        gasStationInDb.FuelPriceHourly = gasStation.FuelPriceHourly;
                        gasStationInDb.HourlyUpdateTimestamp = gasStation.HourlyUpdateTimestamp;
                        gasStationInDb.DailyUpdateTimestamp = gasStation.DailyUpdateTimestamp;
                        gasStationInDb.FuelPriceDaily = gasStation.FuelPriceDaily;

                        //gasStationInDb.
                        //_gasStationRepository.UpdateGasStation(gasStation);
                    }
                }

                await _gasStationRepository.SaveChangesAsync();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
