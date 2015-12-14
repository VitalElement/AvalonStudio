﻿namespace AvalonStudio.Models.LanguageServices.CPlusPlus
{
    using Solutions;
    using NClang;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.IO;

    public class CPlusPlusLanguageService : ILanguageService
    {
        private ClangIndex index;
        private ProjectFile file;
        private ClangTranslationUnit translationUnit;
        private bool translationUnitIsDirty;

        public CPlusPlusLanguageService(ClangIndex index, ProjectFile file)
        {
            this.index = index;
            this.file = file;
        }

        private NClang.ClangTranslationUnit GenerateTranslationUnit()
        {
            NClang.ClangTranslationUnit result = null;

            if (File.Exists(file.Location) && file.IsCodeFile)
            {
                var args = new List<string>();

                var arguments = file.DefaultProject.IncludeArguments;

                foreach (string argument in arguments)
                {
                    args.Add(argument.Replace("\"", ""));
                }

                foreach (var define in file.DefaultProject.SelectedConfiguration.Defines)
                {
                    if (define != string.Empty)
                    {
                        args.Add(string.Format("-D{0}", define));
                    }
                }

                if (file.FileType == Solutions.FileType.CPlusPlus || file.FileType == FileType.Header)
                {
                    args.Add("-xc++");
                    args.Add("-std=c++14");
                }

                if (VEStudioSettings.This.ShowAllWarnings)
                {
                    args.Add("-Weverything");
                }

                if (translationUnit == null || translationUnitIsDirty)
                {
                    if (this.translationUnit != null)
                    {
                        this.translationUnit.Reparse(file.Project.UnsavedFiles.ToArray(), this.translationUnit.DefaultReparseOptions);

                        result = this.translationUnit;
                    }
                    else
                    {
                        result = file.Solution.NClangIndex.ParseTranslationUnit(file.Location, args.ToArray(), file.Project.UnsavedFiles.ToArray(), TranslationUnitFlags.CacheCompletionResults | TranslationUnitFlags.PrecompiledPreamble);
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

        public CodeAnalysisResults RunCodeAnalysis(List<UnsavedFile> unsavedFiles, Func<bool> interruptRequested)
        {
            var result = new CodeAnalysisResults();

            if (translationUnit == null)
            {
                translationUnit = GenerateTranslationUnit();
            }

            List<ClangUnsavedFile> clangUnsavedFiles = new List<ClangUnsavedFile>();

            foreach (var unsavedFile in unsavedFiles)
            {
                clangUnsavedFiles.Add(new ClangUnsavedFile(unsavedFile.FileName, unsavedFile.Contents));
            }

            translationUnit.Reparse(clangUnsavedFiles.ToArray(), ReparseTranslationUnitFlags.None);

            if (file != null && file.IsCodeFile)
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

                    var indexAction = file.Solution.NClangIndex.CreateIndexAction();

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
    }
}
