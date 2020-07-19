using Acr.UserDialogs;
using quizapp.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace quizapp.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private INavigation _navigation;
        public DashboardViewModel(INavigation nav)
        {
            _navigation = nav;
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
    }
}
