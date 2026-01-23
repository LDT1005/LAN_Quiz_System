namespace Quiz.Client.Model
{
    public class Question
    {
        private System.Windows.Forms.Label lblQuestionText;
        private System.Windows.Forms.Panel pnlOptionA;
        private System.Windows.Forms.Panel pnlOptionB;
        private System.Windows.Forms.Panel pnlOptionC;
        private System.Windows.Forms.Panel pnlOptionD;
        private System.Windows.Forms.RadioButton rbA;
        private System.Windows.Forms.RadioButton rbC;
        private System.Windows.Forms.RadioButton rbB;
        private System.Windows.Forms.RadioButton rbD;
        private System.Windows.Forms.Label lblAnswerA;
        private System.Windows.Forms.Label lblAnswerB;
        private System.Windows.Forms.Label lblAnswerC;
        private System.Windows.Forms.Label lblAnswerD;

        public int Id { get; set; }
        public string Text { get; set; }
        public string[] Options { get; set; } // A-D
        public int? SelectedIndex { get; set; } // 0..3
    }
}
