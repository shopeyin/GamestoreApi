using GameStore.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Features.Genres.GetGenres
{
    public static class GetGenreEndpoint
    {
        public static void MapGetGenresEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/", (GameStoreContext dbContext) => dbContext.Genres
            .Select(genre => new GenreDto(genre.Id, genre.Name)).AsNoTracking());
        }
    }
}
