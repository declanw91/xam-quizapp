namespace quizapp.Models
{
    public enum MenuItemType
    {
        Browse,
        Settings
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
