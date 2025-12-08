using ARMzalogApp.Models;
using ARMzalogApp.ViewModels;

namespace ARMzalogApp.Views.ZavkrPages;

public partial class SocFondPage : ContentPage
{
    public SocFondPage(Zavkr zavkr)
    {
        InitializeComponent();
        BindingContext = new SocFondPageViewModel(zavkr);
    }
}