using quizapp.Models;
using System.Collections.Generic;
using Xamarin.Forms;

namespace quizapp.ViewModels
{
    public class HelpViewModel : BaseViewModel
    {
        private List<FaqItem> _faqItems;
        public HelpViewModel(INavigation nav)
        {
            _faqItems = new List<FaqItem>();
            Configure();
        }

        public List<FaqItem> FAQItems
        {
            get => _faqItems;
            set
            {
                _faqItems = value;
                OnPropertyChanged("FAQItems");
            }
        }

        private void Configure()
        {
            PopulateFaqItems();
        }

        private void PopulateFaqItems()
        {
            FAQItems.Clear();
            FAQItems.Add(new FaqItem { Id = 1, Question = "How you a submit an answer?", Answer = "Just tap the answer you wish to submit, once sure tap the submit button" });
            FAQItems.Add(new FaqItem { Id = 2, Question = "How does the scoring work?", Answer = "You score a point for each correct answer you submit" });
            FAQItems.Add(new FaqItem { Id = 3, Question = "How do I move to the next question?", Answer = "Just tap the next button, you must have submitted an answer first" });
        }
    }
}
