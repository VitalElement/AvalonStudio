using Avalonia.Threading;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.MVVM;
using AvalonStudio.TextEditor.Rendering;
using Mono.Debugging.Client;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive.Linq;
using System.Text;

namespace AvalonStudio.Debugging
{
    public class DisassemblyViewModel : ToolViewModel, IExtension
    {
        private IDebugManager2 _debugManager;

        List<AssemblyLine> cachedLines = new List<AssemblyLine>();
        Dictionary<string, int> addressLines = new Dictionary<string, int>();
        DebuggerSession session;
        private int _caretIndex;
        private bool _mixedMode;
        int firstLine;
        int lastLine;
        TextDocument document;
        TextDocument runModeDocument;
        TextDocument visibleDocument;

        SelectedDebugLineBackgroundRenderer _selectedLineMarker;

        string cachedLinesAddrSpace;

        string currentFile;

        private bool enabled;

        private ulong selectedIndex;

        public DisassemblyViewModel()
        {
            Dispatcher.UIThread.InvokeAsync(() => { IsVisible = false; });

            Title = "Disassembly";

            document = new TextDocument();
            runModeDocument = new TextDocument
            {
                Text = "Disassembly cannot be displayed in run mode."
            };

            _backgroundRenderers = new ObservableCollection<IBackgroundRenderer>();
            _lineTransformers = new ObservableCollection<IVisualLineTransformer>();

            _selectedLineMarker = new SelectedDebugLineBackgroundRenderer();
            _backgroundRenderers.Add(_selectedLineMarker);

            _lineTransformers.Add(new DisassemblyViewTextColorizer(addressLines));
            _lineTransformers.Add(_selectedLineMarker);

            _mixedMode = true;
        }

        public bool MixedMode
        {
            get
            {
                return _mixedMode;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _mixedMode, value);
                Update();
            }
        }

        public int CaretIndex
        {
            get { return _caretIndex; }
            set { this.RaiseAndSetIfChanged(ref _caretIndex, value); }
        }

        private ObservableCollection<IBackgroundRenderer> _backgroundRenderers;

        public ObservableCollection<IBackgroundRenderer> BackgroundRenderers
        {
            get { return _backgroundRenderers; }
            set { this.RaiseAndSetIfChanged(ref _backgroundRenderers, value); }
        }

        private ObservableCollection<IVisualLineTransformer> _lineTransformers;

        public ObservableCollection<IVisualLineTransformer> LineTransformers
        {
            get { return _lineTransformers; }
            set { this.RaiseAndSetIfChanged(ref _lineTransformers, value); }
        }

        public TextDocument Document
        {
            get { return visibleDocument; }
            set { this.RaiseAndSetIfChanged(ref visibleDocument, value); }
        }

        public bool Enabled
        {
            get { return enabled; }
            set { this.RaiseAndSetIfChanged(ref enabled, value); }
        }

        public ulong SelectedIndex
        {
            get { return selectedIndex; }
            set { this.RaiseAndSetIfChanged(ref selectedIndex, value); }
        }

        public override Location DefaultLocation
        {
            get { return Location.RightTop; }
        }

        public void BeforeActivation()
        {
        }

