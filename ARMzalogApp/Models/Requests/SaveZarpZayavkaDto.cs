namespace ARMzalogApp.Models.Requests;

public class SaveZarpZayavkaDto
{
    public int? ZvPozn { get; set; }
    public string? OtNom { get; set; }

    // Паспорт
    public string? ZvDok { get; set; }
    public string? ZvSrdok { get; set; }
    public string? ZvNdok { get; set; }
    public DateTime? ZvDatevp { get; set; }
    public DateTime? ZvDokend { get; set; }
    public string? ZvMvd { get; set; }

    // Клиент
    public string? KlFam { get; set; }
    public string? KlName { get; set; }
    public string? KlOtch { get; set; }
    public string? ZvInn { get; set; }
    public string? FaktAdres { get; set; }
    public string? Doljnoct { get; set; }
    public string? BvRabStaj { get; set; }

    // Семья
    public FamilyStatusType? FamStat { get; set; }
    public string? FioCupr { get; set; }
    public string? RabCupr { get; set; }
    public string? DoljCup { get; set; }
    public string? BvTelefSupr { get; set; }

    // Финансы
    public decimal? SalaryAmount { get; set; }
    public decimal? MonthlyExpenses { get; set; }
    public decimal? SfLoansService { get; set; }
    public int? ClientType { get; set; }
    public string? IncomeType { get; set; }
    public string? IncomeDescription { get; set; }

    // Кредит
    public decimal? ZvSum { get; set; }
    public short? ZvSrok { get; set; }
    public int? CelKr { get; set; }
    public byte? ZvVidkr { get; set; }
}