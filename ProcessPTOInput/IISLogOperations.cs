using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessPTOInput
{
    internal class IISLogOperations
    {

        class LogEntry
        {
            public string? DayType { get; set; }
            public string? URI { get; set; }
            public string? Status { get; set; }
            public int[]? Counts { get; set; } = new int[24];
            public double[]? Durations { get; set; } = new double[24];
        }

        private string NormalDayLog { get; set; }
        private string HeavyDayLog { get; set; }

        private string MergedLog { get; set; }
        private string HourColName { get; } = "HR";
        private string ExecDurationColName { get; } = "Exec";

        public IISLogOperations(string normalDayLog, string heavyDayLog, string mergedLog)
        {
            NormalDayLog = normalDayLog;
            HeavyDayLog = heavyDayLog;
            MergedLog = mergedLog;
        }

        private List<LogEntry> ReadLogFile(string filePath, string dayType)
        {
            var logEntries = new Dictionary<string, LogEntry>();

            // Don't read the headers
            foreach (var line in File.ReadLines(filePath).Skip(1))
            {
                var parts = line.Split(',');

                int hour = int.Parse(parts[0]);
                string uri = parts[1];
                string status = parts[2];
                int count = int.Parse(parts[3]);
                double duration = Double.Parse(parts[4]);
                //int duration = int.Parse(parts[4]);

                string key = $"{uri},{status},{dayType}";

                if (!logEntries.ContainsKey(key))
                {
                    logEntries[key] = new LogEntry
                    {
                        DayType = dayType,
                        URI = uri,
                        Status = status
                    };
                }                                
                logEntries[key].Counts[hour] = count;
                logEntries[key].Durations[hour] = duration;                
            }
            return logEntries.Values.ToList();
        }


        private List<LogEntry> AggregateLogEntries(List<LogEntry> logEntries, string dayType)
        {
            // Group by status and calculate totals
            var groupedByStatus = logEntries.GroupBy(entry => entry.Status);
            var totalEntries = new List<LogEntry>();

            foreach (var group in groupedByStatus)
            {
                var totalEntry = new LogEntry
                {
                    URI = "total",
                    Status = group.Key,
                    DayType = dayType
                };

                for (int i = 0; i < 24; i++)
                {
                    totalEntry.Counts[i] = group.Sum(entry => entry.Counts[i]);
                    totalEntry.Durations[i] = group.Average(entry => entry.Durations[i]);
                }

                totalEntries.Add(totalEntry);
            }

            return totalEntries;
        }


        private List<LogEntry> SortLogEntries(List<LogEntry> logEntries)
        {
            return logEntries
                .OrderBy(entry => entry.URI)
                .ThenBy(entry => entry.Status)
                .ThenBy(entry => entry.DayType)
                .ToList();
        }


        private void WriteMergedLogFile(string outputFilePath, List<LogEntry> logEntries)        
        {            
            using (var writer = new StreamWriter(outputFilePath))
            {
                // Write header
                var header = new List<string> { "DayType", "URI", "Status" };
                //header.AddRange(Enumerable.Range(0, 24).Select(i => $"Count{i}"));
                //header.AddRange(Enumerable.Range(0, 24).Select(i => $"Duration{i}"));
                header.AddRange(Enumerable.Range(0, 24).Select(i => $"{HourColName}{i}"));
                header.AddRange(Enumerable.Range(0, 24).Select(i => $"{ExecDurationColName}{i}"));
                writer.WriteLine(string.Join(",", header));

                
                // Write log entries
                foreach (var entry in logEntries)
                {
                    var line = new List<string> { entry.DayType, entry.URI, entry.Status };
                    line.AddRange(entry.Counts.Select(c => c.ToString()));
                    line.AddRange(entry.Durations.Select(d => d.ToString()));
                    writer.WriteLine(string.Join(",", line));
                }
            }
        }

        public void ProcessIISLogOutput()
        {
            //string normalLoadFilePath = "normal_load.txt";
            //string heavyLoadFilePath = "heavy_load.txt";
            //string outputFilePath = MergedLog + @"\merged_output.txt";
            string outputFilePath = MergedLog; // + Utility.AggregateIISLogSummaryFileName;

            var normalLoadEntries = ReadLogFile(NormalDayLog, "normal");
            var heavyLoadEntries = ReadLogFile(HeavyDayLog, "heavy");
            var aggregatedNormalLoadEntries = AggregateLogEntries(normalLoadEntries, "normal");
            var aggregatedHeavyLoadEntries = AggregateLogEntries(heavyLoadEntries, "heavy");
            normalLoadEntries.AddRange(aggregatedNormalLoadEntries);
            heavyLoadEntries.AddRange(aggregatedHeavyLoadEntries);
            var mergedEntries = normalLoadEntries.Concat(heavyLoadEntries).ToList();

            var sortedEntries = SortLogEntries(mergedEntries);


            WriteMergedLogFile(outputFilePath, sortedEntries);

            Console.WriteLine($"Merged log file has been written to '{outputFilePath}'.");
        }



    }
}
