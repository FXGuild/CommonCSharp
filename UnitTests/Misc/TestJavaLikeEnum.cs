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
   public sealed class Planet : JavaLikeEnum<Planet>
   {
      #region Runtime constants

      public static readonly Planet
         MERCURY = new Planet {Mass = 325.532},
         VENUS = new Planet {Mass = 4124.4214},
         EARTH = new Planet {Mass = 15.12};

      #endregion

      #region Properties

      public double Mass { get; private set; }

      #endregion
   }

   [TestClass]
   public sealed class TestJavaLikeEnum
   {
      #region Methods

      [TestMethod]
      public void SealedEnum_InitWithConstantCall_ValidBehavior()
      {
         var planet = Planet.EARTH;
         Assert.IsNotNull(planet);
      }

      #endregion
   }
}
