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
            adapter = new ResultsAdapter(this.Activity);
            var results = new ApiClient("12313123").GetNewAds();
            ListAdapter = adapter;
            adapter.AddItems(results);
            base.OnViewCreated(view, savedInstanceState);
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