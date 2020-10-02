using System;
using System.Collections.Generic;
using System.Text;

namespace RxDemo.Chat
{
    public class ChatClient
    {
        public ChatConnection ChatConnection { get; private set; }
        public IChatConnection Connect(string user,string password)
        {
            // Connects to the chat service
            ChatConnection = new ChatConnection(user, password);
            return ChatConnection;
        }

        public IObservable<string> ObserveMessages(string user, string password)
        {
            var connection = Connect(user, password);
            return connection.ToObservable();
        }
    }
}
