using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Diagnostics.Runtime;


namespace DbgHelpers.DumpAnalysis
{
    public class Analyse : IAnalyse
    {
        /*Implementation Internals*/
        private DataTarget _dt = null;        
        private ClrInfo _currentCLRVersion = null;
        private List<ClrInfo> _CLRversions = null;
        private ClrRuntime _clrRuntime = null;
        private List<ClrAppDomain> _domains = null;
        private List<ClrModule> _modules = null;
        private List<ClrThread> _threads = null;
                

        public Analyse(string targetName)
        {
            TargetName = targetName;            
        }

        // Initializing all necessary information
        public void Initialize()
        {
            _dt = DataTarget.LoadDump(TargetName);
            _currentCLRVersion = _dt.ClrVersions[0];
            _clrRuntime = _currentCLRVersion.CreateRuntime();

            // Get all of the initialized runtime versions.
            // Later we can warn the user about the multi-runtime case
            _CLRversions = new List<ClrInfo>();
            foreach (ClrInfo info in _dt.ClrVersions)
                _CLRversions.Add(info);

            // Get the Application Domains
            _domains = new List<ClrAppDomain>();
            foreach (ClrAppDomain dom in _clrRuntime.AppDomains)
                _domains.Add(dom);

            // Get the modules in all domains
            _modules = new List<ClrModule>();
            foreach (ClrAppDomain dom in AppDomains)
                foreach (ClrModule mod in dom.Modules)
                    _modules.Add(mod);

            // Get the threads
            _threads = new List<ClrThread>();
            foreach (ClrThread th in _clrRuntime.Threads)
                _threads.Add(th);

            
        }


        /* Properties */

        public DataTarget DataTarget { get => _dt; }

        public string TargetName { get; set; }

        public ClrInfo CurrrentCLRVersion
        {
            get => _currentCLRVersion;
        }

        public List<ClrInfo> CLRVersions
        {
            get => _CLRversions;
        }

        public ClrRuntime CLRRuntime
        {
            get => _clrRuntime;
        }

        public List<ClrAppDomain> AppDomains { get => _clrRuntime.AppDomains.ToList(); }
        public List<ClrModule> Modules { get => _modules; }
        public List<ClrThread> Threads { get => _threads; }
        public List<object> Stacks { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<object> HeapInfo { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<object> GenerationLists { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
