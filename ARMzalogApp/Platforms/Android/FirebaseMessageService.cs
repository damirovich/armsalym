using Android.App;
using Android.Content;
using Android.Util;
using Firebase.Messaging;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Android.OS;

namespace ARMzalogApp
{
    [Service(Exported = true)]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class FirebaseMessageService : FirebaseMessagingService
    {
        const string TAG = "FirebaseMsgService";
        const string CHANNEL_ID = "default_channel";
        public override void OnMessageReceived(RemoteMessage message)
        {
            Log.Debug(TAG, "From: " + message.From);
            Log.Debug(TAG, "Notification Message Body: " + message.GetNotification().Body);
            //////SendNotification(message.GetNotification().Body); //старая рабочая версия 

   //-----
            //base.OnMessageReceived(message);
            //// Check if message contains a notification payload.
            //if (message.GetNotification() != null)
            //{
            //    Log.Debug(TAG, "Message Notification Body: " + message.GetNotification().Body);
            //    SendNotification(message.GetNotification().Body);
            //}
            //// Check if message contains a data payload.
            //else if (message.Data.Count > 0)
            //{
            //    Log.Debug(TAG, "Message data payload: " + message.Data["body"]);
            //    SendNotification(message.Data["body"]);
            //}
    //-----
            CreateNotificationChannel();
            if (message.GetNotification() != null)// проверить 18/06/24
            {
                SendNotification(message.GetNotification().Body);
            }
        }
        void CreateNotificationChannel() // проверить 18/06/24
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel(CHANNEL_ID, "Default Channel", NotificationImportance.Default)
                {
                    Description = "Default Notification Channel"
                };

                var notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager.CreateNotificationChannel(channel);
            }
        }
        void SendNotification(string messageBody)
        {
            var notificationManager = NotificationManagerCompat.From(this);
            var notificationBuilder = new NotificationCompat.Builder(this, "default_channel")
                .SetContentTitle("ARM Залог")
                .SetContentText(messageBody)
                .SetAutoCancel(true)
                .SetContentIntent(PendingIntent.GetActivity(this, 0, new Intent(this, typeof(MainActivity)), PendingIntentFlags.OneShot));

            notificationManager.Notify(0, notificationBuilder.Build());
        }
    }
}
