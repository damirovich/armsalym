using ARMzalogApp.Constants;
using ARMzalogApp.Integrations.Dtos.KibDtos;
using ARMzalogApp.Models;
using ARMzalogApp.Sevices.Integrations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.IO.Compression;
using System.Net.Http.Json;

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
   
        FullName = _zavkr.FullName;
        DateOfBirth = DateTime.TryParse(_zavkr.BirthDate, out var tempDate) ? tempDate.ToString("yyyy-MM-dd") : _zavkr.BirthDate;
        InternalPassport = $"{_zavkr.PassportSeries}{_zavkr.PassportNumber}";

        IdNumber = _zavkr.Inn; // PIN/ИНН
        ZvPozn = (int)_zavkr.PositionalNumber;
        TypeClient = 1;

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
            if (string.IsNullOrWhiteSpace(IdNumber))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "ИНН/ПИН пустой", "OK");
                return;
            }

            var otNomString = await SessionManager.GetOtNomAsync();
            if (!int.TryParse(otNomString, out var userId))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось определить код сотрудника.", "OK");
                return;
            }

            var zvPozn = (int)_zavkr.PositionalNumber;
            var typeClient = 1;

            IsBusy = true; // Показываем крутилку, если есть индикатор

            // 1. Скачиваем ZIP файл (Service должен сохранить его как .zip во временную папку)
            // Убедись, что _kibService.DownloadPdfAsync сохраняет файл с расширением .zip!
            var zipFilePath = await _kibService.DownloadPdfAsync(IdNumber.Trim(), zvPozn, userId, typeClient);

            if (string.IsNullOrWhiteSpace(zipFilePath) || !File.Exists(zipFilePath))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Файл не был загружен", "OK");
                return;
            }

            // 2. Проверяем расширение. Если это ZIP — распаковываем.
            string fileToOpen = zipFilePath;

            if (Path.GetExtension(zipFilePath).ToLower().EndsWith("zip"))
            {
                fileToOpen = await ExtractPdfFromZipAsync(zipFilePath);

                if (string.IsNullOrEmpty(fileToOpen))
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "В архиве не найден PDF файл", "OK");
                    return;
                }
            }

            // 3. Открываем итоговый файл (PDF)
            await Launcher.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(fileToOpen),
                Title = "Отчёт КИБ"
            });
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Ошибка", $"Ошибка при открытии отчета: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    // Вспомогательный метод для распаковки
    private async Task<string?> ExtractPdfFromZipAsync(string zipPath)
    {
        return await Task.Run(() =>
        {
            try
            {
                // Создаем папку для распаковки (в кэше приложения)
                var extractPath = Path.Combine(FileSystem.CacheDirectory, "kib_extracted_" + Guid.NewGuid().ToString());

                if (Directory.Exists(extractPath))
                    Directory.Delete(extractPath, true);

                Directory.CreateDirectory(extractPath);

                // Распаковываем
                ZipFile.ExtractToDirectory(zipPath, extractPath);

                // Ищем PDF файл внутри (берем первый попавшийся)
                var pdfFile = Directory.GetFiles(extractPath, "*.pdf", SearchOption.AllDirectories).FirstOrDefault();

                // Если PDF нет, ищем HTML (иногда отчеты бывают в HTML)
                if (string.IsNullOrEmpty(pdfFile))
                {
                    pdfFile = Directory.GetFiles(extractPath, "*.html", SearchOption.AllDirectories).FirstOrDefault();
                }

                return pdfFile;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка распаковки: {ex.Message}");
                return null;
            }
        });
    }

}
