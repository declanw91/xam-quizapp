using Acr.UserDialogs;
using quizapp.Controllers;
using quizapp.Models;
using quizapp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace quizapp.ViewModels
{
    public class QuizOptionsViewModel : BaseViewModel
    {
        private List<string> _quizCategories;
        private List<QuizCategory> _quizCategoryList;
        private List<string> _quizDifficulties;
        private string _selectedCategory;
        private string _selectedDifficulty;
        private CategoryController _categoryController;
        private DifficultyController _difficultyController;
        private INavigation _navigation;
        private Command _saveCommand;
        public QuizOptionsViewModel(INavigation nav)
        {
            _categoryController = new CategoryController();
            _difficultyController = new DifficultyController();
            QuizDifficulties = new List<string>();
            QuizCategories = new List<string>();
            _navigation = nav;
            SetupPageOptions();
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
                SaveSettings.ChangeCanExecute();
            }
        }

        public string SelectedDifficulty
        {
            get => _selectedDifficulty;
            set
            {
                _selectedDifficulty = value;
                OnPropertyChanged("SelectedDifficulty");
                SaveSettings.ChangeCanExecute();
            }
        }

        public Command SaveSettings => _saveCommand ?? (_saveCommand = new Command(SaveUserSettings, CanSave));

        public bool CanSave()
        {
            return SelectedDifficulty != null && SelectedCategory != null;
        }

        public async void SetupPageOptions()
        {
            UserDialogs.Instance.ShowLoading("Loading...");
            await PopulateQuizCategories();
            await PopulateQuizDifficulties();
            UserDialogs.Instance.HideLoading();
        }
        private async Task PopulateQuizCategories()
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

        private async Task PopulateQuizDifficulties()
        {
            var quizDifficulties = await _difficultyController.GetQuizDifficulties();
            if(quizDifficulties != null)
            {
                QuizDifficulties.Clear();
                QuizDifficulties = quizDifficulties;
            }
        }

        private void SaveUserSettings()
        {
            var selectedCategory = _quizCategoryList.FirstOrDefault(c => c.Name == SelectedCategory);
            if(selectedCategory != null)
            {
                Preferences.Set("QuizCategory", selectedCategory.Id);
            }
            Preferences.Set("QuizDifficulty", SelectedDifficulty);
            var questionPage = new QuestionPage();
            _navigation.PushAsync(questionPage);
        }
    }
}
