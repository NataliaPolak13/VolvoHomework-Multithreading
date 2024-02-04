using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VolvoThirdHomework
{
    internal class BooksStatistics
    {
        public async Task<string> GetLongestSentenceAsync(string text)
        {
            return await Task.Run(() =>
            {
                string[] sentences = text.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
                string longestSentence = sentences.OrderByDescending(s => s.Length).FirstOrDefault();
                return longestSentence != null ? longestSentence.Trim() : "No sentences found.";
            });
        }
        public async Task<string> GetShortestSentenceAsync(string text)
        {
            return await Task.Run(() =>
            {
                var sentences = Regex.Split(text, @"(?<=[.!?])\s+");

                var shortestSentence = sentences
                    .Where(s => s.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length >= 1
                                && !Enum.GetNames(typeof(NonSentenceWords)).Any(word => s.StartsWith(word, StringComparison.OrdinalIgnoreCase))
                                && !char.IsUpper(s.Last())
                                && !(s.Length == 2 && char.IsPunctuation(s.Last()))
                                && !char.IsDigit(s.First())
                                && !s.StartsWith("'")
                                && !s.StartsWith("-")
                                && char.IsUpper(s.First()))
                    .OrderBy(s => s.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length)
                    .FirstOrDefault()?.TrimEnd(' ');

                return shortestSentence ?? "No suitable sentences found.";
            });
        }

        public async Task<string> GetLongestWordAsync(string text)
        {
            return await Task.Run(() =>
            {
                string[] words = Regex.Split(text, @"\W+"); 
                string longestWord = words
                    .Where(word => !string.IsNullOrWhiteSpace(word))
                    .OrderByDescending(word => word.Length)
                    .FirstOrDefault();
                return longestWord != null ? longestWord.Trim() : "No words found.";
            });
        }

        public async Task<string> WordsSortedByUsageDescending(string text)
        {
            return await Task.Run(() =>
            {
                string cleanedText = Regex.Replace(text, @"[^\w\s]", "").ToLower();

                var wordCounts = new Dictionary<string, int>();
                var words = Regex.Matches(cleanedText, @"\b\w+\b")
                                .Cast<Match>()
                                .Select(m => m.Value);

                foreach (var word in words)
                {
                    wordCounts[word] = wordCounts.ContainsKey(word) ? wordCounts[word] + 1 : 1;
                }

                var sortedWords = wordCounts.OrderByDescending(pair => pair.Value)
                                           .Select(pair => $"{pair.Key}: {pair.Value}");

                return string.Join(Environment.NewLine, sortedWords);
            });
        }
        public async Task<string> GetMostCommonLettersAsync(string text)
        {
            return await Task.Run(() =>
            {
                var mostCommonLetters = text?
                    .Where(char.IsLetter)
                    .GroupBy(char.ToLower)
                    .OrderByDescending(group => group.Count())
                    .FirstOrDefault();

                var mostCommonLetterString = mostCommonLetters != null
                    ? $"{mostCommonLetters.Key}: {mostCommonLetters.Count()}"
                    : "No letters found.";

                return mostCommonLetterString;
            });
        }

    }
}

