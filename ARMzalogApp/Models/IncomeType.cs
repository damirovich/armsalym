using System.Collections.ObjectModel;

namespace ARMzalogApp.Models;

public class IncomeType
{
    public int Id { get; set; }
    public string Name { get; set; }

    public static ObservableCollection<IncomeType> DefaultList => new()
    {
        new IncomeType { Id = 1, Name = "Основной доход" },
        new IncomeType { Id = 2, Name = "Дополнительный доход" }
    };
}
