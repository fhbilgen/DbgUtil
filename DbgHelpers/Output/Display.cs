using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Diagnostics.Runtime;
using DbgHelpers.DumpAnalysis;

namespace DbgHelpers.Output
{
    public enum OutputDirection
    {
        Console = 0,
        File = 1,
        ConsoleAndFile = 2
    }

    // TODO: Better output management
    // TODO: Add exception handling
    // TODO: Better File configuration, app.config, etc
    public class Display
    {
        //public static string FileName { get; set; }
        //public static string FullPath { get => Environment.CurrentDirectory + @"\" + FileName; }
        public static string FullPath { get; set; }

        public static void StartFile()
        {            
            FileStream fs = new FileStream(FullPath, FileMode.Create, FileAccess.Write);
            fs.Close();
        }

        public static string BuildRuntimeInfo(Analyse analyse)
        {
            var clrversions = analyse.CLRVersions;
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append($"{System.Environment.NewLine}INITIALIZED RUNTIMES{System.Environment.NewLine}");
            stringBuilder.Append($"======================={System.Environment.NewLine}");

            stringBuilder.Append($"{"CLR Version",-15}{"CLR File",-100}{"CLR Symbol File",-100}{"DAC Version", -15}{"DAC Arch", -10}{"DAC File", -100}{System.Environment.NewLine}");
            foreach (ClrInfo info in clrversions)
            {
                DacInfo dacInfo = info.DacInfo;                
                stringBuilder.Append($"{info.Version.ToString(),-15}{info.ModuleInfo?.FileName,-100}{info.ModuleInfo?.Pdb?.ToString(),-100}{dacInfo.Version.ToString(),-15}{dacInfo.TargetArchitecture.ToString(),-10}{dacInfo?.PlatformAgnosticFileName?.ToString(), -100}{System.Environment.NewLine}");
            }

            return stringBuilder.ToString();

        }
        public static string BuildDomains(Analyse analyse)
        {
            var domains = analyse.AppDomains;
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append($"{System.Environment.NewLine}APPLICATION DOMAIN LIST{System.Environment.NewLine}");
            stringBuilder.Append($"======================={System.Environment.NewLine}");
            
            stringBuilder.Append($"{"Domain ID", -10}{"Domain Name", -25}{"Number of Modules", -26} {System.Environment.NewLine}");
            foreach (ClrAppDomain dom in domains)
                stringBuilder.Append($"{dom.Id, -10}{dom.Name, -25}{dom.Modules.Count(), -26}{System.Environment.NewLine}" );

            return stringBuilder.ToString();
        }

        // TODO: Better Debuggable Module output
        public static string BuildModules( Analyse analyse)
        {
            var modules = analyse.Modules;

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append($"{System.Environment.NewLine}MODULE LIST{System.Environment.NewLine}");
            stringBuilder.Append($"======================={System.Environment.NewLine}");

            stringBuilder.Append($"{"Module Name", -200}{"AppDomain", -25}{"PdbPath", -200}{"DebugMode", -100}{"Dynamic",7}{"PEFile",8}{System.Environment.NewLine}");

            foreach (ClrModule mod in modules.OrderBy(module => module.Name).ThenBy(module => module.AppDomain.Name))
                stringBuilder.Append($"{mod.Name, -200}{mod.AppDomain?.Name,-25}{mod.Pdb?.Path, -200}{mod.DebuggingMode,-100}{ (mod.IsDynamic ? "Yes" : "No"),7}{ (mod.IsPEFile ? "Yes" : "No"),8} {System.Environment.NewLine}");
                        
            return stringBuilder.ToString();
        }

        public static string BuildThreads(Analyse analyse)
        {
            var threads = analyse.Threads;

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append($"{System.Environment.NewLine}THREAD LIST{System.Environment.NewLine}");
            stringBuilder.Append($"======================={System.Environment.NewLine}");

            stringBuilder.Append($"{"Mng Id",-7}{"OS Id",8}{"Lock Count",11}{"Alive",6}{"GC Mode",15} {"Exception",-50}{"Aborted",-8}{"Aborting",-9}{"Background",-11}{"DebugSuspended",-15}{"GCSuspendPending",-17}{"UserSuspended",-15}");
            stringBuilder.Append($"{"CoInit", -7}{"MTA", -4}{"STA", -4}{"Unstarted", -10}{System.Environment.NewLine}");

            foreach (ClrThread th in threads)
            {
                stringBuilder.Append($"{th.ManagedThreadId,-7}{th.OSThreadId,8:x6}{th.LockCount,11}{(th.IsAlive?"Yes":"No"),6}{th.GCMode,15}{th.CurrentException?.Type.Name,-50}{(th.IsAborted?"Yes":"No"),8}");
                stringBuilder.Append($"{(th.IsAbortRequested?"Yes":"No"),9}{(th.IsBackground?"Yes":"No"),11}{(th.IsDebugSuspended?"Yes":"No"), 15}{(th.IsGCSuspendPending?"Yes":"No"),17}{(th.IsUserSuspended?"Yes":"No"),15}");
                stringBuilder.Append($"{(th.IsCoInitialized?"Yes":"No"),7}{(th.IsMTA?"Yes":"No"), 4}{(th.IsSTA?"Yes":"No"),4}{(th.IsUnstarted?"Yes":"No"),10}{System.Environment.NewLine}");
            }

            return stringBuilder.ToString();
        }

