namespace Degano_API.Models.Entities
{
    public class Location
    {
        public double Latitude { set; get; }
        public double Longitude { set; get; }

        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
