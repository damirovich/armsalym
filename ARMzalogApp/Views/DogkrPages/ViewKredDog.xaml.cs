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
}