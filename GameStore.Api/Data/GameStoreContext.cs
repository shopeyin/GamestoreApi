using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data
{
    public class GameStoreContext(DbContextOptions<GameStoreContext> options) : DbContext(options)
    {

        public DbSet<Genre> Genres => Set<Genre>();
        public DbSet<Game> Games => Set<Game>();

        public DbSet<BasketItem> BasketItems => Set<BasketItem>();
        public DbSet<CustomerBasket> Baskets => Set<CustomerBasket>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>().HasKey(g => g.Id);
            modelBuilder.Entity<Game>().HasKey(g => g.Id);
           
        }
    }
    

}
