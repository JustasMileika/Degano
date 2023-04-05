namespace Degano_API.Models.Entities
{
    public class User
    {

        public User(Guid id, string email, string password)
        {
            Id = id;
            Email = email;
            Password = password;
        }

        public User(Guid id, string email, string password, string stripeId)
        {
            Id = id;
            Email = email;
            Password = password;
            StripeId = stripeId;
        }

        public User() { }

        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? StripeId { get; set; }

        //public double MaxSearchDistance { get; set; }

        //public string FuelType { get; set; }

        //public List<GasStationBrand> GasStationBrands { get; set; }
    }
}
