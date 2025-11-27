using ARMzalogApp.Integrations.Dtos.SocialFundDtos;


namespace ARMzalogApp.Sevices.Integrations;
public interface ISocialFundIntegrationService
{
    Task<string> GetPensionInfoWithSumAsync(string pin, int zvPozn, int typeClient, CancellationToken ct = default);
}
