using ARMzalogApp.Models;
using ARMzalogApp.Sevices;

namespace ARMzalogApp.Views;

public partial class CreateZalogPage : ContentPage
{
    private Zavkr _selectedZavkr;
    private Dictionary<string, int> zalValues;

    public CreateZalogPage(Zavkr selectedZavkr)
	{
		InitializeComponent();
        _selectedZavkr = selectedZavkr;
        zalValues = new Dictionary<string, int>
        {
            { "Земельный участок", 1 },
            { "Жилая недвижимость", 3 },
            { "Транспортное средство", 4 },
            { "Другие", 5 }
        };
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        string selectedTeam = zalPicker.SelectedItem?.ToString();

        if (!string.IsNullOrEmpty(selectedTeam) && zalValues.ContainsKey(selectedTeam))
        {
            loadingIndicator.IsRunning = true;
            loadingIndicator.IsVisible = true;
            int selectedValue = zalValues[selectedTeam];
            string _otNom = await SecureStorage.Default.GetAsync("otNom");
            //DisplayAlert("Команда сохранена", $"Вы выбрали: {selectedTeam}", "OK");
            var service = new SavingService();
            string result = await service.SaveNewZalog(selectedValue, _selectedZavkr.PositionalNumber.ToString(), _otNom);
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
            if (result == "OK")
            {
                await DisplayAlert("Успех", "Новый залог создан", "OK");
                await Navigation.PushAsync(new ZavkrPage(_selectedZavkr));
            }
        }
        else
        {
            DisplayAlert("Ошибка", "Пожалуйста, выберите команду", "OK");
        }
    }

}