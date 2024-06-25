using System;
using System.Collections.Generic;

using DbgHelpers;

namespace CountOfInstances
{
    class Program
    {
        


        static void Main(string[] args)
        {
                      
            if (args.Length < 4)
            {
                Console.WriteLine($"HC <PathOfFirstFile> <PathOfSecondFile> <PathOfOutputFile> <true|false>");
                return;
            }
            
            // MergedHeaps = CompareTwoHeaps(args[0], args[1]);  Make the comparision based on the type name always !!!
            bool fRemoveSameCount = false;
            if (string.Equals(args[3].ToLower(), "true"))
                fRemoveSameCount = true;

            List<HeapCompareStatEntry> MergedHeaps = new List<HeapCompareStatEntry>();
            MergedHeaps = ManagedHeapOperations.CompareTwoHeapsBasedOnTypeName(args[0], args[1], fRemoveSameCount);

            ManagedHeapOperations.OutputToFile(MergedHeaps, args[2]);
            Console.WriteLine("Completed");
            //Console.ReadLine();

            //Console.WriteLine("MT\tClassName\tCount1\tCount2\tCountDelta\tTotal1\tTotal2\tTotalDelta");
            //foreach (HeapCompareStatEntry hcse in MergedHeaps)
            //{
            //    Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}", hcse.MT, hcse.ClassName, hcse.Count, hcse.Count2, hcse.Count2 - hcse.Count, hcse.Total, hcse.Total2, hcse.Total2 - hcse.Total);
            //}

            // virtual Memory region Compare
            //MemoryRegions mr1 = new MemoryRegions();
            //MemoryRegions mr2 = new MemoryRegions();
            //MemoryRegions mrDiff = new MemoryRegions();
            //mr1.InitializeMemoryRegions(@"F:\TempData\jiankui\img3.txt");
            //mr2.InitializeMemoryRegions(@"F:\TempData\jiankui\img2.txt");

            //mrDiff.MemRegs = MemoryRegions.Diff(mr2.MemRegs, mr1.MemRegs);

            //foreach (MemoryRegionEntry mer in mrDiff.MemRegs)
            //    Console.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7} ", mer.BaseAddress.ToString("X"), mer.EndAddress.ToString("X"), mer.RegionSize.ToString("X"), mer.Type, mer.State, mer.Protection, mer.Usage, mer.MoreInfo);


        }
    }
}
