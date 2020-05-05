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
    }
}
