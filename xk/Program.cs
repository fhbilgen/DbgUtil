using DbgHelpers;
using DbgHelpers.StackEngine;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace xk
{
    internal class Program
    {

        static string Int64ToIP4(uint ip)
        {
            return new System.Net.IPAddress(ip).ToString();
        }

        static void ConvertListFromHex2Dec(string fileName, string outputFilename)
        {
            string[] lines = System.IO.File.ReadAllLines(fileName);
            // Insteadof displaying the sb variable to console write it into a text file who name is in the outputfilename argument

            using (StreamWriter writer = new StreamWriter(outputFilename))
            {
                foreach (var line in lines)
                {
                    string[] hexValuesSplit = line.Split(',');
                    StringBuilder sb = new StringBuilder();
                    for(int i = 0; i < hexValuesSplit.Length; i++)                    
                    {
                        if (i == 0)
                            sb.Append(hexValuesSplit[i] + " ");
                        else
                            sb.Append(Convert.ToInt32(hexValuesSplit[i], 16) + " ");
                    }
                   
                    writer.WriteLine(sb.ToString());
                }
            }
        }

        static void ConvertComplexListFromHex2Dec(string fileName, string outputFilename)
        {
            string[] lines = System.IO.File.ReadAllLines(fileName);
            // Insteadof displaying the sb variable to console write it into a text file who name is in the outputfilename argument

            for (int i = 0; i!= lines.Count() - 1;i++)
            {
                if ( i % 4 == 0 )
                {
                    lines[i] = lines[i].Remove(0, 20);
                }
            }

            using (StreamWriter writer = new StreamWriter(outputFilename))
            {
                foreach (var line in lines)
                {
                    string[] hexValuesSplit = line.Split(',');
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < hexValuesSplit.Length; i++)
                    {
                        if (i == 0)
                            sb.Append(hexValuesSplit[i] + " ");
                        else
                            sb.Append(Convert.ToInt32(hexValuesSplit[i], 16) + " ");
                    }

                    writer.WriteLine(sb.ToString());
                }
            }
        }

        static void ExecuteQueriesOnStacks(string fileName)
        {

            // Read each line of the file into a string array. Each element
            // of the array is one line of the file.
            string[] lines = System.IO.File.ReadAllLines(fileName);

            //DbgHelpers.Process p = new DbgHelpers.Process();
            //p.AllCallStacks = StackParser.ProcessStackFile(lines);

            var result = lines
            .Select((value, index) => new { value, index })
            //.Where(x => x.value.StartsWith("DynamicClass.lambda_method") && x.index > 0)
            .Where(x => x.value.Contains("DynamicClass.lambda_method") && x.index > 0)
            //.Select(x => new { Current = x.value, Previous = lines[x.index - 1] });
            .Select(x => lines[x.index - 1].Substring(43, lines[x.index - 1].Length - 43));
            //                    ,Previous2 = lines[x.index - 2].Substring(43, lines[x.index - 2].Length - 43),
            //                    Previous3 = lines[x.index - 3].Substring(43, lines[x.index - 3].Length - 43),
            //                    Previous4 = lines[x.index - 4].Substring(43, lines[x.index - 4].Length - 43),
            //                    Previous5 = lines[x.index - 5].Substring(43, lines[x.index - 5].Length - 43),
            //                    Previous6 = lines[x.index - 6].Substring(43, lines[x.index - 6].Length - 43),
            //                    Previous7 = lines[x.index - 7].Substring(43, lines[x.index - 7].Length - 43),
            //                    Previous8 = lines[x.index - 8].Substring(43, lines[x.index - 8].Length - 43),
            //                    Previous9 = lines[x.index - 9].Substring(43, lines[x.index - 9].Length - 43),
            //                    Previous10 = lines[x.index - 10].Substring(43, lines[x.index - 10].Length - 43),
            //                    Previous11 = lines[x.index - 11].Substring(43, lines[x.index - 11].Length - 43),
            //                    Previous12 = lines[x.index - 12].Substring(43, lines[x.index - 12].Length - 43)
            //});


            var grouped = result
            .GroupBy(s => s)
            .Select(g => new { Value = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count);

            foreach (var item in grouped)
            {
                //Console.WriteLine($"{item.Value}: {item.Count}");
                if (item.Value.Equals("mscorlib!System.Reflection.Emit.DynamicMethod.CreateDelegate+0x37"))
                    continue;

                var greaterstack = lines
                .Select((value, index) => new { value, index })
                .Where(x => x.value.Contains(item.Value) && x.index > 1)
                .Select(x => new {
                    Previous = lines[x.index].Substring(43, lines[x.index].Length - 43),
                    Previous1 = lines[x.index - 1].Substring(43, lines[x.index - 1].Length - 43),
                    Previous2 = lines[x.index - 2].Substring(43, lines[x.index - 2].Length - 43),
                    Previous3 = lines[x.index - 3].Substring(43, lines[x.index - 3].Length - 43),
                    Previous4 = lines[x.index - 4].Substring(43, lines[x.index - 4].Length - 43),
                    Previous5 = lines[x.index - 5].Substring(43, lines[x.index - 5].Length - 43),
                    Previous6 = lines[x.index - 6].Substring(43, lines[x.index - 6].Length - 43),
                    Previous7 = lines[x.index - 7].Substring(43, lines[x.index - 7].Length - 43),
                    Previous8 = lines[x.index - 8].Substring(43, lines[x.index - 8].Length - 43),
                    Previous9 = lines[x.index - 9].Substring(43, lines[x.index - 9].Length - 43),
                    Previous10 = lines[x.index - 10].Substring(43, lines[x.index - 10].Length - 43),
                    Previous11 = lines[x.index - 11].Substring(43, lines[x.index - 11].Length - 43),
                    Previous12 = lines[x.index - 12].Substring(43, lines[x.index - 12].Length - 43)
                });

                foreach (var frame in greaterstack)
                {
                    Console.WriteLine($"{frame.Previous12}");
                    Console.WriteLine($"{frame.Previous11}");
                    Console.WriteLine($"{frame.Previous10}");
                    Console.WriteLine($"{frame.Previous9}");
                    Console.WriteLine($"{frame.Previous8}");
                    Console.WriteLine($"{frame.Previous7}");
                    Console.WriteLine($"{frame.Previous6}");
                    Console.WriteLine($"{frame.Previous5}");
                    Console.WriteLine($"{frame.Previous4}");
                    Console.WriteLine($"{frame.Previous3}");
                    Console.WriteLine($"{frame.Previous2}");
                    Console.WriteLine($"{frame.Previous}");
                    Console.WriteLine();
                }

            }

            foreach (var item in grouped)
            {
                Console.WriteLine($"{item.Value}: {item.Count}");
            }
            //foreach (var item in result)
            //{
            //    Console.WriteLine($"{item.Previous}");
            //    //Console.WriteLine($"{item.Current}");
            //    Console.WriteLine();
            //}

    }

        static void FindFuncInUniqStackGroup(string fileName, string uniqString)
        {
            int i = 1;
            Dictionary<byte[], UniqueCallStack> SortedUniqueCallStacks;

            string[] lines = System.IO.File.ReadAllLines(fileName);

            DbgHelpers.Process p = new DbgHelpers.Process();
            p.AllCallStacks = StackParser.ProcessStackFile(lines);
            p.ProcessCallStacks();

            SortedUniqueCallStacks = p.SortedUniqStacks;

            foreach (var item in SortedUniqueCallStacks)
            {
                
                StringBuilder sb = new StringBuilder();

                if (item.Value.Frames.Exists(x=> x.Function.Contains(uniqString)))
                {                    
                    Console.WriteLine($"Thread group: {i++}");
                    Console.WriteLine($"Number of threads with same callstack: {item.Value.SameThreads.Count}");
                    Console.Write($"Threads with same callstacks: ");
                    foreach (string threadid in item.Value.SameThreads)
                        sb.Append(" " + threadid + ",");

                    sb.Remove(sb.Length - 1, 1);
                    Console.Write(sb.ToString());
                    sb.Clear();

                    Console.WriteLine();
                    Console.WriteLine($"CallStack");
                    Console.WriteLine($"------------------");
                    Console.WriteLine($"FrameNo\tFunction");
                    foreach (StackFrame frm in item.Value.Frames)
                    {
                        Console.WriteLine($"{frm.Sequence}\t{frm.Function}");
                    }

                    Console.WriteLine();
                }

            }


        }

        static void MethodNamesAndCounts(string fileName)
        {
            string[] lines = System.IO.File.ReadAllLines(fileName);

            
            var grouped = lines
            .GroupBy(s => s)
            .Select(g => new { Value = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count);

            foreach (var item in grouped)
            {
                Console.WriteLine($"{item.Value}: {item.Count}");
            }
        }


        static void Main(string[] args)
        {
            //FindFuncInUniqStackGroup(args[0], "System_Data!SNINativeMethodWrapper.SNIReadSyncOverAsync+0x66");
            //ConvertListFromHex2Dec(args[0], args[1]);
            //ConvertComplexListFromHex2Dec(args[0], args[1]);
            //MethodNamesAndCounts(args[0]);
            //Console.WriteLine("Enter the int value to get the IPv4 address");
            //var ipStr = Console.ReadLine();
            //Console.WriteLine(Int64ToIP4(uint.Parse(ipStr)));
            var scan = new scanMemPageContent(args[0], args[1]);
            scan.ProcessContentAsync(args).Wait();

        }
    }
}
