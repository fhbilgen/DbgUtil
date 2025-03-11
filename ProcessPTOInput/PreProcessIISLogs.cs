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


    }
}
