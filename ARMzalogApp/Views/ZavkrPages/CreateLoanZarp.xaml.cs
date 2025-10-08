using ARMzalogApp.Constants;
using ARMzalogApp.Models.Responses;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
namespace ARMzalogApp.Views.ZavkrPages;

public partial class CreateLoanZarp : ContentPage
{
	public CreateLoanZarp()
	{
		InitializeComponent();
        CurrencyList = new ObservableCollection<CurrencyResponse>();

        GetDataFromServer();
        BindingContext = this;
    }

    public ObservableCollection<CurrencyResponse> CurrencyList { get; set; }

    private CurrencyResponse _selectedCurrency;
    public CurrencyResponse SelectedCurrency
    {
        get => _selectedCurrency;
        set
        {
            _selectedCurrency = value;
            OnPropertyChanged(); // если у вас INotifyPropertyChanged
        }
    }

    private async void OnOpiuPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("OpiuPage");
    }

    private async void GetDataFromServer()
    {
        
        using var httpClient = new HttpClient();

        string url = ServerConstants.SERVER_ROOT_URL + "api/LoanReference/GetCurrencies";

        HttpResponseMessage response = await httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            string responseData = await response.Content.ReadAsStringAsync();

            List<CurrencyResponse> dataList = JsonConvert.DeserializeObject<List<CurrencyResponse>>(responseData);
            foreach (var item in dataList)
            {
                CurrencyList.Add(item);
            }

        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


}