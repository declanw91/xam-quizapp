using System.Collections.Generic;

namespace quizapp.Controllers
{
    public interface IQuestionTypeController
    {
        List<string> GetQuestionTypeOptions();
    }
}
