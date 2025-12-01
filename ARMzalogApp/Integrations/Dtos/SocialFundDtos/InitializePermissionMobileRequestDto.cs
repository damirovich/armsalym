namespace ARMzalogApp.Integrations.Dtos.SocialFundDtos;

public sealed class InitializePermissionMobileRequestDto
{
    public string Pin { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Patronymic { get; set; }
}