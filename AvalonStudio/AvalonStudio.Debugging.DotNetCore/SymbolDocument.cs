namespace AvalonStudio.Debugging.DotNetCore
{
    using System;
    using System.Diagnostics.SymbolStore;

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
}
