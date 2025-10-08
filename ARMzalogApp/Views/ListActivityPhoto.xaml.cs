using ARMzalogApp.Models;
namespace ARMzalogApp.Views;

public partial class ListActivityPhoto : ContentPage
{
    private Zavkr _selectedZavkr;

    public ListActivityPhoto(Zavkr selectedZavkr)
	{
		InitializeComponent();
        _selectedZavkr = selectedZavkr;
    }
    private void OnTakeFirstActivityButtonClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ActivityPhoto(_selectedZavkr));
    }
    private void OnTakeSecondActivityButtonClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ActivityPhoto_2(_selectedZavkr));
    }
    private void OnTakeThirdActivityButtonClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ActivityPhoto_3(_selectedZavkr));
    }
}