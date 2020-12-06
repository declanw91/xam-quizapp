using Acr.UserDialogs;
using Microsoft.Extensions.DependencyInjection;
using quizapp.Controllers;
using quizapp.DbControllers;
using quizapp.Models;
using quizapp.Views;
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
        public DashboardViewModel(INavigation nav)
        {
            _navigation = nav;
            _faqItems = new List<FaqItem>();
            _playerStatsDbController = StartUp.ServiceProvider.GetService<IPlayerStatsDbController>();
            _categoryStatsDbController = StartUp.ServiceProvider.GetService<ICategoryStatsDbController>();
            _categoryController = StartUp.ServiceProvider.GetService<ICategoryController>();
        }

        public async void SetupPage()
        {
            await GetAllPlayerStats();
            ConfigureFaq();
            GetAppVersion();
            PopulateTotals();
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
        public List<FaqItem> FAQItems
        {
            get => _faqItems;
            set
            {
                _faqItems = value;
                OnPropertyChanged("FAQItems");
            }
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

        public string AppVersion
        {
            get => _appVersion;
            set
            {
                _appVersion = value;
                OnPropertyChanged("AppVersion");
            }
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

        private async Task GetAllPlayerStats()
        {
            var stats = await _playerStatsDbController.GetAllPlayerStats();
            if (stats != null)
            {
                _playerStats = stats;
            }
        }
    }
}
