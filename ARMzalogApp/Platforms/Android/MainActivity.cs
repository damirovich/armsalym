using Android.App;
using Android.Content.PM;
using Android.OS;
using Firebase.Messaging;
using Firebase.Iid;
using Android.Util;
using Firebase;
using Android.Gms.Tasks;
using ARMzalogApp.Models;
using ARMzalogApp.Platforms.Android;
using Android.Content;
using Android.Views.InputMethods;
using Android.Views;
using Android.Widget;
using Plugin.Fingerprint.Abstractions;
using Plugin.Fingerprint;

namespace ARMzalogApp
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        public static MainActivity Instance { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            CrossFingerprint.SetCurrentActivityResolver(() => this);
            Instance = this;
            //Window.SetFlags(WindowManagerFlags.Secure, WindowManagerFlags.Secure); // предотвращение скриншота

            //---
            NotificationPermissionHelper.RequestNotificationPermissionAsync().ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Console.WriteLine("Ошибка при запросе разрешения на уведомления: " + t.Exception?.Message);
                }
                else if (t.IsCompletedSuccessfully)
                {
                    Console.WriteLine("Запрос разрешения на уведомления завершен");
                }
            });
            //----
            // Initialize Firebase
            FirebaseApp.InitializeApp(this);

            // канал уведомления для Android 8.0 и выше
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel("default_channel", "Default Channel", NotificationImportance.Default)
                {
                    Description = "Default Notification Channel"
                };
                var notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager.CreateNotificationChannel(channel);
            }

            // Obtain the Firebase Cloud Messaging token
            FirebaseMessaging.Instance.GetToken()
                .AddOnCompleteListener(new OnCompleteListener());
        }

        private class OnCompleteListener : Java.Lang.Object, IOnCompleteListener
        {
            public void OnComplete(Android.Gms.Tasks.Task task)
            {
                if (task.IsSuccessful)
                {
                    string token = task.Result.ToString();
                    Log.Debug("FCM Token", token);
                    FCMTokenSingleton.Instance.FCMToken = token;
                }
                else
                {
                    Log.Warn("FCM Token", "Fetching FCM registration token failed", task.Exception);
                }
            }
        }

        public override bool DispatchTouchEvent(MotionEvent? e)
        {
            if (e!.Action == MotionEventActions.Down)
            {
                var focusedElement = CurrentFocus;
                if (focusedElement is EditText editText)
                {
                    var editTextLocation = new int[2];
                    editText.GetLocationOnScreen(editTextLocation);
                    var clearTextButtonWidth = 100; // syncfusion clear button at the end of the control
                    var editTextRect = new Rect(editTextLocation[0], editTextLocation[1], editText.Width + clearTextButtonWidth, editText.Height);
                    //var editTextRect = editText.GetGlobalVisibleRect(editTextRect);  //not working in MAUI, always returns 0,0,0,0
                    var touchPosX = (int)e.RawX;
                    var touchPosY = (int)e.RawY;
                    if (!editTextRect.Contains(touchPosX, touchPosY))
                    {
                        editText.ClearFocus();
                        var inputService = GetSystemService(Context.InputMethodService) as InputMethodManager;
                        inputService?.HideSoftInputFromWindow(editText.WindowToken, 0);
                    }
                }
            }
            return base.DispatchTouchEvent(e);
        }

    }
}
