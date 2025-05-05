using GameStore.Api.Data;
using GameStore.Api.Models;

namespace GameStore.Api.Features.Games.GetGame
{
    public static class GetGameEndpoint
    {
        public static void MapGetGameEndpoint(this IEndpointRouteBuilder app )
        {
            app.MapGet("/{id}", (Guid id, GameStoreContext dbContext) =>
            {
                Game? game = dbContext.Games.Find(id);

                return game is null ? Results.NotFound() : Results.Ok(
                    new GameDetailsDto(id,
                        game.Name,
                        game.GenreId,
                        game.Price,
                        game.ReleaseDate,
                        game.Description) // Map to DTO
                    );
            }).WithName("GetGameById");
        }
    }
}
