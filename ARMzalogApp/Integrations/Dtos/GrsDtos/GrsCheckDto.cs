namespace ARMzalogApp.Integrations.Dtos.GrsDtos;

public sealed class GrsCheckDto
{
    public string Pin { get; set; } = string.Empty;
    public string PassportSeries { get; set; } = string.Empty;
    public string PassportNumber { get; set; } = string.Empty;

    public int ZvPozn { get; set; }        // pozn
    public byte TypeClient { get; set; }   // typeClient
    public int UserId { get; set; }        // OT_NOM (пользователь)
}