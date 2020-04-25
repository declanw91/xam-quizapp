using quizapp.Controllers;
using quizapp.Models;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace quizapp.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private List<string> _quizCategories;
        private List<QuizCategory> _quizCategoryList;
        private List<string> _quizDifficulties;
        private string _selectedCategory;
        private string _selectedDifficulty;
        private CategoryController _categoryController;
        private Command _saveCommand;
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

        public Command SaveSettings => _saveCommand ?? (_saveCommand = new Command(SaveUserSettings));

        private async void PopulateQuizCategories()
        {
            var quizCategories = await _categoryController.GetQuizCategories();
            if(quizCategories != null)
            {
                QuizCategories.Clear();
                _quizCategoryList = quizCategories;
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

        private void SaveUserSettings()
        {
            var selectedCategory = _quizCategoryList.FirstOrDefault(c => c.Name == SelectedCategory);
            if(selectedCategory != null)
            {
                Preferences.Set("QuizCategory", selectedCategory.Id);
            }
            Preferences.Set("QuizDifficulty", SelectedDifficulty);
        }
    }
}
