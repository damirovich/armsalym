using ARMzalogApp.Integrations.Dtos.KibDtos;

namespace ARMzalogApp.Sevices.Integrations;

public interface IKibIntegrationService
{
    Task<KibSearchIndividualResult> SearchIndividualAsync(KibSearchIndividualRequestDto dto, CancellationToken ct = default);

}
