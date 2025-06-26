namespace ScreenSound.API.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScreenSound.API.Requests;
using ScreenSound.API.Response;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using ScreenSound.Shared.Modelos.Modelos;
using System;
using System.Data.SqlTypes;
using System.Text.Json;
using System.Text.Json.Serialization;

public static class MusicasExtensions
{
    private static ICollection<MusicaResponse> EntityListToResponseList(IEnumerable<Musica> musicaList)
    {
        return musicaList.Select(a => EntityToResponse(a)).ToList();
    }

    private static MusicaResponse EntityToResponse(Musica musica)
    {
        return new MusicaResponse(musica.Id, musica.Nome!, musica.Artista!.Id, musica.Artista.Nome);
    }

    public static void AddEndPointMusicas(this WebApplication app, JsonSerializerOptions jsonSerializerOptions)
	{
        app.MapGet("/Musicas", ([FromServices] DAL<Musica> dal) =>
        {
            return Results.Json(dal.Listar(), jsonSerializerOptions);
        });

        app.MapGet("/Musicas/{nome}", ([FromServices] DAL<Musica> dal, string nome) =>
        {
            var musica = dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));

            if (musica is null)
            {
                return Results.NotFound();
            }
            return Results.Json(musica, jsonSerializerOptions);
        });

        app.MapPost("/Musicas", ([FromServices] DAL<Musica> dalMusica, // Injete DAL de Musica
                                [FromServices] DAL<Artista> dalArtista, // Injete DAL de Artista
                                [FromServices] DAL<Genero> dalGenero,   // Injete DAL de Genero
                                [FromBody] MusicaRequest musicaRequest) =>
        {
            Artista? artistaParaMusica = null;
            if (musicaRequest.ArtistaId > 0)
            {
                artistaParaMusica = dalArtista.RecuperarPor(a => a.Id == musicaRequest.ArtistaId);
                if (artistaParaMusica == null)
                {
                    return Results.BadRequest("Artista com o ID fornecido não encontrado.");
                }
            }
            else
            {
                return Results.BadRequest("ID do Artista é obrigatório.");
            }

            // 2. Lógica para Gêneros: Converte a lista de GeneroRequest para entidades Genero
            ICollection<Genero> generosDaMusica = ConverterGenerosParaEntidades(musicaRequest.Generos, dalGenero);

            // 3. Cria o objeto Musica usando o NOVO CONSTRUTOR com Gêneros
            var musica = new Musica(
                musicaRequest.nome,
                musicaRequest.anoLancamento,
                musicaRequest.ArtistaId, // FK do artista
                generosDaMusica          // Coleção de entidades Genero
            );


            musica.Artista = artistaParaMusica;
            dalMusica.Adicionar(musica);

            return Results.Ok("Música adicionada com sucesso!");
        });

        app.MapDelete("/Musicas/{id}", ([FromServices] DAL<Musica> dal, int id) => {
            var musica = dal.RecuperarPor(a => a.Id == id);
            if (musica is null)
            {
                return Results.NotFound();
            }
            dal.Deletar(musica);
            return Results.NoContent();

        });

        app.MapPut("/Musicas", ([FromServices] DAL<Musica> dal, [FromBody] MusicaRequestEdit musicaRequestEdit) => {
            var musicaAAtualizar = dal.RecuperarPor(a => a.Id == musicaRequestEdit.Id);
            if (musicaAAtualizar is null)
            {
                return Results.NotFound();
            }
            musicaAAtualizar.Nome = musicaRequestEdit.nome;
            musicaAAtualizar.AnoLancamento = musicaRequestEdit.anoLancamento;

            dal.Atualizar(musicaAAtualizar);
            return Results.Ok();
        });
    }

    private static Genero RequestToEntity(GeneroRequest generoRequest, DAL<Genero> dalGenero)
    {
        // Tenta buscar o gênero existente pelo Nome
        // (Você pode adicionar Descricao à busca se a combinação for única)
        var generoExistente = dalGenero.RecuperarPor(g => g.Nome.Equals(generoRequest.Nome, StringComparison.OrdinalIgnoreCase));

        if (generoExistente == null)
        {
            // Se não existe, cria um novo
            var novoGenero = new Genero()
            {
                Nome = generoRequest.Nome,
                Descricao = generoRequest.Descricao // Certifique-se que Genero.Descricao existe
            };
            dalGenero.Adicionar(novoGenero); // Adiciona o novo gênero ao banco
            return novoGenero;
        }
        else
        {
            return generoExistente;
        }
    }

    private static ICollection<Genero> ConverterGenerosParaEntidades(
        ICollection<GeneroRequest>? generosRequest,
        DAL<Genero> dalGenero)
    {
        var generosDaMusica = new List<Genero>();
        if (generosRequest != null && generosRequest.Any())
        {
            foreach (var generoReq in generosRequest)
            {
                generosDaMusica.Add(RequestToEntity(generoReq, dalGenero));
            }
        }
        return generosDaMusica;
    }

}
