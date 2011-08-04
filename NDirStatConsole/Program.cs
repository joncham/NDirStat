using System;
using System.Collections.Generic;
using System.Text;
using NDirStat;

namespace NDirStatConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string directory = Environment.CurrentDirectory;
            if (args.Length == 1)
            {
                directory = args[0];
            }
            ModelBuilder builder = new ModelBuilder();
            DirStatModel model = builder.Build(new NDirInfo(directory));

            PrintDirs(string.Empty, model.GetRoot());
        }

        static void PrintDirs(string indent, DirStat dir)
        {
            Console.WriteLine(indent + "+" + "{0} {1}", dir.Name, dir.Length);

            foreach (FileStat file in dir.Files)
            {
                Console.WriteLine(indent + "  " + "{0} {1}", file.Name, file.Length);
            }

            foreach (DirStat sub in dir.Directories)
            {
                PrintDirs(indent + " ", sub);
            }
        }
    }
}
