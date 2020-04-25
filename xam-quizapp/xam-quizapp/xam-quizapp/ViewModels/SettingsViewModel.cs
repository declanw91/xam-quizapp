using System;
using System.Collections.Generic;
using System.Text;

namespace quizapp.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private List<string> _quizCategories;
        public SettingsViewModel()
        {
            _quizCategories = new List<string>();
            PopulateQuizCategories();
        }

        public List<string> QuizCategories
        {
            get => _quizCategories;
            set
            {
                _quizCategories = value;
                OnPropertyChanged("QuizCategories");
            }
        }
        
        private void PopulateQuizCategories()
        {
            _quizCategories.Clear();
            _quizCategories.Add("Category 1");
            _quizCategories.Add("Category 2");
        }
    }
}
