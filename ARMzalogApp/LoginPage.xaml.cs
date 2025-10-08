using ARMzalogApp.ViewModels;
using ARMzalogApp.Views;
using FirebaseAdmin.Messaging;
using CommunityToolkit.Mvvm.Messaging;
namespace ARMzalogApp;
using ARMzalogApp.Models;
using Newtonsoft.Json;
using System.Text;
public partial class LoginPage : ContentPage
{
    private static System.Timers.Timer timer;
    private bool _isPasswordVisible = false;
    private string _deviceToken;
    public LoginPage(LoginPageViewModel _viewModel)
    {
        InitializeComponent();
        this.BindingContext = _viewModel;
        //passwordEntry.IsPassword = true;

        UpdatePasswordVisibility();
        if (Preferences.ContainsKey("DeviceToken"))
        {
            _deviceToken = Preferences.Get("DeviceToken", "");
        }
    }
    private void OnShowPassword(object sender, EventArgs e)
    {
        _isPasswordVisible = !_isPasswordVisible;
        UpdatePasswordVisibility();
    }

    private void UpdatePasswordVisibility()
    {
        if (_isPasswordVisible)
        {
            passwordEntry.IsPassword = false;
            showPasswordIcon.Source = "hide_icon.png";
        }
        else
        {
            passwordEntry.IsPassword = true;
            showPasswordIcon.Source = "show_icon.png";
        }
    }

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        //var androidNotificationObject = new Dictionary<string, string>();
        //androidNotificationObject.Add("NavigationID", "2");

        //var iosNotificationObject = new Dictionary<string, object>();
        //iosNotificationObject.Add("NavigationID", "2");

        var pushNotificationRequest = new PushNotificationRequest
        {
            notification = new NotificationMessageBody
            {
                title = "Notification Title",
                body = "Notification body"
            },
            //data = androidNotificationObject,
            registration_ids = new List<string> { _deviceToken }
        };

        var messageList = new List<Message>();

        var obj = new Message
        {
            Token = _deviceToken,
            Notification = new Notification
            {
                Title = "Tilte",
                Body = "message body"
            },
            //Data = androidNotificationObject,
            //Apns = new ApnsConfig()
            //{
            //    Aps = new Aps
            //    {
            //        Badge = 15,
            //        CustomData = iosNotificationObject,
            //    }
            //}
        };

        //messageList.Add(obj);

        //var response = await FirebaseMessaging.DefaultInstance.SendAllAsync(messageList);

        string url = "https://fcm.googleapis.com/fcm/send";

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("key","="+ "BPqhy9YSr9nZNop01XrjCDfhhyWJJFwX5Y5x4QZHPr3ROQt3ctONW_VFu4BFT8E87JEVgsl5PYJtHb9CsaMCaoQ");

            string serializeRequest = JsonConvert.SerializeObject(pushNotificationRequest);
            var response = await client.PostAsync(url, new StringContent(serializeRequest, Encoding.UTF8, "application/json"));
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                await DisplayAlert("Notification sent", "notification sent", "OK");
            }
        }
    }

    private void OnButtonRegisterClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync(nameof(RegistPage));
    }
}