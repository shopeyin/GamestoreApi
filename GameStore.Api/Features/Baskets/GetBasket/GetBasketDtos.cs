namespace GameStore.Api.Features.Baskets.GetBasket
{
    public record class BasketDto(
       
        Guid CustomerId,
        IEnumerable<BasketItemDto> Items
    )
    {
        public decimal TotalPrice => Items.Sum(i => i.Price * i.Quantity);
    };
    public record class BasketItemDto(
        Guid Id,
        string? Name,
        decimal Price,
        int Quantity,
        string? ImageUri
        
        
    );

}
