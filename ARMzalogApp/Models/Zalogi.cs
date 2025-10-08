using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARMzalogApp.Models
{
    public class Zalogi
    {
        public int? ZalogId { get; set; }
        public long? DgPozn { get; set; }
        public int? TipZalog { get; set; }
        public string? Fio { get; set; }
        public string? zvInn { get; set; }
    }
}
