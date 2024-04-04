using System;

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

            if (args.Length == 0)
            {
                Console.WriteLine("No stack file is specified");
                Console.WriteLine("The usage is: gs.exe filename [x86 | 32]");
                return;
            }

            if (args.Length == 1 && ( args[0].ToLower().Equals("help") || args[0].ToLower().Equals("-?")) )
            {
                Console.WriteLine("The usage is: gs.exe filename [x86 | 32]");
                return;
            }

            // The application was crashing when 32-bit stacks were feeded.
            // This is not an ideal fix but it works for now
            if (args.Length == 2)
                if (args[1].ToLower().Equals("x86") || args[1].ToLower().Equals("32"))
                    DbgHelpers.StackEngine.StackHelper.FunctionNameStart = 22;

            // Read each line of the file into a string array. Each element
            // of the array is one line of the file.
            string[] lines = System.IO.File.ReadAllLines(args[0]);

            //// Display the file contents by using a foreach loop.
            //System.Console.WriteLine("Contents of WriteLines2.txt = ");
            //foreach (string line in lines)
            //{
            //    // Use a tab to indent each line of the file.
            //    Console.WriteLine("\t" + line);
            //}

            DbgHelpers.Process p = new DbgHelpers.Process();
            p.AllCallStacks = DbgHelpers.StackEngine.StackParser.ProcessStackFile(lines);
            p.ProcessCallStacks();
            p.ShowProcessInfo();
            p.ShowCallStackGroups();
            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            //System.Console.ReadKey();



        }
    }
}
