using quizapp.ViewModels;
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
    }
}