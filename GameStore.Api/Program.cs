using System.Diagnostics;
using GameStore.Api.Data;

using GameStore.Api.Features.Games;
using GameStore.Api.Features.Genres;
using GameStore.Api.Features.Baskets;
using GameStore.Api.Shared.ErrorHandling;
using GameStore.Api.Shared.FileUpload;
using GameStore.Api.Shared.Timing;
using Microsoft.EntityFrameworkCore;
using GameStore.Api.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;
using GameStore.Api.Features.Baskets.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails().AddExceptionHandler<GlobalExceptionHandler>();

var connectionString = builder.Configuration.GetConnectionString("GameStore");
//DATABASE Connection String


//builder.Services.AddDbContext<GameStoreContext>(options =>
//    options.UseSqlite(connectionString));

builder.Services.AddSqlite<GameStoreContext>(connectionString);

// Add services to the container.

builder.Services.AddHttpLogging(options => { });

builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor().AddSingleton<FileUploader>();

builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters.RoleClaimType = "role";
    });

builder.AddGameStoreAuthorization();
builder.Services.AddSingleton<IAuthorizationHandler, BasketAuthorizationHandler>();

var app = builder.Build();

app.UseStaticFiles();
app.UseAuthorization();

app.MapGames();
app.MapGenres();
app.MapBasket();
app.UseHttpLogging();   
app.UseSwagger();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
}
else
{
    app.UseExceptionHandler();
}


app.UseStatusCodePages();

app.UseMiddleware<RequestTimingMiddleware>();

//await app.InitializeDbAsync();
app.Run();








