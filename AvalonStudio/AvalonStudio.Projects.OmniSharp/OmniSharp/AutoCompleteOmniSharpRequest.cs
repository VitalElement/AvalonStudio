using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Languages.CSharp.OmniSharp
{
    public class CompletionData
    {
        public string CompletionText { get; set; }
        public string Description { get; set; }
        public string DisplayText { get; set; }
        public string RequiredNamespaceImport { get; set; }
        public string MethodHeader { get; set; }
        public string ReturnType { get; set; }
        public string Snippet { get; set; }
        public string Kind { get; set; }
    }
    

    class AutoCompleteOmniSharpRequest : OmniSharpRequest<List<CompletionData>>
    {
        public AutoCompleteOmniSharpRequest()
        {
            WantDocumentationForEveryCompletionResult = true;
            WantImportableTypes = true;
            WantKind = true;
            WantMethodHeader = true;
            WantReturnType = true;
            WantSnippet = true;
        }
        /// <summary>

        ///   Specifies whether to return the code documentation for

        ///   each and every returned autocomplete result.

        /// </summary>

        public bool WantDocumentationForEveryCompletionResult { get; set; }



        /// <summary>

        ///   Specifies whether to return importable types. Defaults to

        ///   false. Can be turned off to get a small speed boost.

        /// </summary>

        public bool WantImportableTypes { get; set; }



        /// <summary>

        /// Returns a 'method header' for working with parameter templating.

        /// </summary>

        public bool WantMethodHeader { get; set; }



        /// <summary>

        /// Returns a snippet that can be used by common snippet libraries

        /// to provide parameter and type parameter placeholders

        /// </summary>

        public bool WantSnippet { get; set; }



        /// <summary>

        /// Returns the return type

        /// </summary>

        public bool WantReturnType { get; set; }



        /// <summary>

        /// Returns the kind (i.e Method, Property, Field)

        /// </summary>

        public bool WantKind { get; set; }

        public override string EndPoint
        {
            get
            {
                return "autocomplete";
            }
        }
    }
}
