﻿using quizapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}