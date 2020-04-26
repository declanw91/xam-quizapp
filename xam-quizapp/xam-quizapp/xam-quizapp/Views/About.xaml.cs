using quizapp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace quizapp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class About : ContentPage
    {
        private AboutViewModel _viewModel => BindingContext as AboutViewModel;
        public About()
        {
            InitializeComponent();
            BindingContext = new AboutViewModel();
        }
    }
}