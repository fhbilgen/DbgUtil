using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbgHelpers
{
    public enum ComparisionMethods
    {
        CompareNetHeap = 1,
        CompareMemPages
    }
    public class ComparisionUtility
    {
        public string File1 { get; set; }
        public string File2 { get; set; }
        public bool RemoveSameCount { get; set; }

        public ComparisionMethods ComparisionMethod { get; set; }
        
        public string CalculateReportFileName(string InputFile)
        {
            StringBuilder sb = new StringBuilder();
            char[] delimiterChars = { '\\' };

            var strArr = InputFile.Split(delimiterChars);
            

            for (int i = 0; i != strArr.Length; i++)
                sb.Append(strArr[i]);

            sb.Append("\\Report.txt");

            return sb.ToString();
        }

        public List<HeapStatEntry> InitializeObjectList(string strPath)
        {
            List<HeapStatEntry> objectsInHeap = new List<HeapStatEntry>();
            string strLine = "";
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
                    long cnt = Convert.ToInt64(strArr2[1].Trim());
                    long sz = Convert.ToInt64(strArr2[2].Trim());
                    hse = new HeapStatEntry { ClassName = strArr2[3]/*, MT = strArr2[0]*/, Count = Convert.ToInt64(strArr2[1].Trim()), Total = Convert.ToInt64(strArr2[2].Trim()) };
                    objectsInHeap.Add(hse);
                }
            }

            return objectsInHeap;
        }

        public List<HeapStatEntry> GetMultiplesCountObjects(long multipleOf, List<HeapStatEntry> objectsInHeap)
        {
            List<HeapStatEntry> multiples = new List<HeapStatEntry>();

            foreach (HeapStatEntry HSE in objectsInHeap)
            {
                if ((HSE.Count % multipleOf) == 0)
                    multiples.Add(HSE);
            }

            return multiples;
        }

        public List<HeapCompareStatEntry> CompareTwoHeapsBasedOnTypeName()
        {
            List<HeapStatEntry> heap1 = new List<HeapStatEntry>();
            List<HeapStatEntry> heap2 = new List<HeapStatEntry>();
            List<HeapCompareStatEntry> diff = new List<HeapCompareStatEntry>();
            HeapCompareStatEntry hcse = null;
            bool foundMatchingEntry = false;

            heap1 = InitializeObjectList(File1);
            heap2 = InitializeObjectList(File2);

            foreach (HeapStatEntry hse1 in heap1)
            {
                foundMatchingEntry = false;
                foreach (HeapStatEntry hse2 in heap2)
                {
                    // If the object instance is found in the second heap then add the one from the first heap and the second heap stats
                    if (hse1.ClassName.Equals(hse2.ClassName))
                    //if (hse1.MT.Equals(hse2.MT))
                    {
                        if ((hse1.Count == hse2.Count) && RemoveSameCount)
                            break;

                        hcse = new HeapCompareStatEntry();
                        foundMatchingEntry = true;
                        hcse.ClassName = hse1.ClassName;
                        hcse.Count = hse1.Count;
                        hcse.Count2 = hse2.Count;
                        //hcse.MT = hse1.MT;
                        hcse.Total = hse1.Total;
                        hcse.Total2 = hse2.Total;
                        diff.Add(hcse);
                        heap2.Remove(hse2);
                        break;
                    }
                }// end of inner foreach

                // If the object instance is not found in the second heap then add the one from the first heap and set the secondary values to zero
                if (!foundMatchingEntry)
                {
                    hcse = new HeapCompareStatEntry();
                    hcse.ClassName = hse1.ClassName;
                    hcse.Count = hse1.Count;
                    hcse.Count2 = 0;
                    //hcse.MT = hse1.MT;
                    hcse.Total = hse1.Total;
                    hcse.Total2 = 0;
                    diff.Add(hcse);
                }
            }

            // Add all the remaining elements from Heap#2

            foreach (HeapStatEntry hse2 in heap2)
            {
                hcse = new HeapCompareStatEntry();
                hcse.ClassName = hse2.ClassName;
                hcse.Count = 0;
                hcse.Count2 = hse2.Count;
                //hcse.MT = hse2.MT;
                hcse.Total = 0;
                hcse.Total2 = hse2.Total;
                diff.Add(hcse);
            }// end of inner foreach

            return diff;
        }
    }
}
