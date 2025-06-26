using ScreenSound.Shared.Modelos.Modelos;

namespace ScreenSound.Modelos;

public class Musica
{
    public Musica () { }
    public Musica(string nome, int? anoLancamento, int artistaId)
    {
        Nome = nome;
        AnoLancamento = anoLancamento;
        ArtistaId = artistaId;
        Generos = new List<Genero>();
    }

    public Musica(string nome, int? anoLancamento, int artistaId, ICollection<Genero> generos)
    {
        Nome = nome;
        AnoLancamento = anoLancamento;
        ArtistaId = artistaId;
        Generos = generos; // Atribui a coleção de gêneros passada
    }

    public string Nome { get; set; }
    public int Id { get; set; }
    public int? AnoLancamento { get; set; }
    public virtual Artista? Artista { get; set; }
    public int ArtistaId { get; set; }

    public virtual ICollection<Genero> Generos { get; set; } = new List<Genero>();

    public void ExibirFichaTecnica()
    {
        Console.WriteLine($"Nome: {Nome}");
      
    }

    public override string ToString()
    {
        return @$"Id: {Id}
        Nome: {Nome}";
    }
}