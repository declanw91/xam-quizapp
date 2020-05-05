using System.Collections.Generic;
using System.Threading.Tasks;

namespace quizapp.Controllers
{
    public class DifficultyController
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
