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
    public static class MathUtils
    {
        #region Static methods

        [Pure]
        public static bool AreAlmostEqual(double a_A,
                                          double a_B,
                                          double a_Epsilon = double.Epsilon)
        {
            return Math.Abs(a_A - a_B) < a_Epsilon;
        }

        [Pure]
        public static bool IsPowerOfTwo(long a_Val)
        {
            return (a_Val > 0) && IsPowerOfTwo((ulong) a_Val);
        }

        [Pure]
        public static bool IsPowerOfTwo(ulong a_Val)
        {
            return (a_Val != 0) && ((a_Val & (~a_Val + 1)) == a_Val);
        }

        [Pure]
        public static ulong PreviousOrCurrentPowerOfTwo(long a_Val)
        {
            return (ulong) PreviousOrCurrentPowerOfTwo((double) a_Val);
        }

        [Pure]
        public static ulong PreviousOrCurrentPowerOfTwo(ulong a_Val)
        {
            return (ulong) PreviousOrCurrentPowerOfTwo((double) a_Val);
        }

        [Pure]
        public static double PreviousOrCurrentPowerOfTwo(double a_Val)
        {
            return a_Val <= 0 ? 0 : Math.Pow(2, Math.Floor(Math.Log(a_Val, 2)));
        }

        [Pure]
        public static ulong NextOrCurrentPowerOfTwo(long a_Val)
        {
            return (ulong) NextOrCurrentPowerOfTwo((double) a_Val);
        }

        [Pure]
        public static ulong NextOrCurrentPowerOfTwo(ulong a_Val)
        {
            return (ulong) NextOrCurrentPowerOfTwo((double) a_Val);
        }

        [Pure]
        public static double NextOrCurrentPowerOfTwo(double a_Val)
        {
            return a_Val <= 0 ? 0 : Math.Pow(2, Math.Ceiling(Math.Log(a_Val, 2)));
        }

        [Pure]
        public static bool IsInteger(double a_Val)
        {
            return AreAlmostEqual(a_Val, Math.Round(a_Val));
        }

        #endregion
    }
}