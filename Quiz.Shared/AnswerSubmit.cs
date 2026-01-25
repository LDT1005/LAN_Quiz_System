using System;
using System.Collections.Generic;

namespace Quiz.Shared
{
    [Serializable]
    public class AnswerSubmit
    {
        public string StudentID { get; set; }
        public string QuestionID { get; set; } // Khả năng tương thích cũ
        public int SelectedAnswer { get; set; } // Khả năng tương thích cũ
        public string EncryptedData { get; set; } // Dữ liệu XOR bảo mật
        public string Checksum { get; set; }      // Xác thực SHA256
        public Dictionary<string, int> Answers { get; set; } = new Dictionary<string, int>();
        public DateTime SubmitTime { get; set; }
        public bool IsAutoSubmit { get; set; }

        public AnswerSubmit() => SubmitTime = DateTime.Now;
    }
}