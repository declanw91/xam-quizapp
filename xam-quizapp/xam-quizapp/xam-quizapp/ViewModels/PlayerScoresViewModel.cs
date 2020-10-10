using quizapp.DbControllers;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using SkiaSharp;
using quizapp.Models;
using System;
using quizapp.Controllers;
using Microcharts;

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
        private List<CatChartLabel> _categoryChartLabels;
        private int _totalQuizsPlayed;
        private int _totalCorrectAnswers;
        private int _totalIncorrectAnswers;
        private Chart _categoryStatChart;
        private Chart _playerAnswerChart;
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

        public Chart CategoryStatChart
        {
            get => _categoryStatChart;
            set
            {
                _categoryStatChart = value;
                OnPropertyChanged("CategoryStatChart");
            }
        }

        public Chart PlayerAnswerChart
        {
            get => _playerAnswerChart;
            set
            {
                _playerAnswerChart = value;
                OnPropertyChanged("PlayerAnswerChart");
            }
        }

        public List<CatChartLabel> CategoryChartLabels
        {
            get => _categoryChartLabels;
            set
            {
                _categoryChartLabels = value;
                OnPropertyChanged("CategoryChartLabels");
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

        public List<Microcharts.ChartEntry> GetPlayerAnswerStatEntries()
        {
            var chartEntries = new List<Microcharts.ChartEntry>();
            if(_playerStats != null)
            {
                var correctAnswers = _playerStats.FirstOrDefault(s => s.Key == "TotalCorrectAnswers");
                if(correctAnswers != null)
                {
                    var correctAnswerEntry = new Microcharts.ChartEntry(float.Parse(correctAnswers.Value)) { Color = SKColor.Parse("#009933"), Label="Correct", ValueLabel = correctAnswers.Value };
                    chartEntries.Add(correctAnswerEntry);
                }
                var incorrectAnswers = _playerStats.FirstOrDefault(s => s.Key == "TotalIncorrectAnswers");
                if (incorrectAnswers != null)
                {
                    var incorrectAnswerEntry = new Microcharts.ChartEntry(float.Parse(incorrectAnswers.Value)) { Color = SKColor.Parse("#FF0000"), Label = "Incorrect", ValueLabel = incorrectAnswers.Value };
                    chartEntries.Add(incorrectAnswerEntry);
                }
            }
            return chartEntries;
        }

        public List<Microcharts.ChartEntry> GetCategoryPlayedStatEntries()
        {
            var chartEntries = new List<Microcharts.ChartEntry>();
            var random = new Random();
            var chartLabels = new List<CatChartLabel>();
            if (_categoryStats != null)
            {
                foreach(var item in _categoryStats)
                {
                    var color = String.Format("#{0:X6}", random.Next(0x1000000));
                    var cat = LookupCategory(item.CategoryName);
                    var entry = new Microcharts.ChartEntry((float)item.TimesPlayed) { Color = SKColor.Parse(color)};
                    var catLabel = new CatChartLabel { Colour = color, Name = cat.Name, Value = item.TimesPlayed.ToString() };
                    chartEntries.Add(entry);
                    chartLabels.Add(catLabel);
                }
                CategoryChartLabels = chartLabels;
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

        public void SetupCategoryStatChart()
        {
            var entries = GetCategoryPlayedStatEntries();
            var chart = new DonutChart() { Entries = entries.AsEnumerable(), LabelTextSize = 12 };
            CategoryStatChart = chart;
        }

        public void SetupPlayerAnswerChart()
        {
            var entries = GetPlayerAnswerStatEntries();
            var chart = new DonutChart() { Entries = entries.AsEnumerable(), LabelTextSize = 12 };
            PlayerAnswerChart = chart;
        }
    }
}
