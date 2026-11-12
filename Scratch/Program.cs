using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using DbgHelpers.StackEngine;

namespace Scratch
{
    class Program
    {
        // Error case:
            //384  Id: 2da8.109c Suspend: 0 Teb: fe9e9000 Unfrozen
            //WARNING: Stack pointer is outside the normal stack bounds.Stack unwinding can be inaccurate.

        // TODO: Add Stack Top Param so that more similar stacks get grouped together
        static void Main(string[] args)
        {

            if (args.Length == 0 || (args.Length == 1 && (args[0].ToLower().Equals("help") || args[0].ToLower().Equals("-?"))) )
            {                
                Console.WriteLine("The usage is: gs.exe <PathOfInputFile> <PathOfOutputFile> [x86|32]");             
                return;
            }

            //if (args.Length == 1 && ( args[0].ToLower().Equals("help") || args[0].ToLower().Equals("-?")) )
            //{
            //    Console.WriteLine("The usage is: gs.exe <PathOfInputFile> <PathOfOutputFile> [x86 | 32]");
            //    return;
            //}

            // The application was crashing when 32-bit stacks were feeded.
            // This is not an ideal fix but it works for now
            if (args.Length == 3)
                if (args[2].ToLower().Equals("x86") || args[2].ToLower().Equals("32"))
                    StackHelper.FunctionNameStart = 22;

            CleanStackFile(args[0]);



            // Read each line of the file into a string array. Each element
            // of the array is one line of the file.
            string[] lines = System.IO.File.ReadAllLines(args[0]);
                       

            DbgHelpers.Process p = new DbgHelpers.Process();
            p.AllCallStacks = StackParser.ProcessStackFile(lines);
            p.ProcessCallStacks();
            //p.ShowProcessInfo();
            //p.ShowCallStackGroups();
            p.OutputToFile(args[1]);
            // Keep the console window open in debug mode.
            //Console.WriteLine("Press any key to exit.");
            //System.Console.ReadKey();


        }

        private static void CleanStackFile(string inputFilePath)
        {
            if (string.IsNullOrWhiteSpace(inputFilePath) || !File.Exists(inputFilePath))
                return;

            List<string> keywords = GetLinesToDeleteFromConfig();

            if (keywords.Count == 0)
                return;

            string tempFilePath = Path.GetTempFileName();
            try
            {
                using (StreamReader reader = new StreamReader(inputFilePath))
                using (StreamWriter writer = new StreamWriter(tempFilePath, false))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        bool shouldDelete = false;
                        foreach (string keyword in keywords)
                        {
                            if (line.Contains(keyword))
                            {
                                shouldDelete = true;
                                break;
                            }
                        }

                        if (!shouldDelete)
                            writer.WriteLine(line);
                    }
                }

                File.Copy(tempFilePath, inputFilePath, true);
            }
            finally
            {
                if (File.Exists(tempFilePath))
                    File.Delete(tempFilePath);
            }
        }

        private static List<string> GetLinesToDeleteFromConfig()
        {
            List<string> keywords = new List<string>();
            string configPath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            if (string.IsNullOrWhiteSpace(configPath) || !File.Exists(configPath))
                return keywords;

            XDocument configDocument = XDocument.Load(configPath);
            XElement linesToDeleteSection = configDocument.Root.Element("LinesToDelete");
            if (linesToDeleteSection == null)
                return keywords;

            foreach (XElement entry in linesToDeleteSection.Elements("add"))
            {
                XAttribute valueAttribute = entry.Attribute("value");
                if (valueAttribute != null && !string.IsNullOrWhiteSpace(valueAttribute.Value))
                    keywords.Add(valueAttribute.Value);
            }

            return keywords;
        }
    }
}
