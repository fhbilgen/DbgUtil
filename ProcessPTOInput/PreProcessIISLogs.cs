using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessPTOInput
{
    class PreProcessIISLogs
    {
        private List<string> normalDayIISLogPaths { get; set; } = new List<string>();
        private List<string> heavyDayIISLogPaths { get; set; } = new List<string>();

        public PreProcessIISLogs(string[] normalDayPaths, string[] heavyDayPath)
        {
            for (int i = 0; i < normalDayIISLogPaths.Count; i++)
                normalDayIISLogPaths.Add(normalDayIISLogPaths[i]);

            for (int i = 0; i < heavyDayIISLogPaths.Count; i++)
                heavyDayIISLogPaths.Add(heavyDayIISLogPaths[i]);
        }

        public void ProcessIISLogFolder(string inputPath, string outputFileName)
        {            
            
            // Run LogPArser and create a summary Log File
            string[] lpArgs = new string[3];
            lpArgs[0] = "-i:W3C ";
            lpArgs[1] = "\"SELECT TO_STRING(TO_LOCALTIME(QUANTIZE(time, 3600)),'HH') AS Hour, cs-uri-stem AS URI, sc-status AS Status, COUNT(*) AS Count, AVG(time-taken) AS Duration INTO " + inputPath + "\\" + outputFileName + " FROM " + inputPath + "\\*.log " + "GROUP BY Hour, URI, Status ORDER BY Hour, URI, Count, Duration ASC\"";
            lpArgs[2] = " -o:CSV ";
            Console.WriteLine($"GS arguments {string.Join(" ", lpArgs)}");
            Utility.LaunchProgram(Utility.LogParserPath, string.Join(" ", lpArgs));
        }

    }
}
