using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace xk
{
    internal class scanMemPageContent
    {

        private string FileName { get; set; }
        private string OutFileName { get; set; }

        public scanMemPageContent()
        {
            FileName = String.Empty;
            OutFileName = String.Empty;
        }
        public scanMemPageContent(string fileName, string outFileName)
        {
            FileName = fileName;
            OutFileName = outFileName;
        }

        private ConcurrentQueue<string> _content = new ConcurrentQueue<string>();

        private async Task ProcessLinesAsync(string[] lines)
        {
            // Simulate processing the lines
            await Task.Run(() =>
            {
                foreach (var line in lines)
                {
                    var addr = line.Substring(0, 17);
                    var content = line.Substring(56, 16);
                    if (!content.Equals("................"))
                    {
                        _content.Enqueue($"{addr}: {content}");
                    }
                }
            });
        }

        static async Task WriteQueueToFileAsync(ConcurrentQueue<string> queue, string outputFileName)
        {
            using (StreamWriter writer = new StreamWriter(outputFileName))
            {
                while (!queue.IsEmpty)
                {
                    if (queue.TryDequeue(out string line))
                    {
                        await writer.WriteLineAsync(line);
                    }
                }
            }
        }


        private async Task ProcessFileInParallel(string fileName)
        {
            // var lines = await File.ReadAllLinesAsync(fileName);
            //var tasks = new List<Task>();
            string[] lines = new string[10];
            string line;


            using (var reader = new StreamReader(fileName))
            {

                while (!reader.EndOfStream)
                {
                    for (int i = 0; i < 10 && !reader.EndOfStream; i++)
                    {
                        line = await reader.ReadLineAsync();
                        if (line == null)
                        { 
                            lines[i] = String.Empty;
                            break;
                        }
                        else                        
                            lines[i] = line;                        
                    }
                    await ProcessLinesAsync(lines);
                }                
            }              
        }

        public async Task ProcessContentAsync(string[] args)
        {
            if ( String.IsNullOrEmpty(FileName) )
                throw new ArgumentNullException("FileName", "FileName is not set");
            
            await ProcessFileInParallel(FileName);
            await WriteQueueToFileAsync(_content, OutFileName);
        }
    }
}
