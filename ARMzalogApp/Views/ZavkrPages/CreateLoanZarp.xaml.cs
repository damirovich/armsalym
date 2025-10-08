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
        LoanTypeList = new ObservableCollection<LoanTypeResponse>();
        SubProductList = new ObservableCollection<Spr>();
        PurposesList = new ObservableCollection<Spr>();

        GetDataCurrencies();
        GetDataLoanType();
        GetDataSubProduct();
        GetDataPurpose();
        BindingContext = this;
    }

    public ObservableCollection<CurrencyResponse> CurrencyList { get; set; }
    public ObservableCollection<LoanTypeResponse> LoanTypeList { get; set; }
    public ObservableCollection<Spr> SubProductList { get; set; }
    public ObservableCollection<Spr> PurposesList { get; set; }

    private CurrencyResponse _selectedCurrency;
    public CurrencyResponse SelectedCurrency
    {
        get => _selectedCurrency;
        set
        {
            _selectedCurrency = value;
            OnPropertyChanged(); // если INotifyPropertyChanged
        }
    }

    private CurrencyResponse _selectedLoanType;
    public CurrencyResponse SelectedLoanType
    {
        get => _selectedLoanType;
        set
        {
            _selectedLoanType = value;
            OnPropertyChanged();
        }
    }

    private CurrencyResponse _selectedSubProduct;
    public CurrencyResponse SelectedSubProduct
    {
        get => _selectedSubProduct;
        set
        {
            _selectedSubProduct = value;
            OnPropertyChanged();
        }
    }
    private CurrencyResponse _selectedPurpose;
    public CurrencyResponse SelectedPurpose
    {
        get => _selectedPurpose;
        set
        {
            _selectedPurpose = value;
            OnPropertyChanged();
        }
    }


    private async void OnOpiuPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("OpiuPage");
    }

    private async void GetDataCurrencies()
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

    private async void GetDataLoanType()
    {
        using var httpClient = new HttpClient();
        string url = ServerConstants.SERVER_ROOT_URL + "api/LoanReference/GetLoanTypes";
        HttpResponseMessage response = await httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            string responseData = await response.Content.ReadAsStringAsync();
            List<LoanTypeResponse> dataList = JsonConvert.DeserializeObject<List<LoanTypeResponse>>(responseData);
            foreach (var item in dataList)
            {
                LoanTypeList.Add(item);
            }

        }
    }

    private async void GetDataSubProduct()
    {
        using var httpClient = new HttpClient();
        string url = ServerConstants.SERVER_ROOT_URL + "api/LoanReference/GetLoanSubProducts";
        HttpResponseMessage response = await httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            string responseData = await response.Content.ReadAsStringAsync();
            List<Spr> dataList = JsonConvert.DeserializeObject<List<Spr>>(responseData);
            foreach (var item in dataList)
            {
                SubProductList.Add(item);
            }
        }
    }

    private async void GetDataPurpose()
    {
        using var httpClient = new HttpClient();
        string url = ServerConstants.SERVER_ROOT_URL + "api/LoanReference/GetLoanPurposes";
        HttpResponseMessage response = await httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            string responseData = await response.Content.ReadAsStringAsync();
            List<Spr> dataList = JsonConvert.DeserializeObject<List<Spr>>(responseData);
            foreach (var item in dataList)
            {
                PurposesList.Add(item);
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


}