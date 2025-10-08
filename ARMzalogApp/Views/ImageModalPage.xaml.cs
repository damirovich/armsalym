namespace ARMzalogApp.Views;

public partial class ImageModalPage : ContentPage
{
    public ImageModalPage(string filePath)
    {
        InitializeComponent();
        LargeImage.Source = filePath;
    }

    private async void OnCloseButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}