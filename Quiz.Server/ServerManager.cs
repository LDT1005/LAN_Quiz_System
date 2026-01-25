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
        private bool _running = false;

        public bool IsRunning => _running;
        public event Action<int> OnClientCountChanged;
        public event Action<string> OnLog;
        public event Action<AnswerSubmit> OnAnswerReceived;

        public void Start(int port)
        {
            if (_running) return;
            try
            {
                _listener = new TcpListener(IPAddress.Any, port);
                _listener.Start();
                _running = true;
                OnLog?.Invoke($"[HỆ THỐNG]: Server đã mở tại cổng {port}. Sẵn sàng kết nối!");
                Task.Run(AcceptLoop);
            }
            catch (Exception ex) { _running = false; throw ex; }
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

        // HÀM QUAN TRỌNG: Giải mã và Chấm điểm tự động
        private void HandleAnswer(ClientHandler h, AnswerSubmit sub)
        {
            try
            {
                // 1. Giải mã XOR dựa trên MSSV
                byte[] decBytes = DataHelper.XorProcess(Convert.FromBase64String(sub.EncryptedData), h.StudentID);
                string decJson = Encoding.UTF8.GetString(decBytes);

                // 2. Kiểm tra tính toàn vẹn dữ liệu
                if (DataHelper.CreateChecksum(decJson) != sub.Checksum) return;

                sub.Answers = JsonConvert.DeserializeObject<Dictionary<string, int>>(decJson);
                OnAnswerReceived?.Invoke(sub);

                // 3. Tính toán điểm số dựa trên đáp án đúng của Server
                var result = CalculateScore(sub);
                _results[sub.StudentID] = result;

                // 4. Gửi trả kết quả ngay để Client thoát trạng thái chờ
                h.SendPacket(new Packet(Packet.TYPE_RESULT, sub.StudentID, result));
            }
            catch (Exception ex) { OnLog?.Invoke("Lỗi chấm bài: " + ex.Message); }
        }

        private ExamResult CalculateScore(AnswerSubmit sub)
        {
            var res = new ExamResult
            {
                StudentID = sub.StudentID,
                StudentName = _clients.FirstOrDefault(c => c.StudentID == sub.StudentID)?.StudentName ?? "N/A",
                TotalQuestions = _masterExam.Questions.Count,
                MaxScore = _masterExam.Questions.Sum(q => q.Points),
                Details = new List<QuestionResult>()
            };

            foreach (var q in _masterExam.Questions)
            {
                int studentChoice = sub.Answers.ContainsKey(q.ID) ? sub.Answers[q.ID] : -1;
                bool isCorrect = (studentChoice == q.CorrectIndex);
                if (isCorrect)
                {
                    res.CorrectCount++;
                    res.TotalScore += q.Points;
                }
                res.Details.Add(new QuestionResult
                {
                    QuestionID = q.ID,
                    SelectedAnswer = studentChoice,
                    CorrectAnswer = q.CorrectIndex,
                    IsCorrect = isCorrect
                });
            }
            return res;
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
                    CorrectIndex = q.CorrectIndex,
                    Options = q.Options.OrderBy(x => rng.Next()).ToList()
                }).OrderBy(x => rng.Next()).ToList()
            };
        }

        public List<ExamResult> GetFinalResults() => _results.Values.ToList();

        public void SendBroadcast(string msg, string priority)
        {
            var p = new Packet(Packet.TYPE_BROADCAST, "SERVER", new BroadcastData(msg, priority));
            byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(p));
            _udpSender.Send(data, data.Length, new IPEndPoint(IPAddress.Parse("224.0.0.1"), 9999));
        }
    }
}