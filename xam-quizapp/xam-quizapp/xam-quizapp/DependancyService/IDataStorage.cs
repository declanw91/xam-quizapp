using System;
using System.Collections.Generic;
using System.Text;

namespace quizapp.DependancyService
{
    public interface IDataStorage
    {
        void CopyFilesToInstalled();
    }
}
