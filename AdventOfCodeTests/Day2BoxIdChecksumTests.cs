using NUnit.Framework;
using AdventOfCode.Day2BoxIdChecksum;

namespace AdventOfCodeTests
{
    [TestFixture]
    public class Day2BoxIdChecksumTests
    {
        // this is the demo input provided by the problem description
        private static readonly string[] TestLines = new[]
        {
            "abcdef",
            "bababc",
            "abbcde",
            "abcccd",
            "aabcdd",
            "abcdee",
            "ababab"
        };

        private static readonly string[] TestSimilarIds = new[]
        {
            "abcde",
            "fghij",
            "klmno",
            "pqrst",
            "fguij",
            "axcye",
            "wvxyz"
        };

        private const string ExpectedSimilarChars = "fgij";
        private const string ExpectedSimilarLine1 = "fghij";
        private const string ExpectedSimilarLine2 = "fguij";

        // expected results
        private const int ExpectedTestChecksum = 12;
        private const int ExpectedTwoIdenticalCharacterLines = 4;
        private const int ExpectedThreeIdenticalCharacterLines = 3;

        [Test]
        public void GetChecksumChecksum_Calculates_Correct_Input_For_Test_Data()
        {
            var result = BoxIdChecksumLauncher.GetChecksum(TestLines, out int twoIdenticalCharLines, out int threeIdenticalCharLines);
            Assert.AreEqual(ExpectedTestChecksum, result);
            Assert.AreEqual(ExpectedTwoIdenticalCharacterLines, twoIdenticalCharLines);
            Assert.AreEqual(ExpectedThreeIdenticalCharacterLines, threeIdenticalCharLines);
        }

        [Test]
        public void GetCommonLettersBetweenIds_Returns_Correct_Letters_For_Test_Data()
        {
            var result = BoxIdChecksumLauncher.GetCommonLettersBetweenIds(TestSimilarIds, out string line1, out string line2);
            Assert.AreEqual(ExpectedSimilarChars, result);
            Assert.AreEqual(ExpectedSimilarLine1, line1);
            Assert.AreEqual(ExpectedSimilarLine2, line2);
        }
    }
}
