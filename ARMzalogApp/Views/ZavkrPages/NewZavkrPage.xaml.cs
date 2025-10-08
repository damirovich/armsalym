namespace ARMzalogApp.Views.ZavkrPages;

public partial class NewZavkrPage : ContentPage
{
	public NewZavkrPage()
	{
		InitializeComponent();
	}

    private async void OnZarplatClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("CreateLoanZarp");
    }

}