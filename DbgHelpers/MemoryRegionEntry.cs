using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DbgHelpers
{

    public static class MemoryInformation
    {
        public static string[] MemoryStates = { "MEM_COMMIT", "MEM_FREE", "MEM_RESERVE" };
        public static string[] AllocationProtections = {"PAGE_EXECUTE", "PAGE_EXECUTE_READ", "PAGE_EXECUTE_READWRITE", "PAGE_EXECUTE_WRITECOPY", "PAGE_NOACCESS", "PAGE_READONLY", "PAGE_READWRITE", "PAGE_WRITECOPY", "PAGE_TARGETS_INVALID", "PAGE_TARGETS_NO_UPDATE", "PAGE_GUARD", "PAGE_NOCACHE", "PAGE_WRITECOMBINE" };
        public static string[] PageTypes = { "MEM_IMAGE", "MEM_MAPPED", "MEM_PRIVATE" };
    }
    
    public class MemoryRegionEntry
    {
        public ulong BaseAddress { get; set; }      //Field 1
        public ulong EndAddress { get; set; }       //Field 2
        public ulong RegionSize { get; set; }       //Field 3
        public string Type { get; set; }            //Field 4
        public string State { get; set; }           //Field 5
        public string Protection { get; set; }         //Field 6
        public string Usage { get; set; }           //Field 7
        public string MoreInfo { get; set; }        //Field 8
        public MemoryRegionEntry()
        {
            
        }
        
    }
}
