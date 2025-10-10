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
        KlientList = new ObservableCollection<Klient>();

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
    public ObservableCollection<Klient> KlientList { get; set; }

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

    private LoanTypeResponse _selectedLoanType;
    public LoanTypeResponse SelectedLoanType
    {
        get => _selectedLoanType;
        set
        {
            _selectedLoanType = value;
            OnPropertyChanged();
        }
    }

    private Spr _selectedSubProduct;
    public Spr SelectedSubProduct
    {
        get => _selectedSubProduct;
        set
        {
            _selectedSubProduct = value;
            OnPropertyChanged();
        }
    }
    private Spr _selectedPurpose;
    public Spr SelectedPurpose
    {
        get => _selectedPurpose;
        set
        {
            _selectedPurpose = value;
            OnPropertyChanged();
        }
    }

    private Klient _selectedKlient;
    public Klient SelectedKlient
    {
        get => _selectedKlient;
        set
        {
            _selectedKlient = value;
            OnPropertyChanged();
        }
    }

    private string _typedFIO;
    private async void OnSaveNumberClicked(object sender, EventArgs e)
    {
        _typedFIO = clientEntry.Text;
        using var httpClient = new HttpClient();
        string url = ServerConstants.SERVER_ROOT_URL + "api/LoanReference/GetClientByFullName?fullName=" + _typedFIO;
        HttpResponseMessage response = await httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            string responseData = await response.Content.ReadAsStringAsync();
            //var dataList = JsonConvert.DeserializeObject<Klient>(responseData);
            //_selectedKlient = dataList;
            //KlientList.Add(dataList);
            SelectedKlient = JsonConvert.DeserializeObject<Klient>(responseData);
            OnPropertyChanged(nameof(SelectedKlient));
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

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        //string selectedTeam = zalPicker.SelectedItem?.ToString();

        //if (!string.IsNullOrEmpty(selectedTeam) && zalValues.ContainsKey(selectedTeam))
        //{
        //    loadingIndicator.IsRunning = true;
        //    loadingIndicator.IsVisible = true;
        //    string _otNom = await SecureStorage.Default.GetAsync("otNom");

        //    loadingIndicator.IsRunning = false;
        //    loadingIndicator.IsVisible = false;
        //    if (result == "OK")
        //    {
        //        await DisplayAlert("Успех", "Новый залог создан", "OK");
        //        await Navigation.PushAsync(new ZavkrPage(_selectedZavkr));
        //    }
        //}
        //else
        //{
        //    DisplayAlert("Ошибка", "Пожалуйста, выберите команду", "OK");
        //}
    }



    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


}