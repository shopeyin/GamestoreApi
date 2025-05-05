using GameStore.Api.Data;
using GameStore.Api.Models;

namespace GameStore.Api.Features.Games.CreateGame
{
    public static class CreateGameEndpoint
    {

        public static void MapCreateGameEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/", (CreateGameDto newGameDto, GameStoreContext dbContext) =>
            {

               

                var game = new Game
                {

                    Name = newGameDto.Name,
                    Description = newGameDto.Description,
                    Price = newGameDto.Price,
                    GenreId = newGameDto.GenreId,
                    ReleaseDate = newGameDto.ReleaseDate
                };

                dbContext.Games.Add(game);
                dbContext.SaveChanges();

                //return Results.Created($"/games/{newGame.Id}", newGame);
                return Results.CreatedAtRoute("GetGameById", new { id = game.Id }, new GameDetailsDto(
                         game.Id,
                        game.Name,
                        game.GenreId,
                        game.Price,
                        game.ReleaseDate,
                        game.Description));
            }).WithParameterValidation();
        }
    }
}
