using System.Net.Sockets;
using System.Text;

namespace Quiz.Client
{
    public class ClientConnection
    {
        private TcpClient _client;
        private NetworkStream _stream;

        // SỬA: Thêm setter cho Client và Stream
        public TcpClient Client
        {
            get => _client;
            set => _client = value;
        }

        public NetworkStream Stream
        {
            get => _stream;
            set => _stream = value;
        }

        // THÊM properties mới
        public string StudentID { get; set; }
        public string StudentName { get; set; }
        public bool IsConnected => _client != null && _client.Connected;

        public void Connect()
        {
            if (IsConnected) return;
            _client = new TcpClient();
            _client.Connect("127.0.0.1", 8888);
            _stream = _client.GetStream();
        }

        public void SendString(string msg)
        {
            byte[] data = Encoding.UTF8.GetBytes(msg + "\n");
            _stream.Write(data, 0, data.Length);
            _stream.Flush();
        }

        public string ReceiveString()
        {
            byte[] buf = new byte[4096];
            int read = _stream.Read(buf, 0, buf.Length);
            return Encoding.UTF8.GetString(buf, 0, read).Trim();
        }
    }
}