using System;
using System.Collections.Generic;

namespace Quiz.Shared
{
    [Serializable]
    public class AnswerSubmit
    {
        public string StudentID { get; set; }

        // Cần thêm 2 dòng này để các Service phía Client hoạt động được
        public string QuestionID { get; set; }
        public int SelectedAnswer { get; set; }

        public Dictionary<string, int> Answers { get; set; } = new Dictionary<string, int>();
        public DateTime SubmitTime { get; set; }
        public bool IsAutoSubmit { get; set; }

        public AnswerSubmit() => SubmitTime = DateTime.Now;
    }
}