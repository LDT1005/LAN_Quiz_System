using Quiz.Shared.Models;

namespace Quiz.Client
{
    public class ExamViewModel
    {
        private TcpClientService tcpService = new TcpClientService();

        public void SubmitAnswer(string studentId, string questionId, string answer)
        {
            ExamAnswer examAnswer = new ExamAnswer
            {
                StudentId = studentId,
                QuestionId = questionId,
                SelectedAnswer = answer
            };

            tcpService.Send(examAnswer);
        }
    }
}
