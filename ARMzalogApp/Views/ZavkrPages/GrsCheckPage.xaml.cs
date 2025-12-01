using ARMzalogApp.Models;
using ARMzalogApp.ViewModels;

namespace ARMzalogApp.Views.ZavkrPages;

public partial class GrsCheckPage : ContentPage
{
    public GrsCheckPage(Zavkr zavkr)
    {
		InitializeComponent();
        BindingContext = new GrsCheckPageViewModel(zavkr);
    }
}