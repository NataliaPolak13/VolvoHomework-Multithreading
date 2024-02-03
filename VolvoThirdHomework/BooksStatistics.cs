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
                string[] sentences = Regex.Split(text, @"(?<=[.!?])\s+");
                string shortestSentence = sentences
                    .Where(s => char.IsUpper(s.FirstOrDefault()) && s.Length > 1)
                    .OrderBy(s => new string(s.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)).ToArray()))
                    .FirstOrDefault();
                return shortestSentence != null ? shortestSentence.Trim() : "No sentences found.";
            });

        }
    }
}
