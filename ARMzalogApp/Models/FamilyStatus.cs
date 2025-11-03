using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARMzalogApp.Models
{
    public enum FamilyStatusType
    {
        MarriedMale = 1,
        SingleMale = 2,
        MarriedFemale = 3,
        SingleFemale = 4
    }

    public class FamilyStatus
    {
        public FamilyStatusType Code { get; set; }
        public string Name { get; set; }

        public static ObservableCollection<FamilyStatus> DefaultList => new ObservableCollection<FamilyStatus>
    {
        new FamilyStatus { Code = FamilyStatusType.MarriedMale, Name = "женат" },
        new FamilyStatus { Code = FamilyStatusType.SingleMale, Name = "не женат" },
        new FamilyStatus { Code = FamilyStatusType.MarriedFemale, Name = "замужем" },
        new FamilyStatus { Code = FamilyStatusType.SingleFemale, Name = "не замужем" }
    };

        public static string GetNameByCode(FamilyStatusType code) // Метод для получения имени по коду
        {
            return DefaultList.FirstOrDefault(x => x.Code == code)?.Name;
        }
    }
}
