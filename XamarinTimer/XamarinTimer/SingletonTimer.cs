using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using XamarinTimer.Helper;

namespace XamarinTimer
{
    /// <summary>
    /// https://www.dofactory.com/net/singleton-design-pattern
    /// </summary>
    public class SingletonTimer
    {
        private static readonly SingletonTimer _instance = new SingletonTimer();

        public CancellationTokenSource cancelTokenSource;
        public BackgroundWorker backgroundWorker1 = new BackgroundWorker();
        public UtcTimestamper timestamper = new UtcTimestamper();
        public System.Timers.Timer timer = new System.Timers.Timer();
        public string ResultMsg = string.Empty;
        AutoResetEvent autoEvent = new AutoResetEvent(false);
        private int invokeCount = 0;
        public static SingletonTimer GetSingletonTimer()
        {
            return _instance;
        }

        private SingletonTimer()
        {
            timer.Interval = 4000;
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);

            backgroundWorker1.WorkerSupportsCancellation = true;

            backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);

            timer.Start(); //타이머 시작

            //var stateTimer = new System.Threading.Timer(StatusChecker, null, 5000, 5000);
        }

        //private void StatusChecker(object state)
        //{
        //    Console.WriteLine("{0} Checking status {1,2}.", DateTime.Now.ToString("h:mm:ss.fff"), (++invokeCount).ToString());
            
        //    if (invokeCount == 5)
        //    {
        //        // Reset the counter and signal the waiting thread.
        //        invokeCount = 0;
        //    }
        //}
              

        public void StopTimer()
        {
            timer.Stop();
            backgroundWorker1.CancelAsync();
            cancelTokenSource.Cancel();
        }

        public void StartTimer()
        {
            timer.Start(); //타이머 시작
            //timestamper.Restart();
        }

        private async Task<string> WorkThread()
        {
            Debug.WriteLine(System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " WorkThread");

            cancelTokenSource = new CancellationTokenSource();
            cancelTokenSource.CancelAfter(7000); //7초 이상 작업시간이 걸리면 취소 한다.

            //task.Result를 하면 ConfigureAwait(false)를 해줘야 dead lock을 피할 수 있다.
            var task1 = Task.Factory.StartNew<string>(LongWorkerAsync, cancelTokenSource.Token).ConfigureAwait(false);
            var result = await task1;

            Console.WriteLine("WorkThread End");

            return result;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Debug.WriteLine(System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " backgroundWorker1_RunWorkerCompleted");
            if (e.Cancelled)
            {
                ResultMsg = "작업 취소됨";
            }
            else if (e.Error != null)
            {
                // 에러 발생시 메시지 표시
                ResultMsg = e.Error.Message.ToString();
            }
            else
            {
                if (e.Result != null)
                {
                    ResultMsg = e.Result.ToString();
                }
            }
        }

        //private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Console.WriteLine(System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " backgroundWorker1_DoWork");

            // 취소 버튼 클릭 했을 경우
            if (backgroundWorker1.CancellationPending)
            {
                e.Cancel = true; //작업취소
                return;
            }

            WorkThread().ContinueWith(task =>
            {
                Console.WriteLine(task.Result);
                e.Result = task.Result;
            }).Wait();

            Console.WriteLine("backgroundWorker1_DoWork End");
        }

        public void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Debug.WriteLine(System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " timer_Elapsed");

            //1초마다 타이머가 실행되고 있는거 확인
            Console.WriteLine("==== Timer Working {0} ", e.SignalTime.ToString());

            //백그라운드 워커 실행 중이냐고 물어 봄
            //타이머를 1초 단위로 해도 backgroundWorker1가 실행 중이면 skip함.

            if (backgroundWorker1.IsBusy)
            {
                Console.WriteLine("==== backgroundWorker1.IsBusy");
            }
            else
            {
                //작업중이지 않으면 backgroundwork  실행 시킴
                backgroundWorker1.RunWorkerAsync();
            }

            this.timer.Stop();
        }

        private string LongWorkerAsync()
        {
            Debug.WriteLine(System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " LongWorkerAsync");
            Task.Delay(7001).Wait();
            if (cancelTokenSource.Token.IsCancellationRequested)
            {
                return "work time over~!";
            }
            else
            {
                return timestamper.GetFormattedTimestamp();
            }
        }
    }
}
