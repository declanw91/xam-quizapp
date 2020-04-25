using quizapp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace quizapp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private SettingsViewModel _viewModel;
        public SettingsPage()
        {
            InitializeComponent();
            _viewModel = new SettingsViewModel();
            BindingContext = _viewModel;
        }
    }
}