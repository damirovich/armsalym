using ARMzalogApp.ViewModels;

namespace ARMzalogApp.Views.ZavkrPages;

public partial class CreateLoanZarp : ContentPage
{
   private readonly CreateLoanZarpViewModel _viewModel;

    public CreateLoanZarp()
    {
        InitializeComponent();

        // Создаем ViewModel
        _viewModel = new CreateLoanZarpViewModel();
        
        // Привязываем контекст данных
        BindingContext = _viewModel;
    }

}