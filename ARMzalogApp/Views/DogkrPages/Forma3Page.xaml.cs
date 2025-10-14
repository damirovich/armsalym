using ARMzalogApp.Models.Responses;
namespace ARMzalogApp.Views.DogkrPages;

public partial class Forma3Page : ContentPage
{

    private DogkrResponse _selectedDogkr;

    public Forma3Page(DogkrResponse selectedDogkr)
    {
        InitializeComponent();
        _selectedDogkr = selectedDogkr;
        webView.Source = $"https://reports.salymfinance.kg//?Report=f3&otv={_selectedDogkr.DgPozn}";

        BindingContext = this;
    }
}