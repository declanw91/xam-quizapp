﻿using quizapp.Views;
using Xamarin.Forms;

namespace quizapp.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private Command _settingsCommand;
        private INavigation _navigation;
        public DashboardViewModel(INavigation nav)
        {
            _navigation = nav;
        }

        public Command SettingsCommand => _settingsCommand ?? (_settingsCommand = new Command(GoToSettings));


        private void GoToSettings()
        {
            var settingsPage = new SettingsPage();
            _navigation.PushAsync(settingsPage);
        }
    }
}
