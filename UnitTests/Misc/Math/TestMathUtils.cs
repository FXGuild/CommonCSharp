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

using FXGuild.Common.Misc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FXGuild.Common.UnitTests.Misc
{
    [TestClass]
    public class TestMathUtils
    {
        #region Methods

        [TestMethod]
        public void AlmostEquals_Equals_ValidResult()
        {
            Assert.IsTrue(MathUtils.AreAlmostEqual(12.9, 12.9));
        }

        [TestMethod]
        public void AlmostEquals_Different_ValidResult()
        {
            Assert.IsFalse(MathUtils.AreAlmostEqual(12.94214, 12.9));
        }

        [TestMethod]
        public void AlmostEquals_CustomEpsilon_ValidResult()
        {
            Assert.IsTrue(MathUtils.AreAlmostEqual(1, 1 + 1e-8, 1e-7));
        }
        
        [TestMethod]
        public void IsInteger_Integer_ValidResult()
        {
            Assert.IsTrue(MathUtils.IsInteger(532532.0));
            Assert.IsTrue(MathUtils.IsInteger(0.0));
            Assert.IsTrue(MathUtils.IsInteger(-1532.0));
        }

        [TestMethod]
        public void IsInterger_NonInteger_ValidResult()
        {
            Assert.IsFalse(MathUtils.IsInteger(532.532));
            Assert.IsFalse(MathUtils.IsInteger(-5.4));
        }

        [TestMethod]
        public void IsPowerOfTwo_Zero_ValidResult()
        {
            Assert.IsFalse(MathUtils.IsPowerOfTwo(0));
            Assert.IsFalse(MathUtils.IsPowerOfTwo(0U));
            Assert.IsFalse(MathUtils.IsPowerOfTwo(0L));
            Assert.IsFalse(MathUtils.IsPowerOfTwo(0UL));
            Assert.IsFalse(MathUtils.IsPowerOfTwo(0.0));
        }

        [TestMethod]
        public void IsPowerOfTwo_Positive_ValidResult()
        {
            Assert.IsTrue(MathUtils.IsPowerOfTwo(16U));
            Assert.IsTrue(MathUtils.IsPowerOfTwo(64UL));
            Assert.IsTrue(MathUtils.IsPowerOfTwo(8UL));
            Assert.IsTrue(MathUtils.IsPowerOfTwo(256.0));
            Assert.IsTrue(MathUtils.IsPowerOfTwo(0.125));

            Assert.IsFalse(MathUtils.IsPowerOfTwo(53));
            Assert.IsFalse(MathUtils.IsPowerOfTwo(2413UL));
            Assert.IsFalse(MathUtils.IsPowerOfTwo(4.53));
        }

        [TestMethod]
        public void IsPowerOfTwo_Negative_ValidResult()
        {
            Assert.IsFalse(MathUtils.IsPowerOfTwo(-65474));
            Assert.IsFalse(MathUtils.IsPowerOfTwo(-8L));
            Assert.IsFalse(MathUtils.IsPowerOfTwo(-256L));
            Assert.IsFalse(MathUtils.IsPowerOfTwo(-241L));
            Assert.IsFalse(MathUtils.IsPowerOfTwo(-0.4214));
        }

        [TestMethod]
        public void PreviousOrCurrentPowerOfTwo_PowersOf2_ValidResult()
        {
            Assert.AreEqual(32U, MathUtils.PreviousOrCurrentPowerOfTwo(32U));
            Assert.AreEqual(1UL, MathUtils.PreviousOrCurrentPowerOfTwo(1UL));
            Assert.AreEqual(2UL, MathUtils.PreviousOrCurrentPowerOfTwo(2UL));
            Assert.AreEqual(64UL, MathUtils.PreviousOrCurrentPowerOfTwo(64UL));
            Assert.AreEqual(2048.0, MathUtils.PreviousOrCurrentPowerOfTwo(2048.0));
        }

        [TestMethod]
        public void PreviousOrCurrentPowerOfTwo_PositiveNonPowersOf2_ValidResult()
        {
            Assert.AreEqual(16UL, MathUtils.PreviousOrCurrentPowerOfTwo(31UL));
            Assert.AreEqual(1024UL, MathUtils.PreviousOrCurrentPowerOfTwo(1252UL));
            Assert.AreEqual(2UL, MathUtils.PreviousOrCurrentPowerOfTwo(3UL));
            Assert.AreEqual(2048.0, MathUtils.PreviousOrCurrentPowerOfTwo(3568.124));
        }

        [TestMethod]
        public void NextOrCurrentPowerOfTwo_Zero_ValidResult()
        {
            Assert.AreEqual(1UL, MathUtils.NextOrCurrentPowerOfTwo(0));
            Assert.AreEqual(1UL, MathUtils.NextOrCurrentPowerOfTwo(0L));
            Assert.AreEqual(1UL, MathUtils.NextOrCurrentIntegralPowerOfTwo(0.0));
        }

        [TestMethod]
        public void NextOrCurrentPowerOfTwo_NegativeInputs_ValidResult()
        {
            Assert.AreEqual(1UL, MathUtils.NextOrCurrentPowerOfTwo(-1));
            Assert.AreEqual(1UL, MathUtils.NextOrCurrentPowerOfTwo(-467347));
            Assert.AreEqual(1UL, MathUtils.NextOrCurrentIntegralPowerOfTwo(-467.432));
        }

        [TestMethod]
        public void NextOrCurrentPowerOfTwo_PowersOf2_ValidResult()
        {
            Assert.AreEqual(1UL, MathUtils.NextOrCurrentPowerOfTwo(1));
            Assert.AreEqual(4UL, MathUtils.NextOrCurrentPowerOfTwo(4));
            Assert.AreEqual(256UL, MathUtils.NextOrCurrentPowerOfTwo(256UL));
            Assert.AreEqual(4096UL, MathUtils.NextOrCurrentIntegralPowerOfTwo(4096.0));
        }

        [TestMethod]
        public void NextOrCurrentPowerOfTwo_PositiveNonPowersOf2_ValidResult()
        {
            Assert.AreEqual(32UL, MathUtils.NextOrCurrentPowerOfTwo(31));
            Assert.AreEqual(2048UL, MathUtils.NextOrCurrentPowerOfTwo(1252));
            Assert.AreEqual(4UL, MathUtils.NextOrCurrentPowerOfTwo(3));
            Assert.AreEqual(128UL, MathUtils.NextOrCurrentIntegralPowerOfTwo(124.5425325));
        }

        #endregion
    }
}