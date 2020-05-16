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
        private string _scoreMessage;
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

        public string ScoreMessage
        {
            get => _scoreMessage;
            set
            {
                _scoreMessage = value;
                OnPropertyChanged("ScoreMessage");
            }
        }

        public Command CloseQuizOver => _closeCommand ?? (_closeCommand = new Command(CloseQuizOverScreen));

        public void CalculateScorePercentage()
        {
            if(UserScore <= 0 || TotalQuestions <= 0) 
            {
                ScorePercentage = "0%";
                return;
            }
            var scoreDec = (double)UserScore / TotalQuestions;
            var percentage = scoreDec * 100;
            ScorePercentage = $"{percentage}%";
        }

        private void CloseQuizOverScreen()
        {
            _navigation.PopToRootAsync();
        }

        public void BuildScoreMesssage()
        {
            ScoreMessage = $"You score {UserScore} out of {TotalQuestions}";
        }
    }
}
