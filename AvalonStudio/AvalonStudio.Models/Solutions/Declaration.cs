//using NClang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Models.Solutions
{
    public class Declaration
    {
        //private CursorKind declarationType;
        //public CursorKind Type
        //{
        //    get { return declarationType; }
        //    set { declarationType = value; }
        //}

        private string spelling;
        public string Spelling
        {
            get { return spelling; }
            set { spelling = value; }
        }

        public int StartOffset { get; set; }

        public int EndOffset { get; set; }

        private int line;
        public int Line
        {
            get { return line; }
            set { line = value; }
        }

        

        private int column;
        public int Column
        {
            get { return column; }
            set { column = value; }
        }
    }
}
