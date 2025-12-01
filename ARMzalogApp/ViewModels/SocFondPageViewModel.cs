using ARMzalogApp.Constants;
using ARMzalogApp.Integrations.Dtos.SocialFundDtos;
using ARMzalogApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Text.Json;

namespace ARMzalogApp.ViewModels;

public class SocFondPageViewModel : ObservableObject
{
    private readonly Zavkr _zavkr;
    private readonly HttpClient _client;

    public SocFondPageViewModel(Zavkr zavkr)
    {
        _zavkr = zavkr;
        _client = new HttpClient
        {
            BaseAddress = new Uri(ServerConstants.SERVER_ROOT_URL)
        };

        InfoText = $"ПИН: {_zavkr.Inn}, заявка № {_zavkr.PositionalNumber}";

        InitializePermissionCommand = new AsyncRelayCommand(InitializePermissionAsync);
        ConfirmPermissionCommand = new AsyncRelayCommand(ConfirmPermissionAsync);
        _getPensionCommand = new AsyncRelayCommand(GetPensionAsync);
        _getWorkPeriodsCommand = new AsyncRelayCommand(GetWorkPeriodsAsync);
    }

    // -------- общие поля --------

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    private string _infoText;
    public string InfoText
    {
        get => _infoText;
        set => SetProperty(ref _infoText, value);
    }

    private string _resultMessage;
    public string ResultMessage
    {
        get => _resultMessage;
        set => SetProperty(ref _resultMessage, value);
    }

    // -------- блок разрешений --------

    private string _lastName;
    public string LastName
    {
        get => _lastName;
        set => SetProperty(ref _lastName, value);
    }

    private string _firstName;
    public string FirstName
    {
        get => _firstName;
        set => SetProperty(ref _firstName, value);
    }

    private string _patronymic;
    public string Patronymic
    {
        get => _patronymic;
        set => SetProperty(ref _patronymic, value);
    }

    private string _phoneNumber;
    public string PhoneNumber
    {
        get => _phoneNumber;
        set => SetProperty(ref _phoneNumber, value);
    }

    private string _permissionCode;
    public string PermissionCode
    {
        get => _permissionCode;
        set => SetProperty(ref _permissionCode, value);
    }

    private string _requestId;
    public string RequestId
    {
        get => _requestId;
        set => SetProperty(ref _requestId, value);
    }

    private string _permissionId;
    public string PermissionId
    {
        get => _permissionId;
        set => SetProperty(ref _permissionId, value);
    }

    private string _permissionStatus;
    public string PermissionStatus
    {
        get => _permissionStatus;
        set => SetProperty(ref _permissionStatus, value);
    }

    public IAsyncRelayCommand InitializePermissionCommand { get; }
    public IAsyncRelayCommand ConfirmPermissionCommand { get; }

    // -------- команды пенсии / периодов --------

    private IAsyncRelayCommand _getPensionCommand;
    public IAsyncRelayCommand GetPensionCommand =>
        _getPensionCommand ??= new AsyncRelayCommand(GetPensionAsync);

    private IAsyncRelayCommand _getWorkPeriodsCommand;
    public IAsyncRelayCommand GetWorkPeriodsCommand =>
        _getWorkPeriodsCommand ??= new AsyncRelayCommand(GetWorkPeriodsAsync);

    public ObservableCollection<WorkPeriodMobileDto> WorkPeriods { get; } = new();


    // ==================  РАЗРЕШЕНИЕ (инициализация) ==================

    private async Task InitializePermissionAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        ResultMessage = string.Empty;

        try
        {
            if (string.IsNullOrWhiteSpace(_zavkr.Inn))
            {
                ResultMessage = "ПИН в заявке пустой.";
                return;
            }

            if (string.IsNullOrWhiteSpace(PhoneNumber))
            {
                ResultMessage = "Укажите номер телефона.";
                return;
            }

            var body = new InitializePermissionMobileRequestDto
            {
                Pin = _zavkr.Inn,
                PhoneNumber = PhoneNumber,
                FirstName = FirstName,
                LastName = LastName,
                Patronymic = Patronymic
            };

            var zvPozn = (int)_zavkr.PositionalNumber;

            var response = await _client.PostAsJsonAsync(
                $"api/v1/socfond/permissions/initialize?pozn={zvPozn}",
                body);

            var text = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                ResultMessage = $"Ошибка инициализации разрешения: {text}";
                return;
            }

