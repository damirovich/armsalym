using ARMzalogApp.Constants;
using ARMzalogApp.Integrations.Dtos.GrsDtos;
using ARMzalogApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net.Http.Json;

namespace ARMzalogApp.ViewModels;

public class GrsCheckPageViewModel : ObservableObject
{
    private readonly Zavkr _zavkr;
    private readonly HttpClient _client;

    public GrsCheckPageViewModel(Zavkr zavkr)
    {
        _zavkr = zavkr;

        _client = new HttpClient
        {
            BaseAddress = new Uri(ServerConstants.SERVER_ROOT_URL)
        };

        Pin = _zavkr.Inn;
        ZvPozn = (int)_zavkr.PositionalNumber;

        InfoText = $"ПИН: {Pin}, заявка № {ZvPozn}";
        CheckGrsCommand = new AsyncRelayCommand(CheckGrsAsync);
    }

    // --- свойства для биндинга ---

    public string Pin { get; }
    public int ZvPozn { get; }

    private bool _isBusy;
    public bool IsBusy { get => _isBusy; set => SetProperty(ref _isBusy, value); }

    private string _infoText;
    public string InfoText { get => _infoText; set => SetProperty(ref _infoText, value); }

    private string _passportSeries;
    public string PassportSeries { get => _passportSeries; set => SetProperty(ref _passportSeries, value); }

    private string _passportNumber;
    public string PassportNumber { get => _passportNumber; set => SetProperty(ref _passportNumber, value); }

    private string _resultMessage;
    public string ResultMessage { get => _resultMessage; set => SetProperty(ref _resultMessage, value); }

    private string _fio;
    public string Fio
    {
        get => _fio;
        set
        {
            if (SetProperty(ref _fio, value))
                OnPropertyChanged(nameof(HasResult));
        }
    }

    private string _birthDate;
    public string BirthDate { get => _birthDate; set => SetProperty(ref _birthDate, value); }

    private string _address;
    public string Address { get => _address; set => SetProperty(ref _address, value); }

    private string _passportStatus;
    public string PassportStatus { get => _passportStatus; set => SetProperty(ref _passportStatus, value); }

    private DateTime? _lastQueryDate;
    public DateTime? LastQueryDate { get => _lastQueryDate; set => SetProperty(ref _lastQueryDate, value); }

    public bool HasResult => !string.IsNullOrWhiteSpace(Fio);

    public IAsyncRelayCommand CheckGrsCommand { get; }

    private async Task CheckGrsAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        ResultMessage = string.Empty;

        try
        {
            // 1. UserId из сессии (otNom)
            var otNomString = await SessionManager.GetOtNomAsync();
            int.TryParse(otNomString, out var userId);

            var dto = new GrsCheckDto
            {
                Pin = Pin,
                PassportSeries = PassportSeries ?? string.Empty,
                PassportNumber = PassportNumber ?? string.Empty,
                ZvPozn = ZvPozn,
                TypeClient = 1,      // ФЛ
                UserId = userId
            };

            // 2. IK-запрос
            var ikResponse = await _client.PostAsJsonAsync("api/ik", dto);
            var ikText = await ikResponse.Content.ReadAsStringAsync();

            if (!ikResponse.IsSuccessStatusCode)
            {
                ResultMessage = $"Ошибка IK-запроса: {ikText}";
                return;
            }

            ResultMessage = "IK-запрос отправлен. Загружаем данные клиента...";

            // 3. Получаем данные из InfoKlientGRS
            var infoUrl = $"api/ik/info?pin={Pin}&zvPozn={ZvPozn}";
            var infoResponse = await _client.GetAsync(infoUrl);
            var infoText = await infoResponse.Content.ReadAsStringAsync();

            if (!infoResponse.IsSuccessStatusCode)
            {
                ResultMessage = $"Ошибка получения данных IK: {infoText}";
                return;
            }

            var info = await infoResponse.Content.ReadFromJsonAsync<GrsIkInfoDto>();

            if (info == null)
            {
                ResultMessage = "Данные IK по клиенту не найдены.";
                return;
            }

            // 4. Заполняем свойства для UI
            Fio = info.Fio;
            BirthDate = info.DateOfBirth;
            Address = info.FullAddress;
            PassportStatus = info.PassportStatus;
            LastQueryDate = info.DateQuery;

            ResultMessage = "Данные из ГРС успешно получены.";
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
}