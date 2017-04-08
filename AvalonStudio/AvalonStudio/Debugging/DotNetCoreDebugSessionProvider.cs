using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using Mono.Debugging.Client;
using Mono.Debugging.Win32;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Windows.Input;

namespace AvalonStudio.Debugging
{
    public class PdbSymbolReaderFactory : ICustomCorSymbolReaderFactory
    {
        private bool IsPortablePdbFormat(string fileName)
        {
            using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                // Read first 4bytes and check if it matched portable pdb files.
                return (int)new BinaryReader(fileStream).ReadUInt32() == 1112167234;
            }
        }

        public ISymbolReader CreateCustomSymbolReader(string assemblyLocation)
        {
            if (!System.IO.File.Exists(assemblyLocation))
            {
                return null;
            }

            string pdbLocation = System.IO.Path.ChangeExtension(assemblyLocation, "pdb");

            if (!System.IO.File.Exists(pdbLocation))
            {
                return null;
            }

            if (!IsPortablePdbFormat(pdbLocation))
            {
                return null;
            }

            return new PdbSymbolReader(pdbLocation);
        }
    }

    public static class Extensions
    {
        public static bool IsWithin(this SequencePoint point, UInt32 line, UInt32 column)
        {
            if (point.StartLine == line)
            {
                if (0 < column && point.StartColumn > column)
                {
                    return false;
                }
            }

            if (point.EndLine == line)
            {
                if (point.EndColumn < column)
                {
                    return false;
                }
            }

            if (!((point.StartLine <= line) && (point.EndLine >= line)))
            {
                return false;
            }

            return true;
        }

        public static bool IsWithinLineOnly(this SequencePoint point, UInt32 line)
        {
            return ((point.StartLine <= line) && (line <= point.EndLine));
        }

        public static bool IsGreaterThan(this SequencePoint point, UInt32 line, UInt32 column)
        {
            return (point.StartLine > line) || (point.StartLine == line && point.StartColumn > column);
        }

        public static bool IsLessThan(this SequencePoint point, UInt32 line, UInt32 column)
        {
            return (point.StartLine < line) || (point.StartLine == line && point.StartColumn < column);
        }
    }

    public class SymbolDocument : ISymbolDocument, IComparable<ISymbolDocument>
    {
        private string _filePath;
        private Guid _documentType;
        private Guid _language;
        private Guid _checksumType;

        public SymbolDocument(string filePath, Guid language, Guid checksumType)
        {
            _filePath = filePath;
            _language = language;
            _checksumType = checksumType;
        }

        internal SymbolDocument()
        {

        }

        public string URL => _filePath;

        public Guid DocumentType => _documentType;

        public Guid Language => _language;

        public Guid LanguageVendor => throw new NotImplementedException();

        public Guid CheckSumAlgorithmId => _language;

        public bool HasEmbeddedSource => throw new NotImplementedException();

        public int SourceLength => throw new NotImplementedException();

        public int CompareTo(ISymbolDocument other)
        {
            return this.URL.CompareTo(other.URL);
        }

        public int FindClosestLine(int line)
        {
            return line;
        }

        public byte[] GetCheckSum()
        {
            throw new NotImplementedException();
        }

        public byte[] GetSourceRange(int startLine, int startColumn, int endLine, int endColumn)
        {
            throw new NotImplementedException();
        }
    }

    public class SymbolMethod : ISymbolMethod
    {
        private List<SequencePoint> _sequencePoints;
        private SymbolToken _token;

        public SymbolMethod(ISymbolDocument document, SequencePointCollection sequencePoints, SymbolToken token)
        {
            _sequencePoints = sequencePoints.ToList();

            _token = token;

            Document = document;
        }

        public List<SequencePoint> SequencePoints => _sequencePoints;

        public ISymbolDocument Document { get; set; }

        public SymbolToken Token => _token;

        public int SequencePointCount => _sequencePoints.Count();

        public ISymbolScope RootScope => throw new NotImplementedException();

        public ISymbolNamespace GetNamespace()
        {
            throw new NotImplementedException();
        }

        public int GetOffset(ISymbolDocument document, int line, int column)
        {
            throw new NotImplementedException();
        }

        public ISymbolVariable[] GetParameters()
        {
            throw new NotImplementedException();
        }

        public int[] GetRanges(ISymbolDocument document, int line, int column)
        {
            throw new NotImplementedException();
        }

        public ISymbolScope GetScope(int offset)
        {
            throw new NotImplementedException();
        }

        public void GetSequencePoints(int[] offsets, ISymbolDocument[] documents, int[] lines, int[] columns, int[] endLines, int[] endColumns)
        {
            int i = 0;

            foreach (var sequencePoint in _sequencePoints)
            {
                offsets[i] = sequencePoint.Offset;
                documents[i] = Document;
                lines[i] = sequencePoint.StartLine;
                columns[i] = sequencePoint.StartColumn;
                endLines[i] = sequencePoint.EndLine;
                endColumns[i] = sequencePoint.EndColumn;
                i++;
            }
        }

        public bool GetSourceStartEnd(ISymbolDocument[] docs, int[] lines, int[] columns)
        {
            throw new NotImplementedException();
        }
    }

    public class PdbSymbolReader : ISymbolReader
    {
        class SourceFileDebugInfo : ISymbolDocument
        {
            public string FullFilePath { get; set; }
            public int FileID { get; set; }
            public byte[] Hash { get; set; }
            public Guid HashAlgorithm { get; set; }

            public string URL => FullFilePath;

            public Guid DocumentType => throw new NotImplementedException();

            public Guid Language => throw new NotImplementedException();

            public Guid LanguageVendor => throw new NotImplementedException();

            public Guid CheckSumAlgorithmId => HashAlgorithm;

            public bool HasEmbeddedSource => throw new NotImplementedException();

            public int SourceLength => throw new NotImplementedException();

            public readonly List<SymbolMethod> PdbMethods;

            public SourceFileDebugInfo(List<SymbolMethod> methods)
            {
                PdbMethods = methods;
            }

            public byte[] GetCheckSum()
            {
                return Hash;
            }

            public int FindClosestLine(int line)
            {
                return line; // todo implement as per mono example.
            }

            public byte[] GetSourceRange(int startLine, int startColumn, int endLine, int endColumn)
            {
                throw new NotImplementedException();
            }
        }

        readonly Dictionary<string, Dictionary<string, List<SourceFileDebugInfo>>> sourceFilesDebugInfo = new Dictionary<string, Dictionary<string, List<SourceFileDebugInfo>>>();
        readonly Dictionary<int, SymbolMethod> methodTokenLookup = new Dictionary<int, SymbolMethod>();

        private Dictionary<string, ISymbolDocument> _documentLookup;

        private List<SymbolDocument> _documents;

        public int Token(MetadataReader reader, Func<Handle> handleFunc, bool displayTable = true)
        {
            Handle handle;

            handle = handleFunc();

            return Token(reader, handle, displayTable);
        }

        private int Token(MetadataReader reader, Handle handle, bool displayTable = true)
        {
            return reader.GetToken(handle);
        }

        bool LoadPdbFile(string pdbFileName)
        {
            if (!System.IO.File.Exists(pdbFileName))
                return false;

            var fileToSourceFileInfos = new Dictionary<string, List<SourceFileDebugInfo>>();
            sourceFilesDebugInfo[pdbFileName] = fileToSourceFileInfos;
            using (var fs = new FileStream(pdbFileName, FileMode.Open))
            using (var metadataReader = MetadataReaderProvider.FromPortablePdbStream(fs))
            {
                var pdb = metadataReader.GetMetadataReader();
                if (pdb.Documents.Count == 0)//If .pdb has 0 documents consider it invalid
                    return false;

                var methodMapping = new Dictionary<DocumentHandle, List<SymbolMethod>>(pdb.Documents.Count);

                foreach (var methodHandle in pdb.MethodDebugInformation)
                {
                    var method = pdb.GetMethodDebugInformation(methodHandle);

                    if (method.Document.IsNil)
                        continue;
                    List<SymbolMethod> list;
                    if (!methodMapping.TryGetValue(method.Document, out list))
                        methodMapping[method.Document] = list = new List<SymbolMethod>();

                    var methodToken = MetadataTokens.GetToken(methodHandle.ToDefinitionHandle());

                    var token = new SymbolToken(methodToken);

                    var newMethod = new SymbolMethod(new SymbolDocument(), method.GetSequencePoints(), token);
                    methodTokenLookup.Add(token.GetToken(), newMethod);
                    list.Add(newMethod);
                }

                foreach (var documentHandle in pdb.Documents)
                {
                    // A CompileUnit may not have methods, so guard against this.
                    List<SymbolMethod> list;
                    if (!methodMapping.TryGetValue(documentHandle, out list))
                        list = new List<SymbolMethod>();

                    SourceFileDebugInfo info = new SourceFileDebugInfo(list);

                    var document = pdb.GetDocument(documentHandle);
                    info.Hash = pdb.GetBlobBytes(document.Hash);
                    info.HashAlgorithm = pdb.GetGuid(document.HashAlgorithm);
                    info.FileID = 0;
                    info.FullFilePath = pdb.GetString(document.Name);

                    foreach (var method in list)
                    {
                        method.Document = info;
                    }

                    fileToSourceFileInfos[info.FullFilePath] = new List<SourceFileDebugInfo>();

                    if (!fileToSourceFileInfos.ContainsKey(System.IO.Path.GetFileName(info.FullFilePath)))
                        fileToSourceFileInfos[System.IO.Path.GetFileName(info.FullFilePath)] = new List<SourceFileDebugInfo>();

                    fileToSourceFileInfos[info.FullFilePath].Add(info);
                    fileToSourceFileInfos[System.IO.Path.GetFileName(info.FullFilePath)].Add(info);
                }
            }

            return true;
        }

        public PdbSymbolReader(string pdbLocation)
        {
            LoadPdbFile(pdbLocation);
        }

        public SymbolToken UserEntryPoint => throw new NotImplementedException();

        public ISymbolDocument GetDocument(string url, Guid language, Guid languageVendor, Guid documentType)
        {
            throw new NotImplementedException();
        }

        public ISymbolDocument[] GetDocuments()
        {
            return sourceFilesDebugInfo.Values.First().Values.Select(s => s).SelectMany(s => s).ToArray();
        }

        public ISymbolVariable[] GetGlobalVariables()
        {
            throw new NotImplementedException();
        }

        public ISymbolMethod GetMethod(SymbolToken method)
        {
            SymbolMethod result = null;

            methodTokenLookup.TryGetValue(method.GetToken(), out result);

            return result;
        }

        public ISymbolMethod GetMethod(SymbolToken method, int version)
        {
            throw new NotImplementedException();
        }

        public ISymbolMethod GetMethodFromDocumentPosition(ISymbolDocument document, int line, int column)
        {
            // See c++ implementation here... https://github.com/dotnet/coreclr/blob/master/src/debug/ildbsymlib/symread.cpp
            bool found = false;
            ISymbolMethod result = null;

            foreach (var pdbDocument in sourceFilesDebugInfo)
            {
                foreach (var sourceFiles in pdbDocument.Value)
                {
                    foreach (var sourceFile in sourceFiles.Value)
                    {
                        if (document.URL == sourceFile.URL)
                        {
                            foreach (var method in sourceFile.PdbMethods)
                            {
                                SequencePoint sequencePointBefore;
                                SequencePoint sequencePointAfter;

                                foreach (var point in method.SequencePoints)
                                {
                                    if (point.IsWithin((uint)line, (uint)column))
                                    {
                                        found = true;
                                        result = method;
                                        break;
                                    }
                                }

                                if (found)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        public ISymbolNamespace[] GetNamespaces()
        {
            throw new NotImplementedException();
        }

        public byte[] GetSymAttribute(SymbolToken parent, string name)
        {
            throw new NotImplementedException();
        }

        public ISymbolVariable[] GetVariables(SymbolToken parent)
        {
            throw new NotImplementedException();
        }
    }

    internal class StepOverCommandDefinition : CommandDefinition
    {
        public override KeyGesture Gesture => KeyGesture.Parse("F10");

        private readonly ReactiveCommand<object> command;

        public StepOverCommandDefinition()
        {
            command = ReactiveCommand.Create();
            command.Subscribe(_ =>
            {
                var manager = IoC.Get<IDebugManager2>();

                manager.SteoOver();
            });
        }

        public override Avalonia.Controls.Shapes.Path IconPath
        {
            get
            {
                return new Avalonia.Controls.Shapes.Path
                {
                    Fill = Brush.Parse("#FF7AC1FF"),
                    UseLayoutRounding = false,
                    Stretch = Stretch.Uniform,
                    Data =
                        StreamGeometry.Parse(
                            "M12,14A2,2 0 0,1 14,16A2,2 0 0,1 12,18A2,2 0 0,1 10,16A2,2 0 0,1 12,14M23.46,8.86L21.87,15.75L15,14.16L18.8,11.78C17.39,9.5 14.87,8 12,8C8.05,8 4.77,10.86 4.12,14.63L2.15,14.28C2.96,9.58 7.06,6 12,6C15.58,6 18.73,7.89 20.5,10.72L23.46,8.86Z")
                };
            }
        }

        public override ICommand Command
        {
            get { return command; }
        }

        public override string Text
        {
            get { return "Step Over"; }
        }

        public override string ToolTip
        {
            get { return "Steps over the current line."; }
        }
    }

    internal class StepIntoCommandDefinition : CommandDefinition
    {
        public override KeyGesture Gesture => KeyGesture.Parse("F11");

        private readonly ReactiveCommand<object> command;

        public StepIntoCommandDefinition()
        {
            command = ReactiveCommand.Create();
            command.Subscribe(_ =>
            {
                var manager = IoC.Get<IDebugManager2>();

                manager.StepInto();
            });
        }

        public override Avalonia.Controls.Shapes.Path IconPath
        {
            get
            {
                return new Avalonia.Controls.Shapes.Path
                {
                    Fill = Brush.Parse("#FF7AC1FF"),
                    UseLayoutRounding = false,
                    Stretch = Stretch.Uniform,
                    Data =
                        StreamGeometry.Parse(
                            "M12,22A2,2 0 0,1 10,20A2,2 0 0,1 12,18A2,2 0 0,1 14,20A2,2 0 0,1 12,22M13,2V13L17.5,8.5L18.92,9.92L12,16.84L5.08,9.92L6.5,8.5L11,13V2H13Z")
                };
            }
        }

        public override ICommand Command
        {
            get { return command; }
        }

        public override string Text
        {
            get { return "Step Into"; }
        }

        public override string ToolTip
        {
            get { return "Steps into the current line."; }
        }
    }

    internal class StartDebuggingCommandDefinition : CommandDefinition
    {
        private readonly ReactiveCommand<object> command;

        public StartDebuggingCommandDefinition()
        {
            command = ReactiveCommand.Create();
            command.Subscribe(_ =>
            {
                var manager = IoC.Get<IDebugManager2>();

                if (manager.CurrentDebugger == null)
                {
                    manager.CurrentDebugger = new DotNetCoreDebugger();
                    manager.Start();
                }
                else
                {
                    manager.Continue();
                }
            });
        }

        public override ICommand Command
        {
            get { return command; }
        }

        public override string Text
        {
            get { return "Start Debugging"; }
        }

        public override string ToolTip
        {
            get { return "Starts a debug session."; }
        }

        public override Avalonia.Controls.Shapes.Path IconPath
        {
            get
            {
                return new Avalonia.Controls.Shapes.Path
                {
                    Fill = Brush.Parse("#FF8DD28A"),
                    UseLayoutRounding = false,
                    Stretch = Stretch.Uniform,
                    Data = StreamGeometry.Parse("M8,5.14V19.14L19,12.14L8,5.14Z")
                };
            }
        }

        public override KeyGesture Gesture => KeyGesture.Parse("F5");
    }

    public interface IDebugger2 : IExtension
    {
        DebuggerSession CreateSession();

        DebuggerStartInfo GetDebuggerStartInfo(IProject project);

        DebuggerSessionOptions GetDebuggerSessionOptions(IProject project);
    }    

    public class DotNetCoreDebugger : IDebugger2
    {
        public void Activation()
        {
            
        }

        public void BeforeActivation()
        {
            
        }

        public DebuggerSession CreateSession()
        {
            var result = new CoreClrDebuggerSession(System.IO.Path.GetInvalidPathChars(), "C:\\Program Files\\dotnet\\shared\\Microsoft.NETCore.App\\1.1.1\\dbgshim.dll");

            result.CustomSymbolReaderFactory = new PdbSymbolReaderFactory();

            return result;
        }

        public DebuggerSessionOptions GetDebuggerSessionOptions(IProject project)
        {
            var evaluationOptions = EvaluationOptions.DefaultOptions.Clone();

            evaluationOptions.EllipsizeStrings = false;
            evaluationOptions.GroupPrivateMembers = false;
            evaluationOptions.EvaluationTimeout = 1000;

            return new DebuggerSessionOptions() { EvaluationOptions = evaluationOptions };
        }

        public DebuggerStartInfo GetDebuggerStartInfo(IProject project)
        {
            var startInfo = new DebuggerStartInfo()
            {
                Command = "dotnet.exe",
                Arguments = project.Executable,
                WorkingDirectory = System.IO.Path.GetDirectoryName(project.Executable),
                UseExternalConsole = true,
                CloseExternalConsoleOnExit = true
            };

            return startInfo;
        }
    }
}
