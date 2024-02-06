﻿using System;
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
            return Regex.Split(text, @"(?<=[.!?])\s+")
        .Select(sentence => Regex.Replace(sentence, @"(\.{4,})$", "..."))
        .Where(sentence =>
                    !Regex.IsMatch(sentence, @"^\d") &&                  
                    Regex.IsMatch(sentence, @"^[A-Z]") &&              
                    !Regex.IsMatch(sentence, @"^[A-Z]+\.$") &&        
                    !Regex.IsMatch(sentence, @"^[A-Z]\!$") &&    
                    !Regex.IsMatch(sentence, @"^[A-Z]+[.!?]+$") &&       
                    !Regex.IsMatch(sentence, @"^.*[.!?][A-Z]+$")&&
                    !Regex.IsMatch(sentence, @".*\*\s") && //due to a problem with the table in the BEOWULF book
                    //gdy za kropką jest zamknięcie cudzysłowy literackiego
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

            return string.Join(Environment.NewLine, shortestSentencesByWords) ?? "No suitable sentences found.";
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
            return string.Join(" ", formattedResult) ?? "No suitable sentences found.";
        }

        public async Task<string> GetLongestWordAsync(string text)
        {

                string[] words = Regex.Split(text, @"\W+"); 
                string longestWord = await Task.Run(() =>
                words
                    .Where(word => !string.IsNullOrWhiteSpace(word))
                    .OrderByDescending(word => word.Length)
                    .FirstOrDefault()
                );

            return longestWord != null ? longestWord.Trim() : "No words found.";
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
                    .FirstOrDefault();

                var mostCommonLetterString = mostCommonLetters != null
                    ? $"{mostCommonLetters.Key}: {mostCommonLetters.Count()}"
                    : "No letters found.";

                return mostCommonLetterString;
            });
        }

    }
}

