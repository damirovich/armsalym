using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARMzalogApp.Theme
{
    public class LightTheme : ResourceDictionary
    {
        public LightTheme()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this["PrimaryTextColor"] = Color.FromArgb("#000000"); // black
            this["BackgroundColor"] = Color.FromArgb("#F5F5DC");
            this["GreyTextColor"] = Color.FromArgb("#99ff99");
            this["SecondTextColor"] = Color.FromArgb("#ffffff"); // white
        }
    }

}
