using System;
using System.Collections.Generic;

namespace Quiz.Shared
{
    [Serializable]
    public class ExamResult
    {
        public string StudentID { get; set; }
        public string StudentName { get; set; }
        public double TotalScore { get; set; }
        public double MaxScore { get; set; }
        public int CorrectCount { get; set; }
        public int TotalQuestions { get; set; }
        public DateTime SubmitTime { get; set; }

        public List<QuestionResult> Details { get; set; } = new List<QuestionResult>();
    }

    [Serializable]
    public class QuestionResult
    {
        public string QuestionID { get; set; }
        public int SelectedAnswer { get; set; }
        public int CorrectAnswer { get; set; }
        public bool IsCorrect { get; set; }
    }
}
