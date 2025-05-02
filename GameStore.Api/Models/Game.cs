using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Models
{
    public class Game
    {
        public Guid Id { get; set; }
       
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public required Genre Genre { get; set; }
        public DateTime ReleaseDate { get; set; }
        //public string ImageUrl { get; set; } = string.Empty;
        //public bool IsAvailable { get; set; } = true;
        //public int StockQuantity { get; set; } = 0;
    }
}
