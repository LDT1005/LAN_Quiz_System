using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quiz.Shared;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Server.Network
{
    public class ClientHandler
    {
        private readonly TcpClient _client;
        private readonly NetworkStream _stream;
        private readonly StringBuilder _buffer = new StringBuilder();
        public event Action<ClientHandler, LoginData> OnLoginRequested;
        public event Action<AnswerSubmit> OnAnswerReceived;
        public event Action<ClientHandler> OnDisconnected;

        public ClientHandler(TcpClient client)
        {
            _client = client;
            _stream = client.GetStream();
        }

        public void Start() => Task.Run(ReceiveLoop);

        private async Task ReceiveLoop()
        {
            byte[] buf = new byte[4096];
            try
            {
                while (true)
                {
                    int read = await _stream.ReadAsync(buf, 0, buf.Length);
                    if (read == 0) break;
                    _buffer.Append(Encoding.UTF8.GetString(buf, 0, read));
                    int idx;
                    while ((idx = _buffer.ToString().IndexOf('\n')) >= 0)
                    {
                        string line = _buffer.ToString(0, idx).Trim();
                        _buffer.Remove(0, idx + 1);
                        if (line.Length > 0) HandlePacket(line);
                    }
                }
            }
            catch { }
            finally { Close(); }
        }

        private void HandlePacket(string json)
        {
            var packet = JsonConvert.DeserializeObject<Packet>(json);
            if (packet.Type == Packet.TYPE_LOGIN)
                OnLoginRequested?.Invoke(this, JsonConvert.DeserializeObject<LoginData>(packet.Data.ToString()));
            else if (packet.Type == Packet.TYPE_SUBMIT)
                OnAnswerReceived?.Invoke(JsonConvert.DeserializeObject<AnswerSubmit>(packet.Data.ToString()));
        }

        public void SendPacket(Packet packet)
        {
            byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(packet) + "\n");
            _stream.Write(data, 0, data.Length);
        }

        public void Close() { _stream?.Close(); _client?.Close(); OnDisconnected?.Invoke(this); }
    }
}