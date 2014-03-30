using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Gcm.Client;


namespace FarApp
{
    [Service]
    public class GcmService : GcmServiceBase
    {
        public GcmService() : base(GcmBroadcastReceiver.SENDER_IDS) { }

        protected override void OnRegistered(Context context, string registrationId)
        {
            Console.WriteLine(" i am registered!");
            Activity1.Client.RegisterID = registrationId;
            var preferences = GetSharedPreferences("registration", FileCreationMode.Private);
            if (preferences.Contains("id"))
            {

            }
            else
            {
                var editor = preferences.Edit();
                editor.PutString("id", registrationId);
                editor.Commit();
            }
        }

        protected override void OnUnRegistered(Context context, string registrationId)
        {
            //Receive notice that the app no longer wants notifications
        }

        protected override void OnMessage(Context context, Intent intent)
        {
            if (intent != null && intent.Extras != null)
            {
                CreateNotification(1,intent.Extras.GetString("message"));
            }
        }

        protected override bool OnRecoverableError(Context context, string errorId)
        {
            return true;
        }

        protected override void OnError(Context context, string errorId)
        {
            Console.WriteLine(errorId);
        }
        void CreateNotification(int resultsCount,string message)
        {
            var notManager = GetSystemService(Context.NotificationService) as NotificationManager;
            var uiIntent = new Intent(this, typeof(Activity1));
            uiIntent.PutExtra("need", true);
            var notification = new Notification(Android.Resource.Drawable.SymActionEmail, "Новое объявление");
            notification.Flags = NotificationFlags.AutoCancel;
            notification.Defaults = NotificationDefaults.Vibrate;
            notification.SetLatestEventInfo(this,"Новые объявления : "+resultsCount,message,PendingIntent.GetActivity(this,0,uiIntent,PendingIntentFlags.CancelCurrent));
            notManager.Notify(1, notification);
        }
    }
}
