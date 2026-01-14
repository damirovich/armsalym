using ARMzalogApp.Constants;
using ARMzalogApp.Models;
using ARMzalogApp.Models.Requests;
using ARMzalogApp.Models.Responses;
using ARMzalogApp.Views.Popups;
using CommunityToolkit.Maui.Views;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace ARMzalogApp.ViewModels;


[QueryProperty(nameof(BvTypRez), "typRez")]
[QueryProperty(nameof(BvTypRez1), "typRez1")]
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

        OpenCitizenshipSearchCommand = new Command(async () => await OpenCountrySearchAsync());
        SearchCountryCommand = new Command<string>((text) => FilterCountries(text));

        SelectedKlient = new ZmainView();

        // Запуск загрузки данных при создании
        LoadDataCommand.Execute(null);
    }

    #region Bindable Properties

    private ZmainView _selectedKlient;
    public ZmainView SelectedKlient
    {
        get => _selectedKlient;
        set { _selectedKlient = value; OnPropertyChanged(); }
    }

    private string _klientFullName;
    public string KlientFullName
    {
        get => _klientFullName;
        set { _klientFullName = value; OnPropertyChanged(); }
    }

    // --- Справочники ---
    public ObservableCollection<LoanTypeResponse> LoanTypeList { get; set; } = new();

    // Эти списки у тебя были статичными, оставляем как есть
    public ObservableCollection<CurrencyResponse> CurrencyList { get; set; } = CurrencyResponse.DefaultList;
    public ObservableCollection<SubProduct> SubProductList { get; set; } = SubProduct.DefaultList;
    public ObservableCollection<FamilyStatus> FamilyStatusList { get; set; } = FamilyStatus.DefaultList;
    public ObservableCollection<ClientType> ClientTypeList { get; set; } = ClientType.DefaultList;
    public ObservableCollection<IncomeType> IncomeTypeList { get; set; } = IncomeType.DefaultList;

    // Загружаемые списки
    public ObservableCollection<Spr> PurposesList { get; set; } = new();
    public ObservableCollection<Spr> NationalityList { get; set; } = new();
    public ObservableCollection<CountryCode> CitizenshipList { get; set; } = new();

    // НОВЫЕ СПИСКИ
    public ObservableCollection<Spr> KredurList { get; set; } = new(); // Уровень КК (s_tip = 27)
    public ObservableCollection<Spr> RegionsList { get; set; } = new(); // Подразделения
    public ObservableCollection<Spr> VidClientList { get; set; } = new();

    private ObservableCollection<CountryCode> _filteredCitizenshipList;
    public ObservableCollection<CountryCode> FilteredCitizenshipList { get => _filteredCitizenshipList; set { _filteredCitizenshipList = value; OnPropertyChanged(); } }

    // --- Выбранные значения ---
    private LoanTypeResponse _selectedLoanType;
    public LoanTypeResponse SelectedLoanType { get => _selectedLoanType; set { _selectedLoanType = value; OnPropertyChanged(); } }

    private CurrencyResponse _selectedCurrency;
    public CurrencyResponse SelectedCurrency { get => _selectedCurrency; set { _selectedCurrency = value; OnPropertyChanged(); } }

    private SubProduct _selectedSubProduct;
    public SubProduct SelectedSubProduct { get => _selectedSubProduct; set { _selectedSubProduct = value; OnPropertyChanged(); } }

    private Spr _selectedPurpose;
    public Spr SelectedPurpose { get => _selectedPurpose; set { _selectedPurpose = value; OnPropertyChanged(); } }

    private FamilyStatus _selectedFamilyStatus;
    public FamilyStatus SelectedFamilyStatus { get => _selectedFamilyStatus; set { _selectedFamilyStatus = value; OnPropertyChanged(); } }

    private ClientType _selectedClientType;
    public ClientType SelectedClientType { get => _selectedClientType; set { _selectedClientType = value; OnPropertyChanged(); } }

    private IncomeType _selectedIncomeType;
    public IncomeType SelectedIncomeType { get => _selectedIncomeType; set { _selectedIncomeType = value; OnPropertyChanged(); } }

    private Spr _selectedNationality;
    public Spr SelectedNationality { get => _selectedNationality; set { _selectedNationality = value; OnPropertyChanged(); } }


    private CountryCode _selectedCitizenship;
    public CountryCode SelectedCitizenship { get => _selectedCitizenship; set { _selectedCitizenship = value; OnPropertyChanged(); } }

    private Spr _selectedVidClient;
    public Spr SelectedVidClient { get => _selectedVidClient; set { _selectedVidClient = value; OnPropertyChanged(); } }

    // НОВЫЕ ВЫБРАННЫЕ ПОЛЯ
    private Spr _selectedKredur;
    public Spr SelectedKredur { get => _selectedKredur; set { _selectedKredur = value; OnPropertyChanged(); } }

    private Spr _selectedRegion; // Это Refund/Подразделение
    public Spr SelectedRegion { get => _selectedRegion; set { _selectedRegion = value; OnPropertyChanged(); } }

    // --- Поля ввода (Финансы и прочее) ---
    private decimal _salaryAmount;
    public decimal SalaryAmount { get => _salaryAmount; set { _salaryAmount = value; OnPropertyChanged(); } }

    private decimal _monthlyExpenses;
    public decimal MonthlyExpenses { get => _monthlyExpenses; set { _monthlyExpenses = value; OnPropertyChanged(); } }

    private decimal _sfLoansService;
    public decimal SfLoansService { get => _sfLoansService; set { _sfLoansService = value; OnPropertyChanged(); } }

    private string _incomeDescription;
    public string IncomeDescription { get => _incomeDescription; set { _incomeDescription = value; OnPropertyChanged(); } }

    // --- Новые поля (Родственники и Адреса) ---
    private string _dbTipbis;
    public string DbTipbis { get => _dbTipbis; set { _dbTipbis = value; OnPropertyChanged(); } }

    private string _dbPeriod;
    public string DbPeriod { get => _dbPeriod; set { _dbPeriod = value; OnPropertyChanged(); } }

    private string _dbGeogr;
    public string DbGeogr { get => _dbGeogr; set { _dbGeogr = value; OnPropertyChanged(); } }

    private string _dbPoctavka;
    public string DbPoctavka { get => _dbPoctavka; set { _dbPoctavka = value; OnPropertyChanged(); } }

    private bool _isRelativeGuarantor;
    public bool IsRelativeGuarantor { get => _isRelativeGuarantor; set { _isRelativeGuarantor = value; OnPropertyChanged(); } }

    private string _idProch;
    public string IdProch { get => _idProch; set { _idProch = value; OnPropertyChanged(); } }

    private string _darek;
    public string Darek { get => _darek; set { _darek = value; OnPropertyChanged(); } }

    private int _childrenCount;
    public int ChildrenCount { get => _childrenCount; set { _childrenCount = value; OnPropertyChanged(); } }

    private string _selectedGender;
    public string SelectedGender { get => _selectedGender; set { _selectedGender = value; OnPropertyChanged(); } }

    private bool _isLoading;
    public bool IsLoading { get => _isLoading; set { _isLoading = value; OnPropertyChanged(); } }

    private int _bvTypRez;
    public int BvTypRez { get => _bvTypRez; set { _bvTypRez = value; OnPropertyChanged(); } }

    private int _bvTypRez1;
    public int BvTypRez1 { get => _bvTypRez1; set { _bvTypRez1 = value; OnPropertyChanged(); } }

    #endregion

    #region Commands
    public ICommand LoadDataCommand { get; }
    public ICommand SearchClientCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand BackCommand { get; }
    public ICommand OpenCitizenshipSearchCommand { get; }
    public ICommand SearchCountryCommand { get; }
    #endregion

    #region Methods

    private async Task InitializeDataAsync()
    {
        IsLoading = true;
        try
        {
            var tasks = new List<Task> {
                GetDataLoanType(),
                GetDataPurpose(),
                GetNationalities(),
                GetCitizenship(),
                GetKredur(),        // Новое
                GetRegions(),       // Новое (Подразделения)
                GetVidClient(), //Вид Клиента
            };

            await Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Ошибка", "Не удалось загрузить справочники: " + ex.Message, "OK");
        }
        finally { IsLoading = false; }
    }

    // === ВОССТАНОВЛЕННЫЕ МЕТОДЫ ЗАГРУЗКИ ===

    private async Task GetDataLoanType()
    {
        using var httpClient = new HttpClient();
        var url = $"{ServerConstants.SERVER_ROOT_URL}api/LoanReference/GetLoanTypes";
        var response = await httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<LoanTypeResponse>>(json);
            LoanTypeList.Clear();
            foreach (var item in list) LoanTypeList.Add(item);
        }
    }

    private async Task GetDataPurpose()
    {
        using var httpClient = new HttpClient();
        var url = $"{ServerConstants.SERVER_ROOT_URL}api/LoanReference/GetLoanPurposes";
        var response = await httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<Spr>>(json);
            PurposesList.Clear();
            foreach (var item in list) PurposesList.Add(item);
        }
    }

    private async Task GetNationalities()
    {
        using var httpClient = new HttpClient();
        var url = $"{ServerConstants.SERVER_ROOT_URL}api/Reference/GetSpr?tip=36";
        var response = await httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<Spr>>(json);
            NationalityList.Clear();
            foreach (var item in list) NationalityList.Add(item);
        }
    }

    private async Task GetCitizenship()
    {
        using var httpClient = new HttpClient();
        var url = $"{ServerConstants.SERVER_ROOT_URL}api/LoanReference/GetCountries";
        var response = await httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<CountryCode>>(json);
            CitizenshipList.Clear();
            foreach (var item in list) CitizenshipList.Add(item);
        }
    }

    // === НОВЫЕ МЕТОДЫ ===

    private async Task GetKredur()
    {
        using var httpClient = new HttpClient();
        // tip=27 - это уровни кредитного комитета
        var url = $"{ServerConstants.SERVER_ROOT_URL}api/LoanReference/GetLoanCreditCommitteeAsync";
        var response = await httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<Spr>>(json);
            KredurList.Clear();
            foreach (var item in list) KredurList.Add(item);
        }
    }
    private void FilterCountries(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            FilteredCitizenshipList = new ObservableCollection<CountryCode>(CitizenshipList);
        }
        else
        {
            var filtered = CitizenshipList
                .Where(x => x.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                .ToList();
            FilteredCitizenshipList = new ObservableCollection<CountryCode>(filtered);
        }
    }
    private async Task OpenCountrySearchAsync()
    {
        // 1. Создаем экземпляр всплывающего окна и передаем в него полный список стран
        // CitizenshipList — это ваш существующий список из ViewModel
        var popup = new CountrySearchPopup(CitizenshipList);

        // 2. Отображаем окно поверх текущей страницы и ждем результат
        // Метод ShowPopupAsync вернет то, что выберет пользователь
        var result = await Shell.Current.CurrentPage.ShowPopupAsync(popup);

        // 3. Проверяем, что пользователь действительно выбрал страну, а не просто закрыл окно
        if (result is CountryCode selectedCountry)
        {
            // Присваиваем выбранное значение основному свойству
            SelectedCitizenship = selectedCountry;

            // Уведомляем систему, что данные изменились (если не используется [ObservableProperty])
            OnPropertyChanged(nameof(SelectedCitizenship));
        }
    }
    private async Task GetRegions()
    {
        using var httpClient = new HttpClient();
        // ВНИМАНИЕ: Проверь этот URL. Это эндпоинт для подразделений (Refud)
        var url = $"{ServerConstants.SERVER_ROOT_URL}api/Reference/GetPodrazdel";

        // Если эндпоинта GetPodrazdel нет, возможно это справочник? Но в cshtml это db.podrazdel
        var response = await httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            // Десериализуем в Spr или спец модель, если структура другая
            // В cshtml там поля: dp_kod, dp_nam. Убедись что Spr или другой класс подходит
            // Здесь я использую Spr для универсальности, но возможно нужно создать PodrazdelResponse
            try
            {
                var list = JsonConvert.DeserializeObject<List<Spr>>(json);
                RegionsList.Clear();
                foreach (var item in list) RegionsList.Add(item);
            }
            catch
            {
                // Если структура не Spr, а другая, то здесь будет ошибка.
                // Тогда создай класс: class Podrazdel { public int Dp_Kod {get;set;} public string Dp_Nam {get;set;} }
            }
        }
    }
    private async Task GetVidClient()
    {
        using var httpClient = new HttpClient();
        // Обычно вид клиента - это tip=11 (физ/юр/ип) или tip=32. 
        // Если есть спец. эндпоинт как у КК, используй его.
        var url = $"{ServerConstants.SERVER_ROOT_URL}api/LoanReference/GetVidClient";
        var response = await httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<Spr>>(json);
            VidClientList.Clear();
            foreach (var item in list) VidClientList.Add(item);
        }
    }
    public async Task SearchClientAsync()
    {
        if (string.IsNullOrWhiteSpace(KlientFullName)) return;
        IsLoading = true;
        try
        {
            using var httpClient = new HttpClient();
            var url = $"{ServerConstants.SERVER_ROOT_URL}api/LoanReference/GetClientByFullName?fullName={KlientFullName}";
            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                await App.Current.MainPage.DisplayAlert("Инфо", "Клиент не найден", "OK");
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            var klientDb = JsonConvert.DeserializeObject<Klient>(json);

            if (klientDb == null) return;

            // 1. Заполняем поля
            SelectedKlient.ZvInn = klientDb.KlInn;
            SelectedKlient.ZvDok = klientDb.KlDok?.Trim();
            SelectedKlient.ZvSrdok = klientDb.KlSrdok;
            SelectedKlient.ZvNdok = klientDb.KlNdok;
            SelectedKlient.ZvMvd = klientDb.KlMvd;
            SelectedKlient.ZvDatevp = klientDb.KlDatevp;
            SelectedKlient.RegAdres = klientDb.KlAdr;
            SelectedKlient.FaktAdres = klientDb.KlAdr;
            SelectedKlient.ZvTel = klientDb.KlTel1;
            SelectedKlient.ZvFmr = klientDb.Fmr;
            SelectedKlient.Doljnoct = klientDb.KlDolgn;
            SelectedKlient.ZvDokend = klientDb.KlDokend;
            SelectedKlient.ZvFdr = klientDb.Fdr;
            SelectedKlient.Atyjoni = klientDb.KlNam;

            IdProch = klientDb.KlAdr;
            Darek = klientDb.KlAdr;

            // Пол
            if (klientDb.KlGr == 1) SelectedGender = "Мужской";
            else if (klientDb.KlGr == 2) SelectedGender = "Женский";

            // Гражданство
            if (!string.IsNullOrEmpty(klientDb.Grajd))
                SelectedCitizenship = CitizenshipList.FirstOrDefault(x => x.Name.Equals(klientDb.Grajd, StringComparison.OrdinalIgnoreCase));

            // Национальность
            if (klientDb.KlNational.HasValue)
                SelectedNationality = NationalityList.FirstOrDefault(x => x.SKod == klientDb.KlNational);

            // Семейное положение
            if (klientDb.KlSempolog.HasValue)
                SelectedFamilyStatus = FamilyStatusList.FirstOrDefault(x => x.Code == (FamilyStatusType)klientDb.KlSempolog);

            OnPropertyChanged(nameof(SelectedKlient));
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Ошибка", ex.Message, "OK");
        }
        finally { IsLoading = false; }
    }

    private async Task SaveAsync()
    {
        IsLoading = true;
        try
        {
            var otNom = await SecureStorage.Default.GetAsync("otNom");

            DateTime? parsedDokEnd = null;
            if (DateTime.TryParse(SelectedKlient.ZvDokend, out var d2)) parsedDokEnd = d2;

            var dto = new SaveZarpZayavkaDto
            {
                ZvPozn = SelectedKlient.ZvPozn,
                OtNom = otNom,

                // Кредит
                ZvSum = SelectedKlient.ZvSum,
                ZvSrok = SelectedKlient.ZvSrok,
                CelKr = SelectedPurpose?.Id,
                ZvVidkr = (byte?)SelectedLoanType?.Id,
                ZvKom = SelectedLoanType?.Id.ToString(),
                ZvKredur = (byte?)SelectedKredur?.SKod,
                ZvKodv = SelectedCurrency?.Code ?? "KGS",
                CelIsp = SelectedSubProduct?.Name,

                // Паспорт
                ZvDok = SelectedKlient.ZvDok,
                ZvSrdok = SelectedKlient.ZvSrdok,
                ZvNdok = SelectedKlient.ZvNdok,
                ZvDatevp = SelectedKlient.ZvDatevp,
                ZvDokend = parsedDokEnd,
                ZvMvd = SelectedKlient.ZvMvd,
                ZvGrajd = SelectedCitizenship?.Code,
                ZvNational = SelectedNationality?.SKod,

                // Клиент
                KlFam = SelectedKlient.IFam,
                KlName = SelectedKlient.KlName,
                KlOtch = SelectedKlient.KlOtch,
                ZvNamkl = KlientFullName,
                ZvInn = SelectedKlient.ZvInn,
                ZvFdr = SelectedKlient.ZvFdr,
                ZvFmr = SelectedKlient.ZvFmr,
                ZvGr = SelectedGender == "Мужской" ? 1 : 2,
                ZvKl = SelectedClientType?.Id ?? 1,
                Atyjoni = SelectedKlient.Atyjoni,
                Z1Vid = SelectedVidClient.SNam,

                // Адреса
                RegAdres = SelectedKlient.RegAdres,
                FaktAdres = SelectedKlient.FaktAdres,
                ZvAdr = SelectedKlient.RegAdres,
                IdProch = IdProch,
                Darek = Darek,
                ZvTel = SelectedKlient.ZvTel,
                ZvTelfax = SelectedKlient.ZvTelfax,

                // Работа
                Doljnoct = SelectedKlient.Doljnoct,
                BvRabStaj = SelectedKlient.BvRabStaj,

                // Семья
                FamStat = SelectedFamilyStatus?.Code,
                FioCupr = SelectedKlient.FioCupr,
                RabCupr = SelectedKlient.RabCupr,
                DoljCup = SelectedKlient.DoljCup,
                BvTelefSupr = SelectedKlient.BvTelefSupr,
                DetKol = ChildrenCount,

                // Родственники
                DbTipbis = DbTipbis,
                DbPeriod = DbPeriod,
                DbGeogr = DbGeogr,
                DbPoctavka = DbPoctavka,
                DbPoruchRods1 = IsRelativeGuarantor ? (short)1 : (short)2,

                // Финансы
                SalaryAmount = SalaryAmount,
                MonthlyExpenses = MonthlyExpenses,
                SfLoansService = SfLoansService,
                IncomeType = SelectedIncomeType?.Id.ToString(),
                IncomeDescription = IncomeDescription,

                // Прочее
                Refund = SelectedRegion?.SKod ?? 0,

                BvTypRez = this.BvTypRez,
                BvTypRez1 = this.BvTypRez1,
            };

            using var http = new HttpClient();
            var url = $"{ServerConstants.SERVER_ROOT_URL}api/zavkr/save";
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await http.PostAsync(url, content);
            var responseJson = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<dynamic>(responseJson);

            if (response.IsSuccessStatusCode && result != null && (bool)result.success)
            {
                // Если успех — берем текст из поля message, пришедшего с сервера
                await App.Current.MainPage.DisplayAlert("Успешно", (string)result.message, "ОК");

                // Возвращаемся назад к списку
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                // Если сервер вернул ошибку в поле message
                string errorText = result?.message ?? "Ошибка при сохранении";
                await App.Current.MainPage.DisplayAlert("Внимание", errorText, "ОК");
            }
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Ошибка сети", ex.Message, "ОК");
        }
        finally { IsLoading = false; }
    }

    private async Task GoBackAsync()
    {
        bool result = await App.Current.MainPage.DisplayAlert("Подтверждение", "Вы уверены, что хотите вернуться? Несохранённые данные будут потеряны.", "Да", "Нет");
        if (result) await Shell.Current.GoToAsync("..");
    }
    #endregion
}