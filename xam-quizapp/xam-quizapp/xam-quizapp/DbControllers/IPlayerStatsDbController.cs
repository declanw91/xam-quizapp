using quizapp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace quizapp.DbControllers
{
    public interface IPlayerStatsDbController
    {
        Task<PlayerStats> GetPlayerStat(string key);
        Task UpdatePlayerStat(PlayerStats stat);
        Task InsertPlayerStat(PlayerStats stat);
        Task<List<PlayerStats>> GetAllPlayerStats();
    }
}