            SfInitializePermissionResponseDto? dto;
            try
            {
                dto = JsonSerializer.Deserialize<SfInitializePermissionResponseDto>(
                    text,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch
            {
                ResultMessage = "Не удалось разобрать ответ Соцфонда.";
                return;
            }

            if (dto == null || !dto.OperationResult)
            {
                ResultMessage = dto?.Message ?? "Не удалось инициализировать разрешение.";
                return;
            }

            RequestId = dto.RequestId ?? string.Empty;
            PermissionStatus = "Код отправлен Соцфондом. Введите SMS-код.";
            ResultMessage = dto.Message ?? "Запрос на разрешение успешно отправлен.";
        }
        catch (Exception ex)
        {
            ResultMessage = $"Ошибка запроса в Соцфонд (инициализация): {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    // ==================  РАЗРЕШЕНИЕ (подтверждение кода) ==================

    private async Task ConfirmPermissionAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        ResultMessage = string.Empty;

        try
        {
            if (string.IsNullOrWhiteSpace(PermissionCode))
            {
                ResultMessage = "Введите код из SMS.";
                return;
            }

            if (string.IsNullOrWhiteSpace(RequestId))
            {
                ResultMessage = "Сначала отправьте запрос на разрешение (получите RequestId).";
                return;
            }

            var body = new SendPermissionCodeMobileRequestDto
            {
                Code = PermissionCode,
                RequestId = RequestId,
                Pin = _zavkr.Inn
            };

            var response = await _client.PostAsJsonAsync(
                "api/v1/socfond/permissions/confirm",
                body);

            var text = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                ResultMessage = $"Ошибка подтверждения разрешения: {text}";
                return;
            }

            SfConfirmPermissionResponseDto? dto;
            try
            {
                dto = JsonSerializer.Deserialize<SfConfirmPermissionResponseDto>(
                    text,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch
            {
                ResultMessage = "Не удалось разобрать ответ при подтверждении разрешения.";
                return;
            }

            if (dto == null || !dto.OperationResult)
            {
                ResultMessage = dto?.Message ?? "Не удалось подтвердить разрешение.";
                return;
            }

            PermissionId = dto.PermissionId ?? string.Empty;
            PermissionStatus = "Разрешение активно (Status = 1).";
            ResultMessage = dto.Message ?? "Разрешение успешно подтверждено.";
        }
        catch (Exception ex)
        {
            ResultMessage = $"Ошибка при подтверждении кода: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    // ==================  ПЕНСИЯ + СУММЫ ==================

    private async Task GetPensionAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        ResultMessage = string.Empty;

        try
        {
            var dto = new PensionWithSumRequestDto
            {
                Pin = _zavkr.Inn,
                ZvPozn = (int)_zavkr.PositionalNumber,
                TypeClient = 1 // ФЛ
            };

            var response = await _client.PostAsJsonAsync("api/v1/socfond/pension-info-with-sum", dto);
            var text = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                ResultMessage = $"Ошибка: {text}";
                return;
            }

            // Сейчас бек возвращает только message, без данных.
            ResultMessage = "Данные о пенсии успешно получены и сохранены.";
        }
        catch (Exception ex)
        {
            ResultMessage = $"Ошибка запроса в Соцфонд: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    // ==================  ПЕРИОДЫ РАБОТЫ ==================

    private async Task GetWorkPeriodsAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        ResultMessage = string.Empty;

        try
        {
            var dto = new WorkPeriodWithSumRequestDto
            {
                Pin = _zavkr.Inn,
                ZvPozn = (int)_zavkr.PositionalNumber
            };

            var response = await _client.PostAsJsonAsync("api/v1/socfond/work-periods-with-sum", dto);
            var text = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                ResultMessage = $"Ошибка: {text}";
                return;
            }

            ResultMessage = "Периоды работы успешно получены и сохранены. Загружаем данные...";

            // 2. Тянем сохранённые периоды из нового endpoint
            var viewResponse = await _client.GetAsync(
                $"api/v1/socfond/work-periods-view?pin={_zavkr.Inn}&zvPozn={(int)_zavkr.PositionalNumber}");

            var viewText = await viewResponse.Content.ReadAsStringAsync();

            if (!viewResponse.IsSuccessStatusCode)
            {
                ResultMessage = $"Периоды сохранены, но не удалось получить для отображения: {viewText}";
                return;
            }

            var periods = JsonSerializer.Deserialize<List<WorkPeriodMobileDto>>(
                viewText,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            WorkPeriods.Clear();
            if (periods != null)
            {
                foreach (var p in periods)
                    WorkPeriods.Add(p);
            }

            ResultMessage = $"Периоды работы загружены. Кол-во записей: {WorkPeriods.Count}.";
        }
        catch (Exception ex)
        {
            ResultMessage = $"Ошибка запроса в Соцфонд: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }


}
