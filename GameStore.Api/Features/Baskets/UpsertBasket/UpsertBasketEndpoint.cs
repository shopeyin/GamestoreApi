using System.Security.Claims;
using GameStore.Api.Data;
using GameStore.Api.Features.Baskets.Authorization;
using GameStore.Api.Models;
using GameStore.Api.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Features.Baskets.UpsertBasket
{
    public static class UpsertBasketEndpoint
    {
        public static void MapUpsertBasketEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPut("/{userId}", async (Guid userId,
                UpsertBasketDtos upsertBasketDtos,
               GameStoreContext dbContext,
                 IAuthorizationService authorizationService,
                ClaimsPrincipal user
               ) =>
            {
                var basket = await dbContext.
                   Baskets.Include(basket => basket.Items).FirstOrDefaultAsync(b => b.Id == userId);

                if (basket == null)
                {
                    basket = new CustomerBasket
                    {
                        Id = userId,
                        Items = upsertBasketDtos.Items.Select(item => new BasketItem
                        {
                            GameId = item.Id,
                            Quantity = item.Quantity
                        }).ToList()
                    };
                   dbContext.Baskets.Add(basket);
                }
                else
                {
                    basket.Items = upsertBasketDtos.Items
                        .Select(item => new BasketItem
                        {
                            GameId = item.Id,
                            Quantity = item.Quantity
                        }).ToList();

                  
                }

                var authResult = await authorizationService.AuthorizeAsync(user, basket, new OwnerOrAdminRequirement());

                if (!authResult.Succeeded)
                {
                    return Results.Forbid();
                }
                await dbContext.SaveChangesAsync();
                return Results.NoContent();
            });
        }
    }
}
