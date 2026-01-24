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

        // THÊM CÁC PROPERTIES NÀY
        public string StudentID { get; private set; }
        public string StudentName { get; private set; }
        public string ClientIP { get; private set; }
        public bool IsAuthenticated { get; private set; }

        public event Action<ClientHandler, LoginData> OnLoginRequested;
        public event Action<ClientHandler, AnswerSubmit> OnAnswerReceived; // SỬA: THÊM ClientHandler
        public event Action<ClientHandler> OnDisconnected;

        public ClientHandler(TcpClient client)
        {
            _client = client;
            _stream = client.GetStream();
            // LẤY IP ADDRESS
            ClientIP = ((System.Net.IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
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
                        if (line.Length > 0)
                        {
                            HandlePacket(line);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ClientHandler error: {ex.Message}");
            }
            finally
            {
                Close();
            }
        }

        private void HandlePacket(string json)
        {
            try
            {
                var packet = JsonConvert.DeserializeObject<Packet>(json);

                if (packet.Type == Packet.TYPE_LOGIN)
                {
                    var loginData = JsonConvert.DeserializeObject<LoginData>(packet.Data.ToString());
                    loginData.ClientIP = ClientIP;
                    OnLoginRequested?.Invoke(this, loginData);
                }
                else if (packet.Type == Packet.TYPE_SUBMIT)
                {
                    var submit = JsonConvert.DeserializeObject<AnswerSubmit>(packet.Data.ToString());
                    OnAnswerReceived?.Invoke(this, submit); // SỬA: THÊM this
                }
                else if (packet.Type == Packet.TYPE_RECONNECT)
                {
                    var loginData = JsonConvert.DeserializeObject<LoginData>(packet.Data.ToString());
                    loginData.ClientIP = ClientIP;
                    OnLoginRequested?.Invoke(this, loginData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HandlePacket error: {ex.Message}");
            }
        }

        public void SendPacket(Packet packet)
        {
            try
            {
                string json = JsonConvert.SerializeObject(packet) + "\n";
                byte[] data = Encoding.UTF8.GetBytes(json);
                _stream.Write(data, 0, data.Length);
                _stream.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SendPacket error: {ex.Message}");
                Close();
            }
        }

        // THÊM METHOD NÀY
        public void SetAuthenticated(string studentID, string studentName)
        {
            StudentID = studentID;
            StudentName = studentName;
            IsAuthenticated = true;
        }

        public void Close()
        {
            try
            {
                _stream?.Close();
                _client?.Close();
                OnDisconnected?.Invoke(this);
            }
            catch { }
        }
    }
}