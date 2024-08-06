using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessPTOInput
{
    internal class DotnetFrameworkOperations
    {
        public string[] GetCommands()
        {             
            return new string[] { ".echo DFO1", "!eeversion", ".echo DFO2", ".load spext", ".echo DFO3", "!finddebugmodules", ".echo DFO4", "!threadpool", ".echo DFO5", ".load mex", ".echo DFO6", "!clrperfcounters", ".echo DFO7", "!aspnetperfcounters", ".echo DFO8", "!dae" };
        }
         
    }
}
