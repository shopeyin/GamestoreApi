  using GameStore.Api.Data;

using GameStore.Api.Features.Games;
using GameStore.Api.Features.Genres;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("GameStore");
//DATABASE Connection String


//builder.Services.AddDbContext<GameStoreContext>(options =>
//    options.UseSqlite(connectionString));

builder.Services.AddSqlite<GameStoreContext>(connectionString);

// Add services to the container.


var app = builder.Build();



app.MapGames();
app.MapGenres();
app.InitializeDb();

app.Run();








