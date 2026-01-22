using Newtonsoft.Json;
using Quiz.Client.Model;
using Quiz.Client.Models;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class NetworkClientService : IDisposable
{
    private TcpClient _client;
    private NetworkStream _stream;
    private CancellationTokenSource _cts;
    public event Action<string> OnRawMessage;
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
        catch (Exception ex)
        {
            OnStatus?.Invoke($"Lỗi kết nối: {ex.Message}");
        }
    }

    private async Task ReceiveLoopAsync(CancellationToken token)
    {
        var buffer = new byte[4096];
        var sb = new StringBuilder();

        while (!token.IsCancellationRequested)
        {
            int read = await _stream.ReadAsync(buffer, 0, buffer.Length);
            if (read == 0)
            {
                OnStatus?.Invoke("Mất kết nối server.");
                break;
            }

            string chunk = Encoding.UTF8.GetString(buffer, 0, read);
            sb.Append(chunk);

            int idx;
            while ((idx = sb.ToString().IndexOf('\n')) >= 0)
            {
                string line = sb.ToString().Substring(0, idx).Trim();
                sb.Remove(0, idx + 1);
                if (line.Length == 0) continue;

                OnRawMessage?.Invoke(line);

                try
                {
                    var obj = JsonConvert.DeserializeObject<dynamic>(line);
                    string type = obj?.type;
                    if (type == "examSession")
                    {
                        var session = JsonConvert.DeserializeObject<ExamViewModel>(line);
                        OnExamSessionReceived?.Invoke(session);
                        OnStatus?.Invoke("Đã nhận đề thi.");
                    }
                }
                catch (Exception ex)
                {
                    OnStatus?.Invoke($"Lỗi parse JSON: {ex.Message}");
                }
            }
        }
    }

    public async Task SendAsync(object obj, CancellationToken token = default)
    {
        if (_stream == null)
        {
            OnStatus?.Invoke("Chưa kết nối server.");
            return;
        }

        var json = JsonConvert.SerializeObject(obj) + "\n";
        var bytes = Encoding.UTF8.GetBytes(json);
        await _stream.WriteAsync(bytes, 0, bytes.Length);
        await _stream.FlushAsync();
    }

    public void Dispose()
    {
        try { _cts?.Cancel(); } catch { }
        try { _stream?.Dispose(); } catch { }
        try { _client?.Close(); } catch { }
    }
}
