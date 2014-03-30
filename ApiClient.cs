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
        const string MAIN_URL = "http://farapp.herokuapp.com";
        string registerID;
        public string RegisterID
        {
            get
            {
                return registerID;
            }
            set
            {
                registerID = value;
            }
        }        

        HttpClient client;
        public ApiClient(string registerID = "")
        {
            client = new HttpClient();
            client.Timeout = new TimeSpan(0, 0, 30);
            this.registerID = registerID;
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
                    Console.WriteLine(e.StackTrace);
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
                    MainPhotoUrl = "http://static.baza.farpost.ru/v/1395901688037_bulletin.html",
                    Photos = new List<string>
                    {
                        "http://static.baza.farpost.ru/v/1395901656018_bulletin",
                        "http://static.baza.farpost.ru/v/1395901676380_bulletin",
                        "http://static.baza.farpost.ru/v/1395901688037_bulletin"
                    }
                },
                new Result
                {
                    Title = "title2",
                    Price = 16700,
                    Details = " short dn ,n sn  devices",
                    MainPhotoUrl = "http://stat20.privet.ru/lr/0d14ce807f3d2da00814601cd8fec104"
                },
            };
            var requestContent = new StringContent(GetJsonRegisterID());                     
            var responce = client.PostAsync(MAIN_URL + "/get",requestContent).Result;
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

        public bool Register(int[] categoryIDs)
        {
            var jContent = new JObject();
            jContent.Add("register_id",registerID);
            jContent.Add("category",new JArray(categoryIDs));
            var content = new StringContent(jContent.ToString());
            var responce = client.PostAsync(MAIN_URL + "/register",content).Result;
            return responce.IsSuccessStatusCode;
        }

        List<Result> BuildResults(string json)
        {
            var results = new List<Result>();
            //TODO: Parsing
            return results;
        }
        string GetJsonRegisterID()
        {
            var jRegisterId = new JObject();
            jRegisterId.Add(new JProperty("registed_id", registerID));
            return jRegisterId.ToString();
        }
    }
}
