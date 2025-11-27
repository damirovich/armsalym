using ARMzalogApp.Integrations.Dtos.GrsDtos;
using ARMzalogApp.Integrations.Dtos.KibDtos;
using ARMzalogApp.Sevices.Integrations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ARMzalogApp.ViewModels;

public partial class CheckClientViewModel : ObservableObject
{
    private readonly IGrsIkService _grsIkService;
    private readonly IKibIntegrationService _kibService;
    private readonly ISocialFundIntegrationService _socFondService;
    public ObservableCollection<string> Services { get; } =
        new(new[] { "ГРС", "КИБ", "Соц-фонд" });

    // выбранный сервис
    [ObservableProperty]
    private string _selectedService = "ГРС";

    // поля ввода (общие/для ГРС)
    [ObservableProperty] private string _pin = string.Empty;
    [ObservableProperty] private string _passportSeries = string.Empty;
    [ObservableProperty] private string _passportNumber = string.Empty;

    // результат
    [ObservableProperty] private string? _resultMessage;
    [ObservableProperty] private DateTime? _lastQuery;
    [ObservableProperty] private bool _isBusy;

    // ===================
    //   КИБ ПОЛЯ
    // ===================

    [ObservableProperty] private string? _kibShortInfo;
    [ObservableProperty] private string _internalPassport;
    [ObservableProperty] private string _dateOfBirth;
    [ObservableProperty] private string _fullName;
    [ObservableProperty] private int _zvPozn;
    [ObservableProperty] private int _userId;
    [ObservableProperty] private string _selectedKibIdType = "Pin";
    [ObservableProperty] private string _selectedClientType = "1";


    [ObservableProperty] private string? _sfMessage;
    public CheckClientViewModel(IGrsIkService grsIkService, IKibIntegrationService kibService, ISocialFundIntegrationService socFondService)
    {
        _grsIkService = grsIkService;
        _kibService = kibService;
        _socFondService = socFondService;
    }

    // удобные флаги для IsVisible в XAML
    public bool IsGrsSelected => SelectedService == "ГРС";
    public bool IsKibSelected => SelectedService == "КИБ";
    public bool IsSfSelected => SelectedService == "Соц-фонд";
    public List<string> KibIdTypes { get; } =
[
    "Pin",
    "IdCard",
    "Passport"
];

    public List<string> ClientTypes { get; } =
[
    "Физлицо",
    "Юрлицо"
];
    partial void OnSelectedServiceChanged(string value)
    {
        OnPropertyChanged(nameof(IsGrsSelected));
        OnPropertyChanged(nameof(IsKibSelected));
        OnPropertyChanged(nameof(IsSfSelected));
    }

    [RelayCommand]
    private async Task CheckAsync()
    {
        if (string.IsNullOrWhiteSpace(Pin))
        {
            await Shell.Current.DisplayAlert("Ошибка", "Введите ПИН", "OK");
            return;
        }

        if (SelectedService == "ГРС")
        {
            await CheckGrsAsync();
        }
        else if (SelectedService == "КИБ")
        {
            await CheckKibAsync();
        }
        else if (SelectedService == "Соц-фонд")
        {
            await CheckSocFondAsync();
        }
    }

    private async Task CheckGrsAsync()
    {
        IsBusy = true;
        ResultMessage = null;
        LastQuery = null;

        try
        {
            var dto = new GrsCheckDto
            {
                Pin = Pin.Trim(),
                PassportSeries = PassportSeries.Trim(),
                PassportNumber = PassportNumber.Trim(),
                // пока отправляем тех. значения, позже подставим реальные из авторизации
                ZvPozn = 0,
                TypeClient = 0,
                UserId = 0
            };

            var res = await _grsIkService.SendIkAsync(dto);

            ResultMessage = res?.Message ?? "Ответ без сообщения";
            LastQuery = res?.LastQuery;
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task CheckKibAsync()
    {
        IsBusy = true;
        KibShortInfo = null;

        try
        {
            var dto = new KibSearchIndividualRequestDto
            {
                IdNumber = Pin,
                IdNumberType = SelectedKibIdType,
                InternalPassport = InternalPassport,
                DateOfBirth = DateOfBirth,
                FullName = FullName,
                ZvPozn = ZvPozn,
                UserId = UserId,
                TypeClient = ConvertClientType(SelectedClientType)
            };

            var result = await _kibService.SearchIndividualAsync(dto, CancellationToken.None);

            KibShortInfo = "Запрос успешно выполнен. XML получен.";
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка КИБ", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
    private async Task CheckSocFondAsync()
    {
        IsBusy = true;
        SfMessage = null;
        ResultMessage = null;

        try
        {
            if (ZvPozn <= 0)
            {
                await Shell.Current.DisplayAlert("Ошибка", "Укажи номер заявки (ZvPozn)", "OK");
                return;
            }

            var typeClient = ConvertClientType(SelectedClientType);

            var msg = await _socFondService.GetPensionInfoWithSumAsync(Pin.Trim(),ZvPozn,typeClient,CancellationToken.None);

            SfMessage = msg;
            ResultMessage = msg;
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка Соцфонда", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
    private int ConvertClientType(string type)
    {
        return type == "Юрлицо" ? 2 : 1;
    }
}