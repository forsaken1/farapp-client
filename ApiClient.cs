﻿using System;
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
        Dictionary<string, List<string>> fields = new Dictionary<string, List<string>>();
        HttpClient client;
        public ApiClient(string registerID = "")
        {
            client = new HttpClient();
            client.Timeout = new TimeSpan(0, 0, 30);
            InitFields();
            this.registerID = registerID;
        }

        void InitFields()
        {
            fields.Add("1", new List<string> 
            { 
                "price","model",
"year",
"displacement",
"transmission",
"drive",
"fuel",
"hasDocuments",
"hasRussianMileage",
"isAfterCrash",
"condition",
"guarantee",
"description",
"author",
            });
            fields.Add("2", new List<string> 
            {
                "subject" ,
"price",
"district",
"street",
"flatType",
"area",
"text" ,
            });
            fields.Add("3", new List<string>
                {
                    "payment",
"paymentform",
"firm",
"branch",
"vacancy",
"employment",
"obligation",
"description",
"education",
"experience",
"author",
                });
            fields.Add("4", new List<string>
                {
                    "subject",
"text",
                });
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

        public Tuple<List<Result>,string> GetNewAds(string time)
        {           
            var requestContent = new StringContent(GetJsonRegisterID(time),Encoding.UTF8,"application/json");
            var sss = GetJsonRegisterID(time);
            var responce = client.PostAsync(MAIN_URL + "/get",requestContent).Result;
            if (responce.IsSuccessStatusCode)
            {
                var content = responce.Content.ReadAsStringAsync().Result;
                return BuildResults(content);
            }
            else
            {
                var error = responce.Content.ReadAsStringAsync().Result;
                return new Tuple<List<Result>,string>(new List<Result>(),"2014-03-29 00:00:00");
            }
        }

        public bool Register(int[] categoryIDs)
        {
            if (String.IsNullOrEmpty(registerID))
                return false;
            var jContent = new JObject();
            jContent.Add("register_id",registerID);
            jContent.Add("category",new JArray(categoryIDs));
            var content = new StringContent(jContent.ToString(Newtonsoft.Json.Formatting.None),Encoding.UTF8,"application/json");            
            var json = jContent.ToString(Newtonsoft.Json.Formatting.None);
            var responce = client.PostAsync(MAIN_URL + "/register",content).Result;
            var error = responce.Content.ReadAsStringAsync().Result;
            
            return responce.IsSuccessStatusCode;
        }

        Tuple<List<Result>,string> BuildResults(string json)
        {
            var results = new List<Result>();
            var jData = JObject.Parse(json);            
                var items = jData["items"] as JArray;
                foreach (var item in items)
                {
                    var result = new Result();
                    result.Parameters = new Dictionary<string, string>();
                    var category = item["category_id"].ToString();
                    foreach (var field in fields[category])
                    {
                        try
                        {
                            result.Parameters.Add(field, item[field].ToString());
                        }
                        catch (Exception)
                        { }
                    }
                    result.Key = item["key"].ToString();
                    result.Link = item["link"].ToString();
                    result.Details = item["annotation"].ToString();
                    result.Price = item["price"].ToString();
                    result.Title = item["subject"].ToString();
                    result.MainPhotoUrl = item["img"].ToString();
                    results.Add(result);
                }
                return new Tuple<List<Result>, string>(results, jData["time"].ToString());
            }           
        
        string GetJsonRegisterID(string time)
        {
            var jRegisterId = new JObject();
            jRegisterId.Add(new JProperty("register_id", registerID));
            jRegisterId.Add(new JProperty("time", time));
            return jRegisterId.ToString();
        }

        public PostInfo GetPostInfo(string postLink)
        {            
            const string URL = "http://farapp.herokuapp.com/getPostInfo?";
            var result = client.GetAsync(URL + "post=" + postLink).Result;
            if (result.IsSuccessStatusCode)
            {
                var info = new PostInfo();
                var jInfo = JObject.Parse(result.Content.ReadAsStringAsync().Result);
                var jImages = jInfo["images"] as JArray;
                foreach (var j in jImages)
                {
                    info.ImageUrls.Add(j.ToString()); 
                }
                var jPhones = jInfo["phones"] as JArray;
                foreach (var j in jPhones)
                {
                    info.PhoneNumbers.Add(j.ToString());
                }
                var jMails = jInfo["emails"] as JArray;
                foreach (var j in jMails)
                {
                    info.EMails.Add(j.ToString());
                }
                return info;
            }
            else
            {
                return new PostInfo();
            }
        }
    }
    public class PostInfo
    {
        public PostInfo()
        {
            PhoneNumbers = new List<string>();
            EMails = new List<string>();
            ImageUrls = new List<string>();
        }
        public List<string> PhoneNumbers { get; set; }
        public List<string> EMails { get; set; }
        public List<string> ImageUrls { get; set; }
    }
}
