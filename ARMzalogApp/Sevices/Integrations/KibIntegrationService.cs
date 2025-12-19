using ARMzalogApp.Integrations.Dtos.KibDtos;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;

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

    /// <summary>
    /// Запрашивает у бэка PDF по КИБ с учётом 2-дневного кеша.
    /// Возвращает путь к локальному PDF-файлу (в памяти устройства) либо null при ошибке.
    /// </summary>
    public async Task<string?> DownloadPdfAsync(string inn, int zvPozn, int userId, int typeClient, CancellationToken ct = default)
    {
        var requestBody = new { inn, zvPozn, userId, typeClient };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("api/v1/kib/pdf-report-mobile", content, ct);

        if (!response.IsSuccessStatusCode)
        {
            var errorText = await response.Content.ReadAsStringAsync(ct);
            Console.WriteLine($"KIB PDF error: {response.StatusCode} {errorText}");
            return null;
        }

        var responseJson = await response.Content.ReadAsStringAsync(ct);

        var dto = JsonSerializer.Deserialize<KibPdfMobileResponseDto>(
            responseJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (dto == null || !dto.Success || string.IsNullOrWhiteSpace(dto.Base64))
            return null;

        byte[] pdfBytes;
        try
        {
            pdfBytes = Convert.FromBase64String(dto.Base64);
        }
        catch
        {
            return null;
        }

        // имя файла – либо из ответа сервера, либо генерируем
        var fileName = string.IsNullOrWhiteSpace(dto.FileName)
            ? $"KIB_{inn}_{DateTime.Now:yyyyMMddHHmmss}.zip"
            : dto.FileName;

        var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

        Directory.CreateDirectory(FileSystem.AppDataDirectory);
        await System.IO.File.WriteAllBytesAsync(filePath, pdfBytes, ct);

        return filePath;
    }
}