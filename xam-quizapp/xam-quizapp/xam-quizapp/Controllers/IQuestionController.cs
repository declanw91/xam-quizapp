using quizapp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace quizapp.Controllers
{
    public interface IQuestionController
    {
        Task<List<QuizQuestion>> GetQuizQuestions(string category, string difficulty, string questionType);
    }
}
