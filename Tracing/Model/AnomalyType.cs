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

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

using FXGuild.Common.Logging;

namespace FXGuild.Common.Tracing.Model
{
   [DataContract]
   public sealed class AnomalyType
   {
      #region Nested types

      public enum SeverityLevel
      {
         MINIMAL,
         MODERATE,
         CRITIC
      }

      #endregion

      #region Runtime constants

      private static readonly Dictionary<SeverityLevel, VerboseLevel> SEVERITY_TO_VERB_LBL
         = new Dictionary<SeverityLevel, VerboseLevel>
         {
            {SeverityLevel.MINIMAL, VerboseLevel.INFO},
            {SeverityLevel.MODERATE, VerboseLevel.WARNING},
            {SeverityLevel.CRITIC, VerboseLevel.ERROR}
         };

      #endregion

      #region Properties

      [DataMember(IsRequired = true)]
      public string Name { get; private set; }

      [DataMember(IsRequired = true)]
      public SeverityLevel Severity { get; private set; }

      [DataMember]
      public string DetailsFormat { get; private set; }

      [DataMember]
      public string Description { get; private set; }

      [DataMember]
      public string Consequence { get; private set; }

      public VerboseLevel VerboseLvl => SEVERITY_TO_VERB_LBL[Severity];

      #endregion

      #region Methods

      internal string FormatMessage(params object[] a_DetailsParams)
      {
         return new StringBuilder()
            .Append(Name)
            .Append(" - ")
            .Append(Consequence)
            .Append(" - ")
            .Append(Severity)
            .Append(" - ")
            .AppendFormat(DetailsFormat, a_DetailsParams).ToString();
      }

      [OnDeserializing]
      private void OnDeserializing(StreamingContext a_Context)
      {
         DetailsFormat = string.Empty;
         Description = "UNSPECIFIED";
         Consequence = "UNSPECIFIED";
      }

      #endregion
   }
}
