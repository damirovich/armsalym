using ARMzalogApp.Constants;
using ARMzalogApp.Models;
using ARMzalogApp.Models.Requests;
using ARMzalogApp.Models.Responses;
using ARMzalogApp.Sevices;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace ARMzalogApp.ViewModels;

public class CreateLoanZarpViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


    public CreateLoanZarpViewModel()
    {
        LoadDataCommand = new Command(async () => await InitializeDataAsync());
        SearchClientCommand = new Command(async () => await SearchClientAsync());
        SaveCommand = new Command(async () => await SaveAsync());
        BackCommand = new Command(async () => await GoBackAsync());

        SelectedKlient = new ZmainView();
        LoadDataCommand.Execute(null);
    }


    #region Bindable Properties

    public ZmainView SelectedKlient { get; set; }

    private string _klientFullName;
    public string KlientFullName
    {
        get => _klientFullName;
        set { _klientFullName = value; OnPropertyChanged(); }
    }

    public ObservableCollection<LoanTypeResponse> LoanTypeList { get; set; } = new();
    public ObservableCollection<CurrencyResponse> CurrencyList { get; set; } = new();
    public ObservableCollection<SubProduct> SubProductList { get; set; } = SubProduct.DefaultList;
    public ObservableCollection<Spr> PurposesList { get; set; } = new();
    public ObservableCollection<FamilyStatus> FamilyStatusList { get; set; } = FamilyStatus.DefaultList;

    public ObservableCollection<ClientType> ClientTypeList { get; set; } = ClientType.DefaultList;
    public ObservableCollection<IncomeType> IncomeTypeList { get; set; } = IncomeType.DefaultList;


    private LoanTypeResponse _selectedLoanType;
    public LoanTypeResponse SelectedLoanType
    {
        get => _selectedLoanType;
        set { _selectedLoanType = value; OnPropertyChanged(); }
    }

    private CurrencyResponse _selectedCurrency;
    public CurrencyResponse SelectedCurrency
    {
        get => _selectedCurrency;
        set { _selectedCurrency = value; OnPropertyChanged(); }
    }

    private SubProduct _selectedSubProduct;
    public SubProduct SelectedSubProduct
    {
        get => _selectedSubProduct;
        set { _selectedSubProduct = value; OnPropertyChanged(); }
    }

    private Spr _selectedPurpose;
    public Spr SelectedPurpose
    {
        get => _selectedPurpose;
        set { _selectedPurpose = value; OnPropertyChanged(); }
    }

    private FamilyStatus _selectedFamilyStatus;
    public FamilyStatus SelectedFamilyStatus
    {
        get => _selectedFamilyStatus;
        set { _selectedFamilyStatus = value; OnPropertyChanged(); }
    }

    private ClientType _selectedClientType;
    public ClientType SelectedClientType
    {
        get => _selectedClientType;
        set { _selectedClientType = value; OnPropertyChanged(); }
    }

    private IncomeType _selectedIncomeType;
    public IncomeType SelectedIncomeType
    {
        get => _selectedIncomeType;
        set { _selectedIncomeType = value; OnPropertyChanged(); }
    }
    private decimal _salaryAmount;
    public decimal SalaryAmount
    {
        get => _salaryAmount;
        set { _salaryAmount = value; OnPropertyChanged(); }
    }

    private decimal _monthlyExpenses;
    public decimal MonthlyExpenses
    {
        get => _monthlyExpenses;
        set { _monthlyExpenses = value; OnPropertyChanged(); }
    }

    private decimal _sfLoansService;
    public decimal SfLoansService
    {
        get => _sfLoansService;
        set { _sfLoansService = value; OnPropertyChanged(); }
    }

    private string _incomeDescription;
    public string IncomeDescription
    {
        get => _incomeDescription;
        set { _incomeDescription = value; OnPropertyChanged(); }
    }
    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set { _isLoading = value; OnPropertyChanged(); }
    }

    #endregion


    #region Commands

    public ICommand LoadDataCommand { get; }
    public ICommand SearchClientCommand { get; }
    public ICommand SaveCommand { get; }

    public ICommand BackCommand { get; }

    #endregion



    #region Methods

    private async Task InitializeDataAsync()
    {
        IsLoading = true;

        try
        {
            await GetDataLoanType();
            await GetDataPurpose();
        }
        finally
        {
            IsLoading = false;
        }
    }


    public async Task SearchClientAsync()
    {
        if (string.IsNullOrWhiteSpace(KlientFullName))
            return;

        using var httpClient = new HttpClient();
        var url = $"{ServerConstants.SERVER_ROOT_URL}api/LoanReference/GetClientByFullName?fullName={KlientFullName}";
        var response = await httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return;

        var json = await response.Content.ReadAsStringAsync();
        var klient = JsonConvert.DeserializeObject<Klient>(json);

        if (klient == null) return;

        SelectedKlient = new ZmainView
        {
            ZvNdok = klient.KlNdok,
            ZvSrdok = klient.KlSrdok,
            ZvDok = klient.KlDok?.Trim(),
            ZvMvd = klient.KlMvd,
            ZvDatevp = klient.KlDatevp,
            ZvDokend = klient.KlDokend
        };

        OnPropertyChanged(nameof(SelectedKlient));
    }


    private async Task GetDataLoanType()
    {
        using var httpClient = new HttpClient();
        var url = $"{ServerConstants.SERVER_ROOT_URL}api/LoanReference/GetLoanTypes";

        var response = await httpClient.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();

        var list = JsonConvert.DeserializeObject<List<LoanTypeResponse>>(json);

        LoanTypeList.Clear();
        foreach (var item in list)
            LoanTypeList.Add(item);
    }


    private async Task GetDataPurpose()
    {
        using var httpClient = new HttpClient();
        var url = $"{ServerConstants.SERVER_ROOT_URL}api/LoanReference/GetLoanPurposes";

        var response = await httpClient.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();

        var list = JsonConvert.DeserializeObject<List<Spr>>(json);

        PurposesList.Clear();
        foreach (var item in list)
            PurposesList.Add(item);
    }


    private async Task SaveAsync()
    {
        var otNom = await SecureStorage.Default.GetAsync("otNom");
       
        var dto = new SaveZarpZayavkaDto
        {
            ZvPozn = SelectedKlient.ZvPozn,
            OtNom = otNom,

            // Паспорт
            ZvDok = SelectedKlient.ZvDok,
            ZvSrdok = SelectedKlient.ZvSrdok,
            ZvNdok = SelectedKlient.ZvNdok,
            ZvDatevp = SelectedKlient.ZvDatevp,
            ZvDokend = SelectedKlient.ZvDokend,
            ZvMvd = SelectedKlient.ZvMvd,

            // Клиент
            KlFam = SelectedKlient.IFam,
            KlName = SelectedKlient.KlName,
            KlOtch = SelectedKlient.KlOtch,
            ZvInn = SelectedKlient.ZvInn,
            FaktAdres = SelectedKlient.FaktAdres,
            Doljnoct = SelectedKlient.Doljnoct,
            BvRabStaj = SelectedKlient.BvRabStaj,

            // Семья
            FamStat = SelectedFamilyStatus?.Code,
            FioCupr = SelectedKlient.FioCupr,
            RabCupr = SelectedKlient.RabCupr,
            DoljCup = SelectedKlient.DoljCup,
            BvTelefSupr = SelectedKlient.BvTelefSupr,

            // Финансы
            SalaryAmount = SalaryAmount,
            MonthlyExpenses = MonthlyExpenses,
            SfLoansService = SfLoansService,
            ClientType = SelectedClientType?.Id,
            IncomeType = SelectedIncomeType?.Id,
            IncomeDescription = IncomeDescription,

            // Кредит
            ZvSum = SelectedKlient.ZvSum,
            ZvSrok = SelectedKlient.ZvSrok,
            CelKr = SelectedPurpose?.Id,
            ZvVidkr = (byte?)SelectedLoanType?.Id
        };

        using var http = new HttpClient();
        var url = $"{ServerConstants.SERVER_ROOT_URL}api/zavkr/save";

        var json = JsonConvert.SerializeObject(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await http.PostAsync(url, content);
        var responseJson = await response.Content.ReadAsStringAsync();

        await App.Current.MainPage.DisplayAlert("Ответ", responseJson, "OK");

    }

    private async Task GoBackAsync()
    {
        bool result = await App.Current.MainPage.DisplayAlert("Подтверждение", "Вы уверены, что хотите вернуться? Несохранённые данные будут потеряны.", "Да", "Нет");

        if (result)
            await Shell.Current.GoToAsync("..");
    }

    #endregion
}