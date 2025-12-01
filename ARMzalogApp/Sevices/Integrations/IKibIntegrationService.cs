using ARMzalogApp.Integrations.Dtos.KibDtos;

namespace ARMzalogApp.Sevices.Integrations;

public interface IKibIntegrationService
{
    Task<KibSearchIndividualResult> SearchIndividualAsync(KibSearchIndividualRequestDto dto, CancellationToken ct = default);

    /// <summary>
    /// Запросить PDF-отчёт по КИБ для клиента.
    /// Бэк сам решает: взять из кеша (2 дня) или сходить в КИБ.
    /// Возвращает локальный путь к PDF файлу (в памяти устройства) либо null при ошибке.
    /// </summary>
    Task<string?> DownloadPdfAsync(string inn,int zvPozn,int userId,int typeClient,CancellationToken ct = default);
}
