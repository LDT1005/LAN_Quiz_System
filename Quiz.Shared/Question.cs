using System;
using System.Collections.Generic;

namespace Quiz.Shared
{
    [Serializable]
    public class Question
    {
        public string ID { get; set; }              // VD: "Q001", "Q002"
        public string Content { get; set; }         // Nội dung câu hỏi
        public List<string> Options { get; set; }   // Danh sách đáp án
        public int CorrectIndex { get; set; }       // Index của đáp án đúng (0-based)
        public double Points { get; set; }          // Điểm của câu hỏi (mặc định 1.0)

        // Constructor mặc định
        public Question()
        {
            Options = new List<string>();
            Points = 1.0;
        }

        // Constructor đầy đủ
        public Question(string id, string content, List<string> options, int correctIndex, double points = 1.0)
        {
            ID = id;
            Content = content;
            Options = options;
            CorrectIndex = correctIndex;
            Points = points;
        }

        // Validate câu hỏi hợp lệ
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(ID) &&
                   !string.IsNullOrEmpty(Content) &&
                   Options != null &&
                   Options.Count >= 2 &&
                   CorrectIndex >= 0 &&
                   CorrectIndex < Options.Count;
        }
    }
}