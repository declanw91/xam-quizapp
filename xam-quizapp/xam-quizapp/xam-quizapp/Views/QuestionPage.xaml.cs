using quizapp.ViewModels;
using System;
using System.ComponentModel;
using System.Linq;
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
            _viewModel.UserSubmittedAnswer += UpdateUI;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.StartQuiz();
        }

        private void UpdateUI(object sender, PropertyChangedEventArgs e)
        {

        }
    }
}