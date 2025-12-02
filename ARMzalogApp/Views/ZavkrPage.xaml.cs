using ARMzalogApp.Models;
using ARMzalogApp.Views.ZavkrPages;
using ARMzalogApp.Views.ZavkrPages.ResumePages;

namespace ARMzalogApp.Views;

public partial class ZavkrPage : ContentPage
{
    private Zavkr _selectedZavkr;
    //private ZmainView _selectedZmainView;

    public ZavkrPage(Zavkr selectedZavkr)
    {
        InitializeComponent();
        _selectedZavkr = selectedZavkr;
        //_selectedZmainView = selectedZavkr;

        titleLabel.Text = _selectedZavkr.Title;
        //descriptionLabel.Text = _selectedZavkr.LoanReferenceName;

        BindingContext = this;
        InitializeLocationAsync();
    }

    private async void InitializeLocationAsync()
    {
        try
        {
            var location = await Geolocation.GetLocationAsync();

            if (location != null)
            {
                string latitude = location.Latitude.ToString();
                string longitude = location.Longitude.ToString();
                await SecureStorage.SetAsync("Latitude", latitude);
                await SecureStorage.SetAsync("Longitude", longitude);
            }
            else
            {
                await DisplayAlert("Предупреждение", "Геолокация у вас не работает. Фотографии будут без местоположения", "OK");
            }
        }
        catch (FeatureNotEnabledException ex)
        {
            await DisplayAlert("Ошибка", "Не удалось получить местоположение", "OK");
            await DisplayAlert("Ошибка", "Включите геолокацию", "OK");
            await Shell.Current.GoToAsync("//HomePage");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Unable to get location: {ex.Message}", "OK");
        }
    }

    private void OnButtonPhotoClientClicked2(object sender, EventArgs e, int i)
    {
        Navigation.PushAsync(new PhotoClient(_selectedZavkr));
    }
    private void OnButtonZhitelstvaClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ResidencePhoto(_selectedZavkr));
    }

    private void OnButtonDeatelnostiClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ListActivityPhoto(_selectedZavkr));
    }

    private void OnButtonZalogClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Zalog(_selectedZavkr));
    }
    private void OnButtonPhotoClientClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new PhotoClient(_selectedZavkr));
    }

    private async void OnOpiuPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("OpiuPage");
        //Navigation.PushAsync(new OpiuPage(_selectedZmainView));
    }

    private async void OnAnketaPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("AnketaPage");
    }

    // =========== НОВОЕ: Соц-фонд ===========
    private async void OnSocFondClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SocFondPage(_selectedZavkr));
    }

    // =========== НОВОЕ: ГРС (паспорт/IK) ===========
    private async void OnPassportClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new GrsCheckPage(_selectedZavkr));
    }

    // =========== НОВОЕ: КИБ ===========
    private async void OnKibClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new KibCheckPage(_selectedZavkr));
    }
    private async void OnResumePage(object sender, EventArgs e)
    {
        if(_selectedZavkr.PositionalNumber == null)
        {
            await DisplayAlert("Ошибка", "Номер позиции не может быть пустым", "OK");
            return;
        }
        Navigation.PushAsync(new ResumeZarplPage(_selectedZavkr.PositionalNumber.ToString() ));
    }

}