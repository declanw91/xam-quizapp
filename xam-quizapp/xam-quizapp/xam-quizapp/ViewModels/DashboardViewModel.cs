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

        public void GoToStartAQuiz()
        {
            var questionPage = new QuestionPage();
            _navigation.PushAsync(questionPage);
        }
    }
}
