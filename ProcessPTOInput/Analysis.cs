using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessPTOInput
{
    internal class Analysis
    {
        
        private List<string> cdbCommands = new List<string>();

        private List<string> Paths { get; set; } = new List<string>();
        private string RootPath { get; set; }        
        private readonly string cdbConfigFileSuffix = "_Analysisconfig.cfg";
        private readonly string analysisFileSuffix = "_Analysis.txt";
        

        public Analysis(string rootFolder)
        {
            RootPath = rootFolder;            
        }


        private void Initialize()
        {
            DataFolders dfs = new DataFolders();

            Paths = dfs.GetFoldersWithDumpFiles(RootPath);

            foreach (var path in Paths)
            {
                Console.WriteLine($"Folder with dump files: {path}");
            }
        }

        public async Task ProcessFolder(string folder)
        {
            var tasks = new List<Task>();
            var dumpFiles = Directory.GetFiles(folder, "*.dmp");

            Parallel.ForEach(dumpFiles, dmpFile =>
            {
                var task = Task.Run(() => ProcessDumpFile(dmpFile));
                tasks.Add(task);
            });

            await Task.WhenAll(tasks);
        }

        public async Task ProcessAllFolders()
        {
            Initialize();

            foreach (var dumpFolder in Paths)
            {
                Console.WriteLine($"Processing folder: {dumpFolder}");
                await ProcessFolder(dumpFolder);
            }
        }

        public void ProcessDumpFile(string dumpPath)
        {
            // Create the config file for cdb.exe            
            var interim = dumpPath.Substring(0, dumpPath.Length - 4);
            string cdbConfigFileName = interim + cdbConfigFileSuffix;
            string outputFileName = interim + analysisFileSuffix;
            bool isDumpFile64Bit;
            string cdbFolder;

            if (Utility.symPath != null && Utility.cdbPath != null && Utility.cdb32Path != null)
            {
                isDumpFile64Bit = Utility.Is64bit(dumpPath, Utility.symPath);
                cdbFolder = isDumpFile64Bit ? Utility.cdbPath : Utility.cdb32Path;
            }
            else
            {
                Console.WriteLine($"There is a null value in Utility.symPath, Utility.cdbPath, Utility.cdb32Path ");
                return;
            }

                CreateConfigFile(cdbConfigFileName, outputFileName);

            // Run cdb.exe and produce a log file
            string[] cdbArgs = new string[3];
            cdbArgs[0] = "-z " + dumpPath;
            cdbArgs[1] = "-y " + Utility.symPath;
            cdbArgs[2] = "-cf " + cdbConfigFileName;


            Utility.LaunchProgram(cdbFolder, string.Join(" ", cdbArgs));

            // Check the log file and remove epilog&prolog lines and any line with warning error messages
            Utility.NormalizeTheOutputFile(outputFileName);            

            // Delete the config file for cdb.exe 
            Utility.DeleteFile(cdbConfigFileName);

            Console.WriteLine($"Processing of dump file {dumpPath} is complete.");
        }


        public void IISDotnetFrameworkAnalysis()
        {
            
            Console.WriteLine("IISDotnetFrameworkAnalysis");

            if (Utility.rootFolder == null)
                return;
            //StackOperations so = new StackOperations(Utility.rootFolder);
            //so.ProcessAllFolders().Wait();
                        
            
            DotnetFrameworkOperations dfo = new DotnetFrameworkOperations();
            var dfoCommands = dfo.GetCommands();
            foreach (var command in dfoCommands) 
                cdbCommands.Add(command);
            

            CommonNativeOperations cno = new CommonNativeOperations();
            var cnoCommands = cno.GetCommands();
            foreach (var command in cnoCommands)
                cdbCommands.Add(command);

            cdbCommands.Add($".logclose");
            cdbCommands.Add($"q");

            ProcessAllFolders().Wait();
        }


        // Create the config file
        private void CreateConfigFile(string cdbConfigName, string analysisFileName)
        {
            using (StreamWriter writer = new StreamWriter(cdbConfigName, false)) // 'false' to overwrite the file if it exists
            {
                writer.WriteLine($".logopen {analysisFileName}");
                foreach (var line in cdbCommands)                
                    writer.WriteLine(line);
                
            }
        }
    }
}
