using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace NDirStat
{
    public class DirStatModel
    {
        DirStat root;

        public DirStatModel(DirStat root)
        {
            this.root = root;
        }

        public DirStat GetRoot()
        {
            return root;
        }
    }

    public class FileStat
    {
        public FileStat(string name, long length)
        {
            Name = name;
            Length = length;
        }

        public readonly string Name;

        public readonly long Length;
    }

    public class DirStat
    {
        string name;

        public DirStat(string name, IEnumerable<DirStat> directories, IEnumerable<FileStat> files)
        {
            Name = name;
            Directories = new ReadOnlyCollection<DirStat>(new List<DirStat>(directories));
            Files = new ReadOnlyCollection<FileStat>(new List<FileStat>(files));

            Length = 0;
            foreach (var val in Directories)
                Length += val.Length;
            foreach (var val in Files)
                Length += val.Length;
        }

        public readonly string Name;

        public readonly long Length;

        public ReadOnlyCollection<FileStat> Files;

        public ReadOnlyCollection<DirStat> Directories;
    }
}
