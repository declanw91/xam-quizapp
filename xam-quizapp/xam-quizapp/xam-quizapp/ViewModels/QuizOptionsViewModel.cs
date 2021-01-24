using Acr.UserDialogs;
using Microsoft.Extensions.DependencyInjection;
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
        private ICategoryController _categoryController;
        private IDifficultyController _difficultyController;
        private IQuestionTypeController _questiontypeController;
        private INavigation _navigation;
        private Command _saveCommand;
        private string _quizMode;
        private bool _categorySelectable;
        private List<string> _questionTypes;
        private string _selectedQuestionType;
        public QuizOptionsViewModel(INavigation nav)
        {
            _categoryController = StartUp.ServiceProvider.GetService<ICategoryController>();
            _difficultyController = StartUp.ServiceProvider.GetService<IDifficultyController>();
            _questiontypeController = StartUp.ServiceProvider.GetService<IQuestionTypeController>();
            QuizDifficulties = new List<string>();
            QuizCategories = new List<string>();
            QuestionTypes = new List<string>();
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

        public List<string> QuestionTypes
        {
            get => _questionTypes;
            set
            {
                _questionTypes = value;
                OnPropertyChanged("QuestionTypes");
            }
        }

        public string SelectedQuestionType
        {
            get => _selectedQuestionType;
            set
            {
                _selectedQuestionType = value;
                OnPropertyChanged("SelectedQuestionType");
                SaveSettings.ChangeCanExecute();
            }
        }

        public Command SaveSettings => _saveCommand ?? (_saveCommand = new Command(SaveUserSettings, CanSave));

        public bool CanSave()
        {
            return SelectedDifficulty != null && CategorySelected() && SelectedQuestionType != null;
        }

        public async Task SetupPageOptions()
        {
            UserDialogs.Instance.ShowLoading("Loading...");
            PopulateQuizDifficulties();
            PopulateQuestionTypes();
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

        private void PopulateQuestionTypes()
        {
            var questionTypes = _questiontypeController.GetQuestionTypeOptions();
            if (questionTypes != null)
            {
                QuestionTypes.Clear();
                QuestionTypes = questionTypes;
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
            Preferences.Set("QuestionType", SelectedQuestionType);
            var questionPage = new QuestionPage();
            _navigation.PushAsync(questionPage);
        }

        private async Task CheckQuizMode()
        {
            var selectedCategory = SelectedCategory;
            if(QuizMode == "RC" || QuizMode == null)
            {
                await PopulateQuizCategories();
                if(QuizCategories.Count > 0 && QuizMode == "RC")
                {
                    SelectRandomCategory();
                }
                if(!String.IsNullOrWhiteSpace(selectedCategory))
                {
                    SelectedCategory = selectedCategory;
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
