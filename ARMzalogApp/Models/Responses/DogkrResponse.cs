using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARMzalogApp.Models.Responses
{
    public class DogkrResponse
    {
        public long DgPozn { get; set; }

        public string DgNom { get; set; }

        public DateTime? LoanIssuedDate { get; set; }

        public decimal? LoanSum { get; set; }

        public string ContractName { get; set; }
    }

}
