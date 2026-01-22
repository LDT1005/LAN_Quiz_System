using Newtonsoft.Json;
using Quiz.Shared;
using System.Text;

namespace Quiz.Client.Network
{
    public class ExamClientService
    {
        private readonly ClientConnection _connection;

        public ExamClientService(ClientConnection connection)
        {
            _connection = connection;
        }

        public void SubmitAnswer(string studentId, string questionId, int selectedIndex)
        {
            var submit = new AnswerSubmit
            {
                StudentID = studentId,
                QuestionID = questionId,
                SelectedAnswer = selectedIndex
            };

            var packet = new { type = "submit", data = submit };
            string json = JsonConvert.SerializeObject(packet) + "\n"; // Đảm bảo có \n
            byte[] bytes = Encoding.UTF8.GetBytes(json);

            _connection.Stream.Write(bytes, 0, bytes.Length);
            _connection.Stream.Flush();
        }
    }
}