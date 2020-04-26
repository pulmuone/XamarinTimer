using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinTimer.Helper;

namespace XamarinTimer
{
    public partial class App : Application
    {


        public App()
        {
            InitializeComponent();
            SingletonTimer.GetSingletonTimer();

            MainPage = new MainPage();
        }
        

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
