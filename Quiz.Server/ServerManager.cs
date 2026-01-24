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

        public event Action<int> OnClientCountChanged;
        public event Action<AnswerSubmit> OnAnswerReceived;
        public event Action<string> OnLog;
        public event Action<ClientHandler> OnClientConnected;

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
            catch (Exception ex)
            {
                OnLog?.Invoke($"Server start error: {ex.Message}");
            }
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
                    handler.OnDisconnected += ClientDisconnected;

                    _clients.Add(handler);
                    OnLog?.Invoke($"Client connected from {handler.ClientIP}");

                    handler.Start();
                }
                catch (Exception ex)
                {
                    if (_running)
                        OnLog?.Invoke($"Accept error: {ex.Message}");
                }
            }
        }

        private void HandleLogin(ClientHandler client, LoginData loginData)
        {
            if (_clients.Any(c => c.IsAuthenticated && c.StudentID == loginData.StudentID && c != client))
            {
                var failPacket = new Packet(Packet.TYPE_LOGIN_FAILED, "", new
                {
                    Reason = "Mã sinh viên đã tồn tại"
                });
                client.SendPacket(failPacket);
                OnLog?.Invoke($"LOGIN FAILED: Duplicate StudentID {loginData.StudentID}");
                return;
            }

            client.SetAuthenticated(loginData.StudentID, loginData.StudentName);

            var successPacket = new Packet(Packet.TYPE_LOGIN_SUCCESS, loginData.StudentID, new
            {
                Message = "Đã kết nối. Chờ giáo viên phát đề...",
                ServerTime = DateTime.Now
            });

            client.SendPacket(successPacket);
            OnLog?.Invoke($"LOGIN SUCCESS: {loginData.StudentName} ({loginData.StudentID})");
            OnClientCountChanged?.Invoke(_clients.Count(c => c.IsAuthenticated));
            OnClientConnected?.Invoke(client);
        }

        private void HandleAnswerSubmit(ClientHandler client, AnswerSubmit submit)
        {
            OnLog?.Invoke($"Received submission from {submit.StudentID}");
            OnAnswerReceived?.Invoke(submit);

            if (_currentExam == null)
            {
                OnLog?.Invoke("ERROR: No exam session active");
                return;
            }

            var result = CalculateScore(submit);
            _results[submit.StudentID] = result;

            var resultPacket = new Packet(Packet.TYPE_RESULT, submit.StudentID, result);
            client.SendPacket(resultPacket);

            OnLog?.Invoke($"Sent result to {submit.StudentID}: {result.TotalScore}/{result.MaxScore}");
        }

        private ExamResult CalculateScore(AnswerSubmit submit)
        {
            var result = new ExamResult
            {
                StudentID = submit.StudentID,
                SubmitTime = submit.SubmitTime,
                TotalQuestions = _currentExam.Questions.Count,
                MaxScore = _currentExam.Questions.Sum(q => q.Points)
            };

            var handler = _clients.FirstOrDefault(c => c.StudentID == submit.StudentID);
            if (handler != null)
                result.StudentName = handler.StudentName;

            foreach (var question in _currentExam.Questions)
            {
                var questionResult = new QuestionResult
                {
                    QuestionID = question.ID,
                    CorrectAnswer = question.CorrectIndex,
                    SelectedAnswer = submit.Answers.ContainsKey(question.ID) ? submit.Answers[question.ID] : -1
                };

                questionResult.IsCorrect = questionResult.SelectedAnswer == questionResult.CorrectAnswer;

                if (questionResult.IsCorrect)
                {
                    result.TotalScore += question.Points;
                    result.CorrectCount++;
                }

                result.Details.Add(questionResult);
            }

            return result;
        }

        private void ClientDisconnected(ClientHandler client)
        {
            _clients.Remove(client);
            OnLog?.Invoke($"Client disconnected: {client.ClientIP}");
            OnClientCountChanged?.Invoke(_clients.Count(c => c.IsAuthenticated));
        }

        // CHỈ GIỮ LẠI 1 PHƯƠNG THỨC BroadcastExam (XÓA CÁI TRÙNG LẶP)
        public void BroadcastExam(ExamSession exam)
        {
            _currentExam = exam;

            var clientExam = new ExamSession
            {
                ExamTitle = exam.ExamTitle,
                DurationMinutes = exam.DurationMinutes,
                StartTime = DateTime.Now,
                Questions = exam.Questions.Select(q => new Question
                {
                    ID = q.ID,
                    Content = q.Content,
                    Options = new List<string>(q.Options),
                    CorrectIndex = -1,
                    Points = q.Points
                }).ToList()
            };

            var packet = new Packet(Packet.TYPE_START_EXAM, "", clientExam);

            foreach (var client in _clients.Where(c => c.IsAuthenticated))
            {
                client.SendPacket(packet);
            }

            BroadcastUdp(packet);

            OnLog?.Invoke($"Exam broadcast: {exam.ExamTitle} to {_clients.Count(c => c.IsAuthenticated)} clients");
        }

        public void SendBroadcastMessage(string message, string priority = "NORMAL")
        {
            var broadcastData = new BroadcastData(message, priority);
            var packet = new Packet(Packet.TYPE_BROADCAST, "", broadcastData);

            foreach (var client in _clients.Where(c => c.IsAuthenticated))
            {
                client.SendPacket(packet);
            }

            BroadcastUdp(packet);

            OnLog?.Invoke($"Broadcast sent: {message}");
        }

        private void BroadcastUdp(Packet packet)
        {
            try
            {
                string json = JsonConvert.SerializeObject(packet);
                byte[] data = Encoding.UTF8.GetBytes(json);
                var endpoint = new IPEndPoint(IPAddress.Parse("224.0.0.1"), 9999);
                _udpClient.Send(data, data.Length, endpoint);
            }
            catch (Exception ex)
            {
                OnLog?.Invoke($"UDP Broadcast error: {ex.Message}");
            }
        }

        public List<ExamResult> GetResults()
        {
            return _results.Values.ToList();
        }

        public void DisconnectAll()
        {
            var disconnectPacket = new Packet(Packet.TYPE_DISCONNECT, "", new
            {
                Message = "Kết thúc phiên thi"
            });

            foreach (var client in _clients.ToList())
            {
                client.SendPacket(disconnectPacket);
                client.Close();
            }

            OnLog?.Invoke("All clients disconnected");
        }

        public void Stop()
        {
            _running = false;
            DisconnectAll();
            _listener?.Stop();
            _udpClient?.Close();
            OnLog?.Invoke("Server stopped");
        }
    }
}