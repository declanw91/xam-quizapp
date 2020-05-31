using quizapp.Views;
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
    }
}
