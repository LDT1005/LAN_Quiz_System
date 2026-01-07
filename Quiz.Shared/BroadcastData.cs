using Quiz.Shared;
using System;

namespace Quiz.Shared
{
    [Serializable]
    public class BroadcastData
    {
        public string Message { get; set; }
        public string Priority { get; set; }  // LOW, NORMAL, HIGH, URGENT
        public DateTime Timestamp { get; set; }

        public BroadcastData()
        {
            Priority = "NORMAL";
            Timestamp = DateTime.Now;
        }

        public BroadcastData(string message, string priority = "NORMAL")
        {
            Message = message;
            Priority = priority;
            Timestamp = DateTime.Now;
        }
    }
}
