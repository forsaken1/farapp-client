using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics.Drawables;

namespace FarApp
{
    public class ListResults : ListFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);                        
        }
        ResultsAdapter adapter;
        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            adapter = new ResultsAdapter(this.Activity,System.IO.Path.Combine(this.Activity.FilesDir.AbsolutePath,"Images"));
            var results = Activity1.Client.GetNewAds(GetTime());
            ListAdapter = adapter;
            
            ListView.ItemClick += ListView_ItemSelected;
            ListView.Divider = new ColorDrawable(Android.Graphics.Color.Orange);
            ListView.DividerHeight = 1;
            adapter.AddItems(results.Item1);
            SetTime(results.Item2);
            base.OnViewCreated(view, savedInstanceState);
        }

        string GetTime()
        {
            var prefs = Activity.GetSharedPreferences("prefs", FileCreationMode.Private);
            return prefs.GetString("time", "2014-03-29 10:00:00"); 
        }
        void SetTime(string time)
        {
            var edit = Activity.GetSharedPreferences("prefs", FileCreationMode.Private).Edit();
            edit.PutString("time", time);
            edit.Commit();
        }

        void ListView_ItemSelected(object sender, AdapterView.ItemClickEventArgs e)
        {            
            var detailsFragment = new ResultDetails(adapter[e.Position]);
            this.FragmentManager.BeginTransaction().
                Replace(Resource.Id.Main_Layout, detailsFragment,"details")
                .AddToBackStack("details")
                .Commit();
        }

        public override void OnActivityResult(int requestCode, Android.App.Result resultCode, Intent data)
        {
            if (requestCode == 100 && resultCode == Android.App.Result.Ok)
            {
                var stringResults = data.GetStringExtra("data");
                if (stringResults != null)
                {
                    var results = JsonParser.ParseResults(stringResults);
                    adapter.AddItems(results);
                }
            }
            base.OnActivityResult(requestCode, resultCode, data);
        }
    }
}