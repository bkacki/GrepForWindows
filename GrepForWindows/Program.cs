using System;
using System.Text.RegularExpressions;

namespace GrepForWindows
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Contains("--help"))
            {
                ShowHelp();
                return;
            }

            try
            {
                ProcessArguments(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine("Use '--help' to see usage information.");
            }
        }

        private static void ShowHelp()
        {
            Console.WriteLine("GrepForWindows - A simple text search utility.");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("  GrepForWindows <pattern> <file>");
            Console.WriteLine("  GrepForWindows <options> <pattern> <file>");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  -i      Case-insensitive search.");
            Console.WriteLine("  -n      Display line numbers where the pattern matches.");
            Console.WriteLine("  --help  Display this help message.");
            Console.WriteLine();
            Console.WriteLine("Example:");
            Console.WriteLine("  GrepForWindows -in \"error\" log.txt");
        }

        private static void ProcessArguments(string[] args)
        {
            if (args.Length == 2)
            {
                Grep.FindText(args[0], args[1], RegexOptions.None, NumberTheLines.No);
            }
            else if (args.Length == 3)
            {
                var options = ParseOptions(args[0]);
                Grep.FindText(args[1], args[2], options.RegexOptions, options.NumberLines);
            }
            else
            {
                throw new ArgumentException("Invalid number of arguments.");
            }
        }

        private static (RegexOptions RegexOptions, NumberTheLines NumberLines) ParseOptions(string optionsString)
        {
            if (!optionsString.StartsWith("-"))
                throw new ArgumentException("Options must start with '-'.");

            RegexOptions regexOptions = RegexOptions.None;
            NumberTheLines numberLines = NumberTheLines.No;

            if (optionsString.Contains('i'))
                regexOptions = RegexOptions.IgnoreCase;
            if (optionsString.Contains('n'))
                numberLines = NumberTheLines.Yes;

            return (regexOptions, numberLines);
        }
    }
}
