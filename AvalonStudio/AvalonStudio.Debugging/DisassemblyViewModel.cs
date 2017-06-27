using Avalonia.Threading;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.MVVM;
using Mono.Debugging.Client;
using ReactiveUI;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;
using System.Collections.ObjectModel;
using AvalonStudio.TextEditor.Rendering;

namespace AvalonStudio.Debugging
{
    public class DisassemblyViewModel : ToolViewModel, IExtension
    {
        private IDebugger2 _debugger;
        private IDebugManager2 _debugManager;

        List<AssemblyLine> cachedLines = new List<AssemblyLine>();
        Dictionary<string, int> addressLines = new Dictionary<string, int>();
        bool autoRefill;
        int firstLine;
        int lastLine;
        TextDocument editor;

        SelectedDebugLineBackgroundRenderer _selectedLineMarker;

        string cachedLinesAddrSpace;

        string currentFile;

        private bool enabled;

        private ulong selectedIndex;

        public DisassemblyViewModel()
        {
            Dispatcher.UIThread.InvokeAsync(() => { IsVisible = false; });

            Title = "Disassembly";

            editor = new TextDocument();

            _backgroundRenderers = new ObservableCollection<IBackgroundRenderer>();
            _lineTransformers = new ObservableCollection<IVisualLineTransformer>();

            _selectedLineMarker = new SelectedDebugLineBackgroundRenderer();
            _backgroundRenderers.Add(_selectedLineMarker);

            _lineTransformers.Add(_selectedLineMarker);
        }

        private int _caretIndex;

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
            get { return editor; }
            set { this.RaiseAndSetIfChanged(ref editor, value); }
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

        public void Activation()
        {
            _debugManager = IoC.Get<IDebugManager2>();

            _debugManager.DebugSessionStarted += (sender, e) => { IsVisible = true; };

            _debugManager.DebugSessionEnded += (sender, e) =>
            {
                IsVisible = false;
            };

            _debugManager.TargetStopped += (sender, e) =>
            {
                
            };

            _debugManager.FrameChanged += (sender, e) =>
            {
                if (!_debugManager.Session.IsRunning)
                {
                    Update();
                }
            };
        }

        public void Update()
        {
            autoRefill = false;
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

            /*if (!string.IsNullOrEmpty(sf.SourceLocation.FileName) && File.Exists(sf.SourceLocation.FileName))
                FillWithSource();
            else*/
                Fill();
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
                    autoRefill = true;
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
            
            editor.Text = string.Empty;
            InsertLines(0, firstLine, lastLine, out firstLine, out lastLine);

            autoRefill = true;

            UpdateCurrentLineMarker(true);
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
            int editorLine = editor.GetLineByOffset(offset).LineNumber;

            foreach (AssemblyLine li in lines)
            {
                if (li.IsOutOfRange)
                    continue;
                InsertAssemblerLine(sb, editorLine++, li);
                lineCount++;
            }

            //editor.IsReadOnly = false;
            editor.Insert(offset, sb.ToString());
            //editor.IsReadOnly = true;

            if (offset == 0)
                this.cachedLines.InsertRange(0, lines);
            else
                this.cachedLines.AddRange(lines);
            return lineCount;
        }

        void InsertAssemblerLine(StringBuilder sb, int line, AssemblyLine asm)
        {
            sb.AppendFormat("{0:x8}   {1}\n", asm.Address, asm.Code);
            addressLines[GetAddrId(asm.Address, asm.AddressSpace)] = line;
        }

        string GetAddrId(long addr, string addrSpace)
        {
            return addrSpace + " " + addr;
        }

        void UpdateCurrentLineMarker(bool moveCaret)
        {
            /*if (currentDebugLineMarker != null)
            {
                editor.RemoveMarker(currentDebugLineMarker);
                currentDebugLineMarker = null;
            }*/

            StackFrame sf = _debugManager.SelectedFrame;
            int line;
            if (addressLines.TryGetValue(GetAddrId(sf.Address, sf.AddressSpace), out line))
            {
                var docLine = editor.GetLineByNumber(line);

                _selectedLineMarker.SetLocation(docLine.LineNumber);
                /*currentDebugLineMarker = TextMarkerFactory.CreateCurrentDebugLineTextMarker(editor, docLine.Offset, docLine.Length);
                editor.AddMarker(line, currentDebugLineMarker);*/
                if (moveCaret)
                {
                    CaretIndex = docLine.Offset;
                }
            }
        }


        public void BeforeActivation()
        {
        }

        public void SetAddress(ulong currentAddress)
        {
            //if (DisassemblyData == null)
            //{
            //    DisassemblyData = new AsyncVirtualizingCollection<InstructionLine>(dataProvider, 100, 6000);

            //    Task.Factory.StartNew(async () =>
            //    {
            //        await Task.Delay(50);

            //        Dispatcher.UIThread.InvokeAsync(() =>
            //        {
            //            SelectedIndex = currentAddress;
            //        });
            //    });
            //}
            //else
            //{
            //    SelectedIndex = currentAddress;
            //}
        }
    }
}