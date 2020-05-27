using quizapp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace quizapp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuizOver : ContentPage
    {
        private QuizOverViewModel _viewModel => BindingContext as QuizOverViewModel;
        public QuizOver()
        {
            InitializeComponent();
            BindingContext = new QuizOverViewModel(Navigation);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.SetupPageOptions();
        }
    }
}