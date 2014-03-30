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
    [Activity(Label = "FarApp", MainLauncher = true, Icon = "@drawable/orange_arrow",
        ConfigurationChanges = ConfigChanges.KeyboardHidden|ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class Activity1 : Activity
    {
        void Init()
        {
            var imagesPath = Path.Combine(FilesDir.AbsolutePath,"Images");
            if (!Directory.Exists(imagesPath))
            {
                Directory.CreateDirectory(imagesPath);
            }
            GcmClient.CheckDevice(this);
            GcmClient.CheckManifest(this);
            var prefs = GetSharedPreferences("registration", FileCreationMode.Private);
            if (prefs.Contains("id"))
            {
                apiClient.RegisterID = prefs.GetString("id", "");
            }
            if (!GcmClient.IsRegisteredOnServer(this) || GcmClient.IsRegistered(this))
            {
                GcmClient.Register(this, GcmBroadcastReceiver.SENDER_IDS);
                GcmClient.SetRegisteredOnServer(this, true);
            }            
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Init();           
            
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
        protected override void OnActivityResult(int requestCode, Android.App.Result resultCode, Intent data)
        {
            if (resultCode == Android.App.Result.Ok && requestCode == 101)
            {
                var f = FragmentManager.FindFragmentByTag("details");
                if (f != null)
                { 
                    f.OnActivityResult(requestCode, resultCode, data);
                }
            }
            base.OnActivityResult(requestCode, resultCode, data);
        }
        static ApiClient apiClient = new ApiClient();
        public static ApiClient Client
        {
            get
            {
                return apiClient;
            }
        }
    }
}

