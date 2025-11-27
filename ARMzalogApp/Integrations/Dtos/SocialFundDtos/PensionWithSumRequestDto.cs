namespace ARMzalogApp.Integrations.Dtos.SocialFundDtos;

public sealed class PensionWithSumRequestDto
{
    public string Pin { get; set; } = string.Empty;
    public int ZvPozn { get; set; }
    public int TypeClient { get; set; }
}