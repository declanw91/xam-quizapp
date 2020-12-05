using Acr.UserDialogs;
using quizapp.Models;
using quizapp.Views;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace quizapp.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private INavigation _navigation;
        private List<FaqItem> _faqItems;
        private string _appVersion;
        public DashboardViewModel(INavigation nav)
        {
            _navigation = nav;
            _faqItems = new List<FaqItem>();
            ConfigureFaq();
            GetAppVersion();
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
    }
}
