using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using NDirStat;

namespace NDirStatWindowsForms
{
    class Code
    {
        TreeMapModel treeMapModel;

        bool IsUnix
        {
            get
            {
                var platform = (int)Environment.OSVersion.Platform;
                return (platform == 4) || (platform == 6) || (platform == 128);
            }
        }

        void PaintWindows(PaintEventArgs e)
        {
            if (treeMapModel != null)
            {

                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddEllipse(new RectangleF(-.25f, -.25f, 1f, 1f));

                    using (PathGradientBrush pthGrBrush = new PathGradientBrush(path))
                    {
                        pthGrBrush.CenterPoint = new PointF(pthGrBrush.CenterPoint.X / 2, pthGrBrush.CenterPoint.Y / 2);

                        pthGrBrush.CenterColor = Color.White;

                        Color[] colors = { Color.DarkBlue };
                        pthGrBrush.SurroundColors = colors;

                        foreach (var item in treeMapModel.Items)
                        {
                            pthGrBrush.TranslateTransform((float)item.Rectangle.X, (float)item.Rectangle.Y);
                            pthGrBrush.ScaleTransform((float)item.Rectangle.Width * 2, (float)item.Rectangle.Height * 2);
                            e.Graphics.FillRectangle(pthGrBrush, (float)item.Rectangle.X, (float)item.Rectangle.Y, (float)item.Rectangle.Width, (float)item.Rectangle.Height);
                            pthGrBrush.ResetTransform();
                        }
                    }
                }
            }
        }

        void PaintUnix(PaintEventArgs e)
        {
            if (treeMapModel != null)
            {
                foreach (var item in treeMapModel.Items)
                {
                    e.Graphics.FillRectangle(Brushes.Blue, (float)item.Rectangle.X, (float)item.Rectangle.Y, (float)item.Rectangle.Width, (float)item.Rectangle.Height);
                    e.Graphics.DrawRectangle(Pens.White, (float)item.Rectangle.X, (float)item.Rectangle.Y, (float)item.Rectangle.Width, (float)item.Rectangle.Height);
                }
            }
        }
    }
}
