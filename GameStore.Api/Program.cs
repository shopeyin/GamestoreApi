using System.ComponentModel.DataAnnotations;
using GameStore.Api.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<Genre> genres = new List<Genre>
{
    new Genre  {
        Id = Guid.NewGuid(),
        Name = "Sport",
    },
      new Genre  {
        Id = Guid.NewGuid(),
        Name = "Adventure",
    },

        new Genre  {
        Id = Guid.NewGuid(),
        Name = "Fighting",
    },
};


var games = new List<Game>
{
    new Game
    {
        Id = Guid.NewGuid(),
        Name = "The Witcher 3: Wild Hunt",
        Description = "An open-world Witcher 3 set in a fantasy universe.",
        Price = 29.99m,
        Genre = genres[0],
        ReleaseDate = new DateTime(2015, 5, 19)
    },
    new Game
    {
        Id = Guid.NewGuid(),
        Name = "God of War",
        Description = "An action-adventure game set in Norse mythology.",
        Price = 49.99m,
        Genre = genres[1],
        ReleaseDate = new DateTime(2018, 4, 20)
    },
    new Game
    {
        Id = Guid.NewGuid(),
        Name = "Minecraft",
        Description = "A sandbox game that allows players to build and explore virtual worlds.",
        Price = 26.95m,
        Genre = genres[2],
        ReleaseDate = new DateTime(2011, 11, 18)
    },
    new Game
    {
        Id = Guid.NewGuid(),
        Name = "Hades",
        Description = "A rogue-like dungeon crawler set in the Greek underworld.",
        Price = 24.99m,
        Genre = genres[0],
        ReleaseDate = new DateTime(2020, 9, 17)
    },
    new Game
    {
        Id = Guid.NewGuid(),
        Name = "FIFA 24",
        Description = "The latest installment in the FIFA series, featuring updated teams and gameplay.",
        Price = 69.99m,
        Genre = genres[0], 
        ReleaseDate = new DateTime(2023, 9, 29)
    }
};


app.MapGet("/games", () => games.Select(game => new GameSummaryDto(game.Id, game.Name, game.Genre.Name, game.Price, game.ReleaseDate)));

app.MapGet("/games/{id}", (Guid id) =>
{
   Game? game =  games.Find(game => game.Id == id);

    return game is null ? Results.NotFound() : Results.Ok(
        new GameDetailsDto(id,
            game.Name,
            game.Genre.Id,
            game.Price,
            game.ReleaseDate,
            game.Description) // Map to DTO
        );
}).WithName("GetGameById");

app.MapPost("/games", (CreateGameDto newGameDto) => { 

    var genre = genres.Find(genre => genre.Id == newGameDto.GenreId);

    if (genre is null)
        return Results.BadRequest("Genre not found");

   var game = new Game
   {
       Id = Guid.NewGuid(),
       Name = newGameDto.Name,
       Description = newGameDto.Description,
       Price = newGameDto.Price,
       Genre = genre,
       ReleaseDate = newGameDto.ReleaseDate
   };

    games.Add(game);

    //return Results.Created($"/games/{newGame.Id}", newGame);
    return Results.CreatedAtRoute("GetGameById", new {id = game.Id}, new GameDetailsDto(
             game.Id,
            game.Name,
            game.Genre.Id,
            game.Price,
            game.ReleaseDate,
            game.Description) );
}).WithParameterValidation();


app.MapPut("/games/{id}", (Guid id, UpdateGameDto updatedGame) =>
{

    var genre = genres.Find(genre => genre.Id == updatedGame.GenreId);

    if (genre is null)
        return Results.BadRequest("Genre not found");

    var existingGame = games.Find(g => g.Id == id);
    if (existingGame is null)
        return Results.NotFound();

    // Update properties
    existingGame.Name = updatedGame.Name;
    existingGame.Price = updatedGame.Price;
    existingGame.Genre = genre;
    existingGame.ReleaseDate = updatedGame.ReleaseDate;
    existingGame.Description = updatedGame.Description;

    return Results.Ok(existingGame);
}).WithParameterValidation();

app.MapDelete("/games/{id}", (Guid id) =>
{
    var game = games.Find(g => g.Id == id);
    if (game is null)
        return Results.NotFound();

    games.Remove(game);
    return Results.NoContent(); // 204 No Content
});

app.MapGet("/genres", () => genres.Select(genre => new GenreDto(genre.Id, genre.Name)));

app.Run();


public record GameDetailsDto(Guid Id, 
    string Name, 
    Guid GenreId, 
    decimal Price, 
    DateTime ReleaseDate, 
    string Description);


public record GameSummaryDto(
    Guid Id,
    string Name,
   string Genre,
    decimal Price,
    DateTime ReleaseDate
    );

public record GenreDto(
    Guid Id,
    string Name);


public record CreateGameDto(
   [Required]
   [StringLength(30)]
    string Name,

   Guid GenreId,

   [Range(1, 100.00)]
    decimal Price,

    DateTime ReleaseDate,

     [Required]
   [StringLength(530)]
     string Description
    );


public record UpdateGameDto(
   [Required]
   [StringLength(30)]
    string Name,

   Guid GenreId,

   [Range(1, 100.00)]
    decimal Price,

    DateTime ReleaseDate,

     [Required]
   [StringLength(530)]
     string Description
    );
