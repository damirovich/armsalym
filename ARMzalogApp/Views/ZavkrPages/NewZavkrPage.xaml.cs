namespace ARMzalogApp.Views.ZavkrPages;

public partial class NewZavkrPage : ContentPage
{
	public NewZavkrPage()
	{
		InitializeComponent();
	}
    // 1. Зарплатные (70, 71)
    private async void OnZarplatClicked(object sender, EventArgs e)
    {
        if (sender is Button btn)
        {
            btn.IsEnabled = false; // Блокируем, чтобы не нажали дважды
            btn.Opacity = 0.5;     // Визуальный эффект нажатия
        }
        await Shell.Current.GoToAsync($"{nameof(CreateLoanZarp)}?typRez=70&typRez1=71");

        if (sender is Button btnBack)
        {
            btnBack.IsEnabled = true;
            btnBack.Opacity = 1.0;
        }
    }

    // 2. Беззалоговые (60, 61)
    private async void OnBezzalogClicked(object sender, EventArgs e)
    {
        if (sender is Button btn)
        {
            btn.IsEnabled = false; // Блокируем, чтобы не нажали дважды
            btn.Opacity = 0.5;     // Визуальный эффект нажатия
        }
        await Shell.Current.GoToAsync($"{nameof(CreateLoanZarp)}?typRez=60&typRez1=61");

        if (sender is Button btnBack)
        {
            btnBack.IsEnabled = true;
            btnBack.Opacity = 1.0;
        }
    }

    // 3. Залоговые (50, 51)
    private async void OnZalogClicked(object sender, EventArgs e)
    {
        if (sender is Button btn)
        {
            btn.IsEnabled = false; // Блокируем, чтобы не нажали дважды
            btn.Opacity = 0.5;     // Визуальный эффект нажатия
        }
        await Shell.Current.GoToAsync($"{nameof(CreateLoanZarp)}?typRez=50&typRez1=51");
        if (sender is Button btnBack)
        {
            btnBack.IsEnabled = true;
            btnBack.Opacity = 1.0;
        }
    }

}