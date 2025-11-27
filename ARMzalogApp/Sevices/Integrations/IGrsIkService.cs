using ARMzalogApp.Integrations.Dtos.GrsDtos;

namespace ARMzalogApp.Sevices.Integrations;

public interface IGrsIkService
{
    Task<GrsIkResponseDto?> SendIkAsync(GrsCheckDto dto, CancellationToken ct = default);
}