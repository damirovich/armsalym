
using ARMzalogApp.Integrations.Dtos.GrsDtos;
using System.Net.Http.Json;

namespace ARMzalogApp.Sevices.Integrations;
public sealed class GrsIkService : IGrsIkService
{
    private readonly HttpClient _client;

    public GrsIkService(IHttpClientFactory factory)
    {
        _client = factory.CreateClient("PersonalAccountsApi");
    }

    public async Task<GrsIkResponseDto?> SendIkAsync(GrsCheckDto dto, CancellationToken ct = default)
    {
        using var response = await _client.PostAsJsonAsync("api/ik", dto, ct);

        // 4xx/5xx — кинем понятное исключение
        if (!response.IsSuccessStatusCode)
        {
            var text = await response.Content.ReadAsStringAsync(ct);
            throw new InvalidOperationException(
                $"Ошибка ГРС ({(int)response.StatusCode}): {text}");
        }

        // Парсим анонимный объект { message, lastQuery }
        var json = await response.Content.ReadFromJsonAsync<GrsIkResponseDto>(cancellationToken: ct);
        return json;
    }
}