using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbgHelpers.StackEngine;
using DbgHelpers.Utilities;

namespace DbgHelpers
{

    public class UniqueCallStack: CallStack
    {
        public UniqueCallStack()
            :base()
        {
            SameThreads = new List<string>();
        }

        public List<string> SameThreads { get; set; }

    }

    public class Process
    {
        public List<CallStack> AllCallStacks { get; set; }        
        private Dictionary<byte[], UniqueCallStack> UniqueCallStacks;
        private Dictionary<byte[], UniqueCallStack> SortedUniqueCallStacks;
        private List<byte[]> UniqueHashCodes = new List<byte[]>();
        private Dictionary<byte[], List<string>> CallStackGroups = new Dictionary<byte[], List<string>>();
        

        public Process()
        {
            UniqueCallStacks = new Dictionary<byte[], UniqueCallStack>();
            SortedUniqueCallStacks = new Dictionary<byte[], UniqueCallStack>();
        }

        private void FindUniqueCallStacks()
        {
            foreach (CallStack cs in AllCallStacks)
            {
                bool exists = false;
                UniqueCallStack uniqueCallStack;

                foreach (byte[] hashCode in UniqueHashCodes)
                    if (HashHelper.HashCompare(hashCode, cs.StackHash))
                    {
                        exists = true;
                        CallStackGroups[hashCode].Add(cs.ThreadID);
                        UniqueCallStacks[hashCode].SameThreads.Add(cs.ThreadID);
                    }

                if (!exists)
                {
                    UniqueHashCodes.Add(cs.StackHash);

                    CallStackGroups.Add(cs.StackHash, new List<string>());
                    CallStackGroups[cs.StackHash].Add(cs.ThreadID);

                    uniqueCallStack = new UniqueCallStack();
                    uniqueCallStack.Frames = cs.Frames;
                    uniqueCallStack.SameThreads.Add(cs.ThreadID);
                    uniqueCallStack.StackHash = cs.StackHash;
                    uniqueCallStack.ThreadID = string.Empty;
                    UniqueCallStacks.Add(cs.StackHash, uniqueCallStack);
                }
            }
        }


        private void SortCallStacks()
        {

            int loopCount = UniqueCallStacks.Count;

            for (int i = 0; i != loopCount; i++)
            {
                byte[] currentHash = null;
                int currentCount = 0;

                foreach (var item in UniqueCallStacks)
                {
                    if (item.Value.SameThreads.Count > currentCount)
                    {
                        currentHash = item.Key;
                        currentCount = item.Value.SameThreads.Count;
                    }
                }

                UniqueCallStack uniqueCallStack = new UniqueCallStack();
                uniqueCallStack.Frames = UniqueCallStacks[currentHash].Frames;
                uniqueCallStack.SameThreads = UniqueCallStacks[currentHash].SameThreads;
                uniqueCallStack.StackHash = UniqueCallStacks[currentHash].StackHash;

                SortedUniqueCallStacks.Add(currentHash, uniqueCallStack);
                UniqueCallStacks.Remove(currentHash);
            }
            

        }

        public void ProcessCallStacks()
        {
            FindUniqueCallStacks();
            SortCallStacks();
        }


        public void ShowProcessInfo()
        {
            Console.WriteLine("========================================================");
            Console.WriteLine($"There are {AllCallStacks.Count} threads in the process.");
            Console.WriteLine("========================================================");
            Console.WriteLine();
        }
        public void ShowCallStackGroups()
        {
            int i = 1;
            StringBuilder sb = new StringBuilder();
            foreach( var item in SortedUniqueCallStacks)
            {
                //Console.WriteLine($"Thread group: {HashHelper.ByteArrayToString(item.Key)}");

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

        public void OutputToFile(string path)
        {
            int i = 1;
            StringBuilder sb = new StringBuilder();

            using (StreamWriter streamWriter = new StreamWriter(path))
            {

                streamWriter.WriteLine("========================================================");
                streamWriter.WriteLine($"There are {AllCallStacks.Count} threads in the process.");
                streamWriter.WriteLine("========================================================");
                streamWriter.WriteLine();


                foreach (var item in SortedUniqueCallStacks)
                {

                    streamWriter.WriteLine($"Thread group: {i++}");
                    streamWriter.WriteLine($"Number of threads with same callstack: {item.Value.SameThreads.Count}");
                    streamWriter.Write($"Threads with same callstacks: ");
                    foreach (string threadid in item.Value.SameThreads)
                        sb.Append(" " + threadid + ",");

                    sb.Remove(sb.Length - 1, 1);
                    streamWriter.Write(sb.ToString());
                    sb.Clear();

                    streamWriter.WriteLine();
                    streamWriter.WriteLine($"CallStack");
                    streamWriter.WriteLine($"------------------");
                    streamWriter.WriteLine($"FrameNo\tFunction");
                    foreach (StackFrame frm in item.Value.Frames)
                    {
                        streamWriter.WriteLine($"{frm.Sequence}\t{frm.Function}");
                    }

                    streamWriter.WriteLine();
                }


                streamWriter.Close();
            }
        }
        //public static void OutputToFile(List<HeapCompareStatEntry> result, string path)
        //{
        //    string line;
        //    using (StreamWriter streamWriter = new StreamWriter(path))
        //    {

        //        streamWriter.WriteLine("ClassName\tCount1\tCount2\tCountDelta\tTotal1\tTotal2\tTotalDelta");

        //        //Console.WriteLine("MT\tClassName\tCount1\tCount2\tCountDelta\tTotal1\tTotal2\tTotalDelta");

        //        foreach (HeapCompareStatEntry hcse in result)
        //        {
        //            line = $"{hcse.ClassName}\t{hcse.Count}\t{hcse.Count2}\t{hcse.Count2 - hcse.Count}\t{hcse.Total}\t{hcse.Total2}\t{hcse.Total2 - hcse.Total}";
        //            //Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}", hcse.MT, hcse.ClassName, hcse.Count, hcse.Count2, hcse.Count2 - hcse.Count, hcse.Total, hcse.Total2, hcse.Total2 - hcse.Total);
        //            streamWriter.WriteLine(line);
        //            //  Console.WriteLine(line);
        //        }

        //        streamWriter.Close();
        //    }
        //}

    }
}
