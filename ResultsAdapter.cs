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
            items.Concat(newItems);
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
            return view;
        }
    }
}