using quizapp.Controllers;
using quizapp.Models;
using System.Collections.Generic;
using Xamarin.Essentials;

namespace quizapp.ViewModels
{
    public class QuestionPageViewModel : BaseViewModel
    {
        private List<QuizQuestion> _questionList;
        private QuizQuestion _currentQuestion;
        private QuestionController _questionController;
        public QuestionPageViewModel()
        {
            _questionList = new List<QuizQuestion>();
            _questionController = new QuestionController();
            GetQuizQuestions();
        }

        private async void GetQuizQuestions()
        {
            var category = Preferences.Get("QuizCategory", "");
            var difficulty = Preferences.Get("QuizDifficulty", "");
            var questions = await _questionController.GetQuizQuestions(category, difficulty);
        }
    }
}
