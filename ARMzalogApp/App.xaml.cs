using ARMzalogApp.Models;
using ARMzalogApp.Theme;

namespace ARMzalogApp
{
    public partial class App : Application
    {
        public static UserInfo UserIndo;
        public static AppSettings Settings { get; private set; }
        private LightTheme lightTheme = new LightTheme();
        private DarkTheme darkTheme = new DarkTheme();
        public App()
        {
            InitializeComponent();

            Settings = new AppSettings()
            {
                FontSize = 16,
            };
            ApplyTheme(Application.Current.RequestedTheme);
            Application.Current.RequestedThemeChanged += (s, a) => ApplyTheme(a.RequestedTheme);

            //MainPage = new AppShell();
            MainPage = new NavigationPage(new MainPage());

        }

        protected override async void OnStart()
        {
            var session = await SessionManager.GetSessionAsync();
            if (!string.IsNullOrEmpty(session.Pin))
            {
                //MainPage = new NavigationPage(new PinVerificationPage());
                MainPage = new AppShell();
                await Shell.Current.GoToAsync($"PinVerificationPage");
            }
            else
            {
                MainPage = new AppShell();
                await Shell.Current.GoToAsync($"LoginPage");
            }
        }

        private void ApplyTheme(AppTheme theme)
        {
            //Resources.MergedDictionaries.Clear();
            if (theme == AppTheme.Dark)
            {
                Resources.MergedDictionaries.Add(darkTheme);
            }
            else
            {
                Resources.MergedDictionaries.Add(lightTheme);
            }
          
        }
    }
}
