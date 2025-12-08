using ARMzalogApp.ViewModels;

namespace ARMzalogApp.Views.ZavkrPages;

public partial class CreateLoanZarp : ContentPage
{
    public CreateLoanZarp()
    {
        InitializeComponent();
        BindingContext = new CreateLoanZarpViewModel();
    }

}