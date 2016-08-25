// MIT License
//
// Copyright (c) 2016 FXGuild
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute,
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Diagnostics.Contracts;

namespace FXGuild.Common.Misc
{
    [Pure]
    public static class MathUtils
    {
        #region Static methods

        public static bool AreAlmostEqual(double a_A,
                                          double a_B,
                                          double a_Epsilon = double.Epsilon)
        {
            return Math.Abs(a_A - a_B) < a_Epsilon;
        }
        
        public static bool IsInteger(double a_Val, double a_Epsilon = double.Epsilon)
        {
            return AreAlmostEqual(a_Val, Math.Round(a_Val), a_Epsilon);
        }

        public static bool IsPowerOfTwo(int a_Val)
        {
            return IsPowerOfTwo((long) a_Val);
        }

        public static bool IsPowerOfTwo(uint a_Val)
        {
            return IsPowerOfTwo((ulong) a_Val);
        }

        public static bool IsPowerOfTwo(long a_Val)
        {
            return (a_Val > 0) && IsPowerOfTwo((ulong) a_Val);
        }
        
        public static bool IsPowerOfTwo(ulong a_Val)
        {
            return (a_Val != 0) && ((a_Val & (~a_Val + 1)) == a_Val);
        }

        public static bool IsPowerOfTwo(double a_Val, double a_Epsilon = double.Epsilon)
        {
            double log2 = Math.Log(a_Val, 2);
            return IsInteger(log2, a_Epsilon);
        }

        public static uint PreviousOrCurrentPowerOfTwo(uint a_Val)
        {
            Contract.Requires(a_Val != 0);
            Contract.Ensures(IsPowerOfTwo(Contract.Result<uint>()));
            
            Contract.Assume((double) a_Val > 0);
            uint result = (uint) PreviousOrCurrentPowerOfTwo((double) a_Val);
            Contract.Assume(IsPowerOfTwo(result));

            return result;
        }

        public static ulong PreviousOrCurrentPowerOfTwo(ulong a_Val)
        {
            Contract.Requires(a_Val != 0);
            Contract.Ensures(IsPowerOfTwo(Contract.Result<ulong>()));
            
            Contract.Assume((double) a_Val > 0);
            ulong result = (ulong) PreviousOrCurrentPowerOfTwo((double) a_Val);
            Contract.Assume(IsPowerOfTwo(result));

            return result;
        }

        public static double PreviousOrCurrentPowerOfTwo(double a_Val)
        {
            Contract.Requires(a_Val > 0);
            Contract.Ensures(IsPowerOfTwo(Contract.Result<double>()));

            double result = Math.Pow(2, Math.Floor(Math.Log(a_Val, 2)));
            Contract.Assume(IsPowerOfTwo(result));

            return result;
        }

        public static int NextOrCurrentPowerOfTwo(int a_Val)
        {
            Contract.Ensures(IsPowerOfTwo(Contract.Result<int>()));
            int result = (int) NextOrCurrentIntegralPowerOfTwo(a_Val);
            Contract.Assume(IsPowerOfTwo(result));

            return result;
        }

        public static uint NextOrCurrentPowerOfTwo(uint a_Val)
        {
            Contract.Ensures(IsPowerOfTwo(Contract.Result<uint>()));
            uint result = (uint) NextOrCurrentIntegralPowerOfTwo(a_Val);
            Contract.Assume(IsPowerOfTwo(result));

            return result;
        }

        public static ulong NextOrCurrentPowerOfTwo(long a_Val)
        {
            Contract.Ensures(IsPowerOfTwo(Contract.Result<ulong>()));
            return NextOrCurrentIntegralPowerOfTwo(a_Val);
        }

        public static ulong NextOrCurrentPowerOfTwo(ulong a_Val)
        {
            Contract.Ensures(IsPowerOfTwo(Contract.Result<ulong>()));
            return NextOrCurrentIntegralPowerOfTwo(a_Val);
        }

        public static ulong NextOrCurrentIntegralPowerOfTwo(double a_Val)
        {
            Contract.Ensures(IsPowerOfTwo(Contract.Result<ulong>()));

            ulong result = a_Val <= 1 
                ? 1
                : (ulong) Math.Pow(2, Math.Ceiling(Math.Log(a_Val, 2)));
            Contract.Assume(IsPowerOfTwo(result));

            return result;
        }

        #endregion
    }
}