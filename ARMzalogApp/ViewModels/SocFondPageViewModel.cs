using ARMzalogApp.Constants;
using ARMzalogApp.Integrations.Dtos.SocialFundDtos;
using ARMzalogApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net.Http.Json;

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
    }

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

    private IAsyncRelayCommand _getPensionCommand;
    public IAsyncRelayCommand GetPensionCommand =>
        _getPensionCommand ??= new AsyncRelayCommand(GetPensionAsync);

    private IAsyncRelayCommand _getWorkPeriodsCommand;
    public IAsyncRelayCommand GetWorkPeriodsCommand =>
        _getWorkPeriodsCommand ??= new AsyncRelayCommand(GetWorkPeriodsAsync);

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
                TypeClient = 1 // пока считаем, что физлицо
            };

            var response = await _client.PostAsJsonAsync("api/v1/socfond/pension-info-with-sum", dto);
            var text = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                ResultMessage = $"Ошибка: {text}";
                return;
            }

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

            ResultMessage = "Периоды работы успешно получены и сохранены.";
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