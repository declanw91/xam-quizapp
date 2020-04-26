﻿using quizapp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace quizapp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dashboard : ContentPage
    {
        private DashboardViewModel _viewModel => BindingContext as DashboardViewModel;
        public Dashboard()
        {
            InitializeComponent();
            BindingContext = new DashboardViewModel(Navigation);
        }

        private void TapGestureRecognizer_StartAQuiz(object sender, System.EventArgs e)
        {
            
        }

        private void TapGestureRecognizer_RandomCategory(object sender, System.EventArgs e)
        {

        }

        private void TapGestureRecognizer_RandomQuestion(object sender, System.EventArgs e)
        {

        }

        private void TapGestureRecognizer_Settings(object sender, System.EventArgs e)
        {
            _viewModel.GoToSettings();
        }

        private void TapGestureRecognizer_About(object sender, System.EventArgs e)
        {
            _viewModel.GoToAbout();
        }
    }
}