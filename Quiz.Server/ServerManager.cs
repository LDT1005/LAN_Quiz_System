using Quiz.Shared;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Quiz.Server.Network
{
    public class ServerManager
    {
        private TcpListener _listener;
        private List<ClientHandler> _clients = new List<ClientHandler>();
        private bool _running;

        public event Action<int> OnClientCountChanged;
        public event Action<AnswerSubmit> OnAnswerReceived;

        public void Start(int port)
        {
            _listener = new TcpListener(IPAddress.Any, port);
            _listener.Start();
            _running = true;

            Task.Run(AcceptLoop);
        }

        private async Task AcceptLoop()
        {
            while (_running)
            {
                TcpClient tcpClient = await _listener.AcceptTcpClientAsync();
                ClientHandler handler = new ClientHandler(tcpClient);

                handler.OnDisconnected += ClientDisconnected;
                handler.OnAnswerReceived += submit =>
                {
                    OnAnswerReceived?.Invoke(submit);
                };

                _clients.Add(handler);
                OnClientCountChanged?.Invoke(_clients.Count);

                handler.Start();
            }
        }

        private void ClientDisconnected(ClientHandler client)
        {
            _clients.Remove(client);
            OnClientCountChanged?.Invoke(_clients.Count);
        }

        public void Stop()
        {
            _running = false;
            _listener.Stop();
        }
    }
}
