using Microcharts;
using quizapp.ViewModels;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace quizapp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerScores : TabbedPage
    {
        private PlayerScoresViewModel _viewModel => BindingContext as PlayerScoresViewModel;
        public PlayerScores()
        {
            InitializeComponent();
            BindingContext = new PlayerScoresViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.SetupPlayerScoresData();
            CreatePlayerAnswerChart();
        }

        private void CreatePlayerAnswerChart()
        {
            var entries = _viewModel.GetPlayerAnswerStatEntries();
            var chart = new DonutChart() { Entries = entries.AsEnumerable() };
            correctIncorrectAnswerChart.Chart = chart;
        }
    }
}