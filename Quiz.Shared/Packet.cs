namespace Quiz.Shared
{
    public class Packet
    {
        public string Type { get; set; } // LOGIN, EXAM, RESULT, MSG
        public string Content { get; set; } // Dữ liệu JSON
        public string SessionID { get; set; } // Tên/MSSV học sinh
    }
}