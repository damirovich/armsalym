namespace ARMzalogApp.Views.ZavkrPages.ResumePages;

public partial class ResumeZarplPage : ContentPage
{
    private string _pozn;
    public ResumeZarplPage(string pozn)
	{
		InitializeComponent();
        _pozn = pozn;
        poznLabel.Text = _pozn;

        BindingContext = this;

    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        bool result = await DisplayAlert("Подтверждение", "Вы уверены что хотите вернуться? ", "Да", "Нет");
        if (result)
        {
            await Navigation.PopAsync();
        }
    }

}