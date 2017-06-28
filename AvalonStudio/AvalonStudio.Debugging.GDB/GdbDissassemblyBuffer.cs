// GdbBacktrace.cs
//
// Authors: Lluis Sanchez Gual <lluis@novell.com>
//          Jeffrey Stedfast <jeff@xamarin.com>
//
// Copyright (c) 2008 Novell, Inc (http://www.novell.com)
// Copyright (c) 2012 Xamarin Inc. (http://www.xamarin.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using Mono.Debugging.Backend;
using Mono.Debugging.Client;
using System.Globalization;

namespace AvalonStudio.Debugging.GDB
{
    internal class GdbDissassemblyBuffer : DissassemblyBuffer
    {
        private GdbSession session;

        public GdbDissassemblyBuffer(GdbSession session, long addr) : base(addr)
        {
            this.session = session;
        }

        public override AssemblyLine[] GetLines(long startAddr, long endAddr)
        {
            try
            {
                int blankLines = 0;
                GdbCommandResult data = null;

                while (startAddr < endAddr)
                {
                    data = session.RunCommand("-data-disassemble", "-s", startAddr.ToString(), "-e", endAddr.ToString(), "--", "0");

                    if(data.Status == CommandStatus.Done)
                    {
                        break;
                    }

                    startAddr++;
                    blankLines++;
                }

                ResultData ins = data.GetObject("asm_insns");

                AssemblyLine[] alines = new AssemblyLine[ins.Count + blankLines];
                for (int n = 0; n < blankLines; n++)
                {
                    alines[n] = new AssemblyLine(startAddr + n, "Invalid Address");
                }

                for (int n = 0; n < ins.Count; n++)
                {
                    ResultData aline = ins.GetObject(n);
                    long addr = long.Parse(aline.GetValue("address").Substring(2), NumberStyles.HexNumber);
                    AssemblyLine line = new AssemblyLine(addr, aline.GetValue("inst"));
                    alines[n + blankLines] = line;
                }

                return alines;
            }
            catch
            {
                return new AssemblyLine[0];
            }
        }
    }
}