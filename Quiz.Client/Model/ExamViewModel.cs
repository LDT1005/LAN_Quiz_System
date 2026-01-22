using System;
using System.Collections.Generic;

namespace Quiz.Client.Model
{
    public class ExamViewModel
    {
        public string ExamTitle { get; set; }
        public DateTime StartTime { get; set; }
        public int DurationMinutes { get; set; }

        public List<QuestionViewModel> Questions { get; set; }

        public ExamViewModel()
        {
            Questions = new List<QuestionViewModel>();
        }
    }

    public class QuestionViewModel
    {
        public string QuestionID { get; set; }
        public string Content { get; set; }
        public List<string> Options { get; set; }

        public int SelectedIndex { get; set; } = -1;

        public QuestionViewModel()
        {
            Options = new List<string>();
        }
    }
}
