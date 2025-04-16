using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;


namespace ProcessPTOInput
{
    internal class Program
    {


        static void ProcessIISLogs()
        {
            PreProcessIISLogs ppIISLog = new PreProcessIISLogs(Utility.normalDayIISLogPaths, Utility.heavyDayIISLogPaths);
            //ppIISLog.ProcessIISLogFolder(Utility.normalDayIISLogPaths[0], "normal.txt");
            //ppIISLog.processAllIISLogFolders().Wait();

            IISLogOperations iisLogOperations;
            for (int i = 0; i < Utility.normalDayIISLogPaths.Length; i++)
            {
                iisLogOperations = new IISLogOperations(Utility.normalDayIISLogPaths[i] + "\\" + Utility.NormalDayIISLogSummaryFileName,
                    Utility.heavyDayIISLogPaths[i] + "\\" + Utility.HeavyDayIISLogSummaryFileName,
                    Utility.aggIISLogs[i] + "\\" + Utility.AggregateIISLogSummaryFileName);
                iisLogOperations.ProcessIISLogOutput();
            }
            //IISLogOperations iisLogOperations = new IISLogOperations(args[0], args[1], args[2]);
            //iisLogOperations.ProcessIISLogOutput();
        }

        static void ProcessStacks()
        {
            //Utility.rootFolder = args[0];
            StackOperations so = new StackOperations(Utility.rootFolder);
            so.ProcessAllFolders().Wait();

        }

        static void ProcessFrameworkOperations()
        {
            Analysis analysis = new Analysis(Utility.rootFolder);
            analysis.IISDotnetFrameworkAnalysis();
        }
        static void Main(string[] args)
        {
            //if (args.Length == 0)
            //{
            //    Console.WriteLine("Please provide the root folder path as an argument.");
            //    return;
            //}

            Utility.Initialize();
            Console.WriteLine("Please select a choice:");
            Console.WriteLine("1. Process IIS Logs");
            Console.WriteLine("2. Process Stacks");
            Console.WriteLine("3. Process Framework Operations");
            Console.WriteLine("4. Process All");
            var i = Console.ReadLine();

            switch (i)
            {
                case "1":
                    ProcessIISLogs();
                    break;
                case "2":
                    ProcessStacks();
                    break;
                case "3":
                    ProcessFrameworkOperations();
                    break;
                case "4":
                    ProcessIISLogs();
                    ProcessStacks();
                    ProcessFrameworkOperations();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    break;
            }

            Console.WriteLine("Processing is completed!");

        }
    }
}
    
