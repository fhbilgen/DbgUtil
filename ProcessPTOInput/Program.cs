using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;


namespace ProcessPTOInput
{
    internal class Program
    {        
        static void Main(string[] args)
        {
            //if (args.Length == 0)
            //{
            //    Console.WriteLine("Please provide the root folder path as an argument.");
            //    return;
            //}

            Utility.Initialize();

            // Already done in analysis.IISDotnetFrameworkAnalysis()
            //StackOperations so = new StackOperations(Utility.rootFolder);
            //so.ProcessAllFolders().Wait();

            //Analysis analysis = new Analysis(Utility.rootFolder);
            //analysis.IISDotnetFrameworkAnalysis();

            IISLogOperations iisLogOperations = new IISLogOperations(Utility.normalDayIISLogPath, Utility.heavyDayIISLogPath, Utility.mergedIISLogPath);
            iisLogOperations.ProcessIISLogOutput();

        }
        
    }
}
    
