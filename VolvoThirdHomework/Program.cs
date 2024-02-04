using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VolvoThirdHomework
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            AsyncOperations program = new AsyncOperations();
            string path = @"C:\Users\natal\OneDrive\Pulpit\VOLVO\VolvoThirdHomework\100 books";
            string[] filesToWork = await program.ReadingAsync(path);
            await program.ParallelResultsWriteAsync(filesToWork);
        }

    }
}
