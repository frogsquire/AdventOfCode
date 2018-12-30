using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Day1FrequencyCalibrator
{
    /* 
     * Input: args[0]: The path to a file containing an arbitrary series of operation/number pairs, one pair per line. 
     *  args[1]: A boolean indicating whether to provide verbose output (optional).
     * Output: The final value of the series of operations.
     * For example, if the input is:
     *  +1
     *  -2
     *  +3
     * The output will be:
     *  2
     * (Zero is always assumed to be the starting value.)
     */
    public class FrequencyCalibratorLauncher
    {
        public const int InitialFrequency = 0;

        public static void Main(string[] args)
        {
            if (args.Length < 1) Console.WriteLine("You must provide a file path.");

            var beVerbose = false; 
            if (args.Length > 1)
            {
                beVerbose = bool.Parse(args[1]);
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
                            if (beVerbose) Console.WriteLine("Queued adjustment " + raw);
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine("Could not create adjustment for " + lineNumber + ": " + e.Message);
                        }
                        lineNumber++;
                    }
                }
            }

            Console.WriteLine("Total of all adjustments (from zero): " + GetAdjustmentHistory(adjustments).Last());
        }

        public static List<int> GetAdjustmentHistory(List<FrequencyAdjustment> adjustments)
        {
            var frequencyHistory = new List<int> { InitialFrequency };
            adjustments.ForEach(a => frequencyHistory.Add(a.Adjust(frequencyHistory.Last())));
            return frequencyHistory; 
        }

    }
}
