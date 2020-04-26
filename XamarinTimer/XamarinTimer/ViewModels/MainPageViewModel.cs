using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinTimer.Helper;

namespace XamarinTimer.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private string _resultMsg = string.Empty;

        public ICommand StartTimerCommand { get; }
        public ICommand StopTimerCommand { get; }

        public ICommand GetTimerCommand { get; }

        private SingletonTimer singletonTimer;

        public MainPageViewModel()
        {
            Debug.WriteLine(System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " MainPageViewModel");
            StartTimerCommand = new Command(StartTimer);
            StopTimerCommand = new Command(StopTimer);
            GetTimerCommand = new Command(GetTimer);

            singletonTimer = SingletonTimer.GetSingletonTimer();

        }

        private void GetTimer(object obj)
        {
            ResultMsg = singletonTimer.ResultMsg;
        }

        private void StopTimer(object obj)
        {
            singletonTimer.StopTimer();
            //timer.Stop();
            //backgroundWorker1.CancelAsync();
            //cancelTokenSource.Cancel();
        }

        private void StartTimer(object obj)
        {
            singletonTimer.StartTimer();
            //timer.Start(); //타이머 시작
            //timestamper.Restart();
        }

    
        public string ResultMsg { get => _resultMsg; set => SetProperty(ref _resultMsg, value); }
    }
}
