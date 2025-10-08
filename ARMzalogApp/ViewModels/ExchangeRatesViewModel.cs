using ARMzalogApp.Models;
using ARMzalogApp.Sevices;
using System.Collections.ObjectModel;


namespace ARMzalogApp.ViewModels
{
    internal class ExchangeRatesViewModel : BaseViewModel
    {
        public ExchangeRatesViewModel()
        {
            IsBusy = false;
            CurrentExchangeRates = new ObservableCollection<ExchangeRates>();
            Today = DateTime.Today.ToString("d");


            Task.Run(async () =>
            {
                //_accessToken = await SecureStorage.Default.GetAsync("UserAccessToken");
                await InitializeExchangeRates();
            });
        }

        string _accessToken;


        public ObservableCollection<ExchangeRates> CurrentExchangeRates { get; set; }


        private string _today;
        public string Today
        {
            get => _today;
            set => SetProperty(ref _today, value);
        }



        private async Task InitializeExchangeRates()
        {
            try
            {
                var response = await ContentService.GetItemsAsync<ExchangeRates>("api/ExchangeRates/GetExchangeRates");

                if (response.Count() > 0)
                {
                    foreach (var item in response)
                    {
                        CurrentExchangeRates.Add(item);
                    }
                }
            }
            catch
            {

            }
        }
    }
}
