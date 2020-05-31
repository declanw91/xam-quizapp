using quizapp.DbControllers;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using SkiaSharp;
using quizapp.Models;
using System;
using quizapp.Controllers;

namespace quizapp.ViewModels
{
    public class PlayerScoresViewModel : BaseViewModel
    {
        private IPlayerStatsDbController _playerStatsDbController;
        private ICategoryStatsDbController _categoryStatsDbController;
        private ICategoryController _categoryController;
        private List<PlayerStats> _playerStats;
        private List<CategoryStats> _categoryStats;
        private List<QuizCategory> _quizCategoryList;
        private int _totalQuizsPlayed;
        private int _totalCorrectAnswers;
        private int _totalIncorrectAnswers;
        public PlayerScoresViewModel()
        {
            _playerStatsDbController = StartUp.ServiceProvider.GetService<IPlayerStatsDbController>();
            _categoryStatsDbController = StartUp.ServiceProvider.GetService<ICategoryStatsDbController>();
            _categoryController = StartUp.ServiceProvider.GetService<ICategoryController>();
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

        public int TotalCorrectAnswers
        {
            get => _totalCorrectAnswers;
            set
            {
                _totalCorrectAnswers = value;
                OnPropertyChanged("TotalCorrectAnswers");
            }
        }

        public int TotalIncorrectAnswers
        {
            get => _totalIncorrectAnswers;
            set
            {
                _totalIncorrectAnswers = value;
                OnPropertyChanged("TotalIncorrectAnswers");
            }
        }

        public async Task SetupPlayerScoresData()
        {
            await GetAllPlayerStats();
            await GetAllCategoryStats();
            await GetAllCategories();
            PopulateTotals();
        }

        private async Task GetAllPlayerStats()
        {
            var stats = await _playerStatsDbController.GetAllPlayerStats();
            if(stats != null)
            {
                _playerStats = stats;
            }
        }

        private async Task GetAllCategoryStats()
        {
            var stats = await _categoryStatsDbController.GetAllCategoryStats();
            if (stats != null)
            {
                _categoryStats = stats;
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

        public List<Microcharts.Entry> GetCategoryPlayedStatEntries()
        {
            var chartEntries = new List<Microcharts.Entry>();
            var random = new Random();
            
            if (_categoryStats != null)
            {
                foreach(var item in _categoryStats)
                {
                    var color = String.Format("#{0:X6}", random.Next(0x1000000));
                    var cat = LookupCategory(item.CategoryName);
                    var entry = new Microcharts.Entry((float)item.TimesPlayed) { Label = cat.Name, ValueLabel = item.TimesPlayed.ToString(), Color = SKColor.Parse(color)};
                    chartEntries.Add(entry);
                }
            }
            return chartEntries;
        }

        private void PopulateTotals()
        {
            var quizsPlayed = _playerStats.FirstOrDefault(s => s.Key == "QuizsPlayed");
            if(quizsPlayed != null)
            {
                TotalQuizsPlayed = int.Parse(quizsPlayed.Value);
            }
            var correctAnswers = _playerStats.FirstOrDefault(s => s.Key == "TotalCorrectAnswers");
            if (correctAnswers != null)
            {
                TotalCorrectAnswers = int.Parse(correctAnswers.Value);
            }
            var incorrectAnswers = _playerStats.FirstOrDefault(s => s.Key == "TotalIncorrectAnswers");
            if (incorrectAnswers != null)
            {
                TotalIncorrectAnswers = int.Parse(incorrectAnswers.Value);
            }
        }

        private async Task GetAllCategories()
        {
            var categories = await _categoryController.GetQuizCategories();
            if (categories != null)
            {
                _quizCategoryList = categories;
            }
        }

        private QuizCategory LookupCategory(string id)
        {
            return _quizCategoryList.FirstOrDefault(c => c.Id.ToLowerInvariant() == id.ToLowerInvariant());
        }
    }
}
