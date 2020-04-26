using System;

namespace quizapp.Models
{
    public class QuizQuestion
    {
        public string Category { get; set; }
        public string Type { get; set; }
        public string Difficulty { get; set; }
        public string Question { get; set; }
        public string Correct_Answer { get; set; }
        public Array Incorrect_Answers { get; set; }
    }
}
