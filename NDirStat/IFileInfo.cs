using System;
using System.Collections.Generic;
using System.Text;

namespace NDirStat
{
    public interface IFileInfo
    {
        string Name { get; }

        long Length { get; }
    }
}
