namespace Degano_API.Models.Entities
{
    public class GasStation
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Type { get; set; } // type variable denotes gas station company (e.g. "Viada", "Circle-K"), whereas name can store entire name of gas station (i.e. "Viada pilaite")
        public Location Location { get; set; }
        public FuelType FuelPriceHourly { get; set; }
        public FuelType FuelPriceDaily { get; set; }
        public DateTime HourlyUpdateTimestamp { get; set; }
        public DateTime? DailyUpdateTimestamp { get; set; }


        public GasStation() { }

        public GasStation(string name, string address, double latitude, double longitude,
            double? price95Hourly, double? price98Hourly, double? priceDieselHourly, double? priceLPGHourly,
            double? price95Daily, double? price98Daily, double? priceDieselDaily, double? priceLPGDaily,
            string brand, DateTime hourlyUpdateTimestamp, DateTime? dailyUpdateTimestamp)
        {
            Name = name;
            Address = address;
            Type = brand;
            HourlyUpdateTimestamp = hourlyUpdateTimestamp;
            DailyUpdateTimestamp = dailyUpdateTimestamp;
            Location = new Location(latitude, longitude);
            FuelPriceHourly = new FuelType(price95Hourly, price98Hourly, priceDieselHourly, priceLPGHourly);
            FuelPriceDaily = new FuelType(price95Daily, price98Daily, priceDieselDaily, priceLPGDaily);
        }

        public GasStation(Guid id, string name, string address, double latitude, double longitude,
            double? price95Hourly, double? price98Hourly, double? priceDieselHourly, double? priceLPGHourly,
            double? price95Daily, double? price98Daily, double? priceDieselDaily, double? priceLPGDaily,
            string brand, DateTime hourlyUpdateTimestamp, DateTime? dailyUpdateTimestamp)
        {
            Id = id;
            Name = name;
            Address = address;
            Type = brand;
            HourlyUpdateTimestamp = hourlyUpdateTimestamp;
            DailyUpdateTimestamp = dailyUpdateTimestamp;
            Location = new Location(latitude, longitude);
            FuelPriceHourly = new FuelType(price95Hourly, price98Hourly, priceDieselHourly, priceLPGHourly);
            FuelPriceDaily = new FuelType(price95Daily, price98Daily, priceDieselDaily, priceLPGDaily);
        }
    }
}
