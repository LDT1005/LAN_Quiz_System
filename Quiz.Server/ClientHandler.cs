using Newtonsoft.Json.Linq;
using Quiz.Shared;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quiz.Server.Network
{
    public class ClientHandler
    {
        private readonly TcpClient _client;
        private readonly NetworkStream _stream;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly StringBuilder _buffer = new StringBuilder();

        public event Action<ClientHandler> OnDisconnected;
        public event Action<AnswerSubmit> OnAnswerReceived;

        public ClientHandler(TcpClient client)
        {
            _client = client;
            _stream = client.GetStream();
        }

        public void Start() => Task.Run(new Func<Task>(ReceiveLoopAsync));

        private async Task ReceiveLoopAsync()
        {
            byte[] buf = new byte[4096];
            try
            {
                while (!_cts.IsCancellationRequested)
                {
                    int read = await _stream.ReadAsync(buf, 0, buf.Length);
                    if (read == 0) break;
                    _buffer.Append(Encoding.UTF8.GetString(buf, 0, read));
                    int idx;
                    while ((idx = _buffer.ToString().IndexOf('\n')) >= 0)
                    {
                        string line = _buffer.ToString(0, idx).Trim();
                        _buffer.Remove(0, idx + 1);
                        if (line.Length > 0) HandleMessage(line);
                    }
                }
            }
            catch { }
            finally { Close(); }
        }

        private void HandleMessage(string json)
        {
            try
            {
                JObject root = JObject.Parse(json);
                if (root["type"]?.ToString() == "submit")
                {
                    var data = root["data"].ToObject<AnswerSubmit>();
                    OnAnswerReceived?.Invoke(data);
                }
            }
            catch { }
        }

        public void Close()
        {
            try { _stream?.Close(); _client?.Close(); } catch { }
            OnDisconnected?.Invoke(this);
        }
    }
}