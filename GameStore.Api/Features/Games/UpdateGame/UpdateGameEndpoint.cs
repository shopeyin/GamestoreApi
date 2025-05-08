using GameStore.Api.Data;
using GameStore.Api.Features.Games.Constants;
using GameStore.Api.Shared.FileUpload;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameStore.Api.Features.Games.UpdateGame
{
    public static class UpdateGameEndpoint
    {

        public static void MapUpdateGame(this IEndpointRouteBuilder app)
        {
            app.MapPut("/{id}", async (Guid id, [FromForm] UpdateGameDto updatedGame, 
                GameStoreContext dbContext, FileUploader fileUploader) =>
            {
                var existingGame = await dbContext.Games.FindAsync(id);
                if (existingGame is null)
                    return Results.NotFound();
                // Validate the uploaded file


                if (updatedGame.ImageFile != null)
                {
                    var uploadResult = await fileUploader.UploadFileAsync(updatedGame.ImageFile, StorageNames.GameImagesFolder);
                    if (uploadResult.IsSuccess)
                    {
                        existingGame.ImageUri = uploadResult.FileUrl!;
                    }
                    else
                    {

                        return Results.BadRequest(new { message = uploadResult.ErrorMessage });
                    }
                }




                // Update properties
                existingGame.Name = updatedGame.Name;
                existingGame.Price = updatedGame.Price;
                existingGame.GenreId = updatedGame.GenreId;
                existingGame.ReleaseDate = updatedGame.ReleaseDate;
                existingGame.Description = updatedGame.Description;

                await dbContext.SaveChangesAsync();
              
                return Results.NoContent();
            }).WithParameterValidation().DisableAntiforgery()
            ;
        }
    }
}
