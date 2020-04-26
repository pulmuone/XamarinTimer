using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using XamarinTimer.ViewModels;

namespace XamarinTimer
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            //(BindingContext as MainPageViewModel).timer.Elapsed -= new ElapsedEventHandler((BindingContext as MainPageViewModel).timer_Elapsed);
        }
    }
}
