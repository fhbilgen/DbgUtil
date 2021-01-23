using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DbgHelpers
{
    public class ManagedHeapOperations
    {
        public static List<HeapStatEntry> InitializeObjectList(string strPath)
        {
            List<HeapStatEntry> objectsInHeap = new List<HeapStatEntry>();
            string strLine;
            string[] strArr;
            string[] strArr2 = new string[4];
            HeapStatEntry hse;
            int j;

            using (StreamReader sr = new StreamReader(strPath))
            {
                strLine = sr.ReadLine();

                while (!sr.EndOfStream)
                {
                    strLine = sr.ReadLine();


                    strArr = strLine.Split(' ');
                    j = 0;
                    for (int i = 0; i < strArr.Count(); i++)
                    {
                        strArr[i] = strArr[i].Trim();

                        if (strArr[i] != "")
                            strArr2[j++] = strArr[i];
                        if (j == 4)
                        {
                            strArr2[3] = strLine.Substring(strLine.IndexOf(strArr[i]));
                            break;
                        }
                    }

                    string clsnm = strArr2[3].Trim();
                    string mt = strArr2[0].Trim();
                    long cnt;
                    long sz;
                    string strtmp = "";

                    try
                    {
                        cnt = Convert.ToInt64(strArr2[1].Trim());
                    }
                    catch
                    {
                        foreach (char c in strArr2[1].ToCharArray())
                            if (char.IsDigit(c))
                                strtmp += c;

                        cnt = Int64.Parse(strtmp);
                    }

                    try
                    {
                        sz = Convert.ToInt64(strArr2[2].Trim());
                    }
                    catch (System.FormatException)
                    {

                        foreach (char c in strArr2[2].ToCharArray())
                            if (char.IsDigit(c))
                                strtmp += c;

                        sz = Int64.Parse(strtmp);
                    }


                    //hse = new HeapStatEntry { ClassName = strArr2[3], Count = Convert.ToInt64(strArr2[1].Trim()), Total = Convert.ToInt64(strArr2[2].Trim()) };
                    hse = new HeapStatEntry { ClassName = clsnm, Count = cnt, Total = sz };
                    objectsInHeap.Add(hse);
                }
            }

            return objectsInHeap;
        }
        public static List<HeapCompareStatEntry> CompareTwoHeapsBasedOnTypeName(string strPath1, string strPath2, bool fRemoveSameCount)
        {
            List<HeapStatEntry> heap1; 
            List<HeapStatEntry> heap2;
            List<HeapCompareStatEntry> diff = new List<HeapCompareStatEntry>();
            HeapCompareStatEntry hcse = null;
            bool foundMatchingEntry;

            heap1 = InitializeObjectList(strPath1);
            heap2 = InitializeObjectList(strPath2);

            foreach (HeapStatEntry hse1 in heap1)
            {
                foundMatchingEntry = false;
                foreach (HeapStatEntry hse2 in heap2)
                {
                    // If the object instance is found in the second heap then add the one from the first heap and the second heap stats     
                    if (String.Equals(hse1.ClassName, hse2.ClassName))
                    {
                        foundMatchingEntry = true;

                        if ((hse1.Count == hse2.Count) && fRemoveSameCount)
                            break;

                        hcse = new HeapCompareStatEntry
                        {
                            ClassName = hse1.ClassName,
                            Count = hse1.Count,
                            Count2 = hse2.Count,
                            Total = hse1.Total,
                            Total2 = hse2.Total
                        };
                        diff.Add(hcse);
                        //heap2.Remove(hse2);
                        break;
                    }
                }// end of inner foreach

                // If the object instance is not found in the second heap then add the one from the first heap and set the secondary values to zero
                if (!foundMatchingEntry)
                {
                    hcse = new HeapCompareStatEntry
                    {
                        ClassName = hse1.ClassName,
                        Count = hse1.Count,
                        Count2 = 0,
                        Total = hse1.Total,
                        Total2 = 0
                    };
                    diff.Add(hcse);
                }
            }

            // Add all the remaining elements from Heap#2

            foreach (HeapStatEntry hse2 in heap2)
            {
                foundMatchingEntry = false;

                foreach (HeapStatEntry hse1 in heap1)
                {
                    // If the object instance is found in the second heap then add the one from the first heap and the second heap stats                                        
                    if (String.Equals(hse2.ClassName, hse1.ClassName))
                    {
                        foundMatchingEntry = true;
                        break;                        
                    }
                }// end of inner foreach

                if (!foundMatchingEntry)
                {
                    hcse = new HeapCompareStatEntry
                    {
                        ClassName = hse2.ClassName,
                        Count = 0,
                        Count2 = hse2.Count,
                        Total = 0,
                        Total2 = hse2.Total
                    };
                    diff.Add(hcse);
                }
                
            }// end of inner foreach

            return diff;
        }


        public static void OutputToFile(List<HeapCompareStatEntry> result, string path)
        {
            string line;
            using (StreamWriter streamWriter = new StreamWriter(path))
            {

                streamWriter.WriteLine("ClassName\tCount1\tCount2\tCountDelta\tTotal1\tTotal2\tTotalDelta");

                //Console.WriteLine("MT\tClassName\tCount1\tCount2\tCountDelta\tTotal1\tTotal2\tTotalDelta");

                foreach (HeapCompareStatEntry hcse in result)
                {
                    line = $"{hcse.ClassName}\t{hcse.Count}\t{hcse.Count2}\t{hcse.Count2 - hcse.Count}\t{hcse.Total}\t{hcse.Total2}\t{hcse.Total2 - hcse.Total}";
                    //Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}", hcse.MT, hcse.ClassName, hcse.Count, hcse.Count2, hcse.Count2 - hcse.Count, hcse.Total, hcse.Total2, hcse.Total2 - hcse.Total);
                    streamWriter.WriteLine(line);
                  //  Console.WriteLine(line);
                }

                streamWriter.Close();
            }
        }
    }
}
