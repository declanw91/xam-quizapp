using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using quizapp.DependancyService;
using quizapp.Droid.DependancyService;
using Xamarin.Forms;
[assembly: Dependency(typeof(DataStorage))]
namespace quizapp.Droid.DependancyService
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
                var pStats = Forms.Context.Resources.OpenRawResource(Resource.Raw.playerstats);
                var writeStream = new FileStream(playerDbPath, FileMode.OpenOrCreate, FileAccess.Write);
                ReadWriteStream(pStats, writeStream);
            }
            if (!File.Exists(categoryDbPath))
            {
                var cStats = Forms.Context.Resources.OpenRawResource(Resource.Raw.categorystats);
                var writeStream = new FileStream(categoryDbPath, FileMode.OpenOrCreate, FileAccess.Write);
                ReadWriteStream(cStats, writeStream);
            }
        }

        private void ReadWriteStream(Stream readStream, Stream writeStream)
        {
            const int length = 256;
            var buffer = new Byte[length];
            var bytesRead = readStream.Read(buffer, 0, length);
            while (bytesRead > 0)    
            {
                writeStream.Write(buffer, 0, bytesRead);
                bytesRead = readStream.Read(buffer, 0, length);
            }
            readStream.Close();
            writeStream.Close();
        }
    }
}