using ARMzalogApp.Models;
using ARMzalogApp.Sevices;
using ARMzalogApp.Views;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Plugin.LocalNotification;

namespace ARMzalogApp.ViewModels
{
    public partial class LoginPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string _userName;

        [ObservableProperty]
        private string _password;

        readonly ILoginRepository loginRepository = new LoginService();

        [ICommand]
        public async void Login()
        {
            if (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password))
            {
                var service = new LoginService();
                string uniq = "Android";
                User userInfo = await loginRepository.Login(UserName, Password);
                if (userInfo != null && userInfo?.UserNumber != 0) 
                {
                    var request = new NotificationRequest
                    {
                        NotificationId = 12345,
                        Title = "Вход",
                        Description = "Вы успешно вошли в свой аккаунт",
                        BadgeNumber = 10,

                        Schedule = new NotificationRequestSchedule
                        {
                            NotifyTime = DateTime.Now.AddSeconds(30)
                        }
                    };
                    LocalNotificationCenter.Current.Show(request);
                    await SaveSessionAsync(UserName, userInfo.UserNumber.ToString(), userInfo.otFio, userInfo.DepartmentId.ToString());

                    string ot_uid = userInfo.UserName ?? "";
                    string fio = userInfo.otFio ?? "";
                    string otdel = userInfo.DepartmentId.ToString() ?? "";
                    
                    string result1 = "моб - Успешный вход";
                    
                    string result = await service.LoginLog(ot_uid, fio, otdel, uniq, result1);

                    await Shell.Current.GoToAsync($"SetPinPage");

                    //await Shell.Current.GoToAsync($"//{nameof(HomePage)}");

                    try
                    {
                        var statusLoc = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                        if (statusLoc != PermissionStatus.Granted)
                        {
                            statusLoc = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                            if (statusLoc != PermissionStatus.Granted)
                            {
                                Console.WriteLine("Отклонено в разрешении на доступ к локации");
                                await Application.Current.MainPage.DisplayAlert("Отказано в разрешении", "нету доступа к локации", "OK");

                                return;
                            }
                        }
                        var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
                        if (status != PermissionStatus.Granted)
                        {
                            status = await Permissions.RequestAsync<Permissions.Camera>();
                            if (status != PermissionStatus.Granted)
                            {
                                Console.WriteLine("Отклонено разрешение на использование Камеры");
                                await Application.Current.MainPage.DisplayAlert("Отказано в разрешении", "Отклонено разрешение на использование Камеры", "OK");
                                await Application.Current.MainPage.DisplayAlert("Включите разрешение", "Дайте доступ к камере", "OK");
                                return;
                            }
                        }
                        var status2 = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
                        if (status2 != PermissionStatus.Granted)
                        {
                            status2 = await Permissions.RequestAsync<Permissions.StorageWrite>();
                            if (status2 != PermissionStatus.Granted)
                            {
                                Console.WriteLine("Отклонено Write External Storage");
                                await Application.Current.MainPage.DisplayAlert("Отказано в разрешении", "Отклонено разрешение на Хранилище", "OK");
                                await Application.Current.MainPage.DisplayAlert("Включите разрешение", "Дайте доступ к Хранилище", "OK");
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
                else
                {
                    string result1 = "моб - Ошибка входа";
                    string result = await service.LoginLog(UserName, "", "", uniq, result1);
                    await Application.Current.MainPage.DisplayAlert("Ошибка входа", "Пользователь не найден или неверный пароль.\n Проверьте СТАТУС аккаунта, написав в компанию Салым Финанс.", "OK");
                    Console.WriteLine("Пользователь не найден или неверный пароль");

                }

            }
            //await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }

        public async Task SaveSessionAsync(string username, string otNom, string fio, string otdel)
        {
            await SecureStorage.SetAsync("username", username);
            await SecureStorage.SetAsync("otNom", otNom);
            await SecureStorage.SetAsync("fio", fio);
            await SecureStorage.SetAsync("otdel", otdel);
            await SecureStorage.SetAsync("Latitude", "");
            await SecureStorage.SetAsync("Longitude", "");
            //await SecureStorage.SetAsync("pin", pin);
        }

        //public async Task<(string Username, string Token, string Pin)> GetSessionAsync()
        //{
        //    var username = await SecureStorage.GetAsync("username");
        //    var token = await SecureStorage.GetAsync("token");
        //    var pin = await SecureStorage.GetAsync("pin");
        //    return (username, token, pin);
        //}

    }
}
