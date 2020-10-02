using RxDemo.Chat;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;

namespace RxDemo.Example
{
    public class ChatExample : IExample
    {
        private ChatClient _client;

        private IObservable<string> _liveMessage;
        private IEnumerable<string> _loadedMessage;

        public ChatExample()
        {
            _client = new ChatClient();
            _liveMessage =
                _client.ObserveMessages("guest", "guest");
            _loadedMessage = LoadMessageFromDB();
        }

        public void Start()
        {
            //Merged();
            LoadFirst();
        }

        private static IEnumerable<string> LoadMessageFromDB()
        {
            yield return "Loaded1";
            yield return "Loaded2";
        }

        private void Merged()
        {
            _loadedMessage.ToObservable()
                .Concat(_liveMessage)
                .SubscribeConsole("merged");

            _client.ChatConnection.Received("live message1");
            _client.ChatConnection.Received("live message2");
        }

        private void LoadFirst()
        {
            _liveMessage
            .StartWith(_loadedMessage)
            .SubscribeConsole("loaded first");

            _client.ChatConnection.Received("live message1");
            _client.ChatConnection.Received("live message2");
        }
    }
}
