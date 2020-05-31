using System;
using System.IO;

namespace quizapp.Constants
{
    public class DbConstants
    {
        private const string _playerStatDatabaseFileName = "playerstats.db";
        private const string _categoryStatDatabaseFileName = "categorystats.db";
        public static string PlayerStatDatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, _playerStatDatabaseFileName);
            }
        }

        public static string CategoryStatDatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, _categoryStatDatabaseFileName);
            }
        }
    }
}
