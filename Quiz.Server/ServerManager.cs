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
        private UdpClient _udpClient;
        private List<ClientHandler> _clients = new List<ClientHandler>();
        private Dictionary<string, ExamResult> _results = new Dictionary<string, ExamResult>();
        private bool _running;
        private ExamSession _currentExam;
        private static readonly Random _rng = new Random();

        public event Action<int> OnClientCountChanged;
        public event Action<AnswerSubmit> OnAnswerReceived;
        public event Action<string> OnLog;

        public void Start(int port)
        {
            try
            {
                _listener = new TcpListener(IPAddress.Any, port);
                _listener.Start();
                _running = true;
                _udpClient = new UdpClient();
                _udpClient.JoinMulticastGroup(IPAddress.Parse("224.0.0.1"));
                OnLog?.Invoke($"Server started on port {port}");
                Task.Run(AcceptLoop);
            }
            catch (Exception ex) { OnLog?.Invoke($"Start Error: {ex.Message}"); }
        }

        private async Task AcceptLoop()
        {
            while (_running)
            {
                try
                {
                    TcpClient tcpClient = await _listener.AcceptTcpClientAsync();
                    ClientHandler handler = new ClientHandler(tcpClient);
                    handler.OnLoginRequested += HandleLogin;
                    handler.OnAnswerReceived += HandleAnswerSubmit;
                    handler.OnDisconnected += (c) => {
                        _clients.Remove(c);
                        OnClientCountChanged?.Invoke(_clients.Count(cl => cl.IsAuthenticated));
                    };
                    _clients.Add(handler);
                    handler.Start();
                }
                catch { }
            }
        }

        private void HandleLogin(ClientHandler client, LoginData loginData)
        {
            if (_clients.Any(c => c.IsAuthenticated && c.StudentID == loginData.StudentID && c != client))
            {
                client.SendPacket(new Packet(Packet.TYPE_LOGIN_FAILED, "", new { Reason = "Mã sinh viên đã tồn tại" }));
                return;
            }
            client.SetAuthenticated(loginData.StudentID, loginData.StudentName);
            client.SendPacket(new Packet(Packet.TYPE_LOGIN_SUCCESS, loginData.StudentID, new { Message = "Đã kết nối." }));
            OnClientCountChanged?.Invoke(_clients.Count(c => c.IsAuthenticated));
        }

        private void HandleAnswerSubmit(ClientHandler client, AnswerSubmit submit)
        {
            try
            {
                // GIẢI MÃ VÀ XÁC THỰC BƯỚC 2
                byte[] decBytes = DataHelper.XorProcess(Convert.FromBase64String(submit.EncryptedData), client.StudentID);
                string decJson = Encoding.UTF8.GetString(decBytes);
                if (DataHelper.CreateChecksum(decJson) != submit.Checksum)
                {
                    OnLog?.Invoke($"[SECURITY] Sai Checksum từ {submit.StudentID}!");
                    return;
                }

                submit.Answers = JsonConvert.DeserializeObject<Dictionary<string, int>>(decJson);
                OnAnswerReceived?.Invoke(submit);
                if (_currentExam == null) return;

                var res = CalculateScore(submit);
                _results[submit.StudentID] = res;
                client.SendPacket(new Packet(Packet.TYPE_RESULT, submit.StudentID, res));
                OnLog?.Invoke($"SV {submit.StudentID} nộp bài thành công.");
            }
            catch (Exception ex) { OnLog?.Invoke($"Lỗi nộp bài: {ex.Message}"); }
        }

        private ExamResult CalculateScore(AnswerSubmit submit)
        {
            var res = new ExamResult { StudentID = submit.StudentID, TotalQuestions = _currentExam.Questions.Count, MaxScore = _currentExam.Questions.Sum(q => q.Points) };
            var h = _clients.FirstOrDefault(c => c.StudentID == submit.StudentID);
            if (h != null) res.StudentName = h.StudentName;

            foreach (var q in _currentExam.Questions)
            {
                int sel = submit.Answers.ContainsKey(q.ID) ? submit.Answers[q.ID] : -1;
                bool ok = sel == q.CorrectIndex;
                if (ok) { res.TotalScore += q.Points; res.CorrectCount++; }
                res.Details.Add(new QuestionResult { QuestionID = q.ID, SelectedAnswer = sel, CorrectAnswer = q.CorrectIndex, IsCorrect = ok });
            }
            return res;
        }

        public void BroadcastExam(ExamSession exam)
        {
            _currentExam = exam;
            // XÁO TRỘN ĐỀ THI BƯỚC 3
            var clientVer = new ExamSession
            {
                ExamTitle = exam.ExamTitle,
                DurationMinutes = exam.DurationMinutes,
                Questions = ShuffleList(exam.Questions.Select(q => new Question
                {
                    ID = q.ID,
                    Content = q.Content,
                    Points = q.Points,
                    CorrectIndex = -1,
                    Options = ShuffleList(new List<string>(q.Options))
                }).ToList())
            };
            var p = new Packet(Packet.TYPE_START_EXAM, "", clientVer);
            foreach (var c in _clients.Where(cl => cl.IsAuthenticated)) c.SendPacket(p);
            OnLog?.Invoke($"[BROADCAST] Đã phát đề (Đã xáo trộn & Ẩn đáp án).");
        }

        private List<T> ShuffleList<T>(List<T> list)
        {
            int n = list.Count;
            while (n > 1) { n--; int k = _rng.Next(n + 1); T val = list[k]; list[k] = list[n]; list[n] = val; }
            return list;
        }

        public void Stop() { _running = false; _listener?.Stop(); _udpClient?.Close(); }
    }
}