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
        private Command _submitAnswer;
        public QuestionPageViewModel(INavigation nav)
        {
            _questionList = new List<QuizQuestion>();
            _questionController = new QuestionController();
            _quizOptionsConfirmed = false;
            _navigation = nav;
            _currentQuestionNumber = 0;
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

        private void CheckAnswer()
        {
            if(UserAnswer == CurrentQuestion.Correct_Answer)
            {
                
            }
            else
            {

            }
            LoadNextQuestion();
        }

        private void LoadNextQuestion()
        {
            if(_currentQuestionNumber < _questionList.Count)
            {
                _currentQuestionNumber++;
                var nextQ = _questionList.ElementAt(_currentQuestionNumber);
                if(nextQ != null)
                {
                    CurrentQuestion = nextQ;
                }
                Reset();
            }
        }
        private void Reset()
        {
            UserAnswer = null;
        }
    }
}
