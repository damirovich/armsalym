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
        await Shell.Current.GoToAsync("NewZavkrPage");
    }

}