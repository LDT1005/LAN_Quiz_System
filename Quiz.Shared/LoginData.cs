using System;

namespace Quiz.Shared
{
    [Serializable]
    public class LoginData
    {
        public string StudentID { get; set; }      // MSSV
        public string StudentName { get; set; }    // Họ tên
        public string ClientIP { get; set; }       // IP máy client (tự động lấy)

        public LoginData() { }

        public LoginData(string studentID, string studentName, string clientIP = "")
        {
            StudentID = studentID;
            StudentName = studentName;
            ClientIP = clientIP;
        }

        // Validate dữ liệu
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(StudentID) &&
                   !string.IsNullOrWhiteSpace(StudentName);
        }
    }
}