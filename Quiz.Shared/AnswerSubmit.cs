using System;
using System.Collections.Generic;

namespace Quiz.Shared
{
    [Serializable]
    public class AnswerSubmit
    {
        public string StudentID { get; set; }
        public Dictionary<string, int> Answers { get; set; } = new Dictionary<string, int>();
        public DateTime SubmitTime { get; set; }
    }
}