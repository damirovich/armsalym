using ARMzalogApp.Models.Responses;

namespace ARMzalogApp.Views.DogkrPages;

public partial class ViewKredDog : ContentPage
{
    private DogkrResponse _selectedDogkr;
    
    public ViewKredDog(DogkrResponse selectedDogkr)
	{
        InitializeComponent();
        _selectedDogkr = selectedDogkr;
        titleLabel.Text = _selectedDogkr.DgPozn.ToString();

        BindingContext = this;
	}

    private async void OnUkzClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UkzPage(_selectedDogkr));
    }

    private async void OnForma3Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Forma3Page(_selectedDogkr));
    }
}