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

            Console.WriteLine("All adjustments read. Calculating frequency adjustment chain (starting from 0)...");

            var frequencies = GetAdjustmentChain(adjustments, haltOnDuplicateFrequency, beVerbose);
            var outputStr = haltOnDuplicateFrequency ? "First duplicate frequency observed: " : "Total of all adjustments (from zero to EOF): ";

            Console.WriteLine(outputStr + frequencies.Last());
        }

        public static List<int> GetAdjustmentChain(List<FrequencyAdjustment> adjustments, bool haltOnDuplicateHistory = false, bool beVerbose = false)
        {
            var overallFrequencyHistory = new List<int> { InitialFrequency };

            var numberOfPasses = 0;

            overallFrequencyHistory = IterateOverAllAdjustments(adjustments, overallFrequencyHistory, haltOnDuplicateHistory, beVerbose, out bool duplicateFound);

            if (haltOnDuplicateHistory && !duplicateFound)
            {
                // todo: specify maximum iterations
                while(!duplicateFound)
                {
                    if (beVerbose) Console.WriteLine("Beginning frequency pass " + ++numberOfPasses);
                    overallFrequencyHistory = IterateOverAllAdjustments(adjustments, overallFrequencyHistory, haltOnDuplicateHistory, beVerbose, out duplicateFound);
                }
            }

            Console.WriteLine("Frequency adjustment completed. Total passes started: " + numberOfPasses);

            return overallFrequencyHistory;            
        }

        private static List<int> IterateOverAllAdjustments(List<FrequencyAdjustment> adjustments, 
            List<int> frequencyHistory, bool haltOnDuplicateHistory, bool beVerbose, out bool duplicateFound)
        {
            duplicateFound = false;

            foreach (var a in adjustments)
            {
                var nextAdjustment = a.Adjust(frequencyHistory.Last());

                if (beVerbose) Console.WriteLine("Adjusting " + frequencyHistory.Last() + ". New frequency: " + nextAdjustment);
                
                var containsNextAdjustment = frequencyHistory.Contains(nextAdjustment);
                frequencyHistory.Add(nextAdjustment);
                if (haltOnDuplicateHistory && containsNextAdjustment)
                {
                    if (beVerbose) Console.WriteLine("Found duplicate in history list! Exiting.");
                    duplicateFound = true;
                    return frequencyHistory;
                }
            }
            return frequencyHistory;
        }

    }
}
