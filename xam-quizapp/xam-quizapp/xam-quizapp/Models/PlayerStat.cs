using SQLite;

namespace quizapp.Models
{
    public class PlayerStats
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