        public static string BuildManagedCallStacks(Analyse analyse )
        {
            StringBuilder stringBuilder = new StringBuilder();

            var threads = analyse.Threads;
            stringBuilder.Append($"{System.Environment.NewLine}CALLSTACK OF ALL MANAGED THREADS{System.Environment.NewLine}");
            stringBuilder.Append($"======================={System.Environment.NewLine}");
                        
            foreach (ClrThread th in threads)
            {
                IEnumerable<ClrStackFrame> stacktrace = th.EnumerateStackTrace();

                if (th.IsAlive && stacktrace.Count() > 1)
                {
                    // Create a new section for the next thread's callstack
                    stringBuilder.Append($"{System.Environment.NewLine}[{th.ManagedThreadId}:{th.OSThreadId:x6}]{System.Environment.NewLine}");
                    stringBuilder.Append($"{"Stack Pointer",-18}{"Instruction Pointer",-20}{"Frame Type",-50}{"MethodDesc",-20}{"Function Name",-300}{System.Environment.NewLine}");

                    foreach (ClrStackFrame frame in stacktrace)
                        stringBuilder.Append($"{frame.StackPointer,-18:x16}{frame.InstructionPointer,-20:x16}{frame.FrameName,-50}{frame?.Method?.MethodDesc,-20:x16}{frame.Method?.ToString(),-300}{System.Environment.NewLine}");

                    
                    Dictionary<ulong, string> stackObjects = new Dictionary<ulong, string>();

                    /*****************************************************************************************************************
                    *** The following part is retrieved from the ClrStack.cs from 
                    *** https://github.com/microsoft/clrmd/blob/master/src/Samples/ClrStack/ClrStack.cs 
                    *** I first tried my chance with EnumerateStackRoots. But it did not work. Maybe, I've missed something.
                    ****************************************************************************************************************/
                    
                    // We'll need heap data to find objects on the stack.
                    ClrHeap heap = analyse.CLRRuntime.Heap;
                    
                    // Walk each pointer aligned address on the stack.  Note that StackBase/StackLimit
                    // is exactly what they are in the TEB.  This means StackBase > StackLimit on AMD64.
                    ulong start = th.StackBase;
                    ulong stop = th.StackLimit;

                    // We'll walk these in pointer order.
                    if (start > stop)
                    {
                        ulong tmp = start;
                        start = stop;
                        stop = tmp;
                    }

                    // Walk each pointer aligned address.  Ptr is a stack address.
                    for (ulong ptr = start; ptr <= stop; ptr += (uint)IntPtr.Size)
                    {
                        // Read the value of this pointer.  If we fail to read the memory, break.  The
                        // stack region should be in the crash dump.
                        if (!analyse.DataTarget.DataReader.ReadPointer(ptr, out ulong obj))
                            break;
                        
                        // 003DF2A4 
                        // We check to see if this address is a valid object by simply calling
                        // GetObjectType.  If that returns null, it's not an object.
                        ClrType type = heap.GetObjectType(obj);
                        if (type == null)
                            continue;
                        
                        // Don't print out free objects as there tends to be a lot of them on
                        // the stack.
                        if (!type.IsFree)
                        {
                            //Console.WriteLine("{0,16:X} {1,16:X} {2}", ptr, obj, type.Name);
                            try
                            {
                                //stackObjects.Add(ptr, type.Name);
                                stackObjects.Add(obj, type.Name);
                            }
                            catch (ArgumentException)
                            { }
                            catch (NullReferenceException)
                            { }
                        }

                    }

                    if (stackObjects.Count > 0)
                    {
                        stringBuilder.Append($"{System.Environment.NewLine}Objects referenced by [{th.ManagedThreadId}:{th.OSThreadId:x6}]{System.Environment.NewLine}");
                        stringBuilder.Append($"{"Object Address",-18}{"Object Type Name",-300}{System.Environment.NewLine}");

                        foreach (KeyValuePair<ulong, string> stackobject in stackObjects)
                            stringBuilder.Append($"{stackobject.Key,-18:x16}{stackobject.Value,-300}{System.Environment.NewLine}");

                        stackObjects.Clear();
                    }

                }
                
            }

            return stringBuilder.ToString();
        }

        public static void DumpString( string data,  OutputDirection od )
        {
            if (od == OutputDirection.Console || od == OutputDirection.ConsoleAndFile)
                Console.WriteLine(data);
                        
            if ( od == OutputDirection.File || od == OutputDirection.ConsoleAndFile)
                using (FileStream fs = new FileStream(FullPath, FileMode.Append, FileAccess.Write)  )
                {
                    using (StreamWriter wr = new StreamWriter(fs))
                        wr.Write(data);
                }
        }
    }
}
