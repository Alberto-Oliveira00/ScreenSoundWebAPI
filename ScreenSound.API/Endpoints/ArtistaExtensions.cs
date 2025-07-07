namespace ScreenSound.API.Endpoints;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScreenSound.API.Requests;
using ScreenSound.API.Response;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using System.Text.Json;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
public static class ArtistaExtensions
{
    private static ICollection<ArtistaResponse> EntityListToResponseList(IEnumerable<Artista> listaDeArtistas)
    {
        return listaDeArtistas.Select(a => EntityToResponse(a)).ToList();
    }

    private static ArtistaResponse EntityToResponse(Artista artista)
    {
        return new ArtistaResponse(artista.Id, artista.Nome, artista.Bio, artista.FotoPerfil);
    }

    public static void AddEndPointsArtistas(this WebApplication app, JsonSerializerOptions jsonSerializerOptions)
	{
        app.MapGet("/Artistas", ([FromServices] DAL<Artista> dal) =>
        {
            return Results.Json(dal.Listar(), jsonSerializerOptions);
        });

        app.MapGet("/Artistas/{nome}", ([FromServices] DAL<Artista> dal,
                                                            [FromServices] ScreenSoundContext context,
                                                            string nome) =>
        {
            var artista = context.Artistas
                                  .Include(a => a.Musicas)
                                  .FirstOrDefault(a => a.Nome.ToUpper().Equals(nome.ToUpper()));
            if (artista is null)
            {
                return Results.NotFound();
            }
            return Results.Json(artista, jsonSerializerOptions);
        });

        app.MapPost("/Artistas",async ([FromServices]IHostEnvironment env, [FromServices] DAL<Artista> dal,
            [FromBody] ArtistaRequest artistaRequest, IConfiguration configuration) =>
        {
            var nome = artistaRequest.nome.Trim();
            var imagemArtistaNomeArquivo = DateTime.Now.ToString("ddMMyyyyhhss") + "_" + nome + ".jpeg";

            var connectionString = configuration.GetConnectionString("AzureBlobStorageConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                return Results.Problem("A Connection String do Azure Blob Storage não foi configurada.");
            }

            var containerName = "fotosartistas";

            // 2. Crie um cliente para interagir com o Blob Storage
            var blobServiceClient = new BlobServiceClient(connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);

            await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var blobClient = blobContainerClient.GetBlobClient(imagemArtistaNomeArquivo);

            // 4. Faça o upload da imagem
            using MemoryStream ms = new MemoryStream(Convert.FromBase64String(artistaRequest.fotoPerfil!));
            await blobClient.UploadAsync(ms, overwrite: true); // 'overwrite: true' permite substituir se já existir um blob com o mesmo nome

            // 5. Obtenha a URL pública do blob
            var blobUrl = blobClient.Uri.ToString(); // Esta é a URL que será salva no DB e usada pelo frontend

            var artista = new Artista(artistaRequest.nome, artistaRequest.bio)
            {
                FotoPerfil = blobUrl // Salva a URL completa do blob no banco de dados
            };

            dal.Adicionar(artista);
            return Results.Ok(artista);
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


        app.MapPut("/Artistas", ([FromServices] DAL<Artista> dal, [FromBody] ArtistaRequestEdit artistaRequestEdit) =>
        {
            var artistaAAtualizar = dal.RecuperarPor(a => a.Id == artistaRequestEdit.Id);
            if (artistaAAtualizar is null)
            {
                return Results.NotFound();
            }
            artistaAAtualizar.Nome = artistaRequestEdit.nome;
            artistaAAtualizar.Bio = artistaRequestEdit.bio;

            dal.Atualizar(artistaAAtualizar);
            return Results.Ok();
        });
    }
}
