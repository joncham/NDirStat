using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;

namespace NDirStat
{
    public class ListModel
    {
        List<ListModelData> items;

        public ListModel(DirStatModel model)
        {
            items = new List<ListModelData>();
            Visit(model.GetRoot());
        }


        public static string FormatSizeString(long bytes)
        {
            if (bytes < 1024)
            {
                return string.Format("{0} Bytes", bytes);
            }
            else if (bytes < 1024 * 1024)
            {
                return string.Format("{0:F1} KB", bytes / 1024.0);
            }
            else if (bytes < 1024 * 1024 * 1024)
            {
                return string.Format("{0:F1} MB", bytes / (1024.0 * 1024.0));
            }

            return bytes.ToString();
        }

        class CalcData
        {
            public long Bytes;
            public long FileCount;
        }

        void Visit2(Dictionary<string, CalcData> temp, DirStat dir)
        {
            foreach (var file in dir.Files)
            {
                string ext = Path.GetExtension(file.Name);
                CalcData data;
                if (!temp.TryGetValue(ext, out data))
                {
                    data = new CalcData();
                    temp.Add(ext, data);
                }

                data.Bytes += file.Length;
                data.FileCount++;
            }

            foreach (var subDir in dir.Directories)
            {
                Visit2(temp, subDir);
            }
        }

        void Visit(DirStat dir)
        {
            Dictionary<string, CalcData> temp = new Dictionary<string, CalcData>();

            Visit2(temp, dir);

            foreach (var item in temp)
            {
                ListModelData data = new ListModelData(item.Key, 0, string.Empty, item.Value.Bytes, 1.0 * item.Value.Bytes / dir.Length, item.Value.FileCount);

                items.Add(data);
            }
        }

        public ReadOnlyCollection<ListModelData> GetItems()
        {
            return items.AsReadOnly();
        }

    }
}
