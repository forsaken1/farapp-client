using System;

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
        
        protected override void OnCreate(Bundle bundle)
        {
            const string id = "amazing-badge-534";
            const string project_number = "1078115685642";
            
            base.OnCreate(bundle);

            GcmClient.CheckDevice(this);
            GcmClient.CheckManifest(this);

            SetContentView(Resource.Layout.Main);
            
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };
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

