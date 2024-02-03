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
            Program program = new Program();
            string path = @"C:\Users\natal\OneDrive\Pulpit\VOLVO\VolvoThirdHomework\100 books";
            string[] filesToWork = await program.ReadingAsync(path);
            await program.ParallelResultsWriteAsync(filesToWork);
        }

        private async Task<string[]> ReadingAsync(string folderPath)
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


                readTaskList.Add(CountWordsAsync(filesToWork[index - 1], filePath));
                await File.WriteAllTextAsync(filePath, $"Longest sentence: {longestSentence}\n Shortest sentence: {shortestSentence}");
            }

            await Task.WhenAll(readTaskList);
        }

        private async Task CountWordsAsync(string text, string fileName)
        {
            try
            {
                string[] words = text.Split(new char[] { ' ', '\n', '\r', '\t', '.', ',', ';', ':', '-', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

                int wordCount = words.Length;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing file {fileName}: {ex.Message}");
            }
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
