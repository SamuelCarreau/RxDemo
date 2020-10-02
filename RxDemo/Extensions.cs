using RxDemo.Chat;
using System;
using System.Collections.Generic;
using System.Text;

namespace RxDemo
{
    public static class Extensions
    {
        public static IDisposable SubscribeConsole<T>(
                this IObservable<T> observable,
                string name = "")
        {
            return observable.Subscribe(new ConsoleObserver<T>(name));
        }
    }

    public static class ChatExtensions
    {
        public static IObservable<string> ToObservable(
            this IChatConnection connection)
        {
            return new ObservableConnection(connection);
        }
    }
}
