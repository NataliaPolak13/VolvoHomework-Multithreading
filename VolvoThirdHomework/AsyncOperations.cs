using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VolvoThirdHomework
{
    internal class AsyncOperations
    {
        private static AsyncOperations instance;
        private static readonly object lockObject = new object();

        private AsyncOperations() { }

        public static AsyncOperations Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new AsyncOperations();
                    }
                    return instance;
                }
            }
        }

        public async Task<string[]> ReadingAsync(string folderPath)
        {
            string[] filePaths = Directory.GetFiles(folderPath);
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

            for (int index = 1; index <= 100; ++index)
            {
                BookTitle bookTitle = new BookTitle();
                string title = bookTitle.GetTitle(filesToWork[index - 1]);
                string fileName = $"{title}.txt";
                string filePath = Path.Combine(folderPath, fileName);

                string longestSentence = await sentenceStatistics.GetLongestSentenceAsync(filesToWork[index - 1]);
                string shortestSentence = await sentenceStatistics.GetShortestSentenceAsync(filesToWork[index - 1]);
                string longestWord = await sentenceStatistics.GetLongestWordAsync(filesToWork[index - 1]);
                string wordsSortedInDescendingOrder = await sentenceStatistics.GetWordsByUsageDescendingAsync(filesToWork[index - 1]);
                string mostCommonLetters = await sentenceStatistics.GetMostCommonLettersAsync(filesToWork[index - 1]);

                writeTasks.Add(File.WriteAllTextAsync(filePath, $"Longest sentence: {longestSentence} \n" +
                    $"\n Shortest sentence: {shortestSentence} \n" +
                    $"\n Longest word: {longestWord}  \n" +
                    $"\n The most common letter is: {mostCommonLetters}  \n" +
                    $"\n Words sorted by the number of uses in descending order:\n{wordsSortedInDescendingOrder}  \n"));
            }

            await Task.WhenAll(writeTasks);
        }
    }
}
