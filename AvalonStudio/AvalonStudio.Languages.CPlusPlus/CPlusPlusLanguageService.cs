namespace AvalonStudio.Languages.CPlusPlus
{
    using Models;
    using Models.Solutions;
    using NClang;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Projects;
    using Projects.Standard;
    public class CPlusPlusLanguageService : ILanguageService
    {
        private ClangIndex clangIndex;

        private ClangTranslationUnit translationUnit;
        private bool translationUnitIsDirty;
        private static ClangIndex index = ClangService.CreateIndex();

        public CPlusPlusLanguageService()
        {
            clangIndex = ClangService.CreateIndex();
        }

        private NClang.ClangTranslationUnit GenerateTranslationUnit(ISourceFile file, List<ClangUnsavedFile> unsavedFiles)
        {
            NClang.ClangTranslationUnit result = null;

            if (File.Exists(file.Location))
            {
                var args = new List<string>();

                var superProject = file.Project.Solution.StartupProject as IStandardProject;
                var project = file.Project as IStandardProject;

                var toolchainIncludes = project.ToolChain.Includes;

                foreach (var include in toolchainIncludes)
                {
                    args.Add(string.Format("-I{0}", include));
                }

                // toolchain includes
                // This code is same as in toolchain, get compiler arguments... does this need a refactor, or toolchain get passed in? Clang take GCC compatible arguments.
                // perhaps this language service has its own clang tool chain, to generate compiler arguments from project configuration?


                // Referenced includes
                var referencedIncludes = project.GetReferencedIncludes();

                foreach (var include in referencedIncludes)
                {
                    args.Add(string.Format("-I\"{0}\"", Path.Combine(project.CurrentDirectory, include)));
                }

                // global includes
                var globalIncludes = superProject.GetGlobalIncludes();

                foreach (var include in globalIncludes)
                {
                    args.Add(string.Format("-I\"{0}\"", include));
                }

                // public includes
                foreach (var include in project.PublicIncludes)
                {
                    args.Add(string.Format("-I\"{0}\"", Path.Combine(project.CurrentDirectory, include)));
                }

                // includes
                foreach (var include in project.Includes)
                {
                    args.Add(string.Format("-I\"{0}\"", Path.Combine(project.CurrentDirectory, include)));
                }

                foreach (var define in superProject.Defines)
                {
                    args.Add(string.Format("-D{0}", define));
                }

                foreach (var arg in superProject.ToolChainArguments)
                {
                    args.Add(string.Format("{0}", arg));
                }

                foreach (var arg in superProject.CompilerArguments)
                {
                    args.Add(string.Format("{0}", arg));
                }

                switch (file.Language)
                {
                    case Language.C:
                        {
                            foreach (var arg in superProject.CCompilerArguments)
                            {
                                args.Add(string.Format("{0}", arg));
                            }
                        }
                        break;

                    case Language.Cpp:
                        {
                            foreach (var arg in superProject.CppCompilerArguments)
                            {
                                args.Add(string.Format("{0}", arg));
                            }
                        }
                        break;
                }

                if (file.Language == Language.Cpp)
                {
                    args.Add("-xc++");
                    args.Add("-std=c++14");
                }

                // this is a dependency on VEStudioSettings.
                if (VEStudioSettings.This.ShowAllWarnings)
                {
                    args.Add("-Weverything");
                }

                if (translationUnit == null || translationUnitIsDirty)
                {
                    if (this.translationUnit != null)
                    {
                        this.translationUnit.Reparse(unsavedFiles.ToArray(), this.translationUnit.DefaultReparseOptions);

                        result = this.translationUnit;
                    }
                    else
                    {
                        result = index.ParseTranslationUnit(file.Location, args.ToArray(), unsavedFiles.ToArray(), TranslationUnitFlags.CacheCompletionResults | TranslationUnitFlags.PrecompiledPreamble);
                    }
                }

                translationUnitIsDirty = false;
            }

            return result;
        }

        public List<CodeCompletionData> CodeCompleteAt(string fileName, int line, int column, List<UnsavedFile> unsavedFiles, string filter)
        {
            List<ClangUnsavedFile> clangUnsavedFiles = new List<ClangUnsavedFile>();

            foreach (var unsavedFile in unsavedFiles)
            {
                clangUnsavedFiles.Add(new ClangUnsavedFile(unsavedFile.FileName, unsavedFile.Contents));
            }

            var completionResults = translationUnit.CodeCompleteAt(fileName, line, column, clangUnsavedFiles.ToArray(), CodeCompleteFlags.IncludeMacros | CodeCompleteFlags.IncludeCodePatterns);
            completionResults.Sort();

            Console.WriteLine(completionResults.Contexts);

            var result = new List<CodeCompletionData>();

            foreach (var codeCompletion in completionResults.Results)
            {                                
                if (codeCompletion.CompletionString.Availability == AvailabilityKind.Available)
                {
                    for(int i = 0; i < codeCompletion.CompletionString.AnnotationCount; i++)
                    {
                        Console.WriteLine(codeCompletion.CompletionString.GetAnnotation(i));
                    }

                    string typedText = string.Empty;

                    string hint = string.Empty;

                    foreach (var chunk in codeCompletion.CompletionString.Chunks)
                    {
                        if (chunk.Kind == CompletionChunkKind.TypedText)
                        {
                            typedText = chunk.Text;
                        }

                        hint += chunk.Text + " ";
                    }

                    result.Add(new CodeCompletionData { Suggestion = typedText, Priority = codeCompletion.CompletionString.Priority });
                }
            }

            return result;
        }

        public CodeAnalysisResults RunCodeAnalysis(ISourceFile file, List<UnsavedFile> unsavedFiles, Func<bool> interruptRequested)
        {
            var result = new CodeAnalysisResults();

            List<ClangUnsavedFile> clangUnsavedFiles = new List<ClangUnsavedFile>();

            foreach (var unsavedFile in unsavedFiles)
            {
                clangUnsavedFiles.Add(new ClangUnsavedFile(unsavedFile.FileName, unsavedFile.Contents));
            }

            if (translationUnit == null)
            {
                translationUnit = GenerateTranslationUnit(file, clangUnsavedFiles);
            }
            else
            {
                translationUnit.Reparse(clangUnsavedFiles.ToArray(), ReparseTranslationUnitFlags.None);
            }

            if (file != null)
            {
                var callbacks = new NClang.ClangIndexerCallbacks();


                callbacks.IndexDeclaration += (handle, e) =>
                {
                    if (e.Cursor.Spelling != null && e.Location.SourceLocation.IsFromMainFile)
                    {
                        switch (e.Cursor.Kind)
                        {
                            case CursorKind.FunctionDeclaration:
                            case CursorKind.CXXMethod:
                            case CursorKind.Constructor:
                            case CursorKind.Destructor:
                            case CursorKind.VarDeclaration:
                            case CursorKind.ParmDeclaration:
                            case CursorKind.StructDeclaration:
                            case CursorKind.ClassDeclaration:
                            case CursorKind.TypedefDeclaration:
                            case CursorKind.ClassTemplate:
                            case CursorKind.EnumDeclaration:
                            case CursorKind.UnionDeclaration:
                                // TODO return code folding data.
                                break;
                        }


                        switch (e.Cursor.Kind)
                        {
                            case CursorKind.StructDeclaration:
                            case CursorKind.ClassDeclaration:
                            case CursorKind.TypedefDeclaration:
                            case CursorKind.ClassTemplate:
                            case CursorKind.EnumDeclaration:
                            case CursorKind.UnionDeclaration:
                            case CursorKind.CXXBaseSpecifier:
                                result.SyntaxHighlightingData.Add(new SyntaxHighlightingData() { Start = e.Cursor.CursorExtent.Start.FileLocation.Offset, Length = e.Cursor.CursorExtent.End.FileLocation.Offset - e.Cursor.CursorExtent.Start.FileLocation.Offset, Type = HighlightType.UserType });
                                break;

                        }
                    }
                };

                callbacks.IndexEntityReference += (handle, e) =>
                {
                    if (e.Cursor.Spelling != null && e.Location.SourceLocation.IsFromMainFile)
                    {
                        switch (e.Cursor.Kind)
                        {
                            case CursorKind.TypeReference:
                            case CursorKind.CXXBaseSpecifier:
                            case CursorKind.TemplateReference:
                                result.SyntaxHighlightingData.Add(new SyntaxHighlightingData() { Start = e.Cursor.CursorExtent.Start.FileLocation.Offset, Length = e.Cursor.CursorExtent.End.FileLocation.Offset - e.Cursor.CursorExtent.Start.FileLocation.Offset, Type = HighlightType.UserType });
                                break;
                        }
                    }
                };

                if (translationUnit != null)
                {
                    var tokens = translationUnit.Tokenize(translationUnit.GetCursor().CursorExtent);

                    foreach (var token in tokens.Tokens)
                    {
                        var highlightData = new SyntaxHighlightingData();
                        highlightData.Start = token.Extent.Start.FileLocation.Offset;
                        highlightData.Length = token.Extent.End.FileLocation.Offset - highlightData.Start; ;

                        switch (token.Kind)
                        {
                            case TokenKind.Comment:
                                highlightData.Type = HighlightType.Comment;
                                break;

                            case TokenKind.Identifier:
                                highlightData.Type = HighlightType.Identifier;
                                break;

                            case TokenKind.Punctuation:
                                highlightData.Type = HighlightType.Punctuation;
                                break;

                            case TokenKind.Keyword:
                                highlightData.Type = HighlightType.Keyword;
                                break;

                            case TokenKind.Literal:
                                highlightData.Type = HighlightType.Literal;
                                break;
                        }

                        result.SyntaxHighlightingData.Add(highlightData);
                    }

                    var indexAction = index.CreateIndexAction();
                    indexAction.IndexTranslationUnit(IntPtr.Zero, new NClang.ClangIndexerCallbacks[] { callbacks }, NClang.IndexOptionFlags.SkipParsedBodiesInSession, translationUnit);
                    indexAction.Dispose();
                }
            }

            var diags = translationUnit.DiagnosticSet.Items;

            foreach (var diag in diags)
            {
                result.Diagnostics.Add(new Diagnostic()
                {
                    Offset = diag.Location.FileLocation.Offset,
                    Spelling = diag.Spelling,
                });
            }

            return result;
        }

        public bool CanHandle(ISourceFile file)
        {
            bool result = false;

            switch(Path.GetExtension(file.Location))
            {
                case ".h":
                case ".cpp":
                case ".hpp":
                case ".c":
                    result = true;
                    break;
            }
            
            if(result)
            {
                if (!(file.Project is IStandardProject))
                {
                    result = false;
                }
            }

            return result;
        }
    }
}
