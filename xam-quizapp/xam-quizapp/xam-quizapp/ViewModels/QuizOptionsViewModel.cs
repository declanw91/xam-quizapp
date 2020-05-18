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
        private string _quizMode;
        private bool _categorySelectable;
        public QuizOptionsViewModel(INavigation nav)
        {
            _categoryController = new CategoryController();
            _difficultyController = new DifficultyController();
            QuizDifficulties = new List<string>();
            QuizCategories = new List<string>();
            _navigation = nav;
            CategorySelectable = true;
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

        public string QuizMode
        {
            get => _quizMode;
            set
            {
                _quizMode = value;
                OnPropertyChanged("QuizMode");
            }
        }

        public bool CategorySelectable
        {
            get => _categorySelectable;
            set
            {
                _categorySelectable = value;
                OnPropertyChanged("CategorySelectable");
            }
        }

        public Command SaveSettings => _saveCommand ?? (_saveCommand = new Command(SaveUserSettings, CanSave));

        public bool CanSave()
        {
            return SelectedDifficulty != null && CategorySelected();
        }

        public async Task SetupPageOptions()
        {
            UserDialogs.Instance.ShowLoading("Loading...");
            PopulateQuizDifficulties();
            await CheckQuizMode();
            UserDialogs.Instance.HideLoading();
        }
        private async Task PopulateQuizCategories()
        {
            var quizCategories = await _categoryController.GetQuizCategories();
            if(quizCategories != null && quizCategories.Count > 0)
            {
                QuizCategories.Clear();
                _quizCategoryList = quizCategories;
                foreach (var item in quizCategories)
                {
                    QuizCategories = quizCategories.Select(c => c.Name).ToList();
                }
            }
            else
            {
                await UserDialogs.Instance.AlertAsync("Sorry but we are unable to load the categories.\nPlease check your device internet connection and try again.", "Warning", "Ok");
            }
        }

        private void PopulateQuizDifficulties()
        {
            var quizDifficulties = _difficultyController.GetQuizDifficulties();
            if(quizDifficulties != null)
            {
                QuizDifficulties.Clear();
                QuizDifficulties = quizDifficulties;
            }
        }

        private void SaveUserSettings()
        {
            if(QuizMode != "RQ")
            {
                var selectedCategory = _quizCategoryList.FirstOrDefault(c => c.Name == SelectedCategory);
                if (selectedCategory != null)
                {
                    Preferences.Set("QuizCategory", selectedCategory.Id);
                }
            }
            Preferences.Set("QuizDifficulty", SelectedDifficulty);
            var questionPage = new QuestionPage();
            _navigation.PushAsync(questionPage);
        }

        private async Task CheckQuizMode()
        {
            if(QuizMode == "RC" || QuizMode == null)
            {
                await PopulateQuizCategories();
                if(QuizCategories.Count > 0 && QuizMode == "RC")
                {
                    SelectRandomCategory();
                }
            }
            else if (QuizMode == "RQ")
            {
                SelectRandomQuestions();
            }
        }

        private void SelectRandomCategory()
        {
            Random random = new Random();
            var randomNumber = random.Next(0, QuizCategories.Count);
            SelectedCategory = QuizCategories.ElementAt(randomNumber);
            CategorySelectable = false;
        }

        private void SelectRandomQuestions()
        {
            CategorySelectable = false;
        }

        private bool CategorySelected()
        {
            return SelectedCategory != null || !CategorySelectable;
        }
    }
}
