using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.trolltech.qt.gui;
using com.trolltech.qt.webkit;
using com.trolltech.qt.core;
using NDirStat;

namespace NDirStatQtWeb
{

    class Program
    {
        static Program()
        {
            string path = Environment.GetEnvironmentVariable("PATH");
            Environment.SetEnvironmentVariable("PATH", path + ";" + @"C:\Users\chambers\Development\qtjambi-4.6.3;C:\Users\chambers\Development\qtjambi-4.6.3\bin");
        }


        [STAThread]
        static void Main(string[] args)
        {
            QApplication application = new QApplication(args);

            MainWindow window = new MainWindow();

            window.resize(320, 240);
            window.show();
            window.setWindowTitle(
           QApplication.translate("toplevel", "NDirStatQtWeb"));

            QApplication.exec();
        }
    }
}
