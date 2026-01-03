using System.Net.Http.Json;

namespace ApiDocGen.AI.Ollama;

public sealed class OllamaClient
{
    private readonly HttpClient _http;
    private readonly string _baseUrl;

    public OllamaClient(HttpClient http, string baseUrl = "http://localhost:11434")
    {
        _http = http;
        _baseUrl = baseUrl.TrimEnd('/');
        _http.Timeout = TimeSpan.FromMinutes(5);
    }

    public async Task<string> GenerateAsync(string model, string prompt, CancellationToken ct = default)
    {
        var resp = await _http.PostAsJsonAsync($"{_baseUrl}/api/generate", new
        {
            model,
            prompt,
            stream = false
        }, ct);

        resp.EnsureSuccessStatusCode();
        var json = await resp.Content.ReadFromJsonAsync<OllamaGenerateResponse>(cancellationToken: ct);
        return json?.response ?? "";
    }

    private sealed class OllamaGenerateResponse
    {
        public string? response { get; set; }
    }
}
