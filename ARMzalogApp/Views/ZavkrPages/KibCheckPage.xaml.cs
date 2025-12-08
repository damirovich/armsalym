using ARMzalogApp.Helpers;
using ARMzalogApp.Models;
using ARMzalogApp.Sevices.Integrations;
using ARMzalogApp.ViewModels;

namespace ARMzalogApp.Views.ZavkrPages;

public partial class KibCheckPage : ContentPage
{
	public KibCheckPage(Zavkr zavkr)
	{
		InitializeComponent();
        BindingContext = new KibCheckPageViewModel(zavkr, ServiceHelper.GetService<IKibIntegrationService>());
    }
}