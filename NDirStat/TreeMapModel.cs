using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace NDirStat
{
    public struct Rect
    {
        public Rect(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public double X;
        public double Y;
        public double Width;
        public double Height;

    }

    public class TreeMapItem
    {
        public double Area { get; set; }
        public Rect Rectangle { get; set; }
    }

    public class TreeMapModel
    {
        enum Orientation
        {
            Horizontal,
            Vertical
        }

        List<TreeMapItem> items;

        public TreeMapModel(DirStatModel model, double width, double height)
        {
            items = BuildModel(model.GetRoot(), width, height);
        }

        public ReadOnlyCollection<TreeMapItem> Items
        {
            get { return items.AsReadOnly();  }
        }

        void ComputeNextPosition(Orientation orientation, ref double xPos, ref double yPos, double width, double height)
        {
            if (orientation == Orientation.Horizontal)
                yPos += height;
            else
                xPos += width;
        }

        static IEnumerable<FileStat> EnumerateFiles(DirStat dir)
        {
            foreach (var item in dir.Files)
            {
                yield return item;
            }

            foreach (var item in dir.Directories)
            {
                foreach (var file in  EnumerateFiles(item))
                {
                    yield return file;
                }
            }
        }

        List<TreeMapItem> BuildModel(DirStat root, double width, double height)
        {
            Rect emptyArea = new Rect(0, 0, width, height);
            //this.PrepareItems();

            double area = emptyArea.Width * emptyArea.Height;

            List<TreeMapItem> items = new List<TreeMapItem>();
            foreach (var file in EnumerateFiles(root))
            {
                TreeMapItem item = new TreeMapItem();
                item.Area = area * file.Length / root.Length;
                if (item.Area > 1)
                {
                    items.Add(item);
                }
                else
                {
                    int i = 0;
                }
            }

            this.Squarify(items, new List<TreeMapItem>(), ref emptyArea);

            return items;
        }

        private void Squarify(List<TreeMapItem> items, List<TreeMapItem> row, ref Rect emptyArea)
        {
            if (items.Count == 0)
            {
                ComputeTreeMaps(row, ref emptyArea);
                return;
            }

            double sideLength = Math.Min(emptyArea.Width, emptyArea.Height);

            TreeMapItem item = items[0];
            List<TreeMapItem> row2 = new List<TreeMapItem>(row);
            row2.Add(item);
            List<TreeMapItem> items2 = new List<TreeMapItem>(items);
            items2.RemoveAt(0);

            double worst1 = this.Worst(row, sideLength);
            double worst2 = this.Worst(row2, sideLength);

            if (row.Count == 0 || worst1 > worst2)
                this.Squarify(items2, row2, ref emptyArea);
            else
            {
                ComputeTreeMaps(row, ref emptyArea);
                this.Squarify(items, new List<TreeMapItem>(), ref emptyArea);
            }
        }

        private double Worst(List<TreeMapItem> row, double sideLength)
        {
            if (row.Count == 0) return 0;

            double maxArea = 0;
            double minArea = double.MaxValue;
            double totalArea = 0;
            foreach (TreeMapItem item in row)
            {
                maxArea = Math.Max(maxArea, item.Area);
                minArea = Math.Min(minArea, item.Area);
                totalArea += item.Area;
            }
            if (minArea == double.MaxValue) minArea = 0;

            double val1 = (sideLength * sideLength * maxArea) / (totalArea * totalArea);
            double val2 = (totalArea * totalArea) / (sideLength * sideLength * minArea);
            return Math.Max(val1, val2);
        }

        Orientation GetOrientation(Rect emptyArea)
        {
            return (emptyArea.Width > emptyArea.Height ? Orientation.Horizontal : Orientation.Vertical);
        }

        protected void ComputeTreeMaps(List<TreeMapItem> items, ref Rect emptyArea)
        {
            Orientation orientation = this.GetOrientation(emptyArea);

            double areaSum = 0;

            foreach (TreeMapItem item in items)
                areaSum += item.Area;

            Rect currentRow;
            if (orientation == Orientation.Horizontal)
            {
                currentRow = new Rect(emptyArea.X, emptyArea.Y, areaSum / emptyArea.Height, emptyArea.Height);
                emptyArea = new Rect(emptyArea.X + currentRow.Width, emptyArea.Y, Math.Max(0, emptyArea.Width - currentRow.Width), emptyArea.Height);
            }
            else
            {
                currentRow = new Rect(emptyArea.X, emptyArea.Y, emptyArea.Width, areaSum / emptyArea.Width);
                emptyArea = new Rect(emptyArea.X, emptyArea.Y + currentRow.Height, emptyArea.Width, Math.Max(0, emptyArea.Height - currentRow.Height));
            }

            double prevX = currentRow.X;
            double prevY = currentRow.Y;

            foreach (TreeMapItem item in items)
            {
                Rect rect = GetRectangle(orientation, item, prevX, prevY, currentRow.Width, currentRow.Height);

                item.Rectangle = rect;

                this.ComputeNextPosition(orientation, ref prevX, ref prevY, rect.Width, rect.Height);
            }
        }


        static Rect GetRectangle(Orientation orientation, TreeMapItem item, double x, double y, double width, double height)
        {
            if (orientation == Orientation.Horizontal)
                return new Rect(x, y, width, item.Area / width);
            else
                return new Rect(x, y, item.Area / height, height);
        }
    }
}
