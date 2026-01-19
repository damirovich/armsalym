namespace ARMzalogApp.Views;

public partial class ChosePage : ContentPage
{
	public ChosePage()
	{
		InitializeComponent();
	}


    private async void OnZavkrPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("HomePage");
    }

    private async void OnDogvkPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("ListDogkr");
    }

    private async void OnNewZavkrPage(object sender, EventArgs e)
    {
        if (sender is Button btn)
        {
            btn.IsEnabled = false; // Блокируем, чтобы не нажали дважды
            btn.Opacity = 0.5;     // Визуальный эффект нажатия
        }

        await Shell.Current.GoToAsync("CreateLoanZarp");

        if (sender is Button btnBack)
        {
            btnBack.IsEnabled = true;
            btnBack.Opacity = 1.0;
        }
    }

}