using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScreenSound.API.Endpoints;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using System.Data.SqlTypes;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ScreenSoundContext>();
builder.Services.AddTransient<DAL<Artista>>();
builder.Services.AddTransient<DAL<Musica>>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var jsonSerializerOptions = new System.Text.Json.JsonSerializerOptions
{
    ReferenceHandler = ReferenceHandler.IgnoreCycles,
    WriteIndented = true
};

app.AddEndPointsArtistas(jsonSerializerOptions);
app.AddEndPointMusicas(jsonSerializerOptions);

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
