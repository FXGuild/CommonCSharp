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

using FXGuild.Common.Logging;
using FXGuild.Common.Tracing.Model;
using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Reflection;
using Module = FXGuild.Common.Tracing.Model.Module;

namespace FXGuild.Common.Tracing.TraceEnumGenerator
{
    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/650ax5cx(v=vs.110).aspx
    /// </summary>
    public static class MainProgram
    {
        #region Static methods

        private static void Main(string[] a_Args)
        {
            var logger = new Logger();

            // Check args
            if (a_Args.Length != 1)
            {
                logger.Error("Expected one argument");
                Environment.Exit(-1);
            }
            // Deserialize trace model
            var model = TraceModel.DeserializeTraceModel(a_Args[0]);

            // Create enum file DOM
            var compileUnit = new CodeCompileUnit();

            // Process modules
            ProcessModule(model.RootModule, compileUnit, model.RootModule.Namespace);

            // Create C# code provider
            var provider = new CSharpCodeProvider();

            // Write file
            using (var sw = new StreamWriter(a_Args[0].Replace(".json", ".cs"), false))
            {
                using (var tw = new IndentedTextWriter(sw))
                {
                    provider.GenerateCodeFromCompileUnit(compileUnit, tw,
                        new CodeGeneratorOptions
                        {
                            BlankLinesBetweenMembers = false,
                            BracingStyle = "C",
                            IndentString = "    "
                        });
                }
            }
        }

        private static void ProcessModule(Module a_Module,
                                          CodeCompileUnit a_CompileUnit,
                                          string a_Namespace)
        {
            // Create module namespace
            var namespaceDecl = new CodeNamespace(a_Namespace);

            // Skip namespaces without task classes
            if (a_Module.TaskClassDescriptions.Count > 0)
            {
                a_CompileUnit.Namespaces.Add(namespaceDecl);

                // For each Task class in the module
                foreach (var taskClassDesc in a_Module.TaskClassDescriptions)
                {
                    // Skip classes without tasks or anomaly types
                    if (taskClassDesc.TaskTypes.Count +
                        taskClassDesc.AnomalyTypes.Count == 0)
                    {
                        continue;
                    }

                    // Extend existing partial Task class
                    var partialClassDecl = new CodeTypeDeclaration(taskClassDesc.Name)
                    {
                        IsPartial = true,
                        TypeAttributes =
                            taskClassDesc.IsPublic
                                ? TypeAttributes.Public
                                : TypeAttributes.NotPublic
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
                            a_TaskType =>
                                new CodeMemberField("Task", a_TaskType.EnumName) as
                                    CodeTypeMember)
                            .ToArray());
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
                            a_AnomalyType =>
                                new CodeMemberField("Anomaly", a_AnomalyType.EnumName) as
                                    CodeTypeMember)
                            .ToArray());
                    }
                }
            }

            // Process submodules
            foreach (var subModule in a_Module.SubModules)
            {
                ProcessModule(subModule, a_CompileUnit,
                    namespaceDecl.Name + '.' + subModule.Namespace);
            }
        }

        #endregion
    }
}