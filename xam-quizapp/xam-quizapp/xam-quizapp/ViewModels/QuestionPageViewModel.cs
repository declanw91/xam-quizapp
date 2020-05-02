using quizapp.Controllers;
using quizapp.Models;
using quizapp.Views;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace quizapp.ViewModels
{
    public class QuestionPageViewModel : BaseViewModel
    {
        private List<QuizQuestion> _questionList;
        private QuizQuestion _currentQuestion;
        private QuestionController _questionController;
        private bool _quizOptionsConfirmed;
        private INavigation _navigation;
        private string _userAnswer;
        private int _currentQuestionNumber;
        private int _userScore;
        private Command _submitAnswer;
        public QuestionPageViewModel(INavigation nav)
        {
            _questionList = new List<QuizQuestion>();
            _questionController = new QuestionController();
            _quizOptionsConfirmed = false;
            _navigation = nav;
            _currentQuestionNumber = 1;
            _userScore = 0;
        }

        public QuizQuestion CurrentQuestion
        {
            get => _currentQuestion;
            set
            {
                _currentQuestion = value;
                OnPropertyChanged("CurrentQuestion");
            }
        }

        public string UserAnswer
        {
            get => _userAnswer;
            set
            {
                _userAnswer = value;
                OnPropertyChanged("UserAnswer");
                SubmitAnswer.ChangeCanExecute();
            }
        }

        public int CurrentQuestionNumber
        {
            get => _currentQuestionNumber;
            set
            {
                _currentQuestionNumber = value;
                OnPropertyChanged("CurrentQuestionNumber");
            }
        }

        public Command SubmitAnswer => _submitAnswer ?? (_submitAnswer = new Command(CheckAnswer, CanSubmit));
        public bool CanSubmit()
        {
            return !string.IsNullOrWhiteSpace(UserAnswer);
        }

        public async void StartQuiz()
        {
            if(_quizOptionsConfirmed)
            {
                //start the quiz!
                _questionList = await GetQuizQuestions();
                if(_questionList != null && _questionList.Count > 0)
                {
                    CurrentQuestion = _questionList.First();
                }
            }
            else
            {
                _quizOptionsConfirmed = true;
                var settings = new SettingsPage();
                await _navigation.PushAsync(settings);
            }
        }

        private async Task<List<QuizQuestion>> GetQuizQuestions()
        {
            var category = Preferences.Get("QuizCategory", "");
            var difficulty = Preferences.Get("QuizDifficulty", "");
            var questions = await _questionController.GetQuizQuestions(category, difficulty);
            return questions;
        }

        private async void CheckAnswer()
        {
            if(UserAnswer == CurrentQuestion.Correct_Answer)
            {
                _userScore++;
                await App.Current.MainPage.DisplayAlert("Answer Correct", "You got the question correct!", "Ok");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Answer Incorrect", "Sorry that is incorrect", "Ok");
            }
            LoadNextQuestion();
        }

        private void LoadNextQuestion()
        {
            CurrentQuestionNumber++;
            var questionNumber = CurrentQuestionNumber - 1;
            if (questionNumber < _questionList.Count)
            {
                var nextQ = _questionList.ElementAt(questionNumber);
                if(nextQ != null)
                {
                    CurrentQuestion = nextQ;
                }
                Reset();
            }
            else
            {
                QuizOver();
            }
        }
        private void Reset()
        {
            UserAnswer = null;
        }

        private void QuizOver()
        {
            var quizOverScreen = new QuizOver();
            var destVm = quizOverScreen.BindingContext as QuizOverViewModel;
            destVm.TotalQuestions = _questionList.Count;
            destVm.UserScore = _userScore;
            destVm.ScorePercentage = destVm.CalculateScorePercentage(_userScore, destVm.TotalQuestions);
            _navigation.PushAsync(quizOverScreen);
        }
    }
}
