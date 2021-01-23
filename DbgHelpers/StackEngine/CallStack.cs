using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbgHelpers.StackEngine
{
    public class CallStack
    {
        public List<StackFrame> Frames { get; set; }
        public string ThreadID { get; set; }
        public byte[] StackHash { get; set; }

        public CallStack()
        {
            Frames = new List<StackFrame>();
        }
    }

}
