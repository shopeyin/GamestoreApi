using GameStore.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Features.Games.DeleteGame
{
    public static class DeleteGameEndpoint
    {

        public static void MapDeleteGame(this IEndpointRouteBuilder app)
        {
            app.MapDelete("/{id}", async (Guid id, GameStoreContext dbContext) =>
            {
              
               await dbContext.Games.Where(g => g.Id == id).ExecuteDeleteAsync();
             

                return Results.NoContent(); // 204 No Content
            });
        }
    }
}
