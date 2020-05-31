using quizapp.DbControllers;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using SkiaSharp;
using quizapp.Models;

namespace quizapp.ViewModels
{
    public class PlayerScoresViewModel : BaseViewModel
    {
        private IPlayerStatsDbController _playerStatsDbController;
        private List<PlayerStats> _playerStats;
        private int _totalQuizsPlayed;
        public PlayerScoresViewModel()
        {
            _playerStatsDbController = StartUp.ServiceProvider.GetService<IPlayerStatsDbController>();
        }

        public int TotalQuizsPlayed
        {
            get => _totalQuizsPlayed;
            set
            {
                _totalQuizsPlayed = value;
                OnPropertyChanged("TotalQuizsPlayed");
            }
        }

        public async Task SetupPlayerScoresData()
        {
            await GetAllPlayerStats();
            PopulateTotalQuizsPlayed();
        }

        private async Task GetAllPlayerStats()
        {
            var stats = await _playerStatsDbController.GetAllPlayerStats();
            if(stats != null)
            {
                _playerStats = stats;
            }
        }

        public List<Microcharts.Entry> GetPlayerAnswerStatEntries()
        {
            var chartEntries = new List<Microcharts.Entry>();
            if(_playerStats != null)
            {
                var correctAnswers = _playerStats.FirstOrDefault(s => s.Key == "TotalCorrectAnswers");
                if(correctAnswers != null)
                {
                    var correctAnswerEntry = new Microcharts.Entry(float.Parse(correctAnswers.Value)) { Color = SKColor.Parse("#00FF00"), Label="Correct", ValueLabel = correctAnswers.Value };
                    chartEntries.Add(correctAnswerEntry);
                }
                var incorrectAnswers = _playerStats.FirstOrDefault(s => s.Key == "TotalIncorrectAnswers");
                if (incorrectAnswers != null)
                {
                    var incorrectAnswerEntry = new Microcharts.Entry(float.Parse(incorrectAnswers.Value)) { Color = SKColor.Parse("#FF0000"), Label = "Incorrect", ValueLabel = incorrectAnswers.Value };
                    chartEntries.Add(incorrectAnswerEntry);
                }
            }
            return chartEntries;
        }

        private void PopulateTotalQuizsPlayed()
        {
            var quizsPlayed = _playerStats.FirstOrDefault(s => s.Key == "QuizsPlayed");
            if(quizsPlayed != null)
            {
                TotalQuizsPlayed = int.Parse(quizsPlayed.Value);
            }
        }
    }
}