        public void Activation()
        {
            _debugManager = IoC.Get<IDebugManager2>();

            _debugManager.DebugSessionStarted += (sender, e) => { Document = runModeDocument; IsVisible = true; };

            _debugManager.DebugSessionEnded += (sender, e) =>
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    IsVisible = false;

                    Clear();
                });
            };

            _debugManager.FrameChanged += (sender, e) =>
            {
                Document = document;
                Update();
            };

            var started = Observable.FromEventPattern(_debugManager, nameof(_debugManager.TargetStarted));
            var stopped = Observable.FromEventPattern(_debugManager, nameof(_debugManager.TargetStopped));

            started.SelectMany(_ => Observable.Amb(Observable.Timer(TimeSpan.FromMilliseconds(250)).Select(o => true), stopped.Take(1).Select(o => false))).Where(timeout => timeout == true).Subscribe(s => 
            {
                Document = runModeDocument;
            });
        }

        public void Update()
        {
            if (_debugManager.Session != null && !_debugManager.Session.IsRunning)
            {
                /*if (currentDebugLineMarker != null)
                {
                    editor.RemoveMarker(currentDebugLineMarker);
                    currentDebugLineMarker = null;
                }*/

                if (_debugManager.SelectedFrame == null)
                {
                    /*if (messageOverlayContent != null)
                    {
                        editor.RemoveOverlay(messageOverlayContent);
                        messageOverlayContent = null;
                    }
                    sw.Sensitive = false;*/
                    return;
                }

                var sf = _debugManager.SelectedFrame;
                /*if (!string.IsNullOrWhiteSpace(sf.SourceLocation.FileName) && sf.SourceLocation.Line != -1 && sf.SourceLocation.FileHash != null)
                {
                    ShowLoadSourceFile(sf);
                }
                else
                {
                    if (messageOverlayContent != null)
                    {
                        editor.RemoveOverlay(messageOverlayContent);
                        messageOverlayContent = null;
                    }
                }*/

                if (sf.SourceLocation.Line >= 0 && !string.IsNullOrEmpty(sf.SourceLocation.FileName) && File.Exists(sf.SourceLocation.FileName) && MixedMode)
                {
                    FillWithSource();
                }
                else
                {
                    Fill();
                }
            }
        }

        public void Clear()
        {
            cachedLines.Clear();
            addressLines.Clear();

            firstLine = -150;
            lastLine = 150;

            Document = null;
        }

        public void FillWithSource()
        {
            cachedLines.Clear();

            StackFrame sf = _debugManager.SelectedFrame;
            session = sf.DebuggerSession;
            if (currentFile != sf.SourceLocation.FileName)
            {
                AssemblyLine[] asmLines = sf.DebuggerSession.DisassembleFile(sf.SourceLocation.FileName);
                if (asmLines == null)
                {
                    // Mixed disassemble not supported
                    Fill();
                    return;
                }
                currentFile = sf.SourceLocation.FileName;
                addressLines.Clear();
                document.Text = string.Empty;
                using (var sr = new StreamReader(sf.SourceLocation.FileName))
                {
                    string line;
                    int sourceLine = 1;
                    int na = 0;
                    int editorLine = 1;
                    var sb = new StringBuilder();
                    var asmLineNums = new List<int>();
                    while ((line = sr.ReadLine()) != null)
                    {
                        InsertSourceLine(sb, editorLine++, line);
                        while (na < asmLines.Length && asmLines[na].SourceLine == sourceLine)
                        {
                            asmLineNums.Add(editorLine);
                            InsertAssemblerLine(sb, editorLine++, asmLines[na++]);
                        }
                        sourceLine++;
                    }
                    document.Text = sb.ToString();
                    /*foreach (int li in asmLineNums)
                        editor.AddMarker(li, asmMarker);*/
                }
            }
           /* int aline;
            if (!addressLines.TryGetValue(GetAddrId(sf.Address, sf.AddressSpace), out aline))
                return;*/
            UpdateCurrentLineMarker(true);
        }

        public void Fill()
        {
            currentFile = null;
            StackFrame sf = _debugManager.SelectedFrame;
            if (cachedLines.Count > 0 && cachedLinesAddrSpace == sf.AddressSpace)
            {
                if (sf.Address >= cachedLines[0].Address && sf.Address <= cachedLines[cachedLines.Count - 1].Address)
                {
                    // The same address range can be reused
                    UpdateCurrentLineMarker(true);
                    return;
                }
            }

            // New address view
            cachedLinesAddrSpace = sf.AddressSpace;
            cachedLines.Clear();
            addressLines.Clear();

            firstLine = -150;
            lastLine = 150;

            document.Text = string.Empty;
            InsertLines(0, firstLine, lastLine, out firstLine, out lastLine);

            UpdateCurrentLineMarker(true);
        }

        void InsertSourceLine(StringBuilder sb, int line, string text)
        {
            sb.Append(text).Append('\n');
        }

        int InsertLines(int offset, int start, int end, out int newStart, out int newEnd)
        {
            StringBuilder sb = new StringBuilder();
            StackFrame ff = _debugManager.SelectedFrame;
            List<AssemblyLine> lines = new List<AssemblyLine>(ff.Disassemble(start, end - start + 1));

            int i = lines.FindIndex(al => !al.IsOutOfRange);
            if (i == -1)
            {
                newStart = int.MinValue;
                newEnd = int.MinValue;
                return 0;
            }

            newStart = i == 0 ? start : int.MinValue;
            lines.RemoveRange(0, i);

            int j = lines.FindLastIndex(al => !al.IsOutOfRange);
            newEnd = j == lines.Count - 1 ? end : int.MinValue;
            lines.RemoveRange(j + 1, lines.Count - j - 1);

            int lineCount = 0;
            int editorLine = document.GetLineByOffset(offset).LineNumber;

            foreach (AssemblyLine li in lines)
            {
                if (li.IsOutOfRange)
                    continue;
                InsertAssemblerLine(sb, editorLine++, li);
                lineCount++;
            }

            document.Insert(offset, sb.ToString());

            if (offset == 0)
                this.cachedLines.InsertRange(0, lines);
            else
                this.cachedLines.AddRange(lines);
            return lineCount;
        }

        void InsertAssemblerLine(StringBuilder sb, int line, AssemblyLine asm)
        {
            if (asm.Code.Contains("\\t"))
            {
                var opcodeParts = asm.Code.Split("\\t");
                sb.AppendFormat("{0:x8}   {1}", asm.Address, opcodeParts[0]);

                if (opcodeParts.Length > 1)
                {
                    var extraSpaces = 8 - opcodeParts[0].Length;

                    sb.Append(' ', 4 + extraSpaces);

                    sb.Append(opcodeParts[1]);

                    sb.Append("\n");
                }
            }
            else
            {
                sb.AppendFormat("{0:x8}   {1}\n", asm.Address, asm.Code);
            }

            addressLines[GetAddrId(asm.Address, asm.AddressSpace)] = line;
        }

        string GetAddrId(long addr, string addrSpace)
        {
            return addrSpace + " " + addr;
        }

        void UpdateCurrentLineMarker(bool moveCaret)
        {
            StackFrame sf = _debugManager.SelectedFrame;
            int line;
            if (addressLines.TryGetValue(GetAddrId(sf.Address, sf.AddressSpace), out line))
            {
                var docLine = document.GetLineByNumber(line);

                _selectedLineMarker.SetLocation(docLine.LineNumber);

                if (moveCaret)
                {
                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        CaretIndex = docLine.Offset;
                    });
                }
            }
        }
    }
}