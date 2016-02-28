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
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Xml.Linq;
    using TextEditor;
    using TextEditor.Document;
    using TextEditor.Rendering;
    using TextEditor.Indentation;
    using Perspex.Utilities;
    using Perspex.Input;
    using Utils;
    using Extensibility;
    using System.Threading;
    public class CPlusPlusLanguageService : ILanguageService
    {
        private static ClangIndex index = ClangService.CreateIndex();
        private static ConditionalWeakTable<ISourceFile, CPlusPlusDataAssociation> dataAssociations = new ConditionalWeakTable<ISourceFile, CPlusPlusDataAssociation>();

        public CPlusPlusLanguageService()
        {
            indentationStrategy = new CppIndentationStrategy();

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

        private IIndentationStrategy indentationStrategy;
        public IIndentationStrategy IndentationStrategy
        {
            get
            {
                return indentationStrategy;
            }
        }

        void AddArgument (List<string> list, string argument)
        {
            if(!list.Contains(argument))
            {
                list.Add(argument);
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
                        AddArgument(args, string.Format("-I{0}", include));
                    }
                }

                // toolchain includes
                // This code is same as in toolchain, get compiler arguments... does this need a refactor, or toolchain get passed in? Clang take GCC compatible arguments.
                // perhaps this language service has its own clang tool chain, to generate compiler arguments from project configuration?


                // Referenced includes
                var referencedIncludes = project.GetReferencedIncludes();

                foreach (var include in referencedIncludes)
                {
                    AddArgument(args, string.Format("-I{0}", include));
                }

                // global includes
                var globalIncludes = superProject.GetGlobalIncludes();

                foreach (var include in globalIncludes)
                {
                    AddArgument(args, string.Format("-I{0}", include));
                }

                // public includes
                foreach (var include in project.PublicIncludes)
                {
                    AddArgument(args, string.Format("-I{0}", include));
                }

                // includes
                foreach (var include in project.Includes)
                {
                    AddArgument(args, string.Format("-I{0}", Path.Combine(project.CurrentDirectory, include.Value)));
                }

                var referencedDefines = project.GetReferencedDefines();
                foreach (var define in referencedDefines)
                {
                    AddArgument(args, string.Format("-D{0}", define));
                }

                // global includes
                var globalDefines = superProject.GetGlobalDefines();

                foreach (var define in globalDefines)
                {
                    AddArgument(args, string.Format("-D{0}", define));
                }

                foreach (var define in project.Defines)
                {
                    AddArgument(args, string.Format("-D{0}", define));
                }

                //foreach (var arg in superProject.ToolChainArguments)
                //{
                //    args.Add(string.Format("{0}", arg));
                //}

                //foreach (var arg in superProject.CompilerArguments)
                //{
                //    args.Add(string.Format("{0}", arg));
                //}

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

                // TODO find out why TranslationUnitFlags.PreCompiledPreAmble causes crashing. in Libclang 3.8RC2
                result = index.ParseTranslationUnit(file.Location, args.ToArray(), unsavedFiles.ToArray(), TranslationUnitFlags.IncludeBriefCommentsInCodeCompletion | TranslationUnitFlags.PrecompiledPreamble | TranslationUnitFlags.CacheCompletionResults);
            }

            if (result == null)
            {
                throw new Exception("Error generating translation unit.");
            }

            return result;
        }

        private CPlusPlusDataAssociation GetAssociatedData(ISourceFile sourceFile)
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

        private Semaphore clangAccessSemaphore = new Semaphore(1, 1);

        public List<CodeCompletionData> CodeCompleteAt(ISourceFile file, int line, int column, List<UnsavedFile> unsavedFiles, string filter)
        {
            List<ClangUnsavedFile> clangUnsavedFiles = new List<ClangUnsavedFile>();

            foreach (var unsavedFile in unsavedFiles)
            {
                clangUnsavedFiles.Add(new ClangUnsavedFile(unsavedFile.FileName, unsavedFile.Contents));
            }

            clangAccessSemaphore.WaitOne();
            var translationUnit = GetAndParseTranslationUnit(file, clangUnsavedFiles);

            var completionResults = translationUnit.CodeCompleteAt(file.Location, line, column, clangUnsavedFiles.ToArray(), CodeCompleteFlags.IncludeBriefComments | CodeCompleteFlags.IncludeMacros | CodeCompleteFlags.IncludeCodePatterns);
            completionResults.Sort();

            var result = new List<CodeCompletionData>();

            foreach (var codeCompletion in completionResults.Results)
            {
                if (codeCompletion.CompletionString.Availability == AvailabilityKind.Available)
                {
                    string typedText = string.Empty;

                    string hint = string.Empty;

                    foreach (var chunk in codeCompletion.CompletionString.Chunks)
                    {
                        if (chunk.Kind == CompletionChunkKind.TypedText)
                        {
                            typedText = chunk.Text;
                        }

                        hint += chunk.Text;

                        switch (chunk.Kind)
                        {
                            case CompletionChunkKind.LeftParen:
                            case CompletionChunkKind.LeftAngle:
                            case CompletionChunkKind.LeftBrace:
                            case CompletionChunkKind.LeftBracket:
                            case CompletionChunkKind.RightAngle:
                            case CompletionChunkKind.RightBrace:
                            case CompletionChunkKind.RightBracket:
                            case CompletionChunkKind.RightParen:
                            case CompletionChunkKind.Placeholder:
                            case CompletionChunkKind.Comma:
                                break;

                            default:
                                hint += " ";
                                break;
                        }
                    }

                    result.Add(new CodeCompletionData { Suggestion = typedText, Priority = codeCompletion.CompletionString.Priority, Kind = (AvalonStudio.Languages.CursorKind)codeCompletion.CursorKind, Hint = hint, BriefComment = codeCompletion.CompletionString.BriefComment });
                }
            }

            clangAccessSemaphore.Release();
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

            clangAccessSemaphore.WaitOne();
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

                    if (diagnostic.FixItCount > 0)
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

            clangAccessSemaphore.Release();
            dataAssociation.TextColorizer.SetTransformations(result.SyntaxHighlightingData);

            return result;
        }

        public bool CanHandle(ISourceFile file)
        {
            bool result = false;

            switch (Path.GetExtension(file.Location))
            {
                case ".h":
                case ".cpp":
                case ".hpp":
                case ".c":
                    result = true;
                    break;
            }

            if (result)
            {
                if (!(file.Project is IStandardProject))
                {
                    result = false;
                }
            }

            return result;
        }

        private void OpenBracket(TextEditor editor, TextDocument document, string text)
        {
            if (text[0].IsOpenBracketChar() && editor.CaretIndex < document.TextLength && editor.CaretIndex > 0)
            {
                char nextChar = document.GetCharAt(editor.CaretIndex);

                if (char.IsWhiteSpace(nextChar) || nextChar.IsCloseBracketChar())
                {
                    document.Insert(editor.CaretIndex, text[0].GetCloseBracketChar().ToString());
                }
            }
        }

        private void CloseBracket(TextEditor editor, TextDocument document, string text)
        {
            if (text[0].IsCloseBracketChar() && editor.CaretIndex < document.TextLength && editor.CaretIndex > 0)
            {
                if (document.GetCharAt(editor.CaretIndex) == text[0])
                {
                    document.Replace(editor.CaretIndex - 1, 1, string.Empty);
                }
            }
        }

        public void RegisterSourceFile(TextEditor editor, ISourceFile file, TextDocument doc)
        {
            CPlusPlusDataAssociation association = null;

            if (dataAssociations.TryGetValue(file, out association))
            {
                throw new Exception("Source file already registered with language service.");
            }
            else
            {
                association = new CPlusPlusDataAssociation(doc);
                dataAssociations.Add(file, association);
            }

            association.KeyUpHandler = (sender, e) =>
            {
                if (editor.TextDocument == doc)
                {
                    switch (e.Key)
                    {
                        case Key.Return:
                            {
                                if (editor.CaretIndex < editor.TextDocument.TextLength)
                                {
                                    if (editor.TextDocument.GetCharAt(editor.CaretIndex) == '}')
                                    {
                                        editor.TextDocument.Insert(editor.CaretIndex, Environment.NewLine);
                                        editor.CaretIndex--;

                                        var currentLine = editor.TextDocument.GetLineByOffset(editor.CaretIndex);

                                        editor.CaretIndex = IndentationStrategy.IndentLine(editor.TextDocument, currentLine, editor.CaretIndex);
                                        editor.CaretIndex = IndentationStrategy.IndentLine(editor.TextDocument, currentLine.NextLine.NextLine, editor.CaretIndex);
                                        editor.CaretIndex = IndentationStrategy.IndentLine(editor.TextDocument, currentLine.NextLine, editor.CaretIndex);
                                    }

                                    var newCaret = IndentationStrategy.IndentLine(editor.TextDocument, editor.TextDocument.GetLineByOffset(editor.CaretIndex), editor.CaretIndex);

                                    editor.CaretIndex = newCaret;
                                }
                            }
                            break;
                    }
                }
            };

            association.TextInputHandler = (sender, e) =>
            {
                if (editor.TextDocument == doc)
                {
                    OpenBracket(editor, editor.TextDocument, e.Text);
                    CloseBracket(editor, editor.TextDocument, e.Text);

                    switch (e.Text)
                    {
                        case "}":
                        case ";":
                            editor.CaretIndex = Format(file, editor.TextDocument, 0, (uint)editor.TextDocument.TextLength, editor.CaretIndex);
                            break;

                        case "{":
                            editor.CaretIndex = Format(file, editor.TextDocument, 0, (uint)editor.TextDocument.TextLength, editor.CaretIndex) - Environment.NewLine.Length;
                            break;
                    }
                }
            };

            editor.KeyUp += association.KeyUpHandler;
            editor.TextInput += association.TextInputHandler;
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

        public void UnregisterSourceFile(TextEditor editor, ISourceFile file)
        {
            var association = GetAssociatedData(file);

            editor.KeyUp -= association.KeyUpHandler;
            editor.TextInput -= association.TextInputHandler;

            dataAssociations.Remove(file);
        }

        public int Format(ISourceFile file, TextDocument textDocument, uint offset, uint length, int cursor)
        {
            var replacements = ClangFormat.FormatXml(textDocument.Text, offset, length, (uint)cursor, ClangFormatSettings.Default);

            return ApplyReplacements(textDocument, cursor, replacements);
        }

        public static int ApplyReplacements(TextDocument document, int cursor, XDocument replacements)
        {
            var elements = replacements.Elements().First().Elements();

            document.BeginUpdate();

            int offsetChange = 0;
            foreach (var element in elements)
            {
                switch (element.Name.LocalName)
                {
                    case "cursor":
                        cursor = Convert.ToInt32(element.Value);
                        break;

                    case "replacement":
                        int offset = -1;
                        int replacementLength = -1;
                        var attributes = element.Attributes();

                        foreach (var attribute in attributes)
                        {
                            switch (attribute.Name.LocalName)
                            {
                                case "offset":
                                    offset = Convert.ToInt32(attribute.Value);
                                    break;

                                case "length":
                                    replacementLength = Convert.ToInt32(attribute.Value);
                                    break;
                            }
                        }

                        if (offset >= document.TextLength)
                        {
                            //document.Insert(offset, element.Value);
                        }
                        if (offset + replacementLength > document.TextLength)
                        {
                            //document.Replace(offset, document.TextLength - offset, element.Value);
                        }
                        else
                        {
                            document.Replace(offsetChange + offset, replacementLength, element.Value);
                        }

                        offsetChange += element.Value.Length - replacementLength;
                        break;
                }

            }

            document.EndUpdate();

            return cursor;
        }

        public Symbol GetSymbol(ISourceFile file, List<UnsavedFile> unsavedFiles, int offset)
        {
            Symbol result = new Symbol();
            var associatedData = GetAssociatedData(file);

            clangAccessSemaphore.WaitOne();
            var tu = associatedData.TranslationUnit;
            var cursor = tu.GetCursor(tu.GetLocationForOffset(tu.GetFile(file.File), offset));

            switch (cursor.Kind)
            {
                case CursorKind.MemberReferenceExpression:
                case CursorKind.DeclarationReferenceExpression:
                case CursorKind.CallExpression:
                case CursorKind.TypeReference:
                    cursor = cursor.Referenced;
                    break;
            }

            result.Name = cursor.Spelling;
            result.Kind = (AvalonStudio.Languages.CursorKind)cursor.Kind;
            result.BriefComment = cursor.BriefCommentText;
            result.TypeDescription = cursor.CursorType?.Spelling;
            result.EnumDescription = cursor.EnumConstantDeclValue.ToString();
            result.Definition = cursor.Definition.DisplayName;
            result.Linkage = (AvalonStudio.Languages.LinkageKind)cursor.Linkage;
            result.IsBuiltInType = IsBuiltInType(cursor.CursorType);
            result.SymbolType = cursor.CursorType?.Spelling.Replace(" &", "&").Replace(" *", "*") + " ";
            result.ResultType = cursor.ResultType?.Spelling;
            result.Arguments = new List<ParameterSymbol>();

            switch (result.Kind)
            {
                case Languages.CursorKind.FunctionDeclaration:
                case Languages.CursorKind.CXXMethod:
                case Languages.CursorKind.Constructor:
                case Languages.CursorKind.Destructor:
                    for (int i = 0; i < cursor.ArgumentCount; i++)
                    {
                        var argument = cursor.GetArgument(i);

                        var arg = new ParameterSymbol();
                        arg.IsBuiltInType = IsBuiltInType(argument.CursorType);
                        arg.Name = argument.Spelling;

                        if (i < cursor.ArgumentCount - 1)
                        {
                            arg.Name += ", ";
                        }

                        arg.Comment = argument.BriefCommentText;
                        arg.TypeDescription = argument.CursorType.Spelling;
                        result.Arguments.Add(arg);
                    }

                    //if (cursor.ParsedComment.FullCommentAsXml != null)
                    //{
                    //    var documentation = XDocument.Parse(cursor.ParsedComment.FullCommentAsXml);

                    //    var function = documentation.Element("Function");

                    //    var parameters = documentation.Element("Parameters");
                    //    //documentation.Elements().Where((e) => e.Name == );
                    //}

                    if (result.Arguments.Count == 0)
                    {
                        result.Arguments.Add(new ParameterSymbol() { Name = "void" });
                    }
                    break;
            }

            clangAccessSemaphore.Release();

            return result;
        }

        private bool IsBuiltInType(ClangType cursor)
        {
            bool result = false;

            if (cursor != null && cursor.Kind >= TypeKind.FirstBuiltin && cursor.Kind <= TypeKind.LastBuiltin)
            {
                return true;
            }

            return result;
        }

        public Symbol GetSymbol(ISourceFile file, List<UnsavedFile> unsavedFiles, string name)
        {
            throw new NotImplementedException();
        }
    }

    class CPlusPlusDataAssociation
    {
        public CPlusPlusDataAssociation(TextDocument textDocument)
        {
            BackgroundRenderers = new List<IBackgroundRenderer>();
            DocumentLineTransformers = new List<IDocumentLineTransformer>();

            TextColorizer = new TextColoringTransformer(textDocument);
            TextMarkerService = new TextMarkerService(textDocument);

            BackgroundRenderers.Add(new BracketMatchingBackgroundRenderer());

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
        public EventHandler<KeyEventArgs> KeyUpHandler { get; set; }
        public EventHandler<TextInputEventArgs> TextInputHandler { get; set; }
    }
}
