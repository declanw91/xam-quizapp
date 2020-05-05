using Xamarin.Essentials;

namespace quizapp.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private string _appVersion;
        public AboutViewModel()
        {
            GetAppVersion();
        }

        public string AppVersion
        {
            get => _appVersion;
            set
            {
                _appVersion = value;
                OnPropertyChanged("AppVersion");
            }
        }

        private void GetAppVersion()
        {
            AppVersion = AppInfo.VersionString;
        }
    }
}
