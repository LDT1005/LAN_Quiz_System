using Newtonsoft.Json;
using Quiz.Shared;
using System.Text;

namespace Quiz.Client.Network
{
    public class ExamClientService
    {
        private readonly ClientConnection _connection;
        public ExamClientService(ClientConnection conn) => _connection = conn;

        public void SubmitAnswer(string studentId, string questionId, int selectedIndex)
        {
            var packet = new { type = "submit", data = new AnswerSubmit { StudentID = studentId, QuestionID = questionId, SelectedAnswer = selectedIndex } };
            string json = JsonConvert.SerializeObject(packet) + "\n";
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            _connection.Stream.Write(bytes, 0, bytes.Length);
            _connection.Stream.Flush();
        }
    }
}