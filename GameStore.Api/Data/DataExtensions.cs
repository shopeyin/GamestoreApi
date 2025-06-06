﻿using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data
{
    public static class DataExtensions
    {

        public static async Task InitializeDbAsync(this WebApplication app)
        {
           await app.MigrateDbAsync();
            await app.SeedDbAsync();
            app.Logger.LogInformation(18, "The database is ready");
        }
        private static async Task MigrateDbAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            GameStoreContext dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();

          await  dbContext.Database.MigrateAsync();
        }


        private static async Task SeedDbAsync(this WebApplication app)
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

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
