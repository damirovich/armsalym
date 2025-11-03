using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARMzalogApp.Models.Responses
{
    public class SubProduct
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Symbol { get; set; }

        public static ObservableCollection<SubProduct> DefaultList => new ObservableCollection<SubProduct>
        {
            new SubProduct { Code = "44", Name = "Нет"},
            new SubProduct { Code = "4", Name = "Комфорт кредит"},
            new SubProduct { Code = "45", Name = "Автокредит"},
            new SubProduct { Code = "4", Name = "Строй кредит" },
            new SubProduct { Code = "5", Name = "За обучение" },
            new SubProduct { Code = "5", Name = "Потребительский" },
            new SubProduct { Code = "5", Name = "Бизнес" },
            new SubProduct { Code = "5", Name = "Ипотека" },
            new SubProduct { Code = "5", Name = "Агрокредит" },
        };
    }
}
