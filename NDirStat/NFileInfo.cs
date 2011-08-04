using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NDirStat
{
    public class NFileInfo : IFileInfo
    {
        readonly FileInfo fileInfo;

        public NFileInfo(string file)
        {
            this.fileInfo = new FileInfo(file);
        }

        public NFileInfo(FileInfo fileInfo)
        {
            this.fileInfo = fileInfo;
        }

        public string Name
        {
            get { return fileInfo.Name; }
        }

        public long Length
        {
            get { return fileInfo.Length; }
        }
    }
}
