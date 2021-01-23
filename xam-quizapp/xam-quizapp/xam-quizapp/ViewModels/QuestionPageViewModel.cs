using Acr.UserDialogs;
using Microsoft.Extensions.DependencyInjection;
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
        private IQuestionController _questionController;
        private INavigation _navigation;
        private QuizAnswer _userAnswer;
        private int _currentQuestionNumber;
        private int _userScore;
        private int _totalQuestions;
        private Command _submitAnswer;
        private List<QuizAnswer> _currentAnswerList;
        private bool _userCanSubmit;
        private Command _nextQuestion;
        private Command _backCommand;
        private float _quizProgress;
        public QuestionPageViewModel(INavigation nav)
        {
            _questionList = new List<QuizQuestion>();
            CurrentQuestionAnswers = new List<QuizAnswer>();
            _questionController = StartUp.ServiceProvider.GetService<IQuestionController>();
            _navigation = nav;
            _currentQuestionNumber = 1;
            _userScore = 0;
            _userCanSubmit = true;
            _quizProgress = 0;
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

        public QuizAnswer UserAnswer
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

        public int TotalQuestions
        {
            get => _totalQuestions;
            set
            {
                _totalQuestions = value;
                OnPropertyChanged("TotalQuestions");
            }
        }

        public List<QuizAnswer> CurrentQuestionAnswers
        {
            get => _currentAnswerList;
            set
            {
                _currentAnswerList = value;
                OnPropertyChanged("CurrentQuestionAnswers");
            }
        }

        public float QuizProgress
        {
            get => _quizProgress;
            set
            {
                _quizProgress = value;
                OnPropertyChanged("QuizProgress");
            }
        }

        public Command SubmitAnswer => _submitAnswer ?? (_submitAnswer = new Command(CheckAnswer, CanSubmit));
        private bool CanSubmit()
        {
            return UserAnswer != null && _userCanSubmit;
        }

        public Command NextQuestion => _nextQuestion ?? (_nextQuestion = new Command(LoadNextQuestion, CanLoadNextQuestion));
        public Command BackCommand => _backCommand ?? (_backCommand = new Command(CustomBackCommand));

        private bool CanLoadNextQuestion()
        {
            return UserAnswer != null && !_userCanSubmit;
        }

        public async void StartQuiz()
        {
            UserDialogs.Instance.ShowLoading("Loading...");
            _questionList = await GetQuizQuestions();
            if (_questionList != null && _questionList.Count > 0)
            {
                CurrentQuestion = _questionList.First();
                TotalQuestions = _questionList.Count;
                PopulateAnswerList();
                CalculateQuizProgress();
            }
            UserDialogs.Instance.HideLoading();
            if (_questionList.Count == 0)
            {
                await UserDialogs.Instance.AlertAsync("Sorry but we are unable to load questions for the selected options.\nPlease try again or try different options.", "Warning", "Ok");
                await _navigation.PopAsync();
            } 
            else if (_questionList == null)
            {
                await UserDialogs.Instance.AlertAsync("Sorry but we are unable to load questions for your quiz.\nPlease check your device's internet connection and try again.", "Warning", "Ok");
                await _navigation.PopAsync();
            }
        }

        private async Task<List<QuizQuestion>> GetQuizQuestions()
        {
            var category = Preferences.Get("QuizCategory", "");
            var difficulty = Preferences.Get("QuizDifficulty", "");
            var questionType = Preferences.Get("QuestionType", "");
            var questions = await _questionController.GetQuizQuestions(category, difficulty, questionType);
            return questions;
        }

        private void CheckAnswer()
        {
            _userCanSubmit = false;
            SubmitAnswer.ChangeCanExecute();
            if (UserAnswer.Description == CurrentQuestion.Correct_Answer)
            {
                _userScore++;
            }
            HighlightCorrectAnswer();
            NextQuestion.ChangeCanExecute();
        }

        private void LoadNextQuestion()
        {
            CurrentQuestionNumber++;
            var questionNumber = CurrentQuestionNumber - 1;
            if (questionNumber < TotalQuestions)
            {
                var nextQ = _questionList.ElementAt(questionNumber);
                if(nextQ != null)
                {
                    CurrentQuestion = nextQ;
                }
                PopulateAnswerList();
                CalculateQuizProgress();
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
            _userCanSubmit = true;
            NextQuestion.ChangeCanExecute();
        }

        private void QuizOver()
        {
            var quizOverScreen = new QuizOver();
            var destVm = quizOverScreen.BindingContext as QuizOverViewModel;
            destVm.TotalQuestions = TotalQuestions;
            destVm.UserScore = _userScore;
            destVm.CategoryPlayed = Preferences.Get("QuizCategory", "");
            destVm.QuizModePlayed = Preferences.Get("QuestionType", "");
            _navigation.PushAsync(quizOverScreen);
        }

        private void PopulateAnswerList()
        {
            var answerList = new List<QuizAnswer>();
            foreach (var item in CurrentQuestion.Answers)
            {
                var answerItem = new QuizAnswer { Description=item };
                answerList.Add(answerItem);
            }
            CurrentQuestionAnswers = answerList;
        }

        private void HighlightCorrectAnswer()
        {
            var answerList = new List<QuizAnswer>();
            foreach (var item in CurrentQuestion.Answers)
            {
                var answerItem = new QuizAnswer { Description = item };
                if(item == CurrentQuestion.Correct_Answer)
                {
                    answerItem.Status = "correct";
                }
                if(item == UserAnswer.Description && item != CurrentQuestion.Correct_Answer)
                {
                    answerItem.Status = "incorrect";
                }
                answerList.Add(answerItem);
            }
            CurrentQuestionAnswers = answerList;
        }

        public void HighlightUsersAnswer()
        {
            var answerList = new List<QuizAnswer>();
            foreach (var item in CurrentQuestion.Answers)
            {
                var answerItem = new QuizAnswer { Description = item };
                if (item == UserAnswer.Description)
                {
                    answerItem.Status = "useranswer";
                }
                answerList.Add(answerItem);
            }
            CurrentQuestionAnswers = answerList;
        }

        private void CalculateQuizProgress()
        {
            QuizProgress = (float)CurrentQuestionNumber / TotalQuestions;
        }

        private async void CustomBackCommand()
        {
            if(TotalQuestions > 0)
            {
                var result = await UserDialogs.Instance.ConfirmAsync("Going back will lose quiz progress. Are you sure you want to go back?", "Warning", "Ok");
                if(!result)
                {
                    return;
                }
            }
            await _navigation.PopAsync();
        }
    }
}
