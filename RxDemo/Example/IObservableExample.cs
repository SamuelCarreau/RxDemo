using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Drawing;

namespace RxDemo.Example
{
    public class IObservableExample : IExample
    {
        public void Start()
        {
           // DelayedSubscription();
            StopEmittingAtScheduledTime(1);
            Console.Read();
        }

        public void DelayedSubscription()
        {
            Console.WriteLine("Creating subscription at {0}", DateTime.Now.ToUniversalTime());
            Observable.Range(1, 5)
                .Timestamp()
                .DelaySubscription(TimeSpan.FromSeconds(5))
                .SubscribeConsole();
        }

        public void StopEmittingAtScheduledTime(int value)
        {
            switch (value)
            {
                case 0:
                    Console.WriteLine("Creating subscription at {0}", DateTimeOffset.Now);
                    Observable.Timer(DateTimeOffset.Now, TimeSpan.FromSeconds(1))
                        .Select(t => DateTimeOffset.Now)
                        .TakeUntil(DateTimeOffset.Now.AddSeconds(5))
                        .SubscribeConsole();
                    break;
                case 1:
                    Console.WriteLine("Creating subscription at {0}", DateTimeOffset.Now);
                    Observable.Timer(DateTimeOffset.Now, TimeSpan.FromSeconds(1))
                        .Select(t => DateTimeOffset.Now)
                        .TakeUntil(Observable.Timer(TimeSpan.FromSeconds(5)))
                        .SubscribeConsole();
                    break;
            }
        }
    }
}
