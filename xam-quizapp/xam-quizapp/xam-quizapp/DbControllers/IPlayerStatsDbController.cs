﻿using quizapp.Models;
using System.Threading.Tasks;

namespace quizapp.DbControllers
{
    public interface IPlayerStatsDbController
    {
        Task<PlayerStats> GetPlayerStat(string key);
    }
}
