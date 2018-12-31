using System;
using System.IO;
using System.Linq;

namespace AdventOfCode.Day2BoxIdChecksum
{
    public class BoxIdChecksumLauncher
    {
        /*
         * Input: A set of strings, one per line, containing alphabet characters (non-alpha characters will be ignored). 
         * Output: A 'checksum' which is calculated as follows:
         *  1. Let A be the number of strings where any letter appears exactly 3 times.
         *  2. Let B be the number of strings where any letter appears exactly 2 times.
         *  3. Return A * B.
         */
        public static void Main(string[] args)
        {
            // todo: consider abstracting shared arguments-parsing behavior
            // it's not all that sophisticated, but it's not necessary to repeat it, either
            if (args.Length < 1) Console.WriteLine("You must provide a file path.");

            var path = Path.GetFullPath(args[0]);
            var lines = File.ReadAllLines(path);

            Console.WriteLine("Processing " + path + " checksum...");

            var result = GetChecksum(lines, out int twoIdenticalCharLines, out int threeIdenticalCharLines);

            Console.WriteLine(twoIdenticalCharLines + " strings with 2 of the same character; " + threeIdenticalCharLines + " strings with 3 of the same character.");
            Console.WriteLine("Checksum: " + twoIdenticalCharLines * threeIdenticalCharLines);
        }

        public static int GetChecksum(string[] lines, out int twoIdenticalCharLines, out int threeIdenticalCharLines)
        {
            twoIdenticalCharLines = 0;
            threeIdenticalCharLines = 0;
            foreach (var line in lines)
            {
                // strip non-alpha characters and rebuild
                // a regex might be simpler, but would be slower, and probably less readable, too
                var alphaChars = Array.FindAll(line.ToCharArray(), c => char.IsLetter(c));
                var charCounts = alphaChars.GroupBy(c => c).Select(c => new { Char = c.Key, Count = c.Count() });
                if (charCounts.Any(c => c.Count == 2)) twoIdenticalCharLines++;
                if (charCounts.Any(c => c.Count == 3)) threeIdenticalCharLines++;
            }
            return twoIdenticalCharLines * threeIdenticalCharLines;
        }
    }
}
