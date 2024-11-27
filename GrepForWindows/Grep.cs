using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GrepForWindows
{
    public static class Grep
    {
        public static void FindText(string pattern, string fileName, RegexOptions options, NumberTheLines numberTheLines)
        {
            ValidateInputs(pattern, fileName);

            try
            {
                if (!File.Exists(fileName))
                {
                    Console.WriteLine($"File \"{fileName}\" doesn't exist.");
                    return;
                }

                var matchingLines = GetMatchingLines(fileName, pattern, options);

                foreach (var entry in matchingLines)
                {
                    PrintLine(entry, pattern, options, numberTheLines);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        private static void ValidateInputs(string pattern, string fileName)
        {
            if (string.IsNullOrWhiteSpace(pattern))
                throw new ArgumentException("Pattern can't be empty", nameof(pattern));

            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("File name can't be empty", nameof(fileName));
        }

        private static IEnumerable<LineEntry> GetMatchingLines(string fileName, string pattern, RegexOptions options)
        {
            return File.ReadLines(fileName)
                       .Select((line, index) => new LineEntry(line, index + 1))
                       .Where(entry => Regex.IsMatch(entry.Line, pattern, options));
        }

        private static void PrintLine(LineEntry entry, string pattern, RegexOptions options, NumberTheLines numberTheLines)
        {
            if (numberTheLines == NumberTheLines.Yes)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{entry.LineNumber}: ");
                Console.ResetColor();
            }

            HighlightMatches(entry.Line, pattern, options);
        }

        private static void HighlightMatches(string line, string pattern, RegexOptions options)
        {
            var matches = Regex.Matches(line, pattern, options);
            int lastIndex = 0;

            foreach (Match match in matches)
            {
                // Text before the match
                Console.Write(line.Substring(lastIndex, match.Index - lastIndex));

                // Highlight the match in red
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(match.Value);
                Console.ResetColor();

                lastIndex = match.Index + match.Length;
            }

            // Print the rest of the line after the last match
            Console.WriteLine(line.Substring(lastIndex));
        }
    }
}
