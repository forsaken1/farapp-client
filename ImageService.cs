using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace FarApp
{
    [Service]
    public class ImageService : Service
    {
        ApiClient apiClient;
        string directoryPath;
        public override void OnCreate()
        {
            apiClient = new ApiClient("");
            directoryPath = System.IO.Path.Combine(FilesDir.AbsolutePath, "Images");
            base.OnCreate();
        }
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            var pi = intent.GetParcelableExtra("pi") as PendingIntent;
            var urls = intent.GetStringArrayListExtra("urls");
            Task.Factory.StartNew(() =>
                {
                    var imagePaths = new List<string>(urls.Count);
                    foreach (var url in urls)
                    {
                        imagePaths.Add(apiClient.DownloadImage(directoryPath, url));
                        pi.Send(this, Android.App.Result.Ok, new Intent().PutExtra("imagePath", imagePaths.Last()));
                    }
                });
            return StartCommandResult.NotSticky;
        }
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
    }
}
