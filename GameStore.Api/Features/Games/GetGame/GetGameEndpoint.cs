using System.Diagnostics;
using GameStore.Api.Data;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Features.Games.GetGame
{
    public static class GetGameEndpoint
    {
        public static void MapGetGameEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/{id}",  async (Guid id, GameStoreContext dbContext, ILogger<Program> logger) =>
            {

                    Game? game = await dbContext.Games.FindAsync(id);

                    return game is null
                        ? Results.NotFound()
                        : Results.Ok(new GameDetailsDto(
                            id,
                            game.Name,
                            game.GenreId,
                            game.Price,
                            game.ReleaseDate,
                            game.Description,
                            game.ImageUri,
                            game.LastUpdatedBy));
               
                   
                
            })
            .WithName("GetGameById");
        }
    }
}
