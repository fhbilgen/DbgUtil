using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbgHelpers
{
    public class HeapStatEntry
    {
       // public string  MT {get; set;}
        public long     Count   { get; set; }
        public long    Total { get; set;}
        public string  ClassName {get; set;}
        
        public HeapStatEntry()
        {
        }
    }
}
