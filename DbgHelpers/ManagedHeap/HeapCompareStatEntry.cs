using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbgHelpers
{
    public class HeapCompareStatEntry : HeapStatEntry
    {
        public long Count2 { get; set; }
        public long Total2 { get; set; }
        public HeapCompareStatEntry() : base()
        {

        }
    }
}
