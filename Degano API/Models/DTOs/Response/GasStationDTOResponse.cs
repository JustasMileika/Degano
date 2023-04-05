using Degano_API.Models.Entities;

namespace Degano_API.Models.DTOs.Response
{
    public class GasStationDTOResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Type { get; set; } // type variable denotes gas station company (e.g. "Viada", "Circle-K"), whereas name can store entire name of gas station (i.e. "Viada pilaite")
        public Location Location { get; set; }

        public FuelType FuelPrice { get; set; }

        //public FuelType FuelPriceHourly { get; set; }
        //public FuelType FuelPriceDaily { get; set; }

        public DateTime UpdateTimestamp { get; set; }

        //public DateTime HourlyUpdateTimestamp { get; set; }
        //public DateTime? DailyUpdateTimestamp { get; set; }


        public GasStationDTOResponse() { }

        public GasStationDTOResponse(Guid id, string name, string address, double latitude, double longitude,
            double? price95, double? price98, double? priceDiesel, double? priceLPG,
            string brand, DateTime updateTimestamp)
        {
            Id = id;
            Name = name;
            Address = address;
            Type = brand;
            UpdateTimestamp = updateTimestamp;
            Location = new Location(latitude, longitude);
            FuelPrice = new FuelType(price95, price98, priceDiesel, priceLPG);
        }
    }
}
