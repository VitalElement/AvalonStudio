namespace AvalonStudio.Languages.CPlusPlus
{
    using NClang;
    using Perspex.Media;
    using Perspex.Threading;
    using Projects;
    using Projects.Standard;
    using Rendering;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using TextEditor.Document;
    using TextEditor.Rendering;    

    class CPlusPlusDataAssociation
    {
        public CPlusPlusDataAssociation(TextDocument textDocument)
        {            
            BackgroundRenderers = new List<IBackgroundRenderer>();
            DocumentLineTransformers = new List<IDocumentLineTransformer>();

            TextColorizer = new TextColoringTransformer(textDocument);
            TextMarkerService = new TextMarkerService(textDocument);
           
            DocumentLineTransformers.Add(TextColorizer);
            DocumentLineTransformers.Add(TextMarkerService);
            DocumentLineTransformers.Add(new DefineTextLineTransformer());
            DocumentLineTransformers.Add(new PragmaMarkTextLineTransformer());
            DocumentLineTransformers.Add(new IncludeTextLineTransformer());
        }

        public ClangTranslationUnit TranslationUnit { get; set; }
        public TextColoringTransformer TextColorizer { get; private set; }
        public TextMarkerService TextMarkerService { get; private set; }        
        public List<IBackgroundRenderer> BackgroundRenderers { get; private set; }
        public List<IDocumentLineTransformer> DocumentLineTransformers { get; private set; }
    }

    public class CPlusPlusLanguageService : ILanguageService
    {
        private static ClangIndex index = ClangService.CreateIndex();
        private static ConditionalWeakTable<ISourceFile, CPlusPlusDataAssociation> dataAssociations = new ConditionalWeakTable<ISourceFile, CPlusPlusDataAssociation>();

        public CPlusPlusLanguageService()
        {
            
        }

        public string Title
        {
            get { return "C/C++"; }
        }

        public IProjectTemplate EmptyProjectTemplate
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Type BaseTemplateType
        {
            get
            {
                return typeof(BlankCPlusPlusLangaguageTemplate);
            }
        }

        private NClang.ClangTranslationUnit GenerateTranslationUnit(ISourceFile file, List<ClangUnsavedFile> unsavedFiles)
        {
            NClang.ClangTranslationUnit result = null;

            if (File.Exists(file.Location))
            {
                var args = new List<string>();

                var superProject = file.Project.Solution.StartupProject as IStandardProject;
                var project = file.Project as IStandardProject;

                var toolchainIncludes = superProject.ToolChain?.Includes;

                if (toolchainIncludes != null)
                {
                    foreach (var include in toolchainIncludes)
                    {
                        args.Add(string.Format("-I{0}", include));
                    }
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
                    args.Add(string.Format("-I\"{0}\"", Path.Combine(project.CurrentDirectory, include.Value)));
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
                //if (VEStudioSettings.This.ShowAllWarnings)
                //{
                //    args.Add("-Weverything");
                //}

                result = index.ParseTranslationUnit(file.Location, args.ToArray(), unsavedFiles.ToArray(), TranslationUnitFlags.CacheCompletionResults | TranslationUnitFlags.PrecompiledPreamble);
            }

            if(result == null)
            {
                throw new Exception("Error generating translation unit.");
            }

            return result;
        }

        private CPlusPlusDataAssociation GetAssociatedData (ISourceFile sourceFile)
        {
            CPlusPlusDataAssociation result = null;

            if (!dataAssociations.TryGetValue(sourceFile, out result))
            {
                throw new Exception("Tried to parse file that has not been registered with the language service.");
            }

            return result;
        }

        private ClangTranslationUnit GetAndParseTranslationUnit(ISourceFile sourceFile, List<ClangUnsavedFile> unsavedFiles)
        {
            var dataAssociation = GetAssociatedData(sourceFile);

            if (dataAssociation.TranslationUnit == null)
            {
                dataAssociation.TranslationUnit = GenerateTranslationUnit(sourceFile, unsavedFiles);
            }
            else
            {
                dataAssociation.TranslationUnit.Reparse(unsavedFiles.ToArray(), ReparseTranslationUnitFlags.None);
            }

            return dataAssociation.TranslationUnit;
        }

        public List<CodeCompletionData> CodeCompleteAt(ISourceFile file, int line, int column, List<UnsavedFile> unsavedFiles, string filter)
        {            
            List<ClangUnsavedFile> clangUnsavedFiles = new List<ClangUnsavedFile>();

            foreach (var unsavedFile in unsavedFiles)
            {
                clangUnsavedFiles.Add(new ClangUnsavedFile(unsavedFile.FileName, unsavedFile.Contents));
            }

            var translationUnit = GetAndParseTranslationUnit(file, clangUnsavedFiles);

            var completionResults = translationUnit.CodeCompleteAt(file.Location, line, column, clangUnsavedFiles.ToArray(), CodeCompleteFlags.IncludeMacros | CodeCompleteFlags.IncludeCodePatterns);
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

            var dataAssociation = GetAssociatedData(file);

            List<ClangUnsavedFile> clangUnsavedFiles = new List<ClangUnsavedFile>();

            foreach (var unsavedFile in unsavedFiles)
            {
                clangUnsavedFiles.Add(new ClangUnsavedFile(unsavedFile.FileName, unsavedFile.Contents));
            }

            var translationUnit = GetAndParseTranslationUnit(file, clangUnsavedFiles);            

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
                    //var annotatedTokens = tokens.Annotate();           //TODO see if this can provide us with additional data.

                    foreach (var token in tokens.Tokens)
                    {
                        var highlightData = new SyntaxHighlightingData();
                        highlightData.Start = token.Extent.Start.FileLocation.Offset;
                        highlightData.Length = token.Extent.End.FileLocation.Offset - highlightData.Start;
                        

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

            dataAssociation.TextMarkerService.Clear();

            var diags = translationUnit.DiagnosticSet.Items;

            foreach (var diagnostic in diags)
            {
                if (diagnostic.Location.IsFromMainFile)
                {
                    var diag = new Diagnostic()
                    {
                        Project = file.Project,
                        Offset = diagnostic.Location.FileLocation.Offset,
                        Line = diagnostic.Location.FileLocation.Line,
                        Spelling = diagnostic.Spelling,
                        File = diagnostic.Location.FileLocation.File.FileName,
                        Level = (DiagnosticLevel)diagnostic.Severity
                    };

                    
                    result.Diagnostics.Add(diag);

                    var data = dataAssociation.TranslationUnit.GetLocationForOffset(dataAssociation.TranslationUnit.GetFile(file.Location), diag.Offset);
                    var length = 0;                                       

                    if (diagnostic.RangeCount > 0)
                    {
                        length = Math.Abs(diagnostic.GetDiagnosticRange(0).End.FileLocation.Offset - diag.Offset);
                    }

                    if(diagnostic.FixItCount > 0)
                    {
                        // TODO implement fixits.
                    }
                    
                    Color markerColor;

                    switch (diag.Level)
                    {
                        case DiagnosticLevel.Error:
                        case DiagnosticLevel.Fatal:
                            markerColor = Color.FromRgb(253, 45, 45);
                            break;

                        case DiagnosticLevel.Warning:
                            markerColor = Color.FromRgb(255, 207, 40);
                            break;

                        default:
                            markerColor = Color.FromRgb(0, 42, 74);
                            break;

                    }

                    dataAssociation.TextMarkerService.Create(diag.Offset, length, diag.Spelling, markerColor);
                }
            }
            
            dataAssociation.TextColorizer.SetTransformations(result.SyntaxHighlightingData);

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

        public void RegisterSourceFile(ISourceFile file, TextDocument textDocument)
        {
            CPlusPlusDataAssociation existingAssociation = null;

            if (dataAssociations.TryGetValue(file, out existingAssociation))
            {                
                throw new Exception("Source file already registered with language service.");
            }
            else
            {
                dataAssociations.Add(file, new CPlusPlusDataAssociation(textDocument));
            }
        }

        public IList<IDocumentLineTransformer> GetDocumentLineTransformers(ISourceFile file)
        {
            var associatedData = GetAssociatedData(file);

            return associatedData.DocumentLineTransformers;
        }

        public IList<IBackgroundRenderer> GetBackgroundRenderers(ISourceFile file)
        {
            var associatedData = GetAssociatedData(file);

            return associatedData.BackgroundRenderers;
        }

        public void UnregisterSourceFile(ISourceFile file)
        {
            dataAssociations.Remove(file);
        }
    }
}
