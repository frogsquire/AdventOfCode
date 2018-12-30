using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Day1FrequencyCalibrator
{
    /* 
     * Input: args[0]: The path to a file containing an arbitrary series of operation/number pairs, one pair per line. 
     *  args[1]: A boolean indicating whether to halt as soon as a repeated frequency is seen.
     *  args[2]: A boolean indicating whether to provide verbose output (optional).
     * Output: The final value of the series of operations.
     * For example, if the input is:
     *  +1
     *  -2
     *  +3
     * and the repeated frequency argument is not set, the output will be 2 (0 + 1 - 2 + 3)
     * if the repeated frequency argument is set, the output will also be 2, because:
     * 0 + 1 = 1
     * 1 - 2 = -1
     * -1 + 3 = 2
     * (loop)
     * 0 + 2 = 2 - but 2 was already seen 
     * (Zero is always assumed to be the starting value.)
     */
    public class FrequencyCalibratorLauncher
    {
        public const int InitialFrequency = 0;

        public static void Main(string[] args)
        {
            if (args.Length < 1) Console.WriteLine("You must provide a file path.");

            bool beVerbose = false, haltOnDuplicateFrequency = false;
            
            if (args.Length > 1)
            {
                haltOnDuplicateFrequency = bool.Parse(args[1]);
            }

            if (args.Length > 2)
            {
                beVerbose = bool.Parse(args[2]);
            }

            // GetFullPath() will except if the argument is not a valid file path
            var path = Path.GetFullPath(args[0]);
            var adjustments = new List<FrequencyAdjustment>(); 
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (var sr = new StreamReader(fs))
                {
                    var lineNumber = 1;
                    while(!sr.EndOfStream)
                    {
                        try
                        {
                            var raw = sr.ReadLine();
                            adjustments.Add(new FrequencyAdjustment(raw));
                            if (beVerbose) Console.WriteLine("Read in adjustment " + raw);
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine("Could not create adjustment for " + lineNumber + ": " + e.Message);
                        }
                        lineNumber++;
                    }
                }
            }

            var frequencies = GetAdjustmentChain(adjustments, haltOnDuplicateFrequency);
            var outputStr = haltOnDuplicateFrequency ? "First duplicate frequency observed: " : "Total of all adjustments (from zero to EOF): ";

            Console.WriteLine(outputStr + GetAdjustmentChain(adjustments).Last());
        }

        public static List<int> GetAdjustmentChain(List<FrequencyAdjustment> adjustments, bool haltOnDuplicateHistory = false)
        {
            var overallFrequencyHistory = new List<int> { InitialFrequency };

            overallFrequencyHistory = IterateOverAllAdjustments(adjustments, overallFrequencyHistory, haltOnDuplicateHistory, out bool duplicateFound);

            if (haltOnDuplicateHistory && !duplicateFound)
            {
                while(!duplicateFound)
                {
                    overallFrequencyHistory = IterateOverAllAdjustments(adjustments, overallFrequencyHistory, haltOnDuplicateHistory, out duplicateFound);
                }
            }

            return overallFrequencyHistory;            
        }

        private static List<int> IterateOverAllAdjustments(List<FrequencyAdjustment> adjustments, List<int> frequencyHistory, bool haltOnDuplicateHistory, out bool duplicateFound)
        {
            duplicateFound = false;
            foreach (var a in adjustments)
            {
                var nextAdjustment = a.Adjust(frequencyHistory.Last());
                var containsNextAdjustment = frequencyHistory.Contains(nextAdjustment);
                frequencyHistory.Add(nextAdjustment);
                if (haltOnDuplicateHistory && containsNextAdjustment)
                {
                    duplicateFound = true;
                    return frequencyHistory;
                }
            }
            return frequencyHistory;
        }

    }
}
