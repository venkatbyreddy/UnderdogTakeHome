using Microsoft.Net.Http.Headers;
using UnderdogTakeHome.Data;
using UnderdogTakeHome.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("Database"));

builder.Services.AddHttpClient("Cbs", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://api.cbssports.com/");
    httpClient.DefaultRequestHeaders.Add(
        HeaderNames.Accept, "application/json");
});
builder.Services.AddSingleton<PlayerService>();
builder.Services.AddSingleton<PlayerFactory>();
builder.Services.AddSingleton<ICbsSportsClient, CbsSportsClient>();

var app = builder.Build();

app.Urls.Add("http://localhost:5000");

app.MapControllers();

app.Run();