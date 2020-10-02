using System;

namespace RxDemo.Chat
{
    public class ChatConnection : IChatConnection
    {
        private string User { get; set; }
        public ChatConnection(string user, string password)
        {
            User = user;
        }
        public Action<string> Received { get; set; }
        public Action Closed { get; set; }
        public Action<Exception> Error { get; set; }

        public void Disconnect()
        {
            Console.WriteLine("{0} disconnected",User);
        }
    }
}
