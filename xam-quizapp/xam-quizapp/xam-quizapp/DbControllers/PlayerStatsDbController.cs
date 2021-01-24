using quizapp.Constants;
using quizapp.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace quizapp.DbControllers
{
    public class PlayerStatsDbController : IPlayerStatsDbController
    {
        private readonly SQLiteAsyncConnection _database;
        public PlayerStatsDbController()
        {
            _database = new SQLiteAsyncConnection(DbConstants.PlayerStatDatabasePath);
        }

        public async Task<PlayerStats> GetPlayerStat(string key)
        {
            var stat = await _database.Table<PlayerStats>().FirstOrDefaultAsync(i => i.Key == key);
            return stat;
        }

        public async Task UpdatePlayerStat(PlayerStats stat)
        {
            await _database.UpdateAsync(stat);
        }

        public async Task InsertPlayerStat(PlayerStats stat)
        {
            await _database.InsertAsync(stat);
        }

        public async Task<List<PlayerStats>> GetAllPlayerStats()
        {
            var stats = await _database.Table<PlayerStats>().ToListAsync();
            return stats;
        }       
    }
}
