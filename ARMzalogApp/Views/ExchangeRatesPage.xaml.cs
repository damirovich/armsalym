using ARMzalogApp.ViewModels;
namespace ARMzalogApp.Views;

public partial class ExchangeRatesPage : ContentPage
{
	public ExchangeRatesPage()
	{
		InitializeComponent();
        BindingContext = new ExchangeRatesViewModel();
    }
}