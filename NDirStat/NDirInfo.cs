using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;

namespace NDirStat
{
    public class NDirInfo : IDirInfo
    {
        readonly DirectoryInfo directoryInfo;

        public NDirInfo(string directory)
        {
            this.directoryInfo = new DirectoryInfo(directory);
        }

        public NDirInfo(DirectoryInfo directoryInfo)
        {
            this.directoryInfo = directoryInfo;
        }

        public ReadOnlyCollection<IFileInfo> GetFiles()
        {
            List<IFileInfo> files = new List<IFileInfo>();
            try
            {
                foreach (var file in directoryInfo.GetFiles())
                {
                    files.Add(new NFileInfo(file));
                }
            }
            catch (UnauthorizedAccessException) { }

            return files.AsReadOnly();
        }

        public ReadOnlyCollection<IDirInfo> GetDirectories()
        {
            List<IDirInfo> directories = new List<IDirInfo>();
            try
            {
                foreach (var directory in directoryInfo.GetDirectories())
                {
                    directories.Add(new NDirInfo(directory));
                }
            }
            catch (UnauthorizedAccessException) { }

            return directories.AsReadOnly();
        }

        public string Name
        {
            get { return directoryInfo.Name; }
        }
    }
}
