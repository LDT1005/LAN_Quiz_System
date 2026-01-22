using System;
using System.Collections.Generic;

namespace Quiz.Shared
{
    public class ExamSession
    {
        public string ExamTitle { get; set; }
        public DateTime StartTime { get; set; }
        public int DurationMinutes { get; set; }
        public List<Question> Questions { get; set; }
    }
}
