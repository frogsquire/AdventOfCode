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
    }
}
