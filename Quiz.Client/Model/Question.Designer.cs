namespace Quiz.Client.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string[] Options { get; set; } // A-D
        public int? SelectedIndex { get; set; } // 0..3
    }
}
