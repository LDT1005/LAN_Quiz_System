using System.Collections.Generic;

namespace Quiz.Client.Model
{
    public partial class ExamSession
    {
        public string ExamId { get; set; }
        public string CandidateId { get; set; }
        public int DurationSeconds { get; set; }

        // Sửa lỗi khởi tạo new() để tương thích mọi phiên bản
        public List<Question> Questions { get; set; } = new List<Question>();
    }
}
