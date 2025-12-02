using ARMzalogApp.Constants;
using ARMzalogApp.Models;
using ARMzalogApp.Models.Responses;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ARMzalogApp.Sevices;

namespace ARMzalogApp.Views.ZavkrPages;

public partial class CreateLoanZarp : ContentPage
{
    public CreateLoanZarp()
    {
        InitializeComponent();
        SelectedKlient = new ZmainView();
        _ = InitializeDataAsync();
        BindingContext = this;

    }


    private async Task InitializeDataAsync()
    {
        IsLoading = true;

        try
        {
            CurrencyList = new ObservableCollection<CurrencyResponse>();
            LoanTypeList = new ObservableCollection<LoanTypeResponse>();
            KlientList = new ObservableCollection<ZmainView>();

            SubProductList = SubProduct.DefaultList;
            PurposesList = new ObservableCollection<Spr>();
            FamilyStatusList = FamilyStatus.DefaultList;
            CurrencyList = CurrencyResponse.DefaultList;

            //await GetDataCurrencies();
            await GetDataLoanType();
            //await GetDataSubProduct();
            await GetDataPurpose();

        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", $"Ошибка загрузки: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    public ObservableCollection<LoanTypeResponse> LoanTypeList { get; set; }
    public ObservableCollection<ZmainView> KlientList { get; set; }

    private ObservableCollection<SubProduct> _subProductList;
    public ObservableCollection<SubProduct> SubProductList
    {
        get => _subProductList;
        set
        {
            _subProductList = value;
            OnPropertyChanged();
        }
    }
    public ObservableCollection<Spr> PurposesList { get; set; }
    private ObservableCollection<FamilyStatus> _familyStatusList;
    public ObservableCollection<FamilyStatus> FamilyStatusList
    {
        get => _familyStatusList;
        set
        {
            _familyStatusList = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<CurrencyResponse> _currencyList;
    public ObservableCollection<CurrencyResponse> CurrencyList
    {
        get => _currencyList;
        set
        {
            _currencyList = value;
            OnPropertyChanged();
        }
    }

    private bool _isLoading = false;
    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged();
        }
    }

    private CurrencyResponse _selectedCurrency;
    public CurrencyResponse SelectedCurrency
    {
        get => _selectedCurrency;
        set
        {
            _selectedCurrency = value;
            OnPropertyChanged(nameof(CurrencyList));  
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

    private SubProduct _selectedSubProduct;
    public SubProduct SelectedSubProduct
    {
        get => _selectedSubProduct;
        set
        {
            _selectedSubProduct = value;
            OnPropertyChanged(nameof(SubProductList));
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

    private ZmainView _selectedKlient;
    public ZmainView SelectedKlient
    {
        get => _selectedKlient;
        set
        {
            _selectedKlient = value;
            OnPropertyChanged();
        }
    }
    private FamilyStatus _selectedFamilyStatus;
    public FamilyStatus SelectedFamilyStatus
    {
        get => _selectedFamilyStatus;
        set
        {
            _selectedFamilyStatus = value;
            OnPropertyChanged();
            //if (value != null && SelectedKlient != null)
            //{
            //    SelectedKlient.BvFamSt = (int)value.Code;
            //}
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
            var klientFind = JsonConvert.DeserializeObject<Klient>(responseData);
            if (klientFind != null)
            {
                SelectedKlient = new ZmainView
                {
                    ZvNdok = klientFind.KlNdok,
                    ZvSrdok = klientFind.KlSrdok,
                    ZvDok = klientFind.KlDok != null ? klientFind.KlDok.Trim() : null,
                    ZvMvd = klientFind.KlMvd,
                    ZvDatevp = klientFind.KlDatevp,
                    ZvDokend = klientFind.KlDokend,
                };
                OnPropertyChanged(nameof(SelectedKlient));
            }
            else
            {
                await DisplayAlert("Информация", "Клиент не найден", "OK");
            }
        }
    }

    private async Task GetDataCurrencies()
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

    private async Task GetDataLoanType()
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

    //private async Task GetDataSubProduct()
    //{
    //    using var httpClient = new HttpClient();
    //    string url = ServerConstants.SERVER_ROOT_URL + "api/LoanReference/GetLoanSubProducts";
    //    HttpResponseMessage response = await httpClient.GetAsync(url);

    //    if (response.IsSuccessStatusCode)
    //    {
    //        string responseData = await response.Content.ReadAsStringAsync();
    //        List<Spr> dataList = JsonConvert.DeserializeObject<List<Spr>>(responseData);
    //        foreach (var item in dataList)
    //        {
    //            SubProductList.Add(item);
    //        }
    //    }
    //}

    private async Task GetDataPurpose()
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
        if (SelectedKlient == null)
        {
            await DisplayAlert("Ошибка", "Пожалуйста, выберите клиента", "OK");
            return;
        }
        if (SelectedKlient.ZvNom == null)
        {
            await DisplayAlert("Ошибка", "Пожалуйста, выведите № заявления", "OK");
            return;
        }
        if (SelectedKlient.ZvSum == null)
        {
            await DisplayAlert("Ошибка", "Пожалуйста, введите сумму", "OK");
            return;
        }

        if (SelectedCurrency != null)
            SelectedKlient.ZvKodv = SelectedCurrency.Code;
        else
        {
            await DisplayAlert("Ошибка", "Пожалуйста, выберите валюту", "OK");
            return;
        }

        if (SelectedKlient.ZvSrok == null)
        {
            await DisplayAlert("Ошибка", "Пожалуйста, выведите срок в месяцах", "OK");
            return;
        }

        if (SelectedLoanType != null)
            SelectedKlient.ZvKom = SelectedLoanType.Code;
        else
        {
            await DisplayAlert("Ошибка", "Пожалуйста, выберите вид займа", "OK");
            return;
        }

        if (SelectedSubProduct != null)
            SelectedKlient.BvSubProd = Convert.ToInt32(SelectedSubProduct.Code);
        else
        {
            await DisplayAlert("Ошибка", "Пожалуйста, выберите суб продукт", "OK");
            return;
        }
        if (SelectedPurpose != null)
            SelectedKlient.CelKr = SelectedPurpose.SKod;
        else
        {
            await DisplayAlert("Ошибка", "Пожалуйста, выберите цель", "OK");
            return;
        }

        if (clientEntry == null)
        {
            await DisplayAlert("Ошибка", "Пожалуйста, введите клиента", "OK");
            return;
        }

        if (SelectedKlient.ZvDok == null)
        {
            await DisplayAlert("Ошибка", "Пожалуйста, выведите документ", "OK");
            return;
        }

        if (SelectedKlient.ZvSrdok == null)
        {
            await DisplayAlert("Ошибка", "Пожалуйста, выведите серию", "OK");
            return;
        }

        if (SelectedKlient.ZvNdok == null)
        {
            await DisplayAlert("Ошибка", "Пожалуйста, выведите № документа", "OK");
            return;
        }

        if (SelectedKlient.ZvDatevp == null)
        {
            await DisplayAlert("Ошибка", "Пожалуйста, выведите дату выдачи", "OK");
            return;
        }

        if (SelectedKlient.ZvMvd == null)
        {
            await DisplayAlert("Ошибка", "Пожалуйста, выведите орган выдачи", "OK");
            return;
        }

        if (SelectedKlient.ZvDokend == null)
        {
            await DisplayAlert("Ошибка", "Пожалуйста, выведите 'годен до'", "OK");
            return;
        }

        if (SelectedKlient.BvRabStaj == null)
        {
            await DisplayAlert("Ошибка", "Пожалуйста, выведите общий стаж работы", "OK");
            return;
        }

        if (SelectedKlient.ZvInn == null)
        {
            await DisplayAlert("Ошибка", "Пожалуйста, выведите ИНН", "OK");
            return;
        }

        if (SelectedKlient.Doljnoct == null)
        {
            await DisplayAlert("Ошибка", "Пожалуйста, выведите место работы и должность", "OK");
            return;
        }

        if (SelectedKlient.FaktAdres == null)
        {
            await DisplayAlert("Ошибка", "Пожалуйста, выведите место фактич. проживания", "OK");
            return;
        }

        if (SelectedFamilyStatus != null)
            SelectedKlient.BvFamSt = SelectedFamilyStatus.Code.ToString();
        else
        {
            await DisplayAlert("Ошибка", "Пожалуйста, выберите семейное пол-е", "OK");
            return;
        }

        if (SelectedKlient.FioCupr == null)
        {
            await DisplayAlert("Ошибка", "Пожалуйста, введите ФИО супруга(и)", "OK");
            return;
        }

        if (SelectedKlient.RabCupr == null)
        {
            await DisplayAlert("Ошибка", "Пожалуйста, введите место работы супруга(и)", "OK");
            return;
        }

        if (SelectedKlient.DoljCup == null)
        {
            await DisplayAlert("Ошибка", "Пожалуйста, введите должность супруга(и)", "OK");
            return;
        }

        if (SelectedKlient.BvTelefSupr == null)
        {
            await DisplayAlert("Ошибка", "Пожалуйста, введите телефон супруга(и)", "OK");
            return;
        }

        string _otNom = await SecureStorage.Default.GetAsync("otNom");
        var service = new SavingService();
        string result = await service.SaveNewLoan(_otNom, SelectedKlient);
        //if (result == "OK")
        //{
        await DisplayAlert("Ответ", result, "OK");
        await Shell.Current.GoToAsync("HomePage");
        //}

    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        bool result = await DisplayAlert("Подтверждение", "Вы уверены, что хотите вернуться? Несохранённые данные будут потеряны.", "Да", "Нет");
        if (result)
        {
            await Navigation.PopAsync();
        }
    }


    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


}