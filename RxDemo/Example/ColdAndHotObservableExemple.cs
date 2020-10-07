using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RxDemo.Example
{
    public class ColdAndHotObservableExemple : IExample
    {
        public void Start()
        {
            //ColdObservable();
            //HotObservable();
            //Numbers();
            //PublishLast();
            //AutomaticDisconnection();
            Replay();
        }

        private static void ColdObservable()
        {
            //A cold observable is an observable that starts emitting notifications only when an observer subscribes, 
            //and each observer receives the full sequence of notifications without sharing them with other observers.[4]
            var coldObservable =
                Observable.Create<string>(async o =>
                {
                    o.OnNext("Hello");
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    o.OnNext("Rx");
                });
            coldObservable.SubscribeConsole("o1");
            Thread.Sleep(TimeSpan.FromSeconds(0.5));
            coldObservable.SubscribeConsole("o2");

           Console.Read();
        }

        private static void HotObservable()
        {
            //A hot observable is an observable that emits notifications regardless of its observers(even if there are none). 
            //The notifications emitted by hot observables are shared among their observers.
            var coldObservable = Observable.Interval(TimeSpan.FromSeconds(1)).Take(5);
            var ConnectableObservable = coldObservable.Publish();

            ConnectableObservable.SubscribeConsole("first");
            ConnectableObservable.SubscribeConsole("second");

            ConnectableObservable.Connect();

            Thread.Sleep(2000);
            ConnectableObservable.SubscribeConsole("third");

            Console.Read();
        }

        private static void Numbers()
        {
            int i = 0;
            var numbers = Observable.Range(1, 5).Select(_ => i++);

            //var zipped = numbers
            //    .Zip(numbers, (a, b) => a + b)
            //    .SubscribeConsole("zipped");

            var publishZip = numbers.Publish(published =>
                published.Zip(published, (a, b) => a + b));
            publishZip.SubscribeConsole("publishZip");
        }

        private void PublishLast()
        {
            var coldObservable =
                Observable.Timer(TimeSpan.FromSeconds(5))
                .Select(_ => "Rx");
            var connectableObservable = coldObservable.PublishLast();
            connectableObservable.SubscribeConsole("first");
            connectableObservable.SubscribeConsole("second");
            connectableObservable.Connect();
            Thread.Sleep(6000);
            connectableObservable.SubscribeConsole("third");
            Console.Read();
        }

        private void AutomaticDisconnection()
        {
            var publishedObservable = Observable.Interval(TimeSpan.FromSeconds(1))
                .Do(x => Console.WriteLine("Generating {0}", x))
                .Publish()
                .RefCount();

            var subscription1 = publishedObservable.SubscribeConsole("first");
            var subscription2 = publishedObservable.SubscribeConsole("second");

            Thread.Sleep(3000);
            subscription1.Dispose();
            Thread.Sleep(3000);
            subscription2.Dispose();
            Console.Read();
        }

        private void Replay()
        {
            var publishedObservable = Observable.Interval(TimeSpan.FromSeconds(1))
                .Take(5)
                .Replay(2);
            publishedObservable.Connect();
            var subscription1 = publishedObservable.SubscribeConsole("first");
            Thread.Sleep(3000);
            var subscription2 = publishedObservable.SubscribeConsole("second");
            Console.Read();
        }

    }
}
