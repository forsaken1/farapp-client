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
    public class Settings : DialogFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
        }
        ListView listView;
        string[] categories;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Settings, null);
            listView = view.FindViewById<ListView>(Resource.Id.settings_list);
            categories = Resources.GetStringArray(Resource.Array.Categories);
            listView.Adapter = new ArrayAdapter<string>(this.Activity, Android.Resource.Layout.SimpleListItemMultipleChoice, categories);
            listView.ChoiceMode = ChoiceMode.Multiple;
                        
            var selectedCategories = GetSelectedCategories();
            for (int i = 0; i < categories.Length;i++)
            {
                if (selectedCategories.Contains(categories[i]))
                {
                    listView.SetItemChecked(i, true);
                }
            }
            var buttonOk = view.FindViewById<Button>(Resource.Id.settings_ok);
            buttonOk.Click += buttonOk_Click;
            return view;
        }

        void buttonOk_Click(object sender, EventArgs e)
        {
            var preferences = this.Activity.GetSharedPreferences("prefs", FileCreationMode.Private);
            var editor = preferences.Edit();
            editor.Remove("categories");
            var selectedCategories = new List<string>();
            var indexes = new List<int>();
            for (int i = 0; i < listView.Count; i++)
            {
                if (listView.IsItemChecked(i))
                {
                    selectedCategories.Add(categories[i]);
                    indexes.Add(i + 1);
                }
            }
            editor.PutStringSet("categories", selectedCategories);
            editor.Commit();
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                Activity1.Client.Register(indexes.ToArray());
            });
            this.Dismiss();
        }

        string[] GetSelectedCategories()
        {
            var preferences = this.Activity.GetSharedPreferences("prefs", FileCreationMode.Private);
            var selectedCategories = preferences.GetStringSet("categories", new List<string>());
            return selectedCategories.ToArray();
        }    
    }
}