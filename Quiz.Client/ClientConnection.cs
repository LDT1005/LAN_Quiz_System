using System;
using System.Net.Sockets;
using System.Text;

namespace Quiz.Client
{
    public class ClientConnection
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private readonly string _serverIp = "127.0.0.1";
        private readonly int _serverPort = 8888;

        // FIX LỖI CS1061: Cho phép truy cập vào Stream
        public NetworkStream Stream => _stream;
        public bool IsConnected => _client != null && _client.Connected;

        public void Connect()
        {
            if (IsConnected) return;
            _client = new TcpClient();
            _client.Connect(_serverIp, _serverPort);
            _stream = _client.GetStream();
        }

        public void SendString(string message)
        {
            if (!IsConnected) throw new Exception("Chưa kết nối server");
            byte[] data = Encoding.UTF8.GetBytes(message + "\n");
            _stream.Write(data, 0, data.Length);
            _stream.Flush();
        }

        public string ReceiveString()
        {
            if (!IsConnected) return "";
            byte[] buffer = new byte[4096];
            int read = _stream.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer, 0, read).Trim();
        }
    }
}