﻿// MIT License
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
using System.Diagnostics;

using FXGuild.Common.Tracing.Model;

namespace FXGuild.Common.Tracing
{
   public sealed class Tracer : IDisposable
   {
      #region Private fields

      private readonly TraceModel m_TraceModel;
      private readonly Stopwatch m_Stopwatch;

      #endregion

      #region Constructors

      public Tracer(TraceModel a_TraceModel)
      {
         m_TraceModel = a_TraceModel;
         m_Stopwatch = Stopwatch.StartNew();
      }

      #endregion

      #region Destructors

      ~Tracer()
      {
         Dispose();
      }

      #endregion

      #region Methods

      public void Dispose()
      {
         m_Stopwatch.Stop();
      }

      #endregion
   }
}
