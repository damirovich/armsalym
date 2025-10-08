using Android.App; 
using Android.Content; 
using Android.Provider;
using AndroidX.Core.App; 
using AndroidX.Core.Content;
using System.Threading.Tasks;
using System;

namespace ARMzalogApp.Platforms.Android
{
    public static class NotificationPermissionHelper
    {
        public static async Task RequestNotificationPermissionAsync()
        {
            if (OperatingSystem.IsAndroidVersionAtLeast(33)) // API 33 == Android 13 версия
            {
                var status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();

                if (status != PermissionStatus.Granted) 
                {
                    status = await Permissions.RequestAsync<Permissions.PostNotifications>(); // не проверен !!!

                    if (status != PermissionStatus.Granted)
                    {
                        Console.WriteLine("Отклонено разрешение на отправку уведомлений");
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Разрешение на отправку уведомлений уже предоставлено");
                }
            }
        }
    }

}
