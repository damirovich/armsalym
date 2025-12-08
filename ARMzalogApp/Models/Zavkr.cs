using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARMzalogApp.Models
{
    public class Zavkr : INotifyPropertyChanged
    {
        private bool isSecretHidden;
        public int OtNom { get; set; }
        public int DepartmentId { get; set; }

        public long PositionalNumber { get; set; }
        public string LoanReferenceName { get; set; }

        public string Title { get; set; }
        public string Inn { get; set; }
        public string ZV_DATE { get; set; }
        public string ZV_SUM { get; set; }

        public string PassportSeries { get; set; }
        public string PassportNumber { get; set; }
        public string BirthDate { get; set; }
        public string FullName { get; set; }
        public string KlFam { get; set; }
        public string KlName { get; set; }
        public string KlOtch { get; set; }
        public string Phone { get; set; }

        public bool IsSecretHidden
        {
            get => isSecretHidden;
            set
            {
                if (isSecretHidden != value)
                {
                    isSecretHidden = value;
                    OnPropertyChanged(nameof(IsSecretHidden));
                    OnPropertyChanged(nameof(DisplaySecret));
                }
            }
        }

        public string DisplaySecret => IsSecretHidden ? new string('*', ZV_SUM.Length) : ZV_SUM;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
