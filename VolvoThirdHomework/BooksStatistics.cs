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
                string[] sentences = text.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
                string shortestSentence = sentences
                    .Where(s => s.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length > 1)
                    .OrderBy(s => s.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length)
                    .FirstOrDefault();
                return shortestSentence != null ? shortestSentence.Trim() : "No sentences found.";
            });
        }
        public async Task<string> GetLongestWordAsync(string text)
        {
            return await Task.Run(() =>
            {
                string[] words = Regex.Split(text, @"\W+"); string longestWord = words
                    .Where(word => !string.IsNullOrWhiteSpace(word))
                    .OrderByDescending(word => word.Length)
                    .FirstOrDefault();
                return longestWord != null ? longestWord.Trim() : "No words found.";
            });
        }

    }
}

