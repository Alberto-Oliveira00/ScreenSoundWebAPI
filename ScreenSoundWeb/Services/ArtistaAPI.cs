using Microsoft.IdentityModel.Clients.ActiveDirectory;
using ScreenSoundWeb.Requests;
using ScreenSoundWeb.Response;
using System.Net.Http; 
using System.Net.Http.Json;

namespace ScreenSoundWeb.Services;

public class ArtistaAPI
{
    private readonly HttpClient _httpClient;

    public ArtistaAPI(System.Net.Http.IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("API");
    }

    public async Task<ICollection<ArtistaResponse>?> GetArtistasAsync()
    {
        return await _httpClient.GetFromJsonAsync<ICollection<ArtistaResponse>>("artistas");
    }

    public async Task AddArtistaAsync(ArtistaRequest artista)
    {
        await _httpClient.PostAsJsonAsync("artistas", artista);
    }
}
