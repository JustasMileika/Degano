using Degano_API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Degano_API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public DbSet<Offer> Offers { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Subscription> Subscriptions { get; set; }

        public DbSet<GasStation> GasStations { get; set; }

        //public DbSet<>


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Subscription>().HasKey(subscription => new {subscription.UserId, subscription.OfferId});

            //modelBuilder.Entity<GasStation>().HasKey()

            modelBuilder.Entity<GasStation>().OwnsOne(gs => gs.Location);
            modelBuilder.Entity<GasStation>().OwnsOne(gs => gs.FuelPriceDaily);
            modelBuilder.Entity<GasStation>().OwnsOne(gs => gs.FuelPriceHourly);



            /*modelBuilder.Entity<Offer>()
                .HasMany(rest => rest.Products)
                .WithOne(prd => prd.Restaurant)
                .HasForeignKey(fk => fk.RestaurantID)
                .OnDelete(DeleteBehavior.Cascade);*/

        }
    }
}
