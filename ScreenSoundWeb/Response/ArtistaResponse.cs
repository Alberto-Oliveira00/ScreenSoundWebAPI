﻿namespace ScreenSoundWeb.Response;

public record ArtistaResponse(int Id, string Nome, string Bio, string? FotoPerfil)
{
    public override string ToString()
    {
        return $"{this.Nome}";
    }
}



