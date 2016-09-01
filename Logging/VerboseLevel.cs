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

namespace FXGuild.Common.Logging
{
    public sealed class VerboseLevel
    {
        #region Runtime constants

        public static readonly VerboseLevel
            DEBUG = new VerboseLevel(ConsoleColor.DarkGray),
            INFO = new VerboseLevel(ConsoleColor.Black),
            WARNING = new VerboseLevel(ConsoleColor.Blue),
            ERROR = new VerboseLevel(ConsoleColor.Cyan);

        #endregion

        #region Properties

        public ConsoleColor TextColor { get; }

        #endregion

        #region Constructors

        private VerboseLevel(ConsoleColor a_ConsoleColor)
        {
            TextColor = a_ConsoleColor;
        }

        #endregion
    }
}