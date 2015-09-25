namespace VEStudio.Models.Solutions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

   // public abstract class IndexEntry : IComparable<IndexEntry>
   // {
   //     public IndexEntry (int offset, int endOffset)
   //     {
   //         this.Offset = offset;
   //         this.EndOffset = endOffset;          
   //     }

   //     public int Line { get; set; }
   //     public int Column { get; set; }
   //     public int Length { get; set; }

   //     public int Offset { get; set; }   // These are to be converted to line / column numbers when rendering.
   //     public int EndOffset { get; set; }

   //     public int CompareTo (IndexEntry other)
   //     {            
   //         if(this.Offset == other.Offset)
   //         {
   //             return this.EndOffset.CompareTo(other.EndOffset);
   //         }

   //         return this.Offset.CompareTo (other.Offset);
   //     }
   // }

   // public class UserTypeLocationIndexEntry : IndexEntry
   // {        
   //     public UserTypeLocationIndexEntry(int offset, int endOffset, int line, int column, int length)
   //         : base (offset, endOffset)
   //     {
   //         this.Line = line;
   //         this.Column = column;
   //         this.Length = length;
   //     }        

   //     public static UserTypeLocationIndexEntry FromClangCursor(NClang.ClangCursor cursor)
   //     {
   //         string spelling = cursor.Spelling;

   //         if (cursor.Definition != null && cursor.Definition.Spelling != null)
   //         {
   //             spelling = cursor.Definition.Spelling;
   //         }

   //         return new UserTypeLocationIndexEntry (cursor.CursorExtent.Start.FileLocation.Offset, cursor.CursorExtent.End.FileLocation.Offset, cursor.Location.SpellingLocation.Line, cursor.Location.SpellingLocation.Column, spelling.Length);
   //     }
   // }

   // public class EnumConstantIndexEntry : UserTypeLocationIndexEntry
   // {
   //     public EnumConstantIndexEntry(int offset, int endOffset, int line, int column, int length) : base (offset, endOffset, line, column, length)
   //     {

   //     }

   //     new public static EnumConstantIndexEntry FromClangCursor (NClang.ClangCursor cursor)
   //     {
   //         string spelling = cursor.Spelling;

   //         if(cursor.Definition.Spelling != null)
   //         {
   //             spelling = cursor.Definition.Spelling;
   //         }

   //         return new EnumConstantIndexEntry (cursor.CursorExtent.Start.FileLocation.Offset, cursor.CursorExtent.End.FileLocation.Offset, cursor.Location.SpellingLocation.Line, cursor.Location.SpellingLocation.Column, spelling.Length);
   //     }
   // }

   // public class Cursor : IndexEntry, IComparable<Cursor>
   // {
   //     public Cursor (int offset, int endOffset, NClang.CursorKind kind, string name)
   //         : base (offset, endOffset)
   //     {
   //         this.Kind = kind;
   //         this.Name = name;
   //     }

   //     public static Cursor FromClangCursor (NClang.ClangCursor cursor)
   //     {
   //         var result = new Cursor (cursor.CursorExtent.Start.FileLocation.Offset,
   //             cursor.CursorExtent.End.FileLocation.Offset,
   //             cursor.Kind,
   //             cursor.Spelling);

   //         return result;
   //     }

   //     public NClang.CursorKind Kind { get; set; }
   //     public string Name { get; set; }

   //     public int CompareTo (Cursor other)
   //     {
   //         return this.Name.CompareTo (other.Name);
   //     }
   // }

   // public class Mark : IndexEntry
   // {
   //     public Mark(int offset, int endOffset, int line, int endLine, string title)
   //         : base (offset, endOffset)
   //     {
   //         this.Title = title;
   //         this.Line = line;
   //         this.EndLine = endLine;
   //     }        

   //     public string Title { get; set; }
   //     public int Line { get; set; }
   //     public int EndLine { get; set; }
   // }


   // public class TranslationUnitIndex
   // {
   //     public TranslationUnitIndex ()
   //     {
   //         this.Entries = new List<IndexEntry> ();
   //         MainFileTypes = new List<IndexEntry> ();
   //         HeaderTypes = new List<IndexEntry> ();
   //     }

   //     public void ParseFile (ProjectFile file)
   //     {
   //         if (file != null && file.IsCodeFile)
   //         {
   //             var callbacks = new NClang.ClangIndexerCallbacks ();

			//	callbacks.IndexDeclaration += (handle, e) =>
			//	{
   //                 if (e.Cursor.Spelling != null && e.Location.SourceLocation.IsFromMainFile)
   //                 {
   //                     switch (e.Cursor.Kind)
   //                     {
   //                         case CursorKind.FunctionDeclaration:
   //                         case CursorKind.CXXMethod:
   //                         case CursorKind.Constructor:
   //                         case CursorKind.Destructor:
   //                         case CursorKind.VarDeclaration:
   //                         case CursorKind.ParmDeclaration:
   //                         case CursorKind.StructDeclaration:
   //                         case CursorKind.ClassDeclaration:
   //                         case CursorKind.TypedefDeclaration:
   //                         case CursorKind.ClassTemplate:
   //                         case CursorKind.EnumDeclaration:
   //                         case CursorKind.UnionDeclaration:
   //                             Insert(Cursor.FromClangCursor(e.Cursor), Entries);
   //                             break;
   //                     }

   //                     switch (e.Cursor.Kind)
   //                     {
   //                         case CursorKind.StructDeclaration:
   //                         case CursorKind.ClassDeclaration:
   //                         case CursorKind.TypedefDeclaration:
   //                         case CursorKind.ClassTemplate:
   //                         case CursorKind.EnumDeclaration:
   //                         case CursorKind.UnionDeclaration:
   //                         case CursorKind.CXXBaseSpecifier:
   //                             Insert(UserTypeLocationIndexEntry.FromClangCursor(e.Cursor), MainFileTypes);
   //                             break;
   //                     }                        
   //                 }
			//	};

   //             callbacks.IndexEntityReference += (handle, e) =>
   //             {
   //                 if (e.Cursor.Spelling != null && e.Location.SourceLocation.IsFromMainFile)
   //                 {
   //                     switch (e.Cursor.Kind)
   //                     {
   //                         case CursorKind.TypeReference:
			//				case CursorKind.CXXBaseSpecifier:
			//				case CursorKind.TemplateReference:
			//					Insert (UserTypeLocationIndexEntry.FromClangCursor (e.Cursor), MainFileTypes);           
   //                             break;
   //                     }
   //                 }
   //             };

   //             if (file.TranslationUnit != null)
   //             {
   //                 var indexAction = file.Solution.NClangIndex.CreateIndexAction();

   //                 indexAction.IndexTranslationUnit(IntPtr.Zero, new NClang.ClangIndexerCallbacks[] { callbacks }, NClang.IndexOptionFlags.SkipParsedBodiesInSession, file.TranslationUnit);
   //                 indexAction.Dispose();
   //             }
			//}
   //     }        

   //     public void Insert (IndexEntry entry, List<IndexEntry> destination)
   //     {           
   //         var index = destination.BinarySearch (entry);   
                    
   //         if(index < 0)
   //         {
   //             index = Math.Abs (index) - 1;                
   //         }

   //         destination.Insert (index, entry);            
   //     }

   //     public IEnumerable<IndexEntry> UserTypes
   //     {
   //         get
   //         {
   //             if (HeaderTypes == null || MainFileTypes == null)
   //             {
   //                 return null;
   //             }

   //             return HeaderTypes.Concat (MainFileTypes);
   //         }
   //     }           

   //     public List<IndexEntry> Entries { get; private set; }

   //     public List<IndexEntry> HeaderTypes { get; private set; }

   //     public List<IndexEntry> MainFileTypes { get; private set; }
   // }  
}
