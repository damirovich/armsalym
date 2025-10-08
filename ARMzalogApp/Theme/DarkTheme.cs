using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARMzalogApp.Theme
{
    public class DarkTheme : ResourceDictionary
    {
        public DarkTheme()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this["PrimaryTextColor"] = Color.FromArgb("#ffffff"); // white
            this["BackgroundColor"] = Color.FromArgb("#805500");
            this["GreyTextColor"] = Color.FromArgb("#808080");
            this["SecondTextColor"] = Color.FromArgb("#000000"); // black
        }
    }

}
