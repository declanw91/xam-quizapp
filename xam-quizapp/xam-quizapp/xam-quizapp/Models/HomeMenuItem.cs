namespace quizapp.Models
{
    public enum MenuItemType
    {
        StartAQuiz,
        RandomCategory,
        RandomQuestion
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
