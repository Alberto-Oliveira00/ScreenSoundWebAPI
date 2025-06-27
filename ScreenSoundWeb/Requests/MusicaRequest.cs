using System.ComponentModel.DataAnnotations;

namespace ScreenSoundWeb.Requests;
    public record MusicaRequest([Required]string nome, int? anoLancamento, [Required] int ArtistaId, ICollection<GeneroRequest> Generos=null);
