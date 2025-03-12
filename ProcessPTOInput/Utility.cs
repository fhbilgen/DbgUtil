using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessPTOInput
{
    internal class Utility
    {       
       
        private static string? dmpchkPath { get; set; }       
        public static string? rootFolder { get; set;}
        public static string? cdbPath { get; set; }
        public static string? cdb32Path { get; set; }
        public static string? symPath { get; set; }
        public static string? gsPath { get; set; }
        public static string? LogParserPath { get; set; }
        //public static string? heavyDayIISLogPath { get; set; }
        //public static string? normalDayIISLogPath { get; set; }
        //public static string? mergedIISLogPath { get; set; }

        public static string[]? normalDayIISLogPaths { get; set; }
        public static string[]? heavyDayIISLogPaths { get; set; }
        public static string[]? aggIISLogs { get; set; }

        public static  string NormalDayIISLogSummaryFileName { get; } = "Normal.txt";
        public static string HeavyDayIISLogSummaryFileName { get; } = "Heavy.txt";
        public static string AggregateIISLogSummaryFileName { get; } = "Agg.txt";

        public static bool Initialize()
        {
            IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

            if (config == null)
                return false;
            else
            {
                dmpchkPath = config["Folders:dmpchkPath"];                
                rootFolder = config["Folders:rootFolder"];
                cdbPath = config["Folders:cdbPath"];
                cdb32Path = config["Folders:cdbPath"];
                symPath = config["Folders:symPath"];
                gsPath = config["Folders:gsPath"];
                LogParserPath = config["Folders:LogParserPath"];

                normalDayIISLogPaths = config.GetSection("IISLogFolders:NormalDays").Get<string[]>();
                heavyDayIISLogPaths = config.GetSection("IISLogFolders:HeavyDays").Get<string[]>();
                aggIISLogs = config.GetSection("IISLogFolders:AggIISLogs").Get<string[]>();
            }

            return true;
        }

        // Launch the dumpchk.exe tool to determine if the dump file is 32-bit or 64-bit
        public static bool Is64bit(string dumpPath, string symPath)
        {

            string output = string.Empty;
            string error = string.Empty;
            string arguments = $" -y \"{symPath} \"  \" {dumpPath}\"  ";
            ProcessStartInfo psi = new ProcessStartInfo(dmpchkPath, arguments)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process process = new Process()
            {
                StartInfo = psi
            };

            process.OutputDataReceived += (sender, e) => output += e.Data;
            process.ErrorDataReceived += (sender, e) => error += e.Data;

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
           
            if (output.Contains("Free x86 compatible"))
            {
                return false;
            }

            if (output.Contains("Free x64"))
            {
                return true;
            }

            return true;
        }

        public static void LaunchProgram(string programPath, string arguments)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = programPath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(startInfo))
                {
                    if (process != null)
                    {
                        // Read the output (or error)
                        string output = process.StandardOutput.ReadToEnd();
                        string error = process.StandardError.ReadToEnd();

                        // Wait for the process to exit
                        process.WaitForExit();

                        // Optionally, print the output and error
                        Console.WriteLine("Output:");
                        Console.WriteLine(output);
                        Console.WriteLine("Error:");
                        Console.WriteLine(error);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while launching the program: {ex.Message}");
            }
        }

        public static void NormalizeTheOutputFile(string fileName)
        {
            string[] lines = System.IO.File.ReadAllLines(fileName);
            var filteredLines = lines.Skip(3).Take(lines.Length - 5).ToArray();
            File.WriteAllLines(fileName, filteredLines);
        }

        public static void DeleteFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                try
                {
                    File.Delete(fileName);
                    Console.WriteLine($"File '{fileName}' has been deleted.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while deleting the file: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"File '{fileName}' does not exist.");
            }
        }
    }
}
