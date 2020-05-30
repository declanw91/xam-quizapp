using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using quizapp.DependancyService;
using quizapp.iOS.DependancyService;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(DataStorage))]
namespace quizapp.iOS.DependancyService
{
    public class DataStorage : IDataStorage
    {
        public void CopyFilesToInstalled()
        {
            var directoryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            var playerDbPath = Path.Combine(directoryPath, "playerstats.db");
            var categoryDbPath = Path.Combine(directoryPath, "categorystats.db");
            if (!File.Exists(playerDbPath))
            {
                File.Copy("playerstats.db", playerDbPath);
            }
            if (!File.Exists(categoryDbPath))
            {
                File.Copy("categorystats.db", categoryDbPath);
            }
        }
    }
}