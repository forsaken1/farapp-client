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
    public class Result
    {
        public string Title { get; set; }        
        public string Key { get; set; }
        public string Link { get; set; }
        public List<string> Photos { get; set; }
        public string Category { get; set; }
        public Dictionary<string,string> Parameters { get; set; }
        public string Text { get; set; }
        public string MainPhotoUrl { get; set; }
        public string Details { get; set; }
        public string Price { get; set; }
        public string MainImagePath { get; set; }
    }
}