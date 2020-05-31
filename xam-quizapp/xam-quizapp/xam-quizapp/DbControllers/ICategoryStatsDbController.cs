using quizapp.Models;
using System.Threading.Tasks;

namespace quizapp.DbControllers
{
    public interface ICategoryStatsDbController
    {
        Task<CategoryStats> GetCategoryStat(string key);
        Task UpdateCategoryStat(CategoryStats stat);
        Task InsertCategoryStat(CategoryStats stat);
    }
}
