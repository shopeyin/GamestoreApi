using GameStore.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Features.Games.GetGames
{
    public static class GetGamesEndpoint
    {

        public static void MapGetGamesEndpoint(this IEndpointRouteBuilder app)
        {
            //app.MapGet("/", (GameStoreContext dbContext) =>
            //{

            //    var gameDtos = dbContext.Games.Select(game => new GameSummaryDto(
            //        game.Id,
            //        game.Name,
            //        game.Genre!.Name,
            //        game.Price,
            //        game.ReleaseDate
            //    )).ToList();
            //    return Results.Ok(gameDtos);
            //}).WithName("GetAllGames");

            app.MapGet("/", (GameStoreContext dbContext) =>
            {

                var gameDtos =  dbContext.Games.Include(game => game.Genre)
                .Select(game => new GameSummaryDto(
                    game.Id,
                    game.Name,
                    game.Genre!.Name,
                    game.Price,
                    game.ReleaseDate
                )).AsNoTracking();
                return Results.Ok(gameDtos);
            });
        }
    }
}
