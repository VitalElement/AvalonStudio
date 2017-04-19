namespace AvalonStudio.Languages
{
    /// <summary>
    ///     Describes the kind of entity that a cursor refers to.
    /// </summary>
    public enum CursorKind
    {
        /// <summary>
        ///     A declaration whose specific kind is not exposed via this
        ///     interface.
        ///     Unexposed declarations have the same operations as any other kind
        ///     of declaration; one can extract their location information,
        ///     spelling, find their definitions, etc. However, the specific kind
        ///     of the declaration is not reported.
        /// </summary>
        UnexposedDeclaration = 1,

        /// <summary>
        ///     A C or C++ struct.
        /// </summary>
        StructDeclaration = 2,

        /// <summary>
        ///     A C or C++ union.
        /// </summary>
        UnionDeclaration = 3,

        /// <summary>
        ///     A C++ class.
        /// </summary>
        ClassDeclaration = 4,

        /// <summary>
        ///     An enumeration.
        /// </summary>
        EnumDeclaration = 5,

        /// <summary>
        ///     A field (in C) or non-static data member (in C++) in a
        ///     struct, union, or C++ class.
        /// </summary>
        FieldDeclaration = 6,

        /// <summary>
        ///     An enumerator constant.
        /// </summary>
        EnumConstantDeclaration = 7,

        /// <summary>
        ///     A function.
        /// </summary>
        FunctionDeclaration = 8,

        /// <summary>
        ///     A variable.
        /// </summary>
        VarDeclaration = 9,

        /// <summary>
        ///     A function or method parameter.
        /// </summary>
        ParmDeclaration = 10,

        /// <summary>
        ///     An Objective-C @interface.
        /// </summary>
        ObjCInterfaceDeclaration = 11,

        /// <summary>
        ///     An Objective-C @interface for a category.
        /// </summary>
        ObjCCategoryDeclaration = 12,

        /// <summary>
        ///     An Objective-C @protocol declaration.
        /// </summary>
        ObjCProtocolDeclaration = 13,

        /// <summary>
        ///     An Objective-C @property declaration.
        /// </summary>
        ObjCPropertyDeclaration = 14,

        /// <summary>
        ///     An Objective-C instance variable.
        /// </summary>
        ObjCIvarDeclaration = 15,

        /// <summary>
        ///     An Objective-C instance method.
        /// </summary>
        ObjCInstanceMethodDeclaration = 16,

        /// <summary>
        ///     An Objective-C class method.
        /// </summary>
        ObjCClassMethodDeclaration = 17,

        /// <summary>
        ///     An Objective-C @implementation.
        /// </summary>
        ObjCImplementationDeclaration = 18,

        /// <summary>
        ///     An Objective-C @implementation for a category.
        /// </summary>
        ObjCCategoryImplDeclaration = 19,

        /// <summary>
        ///     A typedef.
        /// </summary>
        TypedefDeclaration = 20,

        /// <summary>
        ///     A C++ class method.
        /// </summary>
        CXXMethod = 21,

        /// <summary>
        ///     A C++ namespace.
        /// </summary>
        Namespace = 22,

        /// <summary>
        ///     A linkage specification, e.g. 'extern "C"'.
        /// </summary>
        LinkageSpec = 23,

        /// <summary>
        ///     A C++ constructor.
        /// </summary>
        Constructor = 24,

        /// <summary>
        ///     A C++ destructor.
        /// </summary>
        Destructor = 25,

        /// <summary>
        ///     A C++ conversion function.
        /// </summary>
        ConversionFunction = 26,

        /// <summary>
        ///     A C++ template type parameter.
        /// </summary>
        TemplateTypeParameter = 27,

        /// <summary>
        ///     A C++ non-type template parameter.
        /// </summary>
        NonTypeTemplateParameter = 28,

        /// <summary>
        ///     A C++ template template parameter.
        /// </summary>
        TemplateTemplateParameter = 29,

        /// <summary>
        ///     A C++ function template.
        /// </summary>
        FunctionTemplate = 30,

        /// <summary>
        ///     A C++ class template.
        /// </summary>
        ClassTemplate = 31,

        /// <summary>
        ///     A C++ class template partial specialization.
        /// </summary>
        ClassTemplatePartialSpecialization = 32,

        /// <summary>
        ///     A C++ namespace alias declaration.
        /// </summary>
        NamespaceAlias = 33,

        /// <summary>
        ///     A C++ using directive.
        /// </summary>
        UsingDirective = 34,

        /// <summary>
        ///     A C++ using declaration.
        /// </summary>
        UsingDeclaration = 35,

        /// <summary>
        ///     A C++ alias declaration.
        /// </summary>
        TypeAliasDeclaration = 36,

        /// <summary>
        ///     An Objective-C @synthesize definition.
        /// </summary>
        ObjCSynthesizeDeclaration = 37,

        /// <summary>
        ///     An Objective-C @dynamic definition.
        /// </summary>
        ObjCDynamicDeclaration = 38,

        /// <summary>
        ///     An access specifier.
        /// </summary>
        CXXAccessSpecifier = 39,

        FirstDeclaration = UnexposedDeclaration,
        LastDeclaration = CXXAccessSpecifier,

        FirstReference = 40,

        /* Declaration references */

        ObjCSuperClassReference = 40,

        ObjCProtocolReference = 41,

        ObjCClassReference = 42,

        /// <summary>
        ///     A reference to a type declaration.
        ///     A type reference occurs anywhere where a type is named but not
        ///     declared. For example, given:
        ///     <code>
        /// typedef unsigned size_type;
        /// size_type size;
        /// </code>
        ///     The typedef is a declaration of size_type (CXCursor_TypedefDecl),
        ///     while the type of the variable "size" is referenced. The cursor
        ///     referenced by the type of size is the typedef for size_type.
        /// </summary>
        TypeReference = 43,

        CXXBaseSpecifier = 44,

        /// <summary>
        ///     A reference to a class template, function template, template
        ///     template parameter, or class template partial specialization.
        /// </summary>
        TemplateReference = 45,

        /// <summary>
        ///     A reference to a namespace or namespace alias.
        /// </summary>
        NamespaceReference = 46,

        /// <summary>
        ///     A reference to a member of a struct, union, or class that occurs in
        ///     some non-expression context, e.g., a designated initializer.
        /// </summary>
        MemberReference = 47,

        /// <summary>
        ///     A reference to a labeled statement.
        ///     This cursor kind is used to describe the jump to "start_over" in the
        ///     goto statement in the following example:
        ///     <code>
        /// start_over:
        ///     ++counter;
        ///
        ///     goto start_over;
        /// </code>
        ///     A label reference cursor refers to a label statement.
        /// </summary>
        LabelReference = 48,

        /// <summary>
        ///     A reference to a set of overloaded functions or function templates
        ///     that has not yet been resolved to a specific function or function template.
        ///     An overloaded declaration reference cursor occurs in C++ templates where
        ///     a dependent name refers to a function. For example:
        ///     <code>
        ///  template<typename T>
        ///             void swap(T&, T&);
        ///             struct X { ... };
        ///             void swap(X&, X&);
        ///             template
        ///             <typename T>
        ///                 void reverse(T* first, T* last) {
        ///                 while (first
        ///                 < last - 1) {
        ///                     swap(* first, *-- last);
        ///                     ++ first;
        ///                     }
        ///                     }
        ///                     struct Y { };
        ///                     void swap( Y&, Y&);
        ///  </code>
        ///     Here, the identifier "swap" is associated with an overloaded declaration
        ///     reference. In the template definition, "swap" refers to either of the two
        ///     "swap" functions declared above, so both results will be available. At
        ///     instantiation time, "swap" may also refer to other functions found via
        ///     argument-dependent lookup (e.g., the "swap" function at the end of the
        ///     example).
        ///     The functions <c>clang_getNumOverloadedDecls()</c> and
        ///     <c>clang_getOverloadedDecl()</c> can be used to retrieve the definitions
        ///     referenced by this cursor.
        /// </summary>
        OverloadedDeclarationReference = 49,

        /// <summary>
        ///     A reference to a variable that occurs in some non-expression
        ///     context, e.g., a C++ lambda capture list.
        /// </summary>
        VariableReference = 50,

        LastReference = VariableReference,

        /* Error conditions */

        FirstInvalid = 70,
        InvalidFile = 70,
        NoDeclarationFound = 71,
        NotImplemented = 72,
        InvalidCode = 73,
        LastInvalid = InvalidCode,

        /* Expressions */

        FirstExpression = 100,

        /// <summary>
        ///     An expression whose specific kind is not exposed via this
        ///     interface.
        ///     Unexposed expressions have the same operations as any other kind
        ///     of expression; one can extract their location information,
        ///     spelling, children, etc. However, the specific kind of the
        ///     expression is not reported.
        /// </summary>
        UnexposedExpression = 100,

        /// <summary>
        ///     An expression that refers to some value declaration, such
        ///     as a function, varible, or enumerator.
        /// </summary>
        DeclarationReferenceExpression = 101,

        /// <summary>
        ///     An expression that refers to a member of a struct, union,
        ///     class, Objective-C class, etc.
        /// </summary>
        MemberReferenceExpression = 102,

        /// <summary>
        ///     An expression that calls a function.
        /// </summary>
        CallExpression = 103,

        /// <summary>
        ///     An expression that sends a message to an Objective-C
        ///     object or class.
        /// </summary>
        ObjCMessageExpression = 104,

        /// <summary>
        ///     An expression that represents a block literal.
        /// </summary>
        BlockExpression = 105,

        /// <summary>
        ///     An integer literal.
        /// </summary>
        IntegerLiteral = 106,

        /// <summary>
        ///     A floating point number literal.
        /// </summary>
        FloatingLiteral = 107,

        /// <summary>
        ///     An imaginary number literal.
        /// </summary>
        ImaginaryLiteral = 108,

        /// <summary>
        ///     A string literal.
        /// </summary>
        StringLiteral = 109,

        /// <summary>
        ///     A character literal.
        /// </summary>
        CharacterLiteral = 110,

        /// <summary>
        ///     A parenthesized expression, e.g. "(1)".
        ///     This AST node is only formed if full location information is requested.
        /// </summary>
        ParenExpression = 111,

        /// <summary>
        ///     This represents the unary-expression's (except sizeof and
        ///     alignof).
        /// </summary>
        UnaryOperator = 112,

        /// <summary>
        ///     [C99 6.5.2.1] Array Subscripting.
        /// </summary>
        ArraySubscriptExpression = 113,

        /// <summary>
        ///     A builtin binary operation expression such as "x + y" or
        ///     "x &lt;= y".
        /// </summary>
        BinaryOperator = 114,

        /// <summary>
        ///     Compound assignment such as "+=".
        /// </summary>
        CompoundAssignOperator = 115,

        /// <summary>
        ///     The ?: ternary operator.
        /// </summary>
        ConditionalOperator = 116,

        /// <summary>
        ///     An explicit cast in C (C99 6.5.4) or a C-style cast in C++
        ///     (C++ [expr.cast]), which uses the syntax (Type)expr.
        ///     For example: (int)f.
        /// </summary>
        CStyleCastExpression = 117,

        /// <summary>
        ///     [C99 6.5.2.5]
        /// </summary>
        CompoundLiteralExpression = 118,

        /// <summary>
        ///     Describes an C or C++ initializer list.
        /// </summary>
        InitListExpression = 119,

        /// <summary>
        ///     The GNU address of label extension, representing &&label.
        /// </summary>
        AddrLabelExpression = 120,

        /// <summary>
        ///     This is the GNU Statement Expression extension: ({int X=4; X;})
        /// </summary>
        StatementExpression = 121,

        /// <summary>
        ///     Represents a C11 generic selection.
        /// </summary>
        GenericSelectionExpression = 122,

        /// <summary>
        ///     Implements the GNU __null extension, which is a name for a null
        ///     pointer constant that has integral type (e.g., int or long) and is the same
        ///     size and alignment as a pointer.
        ///     The __null extension is typically only used by system headers, which define
        ///     NULL as __null in C++ rather than using 0 (which is an integer that may not
        ///     match the size of a pointer).
        /// </summary>
        GNUNullExpression = 123,

        /// <summary>
        ///     C++'s static_cast&lt;&gt; expression.
        /// </summary>
        CXXStaticCastExpression = 124,

        /// <summary>
        ///     C++'s dynamic_cast&lt;&gt; expression.
        /// </summary>
        CXXDynamicCastExpression = 125,

        /// <summary>
        ///     C++'s reinterpret_cast&lt;&gt; expression.
        /// </summary>
        CXXReinterpretCastExpression = 126,

        /// <summary>
        ///     C++'s const_cast&lt;&gt; expression.
        /// </summary>
        CXXConstCastExpression = 127,

        /// <summary>
        ///     Represents an explicit C++ type conversion that uses "functional"
        ///     notion (C++ [expr.type.conv]).
        ///     Example:
        ///     <code>
        ///    x = int(0.5);
        ///  </code>
        /// </summary>
        CXXFunctionalCastExpression = 128,

        /// <summary>
        ///     A C++ typeid expression (C++ [expr.typeid]).
        /// </summary>
        CXXTypeidExpression = 129,

        /// <summary>
        ///     [C++ 2.13.5] C++ Boolean Literal.
        /// </summary>
        CXXBoolLiteralExpression = 130,

        /// <summary>
        ///     [C++0x 2.14.7] C++ Pointer Literal.
        /// </summary>
        CXXNullPtrLiteralExpression = 131,

        /// <summary>
        ///     Represents the "this" expression in C++
        /// </summary>
        CXXThisExpression = 132,

        /// <summary>
        ///     [C++ 15] C++ Throw Expression.
        ///     This handles 'throw' and 'throw' assignment-expression. When
        ///     assignment-expression isn't present, Op will be null.
        /// </summary>
        CXXThrowExpression = 133,

        /// <summary>
        ///     A new expression for memory allocation and constructor calls, e.g:
        ///     "new CXXNewExpr(foo)".
        /// </summary>
        CXXNewExpression = 134,

        /// <summary>
        ///     A delete expression for memory deallocation and destructor calls,
        ///     e.g. "delete[] pArray".
        /// </summary>
        CXXDeleteExpression = 135,

        /// <summary>
        ///     A unary expression.
        /// </summary>
        UnaryExpression = 136,

        /// <summary>
        ///     An Objective-C string literal i.e. @"foo".
        /// </summary>
        ObjCStringLiteral = 137,

        /// <summary>
        ///     An Objective-C @encode expression.
        /// </summary>
        ObjCEncodeExpression = 138,

        /// <summary>
        ///     An Objective-C @selector expression.
        /// </summary>
        ObjCSelectorExpression = 139,

        /// <summary>
        ///     An Objective-C @protocol expression.
        /// </summary>
        ObjCProtocolExpression = 140,

        /// <summary>
        ///     An Objective-C "bridged" cast expression, which casts between
        ///     Objective-C pointers and C pointers, transferring ownership in the process.
        ///     <code>
        ///    NSString *str = (__bridge_transfer NSString *)CFCreateString();
        ///  </code>
        /// </summary>
        ObjCBridgedCastExpression = 141,

        /// <summary>
        ///     Represents a C++0x pack expansion that produces a sequence of
        ///     expressions.
        ///     A pack expansion expression contains a pattern (which itself is an
        ///     expression) followed by an ellipsis. For example:
        ///     <code>
        ///  template<typename F, typename ... Types>
        ///             void forward(F f, Types &&...args) {
        ///             f(static_cast
        ///             <Types&&>
        ///                 (args)...);
        ///                 }
        ///  </code>
        /// </summary>
        PackExpansionExpression = 142,

        /// <summary>
        ///     Represents an expression that computes the length of a parameter
        ///     pack.
        ///     <code>
        ///  template<typename ... Types>
        ///             struct count {
        ///             static const unsigned value = sizeof...(Types);
        ///             };
        ///  </code>
        /// </summary>
        SizeOfPackExpression = 143,

        /// <summary>
        ///     Represents a C++ lambda expression that produces a local function
        ///     object.
        ///     <code>
        ///  void abssort(float *x, unsigned N) {
        ///    std::sort(x, x + N,
        ///              [](float a, float b) {
        ///                return std::abs(a) &lt; std::abs(b);
        ///              });
        ///  }
        ///  </code>
        /// </summary>
        LambdaExpression = 144,

        /// <summary>
        ///     Objective-c Boolean Literal.
        /// </summary>
        ObjCBoolLiteralExpression = 145,

        /// <summary>
        ///     Represents the "self" expression in a ObjC method.
        /// </summary>
        ObjCSelfExpression = 146,

        LastExpression = ObjCSelfExpression,

        /* Statements */

        FirstStatement = 200,

        /// <summary>
        ///     A statement whose specific kind is not exposed via this
        ///     interface.
        ///     Unexposed statements have the same operations as any other kind of
        ///     statement; one can extract their location information, spelling,
        ///     children, etc. However, the specific kind of the statement is not
        ///     reported.
        /// </summary>
        UnexposedStatement = 200,

        /// <summary>
        ///     A labelled statement in a function.
        ///     This cursor kind is used to describe the "start_over:" label statement in
        ///     the following example:
        ///     <code>
        ///    start_over:
        ///      ++counter;
        ///  </code>
        /// </summary>
        LabelStatement = 201,

        /// <summary>
        ///     A group of statements like { stmt stmt }.
        ///     This cursor kind is used to describe compound statements, e.g. function
        ///     bodies.
        /// </summary>
        CompoundStatement = 202,

        /// <summary>
        ///     A case statement.
        /// </summary>
        CaseStatement = 203,

        /// <summary>
        ///     A default statement.
        /// </summary>
        DefaultStatement = 204,

        /// <summary>
        ///     An if statement
        /// </summary>
        IfStatement = 205,

        /// <summary>
        ///     A switch statement.
        /// </summary>
        SwitchStatement = 206,

        /// <summary>
        ///     A while statement.
        /// </summary>
        WhileStatement = 207,

        /// <summary>
        ///     A do statement.
        /// </summary>
        DoStatement = 208,

        /// <summary>
        ///     A for statement.
        /// </summary>
        ForStatement = 209,

        /// <summary>
        ///     A goto statement.
        /// </summary>
        GotoStatement = 210,

        /// <summary>
        ///     An indirect goto statement.
        /// </summary>
        IndirectGotoStatement = 211,

        /// <summary>
        ///     A continue statement.
        /// </summary>
        ContinueStatement = 212,

        /// <summary>
        ///     A break statement.
        /// </summary>
        BreakStatement = 213,

        /// <summary>
        ///     A return statement.
        /// </summary>
        ReturnStatement = 214,

        /// <summary>
        ///     A GCC inline assembly statement extension.
        /// </summary>
        GCCAsmStatement = 215,

        AsmStatement = GCCAsmStatement,

        /// <summary>
        ///     Objective-C's overall @try-@catch-@finally statement.
        /// </summary>
        ObjCAtTryStatement = 216,

        /// <summary>
        ///     Objective-C's @catch statement.
        /// </summary>
        ObjCAtCatchStatement = 217,

        /// <summary>
        ///     Objective-C's @finally statement.
        /// </summary>
        ObjCAtFinallyStatement = 218,

        /// <summary>
        ///     Objective-C's @throw statement.
        /// </summary>
        ObjCAtThrowStatement = 219,

        /// <summary>
        ///     Objective-C's @synchronized statement.
        /// </summary>
        ObjCAtSynchronizedStatement = 220,

        /// <summary>
        ///     Objective-C's autorelease pool statement.
        /// </summary>
        ObjCAutoreleasePoolStatement = 221,

        /// <summary>
        ///     Objective-C's collection statement.
        /// </summary>
        ObjCForCollectionStatement = 222,

        /// <summary>
        ///     C++'s catch statement.
        /// </summary>
        CXXCatchStatement = 223,

        /// <summary>
        ///     C++'s try statement.
        /// </summary>
        CXXTryStatement = 224,

        /// <summary>
        ///     C++'s for (* : *) statement.
        /// </summary>
        CXXForRangeStatement = 225,

        /// <summary>
        ///     Windows Structured Exception Handling's try statement.
        /// </summary>
        SEHTryStatement = 226,

        /// <summary>
        ///     Windows Structured Exception Handling's except statement.
        /// </summary>
        SEHExceptStatement = 227,

        /// <summary>
        ///     Windows Structured Exception Handling's finally statement.
        /// </summary>
        SEHFinallyStatement = 228,

        /// <summary>
        ///     A MS inline assembly statement extension.
        /// </summary>
        MSAsmStatement = 229,

        /// <summary>
        ///     The null satement ";": C99 6.8.3p3.
        ///     This cursor kind is used to describe the null statement.
        /// </summary>
        NullStatement = 230,

        /// <summary>
        ///     Adaptor class for mixing declarations with statements and
        ///     expressions.
        /// </summary>
        DeclarationStatement = 231,

        /// <summary>
        ///     OpenMP parallel directive.
        /// </summary>
        OMPParallelDirective = 232,

        LastStatement = OMPParallelDirective,

        /// <summary>
        ///     Cursor that represents the translation unit itself.
        ///     The translation unit cursor exists primarily to act as the root
        ///     cursor for traversing the contents of a translation unit.
        /// </summary>
        TranslationUnit = 300,

        /* Attributes */

        FirstAttribute = 400,

        /// <summary>
        ///     An attribute whose specific kind is not exposed via this
        ///     interface.
        /// </summary>
        UnexposedAttribute = 400,

        IBActionAttribute = 401,
        IBOutletAttribute = 402,
        IBOutletCollectionAttribute = 403,
        CXXFinalAttribute = 404,
        CXXOverrideAttribute = 405,
        AnnotateAttribute = 406,
        AsmLabelAttribute = 407,
        PackedAttribute = 408,
        LastAttribute = PackedAttribute,

        /* Preprocessing */

        PreprocessingDirective = 500,
        MacroDefinition = 501,
        MacroExpansion = 502,
        MacroInstantiation = MacroExpansion,
        InclusionDirective = 503,
        FirstPreprocessing = PreprocessingDirective,
        LastPreprocessing = InclusionDirective
    }
}