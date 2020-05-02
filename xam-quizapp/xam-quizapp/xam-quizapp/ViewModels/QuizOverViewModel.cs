using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace quizapp.ViewModels
{
    public class QuizOverViewModel :BaseViewModel
    {
        private INavigation _navigation;
        private int _totalQuestions;
        private int _userScore;
        private string _scorePercentage;
        private Command _closeCommand;
        public QuizOverViewModel(INavigation nav)
        {
            _navigation = nav;
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

        public int UserScore
        {
            get => _userScore;
            set
            {
                _userScore = value;
                OnPropertyChanged("UserScore");
            }
        }

        public string ScorePercentage
        {
            get => _scorePercentage;
            set
            {
                _scorePercentage = value;
                OnPropertyChanged("ScorePercentage");
            }
        }

        public Command CloseQuizOver => _closeCommand ?? (_closeCommand = new Command(CloseQuizOverScreen));

        public string CalculateScorePercentage(int score, int totalQuestions)
        {
            if(score <= 0 || totalQuestions <= 0) 
            {
                return String.Empty;
            }
            var scoreDec = (double)score / totalQuestions;
            var percentage = scoreDec * 100;
            return $"{percentage}%";
        }

        private void CloseQuizOverScreen()
        {
            _navigation.PopToRootAsync();
        }
    }
}
