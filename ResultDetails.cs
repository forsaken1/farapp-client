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
        PostInfo postInfo;
        ProgressBar progress;
        public ResultDetails(Result result)
        {
            this.result = result;
            images = new List<string>();
        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            if (postInfo == null)
            {
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                    {
                       var info = Activity1.Client.GetPostInfo(result.Link);
                       this.Activity.RunOnUiThread(() =>
                           {
                               postInfo = info;
                               RefreshPostInfo();
                           });
                    });
            }
            base.OnCreate(savedInstanceState);
        }

        void RefreshPostInfo()
        {            
            StartImageService();
            if (View != null)
            {
                var allContacts = postInfo.PhoneNumbers;
                allContacts.AddRange(postInfo.EMails);
                View.FindViewById<TextView>(Resource.Id.details_contacts).Text = 
                    String.Join(System.Environment.NewLine,allContacts);
            }
        }

        void StartImageService()
        {
            var intent = new Intent(this.Activity, typeof(ImageService));
            var pi = this.Activity.CreatePendingResult(101, new Intent(), PendingIntentFlags.UpdateCurrent);
            intent.PutExtra("pi", pi);
            intent.PutStringArrayListExtra("urls", postInfo.ImageUrls);
            this.Activity.StartService(intent); 
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.ResultDetails, null);
            progress = view.FindViewById<ProgressBar>(Resource.Id.details_progress);
            switcher = view.FindViewById<ImageSwitcher>(Resource.Id.details_switcher);

            switcher.SetFactory(this);
            switcher.SetAnimateFirstView(false);
            switcher.SetOutAnimation(this.Activity, Android.Resource.Animation.SlideOutRight);
            switcher.SetInAnimation(this.Activity, Android.Resource.Animation.SlideInLeft);
            if (images.Count != 0)
            {
                progress.Visibility = ViewStates.Invisible;
                switcher.SetImageURI(Android.Net.Uri.Parse(images[0]));
            }
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
            var link = view.FindViewById<TextView>(Resource.Id.details_link);
            title.Text = result.Title;
            price.SetText(Android.Text.Html.FromHtml(result.Price),TextView.BufferType.Spannable);
            parameters.Text = result.Details;
            description.Text = result.Text;
            link.Text = "http://vladivostok.farpost.ru/" + result.Link + ".html";
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

        public override void OnActivityResult(int requestCode, Android.App.Result resultCode, Intent data)
        {
            if (requestCode == 101 && resultCode == Android.App.Result.Ok)
            {
                if (images.Count == 0 && IsVisible)
                {
                    progress.Visibility = ViewStates.Invisible;
                    switcher.SetImageURI(Android.Net.Uri.Parse(data.GetStringExtra("imagePath")));
                }
                images.Add(data.GetStringExtra("imagePath"));                
            }
            base.OnActivityResult(requestCode, resultCode, data);
        }
    }
}