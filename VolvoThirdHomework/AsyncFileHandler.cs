using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace VolvoThirdHomework
{
    internal class AsyncFileHandler
    {
        private static AsyncFileHandler instance;
        private static readonly object lockObject = new object();

        private AsyncFileHandler() { }

        public static AsyncFileHandler Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new AsyncFileHandler();
                    }
                    return instance;
                }
            }
        }
        public async Task<string[]> ReadingAsync(string folderPath)
        {
            string[] filePaths = Directory.GetFiles(folderPath, "*", SearchOption.TopDirectoryOnly)
                .Where(path => !new FileInfo(path).Attributes.HasFlag(FileAttributes.System | FileAttributes.Hidden))
                .ToArray();
            var readTasks = filePaths.Select(async filePath =>
            {
                try
                {
                    return await File.ReadAllTextAsync(filePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading file '{filePath}': {ex.Message}");
                    return string.Empty;
                }
            });

            string[] texts = await Task.WhenAll(readTasks);
            return texts;
        }
        public async Task ParallelResultsWriteAsync(string[] filesToWork)
        {
            string folderPath = Path.Combine(Environment.CurrentDirectory, "BooksOperations");
            Directory.CreateDirectory(folderPath);

            BooksStatistics sentenceStatistics = new BooksStatistics();

            List<Task> writeTasks = new List<Task>();

            writeTasks = filesToWork.Select(async file =>
            {
                BookTitle bookTitle = new BookTitle();
                string title = bookTitle.GetTitle(file);
                string fileName = $"{title}.txt";
                string filePath = Path.Combine(folderPath, fileName);

                string longestSentence = await sentenceStatistics.GetLongestSentenceAsync(file);
                string shortestSentence = await sentenceStatistics.GetShortestSentenceAsync(file);
                string longestWord = await sentenceStatistics.GetLongestWordAsync(file);
                string wordsSortedInDescendingOrder = await sentenceStatistics.GetWordsByUsageDescendingAsync(file);
                string mostCommonLetters = await sentenceStatistics.GetMostCommonLettersAsync(file);

                await File.WriteAllTextAsync(filePath, $"Longest sentence:\n{longestSentence} \n" +
                    $"\nShortest sentence:\n{shortestSentence} \n" +
                    $"\nLongest word:\n{longestWord}  \n" +
                    $"\nThe most common letter:\n{mostCommonLetters}  \n" +
                    $"\nWords sorted by the number of uses in descending order:\n{wordsSortedInDescendingOrder}  \n");

                Console.WriteLine($"File title '{title}' processed.");

            }).ToList();

            await Task.WhenAll(writeTasks);
            Console.WriteLine("Processing completed.");
            string combinedFilePath = Path.Combine(folderPath, "Top10.txt");
            using (StreamWriter writer = new StreamWriter(combinedFilePath))
            {
                foreach (var filePath in filesToWork.Select(file => Path.Combine(folderPath, $"{new BookTitle().GetTitle(file)}.txt")))
                {
                    string[] lines = await File.ReadAllLinesAsync(filePath);
                    foreach (string line in lines)
                    {
                        await writer.WriteLineAsync(line);
                    }
                }
            }

            Top10Analyzer.CombineResultsToFile(folderPath);

        }
    }
}