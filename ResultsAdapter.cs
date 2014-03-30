using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace FarApp
{
    public class ResultsAdapter : BaseAdapter<Result>
    {
        List<Result> items;
        Activity context;
        ApiClient client;
        string directoryPath;
        public ResultsAdapter(Activity context,string directoryPath)
        {
            this.directoryPath = directoryPath;
            client = new ApiClient("");
            items = new List<Result>();
            this.context = context; 
        }
        public void AddItems(IEnumerable<Result> newItems)
        {
            items.AddRange(newItems);
            Task.Factory.StartNew(() =>
                {
                    foreach (var item in newItems.Where(i=>i.MainPhotoUrl != null && i.MainPhotoUrl != ""))
                    {
                        var path = client.DownloadImage(directoryPath, item.MainPhotoUrl);
                        item.MainImagePath = path;
                        context.RunOnUiThread(() => this.NotifyDataSetChanged());
                    }
                });
            this.NotifyDataSetChanged();
        }
        public void Clear()
        {
            items.Clear();
            this.NotifyDataSetChanged();
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override int Count
        {
            get { return items.Count; }
        }

        public override Result this[int position]
        {
            get { return items[position]; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.ResultItem, null); 
            }
            var title = view.FindViewById<TextView>(Resource.Id.resultItem_title);
            var details = view.FindViewById<TextView>(Resource.Id.resultItem_details);
            var price = view.FindViewById<TextView>(Resource.Id.resultItem_price);
            var imageView = view.FindViewById<ImageView>(Resource.Id.resultItem_image);
            title.Text = items[position].Title;
            details.Text = items[position].Details;
            price.SetText(Android.Text.Html.FromHtml(items[position].Price.ToString()),TextView.BufferType.Spannable);
            if (items[position].MainImagePath == null)
            {
                imageView.SetImageResource(Resource.Drawable.no_image_placeholder);
            }
            else
            {
                imageView.SetImageURI(Android.Net.Uri.Parse(items[position].MainImagePath));
            }
            return view;
        }
    }
}