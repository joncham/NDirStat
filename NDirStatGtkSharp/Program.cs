using System;
using System.Collections.Generic;
using System.Text;
using Gtk;
using NDirStat;

namespace NDirStatGtkSharp
{

    class Program
    {
        static Program()
        {
            string path = Environment.GetEnvironmentVariable("PATH");
            Environment.SetEnvironmentVariable("PATH", path + ";" + @"C:\Users\chambers\Development\GtkSharp\2.12\bin");
        }

        static void Main()
        {
            Application.Init();

            MainWindow window = new MainWindow("NDirStat");
            window.ShowAll();

            Application.Run();
        }
    }
}
