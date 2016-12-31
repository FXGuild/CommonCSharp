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
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;

using FXGuild.Common.Logging;
using FXGuild.Common.Tracing.Model;

using Microsoft.CSharp;

using Module = FXGuild.Common.Tracing.Model.Module;

// ReSharper disable BitwiseOperatorOnEnumWithoutFlags

namespace FXGuild.Common.Tracing.TraceEnumGenerator
{
   /// <summary>
   /// https://msdn.microsoft.com/en-us/library/650ax5cx(v=vs.110).aspx
   /// </summary>
   public static class MainProgram
   {
      #region Compile-time constants

      private const string HELP_MSG = "Expected two argument: " +
                                      "(1) path to a TraceModel.json file " +
                                      "(2) path to the bin directory";

      #endregion

      #region Static methods

      private static void Main(string[] a_Args)
      {
         var logger = new Logger();
         logger.Info("Trace Enum Generator");

         // Check args
         if (a_Args.Length != 2)
         {
            logger.Error(HELP_MSG);
            Environment.Exit(-1);
         }

         // Deserialize trace model
         logger.Info("Deserializing trace model \"" + a_Args[0] + "\"");
         TraceModel model;
         using (var stream = File.OpenRead(a_Args[0]))
         {
            var serializer = new DataContractJsonSerializer(typeof(TraceModel));
            model = serializer.ReadObject(stream) as TraceModel;
            if (model == null)
               // TODO exception details
               throw new Exception();
         }

         // Create enum file DOM
         var compileUnit = new CodeCompileUnit();

         // Add Tracing using statement
         compileUnit.Namespaces.Add(new CodeNamespace
         {
            Imports = {new CodeNamespaceImport("FXGuild.Common.Tracing")}
         });

         // Process modules
         logger.Info("Generating enum file DOM...");
         ProcessModule(model.RootModule, compileUnit, model.RootModule.Namespace);

         // Write C# enum file
         string outputFile = a_Args[0].Replace(".json", ".cs");
         logger.Info("Writing C# file to \"" + outputFile + "\"");
         var provider = new CSharpCodeProvider();
         using (var sw = new StreamWriter(outputFile, false))
         using (var tw = new IndentedTextWriter(sw))
            provider.GenerateCodeFromCompileUnit(compileUnit, tw,
               new CodeGeneratorOptions
               {
                  BlankLinesBetweenMembers = false,
                  BracingStyle = "C",
                  IndentString = "    "
               });

         // Check if program must pause before existing
         bool mustPause = a_Args[1].Contains(" & pause");
         if (mustPause)
            a_Args[1] = a_Args[1].Replace(" & pause", string.Empty);

         // Copy TraceModel.json file to bin directory
         outputFile = a_Args[1] + "/" + model.RootModule.Namespace +
                      TracerProvider.TRACE_MODEL_SUFFIX;
         logger.Info("Copying C# file to \"" + outputFile + "\"");
         File.Copy(a_Args[0], outputFile, true);

         // Pause if asked
         if (mustPause)
         {
            Console.WriteLine("Press any key to continue...");
            Console.Read();
         }
      }

      private static void ProcessModule(Module a_Module,
                                        CodeCompileUnit a_CompileUnit,
                                        string a_Namespace)
      {
         // Create module namespace
         var namespaceDecl = new CodeNamespace(a_Namespace);

         // Skip namespaces without task classes
         if (a_Module.TaskClassesDescriptions.Count > 0)
         {
            a_CompileUnit.Namespaces.Add(namespaceDecl);

            // For each Task class in the module
            foreach (var taskClassDesc in a_Module.TaskClassesDescriptions)
            {
               // Skip classes without tasks or anomaly types
               if (taskClassDesc.TaskTypes.Count + taskClassDesc.AnomalyTypes.Count == 0)
                  continue;

               // Extend existing partial Task class
               var typeAttr = taskClassDesc.IsPublic
                  ? TypeAttributes.Public
                  : TypeAttributes.NotPublic;
               if (taskClassDesc.IsSealed)
                  typeAttr |= TypeAttributes.Sealed;
               var partialClassDecl = new CodeTypeDeclaration(taskClassDesc.Name)
               {
                  IsPartial = true,
                  TypeAttributes = typeAttr
               };
               namespaceDecl.Types.Add(partialClassDecl);

               // Create task enum
               if (taskClassDesc.TaskTypes.Count > 0)
               {
                  var enumTypeDecl = new CodeTypeDeclaration("Task")
                  {
                     IsEnum = true,
                     TypeAttributes = TypeAttributes.NestedPrivate
                  };
                  partialClassDecl.Members.Add(enumTypeDecl);
                  enumTypeDecl.Members.AddRange(taskClassDesc.TaskTypes.Select(
                     a_TaskType => new CodeMemberField("Task",
                        ConvertToEnumName(a_TaskType.Name)) as CodeTypeMember).ToArray());
               }

               // Create anomaly enum
               if (taskClassDesc.AnomalyTypes.Count > 0)
               {
                  var enumTypeDecl = new CodeTypeDeclaration("Anomaly")
                  {
                     IsEnum = true,
                     TypeAttributes = TypeAttributes.NestedPrivate
                  };
                  partialClassDecl.Members.Add(enumTypeDecl);
                  enumTypeDecl.Members.AddRange(taskClassDesc.AnomalyTypes.Select(
                     a_AnomalyType => new CodeMemberField("Anomaly",
                        ConvertToEnumName(a_AnomalyType.Name)) as CodeTypeMember).ToArray());
               }

               // Add Tracer handle
               partialClassDecl.Members.Add(new CodeMemberField
               {
                  Attributes = MemberAttributes.Static |
                               (taskClassDesc.IsSealed
                                  ? MemberAttributes.Private
                                  : MemberAttributes.Family),
                  Type = new CodeTypeReference {BaseType = "Tracer"},
                  Name = "CLASS_TRACER",
                  InitExpression = new CodeMethodInvokeExpression
                  {
                     Method = new CodeMethodReferenceExpression
                     {
                        TargetObject = new CodeTypeReferenceExpression
                        {
                           Type = new CodeTypeReference {BaseType = "TracerProvider"}
                        },
                        MethodName = "GetTracer",
                        TypeArguments = {new CodeTypeReference {BaseType = taskClassDesc.Name}}
                     }
                  }
               });
            }
         }

         // Process submodules
         foreach (var subModule in a_Module.SubModules)
            ProcessModule(subModule, a_CompileUnit,
               namespaceDecl.Name + '.' + subModule.Namespace);
      }

      private static string ConvertToEnumName(string a_Str)
      {
         string clean = Regex.Replace(a_Str, @"[^\w\s]", string.Empty);
         return clean.Replace(' ', '_').ToUpper();
      }

      #endregion
   }
}
