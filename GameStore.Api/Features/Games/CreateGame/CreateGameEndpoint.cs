using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GameStore.Api.Data;
using GameStore.Api.Features.Games.Constants;
using GameStore.Api.Models;
using GameStore.Api.Shared.FileUpload;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Api.Features.Games.CreateGame
{
    public static class CreateGameEndpoint
    {

        private const string DefaultImageUri = "https://placehold.co/100";
        public static void MapCreateGameEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/", async (
                [FromForm] CreateGameDto newGameDto, 
                GameStoreContext dbContext, 
                ILogger<Program> logger, FileUploader fileUploader, ClaimsPrincipal user) =>
            {

                if (user.Identity?.IsAuthenticated == false)
                {
                    return Results.Unauthorized();
                }

                var currentUserId = user?.FindFirstValue(JwtRegisteredClaimNames.Sub);

                if (string.IsNullOrEmpty(currentUserId))
                {
                    return Results.Unauthorized();
                }


                var imageUri = DefaultImageUri;

                if (newGameDto.ImageFile != null)
                {
                    var uploadResult = await fileUploader.UploadFileAsync(newGameDto.ImageFile, StorageNames.GameImagesFolder);
                    if (uploadResult.IsSuccess)
                    {
                        imageUri = uploadResult.FileUrl;
                    }
                    else
                    {
                      
                        return Results.BadRequest(new { message = uploadResult.ErrorMessage });
                    }
                }

                var game = new Game
                {

                    Name = newGameDto.Name,
                    Description = newGameDto.Description,
                    Price = newGameDto.Price,
                    GenreId = newGameDto.GenreId,
                    ReleaseDate = newGameDto.ReleaseDate,
                    ImageUri = imageUri!,
                    LastUpdatedBy = currentUserId
                };

                dbContext.Games.Add(game);
                await dbContext.SaveChangesAsync();
             
                logger.LogInformation("Game created with ID {Id} and NAME {Name} :", game.Id, game.Name);
                //return Results.Created($"/games/{newGame.Id}", newGame);
                return Results.CreatedAtRoute("GetGameById", new { id = game.Id }, new GameDetailsDto(
                         game.Id,
                        game.Name,
                        game.GenreId,
                        game.Price,
                        game.ReleaseDate,
                        game.Description,
                        game.ImageUri,
                        game.LastUpdatedBy));
            }).WithParameterValidation().DisableAntiforgery();
        }
    }
}
