using ARMzalogApp.Constants;
using ARMzalogApp.Models;
using ARMzalogApp.Models.Responses;
using ARMzalogApp.Sevices;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
        // Здесь будет твоя логика:
        // - проверить все поля
        // - вызвать SavingService.SaveNewLoan()
        // - вызвать POST /applications/{id}/salary-block

        var otNom = await SecureStorage.Default.GetAsync("otNom");

        var service = new SavingService();
        var result = await service.SaveNewLoan(otNom, SelectedKlient);

        await App.Current.MainPage.DisplayAlert("Ответ", result, "OK");
    }

    private async Task GoBackAsync()
    {
        bool result = await App.Current.MainPage.DisplayAlert("Подтверждение", "Вы уверены, что хотите вернуться? Несохранённые данные будут потеряны.", "Да", "Нет");

        if (result)
            await Shell.Current.GoToAsync("..");
    }

    #endregion
}