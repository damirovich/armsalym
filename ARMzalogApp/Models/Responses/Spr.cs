using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARMzalogApp.Models.Responses
{
    public partial class Spr
    {
        public int Id { get; set; }

        public byte STip { get; set; }

        public int SKod { get; set; }

        public string? SNam { get; set; }

        public string? SKodnew { get; set; }

        public string? SNamd { get; set; }

        public int? Aktiv { get; set; }

        public string? SNamKr { get; set; }

        public string? SNamd1 { get; set; }

        public int? Status { get; set; }

        public string? Crif { get; set; }
    }
}
