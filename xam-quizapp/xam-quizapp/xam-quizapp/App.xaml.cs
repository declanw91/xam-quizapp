using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using quizapp.Views;

namespace quizapp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            StartUp.Init();
            MainPage = new NavigationPage(new Dashboard());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
