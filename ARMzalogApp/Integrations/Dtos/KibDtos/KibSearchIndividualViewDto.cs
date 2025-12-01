namespace ARMzalogApp.Integrations.Dtos.KibDtos;

public sealed class KibSearchIndividualViewDto
{
    public string IdNumber { get; set; } = string.Empty;
    public string IdNumberType { get; set; } = string.Empty;
    public string InternalPassport { get; set; } = string.Empty;
    public string DateOfBirth { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;

    public string CreditInfoId { get; set; } = string.Empty;
    public string SearchRuleApplied { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;

    public DateTime DateOfQuery { get; set; }
}