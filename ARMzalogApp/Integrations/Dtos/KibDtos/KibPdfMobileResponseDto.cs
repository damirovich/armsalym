namespace ARMzalogApp.Integrations.Dtos.KibDtos;


public sealed class KibPdfMobileResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;

    public bool FromCache { get; set; }
    public string? FileName { get; set; }
    public string? FilePath { get; set; }
    public string? Base64 { get; set; }
}