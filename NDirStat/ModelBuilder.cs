using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace NDirStat
{
    public class ModelBuilder
    {
        public DirStatModel Build(IDirInfo dirInfo)
        {

            if (dirInfo == null)
            {
                throw new ArgumentNullException();
            }

            DirStat root = RecurseDir(dirInfo);
            DirStatModel model = new DirStatModel(root);

            return model;
        }

        DirStat RecurseDir(IDirInfo dirInfo)
        {
            List<DirStat> directories = new List<DirStat>();
            foreach (IDirInfo subDir in dirInfo.GetDirectories())
            {
                directories.Add(RecurseDir(subDir));
            }

            List<FileStat> files = new List<FileStat>();
            foreach (IFileInfo file in dirInfo.GetFiles())
            {
                files.Add(new FileStat(file.Name, file.Length));
            }

            return new DirStat(dirInfo.Name, directories, files);

        }
    }



}
