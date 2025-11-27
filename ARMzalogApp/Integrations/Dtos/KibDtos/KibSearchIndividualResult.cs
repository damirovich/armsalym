namespace ARMzalogApp.Integrations.Dtos.KibDtos;


/// <summary>
/// Результат запроса в КИБ, как минимум содержит сырое XML.
/// </summary>
public sealed class KibSearchIndividualResult
{
    public string Xml { get; set; } = string.Empty;
}
