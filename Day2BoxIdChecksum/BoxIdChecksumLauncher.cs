using System;
using System.IO;
using System.Linq;

namespace AdventOfCode.Day2BoxIdChecksum
{
    public class BoxIdChecksumLauncher
    {
        /*
         * Input: A set of strings, one per line, containing alphabet characters (non-alpha characters will be ignored). 
         * Output part 1: A 'checksum' which is calculated as follows:
         *  1. Let A be the number of strings where any letter appears exactly 3 times.
         *  2. Let B be the number of strings where any letter appears exactly 2 times.
         *  3. Return A * B.
         * Output part 2: A string of characters common to two strings which represent sequential IDs. 
         *  For example, if the input is ['abc','def','ajc','bab'] the output will be 'ac' because 'abc' and 'ajc' are identical save for one character.
         *  (Only the first two such strings will be returned.)
         */
        public static void Main(string[] args)
        {
            // todo: consider abstracting shared arguments-parsing behavior
            // it's not all that sophisticated, but it's not necessary to repeat it, either
            if (args.Length < 1) Console.WriteLine("You must provide a file path.");

            var path = Path.GetFullPath(args[0]);
            var lines = File.ReadAllLines(path);

            Console.WriteLine("Processing " + path + " checksum...");

            var checksumResult = GetChecksum(lines, out int twoIdenticalCharLines, out int threeIdenticalCharLines);

            Console.WriteLine(twoIdenticalCharLines + " strings with 2 of the same character; " + threeIdenticalCharLines + " strings with 3 of the same character.");
            Console.WriteLine("Checksum: " + checksumResult);

            var commonLettersResult = GetCommonLettersBetweenIds(lines, out string line1, out string line2);

            if (string.IsNullOrEmpty(commonLettersResult))
                Console.WriteLine("No common IDs were found!");
            else
                Console.WriteLine("IDs " + line1 + " and " + line2 + " share characters " + commonLettersResult);
        }

        public static int GetChecksum(string[] lines, out int twoIdenticalCharLines, out int threeIdenticalCharLines)
        {
            twoIdenticalCharLines = 0;
            threeIdenticalCharLines = 0;
            foreach (var line in lines)
            {
                var alphaChars = GetAlphaChars(line);
                var charCounts = alphaChars.GroupBy(c => c).Select(c => new { Char = c.Key, Count = c.Count() });
                if (charCounts.Any(c => c.Count == 2)) twoIdenticalCharLines++;
                if (charCounts.Any(c => c.Count == 3)) threeIdenticalCharLines++;
            }
            return twoIdenticalCharLines * threeIdenticalCharLines;
        }

        public static string GetCommonLettersBetweenIds(string[] lines, out string line1, out string line2)
        {
            // sort sequentially by characters and remove duplicates
            var distinctLines = lines.Distinct();
            var linesRemaining = distinctLines.ToList();
            var length = distinctLines.ElementAt(0).Length; // assumption: all entries are same length

            line1 = string.Empty; line2 = string.Empty;
            foreach(var line in distinctLines)
            {
                linesRemaining.Remove(line);
                // get candidate matches - strings that differ by exactly one character
                var sharedChars = string.Empty;
                var similarLine = linesRemaining.FirstOrDefault(l => 
                {
                    sharedChars = string.Empty;
                    var lineChars = line.ToCharArray();
                    var candidateChars = l.ToCharArray();
                    var differenceSeen = false;
                    for(var i = 0; i < length; i++)
                    {
                        if (lineChars[i] == candidateChars[i]) sharedChars += lineChars[i];
                        else if (!differenceSeen) differenceSeen = true;
                        else return false;
                    }
                    return true;
                });

                if (similarLine != null)
                {
                    line1 = line;
                    line2 = similarLine;
                    return sharedChars;
                }
            }

            // none found
            return string.Empty;            
        }

        // strip non-alpha characters and rebuild
        // a regex might be simpler, but would be slower, and probably less readable, too
        private static char[] GetAlphaChars(string str)
        {
            return Array.FindAll(str.ToCharArray(), c => char.IsLetter(c));
        }
    }
}
