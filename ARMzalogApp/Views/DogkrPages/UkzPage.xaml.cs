using ARMzalogApp.Models.Responses;
namespace ARMzalogApp.Views.DogkrPages;

public partial class UkzPage : ContentPage
{
    private DogkrResponse _selectedDogkr;

    public UkzPage(DogkrResponse selectedDogkr)
    {
        InitializeComponent();
        _selectedDogkr = selectedDogkr;

        webView.Source = $"https://reports.salymfinance.kg//?Report=ukz&otv={_selectedDogkr.DgPozn}";

        webView.Navigating += (s, e) =>
        {
            Console.WriteLine($"Loading: {e.Url}");
        };

        webView.Navigated += (s, e) =>
        {
            Console.WriteLine($"Loaded: {e.Url}");
        };

        BindingContext = this;
    }
}