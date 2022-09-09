using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbgHelpers.DumpAnalysis;
using DbgHelpers.Output;


namespace ad
{
    class Program
    {
        static void Main(string[] args)
        {
            string str;
            string inputFileName="", outputFileName="";

            if ( args.Count() > 0 )
            {
                inputFileName = args[0];
                outputFileName = inputFileName + ".txt";
            }
            else
            {
                Console.WriteLine("Please provide an input dumpfile full path");
                return;
            }


            //Analyse analyse = new Analyse(@"F:\TempData\someprocess.exe_201108_010030.dmp");
            Analyse analyse = new Analyse(inputFileName);
            analyse.Initialize();

            //Display.FileName = "Deneme.txt";
            //Display.FileName = outputFileName;
            Display.FullPath = outputFileName;
            Display.StartFile();
            
            str = Display.BuildRuntimeInfo(analyse);
            Display.DumpString(str, OutputDirection.ConsoleAndFile);

            str = Display.BuildDomains(analyse);
            Display.DumpString(str, OutputDirection.ConsoleAndFile);
            
            str = Display.BuildModules(analyse);
            Display.DumpString(str, OutputDirection.ConsoleAndFile);
            
            str = Display.BuildThreads(analyse);
            Display.DumpString(str, OutputDirection.ConsoleAndFile);

            str = Display.BuildManagedCallStacks(analyse);
            Display.DumpString(str, OutputDirection.ConsoleAndFile);
            
        }
    }
}
