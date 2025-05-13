namespace GameStore.Api.Features.Baskets.UpsertBasket
{
    public record class UpsertBasketDtos(IEnumerable<UpsertBasketItemDto> Items);

    public record class UpsertBasketItemDto(Guid Id, int Quantity);

}
