using Newtonsoft.Json;
using Quiz.Client.Model;
using Quiz.Shared;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quiz.Client.Network
{
    public class NetworkClientService : IDisposable
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private CancellationTokenSource _cts;

        public event Action<ExamViewModel> OnExamSessionReceived;
        public event Action<string> OnStatus;

        public async Task ConnectAsync(string host, int port)
        {
            try
            {
                _cts = new CancellationTokenSource();
                _client = new TcpClient();
                await _client.ConnectAsync(host, port);
                _stream = _client.GetStream();
                OnStatus?.Invoke("Đã kết nối server.");

                _ = Task.Run(() => ReceiveLoopAsync(_cts.Token));
            }
            catch (Exception ex) { OnStatus?.Invoke($"Lỗi kết nối: {ex.Message}"); }
        }

        private async Task ReceiveLoopAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested && _client.Connected)
                {
                    // Đọc gói tin có Framing 4 byte header
                    byte[] payload = await Task.Run(() => DataHelper.ReadPacket(_stream));
                    if (payload == null) break;

                    string json = Encoding.UTF8.GetString(payload);
                    var packet = JsonConvert.DeserializeObject<Packet>(json);

                    if (packet.Type == Packet.TYPE_START_EXAM)
                    {
                        var session = JsonConvert.DeserializeObject<ExamViewModel>(packet.Data.ToString());
                        OnExamSessionReceived?.Invoke(session);
                    }
                }
            }
            catch { OnStatus?.Invoke("Mất kết nối server."); }
        }

        public async Task SendAsync(object obj, CancellationToken token = default)
        {
            if (_stream == null) return;
            await Task.Run(() => DataHelper.SendObject(_stream, obj));
        }

        public void Dispose()
        {
            _cts?.Cancel(); _stream?.Dispose(); _client?.Close();
        }
    }
}