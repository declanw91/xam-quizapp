using quizapp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace quizapp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Help : ContentPage
    {
        private HelpViewModel _viewModel => BindingContext as HelpViewModel;
        public Help()
        {
            InitializeComponent();
            BindingContext = new HelpViewModel(Navigation);
        }
    }
}