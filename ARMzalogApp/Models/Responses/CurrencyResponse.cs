using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARMzalogApp.Models.Responses
{
    public class CurrencyResponse
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Symbol { get; set; }

        public static ObservableCollection<CurrencyResponse> DefaultList => new ObservableCollection<CurrencyResponse>
        {
            new CurrencyResponse { Code = "417", Name = "Кыргызский сом", Symbol = "с" },
            new CurrencyResponse { Code = "840", Name = "Доллар США", Symbol = "$" },
            new CurrencyResponse { Code = "978", Name = "Евро", Symbol = "€" },
            new CurrencyResponse { Code = "643", Name = "Российский рубль", Symbol = "₽" },
            new CurrencyResponse { Code = "398", Name = "Казахстанский тенге", Symbol = "₸" }
        };

    }
}
