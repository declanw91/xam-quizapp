using System.Collections.Generic;

namespace quizapp.Controllers
{
    public interface IDifficultyController
    {
        List<string> GetQuizDifficulties();
    }
}
