using Newtonsoft.Json;
using Quiz.Client.Model;
using Quiz.Shared;
using System;
using System.Net;
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
        private UdpClient _udpListener;
        private CancellationTokenSource _cts;

        public event Action<ExamSession> OnExamReceived;
        public event Action<string, string> OnBroadcastReceived; // Message, Priority
        public event Action<string> OnStatus;

        public async Task ConnectAsync(string host, int port)
        {
            try
            {
                _cts = new CancellationTokenSource();
                _client = new TcpClient();
                await _client.ConnectAsync(host, port);
                _stream = _client.GetStream();
                OnStatus?.Invoke("Đã kết nối TCP Server.");

                _ = Task.Run(() => ReceiveLoopAsync(_cts.Token));
                _ = Task.Run(() => UdpListenLoop(_cts.Token));
            }
            catch (Exception ex) { OnStatus?.Invoke($"Lỗi: {ex.Message}"); }
        }

        private async Task ReceiveLoopAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested && _client.Connected)
            {
                byte[] payload = await Task.Run(() => DataHelper.ReadPacket(_stream));
                if (payload == null) break;

                string json = Encoding.UTF8.GetString(payload);
                var packet = JsonConvert.DeserializeObject<Packet>(json);

                if (packet.Type == Packet.TYPE_START_EXAM)
                {
                    var exam = JsonConvert.DeserializeObject<ExamSession>(packet.Data.ToString());
                    OnExamReceived?.Invoke(exam);
                }
                // Thêm các xử lý RESULT, DISCONNECT tại đây
            }
            OnStatus?.Invoke("Mất kết nối TCP.");
        }

        private void UdpListenLoop(CancellationToken token)
        {
            try
            {
                _udpListener = new UdpClient(9999);
                _udpListener.JoinMulticastGroup(IPAddress.Parse("224.0.0.1"));
                IPEndPoint remoteEp = new IPEndPoint(IPAddress.Any, 0);

                while (!token.IsCancellationRequested)
                {
                    byte[] data = _udpListener.Receive(ref remoteEp);
                    string json = Encoding.UTF8.GetString(data);
                    var packet = JsonConvert.DeserializeObject<Packet>(json);

                    if (packet.Type == Packet.TYPE_BROADCAST)
                    {
                        var bData = JsonConvert.DeserializeObject<BroadcastData>(packet.Data.ToString());
                        OnBroadcastReceived?.Invoke(bData.Message, bData.Priority);
                    }
                }
            }
            catch { }
        }

        public void Send(object obj) => DataHelper.SendObject(_stream, obj);

        public void Dispose()
        {
            _cts?.Cancel(); _stream?.Dispose(); _client?.Close(); _udpListener?.Close();
        }
    }
}