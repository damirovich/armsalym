using ARMzalogApp.Models;


namespace ARMzalogApp.Views.ZavkrPages;

public partial class OpiuPage : ContentPage
{
    private ZmainView _selectedZavkr;

    public OpiuPage(ZmainView selectedZavkr)
	{
		InitializeComponent();
        _selectedZavkr = selectedZavkr;

    }

    public OpiuPage()
    {
        InitializeComponent();

    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        bool result = await DisplayAlert("Подтверждение", "Вы уверены, что хотите вернуться? Несохранённые данные будут потеряны.", "Да", "Нет");
        if (result)
        {
            await Navigation.PopAsync();
        }
    }
}