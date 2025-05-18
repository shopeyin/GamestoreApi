using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GameStore.Api.Data;
using GameStore.Api.Features.Games.Constants;
using GameStore.Api.Shared.Authorization;
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
                GameStoreContext dbContext, FileUploader fileUploader, ClaimsPrincipal user) =>
            {

                var currentUserId = user?.FindFirstValue(JwtRegisteredClaimNames.Email) ?? user?.FindFirstValue(JwtRegisteredClaimNames.Sub);


                if (string.IsNullOrEmpty(currentUserId))
                {
                    return Results.Unauthorized();
                }
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
                existingGame.LastUpdatedBy = currentUserId;
                await dbContext.SaveChangesAsync();
              
                return Results.NoContent();
            }).WithParameterValidation().DisableAntiforgery().RequireAuthorization(Policies.AdminAccess);
            ;
        }
    }
}
