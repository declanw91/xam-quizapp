using quizapp.DependancyService;
using quizapp.UWP.DependancyService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Xamarin.Forms;

[assembly: Dependency(typeof(DataStorage))]
namespace quizapp.UWP.DependancyService
{
    public class DataStorage : IDataStorage
    {
        public void CopyFilesToInstalled()
        {
            var sourcePath = Package.Current.InstalledLocation.Path;
            var directoryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            var playerDbPath = Path.Combine(directoryPath, "playerstats.db");
            var categoryDbPath = Path.Combine(directoryPath, "categorystats.db");
            var sourcePlayerStatsDb = Path.Combine(sourcePath, "playerstats.db");
            var sourceCategoryStatsDb = Path.Combine(sourcePath, "categorystats.db");
            if (!File.Exists(playerDbPath)) 
            {
                File.Copy(sourcePlayerStatsDb, playerDbPath);
            }
            if (!File.Exists(categoryDbPath))
            {
                File.Copy(sourceCategoryStatsDb, categoryDbPath);
            }
        }
    }
}
