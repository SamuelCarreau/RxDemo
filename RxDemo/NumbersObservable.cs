using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;

namespace RxDemo
{
    public class NumbersObservable : IObservable<int>
    {
        private readonly int _amount;

        public NumbersObservable(int amount)
        {
            _amount = amount;
        }

        internal object SubcribeConsole(string v)
        {
            throw new NotImplementedException();
        }

        public IDisposable Subscribe(IObserver<int> observer)
        {
            for (int i = 0; i < _amount; i++)
            {
                observer.OnNext(i);
            }
            observer.OnCompleted();
            return Disposable.Empty;
        }
    }
}
