using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data
{
    public static class DataExtensions
    {

        public static void InitializeDb(this WebApplication app)
        {
            app.MigrateDb();
            app.SeedDb();
        }
        private static void MigrateDb(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            GameStoreContext dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();

            dbContext.Database.Migrate();
        }


        private static void SeedDb(this WebApplication app)
        {

            using var scope = app.Services.CreateScope();
            GameStoreContext dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();

            if (!dbContext.Genres.Any())
            {
                dbContext.Genres.AddRange(new List<Genre>
                {
                    new Genre
                    {
                       
                        Name = "Sport",
                    },
                    new Genre
                    {
                       
                        Name = "Adventure",
                    },
                    new Genre
                    {
                      
                        Name = "Fighting",
                    }
                });

                dbContext.SaveChanges();
            }
        }
    }
}
