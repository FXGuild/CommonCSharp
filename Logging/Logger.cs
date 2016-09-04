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
using System;
using System.Diagnostics;
using System.Text;

namespace FXGuild.Common.Logging
{
    public sealed class Logger
    {
        // This is because in Release builds, wrapper functions are inline so call stack is shorter
        private const uint WRAPPER_LOG_FUNC_STACK_FRAMES_TO_SKIP =
#if DEBUG
            1;
#else
            0;
#endif

        #region Properties

        public uint FunctionSignatureWidth { get; set; }
        public MethodSignatureVerboseLevel SignatureVerbose { get; set; }

        #endregion

        #region Constructors

        public Logger(uint a_FunctionSignatureWidth = 80,
                      MethodSignatureVerboseLevel a_SignatureVerbose =
                          MethodSignatureVerboseLevel.NAMESPACE_AND_NAME)
        {
            FunctionSignatureWidth = a_FunctionSignatureWidth;
            SignatureVerbose = a_SignatureVerbose;
        }

        #endregion

        #region Methods

        public void Log(VerboseLevel a_VerboseLvl,
                        uint a_StackFramesToSkip,
                        string a_MsgFormat,
                        params object[] a_Args)
        {
            var sb = new StringBuilder();

            // Append timestamp
            sb.Append(DateTime.Now.ToString("yyyy-MM-dd·HH:mm:ss.fff·"));

            // Append calling method signature
            sb.AppendFormat(GetCurrentMethodSignature(a_StackFramesToSkip + 1));

            // Append blank spaces if needed
            for (uint i = (uint) sb.Length - 1; i < FunctionSignatureWidth; ++i)
            {
                sb.Append(' ');
            }

            // Append separator
            sb.Append(" - ");

            // Append message
            sb.AppendFormat(a_MsgFormat, a_Args);

            // Print to console
            var prevColor = Console.ForegroundColor;
            Console.ForegroundColor = a_VerboseLvl.TextColor;
            Console.WriteLine(sb.ToString());
            Console.ForegroundColor = prevColor;
        }

        public void Debug(string a_MsgFormat, params object[] a_Args)
        {
            Log(VerboseLevel.DEBUG, WRAPPER_LOG_FUNC_STACK_FRAMES_TO_SKIP, a_MsgFormat, a_Args);
        }

        public void Info(string a_MsgFormat, params object[] a_Args)
        {
            Log(VerboseLevel.INFO, WRAPPER_LOG_FUNC_STACK_FRAMES_TO_SKIP, a_MsgFormat, a_Args);
        }

        public void Warning(string a_MsgFormat, params object[] a_Args)
        {
            Log(VerboseLevel.WARNING, WRAPPER_LOG_FUNC_STACK_FRAMES_TO_SKIP, a_MsgFormat, a_Args);
        }

        public void Error(string a_MsgFormat, params object[] a_Args)
        {
            Log(VerboseLevel.ERROR, WRAPPER_LOG_FUNC_STACK_FRAMES_TO_SKIP, a_MsgFormat, a_Args);
        }

        private string GetCurrentMethodSignature(uint a_StackFramesToSkip)
        {
            // Get current method
            var stackTrace = new StackTrace((int) a_StackFramesToSkip + 1);

            if (stackTrace.FrameCount == 0)
            {
                throw new ArgumentException("Too many stack frames to skip");
            }
            var method = stackTrace.GetFrame(0).GetMethod();

            // Append namespace
            var signSb = new StringBuilder();
            if (method.DeclaringType != null)
            {
                if (SignatureVerbose != MethodSignatureVerboseLevel.NAME_ONLY)
                {
                    signSb.Append(method.DeclaringType.Namespace).Append('.');
                }

                // Append type and method name
                signSb.Append(method.DeclaringType.Name).Append('.').Append(method.Name);
            }

            // Append parameters
            if (SignatureVerbose == MethodSignatureVerboseLevel.NAMESPACE_NAME_AND_PARAM_NAMES
                ||
                SignatureVerbose ==
                MethodSignatureVerboseLevel.NAMESPACE_NAME_PARAMS_NAMES_AND_NAMESPACES)
            {
                signSb.Append('(');
                var parameters = method.GetParameters();
                for (int i = 0; i < parameters.Length; ++i)
                {
                    // Append parameter namespaces
                    if (SignatureVerbose ==
                        MethodSignatureVerboseLevel.NAMESPACE_NAME_PARAMS_NAMES_AND_NAMESPACES)
                    {
                        signSb.Append(parameters[i].ParameterType.Namespace).Append('.');
                    }
                    signSb.Append(parameters[i].ParameterType.Name);
                    if (i < parameters.Length - 1)
                    {
                        signSb.Append(", ");
                    }
                }
                signSb.Append(')');
            }

            // Trim to fit
            string signature = signSb.ToString();
            return signature.Length < FunctionSignatureWidth
                ? signature.PadRight((int) FunctionSignatureWidth, ' ')
                : signature.Substring(0, (int) FunctionSignatureWidth);
        }

        #endregion
    }
}