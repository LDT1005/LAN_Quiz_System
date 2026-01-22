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
        private readonly CancellationTokenSource _cts = new CancellationTokenSource(); // Sử dụng khai báo đầy đủ cho C# 7.3
        private readonly StringBuilder _buffer = new StringBuilder();

        // Các sự kiện để báo về ServerManager
        public event Action<ClientHandler> OnDisconnected;
        public event Action<AnswerSubmit> OnAnswerReceived;

        public ClientHandler(TcpClient client)
        {
            _client = client;
            _stream = client.GetStream();
        }

        /// <summary>
        /// Bắt đầu tiến trình nhận dữ liệu từ Client
        /// </summary>
        public void Start()
        {
            Task.Run(new Func<Task>(ReceiveLoopAsync));
        }

        private async Task ReceiveLoopAsync()
        {
            byte[] buf = new byte[4096];

            try
            {
                while (!_cts.IsCancellationRequested)
                {
                    // Đọc dữ liệu từ stream một cách bất đồng bộ
                    int read = await _stream.ReadAsync(buf, 0, buf.Length);

                    // Nếu read == 0 nghĩa là Client đã ngắt kết nối chủ động
                    if (read == 0) break;

                    // Chuyển byte nhận được thành string và đưa vào buffer
                    _buffer.Append(Encoding.UTF8.GetString(buf, 0, read));

                    // Xử lý gói tin theo dòng (dấu \n)
                    int idx;
                    while ((idx = _buffer.ToString().IndexOf('\n')) >= 0)
                    {
                        string line = _buffer.ToString(0, idx).Trim();
                        _buffer.Remove(0, idx + 1);

                        if (line.Length > 0)
                        {
                            HandleMessage(line);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi trong vòng lặp nhận dữ liệu: " + ex.Message);
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// Giải mã JSON và xử lý các loại yêu cầu từ Client
        /// </summary>
        private void HandleMessage(string json)
        {
            try
            {
                JObject root = JObject.Parse(json);
                string type = root["type"]?.ToString();

                // Kiểm tra nếu là gói tin nộp bài (submit)
                if (string.Equals(type, "submit", StringComparison.OrdinalIgnoreCase))
                {
                    if (root["data"] != null)
                    {
                        AnswerSubmit submit = root["data"].ToObject<AnswerSubmit>();
                        OnAnswerReceived?.Invoke(submit);
                    }
                }
                // Bạn có thể thêm các loại type khác tại đây (ví dụ: "login")
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi xử lý gói tin JSON: " + ex.Message);
            }
        }

        /// <summary>
        /// Đóng kết nối và giải phóng tài nguyên
        /// </summary>
        public void Close()
        {
            try
            {
                _cts.Cancel();
                _stream?.Close();
                _client?.Close();
            }
            catch { }
            finally
            {
                // Thông báo cho ServerManager biết Client này đã ngắt kết nối
                OnDisconnected?.Invoke(this);
            }
        }
    }
}