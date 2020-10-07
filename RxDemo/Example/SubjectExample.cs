using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RxDemo.Example
{
    public class SubjectExample : IExample
    {
        public void Start()
        {
            //ConvertTaskToObservable();

            var inttask = Task.Run(async () => { await Task.Delay(1000); return 42; })
                .ToObservable()
                .SubscribeConsole("task<int>");
            var task = Task.Run(async () => await Task.Delay(2000))
                .ToObservable()
                .SubscribeConsole("task");
            var taskEx = Task.Run(() => throw new ApplicationException("someting wrong"))
                .ToObservable()
                .SubscribeConsole();
            Console.Read();
        }
        /// <summary>
        /// 6:
        /// The program output shows that even though the Task completed before the observer subscribed, the observer is notified of the result:
        /// </summary>
        private void ConvertTaskToObservable()
        {
            var tcs = new TaskCompletionSource<bool>();
            var task = tcs.Task;

            AsyncSubject<bool> sbj = new AsyncSubject<bool>();
            task.ContinueWith(t =>
            {
                switch (t.Status)
                {
                    case TaskStatus.RanToCompletion:
                        sbj.OnNext(t.Result);
                        sbj.OnCompleted();
                        break;
                    case TaskStatus.Faulted:
                        sbj.OnError(t.Exception.InnerException);
                        break;
                    case TaskStatus.Canceled:
                        sbj.OnError(new TaskCanceledException(t));
                        break;
                }
            }, TaskContinuationOptions.ExecuteSynchronously);
            tcs.SetResult(true);
            sbj.SubscribeConsole();
        }
    }

    public static class TaskExtension
    {
        private static IObservable<T> ToObservable<T>(this Task<T> task)
        {
            AsyncSubject<T> sbj = new AsyncSubject<T>();
            task.ContinueWith(t =>
            {
                switch (t.Status)
                {
                    case TaskStatus.RanToCompletion:
                        sbj.OnNext(t.Result);
                        sbj.OnCompleted();
                        break;
                    case TaskStatus.Faulted:
                        sbj.OnError(t.Exception.InnerException);
                        break;
                    case TaskStatus.Canceled:
                        sbj.OnError(new TaskCanceledException(t));
                        break;
                }
            }, TaskContinuationOptions.ExecuteSynchronously);
            sbj.SubscribeConsole();
            return sbj.AsObservable();
        }

        private static IObservable<Unit> Observable<Unit>(this Task task)
        {
            AsyncSubject<Unit> sbj = new AsyncSubject<Unit>();
            task.ContinueWith(t =>
            {
                switch (t.Status)
                {
                    case TaskStatus.RanToCompletion:
                        sbj.OnCompleted();
                        break;
                    case TaskStatus.Faulted:
                        sbj.OnError(t.Exception.InnerException);
                        break;
                    case TaskStatus.Canceled:
                        sbj.OnError(new TaskCanceledException(t));
                        break;
                }
            }, TaskContinuationOptions.ExecuteSynchronously);
            sbj.SubscribeConsole();
            return sbj.AsObservable();
        }
    }
}
