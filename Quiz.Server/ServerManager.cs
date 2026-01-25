using Newtonsoft.Json;
using Quiz.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Server.Network
{
    public class ServerManager
    {
        private TcpListener _listener;
        private UdpClient _udpSender = new UdpClient();
        private List<ClientHandler> _clients = new List<ClientHandler>();
        private Dictionary<string, ExamResult> _results = new Dictionary<string, ExamResult>();
        private Dictionary<string, ExamSession> _studentExams = new Dictionary<string, ExamSession>();
        private ExamSession _masterExam;
        private bool _running;

        // CÁC SỰ KIỆN GỬI DỮ LIỆU RA GIAO DIỆN
        public event Action<int> OnClientCountChanged;
        public event Action<string> OnLog;
        public event Action<AnswerSubmit> OnAnswerReceived; // FIX: Đã thêm sự kiện này

        public void Start(int port)
        {
            try
            {
                _listener = new TcpListener(IPAddress.Any, port);
                _listener.Start();
                _running = true;
                OnLog?.Invoke($"Server started on port {port}");
                Task.Run(AcceptLoop);
            }
            catch (Exception ex) { OnLog?.Invoke("Start Error: " + ex.Message); }
        }

        private async Task AcceptLoop()
        {
            while (_running)
            {
                try
                {
                    var tcpClient = await _listener.AcceptTcpClientAsync();
                    var handler = new ClientHandler(tcpClient);

                    handler.OnLoginRequested += (h, data) => {
                        var existing = _clients.FirstOrDefault(c => c.StudentID == data.StudentID);
                        if (existing != null) existing.Close();

                        h.SetAuthenticated(data.StudentID, data.StudentName);
                        h.SendPacket(new Packet(Packet.TYPE_LOGIN_SUCCESS, data.StudentID, "Welcome"));

                        if (_studentExams.ContainsKey(data.StudentID))
                            h.SendPacket(new Packet(Packet.TYPE_START_EXAM, "", _studentExams[data.StudentID]));

                        OnClientCountChanged?.Invoke(_clients.Count(c => c.IsAuthenticated));
                    };

                    handler.OnAnswerReceived += HandleAnswer;
                    _clients.Add(handler);
                    handler.Start();
                }
                catch { }
            }
        }

        public void BroadcastExam(ExamSession exam)
        {
            _masterExam = exam;
            foreach (var client in _clients.Where(c => c.IsAuthenticated))
            {
                var shuffled = ShuffleExam(exam);
                _studentExams[client.StudentID] = shuffled;
                client.SendPacket(new Packet(Packet.TYPE_START_EXAM, "", shuffled));
            }
            OnLog?.Invoke("Đã phát đề thi xáo trộn tới tất cả thí sinh.");
        }

        private void HandleAnswer(ClientHandler h, AnswerSubmit sub)
        {
            try
            {
                // Giải mã dữ liệu nộp bài
                byte[] decBytes = DataHelper.XorProcess(Convert.FromBase64String(sub.EncryptedData), h.StudentID);
                string decJson = Encoding.UTF8.GetString(decBytes);

                if (DataHelper.CreateChecksum(decJson) != sub.Checksum)
                {
                    OnLog?.Invoke($"[CẢNH BÁO] Sai Checksum từ SV: {h.StudentID}");
                    return;
                }

                sub.Answers = JsonConvert.DeserializeObject<Dictionary<string, int>>(decJson);

                // KÍCH HOẠT SỰ KIỆN RA GIAO DIỆN
                OnAnswerReceived?.Invoke(sub);

                // Tính điểm và lưu kết quả
                var result = CalculateScore(sub);
                _results[sub.StudentID] = result;
                h.SendPacket(new Packet(Packet.TYPE_RESULT, sub.StudentID, result));
            }
            catch (Exception ex) { OnLog?.Invoke("Lỗi xử lý bài nộp: " + ex.Message); }
        }

        private ExamResult CalculateScore(AnswerSubmit sub)
        {
            var res = new ExamResult
            {
                StudentID = sub.StudentID,
                TotalQuestions = _masterExam.Questions.Count,
                MaxScore = _masterExam.Questions.Sum(q => q.Points)
            };
            // Logic chấm điểm...
            return res;
        }

        public void SendBroadcast(string msg, string priority)
        {
            var p = new Packet(Packet.TYPE_BROADCAST, "SERVER", new BroadcastData(msg, priority));
            byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(p));
            _udpSender.Send(data, data.Length, new IPEndPoint(IPAddress.Parse("224.0.0.1"), 9999));
        }

        private ExamSession ShuffleExam(ExamSession master)
        {
            var rng = new Random();
            return new ExamSession
            {
                ExamTitle = master.ExamTitle,
                DurationMinutes = master.DurationMinutes,
                Questions = master.Questions.Select(q => new Question
                {
                    ID = q.ID,
                    Content = q.Content,
                    Points = q.Points,
                    CorrectIndex = -1,
                    Options = q.Options.OrderBy(x => rng.Next()).ToList()
                }).OrderBy(x => rng.Next()).ToList()
            };
        }

        public List<ExamResult> GetFinalResults() => _results.Values.ToList();
    }
}