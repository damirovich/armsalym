using ARMzalogApp.Constants;
using ARMzalogApp.Integrations.Dtos.KibDtos;
using ARMzalogApp.Models;
using ARMzalogApp.Sevices.Integrations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Net.Http.Json;
using System.Xml;

namespace ARMzalogApp.ViewModels;

public partial class KibCheckPageViewModel : ObservableObject
{
    private readonly Zavkr _zavkr;
    private readonly HttpClient _client;
    private readonly IKibIntegrationService _kibService;

    public KibCheckPageViewModel(Zavkr zavkr, IKibIntegrationService kibService)
    {
        _zavkr = zavkr;
        _kibService = kibService;

        _client = new HttpClient
        {
            BaseAddress = new Uri(ServerConstants.SERVER_ROOT_URL)
        };
        _inn = zavkr.Inn;
        IdNumber = _zavkr.Inn;                // по умолчанию PIN/ИНН из заявки
        ZvPozn = (int)_zavkr.PositionalNumber;
        TypeClient = 1;                       // пока считаем ФЛ

        InfoText = $"ПИН/ИНН: {IdNumber}, заявка № {ZvPozn}";

        IdNumberTypes = new List<string> { "Pin", "IdCard", "Passport" };
        SelectedIdNumberType = "Pin";

        CheckKibCommand = new AsyncRelayCommand(CheckKibAsync);
        DownloadPdfCommand = new AsyncRelayCommand(DownloadPdfAsync);
    }

    // --------- свойства для ввода ---------

    public string InfoText { get => _infoText; set => SetProperty(ref _infoText, value); }
    private string _infoText;

    public string IdNumber { get => _idNumber; set => SetProperty(ref _idNumber, value); }
    private string _idNumber;

    public List<string> IdNumberTypes { get; }
    public string SelectedIdNumberType
    {
        get => _selectedIdNumberType;
        set => SetProperty(ref _selectedIdNumberType, value);
    }
    private string _selectedIdNumberType;

    public string InternalPassport
    {
        get => _internalPassport;
        set => SetProperty(ref _internalPassport, value);
    }
    private string _internalPassport;

    public string DateOfBirth
    {
        get => _dateOfBirth;
        set => SetProperty(ref _dateOfBirth, value);
    }
    private string _dateOfBirth;

    public string FullName
    {
        get => _fullName;
        set => SetProperty(ref _fullName, value);
    }
    private string _fullName;

    public int ZvPozn { get; }
    public int TypeClient { get; }

    // --------- результат ---------

    public string ResultMessage { get => _resultMessage; set => SetProperty(ref _resultMessage, value); }
    private string _resultMessage;

    public string ResultFullName { get => _resultFullName; set { if (SetProperty(ref _resultFullName, value)) OnPropertyChanged(nameof(HasResult)); } }
    private string _resultFullName;

    public string ResultDateOfBirth { get => _resultDateOfBirth; set => SetProperty(ref _resultDateOfBirth, value); }
    private string _resultDateOfBirth;

    public string ResultInternalPassport { get => _resultInternalPassport; set => SetProperty(ref _resultInternalPassport, value); }
    private string _resultInternalPassport;

    public string ResultCreditInfoId { get => _resultCreditInfoId; set => SetProperty(ref _resultCreditInfoId, value); }
    private string _resultCreditInfoId;

    public string ResultStatus { get => _resultStatus; set => SetProperty(ref _resultStatus, value); }
    private string _resultStatus;

    public string ResultSearchRuleApplied { get => _resultSearchRuleApplied; set => SetProperty(ref _resultSearchRuleApplied, value); }
    private string _resultSearchRuleApplied;

    public DateTime? ResultDateOfQuery { get => _resultDateOfQuery; set => SetProperty(ref _resultDateOfQuery, value); }
    private DateTime? _resultDateOfQuery;

    public bool HasResult => !string.IsNullOrWhiteSpace(ResultFullName) || !string.IsNullOrWhiteSpace(ResultCreditInfoId);

    public bool IsBusy { get => _isBusy; set => SetProperty(ref _isBusy, value); }
    private bool _isBusy;

    [ObservableProperty]
    private string _inn;

    [ObservableProperty]
    private string _status;

    public IAsyncRelayCommand CheckKibCommand { get; }
    public IAsyncRelayCommand DownloadPdfCommand { get; }

    // --------- основная логика ---------

    private async Task CheckKibAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        ResultMessage = string.Empty;

        try
        {
            if (string.IsNullOrWhiteSpace(IdNumber))
            {
                ResultMessage = "Введите IdNumber (ПИН/ИНН/номер документа)";
                return;
            }

            // UserId из сессии (otNom)
            var otNomStr = await SessionManager.GetOtNomAsync();
            int.TryParse(otNomStr, out var userId);

            var dto = new KibSearchIndividualRequestDto
            {
                IdNumber = IdNumber.Trim(),
                IdNumberType = SelectedIdNumberType ?? "Pin",
                InternalPassport = InternalPassport ?? string.Empty,
                DateOfBirth = DateOfBirth ?? string.Empty,
                FullName = FullName ?? string.Empty,
                UserId = userId,
                ZvPozn = ZvPozn,
                TypeClient = TypeClient
            };

            // 1. отправляем запрос в КИБ
            var resp = await _client.PostAsJsonAsync("api/v1/kib/search-individual", dto);
            var text = await resp.Content.ReadAsStringAsync();

            if (!resp.IsSuccessStatusCode)
            {
                ResultMessage = $"Ошибка запроса в КИБ: {text}";
                return;
            }

            ResultMessage = "Запрос в КИБ успешно отправлен. Загружаем последние данные...";

            // 2. забираем последнюю запись из нашей БД
            var lastResp = await _client.GetAsync($"api/v1/kib/last-search-individual?idNumber={IdNumber.Trim()}");
            var lastText = await lastResp.Content.ReadAsStringAsync();

            if (!lastResp.IsSuccessStatusCode)
            {
                ResultMessage = $"Не удалось получить сохранённый результат: {lastText}";
                return;
            }

            var info = await lastResp.Content.ReadFromJsonAsync<KibSearchIndividualViewDto>();
            if (info is null)
            {
                ResultMessage = "Пустой результат от сервера.";
                return;
            }

            // заполняем свойства для UI
            ResultFullName = info.FullName;
            ResultDateOfBirth = info.DateOfBirth;
            ResultInternalPassport = info.InternalPassport;
            ResultCreditInfoId = info.CreditInfoId;
            ResultStatus = info.Status;
            ResultSearchRuleApplied = info.SearchRuleApplied;
            ResultDateOfQuery = info.DateOfQuery;

            ResultMessage = "Результат КИБ успешно получен.";
        }
        catch (Exception ex)
        {
            ResultMessage = $"Ошибка: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task DownloadPdfAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(Inn))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "ИНН/ПИН пустой", "OK");
                return;
            }

            // UserId из SecureStorage через SessionManager
            var otNomString = await SessionManager.GetOtNomAsync();
            if (!int.TryParse(otNomString, out var userId))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось определить код сотрудника. Повторите вход.", "OK");
                return;
            }

            var zvPozn = (int)_zavkr.PositionalNumber;
            var typeClient = 1; // физ. лицо

            var filePath = await _kibService.DownloadPdfAsync(Inn.Trim(),zvPozn,userId,typeClient);

            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось получить PDF из КИБ", "OK");
                return;
            }

            await Launcher.OpenAsync(new OpenFileRequest { File = new ReadOnlyFile(filePath), Title = "Отчёт КИБ" });
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Ошибка", $"Ошибка при получении PDF: {ex.Message}", "OK");
        }
    }

}
