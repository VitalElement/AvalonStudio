namespace AvalonStudio.Languages
{
    using System;

    /// <summary>
    /// Bits that represent the context under which completion is occurring.
    ///
    /// The enumerators in this enumeration may be bitwise-OR'd together if multiple
    /// contexts are occurring simultaneously.
    /// </summary>
    [Flags]
    public enum CompletionContext
    {
        /// <summary>
        /// The context for completions is unexposed, as only Clang results
        /// should be included. (This is equivalent to having no context bits set.)
        /// </summary>
        Unexposed = 0,

        /// <summary>
        /// Completions for any possible type should be included in the results.
        /// </summary>
        AnyType = 1 << 0,

        /// <summary>
        /// Completions for any possible value (variables, function calls, etc.)
        /// should be included in the results.
        /// </summary>
        AnyValue = 1 << 1,

        /// <summary>
        /// Completions for values that resolve to an Objective-C object should
        /// be included in the results.
        /// </summary>
        ObjCObjectValue = 1 << 2,

        /// <summary>
        /// Completions for values that resolve to an Objective-C selector
        /// should be included in the results.
        /// </summary>
        ObjCSelectorValue = 1 << 3,

        /// <summary>
        /// Completions for values that resolve to a C++ class type should be
        /// included in the results.
        /// </summary>
        CXXClassTypeValue = 1 << 4,

        /// <summary>
        /// Completions for fields of the member being accessed using the dot
        /// operator should be included in the results.
        /// </summary>
        DotMemberAccess = 1 << 5,

        /// <summary>
        /// Completions for fields of the member being accessed using the arrow
        /// operator should be included in the results.
        /// </summary>
        ArrowMemberAccess = 1 << 6,

        /// <summary>
        /// Completions for properties of the Objective-C object being accessed
        /// using the dot operator should be included in the results.
        /// </summary>
        ObjCPropertyAccess = 1 << 7,

        /// <summary>
        /// Completions for enum tags should be included in the results.
        /// </summary>
        EnumTag = 1 << 8,

        /// <summary>
        /// Completions for union tags should be included in the results.
        /// </summary>
        UnionTag = 1 << 9,

        /// <summary>
        /// Completions for struct tags should be included in the results.
        /// </summary>
        StructTag = 1 << 10,

        /// <summary>
        /// Completions for C++ class names should be included in the results.
        /// </summary>
        ClassTag = 1 << 11,

        /// <summary>
        /// Completions for C++ namespaces and namespace aliases should be
        /// included in the results.
        /// </summary>
        Namespace = 1 << 12,

        /// <summary>
        /// Completions for C++ nested name specifiers should be included in
        /// the results.
        /// </summary>
        NestedNameSpecifier = 1 << 13,

        /// <summary>
        /// Completions for Objective-C interfaces (classes) should be included
        /// in the results.
        /// </summary>
        ObjCInterface = 1 << 14,

        /// <summary>
        /// Completions for Objective-C protocols should be included in
        /// the results.
        /// </summary>
        ObjCProtocol = 1 << 15,

        /// <summary>
        /// Completions for Objective-C categories should be included in
        /// the results.
        /// </summary>
        ObjCCategory = 1 << 16,

        /// <summary>
        /// Completions for Objective-C instance messages should be included
        /// in the results.
        /// </summary>
        ObjCInstanceMessage = 1 << 17,

        /// <summary>
        /// Completions for Objective-C class messages should be included in
        /// the results.
        /// </summary>
        ObjCClassMessage = 1 << 18,

        /// <summary>
        /// Completions for Objective-C selector names should be included in
        /// the results.
        /// </summary>
        ObjCSelectorName = 1 << 19,

        /// <summary>
        /// Completions for preprocessor macro names should be included in
        /// the results.
        /// </summary>
        MacroName = 1 << 20,

        /// <summary>
        /// Natural language completions should be included in the results.
        /// </summary>
        NaturalLanguage = 1 << 21,

        /// <summary>
        /// The current context is unknown, so set all contexts.
        /// </summary>
        Unknown = ((1 << 22) - 1)
    }
}
