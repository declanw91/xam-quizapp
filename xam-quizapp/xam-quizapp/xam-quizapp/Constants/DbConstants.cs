using System;
using System.IO;

namespace quizapp.Constants
{
    public class DbConstants
    {
        public const string DatabaseFileName = "playerstats.db";
        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, DatabaseFileName);
            }
        }
    }
}
