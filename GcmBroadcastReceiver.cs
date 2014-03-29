using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.App;

using Gcm.Client;


namespace FarApp
{
    [BroadcastReceiver(Permission = Constants.PERMISSION_GCM_INTENTS)]
    [IntentFilter(new string[] { Constants.INTENT_FROM_GCM_MESSAGE },
        Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK },
        Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Constants.INTENT_FROM_GCM_LIBRARY_RETRY },
        Categories = new string[] { "@PACKAGE_NAME@" })]
    public class GcmBroadcastReceiver : GcmBroadcastReceiverBase<GcmService>
    {
        
        //IMPORTANT: Change this to your own Sender ID!
        //The SENDER_ID is your Google API Console App Project Number
        public static string[] SENDER_IDS = new string[] { "617404061855" };
    }
}
