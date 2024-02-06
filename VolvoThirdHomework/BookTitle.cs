using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VolvoThirdHomework
{
    internal class BookTitle
    {
        private string GetValidFileName(string title)
        {
            char[] invalidChars = Path.GetInvalidFileNameChars();
            return new string(title
                .Where(c => !invalidChars.Contains(c))
                .ToArray());
        }

        public string GetTitle(string text)
        {
            string pattern = @"Title:\s*(.*)";
            Match match = Regex.Match(text, pattern);
            string title = match.Success ? match.Groups[1].Value : "NoTitle";
            return GetValidFileName(title);
        }
    }
}

