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

namespace FXGuild.Common.Logging
{
   public static class LoggerExtensions
   {
      #region Compile-time constants

      // This is because in Release builds, wrapper functions are inline so call stack is shorter
      internal const uint WRAPPER_STACK_FRAMES_TO_SKIP =
#if DEBUG
         1;
#else
         0;
#endif

      #endregion

      #region Static methods

      public static void Debug(this Logger a_Logger, string a_MsgFormat, params object[] a_Args)
      {
         a_Logger.Log(VerboseLevel.DEBUG, WRAPPER_STACK_FRAMES_TO_SKIP, a_MsgFormat, a_Args);
      }

      public static void Info(this Logger a_Logger, string a_MsgFormat, params object[] a_Args)
      {
         a_Logger.Log(VerboseLevel.INFO, WRAPPER_STACK_FRAMES_TO_SKIP, a_MsgFormat, a_Args);
      }

      public static void Warning(this Logger a_Logger, string a_MsgFormat, params object[] a_Args)
      {
         a_Logger.Log(VerboseLevel.WARNING, WRAPPER_STACK_FRAMES_TO_SKIP, a_MsgFormat, a_Args);
      }

      public static void Error(this Logger a_Logger, string a_MsgFormat, params object[] a_Args)
      {
         a_Logger.Log(VerboseLevel.ERROR, WRAPPER_STACK_FRAMES_TO_SKIP, a_MsgFormat, a_Args);
      }

      #endregion
   }
}
