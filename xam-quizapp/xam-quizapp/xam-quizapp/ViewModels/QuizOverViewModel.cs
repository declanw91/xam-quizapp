using quizapp.DbControllers;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;
using quizapp.Models;
using System;

namespace quizapp.ViewModels
{
    public class QuizOverViewModel :BaseViewModel
    {
        private INavigation _navigation;
        private int _totalQuestions;
        private int _userScore;
        private string _scorePercentage;
        private string _scoreMessage;
        private string _quizsPlayed;
        private Command _closeCommand;
        private IPlayerStatsDbController _playerStatDbController;
        public QuizOverViewModel(INavigation nav)
        {
            _navigation = nav;
            _playerStatDbController = StartUp.ServiceProvider.GetService<IPlayerStatsDbController>();
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

        public string QuizsPlayed
        {
            get => _quizsPlayed;
            set
            {
                _quizsPlayed = value;
                OnPropertyChanged("QuizsPlayed");
            }
        }

        public Command CloseQuizOver => _closeCommand ?? (_closeCommand = new Command(CloseQuizOverScreen));

        private void CalculateScorePercentage()
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

        private void BuildScoreMesssage()
        {
            ScoreMessage = $"You score {UserScore} out of {TotalQuestions}";
        }

        public void SetupPageOptions()
        {
            CalculateScorePercentage();
            BuildScoreMesssage();
            UpdateQuizsPlayed();
        }

        private async void UpdateQuizsPlayed()
        {
            var stat = await _playerStatDbController.GetPlayerStat("quizsplayed");
            if(stat != null)
            {
                var quizsPlayed = Int32.Parse(stat.Value);
                quizsPlayed++;
                stat.Value = quizsPlayed.ToString();
                await _playerStatDbController.UpdatePlayerStat(stat);
            } 
            else
            {
                stat = new PlayerStats { Key = "quizsplayed", Value = "1" };
                await _playerStatDbController.InsertPlayerStat(stat);
            }
            QuizsPlayed = stat.Value;
        }
    }
}
