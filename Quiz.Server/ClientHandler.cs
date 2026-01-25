using Newtonsoft.Json;
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

        public string StudentID { get; private set; }
        public string StudentName { get; private set; }
        public string ClientIP { get; private set; }
        public bool IsAuthenticated { get; private set; }

        public event Action<ClientHandler, LoginData> OnLoginRequested;
        public event Action<ClientHandler, AnswerSubmit> OnAnswerReceived;
        public event Action<ClientHandler> OnDisconnected;

        public ClientHandler(TcpClient client)
        {
            _client = client;
            _stream = client.GetStream();
            ClientIP = ((System.Net.IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
        }

        public void Start() => Task.Run(ReceiveLoop);

        private async Task ReceiveLoop()
        {
            try
            {
                while (_client.Connected)
                {
                    // Chuyển sang dùng DataHelper để đọc gói tin chuẩn
                    byte[] payload = await Task.Run(() => DataHelper.ReadPacket(_stream));
                    if (payload == null) break;

                    string json = Encoding.UTF8.GetString(payload);
                    HandlePacket(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ClientHandler error: {ex.Message}");
            }
            finally { Close(); }
        }

        private void HandlePacket(string json)
        {
            try
            {
                var packet = JsonConvert.DeserializeObject<Packet>(json);
                if (packet == null) return;

                switch (packet.Type)
                {
                    case Packet.TYPE_LOGIN:
                        var loginData = JsonConvert.DeserializeObject<LoginData>(packet.Data.ToString());
                        loginData.ClientIP = ClientIP;
                        OnLoginRequested?.Invoke(this, loginData);
                        break;
                    case Packet.TYPE_SUBMIT:
                        var submit = JsonConvert.DeserializeObject<AnswerSubmit>(packet.Data.ToString());
                        OnAnswerReceived?.Invoke(this, submit);
                        break;
                }
            }
            catch (Exception ex) { Console.WriteLine($"Parse error: {ex.Message}"); }
        }

        public void SendPacket(Packet packet) => DataHelper.SendObject(_stream, packet);

        public void SetAuthenticated(string id, string name)
        {
            StudentID = id; StudentName = name; IsAuthenticated = true;
        }

        public void Close()
        {
            _stream?.Close(); _client?.Close(); OnDisconnected?.Invoke(this);
        }
    }
}