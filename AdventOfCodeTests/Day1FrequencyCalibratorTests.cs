using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Day1FrequencyCalibrator
{
    /*
     * This fixture has something of a misnomer; it also tests the FrequencyCalibratorLauncher.
     * TODO: add tests against main method
     */
    [TestFixture]
    public class Day1FrequencyCalibratorTests
    {
        [Test]
        [TestCase("*8"),
         TestCase("/100"),
         TestCase("1100"),
         TestCase("app"),
         TestCase("1+")]
        public void FrequencyAdjustment_Rejects_Invalid_Operation(string input)
        {
            try
            {
                var frequency = new FrequencyAdjustment(input);
                Assert.Fail("Invalid input accepted");
            }
            catch(ArgumentException e)
            {
                Assert.AreEqual(FrequencyAdjustment.InvalidOperationMessage, e.Message);
            }
        }

        [Test]
        [TestCase("+A"),
         TestCase("-aBOY"),
         TestCase("+1@200"),
         TestCase("--"),
         TestCase("+!1"),
         TestCase("+1.001")]
        public void FrequencyAdjustment_Rejects_Invalid_Value(string input)
        {
            try
            {
                var frequency = new FrequencyAdjustment(input);
                Assert.Fail("Invalid input accepted");
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual(FrequencyAdjustment.InvalidValueMessage, e.Message);
            }
        }

        [Test]
        [TestCase("+"),
         TestCase("1"),
         TestCase("a"),
         TestCase("")]
        public void FrequencyAdjustment_Rejects_Too_Short_Input(string input)
        {
            try
            {
                var frequency = new FrequencyAdjustment(input);
                Assert.Fail("Invalid input accepted");
            }
            catch (ArgumentOutOfRangeException e)
            {
                Assert.AreEqual("operationValuePair", e.ParamName);
            }
        }

        [Test]
        [TestCase("+1", 0, 1),
            TestCase("-2", 0, -2),
            TestCase("+0", 0, 0),
            TestCase("-0", 0, 0),
            TestCase("+1000", 0, 1000),
            TestCase("-999", 0, -999),
            TestCase("+1", 10, 11),
            TestCase("-2", 10, 8),
            TestCase("+0", 10, 10),
            TestCase("-0", 10, 10),
            TestCase("+1000", 10, 1010),
            TestCase("-999", 10, -989)]

        public void FrequencyAdjustment_Adjusts_Correctly(string frequencyInput, int adjustInput, int expectedResult)
        {
            var frequency = new FrequencyAdjustment(frequencyInput);
            Assert.AreEqual(expectedResult, frequency.Adjust(adjustInput));
        }

        [Test]
        public void GetTotalAdjustment_Aggregates_Adjustments()
        {
            var adjustments = new List<FrequencyAdjustment>()
            {
                new FrequencyAdjustment("+1"),
                new FrequencyAdjustment("-2"),
                new FrequencyAdjustment("+0"),
                new FrequencyAdjustment("-0"),
                new FrequencyAdjustment("+1000"),
                new FrequencyAdjustment("-999"),
            };

            Assert.AreEqual(0, FrequencyCalibratorLauncher.GetAdjustmentChain(adjustments).Last());
        }
    }
}
