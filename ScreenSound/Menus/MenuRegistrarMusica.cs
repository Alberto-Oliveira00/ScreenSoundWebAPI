﻿using ScreenSound.Banco;
using ScreenSound.Modelos;

namespace ScreenSound.Menus;

internal class MenuRegistrarMusica : Menu
{
    public override void Executar(DAL<Artista> artistaDAL)
    {
        base.Executar(artistaDAL);
        ExibirTituloDaOpcao("Registro de músicas");
        Console.Write("Digite o artista cuja música deseja registrar: ");
        string nomeDoArtista = Console.ReadLine()!;
        var artistaRecuperado = artistaDAL.RecuperarPor(a => a.Nome.Equals(nomeDoArtista, StringComparison.OrdinalIgnoreCase));
        if (artistaRecuperado is not null)
        {
            Console.Write("Agora digite o título da música: ");
            string tituloDaMusica = Console.ReadLine()!;

            Console.Write("Agora digite o ano de lançamento da música: ");
            string anoLancamentoInput = Console.ReadLine()!;
            int? anoLancamento = null;
            if (!string.IsNullOrWhiteSpace(anoLancamentoInput) && int.TryParse(anoLancamentoInput, out int parsedAno))
            {
                anoLancamento = parsedAno;
            }

            var novaMusica = new Musica(tituloDaMusica, anoLancamento, artistaRecuperado.Id);

            DAL<Musica> musicaDAL = new DAL<Musica>(new ScreenSoundContext());

            musicaDAL.Adicionar(novaMusica);
            Console.WriteLine($"A música {tituloDaMusica} de {nomeDoArtista} foi registrada com sucesso!");
            artistaDAL.Atualizar(artistaRecuperado);
            Thread.Sleep(4000);
            Console.Clear();
        }
        else
        {
            Console.WriteLine($"\nO artista {nomeDoArtista} não foi encontrado!");
            Console.WriteLine("Digite uma tecla para voltar ao menu principal");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
