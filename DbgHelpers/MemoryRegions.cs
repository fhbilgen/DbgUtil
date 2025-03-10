using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DbgHelpers
{
    public class MemoryRegions
    {
        public MemoryRegions()
        {
            MemRegs = new List<MemoryRegionEntry>();
        }

        public List<MemoryRegionEntry> MemRegs { set; get; }

        public void InitializeMemoryRegions(string strPath)
        {
            //List<MemoryRegionEntry> regionsInMemory = new List<MemoryRegionEntry>();
            if (MemRegs != null)
                MemRegs.Clear();

            string strLine = "";
            string[] strArr;
            string[] strArr2 = new string[8];
            MemoryRegionEntry mre;
            int moreInfoStartsAt, moreInfoEndsAt;
            int j;

            using (StreamReader sr = new StreamReader(strPath))
            {
                strLine = sr.ReadLine();

                while (!sr.EndOfStream)
                {
                    strLine = sr.ReadLine();


                    strArr = strLine.Split(' ');
                    j = 0;
                    for (int k = 0; k != 8; k++)
                        strArr2[k] = "";

                    for (int i = 0; i < strArr.Count(); i++)
                    {
                        strArr[i] = strArr[i].Trim();

                        if (strArr[i] != "")
                        {
                            if (j >= 0 && j < 3)
                            {
                                strArr2[j] = strArr[i];
                                j++;
                                continue;
                            }

                            // If this is the "Type" column
                            // then it should contain one of these entries { "MEM_COMMIT", "MEM_FREE", "MEM_RESERVE" }
                            if (j == 3)
                            {
                                if (strArr[i].Contains(MemoryInformation.PageTypes[0]) || strArr[i].Contains(MemoryInformation.PageTypes[1]) || strArr[i].Contains(MemoryInformation.PageTypes[2]))
                                {
                                    strArr2[j++] = strArr[i];
                                    continue;
                                }
                                else
                                {
                                    strArr2[j] = "";
                                }
                                j++;
                            }

                            // If this is the "State" column
                            // then it should contain one of these entries { "MEM_COMMIT", "MEM_FREE", "MEM_RESERVE" };
                            if (j == 4)
                            {
                                if (strArr[i].Contains(MemoryInformation.MemoryStates[0]) || strArr[i].Contains(MemoryInformation.MemoryStates[1]) || strArr[i].Contains(MemoryInformation.MemoryStates[2]))
                                {
                                    strArr2[j++] = strArr[i];
                                    continue;
                                }
                                else
                                    strArr2[j] = "";
                                j++;
                            }

                            // If this is the "Protection" column
                            // then it should contain one of these entries {"PAGE_EXECUTE", "PAGE_EXECUTE_READ", "PAGE_EXECUTE_READWRITE", "PAGE_EXECUTE_WRITECOPY", "PAGE_NOACCESS", "PAGE_READONLY", "PAGE_READWRITE", "PAGE_WRITECOPY", "PAGE_TARGETS_INVALID", "PAGE_TARGETS_NO_UPDATE", "PAGE_GUARD", "PAGE_NOCACHE", "PAGE_WRITECOMBINE" }
                            if (j == 5)
                            {
                                if (strArr[i].Contains(MemoryInformation.AllocationProtections[0]) || strArr[i].Contains(MemoryInformation.AllocationProtections[1]) || strArr[i].Contains(MemoryInformation.AllocationProtections[2]) || strArr[i].Contains(MemoryInformation.AllocationProtections[3]) ||
                                    strArr[i].Contains(MemoryInformation.AllocationProtections[4]) || strArr[i].Contains(MemoryInformation.AllocationProtections[5]) || strArr[i].Contains(MemoryInformation.AllocationProtections[6]) || strArr[i].Contains(MemoryInformation.AllocationProtections[7]) ||
                                    strArr[i].Contains(MemoryInformation.AllocationProtections[8]) || strArr[i].Contains(MemoryInformation.AllocationProtections[9]) || strArr[i].Contains(MemoryInformation.AllocationProtections[10]) || strArr[i].Contains(MemoryInformation.AllocationProtections[11]) ||
                                    strArr[i].Contains(MemoryInformation.AllocationProtections[12]))

                                {
                                    strArr2[j++] = strArr[i];
                                    continue;
                                }
                                else
                                    strArr2[j] = "";
                                j++;
                                continue;
                            }

                            if (j == 6)
                            {
                                if (strArr[i].StartsWith("["))
                                {
                                    strArr2[j++] = "";
                                }
                                else
                                {
                                    strArr2[j] = strArr[i];
                                    j++;
                                    continue;
                                }
                            }

                            if (j == 7)
                            {
                                if (strArr[i].StartsWith("["))
                                {
                                    moreInfoStartsAt = moreInfoEndsAt = 0;
                                    moreInfoStartsAt = strLine.IndexOf('[');
                                    moreInfoEndsAt = strLine.IndexOf(']');
                                    strArr2[j] = strLine.Substring(moreInfoStartsAt, moreInfoEndsAt - moreInfoStartsAt + 1);
                                    break;
                                }
                            }

                        } // end of IF

                    } // End of FOR

                    // 0`00000 addresses create problem
                    // When there is a + in a row it creates a problem
                    mre = new MemoryRegionEntry
                    {
                        BaseAddress = UInt64.Parse(strArr2[0] == "" ? "0" : strArr2[0], System.Globalization.NumberStyles.HexNumber),
                        EndAddress = UInt64.Parse(strArr2[1] == "" ? "0" : strArr2[1], System.Globalization.NumberStyles.HexNumber),
                        RegionSize = UInt64.Parse(strArr2[2] == "" ? "0" : strArr2[2], System.Globalization.NumberStyles.HexNumber),
                        RegionSizeDec = Convert.ToUInt64(UInt64.Parse(strArr2[2] == "" ? "0" : strArr2[2], System.Globalization.NumberStyles.HexNumber)),
                        Type = (strArr2[3] == "" ? "N/A" : strArr2[3]),
                        State = (strArr2[4] == "" ? "N/A" : strArr2[4]),
                        Protection = (strArr2[5] == "" ? "N/A" : strArr2[5]),
                        Usage = (strArr2[6] == "" ? "N/A" : strArr2[6]),
                        MoreInfo = (strArr2[7] == "" ? "N/A" : strArr2[7])
                    };
                    MemRegs.Add(mre);
                } // End of WHILE
            }
        }
        
        // r1-r2 : memory regions existing in r1 but not in r2
        public static List<MemoryRegionEntry> Diff( List<MemoryRegionEntry> r1, List<MemoryRegionEntry> r2 )
        {
            List<MemoryRegionEntry> result = new List<MemoryRegionEntry>();
            bool found = false;

            foreach( MemoryRegionEntry mer1 in r1 )
            {
                found = false;
                foreach (MemoryRegionEntry mer2 in r2)
                {
                    if (mer2.BaseAddress == mer1.BaseAddress)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    result.Add(mer1);
                }

            }

            return result;
        }

    }
}
