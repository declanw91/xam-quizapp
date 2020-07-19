using quizapp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace quizapp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuestionPage : ContentPage
    {
        private QuestionPageViewModel _viewModel => BindingContext as QuestionPageViewModel;
        public QuestionPage()
        {
            InitializeComponent();
            BindingContext = new QuestionPageViewModel(Navigation);
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.StartQuiz();
        }

        private void lvAnswers_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            _viewModel.HighlightUsersAnswer();
        }
    }
}