using System;
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
    }
}
