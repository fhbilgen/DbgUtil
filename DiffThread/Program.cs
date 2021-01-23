using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffThread
{
    class Program
    {

        public static Dictionary<int, bool> InitializeThreadDict(string strPath)
        {
            Dictionary<int, bool> ThreadDict = new Dictionary<int, bool>();
            string strLine = "";            
            int ThreadID;

            using (StreamReader sr = new StreamReader(strPath))
            {
                strLine = sr.ReadLine();

                while (!sr.EndOfStream)
                {
                    strLine = sr.ReadLine();
                    ThreadID = int.Parse(strLine, System.Globalization.NumberStyles.HexNumber);                    
                    ThreadDict.Add(ThreadID, true);
                    strLine = "";
                }
            }

            return ThreadDict;
        }

        static void Main(string[] args)
        {

            Dictionary<int, bool> Threads1, Threads2;
            Dictionary<int, bool> ThreadsIn1NotIn2 = new Dictionary<int, bool>(), ThreadsIn2NotIn1 = new Dictionary<int, bool>();

            Threads1 = InitializeThreadDict(args[0]);
            Threads2 = InitializeThreadDict(args[1]);

            foreach(KeyValuePair<int, bool> kvp in Threads1)
            {
                if (!(Threads2.ContainsKey(kvp.Key)))
                {
                    ThreadsIn1NotIn2.Add(kvp.Key, kvp.Value);
                }
            }

            foreach(KeyValuePair<int, bool> kvp in Threads2)
            {
                if (!(Threads1.ContainsKey(kvp.Key)) )
                {
                    ThreadsIn2NotIn1.Add(kvp.Key, kvp.Value);
                }
            }

            Console.WriteLine("Threads in First Dump but not in the second dump");
            foreach(KeyValuePair<int, bool> kvp in ThreadsIn1NotIn2)
            {
                Console.WriteLine("{0:X}", kvp.Key);
            }

            Console.WriteLine("Threads in Second Dump but not in the first dump");
            foreach (KeyValuePair<int, bool> kvp in ThreadsIn2NotIn1)
            {
                Console.WriteLine("{0:X}", kvp.Key);
            }


        }
    }
}
