using GameStore.Api.Data;
using GameStore.Api.Features.Baskets.GetBasket;
using GameStore.Api.Features.Baskets.UpsertBasket;

namespace GameStore.Api.Features.Baskets
{
    public static class BasketsEndpoints
    {
        public static void MapBasket(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/baskets");

            group.MapUpsertBasketEndpoint();
            group.MapGetBasketEndpoint();
        }
    }
}
