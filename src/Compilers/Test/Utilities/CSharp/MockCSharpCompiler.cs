﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Test.Utilities;

namespace Microsoft.CodeAnalysis.CSharp.Test.Utilities
{
    internal class MockCSharpCompiler : CSharpCompiler
    {
        protected readonly ImmutableArray<DiagnosticAnalyzer> _analyzers;
        internal Compilation Compilation;
        internal AnalyzerOptions AnalyzerOptions;

        public MockCSharpCompiler(string responseFile, string baseDirectory, string[] args)
            : this(responseFile, baseDirectory, args, ImmutableArray<DiagnosticAnalyzer>.Empty, RuntimeUtilities.CreateAnalyzerAssemblyLoader())
        {
        }

        public MockCSharpCompiler(string responseFile, BuildPaths buildPaths, string[] args)
            : this(responseFile, buildPaths, args, ImmutableArray<DiagnosticAnalyzer>.Empty, RuntimeUtilities.CreateAnalyzerAssemblyLoader())
        {
        }

        public MockCSharpCompiler(string responseFile, string workingDirectory, string[] args, ImmutableArray<DiagnosticAnalyzer> analyzers, AnalyzerAssemblyLoader loader)
            : this(responseFile, CreateBuildPaths(workingDirectory), args, analyzers, loader)
        {
        }

        public MockCSharpCompiler(string responseFile, BuildPaths buildPaths, string[] args, ImmutableArray<DiagnosticAnalyzer> analyzers, AnalyzerAssemblyLoader loader)
            : base(CSharpCommandLineParser.Default, responseFile, args, buildPaths, Environment.GetEnvironmentVariable("LIB"), loader)
        {
            _analyzers = analyzers;
        }

        private static BuildPaths CreateBuildPaths(string workingDirectory) => RuntimeUtilities.CreateBuildPaths(workingDirectory);

        protected override ImmutableArray<DiagnosticAnalyzer> ResolveAnalyzersFromArguments(
            List<DiagnosticInfo> diagnostics,
            CommonMessageProvider messageProvider)
        {
            var analyzers = base.ResolveAnalyzersFromArguments(diagnostics, messageProvider);
            if (!_analyzers.IsDefaultOrEmpty)
            {
                analyzers = analyzers.InsertRange(0, _analyzers);
            }
            return analyzers;
        }

        public Compilation CreateCompilation(
            TextWriter consoleOutput,
            TouchedFileLogger touchedFilesLogger,
            ErrorLogger errorLogger)
            => CreateCompilation(consoleOutput, touchedFilesLogger, errorLogger, syntaxDiagOptionsOpt: default);

        public override Compilation CreateCompilation(
            TextWriter consoleOutput,
            TouchedFileLogger touchedFilesLogger,
            ErrorLogger errorLogger,
            ImmutableArray<ImmutableDictionary<string, ReportDiagnostic>> syntaxDiagOptionsOpt)
        {
            Compilation = base.CreateCompilation(consoleOutput, touchedFilesLogger, errorLogger, syntaxDiagOptionsOpt);
            return Compilation;
        }

        protected override AnalyzerOptions CreateAnalyzerOptions(
            ImmutableArray<AdditionalText> additionalTextFiles,
            AnalyzerConfigOptionsProvider analyzerConfigOptionsProvider)
        {
            AnalyzerOptions = base.CreateAnalyzerOptions(additionalTextFiles, analyzerConfigOptionsProvider);
            return AnalyzerOptions;
        }
    }
}
