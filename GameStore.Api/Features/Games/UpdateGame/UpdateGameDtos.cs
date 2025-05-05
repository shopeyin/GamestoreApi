using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Features.Games.UpdateGame
{
    public record UpdateGameDto(
   [Required]
   [StringLength(30)]
    string Name,

   Guid GenreId,

   [Range(1, 100.00)]
    decimal Price,

    DateOnly ReleaseDate,

     [Required]
   [StringLength(530)]
     string Description
    );

}
