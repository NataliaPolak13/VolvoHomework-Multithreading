using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VolvoThirdHomework
{
    class Top10Analyzer
    {
        public static List<string> LongestSentences(string input)
        {
            List<string> sections = new List<string>();
            string pattern = @"Longest sentence:(.*?)Shortest sentence:";
            Regex regex = new Regex(pattern, RegexOptions.Singleline);
            MatchCollection matches = regex.Matches(input);

            foreach (Match match in matches)
            {
                sections.Add(match.Groups[1].Value.Trim());
            }

            return sections;
        }
        public static List<string> ShortestSentences(string input)
        {
            var sentences = new HashSet<string>();

            string pattern = @"Shortest sentence:(.*?)Longest word:";
            var regex = new Regex(pattern, RegexOptions.Singleline);

            foreach (Match match in regex.Matches(input))
            {
                foreach (var sentence in match.Groups[1].Value.Trim().Split('\n'))
                {
                    sentences.Add(sentence.Trim());
                }
            }

            return new List<string> { "Top 10 shortest sentences:" }
                .Concat(sentences.OrderBy(s => s.Length).Take(10))
                .ToList();
        }
        public static List<string> LongestWord(string input)
        {
            List<string> sections = new List<string>();
            string pattern = @"Longest word:(.*?)The most common letter:";
            Regex regex = new Regex(pattern, RegexOptions.Singleline);
            MatchCollection matches = regex.Matches(input);

            foreach (Match match in matches)
            {
                sections.Add(match.Groups[1].Value.Trim());
            }

            List<string> longestWords = new List<string>();
            foreach (string section in sections)
            {
                string[] words = section.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                longestWords.AddRange(words);
            }

            longestWords = longestWords
                .Distinct()
                .OrderByDescending(word => word.Length)
                .Take(10)
                .ToList();

            // Dodajemy nagłówek
            longestWords.Insert(0, "Top 10 longest words:");

            return longestWords;
        }
        public static List<string> MostCommonLetter(string input)
        {
            var letterCounts = new Dictionary<char, int>();
            string pattern = @"The most common letter:(.*?)Words sorted by the number of uses in descending order:";
            var regex = new Regex(pattern, RegexOptions.Singleline);

            foreach (Match match in regex.Matches(input))
            {
                foreach (var line in match.Groups[1].Value.Trim().Split('\n'))
                {
                    var parts = line.Split(':');

                    if (parts.Length == 2 && char.TryParse(parts[0].Trim(), out var letter) && int.TryParse(parts[1].Trim(), out var count))
                    {
                        letterCounts[letter] = letterCounts.TryGetValue(letter, out var existingCount) ? existingCount + count : count;
                    }
                }
            }

            var top10Letters = letterCounts.OrderByDescending(kvp => kvp.Value).Take(10);

            var sections = top10Letters.Select(kvp => $"{kvp.Key}: {kvp.Value}").ToList();
            sections.Insert(0, "Top 10 letters:");

            return sections;
        }


        public static List<string> WordsByUsageDescending(string input)
        {
            List<string> sections = new List<string>();
            string pattern = @"Words sorted by the number of uses in descending order:(.*?)(?:Longest sentence:|$)";
            Regex regex = new Regex(pattern, RegexOptions.Singleline);
            MatchCollection matches = regex.Matches(input);

            foreach (Match match in matches)
            {
                sections.Add(match.Groups[1].Value.Trim());
            }

            return sections;
        }

        public static void CombineResultsToFile(string folderPath)
        {
            string top10FilePath = Path.Combine(folderPath, "Top10.txt");
            string inputText = File.ReadAllText(top10FilePath);

            List<string> allSections = new List<string>();
            allSections.AddRange(LongestSentences(inputText));
            allSections.AddRange(ShortestSentences(inputText));
            allSections.AddRange(LongestWord(inputText));
            allSections.AddRange(MostCommonLetter(inputText));
            allSections.AddRange(WordsByUsageDescending(inputText));

            string analyzedText = string.Join("\n", allSections);
            File.WriteAllText(top10FilePath, analyzedText);
        }
    }
}