using System;
using System.Collections.Generic;

namespace Quiz.Shared
{
    [Serializable]
    public class Question
    {
        public string Content { get; set; }
        public List<string> Options { get; set; }
        public int CorrectIndex { get; set; }
    }
}