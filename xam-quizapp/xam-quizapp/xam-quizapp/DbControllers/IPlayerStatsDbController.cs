using quizapp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace quizapp.DbControllers
{
    public interface IPlayerStatsDbController
    {
        Task<PlayerStat> GetPlayerStat(int id);
    }
}
