using System;
using System.Collections.Generic;

namespace Quiz.Shared
{
    [Serializable]
    public class ExamData
    {
        public string ExamTitle { get; set; }          // Tiêu đề bài thi
        public int DurationMinutes { get; set; }       // Thời gian làm bài (phút)
        public DateTime StartTime { get; set; }        // Thời điểm bắt đầu
        public List<Question> Questions { get; set; }  // Danh sách câu hỏi (KHÔNG có CorrectIndex)

        public ExamData()
        {
            Questions = new List<Question>();
            StartTime = DateTime.Now;
        }

        // Tạo bản copy KHÔNG có đáp án đúng (gửi cho Client)
        public ExamData CreateClientVersion()
        {
            var clientExam = new ExamData
            {
                ExamTitle = this.ExamTitle,
                DurationMinutes = this.DurationMinutes,
                StartTime = this.StartTime,
                Questions = new List<Question>()
            };

            // Copy câu hỏi nhưng ẩn CorrectIndex
            foreach (var q in this.Questions)
            {
                clientExam.Questions.Add(new Question
                {
                    ID = q.ID,
                    Content = q.Content,
                    Options = new List<string>(q.Options), // Deep copy
                    CorrectIndex = -1, // Ẩn đáp án đúng
                    Points = q.Points
                });
            }

            return clientExam;
        }
    }
}