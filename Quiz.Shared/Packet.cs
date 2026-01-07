using System;

namespace Quiz.Shared
{
    [Serializable]
    public class Packet
    {
        // Các loại packet trong hệ thống
        public const string TYPE_LOGIN = "LOGIN";
        public const string TYPE_LOGIN_SUCCESS = "LOGIN_SUCCESS";
        public const string TYPE_LOGIN_FAILED = "LOGIN_FAILED";
        public const string TYPE_START_EXAM = "START_EXAM";
        public const string TYPE_SUBMIT = "SUBMIT";
        public const string TYPE_RESULT = "RESULT";
        public const string TYPE_BROADCAST = "BROADCAST";
        public const string TYPE_DISCONNECT = "DISCONNECT";
        public const string TYPE_RECONNECT = "RECONNECT";

        public string Type { get; set; }           // Loại packet (dùng constants ở trên)
        public string SessionID { get; set; }      // MSSV hoặc ID định danh client
        public DateTime Timestamp { get; set; }    // Thời gian tạo packet
        public object Data { get; set; }           // Dữ liệu chính (object để serialize thành JSON)

        // Constructor mặc định
        public Packet()
        {
            Timestamp = DateTime.Now;
        }

        // Constructor đầy đủ
        public Packet(string type, string sessionID, object data)
        {
            Type = type;
            SessionID = sessionID;
            Data = data;
            Timestamp = DateTime.Now;
        }

        // Validate packet cơ bản
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Type) && !string.IsNullOrEmpty(SessionID);
        }
    }
}