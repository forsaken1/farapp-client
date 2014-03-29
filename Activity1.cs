using System;
using System.IO;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using Gcm.Client;

namespace FarApp
{
    [Activity(Label = "FarApp", MainLauncher = true, Icon = "@drawable/icon",
        ConfigurationChanges = ConfigChanges.KeyboardHidden|ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class Activity1 : Activity
    {
        int count = 1;

        void Init()
        {
            var imagesPath = Path.Combine(FilesDir.AbsolutePath,"Images");
            if (!Directory.Exists(imagesPath))
            {
                Directory.CreateDirectory(imagesPath);
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Init();
            GcmClient.CheckDevice(this);
            GcmClient.CheckManifest(this);
            GcmClient.Register(this, GcmBroadcastReceiver.SENDER_IDS);
            
            if (GcmClient.IsRegisteredOnServer(this))
            {
                Console.WriteLine("Registered ID: " + GcmClient.GetRegistrationId(this));
            }
            SetContentView(Resource.Layout.Main);
            this.FragmentManager.BeginTransaction().Replace(Resource.Id.Main_Layout, new ListResults()).Commit();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MainMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.settingsMenu)
            {
                this.FragmentManager.BeginTransaction()
                    .Replace(Resource.Id.Main_Layout, new Settings())
                    .AddToBackStack("settings")
                    .Commit();
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}

