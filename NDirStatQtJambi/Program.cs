using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.trolltech.qt.gui;
using NDirStat;
using java.util;
using System.Diagnostics;

namespace NDirStatQtJambi
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

            QMainWindow window = new MainWindow();

            window.resize(320, 240);
            window.show();
            window.setWindowTitle(QApplication.translate("toplevel", "NDirStatQtJambi"));

            QApplication.exec();
        }
    }
}
