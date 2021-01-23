using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbgHelpers.Utilities;

namespace DbgHelpers.StackEngine
{
    public class StackParser
    {
        
        public static StackFrame GetFrame(string RawFrame)
        {
            StackFrame frame = new StackFrame();

            frame.Sequence = int.Parse(RawFrame.Substring(0, StackHelper.FrameStop), System.Globalization.NumberStyles.HexNumber);
            frame.Function = RawFrame.Substring(StackHelper.FunctionNameStart - 1, RawFrame.Length - StackHelper.FunctionNameStart+1);

            return frame;
        }

        public static List<int> GetStackStarts(string[] Stacks)
        {
            List<int> tags = new List<int>();
            int lineNo = 0;

            foreach(string line in Stacks)
            {
                if (line.IndexOf( StackHelper.NewStackTag ) != -1)
                    tags.Add(lineNo);

                lineNo++;
            }

            return tags;
        }


        public static string GetThreadID(string line)
        {
            string threadIDStr = string.Empty;
            //int threadIDInt;
            int threadIDStart = 0;

            for(int i = StackHelper.ThreadIDStart-1; i!=line.Length; i++)
                if ( line[i].CompareTo(StackHelper.ThreadSeparator ) == 0  )
                    threadIDStart = i;

            // For some reason the ProcessID.ThreadID pair is not found
            if (threadIDStart == 0)
                return string.Empty;

            for (int i = threadIDStart+1; i != line.Length; i++)
                if (char.IsWhiteSpace(line[i]))
                    break;
                else
                    threadIDStr += line[i];


            //int.TryParse(threadIDStr, out threadIDInt);

            return threadIDStr;
        }

        public static List<CallStack> ProcessStackFile(string[] lines)
        {
            List<CallStack> stacks = new List<CallStack>();
            List<int> stackStarts = GetStackStarts(lines);
            StringBuilder sb = new StringBuilder();

            foreach (int newStackLine in stackStarts)
            {
                CallStack currentStack = new CallStack();
                sb.Clear();

                currentStack.ThreadID = GetThreadID(lines[newStackLine]);
                for (int currentFrame = newStackLine + 2; currentFrame != lines.Length; currentFrame++)
                {
                    // These conditions may evolve in time.
                    // 9-October-2018, faikb, added the symbol resolution error skipping
                    // 11-October, faikb, added the symbol resolution warning skipping

                    // In case of the following
                    //0:000 > .logclose
                    //Closing open log file F:\TempData\JollyTur\20181013\CallStacks.txt
                    // The FormatException happened. Hence, added the exception handler block.

                    try
                    {
                        if (string.IsNullOrEmpty(lines[currentFrame]) || string.IsNullOrWhiteSpace(lines[currentFrame]) || lines[currentFrame].Contains("ERROR") || lines[currentFrame].Contains("WARNING"))
                            break;
                        else
                        {
                            currentStack.Frames.Add(GetFrame(lines[currentFrame]));
                        }
                    }
                    catch(FormatException fe)
                    {
                        Console.WriteLine("EXCEPTION HAPPENED");
                        Console.WriteLine("==================");
                        Console.WriteLine($"Message: {fe.Message}");
                        Console.WriteLine($"Input Line No: {currentFrame}");
                        Console.WriteLine($"Input Line: {lines[currentFrame]}");
                    }
                }

                foreach(StackFrame frm in currentStack.Frames)
                    sb.Append(frm.Function);

                currentStack.StackHash = HashHelper.HashGenerate(sb.ToString());
                stacks.Add(currentStack);
            }


            return stacks;
        }
    }
}
