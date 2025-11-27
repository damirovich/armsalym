using ARMzalogApp.Integrations.Dtos.KibDtos;
using System.Net.Http.Json;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ARMzalogApp.Sevices.Integrations;

public sealed class KibIntegrationService : IKibIntegrationService
{
    private readonly HttpClient _client;

    public KibIntegrationService(IHttpClientFactory factory)
    {
        _client = factory.CreateClient("PersonalAccountsApi");
    }
    public async Task<KibSearchIndividualResult> SearchIndividualAsync(KibSearchIndividualRequestDto request, CancellationToken ct = default)
    {
        using var response = await _client.PostAsJsonAsync("api/v1/kib/search-individual", request, ct);

        if (!response.IsSuccessStatusCode)
        {
            var e = await response.Content.ReadAsStringAsync(ct);
            throw new InvalidOperationException($"Ошибка КИБ: {e}");
        }

        return new KibSearchIndividualResult
        {
            Xml = await response.Content.ReadAsStringAsync(ct)
        };
    }
}