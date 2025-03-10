using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessPTOInput
{
    internal class StackOperations
    {
        private List<string> Paths { get; set; } = new List<string>();
        private string RootPath { get; set; }
        private string[] cdbConfigFileContent = { "~*knL", ".logopen ", "~*knL", ".logclose", "q" };
        private readonly string cdbConfigFileSuffix = "_cdbConfig.cfg";
        private readonly string stacksFileSuffix = "_stacks.txt";
        private readonly string summaryStacksFileSuffix = "_S.txt";
        // TODO: The followings need to be adjustable via a contructor call chain
  


        public StackOperations(string rootFolder)
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
            string stacksFileName = interim + stacksFileSuffix;
            string SummaryStacksFileName = interim + summaryStacksFileSuffix;
            bool isDumpFile64Bit = Utility.Is64bit(dumpPath, Utility.symPath);
            string cdbFolder = isDumpFile64Bit ? Utility.cdbPath : Utility.cdb32Path;

            CreateConfigFile(cdbConfigFileName, stacksFileName);

            
            // Run cdb.exe and produce a log file
            string[] cdbArgs= new string[3];
            cdbArgs[0] = "-z " + dumpPath;
            cdbArgs[1] = "-y " + Utility.symPath;
            cdbArgs[2] = "-cf " + cdbConfigFileName;
            
            

            Utility.LaunchProgram(cdbFolder, string.Join(" ", cdbArgs));

            // Check the log file and remove epilog&prolog lines and any line with warning error messages
            Utility.NormalizeTheOutputFile(stacksFileName);

            // Call the gs.exe pass the log files as input and produce the output file
            string[] gsArgs = new string[3];
            gsArgs[0] = stacksFileName;
            gsArgs[1] = SummaryStacksFileName;
            gsArgs[2] = isDumpFile64Bit ? "" : " x86 ";
            Utility.LaunchProgram(Utility.gsPath, string.Join(" ", gsArgs));    

            // Delete the config file for cdb.exe 
            Utility.DeleteFile(cdbConfigFileName);            

            Console.WriteLine($"Processing of dump file {dumpPath} is complete.");
        }

        // Create the config file
        private void CreateConfigFile(string cdbConfigName, string stacksFileName)
        {
            //string[] cdbcfgflcon = new string[5];
            string[] cdbcfgflcon = new string[cdbConfigFileContent.Length];

            //for (int i = 0; i != 5; i++)
            for (int i = 0; i != cdbConfigFileContent.Length; i++)
                cdbcfgflcon[i] = cdbConfigFileContent[i];

            cdbcfgflcon[1] = cdbcfgflcon[1] + stacksFileName;

            using (StreamWriter writer = new StreamWriter(cdbConfigName, false)) // 'false' to overwrite the file if it exists
            {
                foreach (var line in cdbcfgflcon)
                {
                    writer.WriteLine(line);
                }
            }
        }

    }
}
