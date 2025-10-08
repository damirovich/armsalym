using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARMzalogApp.Models
{
    public class SavingRequest
    {
        public string clientInn { get; set; }
        public string ZvPozn { get; set; }

        public string IdZalog { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        public byte[]? PhotoData { get; set; }
        public string Type { get; set; }
        public int otNom { get; set; }
        public string token { get; set; }

    }
}
