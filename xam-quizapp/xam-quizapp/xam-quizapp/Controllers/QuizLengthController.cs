using System.Collections.Generic;

namespace quizapp.Controllers
{
    public class QuizLengthController
    {
        public QuizLengthController()
        {

        }

        public List<int> GetQuizLengthOptions()
        {
            var quizLengthList = new List<int>();
            quizLengthList.Add(10);
            quizLengthList.Add(15);
            quizLengthList.Add(25);
            quizLengthList.Add(50);
            return quizLengthList;
        }
    }
}
