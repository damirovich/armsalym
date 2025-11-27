

namespace ARMzalogApp.Integrations.Dtos.KibDtos;


/// <summary>
/// Запрос на поиск физического лица в КИБ (мобильная версия).
/// </summary>
public sealed class KibSearchIndividualRequestDto
{
    public string IdNumber { get; set; } = string.Empty;     // ПИН
    public string IdNumberType { get; set; } = "Pin";        // фиксируем Pin
    public string InternalPassport { get; set; } = string.Empty;
    public string DateOfBirth { get; set; } = string.Empty;  // опционально
    public string FullName { get; set; } = string.Empty;

    public int UserId { get; set; }      // пока 0, потом подставим реального пользователя
    public int ZvPozn { get; set; }      // номер заявки
    public int TypeClient { get; set; }  // 1 – физлицо
}