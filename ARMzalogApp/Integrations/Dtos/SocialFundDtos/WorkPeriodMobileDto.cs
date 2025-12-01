namespace ARMzalogApp.Integrations.Dtos.SocialFundDtos;

public sealed class WorkPeriodMobileDto
{
    public DateTime DateBegin { get; set; }
    public DateTime DateEnd { get; set; }

    public string Inn { get; set; } = string.Empty;
    public string NumSf { get; set; } = string.Empty;
    public string Payer { get; set; } = string.Empty;
    public string PinLss { get; set; } = string.Empty;

    public decimal Salary { get; set; }

    // Удобные свойства для UI
    public string PeriodText => $"{DateBegin:dd.MM.yyyy} - {DateEnd:dd.MM.yyyy}";
    public string EmployerText => $"Работодатель: {Payer} (ИНН: {Inn}, №СФ: {NumSf})";
    public string SalaryText => $"Зарплата: {Salary:N2}";
}
