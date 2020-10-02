using System;

namespace RxDemo.Chat
{
    public interface IChatConnection
    {
        Action<string> Received { get; set; }
        Action Closed { get; set; }
        Action<Exception> Error { get; set; }
        void Disconnect();
    }
}