using System;

namespace Quiz.Shared
{
    [Serializable]
    public class AnswerSubmit
    {
        public string StudentID { get; set; }
        public string QuestionID { get; set; }
        public int SelectedAnswer { get; set; }
    }
}