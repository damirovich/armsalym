using ARMzalogApp.Sevices;
using ARMzalogApp.Models;
namespace ARMzalogApp.Views;

public partial class PoruchPhoto : ContentPage
{
    private PoruchZavkr _selectedPoruch;
    public PoruchPhoto(PoruchZavkr selectedPoruch)
	{
		InitializeComponent();
        _selectedPoruch = selectedPoruch;
    }

    private async void OnTakePhotoSelfieButtonClicked(object sender, EventArgs e)
    {
        try
        {
            string latitude = "";
            string longitude = "";

            var photoData = await TakePhotoAsync();
            if (photoData != null)
            {
                loadingIndicator.IsRunning = true;
                loadingIndicator.IsVisible = true;
                string inn = _selectedPoruch.Inn;
                string ZvPozn = _selectedPoruch.Pozn.ToString();
                string type = "4";
                string id = "";
                //int otNom = UserData.CurrentUser.UserNumber;
                string _otNom = await SecureStorage.Default.GetAsync("otNom");
                int otNom = Int32.Parse(_otNom);
                string token = FCMTokenSingleton.Instance.FCMToken;

                var service = new SavingService();
                string result = await service.SaveAbsFile(inn, ZvPozn, longitude, latitude, photoData, type, id, otNom, token);
                loadingIndicator.IsRunning = false;
                loadingIndicator.IsVisible = false;
                //await DisplayAlert("Успех", "Успешно сохранено", "OK");
                if (result == "OK")
                {
                    await DisplayAlert("Успех", "Успешно сохранено", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
    private async void OnTakePhotoFrontPassButtonClicked(object sender, EventArgs e)
    {
        try
        {
            string latitude = "";
            string longitude = "";
            var photoData = await TakePhotoAsync();
            if (photoData != null)
            {
                loadingIndicator.IsRunning = true;
                loadingIndicator.IsVisible = true;
                string inn = _selectedPoruch.Inn;
                string ZvPozn = _selectedPoruch.Pozn.ToString();
                string type = "5";
                string id = "";
                //int otNom = UserData.CurrentUser.UserNumber;
                string _otNom = await SecureStorage.Default.GetAsync("otNom");
                int otNom = Int32.Parse(_otNom);
                string token = FCMTokenSingleton.Instance.FCMToken;

                var service = new SavingService();
                string result = await service.SaveAbsFile(inn, ZvPozn, longitude, latitude, photoData, type, id, otNom, token);
                loadingIndicator.IsRunning = false;
                loadingIndicator.IsVisible = false;
                if (result == "OK")
                {
                    await DisplayAlert("Успех", "Успешно сохранено", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
    private async void OnTakePhotoBackPassButtonClicked(object sender, EventArgs e)
    {
        try
        {
            string latitude = "";
            string longitude = "";
            var photoData = await TakePhotoAsync();
            if (photoData != null)
            {
                loadingIndicator.IsRunning = true;
                loadingIndicator.IsVisible = true;
                string inn = _selectedPoruch.Inn;
                string ZvPozn = _selectedPoruch.Pozn.ToString();
                string type = "6";
                string id = "";
                //int otNom = UserData.CurrentUser.UserNumber;
                string _otNom = await SecureStorage.Default.GetAsync("otNom");
                int otNom = Int32.Parse(_otNom);
                string token = FCMTokenSingleton.Instance.FCMToken;

                var service = new SavingService();
                string result = await service.SaveAbsFile(inn, ZvPozn, longitude, latitude, photoData, type, id, otNom, token);
                loadingIndicator.IsRunning = false;
                loadingIndicator.IsVisible = false;
                if (result == "OK")
                {
                    await DisplayAlert("Успех", "Успешно сохранено", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private async Task<byte[]> TakePhotoAsync()
    {
        var photo = await MediaPicker.CapturePhotoAsync();
        if (photo != null)
        {
            using (var stream = await photo.OpenReadAsync())
            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
        return null;
    }

}