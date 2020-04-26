namespace quizapp.Models
{
    public enum MenuItemType
    {
        Browse,
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
