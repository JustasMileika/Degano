namespace Degano_API.Models.Entities
{
    public class FuelType
    {
        public double? price95 { get; set; }
        public double? price98 { get; set; }
        public double? priceDiesel { get; set; }
        public double? priceLPG { get; set; }

        public FuelType() { }

        public FuelType(double? price95, double? price98, double? priceDiesel, double? priceLPG)
        {
            this.price95 = price95;
            this.price98 = price98;
            this.priceDiesel = priceDiesel;
            this.priceLPG = priceLPG;
        }
    }
}
