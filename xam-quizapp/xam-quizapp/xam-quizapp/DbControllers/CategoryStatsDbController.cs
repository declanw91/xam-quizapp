using quizapp.Constants;
using quizapp.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace quizapp.DbControllers
{
    public class CategoryStatsDbController : ICategoryStatsDbController
    {
        private readonly SQLiteAsyncConnection _database;
        public CategoryStatsDbController()
        {
            _database = new SQLiteAsyncConnection(DbConstants.CategoryStatDatabasePath);
        }

        public async Task<List<CategoryStats>> GetAllCategoryStats()
        {
            var stats = await _database.Table<CategoryStats>().ToListAsync();
            return stats;
        }

        public async Task<CategoryStats> GetCategoryStat(string key)
        {
            var stat = await _database.Table<CategoryStats>().FirstOrDefaultAsync(i => i.CategoryName == key);
            return stat;
        }

        public async Task InsertCategoryStat(CategoryStats stat)
        {
            await _database.InsertAsync(stat);
        }

        public async Task UpdateCategoryStat(CategoryStats stat)
        {
            await _database.UpdateAsync(stat);
        }

        public async Task<CategoryStats> GetMostPlayedCategory()
        {
            var stat = await _database.Table<CategoryStats>().OrderByDescending(t => t.TimesPlayed).FirstOrDefaultAsync();
            return stat;
        }
    }
}
