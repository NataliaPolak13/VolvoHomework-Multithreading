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
        public static string[] SplitIntoSentences(string text)
        {
            return Regex.Split(text, @"(?<=[.!?]([""”])?)\s+")
        .Select(sentence => Regex.Replace(sentence, @"(\.{4,})$", "..."))
        .Where(sentence =>
                    !Regex.IsMatch(sentence, @"^\d") &&                  
                    Regex.IsMatch(sentence, @"^[A-Z]") &&              
                   // !Regex.IsMatch(sentence, @"^[A-Z]+\.$") &&        
                    //!Regex.IsMatch(sentence, @"^[A-Z]\!$") &&    
                    !Regex.IsMatch(sentence, @"^[A-Z]+[.!?]+$") &&       
                    !Regex.IsMatch(sentence, @"^.*[.!?][A-Z]+$")&&
                    !Regex.IsMatch(sentence, @".*\*\s") && //due to a problem with the table in the BEOWULF book
                     !Regex.IsMatch(sentence, @"\b[A-Z]+\.[A-Z]+\b"))
                .ToArray();
        }

        public async Task<string> GetShortestSentenceAsync(string text)
        {
            var nonSentenceWords = Enum.GetNames(typeof(NonSentenceWords));

            string[] sentences = SplitIntoSentences(text);

            var shortestSentencesByWords = await Task.Run(() =>
                sentences
                    .Where(sentence => !nonSentenceWords.Any(nonSentenceWord => sentence.TrimStart().StartsWith(nonSentenceWord, StringComparison.OrdinalIgnoreCase)))
                    .OrderBy(sentence => sentence.Split().Length)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase)
                    .Take(10)
                    .ToList()
            );

            return string.Join(Environment.NewLine, shortestSentencesByWords) ?? "No sentences found.";
        }

        public async Task<string> GetLongestSentenceAsync(string text)
        {
            var nonSentenceWords = Enum.GetNames(typeof(NonSentenceWords));

            string[] sentences = SplitIntoSentences(text);

            var longestSentenceByChars = await Task.Run(() =>
                sentences
                    .Where(sentence => !nonSentenceWords.Any(nonSentenceWord 
                    => sentence.Contains(nonSentenceWord, StringComparison.OrdinalIgnoreCase)))
                    .OrderByDescending(sentence => sentence.Length)
                    .Take(10)
                    .ToList());

            var formattedResult = longestSentenceByChars.Select(sentence => $"{sentence}{Environment.NewLine}{Environment.NewLine}");
            return string.Join(" ", formattedResult) ?? "No sentences found.";
        }

        public async Task<string> GetLongestWordAsync(string text)
        {

                string[] words = Regex.Split(text, @"\W+"); 
                var longestWord = await Task.Run(() =>
                words
                    .Where(word => !string.IsNullOrWhiteSpace(word))
                    .Select(word => word.Trim('_'))
                    .OrderByDescending(word => word.Length)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase)
                    .Take(10)
                    .ToList()

                );

            return string.Join(Environment.NewLine, longestWord) ?? "No words found.";
        }

        public async Task<string> GetWordsByUsageDescendingAsync(string text)
        {
            return await Task.Run(() =>
            {
                string[] words = Regex.Split(text, @"\W+")
                    .Where(word => !string.IsNullOrWhiteSpace(word))
                    .ToArray();

                var wordsByUsage = words
                    .GroupBy(word => word, StringComparer.OrdinalIgnoreCase)
                    .OrderByDescending(group => group.Count())
                    .Select(group => $"{group.Key}: {group.Count()}")
                    .Take(10)
                    .ToList();

                return string.Join(Environment.NewLine, wordsByUsage);
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
                    .Take(10);

                var mostCommonLetterString = mostCommonLetters != null
                ? string.Join(Environment.NewLine, mostCommonLetters.Select(group => $"{group.Key}: {group.Count()}"))
                    : "No letters found.";

                return mostCommonLetterString;
            });
        }

    }
}

