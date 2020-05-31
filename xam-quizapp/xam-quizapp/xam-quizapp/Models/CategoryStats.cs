using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace quizapp.Models
{
    public class CategoryStats
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int CategoryStatId { get; set; }
        public string CategoryName { get; set; }
        public int TimesPlayed { get; set; }
        public int CorrectAnswers { get; set; }
        public int IncorrectAnswers { get; set; }
    }
}
