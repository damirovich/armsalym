namespace ARMzalogApp.Integrations.Dtos.SocialFundDtos;

 public sealed class SendPermissionCodeMobileRequestDto
{
    public string Code { get; set; } = string.Empty;
    public string RequestId { get; set; } = string.Empty;
    public string Pin { get; set; } = string.Empty;
}
