using Acr.UserDialogs;
using Microcharts;
using Microsoft.Extensions.DependencyInjection;
using quizapp.Controllers;
using quizapp.DbControllers;
using quizapp.Models;
using quizapp.Views;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace quizapp.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private INavigation _navigation;
        private List<FaqItem> _faqItems;
        private string _appVersion;
        private int _totalQuizsPlayed;
        private int _totalCorrectAnswers;
        private int _totalIncorrectAnswers;
        private IPlayerStatsDbController _playerStatsDbController;
        private ICategoryStatsDbController _categoryStatsDbController;
        private ICategoryController _categoryController;
        private List<PlayerStats> _playerStats;
        private List<NewsItem> _newsItems;
        private Chart _playerAnswerChart;
        private Chart _categoryStatChart;
        private List<QuizCategory> _quizCategoryList;
        private List<CategoryStats> _categoryStats;
        private List<CatChartLabel> _categoryChartLabels;
        private string _topCategory;
        private string _mostPlayedCategory;
        public DashboardViewModel(INavigation nav)
        {
            _navigation = nav;
            _faqItems = new List<FaqItem>();
            _playerStatsDbController = StartUp.ServiceProvider.GetService<IPlayerStatsDbController>();
            _categoryStatsDbController = StartUp.ServiceProvider.GetService<ICategoryStatsDbController>();
            _categoryController = StartUp.ServiceProvider.GetService<ICategoryController>();
        }

        public List<FaqItem> FAQItems
        {
            get => _faqItems;
            set
            {
                _faqItems = value;
                OnPropertyChanged("FAQItems");
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

        public string AppVersion
        {
            get => _appVersion;
            set
            {
                _appVersion = value;
                OnPropertyChanged("AppVersion");
            }
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

        public List<NewsItem> NewsItems
        {
            get => _newsItems;
            set
            {
                _newsItems = value;
                OnPropertyChanged("NewsItems");
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

        public string TopCategory
        {
            get => _topCategory;
            set
            {
                _topCategory = value;
                OnPropertyChanged("TopCategory");
            }
        }

        public string PlayerMostPlayed
        {
            get => _mostPlayedCategory;
            set
            {
                _mostPlayedCategory = value;
                OnPropertyChanged("PlayerMostPlayed");
            }
        }

        public async void SetupPage()
        {
            await GetAllPlayerStats();
            ConfigureFaq();
            GetAppVersion();
            PopulateTotals();
            SetupNewsItems();
            SetupPlayerAnswerChart();
            await GetAllCategoryStats();
            await GetAllCategories();
            SetupCategoryStatChart();
            PopulateTopCategory();
            PopulateMostPlayedCategory();
        }

        public async void GoToStartAQuiz()
        {
            var settings = new QuizOptions();
            await _navigation.PushAsync(settings);
        }

        public async void GoToRandomCategoryQuiz()
        {
            var settings = new QuizOptions();
            var vm = settings.BindingContext as QuizOptionsViewModel;
            vm.QuizMode = "RC";
            await _navigation.PushAsync(settings);
        }

        public async void GoToRandomQuestionQuiz()
        {
            var settings = new QuizOptions();
            var vm = settings.BindingContext as QuizOptionsViewModel;
            vm.QuizMode = "RQ";
            await _navigation.PushAsync(settings);
        }

        public async void GoToAbout()
        {
            var about = new About();
            await _navigation.PushAsync(about);
        }

        public async void GoToPlayerScores()
        {
            var playerScores = new PlayerScores();
            await _navigation.PushAsync(playerScores);
        }

        public async void GoToHelp()
        {
            var helpPage = new Help();
            await _navigation.PushAsync(helpPage);
        }

        public async void CheckNetwork()
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                await UserDialogs.Instance.AlertAsync("You will need a internet connection to play the quiz. Please connect to a network","Warning", "Ok");
            }
        }

        public async void PlayTopCategory()
        {
            var settings = new QuizOptions();
            var vm = settings.BindingContext as QuizOptionsViewModel;
            vm.SelectedCategory = TopCategory;
            await _navigation.PushAsync(settings);
        }

        public async void PlayFavouriteCategory()
        {
            var settings = new QuizOptions();
            var vm = settings.BindingContext as QuizOptionsViewModel;
            vm.SelectedCategory = PlayerMostPlayed;
            await _navigation.PushAsync(settings);
        }

        private void ConfigureFaq()
        {
            PopulateFaqItems();
        }

        private void PopulateFaqItems()
        {
            FAQItems.Clear();
            FAQItems.Add(new FaqItem { Id = 1, Question = "How you a submit an answer?", Answer = "Just tap the answer you wish to submit, once sure tap the submit button" });
            FAQItems.Add(new FaqItem { Id = 2, Question = "How does the scoring work?", Answer = "You score a point for each correct answer you submit" });
            FAQItems.Add(new FaqItem { Id = 3, Question = "How do I move to the next question?", Answer = "Just tap the next button, you must have submitted an answer first" });
        }

        private void GetAppVersion()
        {
            AppVersion = AppInfo.VersionString;
        }

        private void PopulateTotals()
        {
            var quizsPlayed = _playerStats.FirstOrDefault(s => s.Key == "QuizsPlayed");
            if (quizsPlayed != null)
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

        private void SetupPlayerAnswerChart()
        {
            var entries = GetPlayerAnswerStatEntries();
            var chart = new DonutChart() { Entries = entries.AsEnumerable(), LabelTextSize = 12, BackgroundColor = SKColors.Transparent };
            PlayerAnswerChart = chart;
        }

        private void SetupCategoryStatChart()
        {
            var entries = GetCategoryPlayedStatEntries();
            var chart = new DonutChart() { Entries = entries.AsEnumerable(), LabelTextSize = 12, BackgroundColor = SKColors.Transparent };
            CategoryStatChart = chart;
        }

        private QuizCategory LookupCategory(string id)
        {
            return _quizCategoryList.FirstOrDefault(c => c.Id.ToLowerInvariant() == id.ToLowerInvariant());
        }

        private async Task GetAllCategories()
        {
            var categories = await _categoryController.GetQuizCategories();
            if (categories != null)
            {
                _quizCategoryList = categories;
            }
        }

        private List<Microcharts.ChartEntry> GetCategoryPlayedStatEntries()
        {
            var chartEntries = new List<Microcharts.ChartEntry>();
            var random = new Random();
            var chartLabels = new List<CatChartLabel>();
            if (_categoryStats != null)
            {
                foreach (var item in _categoryStats)
                {
                    var color = String.Format("#{0:X6}", random.Next(0x1000000));
                    var cat = LookupCategory(item.CategoryName);
                    var entry = new Microcharts.ChartEntry((float)item.TimesPlayed) { Color = SKColor.Parse(color) };
                    var catLabel = new CatChartLabel { Colour = color, Name = cat.Name, Value = item.TimesPlayed.ToString() };
                    chartEntries.Add(entry);
                    chartLabels.Add(catLabel);
                }
                CategoryChartLabels = chartLabels;
            }
            return chartEntries;
        }

        private async Task GetAllCategoryStats()
        {
            var stats = await _categoryStatsDbController.GetAllCategoryStats();
            if (stats != null)
            {
                _categoryStats = stats;
            }
        }

        private async Task GetAllPlayerStats()
        {
            var stats = await _playerStatsDbController.GetAllPlayerStats();
            if (stats != null)
            {
                _playerStats = stats;
            }
        }

        private void SetupNewsItems()
        {
            var tempList = new List<NewsItem>() {
                new NewsItem { Title = "New UI updates", Description = "The new update comes with a new UI to bring it a little more up to date. If you're having trouble finding a page please check our help section."},
                new NewsItem { Title = "New features coming soon", Description = "In the future there will be the option to pick the length of your quiz and new player stats for you to review"},
                new NewsItem { Title = "Top category", Description = "New feature, from the dashboard you can now jump into the top category of the day, try your luck and see if you can beat the top score"}
            };
            NewsItems = tempList;
        }

        private List<Microcharts.ChartEntry> GetPlayerAnswerStatEntries()
        {
            var chartEntries = new List<Microcharts.ChartEntry>();
            if (_playerStats != null)
            {
                var correctAnswers = _playerStats.FirstOrDefault(s => s.Key == "TotalCorrectAnswers");
                if (correctAnswers != null)
                {
                    var correctAnswerEntry = new Microcharts.ChartEntry(float.Parse(correctAnswers.Value)) { Color = SKColor.Parse("#009933"), Label = "Correct", ValueLabel = correctAnswers.Value };
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

        private void PopulateTopCategory()
        {
            if(_quizCategoryList != null)
            {
                var random = new Random();
                var index = random.Next(_quizCategoryList.Count);
                var topCat = _quizCategoryList.ElementAt(index);
                if (topCat != null)
                {
                    TopCategory = _quizCategoryList.ElementAt(index).Name;
                }
            }
        }

        private async void PopulateMostPlayedCategory()
        {
            var cat = await _categoryStatsDbController.GetMostPlayedCategory(); 
            if(cat != null)
            {
                var catName = LookupCategory(cat.CategoryName);
                PlayerMostPlayed = catName.Name;
            }
        }
    }
}
