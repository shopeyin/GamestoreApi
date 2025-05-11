using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Features.Games.CreateGame
{
    public record CreateGameDto(
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
    )
    {
        public IFormFile? ImageFile { get; set; }
    }

    public record GameDetailsDto(Guid Id,
    string Name,
    Guid GenreId,
    decimal Price,
  DateOnly ReleaseDate,
    string Description,
     string ImageUri ,
      string LastUpdatedBy
        );
}
