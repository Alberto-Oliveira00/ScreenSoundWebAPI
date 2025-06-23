namespace ScreenSound.API.Endpoints;

using Microsoft.AspNetCore.Mvc;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using ScreenSound.API.Requests;

public static class ArtistaExtensions
{
	public static void AddEndPointsArtistas(this WebApplication app, JsonSerializerOptions jsonSerializerOptions)
	{
        app.MapGet("/Artistas", ([FromServices] DAL<Artista> dal) =>
        {
            return Results.Json(dal.Listar(), jsonSerializerOptions);
        });

        app.MapGet("/Artistas/{nome}", ([FromServices] DAL<Artista> dal, string nome) =>
        {
            var context = new ScreenSoundContext();
            var artista = context.Artistas
                                  .Include(a => a.Musicas)
                                  .FirstOrDefault(a => a.Nome.ToUpper().Equals(nome.ToUpper()));
            if (artista is null)
            {
                return Results.NotFound();
            }
            return Results.Json(artista, jsonSerializerOptions);
        });

        app.MapPost("/Artistas", ([FromServices] DAL<Artista> dal,
            [FromBody] ArtistaRequest artistaRequest) =>
        {
            var artista = new Artista(artistaRequest.nome, artistaRequest.bio);

            dal.Adicionar(artista);
            return Results.Ok();
        });

        app.MapDelete("/Artistas/{id}", ([FromServices] DAL<Artista> dal, int id) =>
        {
            var artista = dal.RecuperarPor(a => a.Id == id);

            if (artista is null)
            {
                return Results.NotFound();
            }
            dal.Deletar(artista);
            return Results.NoContent();
        });


        app.MapPut("/Artistas", ([FromServices] DAL<Artista> dal, [FromBody] Artista artista) =>
        {
            var artistaAAtualizar = dal.RecuperarPor(a => a.Id == artista.Id);
            if (artistaAAtualizar is null)
            {
                return Results.NotFound();
            }
            artistaAAtualizar.Nome = artista.Nome;
            artistaAAtualizar.Bio = artista.Bio;
            artistaAAtualizar.FotoPerfil = artista.FotoPerfil;

            dal.Atualizar(artistaAAtualizar);
            return Results.Ok();
        });
    }
}
