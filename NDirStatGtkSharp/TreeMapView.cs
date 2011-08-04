using System;
using System.Collections.Generic;
using System.Text;
using Gtk;
using Cairo;
using NDirStat;

namespace NDirStatGtkSharp
{
    public class TreeMapView : DrawingArea
    {
        DirStatModel model;
        TreeMapModel treeMapModel;
        Gdk.Size prevSize = Gdk.Size.Empty;

        public TreeMapView()
        {
            this.ExposeEvent += OnDrawingAreaExposed;
        }

        public void SetModel(DirStatModel model)
        {
            this.model = model;
            this.QueueDraw();
        }

        void OnDrawingAreaExposed(object o, ExposeEventArgs args)
        {
            if (prevSize != Allocation.Size)
            {
                if (model != null)
                    treeMapModel = new TreeMapModel(model, Allocation.Width, Allocation.Height);
            }

            DrawingArea area = (DrawingArea)o;
            Cairo.Context g = Gdk.CairoHelper.Create(area.GdkWindow);

            if (treeMapModel != null)
            {
                foreach (var item in treeMapModel.Items)
                {
                    double width = item.Rectangle.Width;
                    double height = item.Rectangle.Height;

                    double x1 = item.Rectangle.X;
                    double x2 = item.Rectangle.X + item.Rectangle.Width;
                    double y1 = item.Rectangle.Y;
                    double y2 = item.Rectangle.Y + item.Rectangle.Height;

                    PointD p1, p2, p3, p4;
                    p1 = new PointD(x1, y1);
                    p2 = new PointD(x2, y1);
                    p3 = new PointD(x2, y2);
                    p4 = new PointD(x1, y2);

                    g.MoveTo(p1);
                    g.LineTo(p2);
                    g.LineTo(p3);
                    g.LineTo(p4);
                    g.LineTo(p1);
                    g.ClosePath();

                    g.Save();
                    //using (Gradient pat = new LinearGradient(x1, y1, x2, y2))
                    using (Gradient pat = new RadialGradient(x1 + (x2 - x1) / 4.0, y1 + (y2 - y1) / 4.0, 3, x1 + (x2 - x1) / 4.0, y1 + (y2 - y1) / 4.0, Math.Sqrt(width*width + height*height)))
                    {
                        pat.AddColorStop(0, new Cairo.Color(1, 1, 1, 1));
                        pat.AddColorStop(1, new Cairo.Color(0, 0, 1, 1));
                        g.Pattern = pat;

                        // Fill the path with pattern
                        g.FillPreserve();
                    }

                    // We "undo" the pattern setting here
                    g.Restore();

                    g.Color = new Color(0, 0, 0, 0);
                    g.Stroke();
                }
            }

            ((IDisposable)g.Target).Dispose();
            ((IDisposable)g).Dispose();
        }
    }
}
