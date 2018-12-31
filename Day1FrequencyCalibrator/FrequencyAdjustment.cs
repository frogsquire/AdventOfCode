using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Day1FrequencyCalibrator
{
    /* 
     * A frequency adjustment contains an operation (plus or minus, represented as a boolean)
     * and a value (represented as an integer). 
     */
    public class FrequencyAdjustment
    {
        public const string InvalidOperationMessage = "Operation must be plus (+) or minus (-).";
        public const string InvalidValueMessage = "Value is not an integer.";

        // Create a FrequencyAdjustment from a string consisting of an operation/value pair, 
        // which must be in the form [operation][value], e. g. +1, -2, etc.
        public FrequencyAdjustment(string operationValuePair)
        {
            if (operationValuePair.Length < 2) throw new ArgumentOutOfRangeException("operationValuePair");

            // substrings are used instead of character arrays to avoid having to convert back to string for int.TryParse()
            // using Char.GetNumericValue() was also considered, but that would also require a cast as it returns floating-point values
            var operationString = operationValuePair.Substring(0, 1);
            switch(operationString)
            {
                case "+":
                    _operation = true;
                    break;
                case "-":
                    _operation = false;
                    break;
                default:
                    throw new ArgumentException(InvalidOperationMessage);
            }      

            if (!int.TryParse(operationValuePair.Substring(1), out _value))
            {
                throw new ArgumentException(InvalidValueMessage);
            }
        }

        private readonly bool _operation;

        private int _value;

        public int Adjust(int inputValue)
        {
            return _operation ? inputValue + _value : inputValue - _value; 
        }
    }
}
