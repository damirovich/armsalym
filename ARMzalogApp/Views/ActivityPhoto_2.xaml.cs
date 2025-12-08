using ARMzalogApp.Models;
using ARMzalogApp.Sevices;
using Newtonsoft.Json;
using ARMzalogApp.Constants;
namespace ARMzalogApp.Views;

public partial class ActivityPhoto_2 : ContentPage
{
    private Zavkr _selectedZavkr;
    private string _latitude;
    private string _longitude;

    public ActivityPhoto_2(Zavkr selectedZavkr)
    {
        InitializeComponent();
        _selectedZavkr = selectedZavkr;
        _ = LoadSessionDataAsync();
        totalNum.Text = "загрузка общего количества фотографий";
        LoadDataFromService();
    }

    private async Task LoadSessionDataAsync()
    {
        var (latitude, longitude) = await GetSessionCoordinatesAsync();
        _latitude = latitude;
        _longitude = longitude;
    }

    private async Task<(string Latitude, string Longitude)> GetSessionCoordinatesAsync()
    {
        var latitudeTask = SecureStorage.Default.GetAsync("Latitude");
        var longitudeTask = SecureStorage.Default.GetAsync("Longitude");
        await Task.WhenAll(latitudeTask, longitudeTask);

        return (await latitudeTask, await longitudeTask);
    }

    private async void OnTakePhotoButtonClicked(object sender, EventArgs e)
    {
        string latitude = _latitude;
        string longitude = _longitude;
        var photoData = await TakePhotoAsync();
        if (photoData != null)
        {
            loadingIndicator.IsRunning = true;
            loadingIndicator.IsVisible = true;
            string inn = _selectedZavkr.Inn;
            string ZvPozn = _selectedZavkr.PositionalNumber.ToString();
            string type = "7";
            string id = "";
            string _otNom = await SecureStorage.Default.GetAsync("otNom");
            int otNom = Int32.Parse(_otNom);
            string token = FCMTokenSingleton.Instance.FCMToken;

            var service = new SavingService();
            string result = await service.SaveAbsFile(inn, ZvPozn, longitude, latitude, photoData, type, id, otNom, token);
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
            if (result == "OK")
            {
                await DisplayAlert("Успех", "Успешно отправлено", "OK");
            }
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

    private async void OnTakeGalleryPhotoClicked(object sender, EventArgs e)
    {
        var photoData = await TakePhotoAsync();
        if (photoData != null)
        {
#if ANDROID
            string dataNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string latitude = _latitude;
            string longitude = _longitude;
            string namePic = latitude + "_" + longitude + "_" + dataNow + ".png";
            string ZvPozn = _selectedZavkr.PositionalNumber.ToString();
            var saved =  ARMzalogApp.Platforms.Android.Services.SavePictureService.SavePicture(photoData, namePic, ZvPozn);

            if (saved)
            {
                await DisplayAlert("Óñïåøíî", "Ôîòî ñîõðàíåíî â ãàëåðåþ", "OK");
            }
            else
            {
                await DisplayAlert("Îøèáêà", "Îøèáêà ïðè ñîõðàíåíèè", "OK");
            }
#endif
        }
    }

    private async void OnUploadPhotoClicked(object sender, EventArgs e)
    {
        var photo = await MediaPicker.PickPhotoAsync();

        if (photo != null)
        {
            loadingIndicator.IsRunning = true;
            loadingIndicator.IsVisible = true;
            var fileName = photo.FileName;
            var parts = fileName.Split('_');

            if (parts.Length >= 2)
            {
                var latitude = parts[0];
                var longitude = parts.Length > 2 ? parts[1] : "";

                string inn = _selectedZavkr.Inn;
                string ZvPozn = _selectedZavkr.PositionalNumber.ToString();
                string type = "7";
                string id = "";
                string _otNom = await SecureStorage.Default.GetAsync("otNom");
                int otNom = Int32.Parse(_otNom);
                string token = FCMTokenSingleton.Instance.FCMToken;
                byte[] photoData;
                using (var stream = await photo.OpenReadAsync())
                using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    photoData = memoryStream.ToArray();
                }
                var service = new SavingService();
                string result = await service.SaveAbsFile(inn, ZvPozn, latitude, longitude, photoData, type, id, otNom, token);

                if (result == "OK")
                {
                    //var deleted = DeletePhoto(photo);
                    await DisplayAlert("Успех", "Успешно отправлено", "OK");

                }
            }
            else
            {
                await DisplayAlert("Ошибка", "Фотография без локации", "OK");
            }
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
        }
    }

    private async void LoadDataFromService()
    {
        string inn = _selectedZavkr.Inn;
        string ZvPozn = _selectedZavkr.PositionalNumber.ToString();
        string path = $"{inn}\\ZAVKR\\{ZvPozn}\\ActivityPhoto2";
        //string path = $"11812199000548\\ZAVKR\\838091986\\ActivityPhoto";
        string typePhoto = "7";

        string url = ServerConstants.SERVER_ROOT_URL + "api/LoanReference/GetCountOfImage?path=" + path + "&typePhoto=" + typePhoto;

        using var httpClient = new HttpClient();
        HttpResponseMessage response = await httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            string responseData = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<int>(responseData);
            if (data >= 0)
            {
                totalNum.Text = "Всего " + data.ToString() + " фотографий";
            }
            else
            {
                totalNum.Text = "неизвестно";
            }
        }
    }


}
