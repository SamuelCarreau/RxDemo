using RxDemo.Chat;
using RxDemo.Example;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Xml.Schema;

namespace RxDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            IExample example = new SimpleExample();
            example.Start();
        }
    }
}
