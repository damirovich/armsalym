
namespace ARMzalogApp.Integrations.Dtos.GrsDtos;
public sealed class GrsIkInfoDto
{
    public string Pin { get; set; } = string.Empty;
    public string Fio { get; set; } = string.Empty;
    public string DateOfBirth { get; set; } = string.Empty;
    public string FullAddress { get; set; } = string.Empty;

    public string PassportSeries { get; set; } = string.Empty;
    public string PassportNumber { get; set; } = string.Empty;

    // статус паспорта – из VoidStatusValueText
    public string PassportStatus { get; set; } = string.Empty;

    public DateTime DateQuery { get; set; }
}