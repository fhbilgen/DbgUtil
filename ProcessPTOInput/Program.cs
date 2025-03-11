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

            //Utility.rootFolder = args[0];
            //StackOperations so = new StackOperations(Utility.rootFolder);
            //so.ProcessAllFolders().Wait();

            //Analysis analysis = new Analysis(Utility.rootFolder);
            //analysis.IISDotnetFrameworkAnalysis();

            IISLogOperations iisLogOperations = new IISLogOperations(args[0], args[1], args[2]);
            iisLogOperations.ProcessIISLogOutput();

        }
        
    }
}
    
