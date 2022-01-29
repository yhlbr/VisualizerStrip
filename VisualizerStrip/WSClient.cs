using System;
using System.Threading;
using System.Threading.Tasks;
using Websocket.Client;

namespace VisualizerStrip
{
    public sealed class WSClient
    {
        private static WSClient instance = null;
        private static readonly object padlock = new object();
        private ManualResetEvent ExitEvent;

        private WebsocketClient WsClient = null;

        private WSClient()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (WebsocketURL == null)
            {
                return;
            }

            var client = new WebsocketClient(WebsocketURL);
            client.ReconnectTimeout = TimeSpan.FromSeconds(5);
            client.ErrorReconnectTimeout = TimeSpan.FromSeconds(1);
            client.ReconnectionHappened.Subscribe(info =>
                Console.WriteLine($"Reconnection happened, type: {info.Type}"));

            WsClient = client;
            RunClient();
        }

        private void RunClient()
        {
            Task.Run(() =>
            {
                ExitEvent = new ManualResetEvent(false);
                WsClient.Start();
                ExitEvent.WaitOne();
            });
        }

        public static WSClient Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new WSClient();
                    }
                    return instance;
                }
            }
        }

        public bool SendMessage(string message)
        {
            if (WsClient == null)
            {
                return false;
            }

            Console.WriteLine("s");
            Task.Run(() => WsClient.Send(message));
            return true;
        }

        public bool UpdateUrl()
        {
            if (WebsocketURL != null)
            {
                WsClient.Url = WebsocketURL;
                WsClient.Reconnect();
                return true;
            }

            return false;
        }


        // ws://192.168.1.206:1212
        private Uri WebsocketURL
        {
            get
            {
                string endpoint = Properties.Settings.Default.WebsocketURL;
                if (endpoint == "" || endpoint == "ws://")
                {
                    return null;
                }

                return new Uri(endpoint);
            }
        }
    }
}
