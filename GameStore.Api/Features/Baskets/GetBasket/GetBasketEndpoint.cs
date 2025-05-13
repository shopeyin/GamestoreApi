using System.Security.Claims;
using GameStore.Api.Data;
using GameStore.Api.Features.Baskets.Authorization;
using GameStore.Api.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Features.Baskets.GetBasket
{
    public static class GetBasketEndpoint
    {
        public static void MapGetBasketEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/{userId}", async (Guid userId, GameStoreContext dbContext,
                IAuthorizationService authorizationService,
                ClaimsPrincipal user 
                ) =>
            {

                if (userId == Guid.Empty)
                {
                    return Results.BadRequest("UserId cannot be empty");
                }

                var basket = await dbContext.Baskets.Include(basket => basket.Items).ThenInclude(b => b.Game)
                    .FirstOrDefaultAsync(b => b.Id == userId) ?? new()
                    {
                        Id = userId,

                    }; 


                var authResult = await authorizationService.AuthorizeAsync(user, basket, new OwnerOrAdminRequirement());

                if(!authResult.Succeeded)
                {
                    return Results.Forbid();
                }

                var dto = new BasketDto(
                basket.Id,
                basket.Items
            .OrderBy(item => item.Game!.Name)
            .Select(item => new BasketItemDto(
                item.GameId,
                item.Game?.Name ?? "Unknown",
                item.Game?.Price ?? 0,
                item.Quantity,
                item.Game?.ImageUri ?? string.Empty
            ))
    );

                return Results.Ok(dto);
            });
        }
    }
}
