using GameStore.Api.Data;

namespace GameStore.Api.Features.Games.UpdateGame
{
    public static class UpdateGameEndpoint
    {

        public static void MapUpdateGame(this IEndpointRouteBuilder app)
        {
            app.MapPut("/{id}", (Guid id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
            {

              

                var existingGame = dbContext.Games.Find(id);
                if (existingGame is null)
                    return Results.NotFound();

                // Update properties
                existingGame.Name = updatedGame.Name;
                existingGame.Price = updatedGame.Price;
           
                existingGame.GenreId = updatedGame.GenreId;
                existingGame.ReleaseDate = updatedGame.ReleaseDate;
                existingGame.Description = updatedGame.Description;

                dbContext.SaveChanges();
                //return Results.Ok(existingGame);
                return Results.NoContent();
            }).WithParameterValidation();
        }
    }
}
