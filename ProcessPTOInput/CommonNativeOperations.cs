using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessPTOInput
{
    internal class CommonNativeOperations
    {
        public string[] GetCommands()
        {
            return new string[] { ".echo CNO1", ".time", ".echo CNO2", "!cpuid", ".echo CNO3", "!runaway", ".echo CNO4", "!peb", ".echo CNO5", "!address -summary", ".echo CNO6", "|" };
        }
    }
}
