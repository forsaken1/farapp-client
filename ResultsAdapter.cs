using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        
        public ResultsAdapter(Activity context)
        {            
            items = new List<Result>();
            this.context = context; 
        }
        public void AddItems(IEnumerable<Result> newItems)
        {
            items.AddRange(newItems);
            
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
            price.Text = items[position].Price.ToString() + " �.";
            if (items[position].MainPhotoUrl == null)
            {
                imageView.SetImageResource(Resource.Drawable.no_image_placeholder);
            }
            return view;
        }
    }
}