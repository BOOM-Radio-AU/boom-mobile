using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoomRadio.Droid
{
    /// <summary>
    /// Activity for the splash screen shown when the app opens
    /// </summary>
    [Activity(Label = "BOOM Radio", Theme = "@style/SplashTheme", Icon = "@mipmap/ic_launcher", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.StartActivity(typeof(MainActivity));

        }
    }
}