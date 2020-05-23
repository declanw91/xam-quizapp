using System.Collections.Generic;

namespace quizapp.Controllers
{
    public class DifficultyController : IDifficultyController
    {
        public DifficultyController()
        {

        }

        public List<string> GetQuizDifficulties()
        {
            var catList = new List<string>();
            catList.Add("Easy");
            catList.Add("Medium");
            catList.Add("Hard");
            return catList;
        }
    }
}
