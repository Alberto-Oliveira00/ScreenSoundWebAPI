using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using ScreenSound.API.Endpoints;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using ScreenSound.Shared.Modelos.Modelos;
using System.Data.SqlTypes;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddAzureAppConfiguration("Endpoint=https://screensoundv0-configuration.azconfig.io;Id=hp7x;Secret=75ZApqDpJLAimZkE1rILWkE2KXgnFpfAI09yANNPmQKxM8cFg3IPJQQJ99BGACZoyfiWQ10HAAACAZAC3wTN");

builder.Services.AddDbContext<ScreenSoundContext>((options) => {
    options
            .UseSqlServer(builder.Configuration["ConnectionStrings:ScreenSoundDB"])
            .UseLazyLoadingProxies();
});
builder.Services.AddTransient<DAL<Artista>>();
builder.Services.AddTransient<DAL<Musica>>();
builder.Services.AddTransient<DAL<Genero>>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorAppOrigin", // Nome da sua política CORS
        policy =>
        {
            policy.WithOrigins("https://screensound-webapp-eygwgachadascdcg.brazilsouth-01.azurewebsites.net") // <<<< USE A URL EXATA DA SUA ORIGEM BLazor
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var jsonSerializerOptions = new System.Text.Json.JsonSerializerOptions
{
    ReferenceHandler = ReferenceHandler.IgnoreCycles,
    WriteIndented = true
};

app.UseStaticFiles();

app.AddEndPointsArtistas(jsonSerializerOptions);
app.AddEndPointsMusicas();
app.AddEndPointGeneros(jsonSerializerOptions);

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("BlazorAppOrigin");

app.Run();
