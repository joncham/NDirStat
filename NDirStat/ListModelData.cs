using System;
using System.Collections.Generic;
using System.Text;

namespace NDirStat
{
    public class ListModelData
    {
        public ListModelData(string extension, int color, string description, long bytes, double percentBytes, long fileCount)
        {
            Extension = extension;
            Color = color;
            Description = description;
            Bytes = bytes;
            PercentBytes = percentBytes;
            FileCount = fileCount;
        }

        public readonly string Extension;
        public readonly int Color;
        public readonly string Description;
        public readonly long Bytes;
        public readonly double PercentBytes;
        public readonly long FileCount;
    }
}
