using Newtonsoft.Json;
using System;
using System.Net.Sockets;
using System.Text;

using System.Threading.Tasks;

namespace Quiz.Client.Service
{
    public class NetworkClientService : IDisposable
    {
        private TcpClient _client;
        private NetworkStream _stream;

        public event Action<string> OnStatus;

        public async Task ConnectAsync(string host, int port)
        {
            try
            {
                _client = new TcpClient();
                await _client.ConnectAsync(host, port);
                _stream = _client.GetStream();
                OnStatus?.Invoke("Đã kết nối tới server");
            }
            catch (Exception ex)
            {
                OnStatus?.Invoke($"Lỗi kết nối: {ex.Message}");
            }
        }

        public async Task SendAsync(object payload)
        {
            if (_stream == null) throw new InvalidOperationException("Chưa kết nối server");
            string json = JsonConvert.SerializeObject(payload);
            byte[] data = Encoding.UTF8.GetBytes(json + "\n");
            await _stream.WriteAsync(data, 0, data.Length);
        }

        public void Dispose()
        {
            _stream?.Dispose();
            _client?.Close();
        }
    }
}
