using quizapp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace quizapp.Controllers
{
    public interface ICategoryController
    {
        Task<List<QuizCategory>> GetQuizCategories();
    }
}
