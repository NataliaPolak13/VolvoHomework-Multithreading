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
        public async Task<string[]> ReadingAsync(string folderPath)
        {
            string[] filePaths = Directory.GetFiles(folderPath);
            List<string> texts = new List<string>();

            if (filePaths.Length > 0)
            {
                foreach (var filePath in filePaths)
                {
                    string text = await File.ReadAllTextAsync(filePath);
                    texts.Add(text);
                }
            }
            return texts.ToArray();
        }

        public async Task ParallelResultsWriteAsync(string[] filesToWork)
        {
            string folderPath = Path.Combine(Environment.CurrentDirectory, "BooksOperations");
            Directory.CreateDirectory(folderPath); IList<Task> readTaskList = new List<Task>();

            for (int index = 1; index <= 100; ++index)
            {
                string title = GetTitle(filesToWork[index - 1]);
                string fileName = $"{title}.txt";
                string filePath = Path.Combine(folderPath, fileName);

                BooksStatistics sentenceStatistics = new BooksStatistics();
                string longestSentence = await sentenceStatistics.GetLongestSentenceAsync(filesToWork[index - 1]);
                string shortestSentence = await sentenceStatistics.GetShortestSentenceAsync(filesToWork[index - 1]);
                string longestWord = await sentenceStatistics.GetLongestWordAsync(filesToWork[index - 1]);
                string wordsSortedInDescendingOrder = await sentenceStatistics.WordsSortedByUsageDescending(filesToWork[index - 1]);
                string mostCommonLetters = await sentenceStatistics.GetMostCommonLettersAsync(filesToWork[index - 1]);


                await File.WriteAllTextAsync(filePath, $"Longest sentence: {longestSentence}" +
                    $"\n Shortest sentence: {shortestSentence}" +
                    $"\n Longest word: {longestWord}" +
                    $"\n Words sorted by the number of uses in descending order:\n{wordsSortedInDescendingOrder}" +
                    $"\n The most common letter is: {mostCommonLetters}");
            }

            await Task.WhenAll(readTaskList);
        }
        static string GetValidFileName(string title)
        {
            char[] invalidChars = Path.GetInvalidFileNameChars();
            return new string(title
                .Where(c => !invalidChars.Contains(c))
                .ToArray());
        }

        static string GetTitle(string text)
        {
            string pattern = @"Title:\s*(.*)";
            Match match = Regex.Match(text, pattern);
            string title = match.Success ? match.Groups[1].Value : "NoTitle";

            return GetValidFileName(title);
        }
    }
}
