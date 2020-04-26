using quizapp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace quizapp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private SettingsViewModel _viewModel => BindingContext as SettingsViewModel;
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = new SettingsViewModel(Navigation);
        }
    }
}