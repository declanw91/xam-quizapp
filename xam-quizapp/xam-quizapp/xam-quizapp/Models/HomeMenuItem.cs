namespace quizapp.Models
{
    public enum MenuItemType
    {
        StartAQuiz,
        RandomCategory,
        RandomQuestion,
        About,
        Dashboard
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }

        public string Icon { get; set; }
    }
}
