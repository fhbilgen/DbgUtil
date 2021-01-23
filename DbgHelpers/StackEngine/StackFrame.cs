using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbgHelpers.StackEngine
{
    public class StackFrame
    {
        public int Sequence { get; set; }
        public string Function { get; set; }
    }
}
