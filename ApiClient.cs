using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json.Linq;

namespace FarApp
{
    public class ApiClient
    {
        string deviceUID;
        HttpClient client;
        public ApiClient(string UUID)
        {
            client = new HttpClient();
            client.Timeout = new TimeSpan(0, 0, 30);
            deviceUID = UUID; 
        }

        public string DownloadImage(string directoryPath, string url)
        {            
            var imagePath = Path.Combine(directoryPath, "image" + Math.Abs(url.GetHashCode()).ToString() + ".png");            
            if (!File.Exists(imagePath))
            {
                try                
                {
                    var responce = client.GetAsync(url).Result;
                    if (responce.IsSuccessStatusCode)
                    {
                        var byteImage = responce.Content.ReadAsByteArrayAsync().Result;
                        File.WriteAllBytes(imagePath, byteImage);
                    }
                    else
                    {
                        Console.WriteLine("error");
                    }
                }
                catch (Exception e)
                {
                    var a = e.StackTrace;
           //         Console.WriteLine(e.StackTrace);
                }
                return imagePath;
            }
            else
            {
                return imagePath;
            }            
        }

        public List<Result> GetNewAds()
        {
            return new List<Result>
            {
                new Result
                {
                    Title = "title",
                    Price = 12000,
                    Details = " long sdfsldk lrnejgnlsngsdn ,n sn  devices",
                    MainPhotoUrl = "http://stat20.privet.ru/lr/0d14ce807f3d2da00814601cd8fec104"
                },
                new Result
                {
                    Title = "title2",
                    Price = 16700,
                    Details = " short dn ,n sn  devices",
                    MainPhotoUrl = "http://stat20.privet.ru/lr/0d14ce807f3d2da00814601cd8fec104"
                },
            };

            const string URL = "";
            var responce = client.GetAsync(URL).Result;
            if (responce.IsSuccessStatusCode)
            {
                var content = responce.Content.ReadAsStringAsync().Result;
                return BuildResults(content);
            }
            else
            {
                return new List<Result>();
            }
        }
        List<Result> BuildResults(string json)
        {
            var results = new List<Result>();
            //TODO: Parsing
            return results;
        }
    }
}
