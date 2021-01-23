using System;
using System.Collections.Generic;
using Microsoft.Diagnostics.Runtime;

namespace DbgHelpers.DumpAnalysis
{
    interface IAnalyse
    {
        void Initialize();
        List<ClrAppDomain> AppDomains { get; }
        List<ClrModule> Modules { get; }
        List<ClrThread> Threads { get; }
        List<object> Stacks { get; set; }
        List<object> HeapInfo { get; set; }
        List<object> GenerationLists { get; set; }
    }
}
