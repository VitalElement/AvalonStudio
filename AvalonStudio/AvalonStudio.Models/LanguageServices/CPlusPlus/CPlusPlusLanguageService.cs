﻿namespace AvalonStudio.Models.LanguageServices.CPlusPlus
{
    using Solutions;
    using NClang;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.IO;
    using TextEditor;

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

        public List<CodeCompletionData> CodeCompleteAt(uint line, uint column)
        {
            throw new NotImplementedException();
        }

        public SyntaxHighlightDataList RunCodeAnalysis(Func<bool> interruptRequested)
        {
            SyntaxHighlightDataList result = new SyntaxHighlightDataList();

            if (translationUnit == null)
            {
                translationUnit = GenerateTranslationUnit();
            }

            translationUnit.Reparse(file.Project.UnsavedFiles.ToArray(), ReparseTranslationUnitFlags.None);

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
                                result.Add(new SyntaxHighlightingData() { Start = e.Cursor.CursorExtent.Start.FileLocation.Offset, Length = e.Cursor.CursorExtent.End.FileLocation.Offset - e.Cursor.CursorExtent.Start.FileLocation.Offset, Type = HighlightType.UserType });
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
                                result.Add(new SyntaxHighlightingData() { Start = e.Cursor.CursorExtent.Start.FileLocation.Offset, Length = e.Cursor.CursorExtent.End.FileLocation.Offset - e.Cursor.CursorExtent.Start.FileLocation.Offset, Type = HighlightType.UserType });
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

                        result.Add(highlightData);
                    }

                    var indexAction = file.Solution.NClangIndex.CreateIndexAction();

                    indexAction.IndexTranslationUnit(IntPtr.Zero, new NClang.ClangIndexerCallbacks[] { callbacks }, NClang.IndexOptionFlags.SkipParsedBodiesInSession, translationUnit);
                    indexAction.Dispose();
                }
            }
            
            Console.WriteLine("Code Analysis completed.");

            return result;
        }
    }
}
