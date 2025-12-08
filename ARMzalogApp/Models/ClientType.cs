using System.Collections.ObjectModel;

namespace ARMzalogApp.Models;
 public class ClientType
{
    public int Id { get; set; }
    public string Name { get; set; }

    public static ObservableCollection<ClientType> DefaultList => new()
        {
            new ClientType { Id = 1, Name = "Сотрудник" },
            new ClientType { Id = 2, Name = "Пенсионер" },
            new ClientType { Id = 3, Name = "Бюджетник" }
        };
}
