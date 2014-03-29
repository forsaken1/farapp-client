using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace FarApp
{
    public class JsonParser
    {
        public static List<Result> ParseResults(string data)
        {
            var results = new List<Result>();
            var jData = JArray.Parse(data);
            foreach (var jResult in jData)
            {
                var res = JsonConvert.DeserializeObject<Result>(jResult.ToString());
                results.Add(res);
            }
            return results;
        }
    }
}
