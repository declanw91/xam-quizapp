using quizapp.Constants;
using quizapp.Models;
using SQLite;
using System.Threading.Tasks;

namespace quizapp.DbControllers
{
    public class PlayerStatsDbController : IPlayerStatsDbController
    {
        private readonly SQLiteAsyncConnection _database;
        public PlayerStatsDbController()
        {
            _database = new SQLiteAsyncConnection(DbConstants.DatabasePath);
        }

        public async Task<PlayerStat> GetPlayerStat(int id)
        {
            return await _database.Table<PlayerStat>().FirstOrDefaultAsync(i => i.Id == id);
        }
    }
}
