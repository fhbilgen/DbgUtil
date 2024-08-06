using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessPTOInput
{
    internal class DataFolders
    {
        public List<string> GetFoldersWithDumpFiles(string rootFolder)
        {
            List<string> subfolders = new List<string>();
            List<string> foldersWithDumpFiles = new List<string>();

            void TraverseFolders(string folder)
            {
                try
                {
                    if (Directory.GetFiles(folder, "*.dmp").Length > 0)
                        foldersWithDumpFiles.Add(folder);

                    foreach (var subfolder in Directory.GetDirectories(folder))
                    {
                        if (Directory.GetFiles(subfolder, "*.dmp").Length > 0)
                            foldersWithDumpFiles.Add(subfolder);

                        subfolders.Add(subfolder);
                        TraverseFolders(subfolder); // Recursively traverse subfolders
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    // Handle the case where access to a folder is denied
                    Console.WriteLine($"Access denied to folder: {folder}");
                }
            }

            TraverseFolders(rootFolder);
            return foldersWithDumpFiles;
        }

    }
}
