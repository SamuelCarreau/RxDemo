using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RxDemo.Example
{
    public class SimpleExample : IExample
    {
        public void Start()
        {
            //ToLookup();
            //ObservableTask();
            //IsPrimeObservable();
            IntervalOperator();
            Console.Read();
        }

        private void IntervalOperator()
        {
            IObservable<string> firstObservable =
                Observable
                .Interval(TimeSpan.FromSeconds(1))
                .Select(x => "first" + x);
            IObservable<string> secondObservable =
                Observable
                .Interval(TimeSpan.FromSeconds(2))
                .Select(x => "second" + x)
                .Take(5);

            IObservable<IObservable<string>> immediateObservalbe =
                Observable.Return(firstObservable);

            IObservable<IObservable<string>> scheduledObservalbe =
                Observable.Timer(TimeSpan.FromSeconds(5))
                .Select(x => secondObservable);

            immediateObservalbe
                .Merge(scheduledObservalbe)
                .Switch()
                .Timestamp()
                .SubscribeConsole("timer switch");


        }

        private static void IsPrimeObservable()
        {
            Func<int, Task<bool>> IsPrimeAsync = async (number) =>
            {
                await Task.Delay(500);
                return number switch
                {
                    1 => true,
                    2 => true,
                    3 => true,
                    5 => true,
                    7 => true,
                    _ => false
                };
            };

            //Observable.Range(1, 10)
            //    .SelectMany(number => IsPrimeAsync(number),
            //    (number, isprime) => new { number, isprime })
            //    .Where(x => x.isprime)
            //    .Select(x => x.number)
            //    .SubscribeConsole();
            IObservable<int> primes =
                from number in Observable.Range(1, 10)
                from isprime in IsPrimeAsync(number)
                where isprime
                select number;
            primes.SubscribeConsole();
            Console.Read();
        }

        private static void ObservableTask()
        {
            Observable.Create<int>((o, ct) =>
            {
                return Task.Run(async () =>
                {
                    for (int i = 0; i < 10; i++)
                    {
                        await Task.Delay(500);
                        o.OnNext(i);
                    }
                    o.OnCompleted();
                });
            }).SubscribeConsole();
            Console.Read();
        }

        private void Generate()
        {
            IObservable<int> observable = Observable.Generate(
                0,
                i => i < 10,
                i => i + 1,
                i => i * 2);
            observable.SubscribeConsole();
        }

        private void ToLookup()
        {
            IEnumerable<string> cities =
                new[] { "London", "Tel-Aviv", "Tokyo", "Rome", "Madrid" };
            var lookupObservable =
                cities
                .ToObservable()
                .ToLookup(c => c.Length);

            lookupObservable
                .Select(lookup =>
                {
                    var groups = new StringBuilder();
                    foreach (var grp in lookup)
                        groups.AppendFormat("[key:{0} => {1}]", grp.Key, grp.Count());
                    return groups.ToString();
                })
                .SubscribeConsole();
        }

        private void ToList()
        {
            var observable = Observable.Create<string>(o =>
            {
                o.OnNext("Observalbe");
                o.OnNext("To");
                o.OnNext("List");
                o.OnCompleted();
                return Disposable.Empty;
            });

            IObservable<IList<string>> listObservable = observable.ToList();
            listObservable
                .Select(lst => string.Join(",", lst))
                .SubscribeConsole("list ready");
        }

        private void NumberAndString()
        {
            var numbers = new NumbersObservable(5);
            var subscription = numbers.SubcribeConsole("numbers");

            //var numbers = Observable.Create<int>(obserber =>
            //{
            //    for (int i = 0; i < 5; i++)
            //    {
            //        obserber.OnNext(i);
            //    }
            //    obserber.OnCompleted();
            //    return Disposable.Empty;
            //});
            //var subscription = numbers.SubcribeConsole("numbers");

            //IEnumerable<string> names = new[] { "Shira", "Yonatan", "Gabi", "Tamir" };
            //IObservable<string> observable = names.ToObservable();

            //observable.SubscribeConsole("names");

            //NumbersAndThrow().ToObservable().SubscribeConsole("throws");
        }

        static IEnumerable<int> NumbersAndThrow()
        {
            yield return 1;
            yield return 2;
            yield return 3;
            throw new ApplicationException("Something Bad Happened");
            yield return 4;
        }
    }
}
