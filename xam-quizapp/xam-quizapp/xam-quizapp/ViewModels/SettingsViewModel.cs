using quizapp.Controllers;
using System.Collections.Generic;
using System.Linq;

namespace quizapp.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private List<string> _quizCategories;
        private List<string> _quizDifficulties;
        private string _selectedCategory;
        private string _selectedDifficulty;
        private CategoryController _categoryController;
        public SettingsViewModel()
        {
            _categoryController = new CategoryController();
            QuizDifficulties = new List<string>();
            QuizCategories = new List<string>();
            PopulateQuizCategories();
            PopulateQuizDifficulties();
        }

        public List<string> QuizCategories
        {
            get => _quizCategories;
            set
            {
                _quizCategories = value;
                OnPropertyChanged("QuizCategories");
            }
        }

        public List<string> QuizDifficulties
        {
            get => _quizDifficulties;
            set
            {
                _quizDifficulties = value;
                OnPropertyChanged("QuizDifficulties");
            }
        }

        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged("SelectedCategory");
            }
        }

        public string SelectedDifficulty
        {
            get => _selectedDifficulty;
            set
            {
                _selectedDifficulty = value;
                OnPropertyChanged("SelectedDifficulty");
            }
        }

        private async void PopulateQuizCategories()
        {
            var quizCategories = await _categoryController.GetQuizCategories();
            if(quizCategories != null)
            {
                QuizCategories.Clear();
                foreach (var item in quizCategories)
                {
                    QuizCategories = quizCategories.Select(c => c.Name).ToList();
                }
            }
        }

        private void PopulateQuizDifficulties()
        {
            QuizDifficulties.Clear();
            QuizDifficulties.Add("Easy");
            QuizDifficulties.Add("Medium");
            QuizDifficulties.Add("Hard");
        }
    }
}
