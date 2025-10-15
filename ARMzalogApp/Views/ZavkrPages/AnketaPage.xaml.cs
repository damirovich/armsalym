namespace ARMzalogApp.Views.ZavkrPages;

public partial class AnketaPage : ContentPage
{
	public AnketaPage()
	{
		InitializeComponent();
	}

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        bool result = await DisplayAlert("Подтверждение", "Вы уверены что хотите вернуться? Несохраненные данные будут потеряны.", "Да", "Нет");
        if (result)
        {
            await Navigation.PopAsync();
        }
    }

}