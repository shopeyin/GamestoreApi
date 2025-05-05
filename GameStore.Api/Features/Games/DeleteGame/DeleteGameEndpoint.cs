using GameStore.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Features.Games.DeleteGame
{
    public static class DeleteGameEndpoint
    {

        public static void MapDeleteGame(this IEndpointRouteBuilder app)
        {
            app.MapDelete("/{id}", (Guid id, GameStoreContext dbContext) =>
            {
              
                dbContext.Games.Where(g => g.Id == id).ExecuteDelete();
             

                return Results.NoContent(); // 204 No Content
            });
        }
    }
}
