using System.ComponentModel.DataAnnotations;

namespace ScreenSoundWeb.Requests;

public record ArtistaRequest([Required] string nome, [Required] string bio, string? fotoPerfil);
