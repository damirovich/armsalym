namespace ARMzalogApp.Views;

public partial class SetPinPage : ContentPage
{
    public SetPinPage()
    {
        InitializeComponent();
    }

    private async void OnSetPinButtonClicked(object sender, EventArgs e)
    {
        var pin1 = PinEntry1.Text;
        var pin2 = PinEntry2.Text;

        if (!string.IsNullOrWhiteSpace(pin1) && pin1.Length == 4 &&
            !string.IsNullOrWhiteSpace(pin2) && pin2.Length == 4)
        {
            if (pin1 == pin2)
            {
                //var session = await GetSessionAsync();
                await SaveSessionAsync(pin1);
                //await DisplayAlert("Успех", "PIN-код успешно установлен", "OK");
                await Shell.Current.GoToAsync(nameof(PinVerificationPage));
            }
            else
            {
                await DisplayAlert("Ошибка", "PIN-коды не совпадают", "OK");
                PinEntry1.Text = string.Empty;
                PinEntry2.Text = string.Empty;
            }
        }
        else
        {
            await DisplayAlert("Ошибка", "Введите действительный PIN-код из 4 цифр", "OK");
        }
    }

    private async Task SaveSessionAsync(string pin)
    {
        await SecureStorage.SetAsync("pin", pin);
    }

    //private async Task<(string Username, string Token, string Pin)> GetSessionAsync()
    //{
    //    var username = await SecureStorage.GetAsync("username");
    //    var token = await SecureStorage.GetAsync("token");
    //    var pin = await SecureStorage.GetAsync("pin");
    //    return (username, token, pin);
    //}
}
