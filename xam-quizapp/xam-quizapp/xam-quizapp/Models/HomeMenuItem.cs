namespace quizapp.Models
{
    public enum MenuItemType
    {
        Browse,
        Settings,
        StartAQuiz,
        RandomCategory,
        RandomQuestion,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
