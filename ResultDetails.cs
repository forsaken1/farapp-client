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
    public class ResultDetails : Fragment,ViewSwitcher.IViewFactory
    {
        ImageSwitcher switcher;
        List<string> images;
        Result result;
        public ResultDetails(Result result)
        {
            this.result = result;
        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);            
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.ResultDetails, null);
            switcher = view.FindViewById<ImageSwitcher>(Resource.Id.details_switcher);
            switcher.SetFactory(this);
            switcher.SetAnimateFirstView(false);
            switcher.SetOutAnimation(this.Activity, Android.Resource.Animation.SlideOutRight);
            switcher.SetInAnimation(this.Activity, Android.Resource.Animation.SlideInLeft);
            var downX = 0.0f;
            switcher.Touch += (object sender, View.TouchEventArgs e) =>
            {
                if (e.Event.Action == MotionEventActions.Down)
                {
                    downX = e.Event.GetX();
                }
                if (e.Event.Action == MotionEventActions.Up)
                {
                    if (downX - e.Event.GetX() > 100)
                    {
                        MoveImageLeft();
                    }
                    if (downX - e.Event.GetX() < -100)
                    {
                        MoveImageRight();
                    }
                }
            };

            var title = view.FindViewById<TextView>(Resource.Id.details_title);
            var price = view.FindViewById<TextView>(Resource.Id.details_price);
            var parameters = view.FindViewById<TextView>(Resource.Id.details_parameters);
            var description  = view.FindViewById<TextView>(Resource.Id.details_description);
            title.Text = result.Title;
            price.Text = result.Price + " ð.";
            parameters.Text = result.Details;
            description.Text = result.Text;
            return view;
        }
        int switcherPosition = 0;
        private void MoveImageLeft()
        {
            var _imagesCopy = images;
            if (_imagesCopy != null && _imagesCopy.Count != 0)
            {
                switcher.SetInAnimation(this.Activity, Resource.Animation.slide_in_right);
                switcher.SetOutAnimation(this.Activity, Resource.Animation.slide_out_left);
                switcherPosition = switcherPosition <= 0 ? _imagesCopy.Count - 1 : --switcherPosition;
                using (var d = BitmapDrawable.CreateFromPath(_imagesCopy[switcherPosition]))
                {
                    switcher.SetImageDrawable(d);
                }
            }
        }

        private void MoveImageRight()
        {
            var _imagesCopy = images;
            if (_imagesCopy != null && _imagesCopy.Count != 0)
            {
                switcher.SetInAnimation(this.Activity, Android.Resource.Animation.SlideInLeft);
                switcher.SetOutAnimation(this.Activity, Android.Resource.Animation.SlideOutRight);

                switcherPosition = switcherPosition >= _imagesCopy.Count - 1 ? 0 : ++switcherPosition;
                using (var d = BitmapDrawable.CreateFromPath(_imagesCopy[switcherPosition]))
                {
                    switcher.SetImageDrawable(d);
                }
            }
        }
        public View MakeView()
        {
            var _image = new ImageView(this.Activity);
            _image.SetScaleType(ImageView.ScaleType.CenterCrop);
            _image.SetAdjustViewBounds(true);
            _image.LayoutParameters = new ImageSwitcher.LayoutParams(ViewGroup.LayoutParams.MatchParent, 
                ViewGroup.LayoutParams.MatchParent);
            return _image;
        }
    }
}