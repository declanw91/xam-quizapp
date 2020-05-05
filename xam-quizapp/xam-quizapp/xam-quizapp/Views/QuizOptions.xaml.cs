using quizapp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace quizapp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuizOptions : ContentPage
    {
        private QuizOptionsViewModel _viewModel => BindingContext as QuizOptionsViewModel;
        public QuizOptions()
        {
            InitializeComponent();
            BindingContext = new QuizOptionsViewModel(Navigation);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.SetupPageOptions();
        }
    }
}