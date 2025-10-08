using System.Globalization;
using ARMzalogApp.Resources.Localization;
using ARMzalogApp.Models;
using ARMzalogApp.Sevices;

using static System.Net.Mime.MediaTypeNames;

namespace ARMzalogApp.Views;

public partial class AboutPage : ContentPage
{
    private User _user;
    private bool isLargeFont = false;
    public AboutPage(User user)
    {
        InitializeComponent();
        _user = user;
        //titleLabel.Text = _user.UserName;
        //titleLabel.Text = SessionManager.GetoFio();

        BindingContext = this;
        versionLabel.Text = $"Версия {AppInfo.VersionString}";
        FontSizePicker.SelectedIndex = 0;
        var languages = new List<Language>
            {
                new Language { Code = "ru-RU", Name = "Русский" },
                new Language { Code = "ky-KY", Name = "Кыргызча" }
            };

        foreach (var language in languages)
        {
            LanguagePicker.Items.Add(language.Name);
        }

        LanguagePicker.SelectedIndex = 0;

        LocalizationResourceManager = LocalizationResourceManager.Instance;
        //CodeBehindTranslator.SetBinding(Label.TextProperty, new Binding($"[{nameof(AppResources.Intro)}]", source: LocalizationResourceManager));


    }
    //public ICommand ExitCommand { get; }
    //ExitCommand = new AsyncRelayCommand(OnExit);
    private async void OnExit(object sender, EventArgs e)
    {
        SecureStorage.Default.RemoveAll();
        Preferences.Default.Clear();
        Microsoft.Maui.Controls.Application.Current.Quit();
        //System.Diagnostics.Process.GetCurrentProcess().Kill();
    }


    private void LanguageChanged(object sender, EventArgs e)
    {
        var selectedLanguage = LanguagePicker.SelectedItem.ToString();
        var languages = new Dictionary<string, string>
            {
                { "Русский", "ru-RU" },
                { "Кыргызча", "ky-KY" }
            };

        if (languages.TryGetValue(selectedLanguage, out var cultureCode))
        {
            LocalizationResourceManager.Instance.SetCulture(new CultureInfo(cultureCode));
        }
    }

    public LocalizationResourceManager LocalizationResourceManager { get; }
    private void OnFontSizePickerSelectedIndexChanged(object sender, EventArgs e)
    {
        if (FontSizePicker.SelectedIndex == 0)
        {
            App.Settings.FontSize = 14; // маленький размер шрифта
        }
        else if (FontSizePicker.SelectedIndex == 1)
        {
            App.Settings.FontSize = 22; // Большой размер шрифта
        }
    }
    private void ShowValuta(object sender, EventArgs e)
    {
         //Shell.Current.GoToAsync($"//{nameof(ExchangeRatesPage)}");
         Navigation.PushAsync(new ExchangeRatesPage());
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        //var session = await SessionManager.GetSessionAsync();
        //titleLabel.Text = session.Fio;
        titleLabel.Text = SessionManager.GetoFio();
    }

    private async void OnDelete(object sender, EventArgs e)
    {
        bool isAgree = await DisplayAlert("Подтвердите действие", "Вы уверены что хотите удалить аккаунт", "Да", "Нет");
        if (isAgree)
        {
            var otNom = await SecureStorage.GetAsync("otNom") ?? "";
            int status = 4;
            var service = new AccountRepository();
            string result = await service.UpdateAccountStatus(otNom, status,"0");
            if (result == "OK")
            {
                SecureStorage.Default.RemoveAll();
                // Preferences.Default.Clear();
                await DisplayAlert("Успех", "Аккаунт удален", "Ок");
            }

        }

    }
}