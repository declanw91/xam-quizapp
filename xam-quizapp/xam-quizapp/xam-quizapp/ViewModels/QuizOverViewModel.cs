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
        private string _categoryPlayed;
        private string _quizModePlayed;
        private Command _closeCommand;
        private IPlayerStatsDbController _playerStatDbController;
        private ICategoryStatsDbController _categoryStatDbController;
        public QuizOverViewModel(INavigation nav)
        {
            _navigation = nav;
            _playerStatDbController = StartUp.ServiceProvider.GetService<IPlayerStatsDbController>();
            _categoryStatDbController = StartUp.ServiceProvider.GetService<ICategoryStatsDbController>();
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

        public string CategoryPlayed
        {
            get => _categoryPlayed;
            set
            {
                _categoryPlayed = value;
                OnPropertyChanged("CategoryPlayed");
            }
        }

        public string QuizModePlayed
        {
            get => _quizModePlayed;
            set
            {
                _quizModePlayed = value;
                OnPropertyChanged("QuizModePlayed");
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
            UpdateTotalCorrectAnswers();
            UpdateTotalIncorrectAnswers();
            UpdateCategoryStat();
            UpdateQuizModePlayed();
        }

        private async void UpdateQuizsPlayed()
        {
            var stat = await _playerStatDbController.GetPlayerStat("QuizsPlayed");
            if(stat != null)
            {
                var quizsPlayed = int.Parse(stat.Value);
                quizsPlayed++;
                stat.Value = quizsPlayed.ToString();
                await _playerStatDbController.UpdatePlayerStat(stat);
                QuizsPlayed = stat.Value;
            } 
            else
            {
                InsertNewPlayerStat("QuizsPlayed", "1");
                QuizsPlayed = "1";
            }
            
        }

        private async void UpdateQuizModePlayed()
        {
            var key = "";
            if(QuizModePlayed.ToLowerInvariant() == "multiple choice")
            {
                key = "MultiChoicePlayed";
            }
            else if (QuizModePlayed.ToLowerInvariant() == "true or false")
            {
                key = "TrueFalsePlayed";
            }
            if(!String.IsNullOrWhiteSpace(key))
            {
                var stat = await _playerStatDbController.GetPlayerStat(key);
                if (stat != null)
                {
                    var quizsPlayed = int.Parse(stat.Value);
                    quizsPlayed++;
                    stat.Value = quizsPlayed.ToString();
                    await _playerStatDbController.UpdatePlayerStat(stat);
                    QuizsPlayed = stat.Value;
                }
                else
                {
                    InsertNewPlayerStat(key, "1");
                }
            }
        }

        private async void UpdateTotalCorrectAnswers()
        {
            var stat = await _playerStatDbController.GetPlayerStat("TotalCorrectAnswers");
            if (stat != null)
            {
                var totalCorrectAnswers = int.Parse(stat.Value);
                totalCorrectAnswers += UserScore;
                stat.Value = totalCorrectAnswers.ToString();
                await _playerStatDbController.UpdatePlayerStat(stat);
            }
            else
            {
                InsertNewPlayerStat("TotalCorrectAnswers", UserScore.ToString());
            }
        }

        private async void UpdateTotalIncorrectAnswers()
        {
            var stat = await _playerStatDbController.GetPlayerStat("TotalIncorrectAnswers");
            if (stat != null)
            {
                var totalIncorrectAnswers = int.Parse(stat.Value);
                totalIncorrectAnswers += (TotalQuestions - UserScore);
                stat.Value = totalIncorrectAnswers.ToString();
                await _playerStatDbController.UpdatePlayerStat(stat);
            }
            else
            {
                InsertNewPlayerStat("TotalIncorrectAnswers", (TotalQuestions - UserScore).ToString());
            }
        }

        private async void UpdateCategoryStat()
        {
            var stat = await _categoryStatDbController.GetCategoryStat(CategoryPlayed);
            if (stat != null)
            {
                UpdateCategoryPlayed(stat);
                UpdateTotalCorrectAnswersForCategory(stat);
                UpdateTotalIncorrectAnswersForCategory(stat);
                await _categoryStatDbController.UpdateCategoryStat(stat);
            } 
            else
            {
                InsertNewCategoryStat();
            }
            
        }

        private void UpdateCategoryPlayed(CategoryStats stat)
        {
            if (stat != null)
            {
                var categoryTimesPlayed = stat.TimesPlayed;
                categoryTimesPlayed++;
                stat.TimesPlayed = categoryTimesPlayed;
            }
        }

        private void UpdateTotalCorrectAnswersForCategory(CategoryStats stat)
        {
            if (stat != null)
            {
                var totalCorrectAnswers = stat.CorrectAnswers;
                totalCorrectAnswers += UserScore;
                stat.CorrectAnswers = totalCorrectAnswers;
            }
        }

        private void UpdateTotalIncorrectAnswersForCategory(CategoryStats stat)
        {
            if (stat != null)
            {
                var totalIncorrectAnswers = stat.IncorrectAnswers;
                totalIncorrectAnswers += (TotalQuestions - UserScore);
                stat.IncorrectAnswers = totalIncorrectAnswers;
            }
        }

        private async void InsertNewCategoryStat()
        {
            var stat = new CategoryStats { CategoryName = CategoryPlayed, TimesPlayed = 1, CorrectAnswers = UserScore, IncorrectAnswers = (TotalQuestions - UserScore) };
            await _categoryStatDbController.InsertCategoryStat(stat);
        }

        private async void InsertNewPlayerStat(string key, string value)
        {
            var stat = new PlayerStats { Key = key, Value = value };
            await _playerStatDbController.InsertPlayerStat(stat);
        }
    }
}
