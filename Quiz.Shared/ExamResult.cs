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
        public TimeSpan Duration { get; set; }
        public bool IsAutoSubmit { get; set; }

        // Chi tiết từng câu
        public List<QuestionResult> Details { get; set; }

        public ExamResult()
        {
            Details = new List<QuestionResult>();
        }

        // Tính phần trăm
        public double Percentage => MaxScore > 0 ? (TotalScore / MaxScore) * 100 : 0;
    }

    [Serializable]
    public class QuestionResult
    {
        public string QuestionID { get; set; }
        public bool IsCorrect { get; set; }
        public int YourAnswer { get; set; }      // Index đáp án học sinh chọn
        public int CorrectAnswer { get; set; }   // Index đáp án đúng
        public double Points { get; set; }       // Điểm đạt được
    }
}