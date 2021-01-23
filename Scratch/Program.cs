using System;

namespace Scratch
{
    class Program
    {
        // TODO: Add Stack Top Param so that more similar stacks get grouped together
        static void Main(string[] args)
        {

            if (args.Length == 0)
            {
                Console.WriteLine("No stack file is specified");
                return;
            }

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
