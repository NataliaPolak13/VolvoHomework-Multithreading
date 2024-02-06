using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VolvoThirdHomework
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            AsyncOperations asyncOperations = AsyncOperations.Instance;
            string path = @"C:\Users\natal\OneDrive\Pulpit\VOLVO\VolvoThirdHomework\100 books";
            string[] filesToWork = await asyncOperations.ReadingAsync(path);
            await asyncOperations.ParallelResultsWriteAsync(filesToWork);

            stopwatch.Stop();
            Console.WriteLine($"Czas trwania: {stopwatch.ElapsedMilliseconds} ms");
        }

    }
}
