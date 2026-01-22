using System;
using System.Collections.Generic;
using Quiz.Client.Network;
using Quiz.Shared;

namespace Quiz.Client.Model
{
    public class ExamViewModel
    {
        public string ExamTitle { get; set; }
        public DateTime StartTime { get; set; }
        public int DurationMinutes { get; set; }
        public List<QuestionViewModel> Questions { get; set; }

        private ExamClientService _networkService;

        public ExamViewModel(ClientConnection conn)
        {
            Questions = new List<QuestionViewModel>();
            _networkService = new ExamClientService(conn);
        }

        public void SubmitAll(string studentId)
        {
            foreach (var q in Questions)
            {
                _networkService.SubmitAnswer(studentId, q.QuestionID, q.SelectedIndex);
            }
        }
    }

    public class QuestionViewModel
    {
        public string QuestionID { get; set; }
        public string Content { get; set; }
        public List<string> Options { get; set; }
        public int SelectedIndex { get; set; } = -1;

        public QuestionViewModel() { Options = new List<string>(); }
    }
}