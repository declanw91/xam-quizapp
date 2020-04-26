using System;
using System.Collections.Generic;

namespace quizapp.Models
{
    public class QuizQuestion
    {
        public string Category { get; set; }
        public string Type { get; set; }
        public string Difficulty { get; set; }
        public string Question { get; set; }
        public string Correct_Answer { get; set; }
        public List<string> Incorrect_Answers { get; set; }
        public List<string> Answers { get; set; }
    }
}
