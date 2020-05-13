using System;
using System.Globalization;
using Xamarin.Forms;

namespace quizapp.Helpers
{
    public class AnswerStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null)
            {
                var a = value.ToString().ToLowerInvariant();
                switch (a)
                {
                    case "correct":
                        return (Style)App.Current.Resources["correctAnswerFrameStyle"];
                    case "incorrect":
                        return (Style)App.Current.Resources["incorrectAnswerFrameStyle"];
                    case "useranswer":
                        return (Style)App.Current.Resources["userAnswerFrameStyle"];
                    default:
                        return (Style)App.Current.Resources["answerFrameStyle"];
                }
            }
            return (Style)App.Current.Resources["answerFrameStyle"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return String.Empty;
        }
    }
}
