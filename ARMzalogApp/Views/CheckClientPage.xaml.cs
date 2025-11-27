using ARMzalogApp.Helpers;
using ARMzalogApp.ViewModels;

namespace ARMzalogApp.Views;

public partial class CheckClientPage : ContentPage
{
    public CheckClientPage()
    {
        InitializeComponent();
        BindingContext = ServiceHelper.GetService<CheckClientViewModel>();
    }
}