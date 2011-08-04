using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.trolltech.qt.gui;
using NDirStat;

namespace NDirStatQtJambi
{
    class TreeMapWidget : QWidget
    {

        DirStatModel model;
        TreeMapModel treeMapModel;

        protected override void paintEvent(QPaintEvent arg__1)
        {


            if (treeMapModel != null)
            {

                QPainter painter = new QPainter();
                painter.begin(this);
                painter.setRenderHint(QPainter.RenderHint.Antialiasing);

                QRadialGradient g = new QRadialGradient();
                foreach (var item in treeMapModel.Items)
                {
                    g.setColorAt(0, QColor.white);
                    g.setColorAt(1, QColor.darkBlue);
                    double x = item.Rectangle.X + item.Rectangle.Width / 4;
                    double y = item.Rectangle.Y + item.Rectangle.Height / 4;
                    g.setCenter(x, y);
                    g.setFocalPoint(x, y);
                    g.setRadius(Math.Max(item.Rectangle.Width, item.Rectangle.Height));
                    painter.setBrush(g);
                    painter.setPen(QPen.NoPen);

                    painter.drawRect(new com.trolltech.qt.core.QRectF(item.Rectangle.X, item.Rectangle.Y, item.Rectangle.Width, item.Rectangle.Height));
                    //pthGrBrush.TranslateTransform((float)item.Rectangle.X, (float)item.Rectangle.Y);
                    //pthGrBrush.ScaleTransform((float)item.Rectangle.Width * 2, (float)item.Rectangle.Height * 2);
                    //e.Graphics.DrawRectangle(Pens.Black, (float)item.Rectangle.X, (float)item.Rectangle.Y, (float)item.Rectangle.Width, (float)item.Rectangle.Height);
                    //e.Graphics.FillRectangle(Brushes.DarkBlue, (float)item.Rectangle.X, (float)item.Rectangle.Y, (float)item.Rectangle.Width, (float)item.Rectangle.Height);
                    //e.Graphics.FillRectangle(pthGrBrush, (float)item.Rectangle.X, (float)item.Rectangle.Y, (float)item.Rectangle.Width, (float)item.Rectangle.Height);
                    //pthGrBrush.ResetTransform();
                }
                painter.end();
            }
            base.paintEvent(arg__1);

        }

        protected override void resizeEvent(QResizeEvent arg__1)
        {
            if (model != null)
            {
                treeMapModel = new TreeMapModel(model, this.width(), this.height());
                this.repaint();
            }
            base.resizeEvent(arg__1);
        }

        public void SetModel(DirStatModel model)
        {
            this.model = model;
            treeMapModel = new TreeMapModel(model, this.width(), this.height());
            this.repaint();
        }
    }

}
