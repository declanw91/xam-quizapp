using quizapp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace quizapp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dashboard
    {
        private DashboardViewModel _viewModel => BindingContext as DashboardViewModel;
        public Dashboard()
        {
            InitializeComponent();
            BindingContext = new DashboardViewModel(Navigation);
        }

        private void TapGestureRecognizer_StartAQuiz(object sender, System.EventArgs e)
        {
            _viewModel.GoToStartAQuiz();
        }

        private void TapGestureRecognizer_RandomCategory(object sender, System.EventArgs e)
        {
            _viewModel.GoToRandomCategoryQuiz();
        }

        private void TapGestureRecognizer_RandomQuestion(object sender, System.EventArgs e)
        {
            _viewModel.GoToRandomQuestionQuiz();
        }

        private void TapGestureRecognizer_About(object sender, System.EventArgs e)
        {
            _viewModel.GoToAbout();
        }

        private void TapGestureRecognizer_PlayerScores(object sender, System.EventArgs e)
        {
            _viewModel.GoToPlayerScores();
        }

        private void TapGestureRecognizer_Help(object sender, System.EventArgs e)
        {
            _viewModel.GoToHelp();
        }

        private void TapGestureRecognizer_PlayTopCategory(object sender, System.EventArgs e)
        {
            _viewModel.PlayTopCategory();
        }

        private void TapGestureRecognizer_PlayFavouriteCategory(object sender, System.EventArgs e)
        {
            _viewModel.PlayFavouriteCategory();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.CheckNetwork();
            _viewModel.SetupPage();
        }
    }
}