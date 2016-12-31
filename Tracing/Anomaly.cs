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

using FXGuild.Common.Tracing.Model;

namespace FXGuild.Common.Tracing
{
   public sealed class Anomaly : Observation
   {
      #region Properties

      public AnomalyType Type { get; }

      #endregion

      #region Constructors

      internal Anomaly(AnomalyType a_Type,
                       uint a_StackFramesToSkip,
                       params object[] a_DetailsParams)
         : base(a_Type.FormatMessage(a_DetailsParams), a_Type.VerboseLvl, a_StackFramesToSkip + 1)
      {
         Type = a_Type;
      }

      #endregion
   }
}
