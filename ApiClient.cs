using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;

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
            deviceUID = UUID; 
        }
        public List<Result> GetNewAds()
        {
            return new List<Result>
            {
                new Result
                {
                    Title = "title",
                    Price = 12000,
                    Details = " long sdfsldk lrnejgnlsngsdn ,n sn  devices"
                },
                new Result
                {
                    Title = "title2",
                    Price = 16700,
                    Details = " short dn ,n sn  devices"
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
