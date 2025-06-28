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

    public async Task DeleteArtistaAsync(int id)
    {
        await _httpClient.DeleteAsync($"artistas/{id}");
    }

    public async Task EditarArtistaAsync(ArtistaRequestEdit artista)
    {
        await _httpClient.PutAsJsonAsync("artistas", artista);
    }

    public async Task<ArtistaResponse?> GetArtistaPorNomeAsync(string nome)
    {
        return await _httpClient.GetFromJsonAsync<ArtistaResponse>($"artistas/{nome}");
    }
}
