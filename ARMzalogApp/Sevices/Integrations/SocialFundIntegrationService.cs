
using ARMzalogApp.Integrations.Dtos.SocialFundDtos;
using System.Net.Http.Json;

namespace ARMzalogApp.Sevices.Integrations;
public sealed class SocialFundIntegrationService : ISocialFundIntegrationService
{
    private readonly HttpClient _client;

    public SocialFundIntegrationService(IHttpClientFactory factory)
    {
        _client = factory.CreateClient("PersonalAccountsApi");
    }

    public async Task<string> GetPensionInfoWithSumAsync(string pin,int zvPozn,int typeClient,CancellationToken ct = default)
    {
        var dto = new PensionWithSumRequestDto
        {
            Pin = pin,
            ZvPozn = zvPozn,
            TypeClient = typeClient
        };

        using var response = await _client.PostAsJsonAsync("api/v1/socfond/pension-info-with-sum", dto, ct);

        if (!response.IsSuccessStatusCode)
        {
            var errorText = await response.Content.ReadAsStringAsync(ct);
            throw new InvalidOperationException(
                $"Ошибка Соцфонда ({(int)response.StatusCode}): {errorText}");
        }

        // ожидаем { "message": "..." }
        var obj = await response.Content.ReadFromJsonAsync<SocFondMessageResponse>(cancellationToken: ct);
        return obj?.Message ?? "Данные о пенсии успешно получены";
    }
}