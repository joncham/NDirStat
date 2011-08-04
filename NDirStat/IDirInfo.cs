using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace NDirStat
{
    public interface IDirInfo
    {
        string Name { get; }
        ReadOnlyCollection<IFileInfo> GetFiles();
        ReadOnlyCollection<IDirInfo> GetDirectories();
    }
}
