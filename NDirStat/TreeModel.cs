using System;
using System.Collections.Generic;
using System.Text;

namespace NDirStat
{
    public abstract class TreeModelData
    {
        public abstract string Name { get; }
        public abstract long Length { get; }
        public abstract int Items { get; }
        public abstract int Files { get; }
        public abstract int Directories { get; }
        public abstract IEnumerable<TreeModelData> GetChildren();
    }

    public class FileTreeModelData : TreeModelData
    {
        FileStat file;
        internal FileTreeModelData(FileStat file)
        {
            this.file = file;
        }

        public override string Name { get { return file.Name; } }
        public override long Length { get { return file.Length; } }
        public override int Items { get { return Files + Directories; } }
        public override int Files { get { return 0; } }
        public override int Directories { get { return 0; } }

        public override IEnumerable<TreeModelData> GetChildren()
        {
            yield break;
        }
    }

    public class DirectoryTreeModelData : TreeModelData
    {
        DirStat dir;
        internal DirectoryTreeModelData(DirStat dir)
        {
            this.dir = dir;
        }

        public override string Name { get { return dir.Name; } }
        public override long Length { get { return dir.Length; } }
        public override int Items { get { return Files + Directories; } }
        public override int Files { get { return dir.Files.Count; } }
        public override int Directories { get { return dir.Directories.Count; } }

        public override IEnumerable<TreeModelData> GetChildren()
        {
            foreach (var item in dir.Directories)
            {
                yield return new DirectoryTreeModelData(item);
            }

            foreach (var item in dir.Files)
            {
                yield return new FileTreeModelData(item);
            }
        }
    }

    public class TreeModel
    {
        DirStatModel model;
        public TreeModel(DirStatModel model)
        {
            this.model = model;
        }

        public TreeModelData GetRoot()
        {
            return new DirectoryTreeModelData(model.GetRoot());
        }
    }
}
