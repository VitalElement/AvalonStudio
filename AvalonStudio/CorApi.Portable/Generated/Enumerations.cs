namespace CorApi.Portable
{
    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CeeSectionAttr</unmanaged>
    /// <unmanaged-short>CeeSectionAttr</unmanaged-short>
    public enum CeeSectionAttr : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>sdNone</unmanaged>
        /// <unmanaged-short>sdNone</unmanaged-short>
        SdNone = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>sdReadOnly</unmanaged>
        /// <unmanaged-short>sdReadOnly</unmanaged-short>
        SdReadOnly = unchecked ((System.Int32)(1073741888)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>sdReadWrite</unmanaged>
        /// <unmanaged-short>sdReadWrite</unmanaged-short>
        SdReadWrite = unchecked ((System.Int32)(-1073741760)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>sdExecute</unmanaged>
        /// <unmanaged-short>sdExecute</unmanaged-short>
        SdExecute = unchecked ((System.Int32)(1610612768))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CeeSectionRelocType</unmanaged>
    /// <unmanaged-short>CeeSectionRelocType</unmanaged-short>
    public enum CeeSectionRelocType : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>srRelocAbsolute</unmanaged>
        /// <unmanaged-short>srRelocAbsolute</unmanaged-short>
        SrRelocAbsolute = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>srRelocHighLow</unmanaged>
        /// <unmanaged-short>srRelocHighLow</unmanaged-short>
        SrRelocHighLow = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>srRelocHighAdj</unmanaged>
        /// <unmanaged-short>srRelocHighAdj</unmanaged-short>
        SrRelocHighAdj = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>srRelocMapToken</unmanaged>
        /// <unmanaged-short>srRelocMapToken</unmanaged-short>
        SrRelocMapToken = unchecked ((System.Int32)(5)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>srRelocRelative</unmanaged>
        /// <unmanaged-short>srRelocRelative</unmanaged-short>
        SrRelocRelative = unchecked ((System.Int32)(6)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>srRelocFilePos</unmanaged>
        /// <unmanaged-short>srRelocFilePos</unmanaged-short>
        SrRelocFilePos = unchecked ((System.Int32)(7)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>srRelocCodeRelative</unmanaged>
        /// <unmanaged-short>srRelocCodeRelative</unmanaged-short>
        SrRelocCodeRelative = unchecked ((System.Int32)(8)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>srRelocIA64Imm64</unmanaged>
        /// <unmanaged-short>srRelocIA64Imm64</unmanaged-short>
        SrRelocIA64Imm64 = unchecked ((System.Int32)(9)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>srRelocDir64</unmanaged>
        /// <unmanaged-short>srRelocDir64</unmanaged-short>
        SrRelocDir64 = unchecked ((System.Int32)(10)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>srRelocIA64PcRel25</unmanaged>
        /// <unmanaged-short>srRelocIA64PcRel25</unmanaged-short>
        SrRelocIA64PcRel25 = unchecked ((System.Int32)(11)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>srRelocIA64PcRel64</unmanaged>
        /// <unmanaged-short>srRelocIA64PcRel64</unmanaged-short>
        SrRelocIA64PcRel64 = unchecked ((System.Int32)(12)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>srRelocAbsoluteTagged</unmanaged>
        /// <unmanaged-short>srRelocAbsoluteTagged</unmanaged-short>
        SrRelocAbsoluteTagged = unchecked ((System.Int32)(13)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>srRelocSentinel</unmanaged>
        /// <unmanaged-short>srRelocSentinel</unmanaged-short>
        SrRelocSentinel = unchecked ((System.Int32)(14)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>srNoBaseReloc</unmanaged>
        /// <unmanaged-short>srNoBaseReloc</unmanaged-short>
        SrNoBaseReloc = unchecked ((System.Int32)(16384)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>srRelocPtr</unmanaged>
        /// <unmanaged-short>srRelocPtr</unmanaged-short>
        SrRelocPtr = unchecked ((System.Int32)(32768)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>srRelocAbsolutePtr</unmanaged>
        /// <unmanaged-short>srRelocAbsolutePtr</unmanaged-short>
        SrRelocAbsolutePtr = unchecked ((System.Int32)(32768)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>srRelocHighLowPtr</unmanaged>
        /// <unmanaged-short>srRelocHighLowPtr</unmanaged-short>
        SrRelocHighLowPtr = unchecked ((System.Int32)(32771)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>srRelocRelativePtr</unmanaged>
        /// <unmanaged-short>srRelocRelativePtr</unmanaged-short>
        SrRelocRelativePtr = unchecked ((System.Int32)(32774)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>srRelocIA64Imm64Ptr</unmanaged>
        /// <unmanaged-short>srRelocIA64Imm64Ptr</unmanaged-short>
        SrRelocIA64Imm64Ptr = unchecked ((System.Int32)(32777)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>srRelocDir64Ptr</unmanaged>
        /// <unmanaged-short>srRelocDir64Ptr</unmanaged-short>
        SrRelocDir64Ptr = unchecked ((System.Int32)(32778))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>COINITICOR</unmanaged>
    /// <unmanaged-short>COINITICOR</unmanaged-short>
    public enum Coiniticor : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>COINITCOR_DEFAULT</unmanaged>
        /// <unmanaged-short>COINITCOR_DEFAULT</unmanaged-short>
        CorDefault = unchecked ((System.Int32)(0))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>COINITIEE</unmanaged>
    /// <unmanaged-short>COINITIEE</unmanaged-short>
    public enum Coinitiee : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>COINITEE_DEFAULT</unmanaged>
        /// <unmanaged-short>COINITEE_DEFAULT</unmanaged-short>
        EeDefault = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>COINITEE_DLL</unmanaged>
        /// <unmanaged-short>COINITEE_DLL</unmanaged-short>
        EeDll = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>COINITEE_MAIN</unmanaged>
        /// <unmanaged-short>COINITEE_MAIN</unmanaged-short>
        EeMain = unchecked ((System.Int32)(2))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CompilationRelaxationsEnum</unmanaged>
    /// <unmanaged-short>CompilationRelaxationsEnum</unmanaged-short>
    public enum CompilationRelaxationsEnum : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CompilationRelaxations_NoStringInterning</unmanaged>
        /// <unmanaged-short>CompilationRelaxations_NoStringInterning</unmanaged-short>
        NoStringInterning = unchecked ((System.Int32)(8))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorArgType</unmanaged>
    /// <unmanaged-short>CorArgType</unmanaged-short>
    public enum CorArgType : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IMAGE_CEE_CS_END</unmanaged>
        /// <unmanaged-short>IMAGE_CEE_CS_END</unmanaged-short>
        ImageCeeCsEnd = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IMAGE_CEE_CS_VOID</unmanaged>
        /// <unmanaged-short>IMAGE_CEE_CS_VOID</unmanaged-short>
        ImageCeeCsVoid = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IMAGE_CEE_CS_I4</unmanaged>
        /// <unmanaged-short>IMAGE_CEE_CS_I4</unmanaged-short>
        ImageCeeCsI4 = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IMAGE_CEE_CS_I8</unmanaged>
        /// <unmanaged-short>IMAGE_CEE_CS_I8</unmanaged-short>
        ImageCeeCsI8 = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IMAGE_CEE_CS_R4</unmanaged>
        /// <unmanaged-short>IMAGE_CEE_CS_R4</unmanaged-short>
        ImageCeeCsR4 = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IMAGE_CEE_CS_R8</unmanaged>
        /// <unmanaged-short>IMAGE_CEE_CS_R8</unmanaged-short>
        ImageCeeCsR8 = unchecked ((System.Int32)(5)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IMAGE_CEE_CS_PTR</unmanaged>
        /// <unmanaged-short>IMAGE_CEE_CS_PTR</unmanaged-short>
        ImageCeeCsPtr = unchecked ((System.Int32)(6)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IMAGE_CEE_CS_OBJECT</unmanaged>
        /// <unmanaged-short>IMAGE_CEE_CS_OBJECT</unmanaged-short>
        ImageCeeCsObject = unchecked ((System.Int32)(7)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IMAGE_CEE_CS_STRUCT4</unmanaged>
        /// <unmanaged-short>IMAGE_CEE_CS_STRUCT4</unmanaged-short>
        ImageCeeCsStruct4 = unchecked ((System.Int32)(8)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IMAGE_CEE_CS_STRUCT32</unmanaged>
        /// <unmanaged-short>IMAGE_CEE_CS_STRUCT32</unmanaged-short>
        ImageCeeCsStruct32 = unchecked ((System.Int32)(9)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IMAGE_CEE_CS_BYVALUE</unmanaged>
        /// <unmanaged-short>IMAGE_CEE_CS_BYVALUE</unmanaged-short>
        ImageCeeCsByvalue = unchecked ((System.Int32)(10))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorAssemblyFlags</unmanaged>
    /// <unmanaged-short>CorAssemblyFlags</unmanaged-short>
    public enum CorAssemblyFlags : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>afPublicKey</unmanaged>
        /// <unmanaged-short>afPublicKey</unmanaged-short>
        AfPublicKey = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>afPA_None</unmanaged>
        /// <unmanaged-short>afPA_None</unmanaged-short>
        AfPANone = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>afPA_MSIL</unmanaged>
        /// <unmanaged-short>afPA_MSIL</unmanaged-short>
        AfPAMsil = unchecked ((System.Int32)(16)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>afPA_x86</unmanaged>
        /// <unmanaged-short>afPA_x86</unmanaged-short>
        AfPAX86 = unchecked ((System.Int32)(32)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>afPA_IA64</unmanaged>
        /// <unmanaged-short>afPA_IA64</unmanaged-short>
        AfPAIa64 = unchecked ((System.Int32)(48)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>afPA_AMD64</unmanaged>
        /// <unmanaged-short>afPA_AMD64</unmanaged-short>
        AfPAAmd64 = unchecked ((System.Int32)(64)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>afPA_ARM</unmanaged>
        /// <unmanaged-short>afPA_ARM</unmanaged-short>
        AfPAArm = unchecked ((System.Int32)(80)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>afPA_NoPlatform</unmanaged>
        /// <unmanaged-short>afPA_NoPlatform</unmanaged-short>
        AfPANoPlatform = unchecked ((System.Int32)(112)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>afPA_Specified</unmanaged>
        /// <unmanaged-short>afPA_Specified</unmanaged-short>
        AfPASpecified = unchecked ((System.Int32)(128)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>afPA_Mask</unmanaged>
        /// <unmanaged-short>afPA_Mask</unmanaged-short>
        AfPAMask = unchecked ((System.Int32)(112)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>afPA_FullMask</unmanaged>
        /// <unmanaged-short>afPA_FullMask</unmanaged-short>
        AfPAFullMask = unchecked ((System.Int32)(240)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>afPA_Shift</unmanaged>
        /// <unmanaged-short>afPA_Shift</unmanaged-short>
        AfPAShift = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>afEnableJITcompileTracking</unmanaged>
        /// <unmanaged-short>afEnableJITcompileTracking</unmanaged-short>
        AfEnableJITcompileTracking = unchecked ((System.Int32)(32768)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>afDisableJITcompileOptimizer</unmanaged>
        /// <unmanaged-short>afDisableJITcompileOptimizer</unmanaged-short>
        AfDisableJITcompileOptimizer = unchecked ((System.Int32)(16384)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>afRetargetable</unmanaged>
        /// <unmanaged-short>afRetargetable</unmanaged-short>
        AfRetargetable = unchecked ((System.Int32)(256)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>afContentType_Default</unmanaged>
        /// <unmanaged-short>afContentType_Default</unmanaged-short>
        AfContentTypeDefault = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>afContentType_WindowsRuntime</unmanaged>
        /// <unmanaged-short>afContentType_WindowsRuntime</unmanaged-short>
        AfContentTypeWindowsRuntime = unchecked ((System.Int32)(512)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>afContentType_Mask</unmanaged>
        /// <unmanaged-short>afContentType_Mask</unmanaged-short>
        AfContentTypeMask = unchecked ((System.Int32)(3584))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorAttributeTargets</unmanaged>
    /// <unmanaged-short>CorAttributeTargets</unmanaged-short>
    public enum CorAttributeTargets : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>catAssembly</unmanaged>
        /// <unmanaged-short>catAssembly</unmanaged-short>
        CatAssembly = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>catModule</unmanaged>
        /// <unmanaged-short>catModule</unmanaged-short>
        CatModule = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>catClass</unmanaged>
        /// <unmanaged-short>catClass</unmanaged-short>
        CatClass = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>catStruct</unmanaged>
        /// <unmanaged-short>catStruct</unmanaged-short>
        CatStruct = unchecked ((System.Int32)(8)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>catEnum</unmanaged>
        /// <unmanaged-short>catEnum</unmanaged-short>
        CatEnum = unchecked ((System.Int32)(16)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>catConstructor</unmanaged>
        /// <unmanaged-short>catConstructor</unmanaged-short>
        CatConstructor = unchecked ((System.Int32)(32)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>catMethod</unmanaged>
        /// <unmanaged-short>catMethod</unmanaged-short>
        CatMethod = unchecked ((System.Int32)(64)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>catProperty</unmanaged>
        /// <unmanaged-short>catProperty</unmanaged-short>
        CatProperty = unchecked ((System.Int32)(128)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>catField</unmanaged>
        /// <unmanaged-short>catField</unmanaged-short>
        CatField = unchecked ((System.Int32)(256)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>catEvent</unmanaged>
        /// <unmanaged-short>catEvent</unmanaged-short>
        CatEvent = unchecked ((System.Int32)(512)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>catInterface</unmanaged>
        /// <unmanaged-short>catInterface</unmanaged-short>
        CatInterface = unchecked ((System.Int32)(1024)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>catParameter</unmanaged>
        /// <unmanaged-short>catParameter</unmanaged-short>
        CatParameter = unchecked ((System.Int32)(2048)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>catDelegate</unmanaged>
        /// <unmanaged-short>catDelegate</unmanaged-short>
        CatDelegate = unchecked ((System.Int32)(4096)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>catGenericParameter</unmanaged>
        /// <unmanaged-short>catGenericParameter</unmanaged-short>
        CatGenericParameter = unchecked ((System.Int32)(16384)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>catAll</unmanaged>
        /// <unmanaged-short>catAll</unmanaged-short>
        CatAll = unchecked ((System.Int32)(24575)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>catClassMembers</unmanaged>
        /// <unmanaged-short>catClassMembers</unmanaged-short>
        CatClassMembers = unchecked ((System.Int32)(6140))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorCallingConvention</unmanaged>
    /// <unmanaged-short>CorCallingConvention</unmanaged-short>
    public enum CorCallingConvention : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IMAGE_CEE_CS_CALLCONV_DEFAULT</unmanaged>
        /// <unmanaged-short>IMAGE_CEE_CS_CALLCONV_DEFAULT</unmanaged-short>
        ImageCeeCsCallconvDefault = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IMAGE_CEE_CS_CALLCONV_VARARG</unmanaged>
        /// <unmanaged-short>IMAGE_CEE_CS_CALLCONV_VARARG</unmanaged-short>
        ImageCeeCsCallconvVararg = unchecked ((System.Int32)(5)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IMAGE_CEE_CS_CALLCONV_FIELD</unmanaged>
        /// <unmanaged-short>IMAGE_CEE_CS_CALLCONV_FIELD</unmanaged-short>
        ImageCeeCsCallconvField = unchecked ((System.Int32)(6)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IMAGE_CEE_CS_CALLCONV_LOCAL_SIG</unmanaged>
        /// <unmanaged-short>IMAGE_CEE_CS_CALLCONV_LOCAL_SIG</unmanaged-short>
        ImageCeeCsCallconvLocalSig = unchecked ((System.Int32)(7)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IMAGE_CEE_CS_CALLCONV_PROPERTY</unmanaged>
        /// <unmanaged-short>IMAGE_CEE_CS_CALLCONV_PROPERTY</unmanaged-short>
        ImageCeeCsCallconvProperty = unchecked ((System.Int32)(8)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IMAGE_CEE_CS_CALLCONV_UNMGD</unmanaged>
        /// <unmanaged-short>IMAGE_CEE_CS_CALLCONV_UNMGD</unmanaged-short>
        ImageCeeCsCallconvUnmgd = unchecked ((System.Int32)(9)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IMAGE_CEE_CS_CALLCONV_GENERICINST</unmanaged>
        /// <unmanaged-short>IMAGE_CEE_CS_CALLCONV_GENERICINST</unmanaged-short>
        ImageCeeCsCallconvGenericinst = unchecked ((System.Int32)(10)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IMAGE_CEE_CS_CALLCONV_NATIVEVARARG</unmanaged>
        /// <unmanaged-short>IMAGE_CEE_CS_CALLCONV_NATIVEVARARG</unmanaged-short>
        ImageCeeCsCallconvNativevararg = unchecked ((System.Int32)(11)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IMAGE_CEE_CS_CALLCONV_MAX</unmanaged>
        /// <unmanaged-short>IMAGE_CEE_CS_CALLCONV_MAX</unmanaged-short>
        ImageCeeCsCallconvMax = unchecked ((System.Int32)(12)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IMAGE_CEE_CS_CALLCONV_MASK</unmanaged>
        /// <unmanaged-short>IMAGE_CEE_CS_CALLCONV_MASK</unmanaged-short>
        ImageCeeCsCallconvMask = unchecked ((System.Int32)(15)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IMAGE_CEE_CS_CALLCONV_HASTHIS</unmanaged>
        /// <unmanaged-short>IMAGE_CEE_CS_CALLCONV_HASTHIS</unmanaged-short>
        ImageCeeCsCallconvHasthis = unchecked ((System.Int32)(32)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IMAGE_CEE_CS_CALLCONV_EXPLICITTHIS</unmanaged>
        /// <unmanaged-short>IMAGE_CEE_CS_CALLCONV_EXPLICITTHIS</unmanaged-short>
        ImageCeeCsCallconvExplicitthis = unchecked ((System.Int32)(64)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IMAGE_CEE_CS_CALLCONV_GENERIC</unmanaged>
        /// <unmanaged-short>IMAGE_CEE_CS_CALLCONV_GENERIC</unmanaged-short>
        ImageCeeCsCallconvGeneric = unchecked ((System.Int32)(16))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorCheckDuplicatesFor</unmanaged>
    /// <unmanaged-short>CorCheckDuplicatesFor</unmanaged-short>
    public enum CorCheckDuplicatesFor : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDDupAll</unmanaged>
        /// <unmanaged-short>MDDupAll</unmanaged-short>
        MDDupAll = unchecked ((System.Int32)(-1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDDupENC</unmanaged>
        /// <unmanaged-short>MDDupENC</unmanaged-short>
        MDDupENC = unchecked ((System.Int32)(-1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDNoDupChecks</unmanaged>
        /// <unmanaged-short>MDNoDupChecks</unmanaged-short>
        MDNoDupChecks = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDDupTypeDef</unmanaged>
        /// <unmanaged-short>MDDupTypeDef</unmanaged-short>
        MDDupTypeDef = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDDupInterfaceImpl</unmanaged>
        /// <unmanaged-short>MDDupInterfaceImpl</unmanaged-short>
        MDDupInterfaceImpl = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDDupMethodDef</unmanaged>
        /// <unmanaged-short>MDDupMethodDef</unmanaged-short>
        MDDupMethodDef = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDDupTypeRef</unmanaged>
        /// <unmanaged-short>MDDupTypeRef</unmanaged-short>
        MDDupTypeRef = unchecked ((System.Int32)(8)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDDupMemberRef</unmanaged>
        /// <unmanaged-short>MDDupMemberRef</unmanaged-short>
        MDDupMemberRef = unchecked ((System.Int32)(16)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDDupCustomAttribute</unmanaged>
        /// <unmanaged-short>MDDupCustomAttribute</unmanaged-short>
        MDDupCustomAttribute = unchecked ((System.Int32)(32)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDDupParamDef</unmanaged>
        /// <unmanaged-short>MDDupParamDef</unmanaged-short>
        MDDupParamDef = unchecked ((System.Int32)(64)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDDupPermission</unmanaged>
        /// <unmanaged-short>MDDupPermission</unmanaged-short>
        MDDupPermission = unchecked ((System.Int32)(128)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDDupProperty</unmanaged>
        /// <unmanaged-short>MDDupProperty</unmanaged-short>
        MDDupProperty = unchecked ((System.Int32)(256)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDDupEvent</unmanaged>
        /// <unmanaged-short>MDDupEvent</unmanaged-short>
        MDDupEvent = unchecked ((System.Int32)(512)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDDupFieldDef</unmanaged>
        /// <unmanaged-short>MDDupFieldDef</unmanaged-short>
        MDDupFieldDef = unchecked ((System.Int32)(1024)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDDupSignature</unmanaged>
        /// <unmanaged-short>MDDupSignature</unmanaged-short>
        MDDupSignature = unchecked ((System.Int32)(2048)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDDupModuleRef</unmanaged>
        /// <unmanaged-short>MDDupModuleRef</unmanaged-short>
        MDDupModuleRef = unchecked ((System.Int32)(4096)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDDupTypeSpec</unmanaged>
        /// <unmanaged-short>MDDupTypeSpec</unmanaged-short>
        MDDupTypeSpec = unchecked ((System.Int32)(8192)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDDupImplMap</unmanaged>
        /// <unmanaged-short>MDDupImplMap</unmanaged-short>
        MDDupImplMap = unchecked ((System.Int32)(16384)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDDupAssemblyRef</unmanaged>
        /// <unmanaged-short>MDDupAssemblyRef</unmanaged-short>
        MDDupAssemblyRef = unchecked ((System.Int32)(32768)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDDupFile</unmanaged>
        /// <unmanaged-short>MDDupFile</unmanaged-short>
        MDDupFile = unchecked ((System.Int32)(65536)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDDupExportedType</unmanaged>
        /// <unmanaged-short>MDDupExportedType</unmanaged-short>
        MDDupExportedType = unchecked ((System.Int32)(131072)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDDupManifestResource</unmanaged>
        /// <unmanaged-short>MDDupManifestResource</unmanaged-short>
        MDDupManifestResource = unchecked ((System.Int32)(262144)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDDupGenericParam</unmanaged>
        /// <unmanaged-short>MDDupGenericParam</unmanaged-short>
        MDDupGenericParam = unchecked ((System.Int32)(524288)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDDupMethodSpec</unmanaged>
        /// <unmanaged-short>MDDupMethodSpec</unmanaged-short>
        MDDupMethodSpec = unchecked ((System.Int32)(1048576)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDDupGenericParamConstraint</unmanaged>
        /// <unmanaged-short>MDDupGenericParamConstraint</unmanaged-short>
        MDDupGenericParamConstraint = unchecked ((System.Int32)(2097152)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDDupAssembly</unmanaged>
        /// <unmanaged-short>MDDupAssembly</unmanaged-short>
        MDDupAssembly = unchecked ((System.Int32)(268435456)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDDupDefault</unmanaged>
        /// <unmanaged-short>MDDupDefault</unmanaged-short>
        MDDupDefault = unchecked ((System.Int32)(1058840))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugBlockingReason</unmanaged>
    /// <unmanaged-short>CorDebugBlockingReason</unmanaged-short>
    public enum CorDebugBlockingReason : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>BLOCKING_NONE</unmanaged>
        /// <unmanaged-short>BLOCKING_NONE</unmanaged-short>
        BlockingNone = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>BLOCKING_MONITOR_CRITICAL_SECTION</unmanaged>
        /// <unmanaged-short>BLOCKING_MONITOR_CRITICAL_SECTION</unmanaged-short>
        BlockingMonitorCriticalSection = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>BLOCKING_MONITOR_EVENT</unmanaged>
        /// <unmanaged-short>BLOCKING_MONITOR_EVENT</unmanaged-short>
        BlockingMonitorEvent = unchecked ((System.Int32)(2))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugChainReason</unmanaged>
    /// <unmanaged-short>CorDebugChainReason</unmanaged-short>
    public enum CorDebugChainReason : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CHAIN_NONE</unmanaged>
        /// <unmanaged-short>CHAIN_NONE</unmanaged-short>
        ChainNone = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CHAIN_CLASS_INIT</unmanaged>
        /// <unmanaged-short>CHAIN_CLASS_INIT</unmanaged-short>
        ChainClassInit = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CHAIN_EXCEPTION_FILTER</unmanaged>
        /// <unmanaged-short>CHAIN_EXCEPTION_FILTER</unmanaged-short>
        ChainExceptionFilter = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CHAIN_SECURITY</unmanaged>
        /// <unmanaged-short>CHAIN_SECURITY</unmanaged-short>
        ChainSecurity = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CHAIN_CONTEXT_POLICY</unmanaged>
        /// <unmanaged-short>CHAIN_CONTEXT_POLICY</unmanaged-short>
        ChainContextPolicy = unchecked ((System.Int32)(8)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CHAIN_INTERCEPTION</unmanaged>
        /// <unmanaged-short>CHAIN_INTERCEPTION</unmanaged-short>
        ChainInterception = unchecked ((System.Int32)(16)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CHAIN_PROCESS_START</unmanaged>
        /// <unmanaged-short>CHAIN_PROCESS_START</unmanaged-short>
        ChainProcessStart = unchecked ((System.Int32)(32)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CHAIN_THREAD_START</unmanaged>
        /// <unmanaged-short>CHAIN_THREAD_START</unmanaged-short>
        ChainThreadStart = unchecked ((System.Int32)(64)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CHAIN_ENTER_MANAGED</unmanaged>
        /// <unmanaged-short>CHAIN_ENTER_MANAGED</unmanaged-short>
        ChainEnterManaged = unchecked ((System.Int32)(128)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CHAIN_ENTER_UNMANAGED</unmanaged>
        /// <unmanaged-short>CHAIN_ENTER_UNMANAGED</unmanaged-short>
        ChainEnterUnmanaged = unchecked ((System.Int32)(256)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CHAIN_DEBUGGER_EVAL</unmanaged>
        /// <unmanaged-short>CHAIN_DEBUGGER_EVAL</unmanaged-short>
        ChainDebuggerEval = unchecked ((System.Int32)(512)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CHAIN_CONTEXT_SWITCH</unmanaged>
        /// <unmanaged-short>CHAIN_CONTEXT_SWITCH</unmanaged-short>
        ChainContextSwitch = unchecked ((System.Int32)(1024)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CHAIN_FUNC_EVAL</unmanaged>
        /// <unmanaged-short>CHAIN_FUNC_EVAL</unmanaged-short>
        ChainFuncEval = unchecked ((System.Int32)(2048))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugCodeInvokeKind</unmanaged>
    /// <unmanaged-short>CorDebugCodeInvokeKind</unmanaged-short>
    public enum CorDebugCodeInvokeKind : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CODE_INVOKE_KIND_NONE</unmanaged>
        /// <unmanaged-short>CODE_INVOKE_KIND_NONE</unmanaged-short>
        CodeInvokeKindNone = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CODE_INVOKE_KIND_RETURN</unmanaged>
        /// <unmanaged-short>CODE_INVOKE_KIND_RETURN</unmanaged-short>
        CodeInvokeKindReturn = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CODE_INVOKE_KIND_TAILCALL</unmanaged>
        /// <unmanaged-short>CODE_INVOKE_KIND_TAILCALL</unmanaged-short>
        CodeInvokeKindTailcall = unchecked ((System.Int32)(2))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugCodeInvokePurpose</unmanaged>
    /// <unmanaged-short>CorDebugCodeInvokePurpose</unmanaged-short>
    public enum CorDebugCodeInvokePurpose : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CODE_INVOKE_PURPOSE_NONE</unmanaged>
        /// <unmanaged-short>CODE_INVOKE_PURPOSE_NONE</unmanaged-short>
        CodeInvokePurposeNone = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CODE_INVOKE_PURPOSE_NATIVE_TO_MANAGED_TRANSITION</unmanaged>
        /// <unmanaged-short>CODE_INVOKE_PURPOSE_NATIVE_TO_MANAGED_TRANSITION</unmanaged-short>
        CodeInvokePurposeNativeToManagedTransition = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CODE_INVOKE_PURPOSE_CLASS_INIT</unmanaged>
        /// <unmanaged-short>CODE_INVOKE_PURPOSE_CLASS_INIT</unmanaged-short>
        CodeInvokePurposeClassInit = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CODE_INVOKE_PURPOSE_INTERFACE_DISPATCH</unmanaged>
        /// <unmanaged-short>CODE_INVOKE_PURPOSE_INTERFACE_DISPATCH</unmanaged-short>
        CodeInvokePurposeInterfaceDispatch = unchecked ((System.Int32)(3))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugCreateProcessFlags</unmanaged>
    /// <unmanaged-short>CorDebugCreateProcessFlags</unmanaged-short>
    public enum CorDebugCreateProcessFlags : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>DEBUG_NO_SPECIAL_OPTIONS</unmanaged>
        /// <unmanaged-short>DEBUG_NO_SPECIAL_OPTIONS</unmanaged-short>
        DebugNoSpecialOptions = unchecked ((System.Int32)(0))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugDebugEventKind</unmanaged>
    /// <unmanaged-short>CorDebugDebugEventKind</unmanaged-short>
    public enum CorDebugDebugEventKind : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>DEBUG_EVENT_KIND_MODULE_LOADED</unmanaged>
        /// <unmanaged-short>DEBUG_EVENT_KIND_MODULE_LOADED</unmanaged-short>
        DebugEventKindModuleLoaded = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>DEBUG_EVENT_KIND_MODULE_UNLOADED</unmanaged>
        /// <unmanaged-short>DEBUG_EVENT_KIND_MODULE_UNLOADED</unmanaged-short>
        DebugEventKindModuleUnloaded = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>DEBUG_EVENT_KIND_MANAGED_EXCEPTION_FIRST_CHANCE</unmanaged>
        /// <unmanaged-short>DEBUG_EVENT_KIND_MANAGED_EXCEPTION_FIRST_CHANCE</unmanaged-short>
        DebugEventKindManagedExceptionFirstChance = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>DEBUG_EVENT_KIND_MANAGED_EXCEPTION_USER_FIRST_CHANCE</unmanaged>
        /// <unmanaged-short>DEBUG_EVENT_KIND_MANAGED_EXCEPTION_USER_FIRST_CHANCE</unmanaged-short>
        DebugEventKindManagedExceptionUserFirstChance = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>DEBUG_EVENT_KIND_MANAGED_EXCEPTION_CATCH_HANDLER_FOUND</unmanaged>
        /// <unmanaged-short>DEBUG_EVENT_KIND_MANAGED_EXCEPTION_CATCH_HANDLER_FOUND</unmanaged-short>
        DebugEventKindManagedExceptionCatchHandlerFound = unchecked ((System.Int32)(5)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>DEBUG_EVENT_KIND_MANAGED_EXCEPTION_UNHANDLED</unmanaged>
        /// <unmanaged-short>DEBUG_EVENT_KIND_MANAGED_EXCEPTION_UNHANDLED</unmanaged-short>
        DebugEventKindManagedExceptionUnhandled = unchecked ((System.Int32)(6))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugDecodeEventFlagsWindows</unmanaged>
    /// <unmanaged-short>CorDebugDecodeEventFlagsWindows</unmanaged-short>
    public enum CorDebugDecodeEventFlagsWindows : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IS_FIRST_CHANCE</unmanaged>
        /// <unmanaged-short>IS_FIRST_CHANCE</unmanaged-short>
        IsFirstChance = unchecked ((System.Int32)(1))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugExceptionCallbackType</unmanaged>
    /// <unmanaged-short>CorDebugExceptionCallbackType</unmanaged-short>
    public enum CorDebugExceptionCallbackType : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>DEBUG_EXCEPTION_FIRST_CHANCE</unmanaged>
        /// <unmanaged-short>DEBUG_EXCEPTION_FIRST_CHANCE</unmanaged-short>
        DebugExceptionFirstChance = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>DEBUG_EXCEPTION_USER_FIRST_CHANCE</unmanaged>
        /// <unmanaged-short>DEBUG_EXCEPTION_USER_FIRST_CHANCE</unmanaged-short>
        DebugExceptionUserFirstChance = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>DEBUG_EXCEPTION_CATCH_HANDLER_FOUND</unmanaged>
        /// <unmanaged-short>DEBUG_EXCEPTION_CATCH_HANDLER_FOUND</unmanaged-short>
        DebugExceptionCatchHandlerFound = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>DEBUG_EXCEPTION_UNHANDLED</unmanaged>
        /// <unmanaged-short>DEBUG_EXCEPTION_UNHANDLED</unmanaged-short>
        DebugExceptionUnhandled = unchecked ((System.Int32)(4))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugExceptionFlags</unmanaged>
    /// <unmanaged-short>CorDebugExceptionFlags</unmanaged-short>
    public enum CorDebugExceptionFlags : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>DEBUG_EXCEPTION_NONE</unmanaged>
        /// <unmanaged-short>DEBUG_EXCEPTION_NONE</unmanaged-short>
        DebugExceptionNone = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>DEBUG_EXCEPTION_CAN_BE_INTERCEPTED</unmanaged>
        /// <unmanaged-short>DEBUG_EXCEPTION_CAN_BE_INTERCEPTED</unmanaged-short>
        DebugExceptionCanBeIntercepted = unchecked ((System.Int32)(1))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugExceptionUnwindCallbackType</unmanaged>
    /// <unmanaged-short>CorDebugExceptionUnwindCallbackType</unmanaged-short>
    public enum CorDebugExceptionUnwindCallbackType : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>DEBUG_EXCEPTION_UNWIND_BEGIN</unmanaged>
        /// <unmanaged-short>DEBUG_EXCEPTION_UNWIND_BEGIN</unmanaged-short>
        DebugExceptionUnwindBegin = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>DEBUG_EXCEPTION_INTERCEPTED</unmanaged>
        /// <unmanaged-short>DEBUG_EXCEPTION_INTERCEPTED</unmanaged-short>
        DebugExceptionIntercepted = unchecked ((System.Int32)(2))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugGCType</unmanaged>
    /// <unmanaged-short>CorDebugGCType</unmanaged-short>
    public enum CorDebugGCType : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorDebugWorkstationGC</unmanaged>
        /// <unmanaged-short>CorDebugWorkstationGC</unmanaged-short>
        CorDebugWorkstationGC = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorDebugServerGC</unmanaged>
        /// <unmanaged-short>CorDebugServerGC</unmanaged-short>
        CorDebugServerGC = unchecked ((System.Int32)(1))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugGenerationTypes</unmanaged>
    /// <unmanaged-short>CorDebugGenerationTypes</unmanaged-short>
    public enum CorDebugGenerationTypes : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorDebug_Gen0</unmanaged>
        /// <unmanaged-short>CorDebug_Gen0</unmanaged-short>
        Gen0 = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorDebug_Gen1</unmanaged>
        /// <unmanaged-short>CorDebug_Gen1</unmanaged-short>
        Gen1 = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorDebug_Gen2</unmanaged>
        /// <unmanaged-short>CorDebug_Gen2</unmanaged-short>
        Gen2 = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorDebug_LOH</unmanaged>
        /// <unmanaged-short>CorDebug_LOH</unmanaged-short>
        Loh = unchecked ((System.Int32)(3))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugHandleType</unmanaged>
    /// <unmanaged-short>CorDebugHandleType</unmanaged-short>
    public enum CorDebugHandleType : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>HANDLE_STRONG</unmanaged>
        /// <unmanaged-short>HANDLE_STRONG</unmanaged-short>
        HandleStrong = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>HANDLE_WEAK_TRACK_RESURRECTION</unmanaged>
        /// <unmanaged-short>HANDLE_WEAK_TRACK_RESURRECTION</unmanaged-short>
        HandleWeakTrackResurrection = unchecked ((System.Int32)(2))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugIlToNativeMappingTypes</unmanaged>
    /// <unmanaged-short>CorDebugIlToNativeMappingTypes</unmanaged-short>
    public enum CorDebugIlToNativeMappingTypes : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NO_MAPPING</unmanaged>
        /// <unmanaged-short>NO_MAPPING</unmanaged-short>
        NoMapping = unchecked ((System.Int32)(-1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>PROLOG</unmanaged>
        /// <unmanaged-short>PROLOG</unmanaged-short>
        Prolog = unchecked ((System.Int32)(-2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>EPILOG</unmanaged>
        /// <unmanaged-short>EPILOG</unmanaged-short>
        Epilog = unchecked ((System.Int32)(-3))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugIntercept</unmanaged>
    /// <unmanaged-short>CorDebugIntercept</unmanaged-short>
    public enum CorDebugIntercept : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>INTERCEPT_NONE</unmanaged>
        /// <unmanaged-short>INTERCEPT_NONE</unmanaged-short>
        InterceptNone = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>INTERCEPT_CLASS_INIT</unmanaged>
        /// <unmanaged-short>INTERCEPT_CLASS_INIT</unmanaged-short>
        InterceptClassInit = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>INTERCEPT_EXCEPTION_FILTER</unmanaged>
        /// <unmanaged-short>INTERCEPT_EXCEPTION_FILTER</unmanaged-short>
        InterceptExceptionFilter = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>INTERCEPT_SECURITY</unmanaged>
        /// <unmanaged-short>INTERCEPT_SECURITY</unmanaged-short>
        InterceptSecurity = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>INTERCEPT_CONTEXT_POLICY</unmanaged>
        /// <unmanaged-short>INTERCEPT_CONTEXT_POLICY</unmanaged-short>
        InterceptContextPolicy = unchecked ((System.Int32)(8)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>INTERCEPT_INTERCEPTION</unmanaged>
        /// <unmanaged-short>INTERCEPT_INTERCEPTION</unmanaged-short>
        InterceptInterception = unchecked ((System.Int32)(16)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>INTERCEPT_ALL</unmanaged>
        /// <unmanaged-short>INTERCEPT_ALL</unmanaged-short>
        InterceptAll = unchecked ((System.Int32)(65535))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugInterfaceVersion</unmanaged>
    /// <unmanaged-short>CorDebugInterfaceVersion</unmanaged-short>
    public enum CorDebugInterfaceVersion : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorDebugInvalidVersion</unmanaged>
        /// <unmanaged-short>CorDebugInvalidVersion</unmanaged-short>
        CorDebugInvalidVersion = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorDebugVersion_1_0</unmanaged>
        /// <unmanaged-short>CorDebugVersion_1_0</unmanaged-short>
        CorDebugVersion10 = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugManagedCallback</unmanaged>
        /// <unmanaged-short>ver_ICorDebugManagedCallback</unmanaged-short>
        VerICorDebugManagedCallback = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugUnmanagedCallback</unmanaged>
        /// <unmanaged-short>ver_ICorDebugUnmanagedCallback</unmanaged-short>
        VerICorDebugUnmanagedCallback = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebug</unmanaged>
        /// <unmanaged-short>ver_ICorDebug</unmanaged-short>
        VerICorDebug = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugController</unmanaged>
        /// <unmanaged-short>ver_ICorDebugController</unmanaged-short>
        VerICorDebugController = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugAppDomain</unmanaged>
        /// <unmanaged-short>ver_ICorDebugAppDomain</unmanaged-short>
        VerICorDebugAppDomain = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugAssembly</unmanaged>
        /// <unmanaged-short>ver_ICorDebugAssembly</unmanaged-short>
        VerICorDebugAssembly = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugProcess</unmanaged>
        /// <unmanaged-short>ver_ICorDebugProcess</unmanaged-short>
        VerICorDebugProcess = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugBreakpoint</unmanaged>
        /// <unmanaged-short>ver_ICorDebugBreakpoint</unmanaged-short>
        VerICorDebugBreakpoint = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugFunctionBreakpoint</unmanaged>
        /// <unmanaged-short>ver_ICorDebugFunctionBreakpoint</unmanaged-short>
        VerICorDebugFunctionBreakpoint = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugModuleBreakpoint</unmanaged>
        /// <unmanaged-short>ver_ICorDebugModuleBreakpoint</unmanaged-short>
        VerICorDebugModuleBreakpoint = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugValueBreakpoint</unmanaged>
        /// <unmanaged-short>ver_ICorDebugValueBreakpoint</unmanaged-short>
        VerICorDebugValueBreakpoint = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugStepper</unmanaged>
        /// <unmanaged-short>ver_ICorDebugStepper</unmanaged-short>
        VerICorDebugStepper = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugRegisterSet</unmanaged>
        /// <unmanaged-short>ver_ICorDebugRegisterSet</unmanaged-short>
        VerICorDebugRegisterSet = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugThread</unmanaged>
        /// <unmanaged-short>ver_ICorDebugThread</unmanaged-short>
        VerICorDebugThread = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugChain</unmanaged>
        /// <unmanaged-short>ver_ICorDebugChain</unmanaged-short>
        VerICorDebugChain = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugFrame</unmanaged>
        /// <unmanaged-short>ver_ICorDebugFrame</unmanaged-short>
        VerICorDebugFrame = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugILFrame</unmanaged>
        /// <unmanaged-short>ver_ICorDebugILFrame</unmanaged-short>
        VerICorDebugILFrame = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugNativeFrame</unmanaged>
        /// <unmanaged-short>ver_ICorDebugNativeFrame</unmanaged-short>
        VerICorDebugNativeFrame = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugModule</unmanaged>
        /// <unmanaged-short>ver_ICorDebugModule</unmanaged-short>
        VerICorDebugModule = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugFunction</unmanaged>
        /// <unmanaged-short>ver_ICorDebugFunction</unmanaged-short>
        VerICorDebugFunction = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugCode</unmanaged>
        /// <unmanaged-short>ver_ICorDebugCode</unmanaged-short>
        VerICorDebugCode = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugClass</unmanaged>
        /// <unmanaged-short>ver_ICorDebugClass</unmanaged-short>
        VerICorDebugClass = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugEval</unmanaged>
        /// <unmanaged-short>ver_ICorDebugEval</unmanaged-short>
        VerICorDebugEval = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugValue</unmanaged>
        /// <unmanaged-short>ver_ICorDebugValue</unmanaged-short>
        VerICorDebugValue = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugGenericValue</unmanaged>
        /// <unmanaged-short>ver_ICorDebugGenericValue</unmanaged-short>
        VerICorDebugGenericValue = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugReferenceValue</unmanaged>
        /// <unmanaged-short>ver_ICorDebugReferenceValue</unmanaged-short>
        VerICorDebugReferenceValue = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugHeapValue</unmanaged>
        /// <unmanaged-short>ver_ICorDebugHeapValue</unmanaged-short>
        VerICorDebugHeapValue = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugObjectValue</unmanaged>
        /// <unmanaged-short>ver_ICorDebugObjectValue</unmanaged-short>
        VerICorDebugObjectValue = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugBoxValue</unmanaged>
        /// <unmanaged-short>ver_ICorDebugBoxValue</unmanaged-short>
        VerICorDebugBoxValue = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugStringValue</unmanaged>
        /// <unmanaged-short>ver_ICorDebugStringValue</unmanaged-short>
        VerICorDebugStringValue = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugArrayValue</unmanaged>
        /// <unmanaged-short>ver_ICorDebugArrayValue</unmanaged-short>
        VerICorDebugArrayValue = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugContext</unmanaged>
        /// <unmanaged-short>ver_ICorDebugContext</unmanaged-short>
        VerICorDebugContext = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugEnum</unmanaged>
        /// <unmanaged-short>ver_ICorDebugEnum</unmanaged-short>
        VerICorDebugEnum = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugObjectEnum</unmanaged>
        /// <unmanaged-short>ver_ICorDebugObjectEnum</unmanaged-short>
        VerICorDebugObjectEnum = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugBreakpointEnum</unmanaged>
        /// <unmanaged-short>ver_ICorDebugBreakpointEnum</unmanaged-short>
        VerICorDebugBreakpointEnum = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugStepperEnum</unmanaged>
        /// <unmanaged-short>ver_ICorDebugStepperEnum</unmanaged-short>
        VerICorDebugStepperEnum = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugProcessEnum</unmanaged>
        /// <unmanaged-short>ver_ICorDebugProcessEnum</unmanaged-short>
        VerICorDebugProcessEnum = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugThreadEnum</unmanaged>
        /// <unmanaged-short>ver_ICorDebugThreadEnum</unmanaged-short>
        VerICorDebugThreadEnum = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugFrameEnum</unmanaged>
        /// <unmanaged-short>ver_ICorDebugFrameEnum</unmanaged-short>
        VerICorDebugFrameEnum = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugChainEnum</unmanaged>
        /// <unmanaged-short>ver_ICorDebugChainEnum</unmanaged-short>
        VerICorDebugChainEnum = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugModuleEnum</unmanaged>
        /// <unmanaged-short>ver_ICorDebugModuleEnum</unmanaged-short>
        VerICorDebugModuleEnum = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugValueEnum</unmanaged>
        /// <unmanaged-short>ver_ICorDebugValueEnum</unmanaged-short>
        VerICorDebugValueEnum = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugCodeEnum</unmanaged>
        /// <unmanaged-short>ver_ICorDebugCodeEnum</unmanaged-short>
        VerICorDebugCodeEnum = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugTypeEnum</unmanaged>
        /// <unmanaged-short>ver_ICorDebugTypeEnum</unmanaged-short>
        VerICorDebugTypeEnum = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugErrorInfoEnum</unmanaged>
        /// <unmanaged-short>ver_ICorDebugErrorInfoEnum</unmanaged-short>
        VerICorDebugErrorInfoEnum = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugAppDomainEnum</unmanaged>
        /// <unmanaged-short>ver_ICorDebugAppDomainEnum</unmanaged-short>
        VerICorDebugAppDomainEnum = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugAssemblyEnum</unmanaged>
        /// <unmanaged-short>ver_ICorDebugAssemblyEnum</unmanaged-short>
        VerICorDebugAssemblyEnum = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugEditAndContinueErrorInfo</unmanaged>
        /// <unmanaged-short>ver_ICorDebugEditAndContinueErrorInfo</unmanaged-short>
        VerICorDebugEditAndContinueErrorInfo = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugEditAndContinueSnapshot</unmanaged>
        /// <unmanaged-short>ver_ICorDebugEditAndContinueSnapshot</unmanaged-short>
        VerICorDebugEditAndContinueSnapshot = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorDebugVersion_1_1</unmanaged>
        /// <unmanaged-short>CorDebugVersion_1_1</unmanaged-short>
        CorDebugVersion11 = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorDebugVersion_2_0</unmanaged>
        /// <unmanaged-short>CorDebugVersion_2_0</unmanaged-short>
        CorDebugVersion20 = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugManagedCallback2</unmanaged>
        /// <unmanaged-short>ver_ICorDebugManagedCallback2</unmanaged-short>
        VerICorDebugManagedCallback2 = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugAppDomain2</unmanaged>
        /// <unmanaged-short>ver_ICorDebugAppDomain2</unmanaged-short>
        VerICorDebugAppDomain2 = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugAssembly2</unmanaged>
        /// <unmanaged-short>ver_ICorDebugAssembly2</unmanaged-short>
        VerICorDebugAssembly2 = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugProcess2</unmanaged>
        /// <unmanaged-short>ver_ICorDebugProcess2</unmanaged-short>
        VerICorDebugProcess2 = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugStepper2</unmanaged>
        /// <unmanaged-short>ver_ICorDebugStepper2</unmanaged-short>
        VerICorDebugStepper2 = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugRegisterSet2</unmanaged>
        /// <unmanaged-short>ver_ICorDebugRegisterSet2</unmanaged-short>
        VerICorDebugRegisterSet2 = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugThread2</unmanaged>
        /// <unmanaged-short>ver_ICorDebugThread2</unmanaged-short>
        VerICorDebugThread2 = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugILFrame2</unmanaged>
        /// <unmanaged-short>ver_ICorDebugILFrame2</unmanaged-short>
        VerICorDebugILFrame2 = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugInternalFrame</unmanaged>
        /// <unmanaged-short>ver_ICorDebugInternalFrame</unmanaged-short>
        VerICorDebugInternalFrame = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugModule2</unmanaged>
        /// <unmanaged-short>ver_ICorDebugModule2</unmanaged-short>
        VerICorDebugModule2 = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugFunction2</unmanaged>
        /// <unmanaged-short>ver_ICorDebugFunction2</unmanaged-short>
        VerICorDebugFunction2 = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugCode2</unmanaged>
        /// <unmanaged-short>ver_ICorDebugCode2</unmanaged-short>
        VerICorDebugCode2 = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugClass2</unmanaged>
        /// <unmanaged-short>ver_ICorDebugClass2</unmanaged-short>
        VerICorDebugClass2 = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugValue2</unmanaged>
        /// <unmanaged-short>ver_ICorDebugValue2</unmanaged-short>
        VerICorDebugValue2 = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugEval2</unmanaged>
        /// <unmanaged-short>ver_ICorDebugEval2</unmanaged-short>
        VerICorDebugEval2 = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugObjectValue2</unmanaged>
        /// <unmanaged-short>ver_ICorDebugObjectValue2</unmanaged-short>
        VerICorDebugObjectValue2 = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorDebugVersion_4_0</unmanaged>
        /// <unmanaged-short>CorDebugVersion_4_0</unmanaged-short>
        CorDebugVersion40 = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugThread3</unmanaged>
        /// <unmanaged-short>ver_ICorDebugThread3</unmanaged-short>
        VerICorDebugThread3 = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugThread4</unmanaged>
        /// <unmanaged-short>ver_ICorDebugThread4</unmanaged-short>
        VerICorDebugThread4 = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugStackWalk</unmanaged>
        /// <unmanaged-short>ver_ICorDebugStackWalk</unmanaged-short>
        VerICorDebugStackWalk = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugNativeFrame2</unmanaged>
        /// <unmanaged-short>ver_ICorDebugNativeFrame2</unmanaged-short>
        VerICorDebugNativeFrame2 = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugInternalFrame2</unmanaged>
        /// <unmanaged-short>ver_ICorDebugInternalFrame2</unmanaged-short>
        VerICorDebugInternalFrame2 = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugRuntimeUnwindableFrame</unmanaged>
        /// <unmanaged-short>ver_ICorDebugRuntimeUnwindableFrame</unmanaged-short>
        VerICorDebugRuntimeUnwindableFrame = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugHeapValue3</unmanaged>
        /// <unmanaged-short>ver_ICorDebugHeapValue3</unmanaged-short>
        VerICorDebugHeapValue3 = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugBlockingObjectEnum</unmanaged>
        /// <unmanaged-short>ver_ICorDebugBlockingObjectEnum</unmanaged-short>
        VerICorDebugBlockingObjectEnum = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugValue3</unmanaged>
        /// <unmanaged-short>ver_ICorDebugValue3</unmanaged-short>
        VerICorDebugValue3 = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorDebugVersion_4_5</unmanaged>
        /// <unmanaged-short>CorDebugVersion_4_5</unmanaged-short>
        CorDebugVersion45 = unchecked ((System.Int32)(5)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugComObjectValue</unmanaged>
        /// <unmanaged-short>ver_ICorDebugComObjectValue</unmanaged-short>
        VerICorDebugComObjectValue = unchecked ((System.Int32)(5)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugAppDomain3</unmanaged>
        /// <unmanaged-short>ver_ICorDebugAppDomain3</unmanaged-short>
        VerICorDebugAppDomain3 = unchecked ((System.Int32)(5)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugCode3</unmanaged>
        /// <unmanaged-short>ver_ICorDebugCode3</unmanaged-short>
        VerICorDebugCode3 = unchecked ((System.Int32)(5)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ver_ICorDebugILFrame3</unmanaged>
        /// <unmanaged-short>ver_ICorDebugILFrame3</unmanaged-short>
        VerICorDebugILFrame3 = unchecked ((System.Int32)(5)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorDebugLatestVersion</unmanaged>
        /// <unmanaged-short>CorDebugLatestVersion</unmanaged-short>
        CorDebugLatestVersion = unchecked ((System.Int32)(5))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugInternalFrameType</unmanaged>
    /// <unmanaged-short>CorDebugInternalFrameType</unmanaged-short>
    public enum CorDebugInternalFrameType : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>STUBFRAME_NONE</unmanaged>
        /// <unmanaged-short>STUBFRAME_NONE</unmanaged-short>
        StubframeNone = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>STUBFRAME_M2U</unmanaged>
        /// <unmanaged-short>STUBFRAME_M2U</unmanaged-short>
        StubframeM2U = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>STUBFRAME_U2M</unmanaged>
        /// <unmanaged-short>STUBFRAME_U2M</unmanaged-short>
        StubframeU2M = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>STUBFRAME_APPDOMAIN_TRANSITION</unmanaged>
        /// <unmanaged-short>STUBFRAME_APPDOMAIN_TRANSITION</unmanaged-short>
        StubframeAppdomainTransition = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>STUBFRAME_LIGHTWEIGHT_FUNCTION</unmanaged>
        /// <unmanaged-short>STUBFRAME_LIGHTWEIGHT_FUNCTION</unmanaged-short>
        StubframeLightweightFunction = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>STUBFRAME_FUNC_EVAL</unmanaged>
        /// <unmanaged-short>STUBFRAME_FUNC_EVAL</unmanaged-short>
        StubframeFuncEval = unchecked ((System.Int32)(5)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>STUBFRAME_INTERNALCALL</unmanaged>
        /// <unmanaged-short>STUBFRAME_INTERNALCALL</unmanaged-short>
        StubframeInternalcall = unchecked ((System.Int32)(6)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>STUBFRAME_CLASS_INIT</unmanaged>
        /// <unmanaged-short>STUBFRAME_CLASS_INIT</unmanaged-short>
        StubframeClassInit = unchecked ((System.Int32)(7)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>STUBFRAME_EXCEPTION</unmanaged>
        /// <unmanaged-short>STUBFRAME_EXCEPTION</unmanaged-short>
        StubframeException = unchecked ((System.Int32)(8)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>STUBFRAME_SECURITY</unmanaged>
        /// <unmanaged-short>STUBFRAME_SECURITY</unmanaged-short>
        StubframeSecurity = unchecked ((System.Int32)(9)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>STUBFRAME_JIT_COMPILATION</unmanaged>
        /// <unmanaged-short>STUBFRAME_JIT_COMPILATION</unmanaged-short>
        StubframeJitCompilation = unchecked ((System.Int32)(10))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugJITCompilerFlags</unmanaged>
    /// <unmanaged-short>CorDebugJITCompilerFlags</unmanaged-short>
    public enum CorDebugJITCompilerFlags : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CORDEBUG_JIT_DEFAULT</unmanaged>
        /// <unmanaged-short>CORDEBUG_JIT_DEFAULT</unmanaged-short>
        CordebugJitDefault = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CORDEBUG_JIT_DISABLE_OPTIMIZATION</unmanaged>
        /// <unmanaged-short>CORDEBUG_JIT_DISABLE_OPTIMIZATION</unmanaged-short>
        CordebugJitDisableOptimization = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CORDEBUG_JIT_ENABLE_ENC</unmanaged>
        /// <unmanaged-short>CORDEBUG_JIT_ENABLE_ENC</unmanaged-short>
        CordebugJitEnableEnc = unchecked ((System.Int32)(7))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugJITCompilerFlagsDecprecated</unmanaged>
    /// <unmanaged-short>CorDebugJITCompilerFlagsDecprecated</unmanaged-short>
    public enum CorDebugJITCompilerFlagsDecprecated : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CORDEBUG_JIT_TRACK_DEBUG_INFO</unmanaged>
        /// <unmanaged-short>CORDEBUG_JIT_TRACK_DEBUG_INFO</unmanaged-short>
        CordebugJitTrackDebugInfo = unchecked ((System.Int32)(1))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugMappingResult</unmanaged>
    /// <unmanaged-short>CorDebugMappingResult</unmanaged-short>
    public enum CorDebugMappingResult : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MAPPING_PROLOG</unmanaged>
        /// <unmanaged-short>MAPPING_PROLOG</unmanaged-short>
        MappingProlog = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MAPPING_EPILOG</unmanaged>
        /// <unmanaged-short>MAPPING_EPILOG</unmanaged-short>
        MappingEpilog = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MAPPING_NO_INFO</unmanaged>
        /// <unmanaged-short>MAPPING_NO_INFO</unmanaged-short>
        MappingNoInfo = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MAPPING_UNMAPPED_ADDRESS</unmanaged>
        /// <unmanaged-short>MAPPING_UNMAPPED_ADDRESS</unmanaged-short>
        MappingUnmappedAddress = unchecked ((System.Int32)(8)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MAPPING_EXACT</unmanaged>
        /// <unmanaged-short>MAPPING_EXACT</unmanaged-short>
        MappingExact = unchecked ((System.Int32)(16)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MAPPING_APPROXIMATE</unmanaged>
        /// <unmanaged-short>MAPPING_APPROXIMATE</unmanaged-short>
        MappingApproximate = unchecked ((System.Int32)(32))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugMDAFlags</unmanaged>
    /// <unmanaged-short>CorDebugMDAFlags</unmanaged-short>
    public enum CorDebugMDAFlags : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDA_FLAG_SLIP</unmanaged>
        /// <unmanaged-short>MDA_FLAG_SLIP</unmanaged-short>
        MdaFlagSlip = unchecked ((System.Int32)(2))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugNGENPolicy</unmanaged>
    /// <unmanaged-short>CorDebugNGENPolicy</unmanaged-short>
    public enum CorDebugNGENPolicy : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>DISABLE_LOCAL_NIC</unmanaged>
        /// <unmanaged-short>DISABLE_LOCAL_NIC</unmanaged-short>
        DisableLocalNic = unchecked ((System.Int32)(1))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugPlatform</unmanaged>
    /// <unmanaged-short>CorDebugPlatform</unmanaged-short>
    public enum CorDebugPlatform : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CORDB_PLATFORM_WINDOWS_X86</unmanaged>
        /// <unmanaged-short>CORDB_PLATFORM_WINDOWS_X86</unmanaged-short>
        CordbPlatformWindowsX86 = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CORDB_PLATFORM_WINDOWS_AMD64</unmanaged>
        /// <unmanaged-short>CORDB_PLATFORM_WINDOWS_AMD64</unmanaged-short>
        CordbPlatformWindowsAmd64 = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CORDB_PLATFORM_WINDOWS_IA64</unmanaged>
        /// <unmanaged-short>CORDB_PLATFORM_WINDOWS_IA64</unmanaged-short>
        CordbPlatformWindowsIa64 = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CORDB_PLATFORM_MAC_PPC</unmanaged>
        /// <unmanaged-short>CORDB_PLATFORM_MAC_PPC</unmanaged-short>
        CordbPlatformMacPpc = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CORDB_PLATFORM_MAC_X86</unmanaged>
        /// <unmanaged-short>CORDB_PLATFORM_MAC_X86</unmanaged-short>
        CordbPlatformMacX86 = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CORDB_PLATFORM_WINDOWS_ARM</unmanaged>
        /// <unmanaged-short>CORDB_PLATFORM_WINDOWS_ARM</unmanaged-short>
        CordbPlatformWindowsArm = unchecked ((System.Int32)(5)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CORDB_PLATFORM_MAC_AMD64</unmanaged>
        /// <unmanaged-short>CORDB_PLATFORM_MAC_AMD64</unmanaged-short>
        CordbPlatformMacAmd64 = unchecked ((System.Int32)(6)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CORDB_PLATFORM_WINDOWS_ARM64</unmanaged>
        /// <unmanaged-short>CORDB_PLATFORM_WINDOWS_ARM64</unmanaged-short>
        CordbPlatformWindowsArm64 = unchecked ((System.Int32)(7))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugRecordFormat</unmanaged>
    /// <unmanaged-short>CorDebugRecordFormat</unmanaged-short>
    public enum CorDebugRecordFormat : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>FORMAT_WINDOWS_EXCEPTIONRECORD32</unmanaged>
        /// <unmanaged-short>FORMAT_WINDOWS_EXCEPTIONRECORD32</unmanaged-short>
        FormatWindowsExceptionrecord32 = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>FORMAT_WINDOWS_EXCEPTIONRECORD64</unmanaged>
        /// <unmanaged-short>FORMAT_WINDOWS_EXCEPTIONRECORD64</unmanaged-short>
        FormatWindowsExceptionrecord64 = unchecked ((System.Int32)(2))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugRegister</unmanaged>
    /// <unmanaged-short>CorDebugRegister</unmanaged-short>
    public enum CorDebugRegister : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_INSTRUCTION_POINTER</unmanaged>
        /// <unmanaged-short>REGISTER_INSTRUCTION_POINTER</unmanaged-short>
        RegisterInstructionPointer = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_STACK_POINTER</unmanaged>
        /// <unmanaged-short>REGISTER_STACK_POINTER</unmanaged-short>
        RegisterStackPointer = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_FRAME_POINTER</unmanaged>
        /// <unmanaged-short>REGISTER_FRAME_POINTER</unmanaged-short>
        RegisterFramePointer = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_X86_EIP</unmanaged>
        /// <unmanaged-short>REGISTER_X86_EIP</unmanaged-short>
        RegisterX86Eip = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_X86_ESP</unmanaged>
        /// <unmanaged-short>REGISTER_X86_ESP</unmanaged-short>
        RegisterX86Esp = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_X86_EBP</unmanaged>
        /// <unmanaged-short>REGISTER_X86_EBP</unmanaged-short>
        RegisterX86Ebp = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_X86_EAX</unmanaged>
        /// <unmanaged-short>REGISTER_X86_EAX</unmanaged-short>
        RegisterX86Eax = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_X86_ECX</unmanaged>
        /// <unmanaged-short>REGISTER_X86_ECX</unmanaged-short>
        RegisterX86Ecx = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_X86_EDX</unmanaged>
        /// <unmanaged-short>REGISTER_X86_EDX</unmanaged-short>
        RegisterX86Edx = unchecked ((System.Int32)(5)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_X86_EBX</unmanaged>
        /// <unmanaged-short>REGISTER_X86_EBX</unmanaged-short>
        RegisterX86Ebx = unchecked ((System.Int32)(6)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_X86_ESI</unmanaged>
        /// <unmanaged-short>REGISTER_X86_ESI</unmanaged-short>
        RegisterX86Esi = unchecked ((System.Int32)(7)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_X86_EDI</unmanaged>
        /// <unmanaged-short>REGISTER_X86_EDI</unmanaged-short>
        RegisterX86Edi = unchecked ((System.Int32)(8)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_X86_FPSTACK_0</unmanaged>
        /// <unmanaged-short>REGISTER_X86_FPSTACK_0</unmanaged-short>
        RegisterX86Fpstack0 = unchecked ((System.Int32)(9)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_X86_FPSTACK_1</unmanaged>
        /// <unmanaged-short>REGISTER_X86_FPSTACK_1</unmanaged-short>
        RegisterX86Fpstack1 = unchecked ((System.Int32)(10)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_X86_FPSTACK_2</unmanaged>
        /// <unmanaged-short>REGISTER_X86_FPSTACK_2</unmanaged-short>
        RegisterX86Fpstack2 = unchecked ((System.Int32)(11)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_X86_FPSTACK_3</unmanaged>
        /// <unmanaged-short>REGISTER_X86_FPSTACK_3</unmanaged-short>
        RegisterX86Fpstack3 = unchecked ((System.Int32)(12)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_X86_FPSTACK_4</unmanaged>
        /// <unmanaged-short>REGISTER_X86_FPSTACK_4</unmanaged-short>
        RegisterX86Fpstack4 = unchecked ((System.Int32)(13)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_X86_FPSTACK_5</unmanaged>
        /// <unmanaged-short>REGISTER_X86_FPSTACK_5</unmanaged-short>
        RegisterX86Fpstack5 = unchecked ((System.Int32)(14)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_X86_FPSTACK_6</unmanaged>
        /// <unmanaged-short>REGISTER_X86_FPSTACK_6</unmanaged-short>
        RegisterX86Fpstack6 = unchecked ((System.Int32)(15)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_X86_FPSTACK_7</unmanaged>
        /// <unmanaged-short>REGISTER_X86_FPSTACK_7</unmanaged-short>
        RegisterX86Fpstack7 = unchecked ((System.Int32)(16)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_AMD64_RIP</unmanaged>
        /// <unmanaged-short>REGISTER_AMD64_RIP</unmanaged-short>
        RegisterAmd64Rip = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_AMD64_RSP</unmanaged>
        /// <unmanaged-short>REGISTER_AMD64_RSP</unmanaged-short>
        RegisterAmd64Rsp = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_AMD64_RBP</unmanaged>
        /// <unmanaged-short>REGISTER_AMD64_RBP</unmanaged-short>
        RegisterAmd64Rbp = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_AMD64_RAX</unmanaged>
        /// <unmanaged-short>REGISTER_AMD64_RAX</unmanaged-short>
        RegisterAmd64Rax = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_AMD64_RCX</unmanaged>
        /// <unmanaged-short>REGISTER_AMD64_RCX</unmanaged-short>
        RegisterAmd64Rcx = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_AMD64_RDX</unmanaged>
        /// <unmanaged-short>REGISTER_AMD64_RDX</unmanaged-short>
        RegisterAmd64Rdx = unchecked ((System.Int32)(5)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_AMD64_RBX</unmanaged>
        /// <unmanaged-short>REGISTER_AMD64_RBX</unmanaged-short>
        RegisterAmd64Rbx = unchecked ((System.Int32)(6)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_AMD64_RSI</unmanaged>
        /// <unmanaged-short>REGISTER_AMD64_RSI</unmanaged-short>
        RegisterAmd64Rsi = unchecked ((System.Int32)(7)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_AMD64_RDI</unmanaged>
        /// <unmanaged-short>REGISTER_AMD64_RDI</unmanaged-short>
        RegisterAmd64Rdi = unchecked ((System.Int32)(8)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_AMD64_R8</unmanaged>
        /// <unmanaged-short>REGISTER_AMD64_R8</unmanaged-short>
        RegisterAmd64R8 = unchecked ((System.Int32)(9)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_AMD64_R9</unmanaged>
        /// <unmanaged-short>REGISTER_AMD64_R9</unmanaged-short>
        RegisterAmd64R9 = unchecked ((System.Int32)(10)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_AMD64_R10</unmanaged>
        /// <unmanaged-short>REGISTER_AMD64_R10</unmanaged-short>
        RegisterAmd64R10 = unchecked ((System.Int32)(11)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_AMD64_R11</unmanaged>
        /// <unmanaged-short>REGISTER_AMD64_R11</unmanaged-short>
        RegisterAmd64R11 = unchecked ((System.Int32)(12)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_AMD64_R12</unmanaged>
        /// <unmanaged-short>REGISTER_AMD64_R12</unmanaged-short>
        RegisterAmd64R12 = unchecked ((System.Int32)(13)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_AMD64_R13</unmanaged>
        /// <unmanaged-short>REGISTER_AMD64_R13</unmanaged-short>
        RegisterAmd64R13 = unchecked ((System.Int32)(14)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_AMD64_R14</unmanaged>
        /// <unmanaged-short>REGISTER_AMD64_R14</unmanaged-short>
        RegisterAmd64R14 = unchecked ((System.Int32)(15)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_AMD64_R15</unmanaged>
        /// <unmanaged-short>REGISTER_AMD64_R15</unmanaged-short>
        RegisterAmd64R15 = unchecked ((System.Int32)(16)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_AMD64_XMM0</unmanaged>
        /// <unmanaged-short>REGISTER_AMD64_XMM0</unmanaged-short>
        RegisterAmd64Xmm0 = unchecked ((System.Int32)(17)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_AMD64_XMM1</unmanaged>
        /// <unmanaged-short>REGISTER_AMD64_XMM1</unmanaged-short>
        RegisterAmd64Xmm1 = unchecked ((System.Int32)(18)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_AMD64_XMM2</unmanaged>
        /// <unmanaged-short>REGISTER_AMD64_XMM2</unmanaged-short>
        RegisterAmd64Xmm2 = unchecked ((System.Int32)(19)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_AMD64_XMM3</unmanaged>
        /// <unmanaged-short>REGISTER_AMD64_XMM3</unmanaged-short>
        RegisterAmd64Xmm3 = unchecked ((System.Int32)(20)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_AMD64_XMM4</unmanaged>
        /// <unmanaged-short>REGISTER_AMD64_XMM4</unmanaged-short>
        RegisterAmd64Xmm4 = unchecked ((System.Int32)(21)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_AMD64_XMM5</unmanaged>
        /// <unmanaged-short>REGISTER_AMD64_XMM5</unmanaged-short>
        RegisterAmd64Xmm5 = unchecked ((System.Int32)(22)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_AMD64_XMM6</unmanaged>
        /// <unmanaged-short>REGISTER_AMD64_XMM6</unmanaged-short>
        RegisterAmd64Xmm6 = unchecked ((System.Int32)(23)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_AMD64_XMM7</unmanaged>
        /// <unmanaged-short>REGISTER_AMD64_XMM7</unmanaged-short>
        RegisterAmd64Xmm7 = unchecked ((System.Int32)(24)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_AMD64_XMM8</unmanaged>
        /// <unmanaged-short>REGISTER_AMD64_XMM8</unmanaged-short>
        RegisterAmd64Xmm8 = unchecked ((System.Int32)(25)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_AMD64_XMM9</unmanaged>
        /// <unmanaged-short>REGISTER_AMD64_XMM9</unmanaged-short>
        RegisterAmd64Xmm9 = unchecked ((System.Int32)(26)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_AMD64_XMM10</unmanaged>
        /// <unmanaged-short>REGISTER_AMD64_XMM10</unmanaged-short>
        RegisterAmd64Xmm10 = unchecked ((System.Int32)(27)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_AMD64_XMM11</unmanaged>
        /// <unmanaged-short>REGISTER_AMD64_XMM11</unmanaged-short>
        RegisterAmd64Xmm11 = unchecked ((System.Int32)(28)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_AMD64_XMM12</unmanaged>
        /// <unmanaged-short>REGISTER_AMD64_XMM12</unmanaged-short>
        RegisterAmd64Xmm12 = unchecked ((System.Int32)(29)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_AMD64_XMM13</unmanaged>
        /// <unmanaged-short>REGISTER_AMD64_XMM13</unmanaged-short>
        RegisterAmd64Xmm13 = unchecked ((System.Int32)(30)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_AMD64_XMM14</unmanaged>
        /// <unmanaged-short>REGISTER_AMD64_XMM14</unmanaged-short>
        RegisterAmd64Xmm14 = unchecked ((System.Int32)(31)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_AMD64_XMM15</unmanaged>
        /// <unmanaged-short>REGISTER_AMD64_XMM15</unmanaged-short>
        RegisterAmd64Xmm15 = unchecked ((System.Int32)(32)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_IA64_BSP</unmanaged>
        /// <unmanaged-short>REGISTER_IA64_BSP</unmanaged-short>
        RegisterIa64Bsp = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_IA64_R0</unmanaged>
        /// <unmanaged-short>REGISTER_IA64_R0</unmanaged-short>
        RegisterIa64R0 = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_IA64_F0</unmanaged>
        /// <unmanaged-short>REGISTER_IA64_F0</unmanaged-short>
        RegisterIa64F0 = unchecked ((System.Int32)(131)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM_PC</unmanaged>
        /// <unmanaged-short>REGISTER_ARM_PC</unmanaged-short>
        RegisterArmPc = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM_SP</unmanaged>
        /// <unmanaged-short>REGISTER_ARM_SP</unmanaged-short>
        RegisterArmSp = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM_R0</unmanaged>
        /// <unmanaged-short>REGISTER_ARM_R0</unmanaged-short>
        RegisterArmR0 = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM_R1</unmanaged>
        /// <unmanaged-short>REGISTER_ARM_R1</unmanaged-short>
        RegisterArmR1 = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM_R2</unmanaged>
        /// <unmanaged-short>REGISTER_ARM_R2</unmanaged-short>
        RegisterArmR2 = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM_R3</unmanaged>
        /// <unmanaged-short>REGISTER_ARM_R3</unmanaged-short>
        RegisterArmR3 = unchecked ((System.Int32)(5)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM_R4</unmanaged>
        /// <unmanaged-short>REGISTER_ARM_R4</unmanaged-short>
        RegisterArmR4 = unchecked ((System.Int32)(6)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM_R5</unmanaged>
        /// <unmanaged-short>REGISTER_ARM_R5</unmanaged-short>
        RegisterArmR5 = unchecked ((System.Int32)(7)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM_R6</unmanaged>
        /// <unmanaged-short>REGISTER_ARM_R6</unmanaged-short>
        RegisterArmR6 = unchecked ((System.Int32)(8)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM_R7</unmanaged>
        /// <unmanaged-short>REGISTER_ARM_R7</unmanaged-short>
        RegisterArmR7 = unchecked ((System.Int32)(9)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM_R8</unmanaged>
        /// <unmanaged-short>REGISTER_ARM_R8</unmanaged-short>
        RegisterArmR8 = unchecked ((System.Int32)(10)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM_R9</unmanaged>
        /// <unmanaged-short>REGISTER_ARM_R9</unmanaged-short>
        RegisterArmR9 = unchecked ((System.Int32)(11)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM_R10</unmanaged>
        /// <unmanaged-short>REGISTER_ARM_R10</unmanaged-short>
        RegisterArmR10 = unchecked ((System.Int32)(12)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM_R11</unmanaged>
        /// <unmanaged-short>REGISTER_ARM_R11</unmanaged-short>
        RegisterArmR11 = unchecked ((System.Int32)(13)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM_R12</unmanaged>
        /// <unmanaged-short>REGISTER_ARM_R12</unmanaged-short>
        RegisterArmR12 = unchecked ((System.Int32)(14)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM_LR</unmanaged>
        /// <unmanaged-short>REGISTER_ARM_LR</unmanaged-short>
        RegisterArmLr = unchecked ((System.Int32)(15)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM64_PC</unmanaged>
        /// <unmanaged-short>REGISTER_ARM64_PC</unmanaged-short>
        RegisterArm64Pc = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM64_SP</unmanaged>
        /// <unmanaged-short>REGISTER_ARM64_SP</unmanaged-short>
        RegisterArm64Sp = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM64_FP</unmanaged>
        /// <unmanaged-short>REGISTER_ARM64_FP</unmanaged-short>
        RegisterArm64Fp = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM64_X0</unmanaged>
        /// <unmanaged-short>REGISTER_ARM64_X0</unmanaged-short>
        RegisterArm64X0 = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM64_X1</unmanaged>
        /// <unmanaged-short>REGISTER_ARM64_X1</unmanaged-short>
        RegisterArm64X1 = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM64_X2</unmanaged>
        /// <unmanaged-short>REGISTER_ARM64_X2</unmanaged-short>
        RegisterArm64X2 = unchecked ((System.Int32)(5)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM64_X3</unmanaged>
        /// <unmanaged-short>REGISTER_ARM64_X3</unmanaged-short>
        RegisterArm64X3 = unchecked ((System.Int32)(6)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM64_X4</unmanaged>
        /// <unmanaged-short>REGISTER_ARM64_X4</unmanaged-short>
        RegisterArm64X4 = unchecked ((System.Int32)(7)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM64_X5</unmanaged>
        /// <unmanaged-short>REGISTER_ARM64_X5</unmanaged-short>
        RegisterArm64X5 = unchecked ((System.Int32)(8)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM64_X6</unmanaged>
        /// <unmanaged-short>REGISTER_ARM64_X6</unmanaged-short>
        RegisterArm64X6 = unchecked ((System.Int32)(9)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM64_X7</unmanaged>
        /// <unmanaged-short>REGISTER_ARM64_X7</unmanaged-short>
        RegisterArm64X7 = unchecked ((System.Int32)(10)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM64_X8</unmanaged>
        /// <unmanaged-short>REGISTER_ARM64_X8</unmanaged-short>
        RegisterArm64X8 = unchecked ((System.Int32)(11)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM64_X9</unmanaged>
        /// <unmanaged-short>REGISTER_ARM64_X9</unmanaged-short>
        RegisterArm64X9 = unchecked ((System.Int32)(12)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM64_X10</unmanaged>
        /// <unmanaged-short>REGISTER_ARM64_X10</unmanaged-short>
        RegisterArm64X10 = unchecked ((System.Int32)(13)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM64_X11</unmanaged>
        /// <unmanaged-short>REGISTER_ARM64_X11</unmanaged-short>
        RegisterArm64X11 = unchecked ((System.Int32)(14)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM64_X12</unmanaged>
        /// <unmanaged-short>REGISTER_ARM64_X12</unmanaged-short>
        RegisterArm64X12 = unchecked ((System.Int32)(15)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM64_X13</unmanaged>
        /// <unmanaged-short>REGISTER_ARM64_X13</unmanaged-short>
        RegisterArm64X13 = unchecked ((System.Int32)(16)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM64_X14</unmanaged>
        /// <unmanaged-short>REGISTER_ARM64_X14</unmanaged-short>
        RegisterArm64X14 = unchecked ((System.Int32)(17)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM64_X15</unmanaged>
        /// <unmanaged-short>REGISTER_ARM64_X15</unmanaged-short>
        RegisterArm64X15 = unchecked ((System.Int32)(18)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM64_X16</unmanaged>
        /// <unmanaged-short>REGISTER_ARM64_X16</unmanaged-short>
        RegisterArm64X16 = unchecked ((System.Int32)(19)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM64_X17</unmanaged>
        /// <unmanaged-short>REGISTER_ARM64_X17</unmanaged-short>
        RegisterArm64X17 = unchecked ((System.Int32)(20)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM64_X18</unmanaged>
        /// <unmanaged-short>REGISTER_ARM64_X18</unmanaged-short>
        RegisterArm64X18 = unchecked ((System.Int32)(21)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM64_X19</unmanaged>
        /// <unmanaged-short>REGISTER_ARM64_X19</unmanaged-short>
        RegisterArm64X19 = unchecked ((System.Int32)(22)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM64_X20</unmanaged>
        /// <unmanaged-short>REGISTER_ARM64_X20</unmanaged-short>
        RegisterArm64X20 = unchecked ((System.Int32)(23)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM64_X21</unmanaged>
        /// <unmanaged-short>REGISTER_ARM64_X21</unmanaged-short>
        RegisterArm64X21 = unchecked ((System.Int32)(24)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM64_X22</unmanaged>
        /// <unmanaged-short>REGISTER_ARM64_X22</unmanaged-short>
        RegisterArm64X22 = unchecked ((System.Int32)(25)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM64_X23</unmanaged>
        /// <unmanaged-short>REGISTER_ARM64_X23</unmanaged-short>
        RegisterArm64X23 = unchecked ((System.Int32)(26)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM64_X24</unmanaged>
        /// <unmanaged-short>REGISTER_ARM64_X24</unmanaged-short>
        RegisterArm64X24 = unchecked ((System.Int32)(27)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM64_X25</unmanaged>
        /// <unmanaged-short>REGISTER_ARM64_X25</unmanaged-short>
        RegisterArm64X25 = unchecked ((System.Int32)(28)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM64_X26</unmanaged>
        /// <unmanaged-short>REGISTER_ARM64_X26</unmanaged-short>
        RegisterArm64X26 = unchecked ((System.Int32)(29)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM64_X27</unmanaged>
        /// <unmanaged-short>REGISTER_ARM64_X27</unmanaged-short>
        RegisterArm64X27 = unchecked ((System.Int32)(30)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM64_X28</unmanaged>
        /// <unmanaged-short>REGISTER_ARM64_X28</unmanaged-short>
        RegisterArm64X28 = unchecked ((System.Int32)(31)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>REGISTER_ARM64_LR</unmanaged>
        /// <unmanaged-short>REGISTER_ARM64_LR</unmanaged-short>
        RegisterArm64Lr = unchecked ((System.Int32)(32))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugSetContextFlag</unmanaged>
    /// <unmanaged-short>CorDebugSetContextFlag</unmanaged-short>
    public enum CorDebugSetContextFlag : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>SET_CONTEXT_FLAG_ACTIVE_FRAME</unmanaged>
        /// <unmanaged-short>SET_CONTEXT_FLAG_ACTIVE_FRAME</unmanaged-short>
        SetContextFlagActiveFrame = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>SET_CONTEXT_FLAG_UNWIND_FRAME</unmanaged>
        /// <unmanaged-short>SET_CONTEXT_FLAG_UNWIND_FRAME</unmanaged-short>
        SetContextFlagUnwindFrame = unchecked ((System.Int32)(2))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugStateChange</unmanaged>
    /// <unmanaged-short>CorDebugStateChange</unmanaged-short>
    public enum CorDebugStateChange : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>PROCESS_RUNNING</unmanaged>
        /// <unmanaged-short>PROCESS_RUNNING</unmanaged-short>
        ProcessRunning = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>FLUSH_ALL</unmanaged>
        /// <unmanaged-short>FLUSH_ALL</unmanaged-short>
        FlushAll = unchecked ((System.Int32)(2))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugStepReason</unmanaged>
    /// <unmanaged-short>CorDebugStepReason</unmanaged-short>
    public enum CorDebugStepReason : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>STEP_NORMAL</unmanaged>
        /// <unmanaged-short>STEP_NORMAL</unmanaged-short>
        StepNormal = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>STEP_RETURN</unmanaged>
        /// <unmanaged-short>STEP_RETURN</unmanaged-short>
        StepReturn = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>STEP_CALL</unmanaged>
        /// <unmanaged-short>STEP_CALL</unmanaged-short>
        StepCall = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>STEP_EXCEPTION_FILTER</unmanaged>
        /// <unmanaged-short>STEP_EXCEPTION_FILTER</unmanaged-short>
        StepExceptionFilter = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>STEP_EXCEPTION_HANDLER</unmanaged>
        /// <unmanaged-short>STEP_EXCEPTION_HANDLER</unmanaged-short>
        StepExceptionHandler = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>STEP_INTERCEPT</unmanaged>
        /// <unmanaged-short>STEP_INTERCEPT</unmanaged-short>
        StepIntercept = unchecked ((System.Int32)(5)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>STEP_EXIT</unmanaged>
        /// <unmanaged-short>STEP_EXIT</unmanaged-short>
        StepExit = unchecked ((System.Int32)(6))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugThreadState</unmanaged>
    /// <unmanaged-short>CorDebugThreadState</unmanaged-short>
    public enum CorDebugThreadState : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>THREAD_RUN</unmanaged>
        /// <unmanaged-short>THREAD_RUN</unmanaged-short>
        ThreadRun = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>THREAD_SUSPEND</unmanaged>
        /// <unmanaged-short>THREAD_SUSPEND</unmanaged-short>
        ThreadSuspend = unchecked ((System.Int32)(1))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugUnmappedStop</unmanaged>
    /// <unmanaged-short>CorDebugUnmappedStop</unmanaged-short>
    public enum CorDebugUnmappedStop : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>STOP_NONE</unmanaged>
        /// <unmanaged-short>STOP_NONE</unmanaged-short>
        StopNone = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>STOP_PROLOG</unmanaged>
        /// <unmanaged-short>STOP_PROLOG</unmanaged-short>
        StopProlog = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>STOP_EPILOG</unmanaged>
        /// <unmanaged-short>STOP_EPILOG</unmanaged-short>
        StopEpilog = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>STOP_NO_MAPPING_INFO</unmanaged>
        /// <unmanaged-short>STOP_NO_MAPPING_INFO</unmanaged-short>
        StopNoMappingInfo = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>STOP_OTHER_UNMAPPED</unmanaged>
        /// <unmanaged-short>STOP_OTHER_UNMAPPED</unmanaged-short>
        StopOtherUnmapped = unchecked ((System.Int32)(8)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>STOP_UNMANAGED</unmanaged>
        /// <unmanaged-short>STOP_UNMANAGED</unmanaged-short>
        StopUnmanaged = unchecked ((System.Int32)(16)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>STOP_ALL</unmanaged>
        /// <unmanaged-short>STOP_ALL</unmanaged-short>
        StopAll = unchecked ((System.Int32)(65535))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugUserState</unmanaged>
    /// <unmanaged-short>CorDebugUserState</unmanaged-short>
    public enum CorDebugUserState : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>USER_STOP_REQUESTED</unmanaged>
        /// <unmanaged-short>USER_STOP_REQUESTED</unmanaged-short>
        UserStopRequested = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>USER_SUSPEND_REQUESTED</unmanaged>
        /// <unmanaged-short>USER_SUSPEND_REQUESTED</unmanaged-short>
        UserSuspendRequested = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>USER_BACKGROUND</unmanaged>
        /// <unmanaged-short>USER_BACKGROUND</unmanaged-short>
        UserBackground = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>USER_UNSTARTED</unmanaged>
        /// <unmanaged-short>USER_UNSTARTED</unmanaged-short>
        UserUnstarted = unchecked ((System.Int32)(8)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>USER_STOPPED</unmanaged>
        /// <unmanaged-short>USER_STOPPED</unmanaged-short>
        UserStopped = unchecked ((System.Int32)(16)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>USER_WAIT_SLEEP_JOIN</unmanaged>
        /// <unmanaged-short>USER_WAIT_SLEEP_JOIN</unmanaged-short>
        UserWaitSleepJoin = unchecked ((System.Int32)(32)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>USER_SUSPENDED</unmanaged>
        /// <unmanaged-short>USER_SUSPENDED</unmanaged-short>
        UserSuspended = unchecked ((System.Int32)(64)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>USER_UNSAFE_POINT</unmanaged>
        /// <unmanaged-short>USER_UNSAFE_POINT</unmanaged-short>
        UserUnsafePoint = unchecked ((System.Int32)(128)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>USER_THREADPOOL</unmanaged>
        /// <unmanaged-short>USER_THREADPOOL</unmanaged-short>
        UserThreadpool = unchecked ((System.Int32)(256))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDeclSecurity</unmanaged>
    /// <unmanaged-short>CorDeclSecurity</unmanaged-short>
    public enum CorDeclSecurity : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>dclActionMask</unmanaged>
        /// <unmanaged-short>dclActionMask</unmanaged-short>
        DclActionMask = unchecked ((System.Int32)(31)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>dclActionNil</unmanaged>
        /// <unmanaged-short>dclActionNil</unmanaged-short>
        DclActionNil = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>dclRequest</unmanaged>
        /// <unmanaged-short>dclRequest</unmanaged-short>
        DclRequest = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>dclDemand</unmanaged>
        /// <unmanaged-short>dclDemand</unmanaged-short>
        DclDemand = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>dclAssert</unmanaged>
        /// <unmanaged-short>dclAssert</unmanaged-short>
        DclAssert = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>dclDeny</unmanaged>
        /// <unmanaged-short>dclDeny</unmanaged-short>
        DclDeny = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>dclPermitOnly</unmanaged>
        /// <unmanaged-short>dclPermitOnly</unmanaged-short>
        DclPermitOnly = unchecked ((System.Int32)(5)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>dclLinktimeCheck</unmanaged>
        /// <unmanaged-short>dclLinktimeCheck</unmanaged-short>
        DclLinktimeCheck = unchecked ((System.Int32)(6)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>dclInheritanceCheck</unmanaged>
        /// <unmanaged-short>dclInheritanceCheck</unmanaged-short>
        DclInheritanceCheck = unchecked ((System.Int32)(7)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>dclRequestMinimum</unmanaged>
        /// <unmanaged-short>dclRequestMinimum</unmanaged-short>
        DclRequestMinimum = unchecked ((System.Int32)(8)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>dclRequestOptional</unmanaged>
        /// <unmanaged-short>dclRequestOptional</unmanaged-short>
        DclRequestOptional = unchecked ((System.Int32)(9)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>dclRequestRefuse</unmanaged>
        /// <unmanaged-short>dclRequestRefuse</unmanaged-short>
        DclRequestRefuse = unchecked ((System.Int32)(10)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>dclPrejitGrant</unmanaged>
        /// <unmanaged-short>dclPrejitGrant</unmanaged-short>
        DclPrejitGrant = unchecked ((System.Int32)(11)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>dclPrejitDenied</unmanaged>
        /// <unmanaged-short>dclPrejitDenied</unmanaged-short>
        DclPrejitDenied = unchecked ((System.Int32)(12)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>dclNonCasDemand</unmanaged>
        /// <unmanaged-short>dclNonCasDemand</unmanaged-short>
        DclNonCasDemand = unchecked ((System.Int32)(13)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>dclNonCasLinkDemand</unmanaged>
        /// <unmanaged-short>dclNonCasLinkDemand</unmanaged-short>
        DclNonCasLinkDemand = unchecked ((System.Int32)(14)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>dclNonCasInheritance</unmanaged>
        /// <unmanaged-short>dclNonCasInheritance</unmanaged-short>
        DclNonCasInheritance = unchecked ((System.Int32)(15)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>dclMaximumValue</unmanaged>
        /// <unmanaged-short>dclMaximumValue</unmanaged-short>
        DclMaximumValue = unchecked ((System.Int32)(15))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorElementType</unmanaged>
    /// <unmanaged-short>CorElementType</unmanaged-short>
    public enum CorElementType : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_END</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_END</unmanaged-short>
        ElementTypeEnd = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_VOID</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_VOID</unmanaged-short>
        ElementTypeVoid = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_BOOLEAN</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_BOOLEAN</unmanaged-short>
        ElementTypeBoolean = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_CHAR</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_CHAR</unmanaged-short>
        ElementTypeChar = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_I1</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_I1</unmanaged-short>
        ElementTypeI1 = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_U1</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_U1</unmanaged-short>
        ElementTypeU1 = unchecked ((System.Int32)(5)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_I2</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_I2</unmanaged-short>
        ElementTypeI2 = unchecked ((System.Int32)(6)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_U2</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_U2</unmanaged-short>
        ElementTypeU2 = unchecked ((System.Int32)(7)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_I4</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_I4</unmanaged-short>
        ElementTypeI4 = unchecked ((System.Int32)(8)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_U4</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_U4</unmanaged-short>
        ElementTypeU4 = unchecked ((System.Int32)(9)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_I8</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_I8</unmanaged-short>
        ElementTypeI8 = unchecked ((System.Int32)(10)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_U8</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_U8</unmanaged-short>
        ElementTypeU8 = unchecked ((System.Int32)(11)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_R4</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_R4</unmanaged-short>
        ElementTypeR4 = unchecked ((System.Int32)(12)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_R8</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_R8</unmanaged-short>
        ElementTypeR8 = unchecked ((System.Int32)(13)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_STRING</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_STRING</unmanaged-short>
        ElementTypeString = unchecked ((System.Int32)(14)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_PTR</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_PTR</unmanaged-short>
        ElementTypePtr = unchecked ((System.Int32)(15)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_BYREF</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_BYREF</unmanaged-short>
        ElementTypeByref = unchecked ((System.Int32)(16)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_VALUETYPE</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_VALUETYPE</unmanaged-short>
        ElementTypeValuetype = unchecked ((System.Int32)(17)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_CLASS</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_CLASS</unmanaged-short>
        ElementTypeClass = unchecked ((System.Int32)(18)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_VAR</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_VAR</unmanaged-short>
        ElementTypeVar = unchecked ((System.Int32)(19)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_ARRAY</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_ARRAY</unmanaged-short>
        ElementTypeArray = unchecked ((System.Int32)(20)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_GENERICINST</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_GENERICINST</unmanaged-short>
        ElementTypeGenericinst = unchecked ((System.Int32)(21)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_TYPEDBYREF</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_TYPEDBYREF</unmanaged-short>
        ElementTypeTypedbyref = unchecked ((System.Int32)(22)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_I</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_I</unmanaged-short>
        ElementTypeI = unchecked ((System.Int32)(24)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_U</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_U</unmanaged-short>
        ElementTypeU = unchecked ((System.Int32)(25)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_FNPTR</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_FNPTR</unmanaged-short>
        ElementTypeFnptr = unchecked ((System.Int32)(27)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_OBJECT</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_OBJECT</unmanaged-short>
        ElementTypeObject = unchecked ((System.Int32)(28)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_SZARRAY</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_SZARRAY</unmanaged-short>
        ElementTypeSzarray = unchecked ((System.Int32)(29)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_MVAR</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_MVAR</unmanaged-short>
        ElementTypeMvar = unchecked ((System.Int32)(30)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_CMOD_REQD</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_CMOD_REQD</unmanaged-short>
        ElementTypeCmodReqd = unchecked ((System.Int32)(31)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_CMOD_OPT</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_CMOD_OPT</unmanaged-short>
        ElementTypeCmodOpt = unchecked ((System.Int32)(32)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_INTERNAL</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_INTERNAL</unmanaged-short>
        ElementTypeInternal = unchecked ((System.Int32)(33)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_MAX</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_MAX</unmanaged-short>
        ElementTypeMax = unchecked ((System.Int32)(34)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_MODIFIER</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_MODIFIER</unmanaged-short>
        ElementTypeModifier = unchecked ((System.Int32)(64)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_SENTINEL</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_SENTINEL</unmanaged-short>
        ElementTypeSentinel = unchecked ((System.Int32)(65)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ELEMENT_TYPE_PINNED</unmanaged>
        /// <unmanaged-short>ELEMENT_TYPE_PINNED</unmanaged-short>
        ElementTypePinned = unchecked ((System.Int32)(69))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>COR_ENUM_0</unmanaged>
    /// <unmanaged-short>COR_ENUM_0</unmanaged-short>
    public enum CorEnum0 : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>SIGN_MASK_ONEBYTE</unmanaged>
        /// <unmanaged-short>SIGN_MASK_ONEBYTE</unmanaged-short>
        SignMaskOnebyte = unchecked ((System.Int32)(-64)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>SIGN_MASK_TWOBYTE</unmanaged>
        /// <unmanaged-short>SIGN_MASK_TWOBYTE</unmanaged-short>
        SignMaskTwobyte = unchecked ((System.Int32)(-8192)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>SIGN_MASK_FOURBYTE</unmanaged>
        /// <unmanaged-short>SIGN_MASK_FOURBYTE</unmanaged-short>
        SignMaskFourbyte = unchecked ((System.Int32)(-268435456))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorErrorIfEmitOutOfOrder</unmanaged>
    /// <unmanaged-short>CorErrorIfEmitOutOfOrder</unmanaged-short>
    public enum CorErrorIfEmitOutOfOrder : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDErrorOutOfOrderDefault</unmanaged>
        /// <unmanaged-short>MDErrorOutOfOrderDefault</unmanaged-short>
        MDErrorOutOfOrderDefault = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDErrorOutOfOrderNone</unmanaged>
        /// <unmanaged-short>MDErrorOutOfOrderNone</unmanaged-short>
        MDErrorOutOfOrderNone = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDErrorOutOfOrderAll</unmanaged>
        /// <unmanaged-short>MDErrorOutOfOrderAll</unmanaged-short>
        MDErrorOutOfOrderAll = unchecked ((System.Int32)(-1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDMethodOutOfOrder</unmanaged>
        /// <unmanaged-short>MDMethodOutOfOrder</unmanaged-short>
        MDMethodOutOfOrder = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDFieldOutOfOrder</unmanaged>
        /// <unmanaged-short>MDFieldOutOfOrder</unmanaged-short>
        MDFieldOutOfOrder = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDParamOutOfOrder</unmanaged>
        /// <unmanaged-short>MDParamOutOfOrder</unmanaged-short>
        MDParamOutOfOrder = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDPropertyOutOfOrder</unmanaged>
        /// <unmanaged-short>MDPropertyOutOfOrder</unmanaged-short>
        MDPropertyOutOfOrder = unchecked ((System.Int32)(8)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDEventOutOfOrder</unmanaged>
        /// <unmanaged-short>MDEventOutOfOrder</unmanaged-short>
        MDEventOutOfOrder = unchecked ((System.Int32)(16))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorEventAttr</unmanaged>
    /// <unmanaged-short>CorEventAttr</unmanaged-short>
    public enum CorEventAttr : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>evSpecialName</unmanaged>
        /// <unmanaged-short>evSpecialName</unmanaged-short>
        EvSpecialName = unchecked ((System.Int32)(512)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>evReservedMask</unmanaged>
        /// <unmanaged-short>evReservedMask</unmanaged-short>
        EvReservedMask = unchecked ((System.Int32)(1024)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>evRTSpecialName</unmanaged>
        /// <unmanaged-short>evRTSpecialName</unmanaged-short>
        EvRTSpecialName = unchecked ((System.Int32)(1024))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorExceptionFlag</unmanaged>
    /// <unmanaged-short>CorExceptionFlag</unmanaged-short>
    public enum CorExceptionFlag : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>COR_ILEXCEPTION_CLAUSE_NONE</unmanaged>
        /// <unmanaged-short>COR_ILEXCEPTION_CLAUSE_NONE</unmanaged-short>
        CorIlexceptionClauseNone = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>COR_ILEXCEPTION_CLAUSE_OFFSETLEN</unmanaged>
        /// <unmanaged-short>COR_ILEXCEPTION_CLAUSE_OFFSETLEN</unmanaged-short>
        CorIlexceptionClauseOffsetlen = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>COR_ILEXCEPTION_CLAUSE_DEPRECATED</unmanaged>
        /// <unmanaged-short>COR_ILEXCEPTION_CLAUSE_DEPRECATED</unmanaged-short>
        CorIlexceptionClauseDeprecated = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>COR_ILEXCEPTION_CLAUSE_FILTER</unmanaged>
        /// <unmanaged-short>COR_ILEXCEPTION_CLAUSE_FILTER</unmanaged-short>
        CorIlexceptionClauseFilter = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>COR_ILEXCEPTION_CLAUSE_FINALLY</unmanaged>
        /// <unmanaged-short>COR_ILEXCEPTION_CLAUSE_FINALLY</unmanaged-short>
        CorIlexceptionClauseFinally = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>COR_ILEXCEPTION_CLAUSE_FAULT</unmanaged>
        /// <unmanaged-short>COR_ILEXCEPTION_CLAUSE_FAULT</unmanaged-short>
        CorIlexceptionClauseFault = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>COR_ILEXCEPTION_CLAUSE_DUPLICATED</unmanaged>
        /// <unmanaged-short>COR_ILEXCEPTION_CLAUSE_DUPLICATED</unmanaged-short>
        CorIlexceptionClauseDuplicated = unchecked ((System.Int32)(8))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorFieldAttr</unmanaged>
    /// <unmanaged-short>CorFieldAttr</unmanaged-short>
    public enum CorFieldAttr : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>fdFieldAccessMask</unmanaged>
        /// <unmanaged-short>fdFieldAccessMask</unmanaged-short>
        FdFieldAccessMask = unchecked ((System.Int32)(7)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>fdPrivateScope</unmanaged>
        /// <unmanaged-short>fdPrivateScope</unmanaged-short>
        FdPrivateScope = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>fdPrivate</unmanaged>
        /// <unmanaged-short>fdPrivate</unmanaged-short>
        FdPrivate = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>fdFamANDAssem</unmanaged>
        /// <unmanaged-short>fdFamANDAssem</unmanaged-short>
        FdFamANDAssem = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>fdAssembly</unmanaged>
        /// <unmanaged-short>fdAssembly</unmanaged-short>
        FdAssembly = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>fdFamily</unmanaged>
        /// <unmanaged-short>fdFamily</unmanaged-short>
        FdFamily = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>fdFamORAssem</unmanaged>
        /// <unmanaged-short>fdFamORAssem</unmanaged-short>
        FdFamORAssem = unchecked ((System.Int32)(5)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>fdPublic</unmanaged>
        /// <unmanaged-short>fdPublic</unmanaged-short>
        FdPublic = unchecked ((System.Int32)(6)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>fdStatic</unmanaged>
        /// <unmanaged-short>fdStatic</unmanaged-short>
        FdStatic = unchecked ((System.Int32)(16)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>fdInitOnly</unmanaged>
        /// <unmanaged-short>fdInitOnly</unmanaged-short>
        FdInitOnly = unchecked ((System.Int32)(32)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>fdLiteral</unmanaged>
        /// <unmanaged-short>fdLiteral</unmanaged-short>
        FdLiteral = unchecked ((System.Int32)(64)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>fdNotSerialized</unmanaged>
        /// <unmanaged-short>fdNotSerialized</unmanaged-short>
        FdNotSerialized = unchecked ((System.Int32)(128)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>fdSpecialName</unmanaged>
        /// <unmanaged-short>fdSpecialName</unmanaged-short>
        FdSpecialName = unchecked ((System.Int32)(512)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>fdPinvokeImpl</unmanaged>
        /// <unmanaged-short>fdPinvokeImpl</unmanaged-short>
        FdPinvokeImpl = unchecked ((System.Int32)(8192)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>fdReservedMask</unmanaged>
        /// <unmanaged-short>fdReservedMask</unmanaged-short>
        FdReservedMask = unchecked ((System.Int32)(38144)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>fdRTSpecialName</unmanaged>
        /// <unmanaged-short>fdRTSpecialName</unmanaged-short>
        FdRTSpecialName = unchecked ((System.Int32)(1024)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>fdHasFieldMarshal</unmanaged>
        /// <unmanaged-short>fdHasFieldMarshal</unmanaged-short>
        FdHasFieldMarshal = unchecked ((System.Int32)(4096)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>fdHasDefault</unmanaged>
        /// <unmanaged-short>fdHasDefault</unmanaged-short>
        FdHasDefault = unchecked ((System.Int32)(32768)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>fdHasFieldRVA</unmanaged>
        /// <unmanaged-short>fdHasFieldRVA</unmanaged-short>
        FdHasFieldRVA = unchecked ((System.Int32)(256))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorFileFlags</unmanaged>
    /// <unmanaged-short>CorFileFlags</unmanaged-short>
    public enum CorFileFlags : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ffContainsMetaData</unmanaged>
        /// <unmanaged-short>ffContainsMetaData</unmanaged-short>
        FfContainsMetaData = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ffContainsNoMetaData</unmanaged>
        /// <unmanaged-short>ffContainsNoMetaData</unmanaged-short>
        FfContainsNoMetaData = unchecked ((System.Int32)(1))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorFileMapping</unmanaged>
    /// <unmanaged-short>CorFileMapping</unmanaged-short>
    public enum CorFileMapping : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>fmFlat</unmanaged>
        /// <unmanaged-short>fmFlat</unmanaged-short>
        FmFlat = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>fmExecutableImage</unmanaged>
        /// <unmanaged-short>fmExecutableImage</unmanaged-short>
        FmExecutableImage = unchecked ((System.Int32)(1))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorGCReferenceType</unmanaged>
    /// <unmanaged-short>CorGCReferenceType</unmanaged-short>
    public enum CorGCReferenceType : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorHandleStrong</unmanaged>
        /// <unmanaged-short>CorHandleStrong</unmanaged-short>
        CorHandleStrong = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorHandleStrongPinning</unmanaged>
        /// <unmanaged-short>CorHandleStrongPinning</unmanaged-short>
        CorHandleStrongPinning = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorHandleWeakShort</unmanaged>
        /// <unmanaged-short>CorHandleWeakShort</unmanaged-short>
        CorHandleWeakShort = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorHandleWeakLong</unmanaged>
        /// <unmanaged-short>CorHandleWeakLong</unmanaged-short>
        CorHandleWeakLong = unchecked ((System.Int32)(8)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorHandleWeakRefCount</unmanaged>
        /// <unmanaged-short>CorHandleWeakRefCount</unmanaged-short>
        CorHandleWeakRefCount = unchecked ((System.Int32)(16)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorHandleStrongRefCount</unmanaged>
        /// <unmanaged-short>CorHandleStrongRefCount</unmanaged-short>
        CorHandleStrongRefCount = unchecked ((System.Int32)(32)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorHandleStrongDependent</unmanaged>
        /// <unmanaged-short>CorHandleStrongDependent</unmanaged-short>
        CorHandleStrongDependent = unchecked ((System.Int32)(64)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorHandleStrongAsyncPinned</unmanaged>
        /// <unmanaged-short>CorHandleStrongAsyncPinned</unmanaged-short>
        CorHandleStrongAsyncPinned = unchecked ((System.Int32)(128)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorHandleStrongSizedByref</unmanaged>
        /// <unmanaged-short>CorHandleStrongSizedByref</unmanaged-short>
        CorHandleStrongSizedByref = unchecked ((System.Int32)(256)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorHandleWeakWinRT</unmanaged>
        /// <unmanaged-short>CorHandleWeakWinRT</unmanaged-short>
        CorHandleWeakWinRT = unchecked ((System.Int32)(512)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorReferenceStack</unmanaged>
        /// <unmanaged-short>CorReferenceStack</unmanaged-short>
        CorReferenceStack = unchecked ((System.Int32)(-2147483647)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorReferenceFinalizer</unmanaged>
        /// <unmanaged-short>CorReferenceFinalizer</unmanaged-short>
        CorReferenceFinalizer = unchecked ((System.Int32)(80000002)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorHandleStrongOnly</unmanaged>
        /// <unmanaged-short>CorHandleStrongOnly</unmanaged-short>
        CorHandleStrongOnly = unchecked ((System.Int32)(483)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorHandleWeakOnly</unmanaged>
        /// <unmanaged-short>CorHandleWeakOnly</unmanaged-short>
        CorHandleWeakOnly = unchecked ((System.Int32)(540)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorHandleAll</unmanaged>
        /// <unmanaged-short>CorHandleAll</unmanaged-short>
        CorHandleAll = unchecked ((System.Int32)(2147483647))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorGenericParamAttr</unmanaged>
    /// <unmanaged-short>CorGenericParamAttr</unmanaged-short>
    public enum CorGenericParamAttr : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>gpVarianceMask</unmanaged>
        /// <unmanaged-short>gpVarianceMask</unmanaged-short>
        GpVarianceMask = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>gpNonVariant</unmanaged>
        /// <unmanaged-short>gpNonVariant</unmanaged-short>
        GpNonVariant = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>gpCovariant</unmanaged>
        /// <unmanaged-short>gpCovariant</unmanaged-short>
        GpCovariant = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>gpContravariant</unmanaged>
        /// <unmanaged-short>gpContravariant</unmanaged-short>
        GpContravariant = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>gpSpecialConstraintMask</unmanaged>
        /// <unmanaged-short>gpSpecialConstraintMask</unmanaged-short>
        GpSpecialConstraintMask = unchecked ((System.Int32)(28)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>gpNoSpecialConstraint</unmanaged>
        /// <unmanaged-short>gpNoSpecialConstraint</unmanaged-short>
        GpNoSpecialConstraint = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>gpReferenceTypeConstraint</unmanaged>
        /// <unmanaged-short>gpReferenceTypeConstraint</unmanaged-short>
        GpReferenceTypeConstraint = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>gpNotNullableValueTypeConstraint</unmanaged>
        /// <unmanaged-short>gpNotNullableValueTypeConstraint</unmanaged-short>
        GpNotNullableValueTypeConstraint = unchecked ((System.Int32)(8)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>gpDefaultConstructorConstraint</unmanaged>
        /// <unmanaged-short>gpDefaultConstructorConstraint</unmanaged-short>
        GpDefaultConstructorConstraint = unchecked ((System.Int32)(16))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CORHDR_ENUM_0</unmanaged>
    /// <unmanaged-short>CORHDR_ENUM_0</unmanaged-short>
    public enum CorhdrEnum0 : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>DESCR_GROUP_METHODDEF</unmanaged>
        /// <unmanaged-short>DESCR_GROUP_METHODDEF</unmanaged-short>
        DescrGroupMethoddef = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>DESCR_GROUP_METHODIMPL</unmanaged>
        /// <unmanaged-short>DESCR_GROUP_METHODIMPL</unmanaged-short>
        DescrGroupMethodimpl = unchecked ((System.Int32)(1))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorILMethodFlags</unmanaged>
    /// <unmanaged-short>CorILMethodFlags</unmanaged-short>
    public enum CorILMethodFlags : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorILMethod_InitLocals</unmanaged>
        /// <unmanaged-short>CorILMethod_InitLocals</unmanaged-short>
        InitLocals = unchecked ((System.Int32)(16)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorILMethod_MoreSects</unmanaged>
        /// <unmanaged-short>CorILMethod_MoreSects</unmanaged-short>
        MoreSects = unchecked ((System.Int32)(8)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorILMethod_CompressedIL</unmanaged>
        /// <unmanaged-short>CorILMethod_CompressedIL</unmanaged-short>
        CompressedIL = unchecked ((System.Int32)(64)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorILMethod_FormatShift</unmanaged>
        /// <unmanaged-short>CorILMethod_FormatShift</unmanaged-short>
        FormatShift = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorILMethod_FormatMask</unmanaged>
        /// <unmanaged-short>CorILMethod_FormatMask</unmanaged-short>
        FormatMask = unchecked ((System.Int32)(7)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorILMethod_TinyFormat</unmanaged>
        /// <unmanaged-short>CorILMethod_TinyFormat</unmanaged-short>
        TinyFormat = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorILMethod_SmallFormat</unmanaged>
        /// <unmanaged-short>CorILMethod_SmallFormat</unmanaged-short>
        SmallFormat = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorILMethod_FatFormat</unmanaged>
        /// <unmanaged-short>CorILMethod_FatFormat</unmanaged-short>
        FatFormat = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorILMethod_TinyFormat1</unmanaged>
        /// <unmanaged-short>CorILMethod_TinyFormat1</unmanaged-short>
        TinyFormat1 = unchecked ((System.Int32)(6))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorILMethodSect</unmanaged>
    /// <unmanaged-short>CorILMethodSect</unmanaged-short>
    public enum CorILMethodSect : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorILMethod_Sect_Reserved</unmanaged>
        /// <unmanaged-short>CorILMethod_Sect_Reserved</unmanaged-short>
        SectReserved = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorILMethod_Sect_EHTable</unmanaged>
        /// <unmanaged-short>CorILMethod_Sect_EHTable</unmanaged-short>
        SectEHTable = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorILMethod_Sect_OptILTable</unmanaged>
        /// <unmanaged-short>CorILMethod_Sect_OptILTable</unmanaged-short>
        SectOptILTable = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorILMethod_Sect_KindMask</unmanaged>
        /// <unmanaged-short>CorILMethod_Sect_KindMask</unmanaged-short>
        SectKindMask = unchecked ((System.Int32)(63)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorILMethod_Sect_FatFormat</unmanaged>
        /// <unmanaged-short>CorILMethod_Sect_FatFormat</unmanaged-short>
        SectFatFormat = unchecked ((System.Int32)(64)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CorILMethod_Sect_MoreSects</unmanaged>
        /// <unmanaged-short>CorILMethod_Sect_MoreSects</unmanaged-short>
        SectMoreSects = unchecked ((System.Int32)(128))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorImportOptions</unmanaged>
    /// <unmanaged-short>CorImportOptions</unmanaged-short>
    public enum CorImportOptions : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDImportOptionDefault</unmanaged>
        /// <unmanaged-short>MDImportOptionDefault</unmanaged-short>
        MDImportOptionDefault = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDImportOptionAll</unmanaged>
        /// <unmanaged-short>MDImportOptionAll</unmanaged-short>
        MDImportOptionAll = unchecked ((System.Int32)(-1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDImportOptionAllTypeDefs</unmanaged>
        /// <unmanaged-short>MDImportOptionAllTypeDefs</unmanaged-short>
        MDImportOptionAllTypeDefs = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDImportOptionAllMethodDefs</unmanaged>
        /// <unmanaged-short>MDImportOptionAllMethodDefs</unmanaged-short>
        MDImportOptionAllMethodDefs = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDImportOptionAllFieldDefs</unmanaged>
        /// <unmanaged-short>MDImportOptionAllFieldDefs</unmanaged-short>
        MDImportOptionAllFieldDefs = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDImportOptionAllProperties</unmanaged>
        /// <unmanaged-short>MDImportOptionAllProperties</unmanaged-short>
        MDImportOptionAllProperties = unchecked ((System.Int32)(8)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDImportOptionAllEvents</unmanaged>
        /// <unmanaged-short>MDImportOptionAllEvents</unmanaged-short>
        MDImportOptionAllEvents = unchecked ((System.Int32)(16)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDImportOptionAllCustomAttributes</unmanaged>
        /// <unmanaged-short>MDImportOptionAllCustomAttributes</unmanaged-short>
        MDImportOptionAllCustomAttributes = unchecked ((System.Int32)(32)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDImportOptionAllExportedTypes</unmanaged>
        /// <unmanaged-short>MDImportOptionAllExportedTypes</unmanaged-short>
        MDImportOptionAllExportedTypes = unchecked ((System.Int32)(64))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorLinkerOptions</unmanaged>
    /// <unmanaged-short>CorLinkerOptions</unmanaged-short>
    public enum CorLinkerOptions : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDAssembly</unmanaged>
        /// <unmanaged-short>MDAssembly</unmanaged-short>
        MDAssembly = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDNetModule</unmanaged>
        /// <unmanaged-short>MDNetModule</unmanaged-short>
        MDNetModule = unchecked ((System.Int32)(1))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorLocalRefPreservation</unmanaged>
    /// <unmanaged-short>CorLocalRefPreservation</unmanaged-short>
    public enum CorLocalRefPreservation : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDPreserveLocalRefsNone</unmanaged>
        /// <unmanaged-short>MDPreserveLocalRefsNone</unmanaged-short>
        MDPreserveLocalRefsNone = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDPreserveLocalTypeRef</unmanaged>
        /// <unmanaged-short>MDPreserveLocalTypeRef</unmanaged-short>
        MDPreserveLocalTypeRef = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDPreserveLocalMemberRef</unmanaged>
        /// <unmanaged-short>MDPreserveLocalMemberRef</unmanaged-short>
        MDPreserveLocalMemberRef = unchecked ((System.Int32)(2))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorManifestResourceFlags</unmanaged>
    /// <unmanaged-short>CorManifestResourceFlags</unmanaged-short>
    public enum CorManifestResourceFlags : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mrVisibilityMask</unmanaged>
        /// <unmanaged-short>mrVisibilityMask</unmanaged-short>
        MrVisibilityMask = unchecked ((System.Int32)(7)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mrPublic</unmanaged>
        /// <unmanaged-short>mrPublic</unmanaged-short>
        MrPublic = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mrPrivate</unmanaged>
        /// <unmanaged-short>mrPrivate</unmanaged-short>
        MrPrivate = unchecked ((System.Int32)(2))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorMethodAttr</unmanaged>
    /// <unmanaged-short>CorMethodAttr</unmanaged-short>
    public enum CorMethodAttr : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdMemberAccessMask</unmanaged>
        /// <unmanaged-short>mdMemberAccessMask</unmanaged-short>
        MdMemberAccessMask = unchecked ((System.Int32)(7)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdPrivateScope</unmanaged>
        /// <unmanaged-short>mdPrivateScope</unmanaged-short>
        MdPrivateScope = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdPrivate</unmanaged>
        /// <unmanaged-short>mdPrivate</unmanaged-short>
        MdPrivate = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdFamANDAssem</unmanaged>
        /// <unmanaged-short>mdFamANDAssem</unmanaged-short>
        MdFamANDAssem = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdAssem</unmanaged>
        /// <unmanaged-short>mdAssem</unmanaged-short>
        MdAssem = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdFamily</unmanaged>
        /// <unmanaged-short>mdFamily</unmanaged-short>
        MdFamily = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdFamORAssem</unmanaged>
        /// <unmanaged-short>mdFamORAssem</unmanaged-short>
        MdFamORAssem = unchecked ((System.Int32)(5)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdPublic</unmanaged>
        /// <unmanaged-short>mdPublic</unmanaged-short>
        MdPublic = unchecked ((System.Int32)(6)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdStatic</unmanaged>
        /// <unmanaged-short>mdStatic</unmanaged-short>
        MdStatic = unchecked ((System.Int32)(16)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdFinal</unmanaged>
        /// <unmanaged-short>mdFinal</unmanaged-short>
        MdFinal = unchecked ((System.Int32)(32)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdVirtual</unmanaged>
        /// <unmanaged-short>mdVirtual</unmanaged-short>
        MdVirtual = unchecked ((System.Int32)(64)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdHideBySig</unmanaged>
        /// <unmanaged-short>mdHideBySig</unmanaged-short>
        MdHideBySig = unchecked ((System.Int32)(128)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdVtableLayoutMask</unmanaged>
        /// <unmanaged-short>mdVtableLayoutMask</unmanaged-short>
        MdVtableLayoutMask = unchecked ((System.Int32)(256)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdReuseSlot</unmanaged>
        /// <unmanaged-short>mdReuseSlot</unmanaged-short>
        MdReuseSlot = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdNewSlot</unmanaged>
        /// <unmanaged-short>mdNewSlot</unmanaged-short>
        MdNewSlot = unchecked ((System.Int32)(256)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdCheckAccessOnOverride</unmanaged>
        /// <unmanaged-short>mdCheckAccessOnOverride</unmanaged-short>
        MdCheckAccessOnOverride = unchecked ((System.Int32)(512)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdAbstract</unmanaged>
        /// <unmanaged-short>mdAbstract</unmanaged-short>
        MdAbstract = unchecked ((System.Int32)(1024)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdSpecialName</unmanaged>
        /// <unmanaged-short>mdSpecialName</unmanaged-short>
        MdSpecialName = unchecked ((System.Int32)(2048)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdPinvokeImpl</unmanaged>
        /// <unmanaged-short>mdPinvokeImpl</unmanaged-short>
        MdPinvokeImpl = unchecked ((System.Int32)(8192)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdUnmanagedExport</unmanaged>
        /// <unmanaged-short>mdUnmanagedExport</unmanaged-short>
        MdUnmanagedExport = unchecked ((System.Int32)(8)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdReservedMask</unmanaged>
        /// <unmanaged-short>mdReservedMask</unmanaged-short>
        MdReservedMask = unchecked ((System.Int32)(53248)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdRTSpecialName</unmanaged>
        /// <unmanaged-short>mdRTSpecialName</unmanaged-short>
        MdRTSpecialName = unchecked ((System.Int32)(4096)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdHasSecurity</unmanaged>
        /// <unmanaged-short>mdHasSecurity</unmanaged-short>
        MdHasSecurity = unchecked ((System.Int32)(16384)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdRequireSecObject</unmanaged>
        /// <unmanaged-short>mdRequireSecObject</unmanaged-short>
        MdRequireSecObject = unchecked ((System.Int32)(32768))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorMethodImpl</unmanaged>
    /// <unmanaged-short>CorMethodImpl</unmanaged-short>
    public enum CorMethodImpl : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>miCodeTypeMask</unmanaged>
        /// <unmanaged-short>miCodeTypeMask</unmanaged-short>
        MiCodeTypeMask = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>miIL</unmanaged>
        /// <unmanaged-short>miIL</unmanaged-short>
        MiIL = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>miNative</unmanaged>
        /// <unmanaged-short>miNative</unmanaged-short>
        MiNative = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>miOPTIL</unmanaged>
        /// <unmanaged-short>miOPTIL</unmanaged-short>
        MiOPTIL = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>miRuntime</unmanaged>
        /// <unmanaged-short>miRuntime</unmanaged-short>
        MiRuntime = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>miManagedMask</unmanaged>
        /// <unmanaged-short>miManagedMask</unmanaged-short>
        MiManagedMask = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>miUnmanaged</unmanaged>
        /// <unmanaged-short>miUnmanaged</unmanaged-short>
        MiUnmanaged = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>miManaged</unmanaged>
        /// <unmanaged-short>miManaged</unmanaged-short>
        MiManaged = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>miForwardRef</unmanaged>
        /// <unmanaged-short>miForwardRef</unmanaged-short>
        MiForwardRef = unchecked ((System.Int32)(16)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>miPreserveSig</unmanaged>
        /// <unmanaged-short>miPreserveSig</unmanaged-short>
        MiPreserveSig = unchecked ((System.Int32)(128)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>miInternalCall</unmanaged>
        /// <unmanaged-short>miInternalCall</unmanaged-short>
        MiInternalCall = unchecked ((System.Int32)(4096)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>miSynchronized</unmanaged>
        /// <unmanaged-short>miSynchronized</unmanaged-short>
        MiSynchronized = unchecked ((System.Int32)(32)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>miNoInlining</unmanaged>
        /// <unmanaged-short>miNoInlining</unmanaged-short>
        MiNoInlining = unchecked ((System.Int32)(8)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>miAggressiveInlining</unmanaged>
        /// <unmanaged-short>miAggressiveInlining</unmanaged-short>
        MiAggressiveInlining = unchecked ((System.Int32)(256)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>miNoOptimization</unmanaged>
        /// <unmanaged-short>miNoOptimization</unmanaged-short>
        MiNoOptimization = unchecked ((System.Int32)(64)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>miUserMask</unmanaged>
        /// <unmanaged-short>miUserMask</unmanaged-short>
        MiUserMask = unchecked ((System.Int32)(4604)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>miMaxMethodImplVal</unmanaged>
        /// <unmanaged-short>miMaxMethodImplVal</unmanaged-short>
        MiMaxMethodImplVal = unchecked ((System.Int32)(65535))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorMethodSemanticsAttr</unmanaged>
    /// <unmanaged-short>CorMethodSemanticsAttr</unmanaged-short>
    public enum CorMethodSemanticsAttr : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>msSetter</unmanaged>
        /// <unmanaged-short>msSetter</unmanaged-short>
        MsSetter = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>msGetter</unmanaged>
        /// <unmanaged-short>msGetter</unmanaged-short>
        MsGetter = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>msOther</unmanaged>
        /// <unmanaged-short>msOther</unmanaged-short>
        MsOther = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>msAddOn</unmanaged>
        /// <unmanaged-short>msAddOn</unmanaged-short>
        MsAddOn = unchecked ((System.Int32)(8)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>msRemoveOn</unmanaged>
        /// <unmanaged-short>msRemoveOn</unmanaged-short>
        MsRemoveOn = unchecked ((System.Int32)(16)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>msFire</unmanaged>
        /// <unmanaged-short>msFire</unmanaged-short>
        MsFire = unchecked ((System.Int32)(32))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorNativeLinkFlags</unmanaged>
    /// <unmanaged-short>CorNativeLinkFlags</unmanaged-short>
    public enum CorNativeLinkFlags : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>nlfNone</unmanaged>
        /// <unmanaged-short>nlfNone</unmanaged-short>
        NlfNone = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>nlfLastError</unmanaged>
        /// <unmanaged-short>nlfLastError</unmanaged-short>
        NlfLastError = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>nlfNoMangle</unmanaged>
        /// <unmanaged-short>nlfNoMangle</unmanaged-short>
        NlfNoMangle = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>nlfMaxValue</unmanaged>
        /// <unmanaged-short>nlfMaxValue</unmanaged-short>
        NlfMaxValue = unchecked ((System.Int32)(3))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorNativeLinkType</unmanaged>
    /// <unmanaged-short>CorNativeLinkType</unmanaged-short>
    public enum CorNativeLinkType : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>nltNone</unmanaged>
        /// <unmanaged-short>nltNone</unmanaged-short>
        NltNone = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>nltAnsi</unmanaged>
        /// <unmanaged-short>nltAnsi</unmanaged-short>
        NltAnsi = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>nltUnicode</unmanaged>
        /// <unmanaged-short>nltUnicode</unmanaged-short>
        NltUnicode = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>nltAuto</unmanaged>
        /// <unmanaged-short>nltAuto</unmanaged-short>
        NltAuto = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>nltOle</unmanaged>
        /// <unmanaged-short>nltOle</unmanaged-short>
        NltOle = unchecked ((System.Int32)(5)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>nltMaxValue</unmanaged>
        /// <unmanaged-short>nltMaxValue</unmanaged-short>
        NltMaxValue = unchecked ((System.Int32)(7))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorNativeType</unmanaged>
    /// <unmanaged-short>CorNativeType</unmanaged-short>
    public enum CorNativeType : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_END</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_END</unmanaged-short>
        NativeTypeEnd = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_VOID</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_VOID</unmanaged-short>
        NativeTypeVoid = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_BOOLEAN</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_BOOLEAN</unmanaged-short>
        NativeTypeBoolean = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_I1</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_I1</unmanaged-short>
        NativeTypeI1 = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_U1</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_U1</unmanaged-short>
        NativeTypeU1 = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_I2</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_I2</unmanaged-short>
        NativeTypeI2 = unchecked ((System.Int32)(5)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_U2</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_U2</unmanaged-short>
        NativeTypeU2 = unchecked ((System.Int32)(6)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_I4</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_I4</unmanaged-short>
        NativeTypeI4 = unchecked ((System.Int32)(7)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_U4</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_U4</unmanaged-short>
        NativeTypeU4 = unchecked ((System.Int32)(8)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_I8</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_I8</unmanaged-short>
        NativeTypeI8 = unchecked ((System.Int32)(9)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_U8</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_U8</unmanaged-short>
        NativeTypeU8 = unchecked ((System.Int32)(10)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_R4</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_R4</unmanaged-short>
        NativeTypeR4 = unchecked ((System.Int32)(11)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_R8</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_R8</unmanaged-short>
        NativeTypeR8 = unchecked ((System.Int32)(12)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_SYSCHAR</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_SYSCHAR</unmanaged-short>
        NativeTypeSyschar = unchecked ((System.Int32)(13)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_VARIANT</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_VARIANT</unmanaged-short>
        NativeTypeVariant = unchecked ((System.Int32)(14)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_CURRENCY</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_CURRENCY</unmanaged-short>
        NativeTypeCurrency = unchecked ((System.Int32)(15)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_PTR</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_PTR</unmanaged-short>
        NativeTypePtr = unchecked ((System.Int32)(16)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_DECIMAL</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_DECIMAL</unmanaged-short>
        NativeTypeDecimal = unchecked ((System.Int32)(17)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_DATE</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_DATE</unmanaged-short>
        NativeTypeDate = unchecked ((System.Int32)(18)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_BSTR</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_BSTR</unmanaged-short>
        NativeTypeBstr = unchecked ((System.Int32)(19)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_LPSTR</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_LPSTR</unmanaged-short>
        NativeTypeLpstr = unchecked ((System.Int32)(20)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_LPWSTR</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_LPWSTR</unmanaged-short>
        NativeTypeLpwstr = unchecked ((System.Int32)(21)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_LPTSTR</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_LPTSTR</unmanaged-short>
        NativeTypeLptstr = unchecked ((System.Int32)(22)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_FIXEDSYSSTRING</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_FIXEDSYSSTRING</unmanaged-short>
        NativeTypeFixedsysstring = unchecked ((System.Int32)(23)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_OBJECTREF</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_OBJECTREF</unmanaged-short>
        NativeTypeObjectref = unchecked ((System.Int32)(24)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_IUNKNOWN</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_IUNKNOWN</unmanaged-short>
        NativeTypeIunknown = unchecked ((System.Int32)(25)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_IDISPATCH</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_IDISPATCH</unmanaged-short>
        NativeTypeIdispatch = unchecked ((System.Int32)(26)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_STRUCT</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_STRUCT</unmanaged-short>
        NativeTypeStruct = unchecked ((System.Int32)(27)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_INTF</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_INTF</unmanaged-short>
        NativeTypeIntf = unchecked ((System.Int32)(28)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_SAFEARRAY</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_SAFEARRAY</unmanaged-short>
        NativeTypeSafearray = unchecked ((System.Int32)(29)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_FIXEDARRAY</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_FIXEDARRAY</unmanaged-short>
        NativeTypeFixedarray = unchecked ((System.Int32)(30)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_INT</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_INT</unmanaged-short>
        NativeTypeInt = unchecked ((System.Int32)(31)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_UINT</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_UINT</unmanaged-short>
        NativeTypeUint = unchecked ((System.Int32)(32)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_NESTEDSTRUCT</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_NESTEDSTRUCT</unmanaged-short>
        NativeTypeNestedstruct = unchecked ((System.Int32)(33)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_BYVALSTR</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_BYVALSTR</unmanaged-short>
        NativeTypeByvalstr = unchecked ((System.Int32)(34)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_ANSIBSTR</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_ANSIBSTR</unmanaged-short>
        NativeTypeAnsibstr = unchecked ((System.Int32)(35)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_TBSTR</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_TBSTR</unmanaged-short>
        NativeTypeTbstr = unchecked ((System.Int32)(36)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_VARIANTBOOL</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_VARIANTBOOL</unmanaged-short>
        NativeTypeVariantbool = unchecked ((System.Int32)(37)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_FUNC</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_FUNC</unmanaged-short>
        NativeTypeFunc = unchecked ((System.Int32)(38)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_ASANY</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_ASANY</unmanaged-short>
        NativeTypeAsany = unchecked ((System.Int32)(40)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_ARRAY</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_ARRAY</unmanaged-short>
        NativeTypeArray = unchecked ((System.Int32)(42)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_LPSTRUCT</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_LPSTRUCT</unmanaged-short>
        NativeTypeLpstruct = unchecked ((System.Int32)(43)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_CUSTOMMARSHALER</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_CUSTOMMARSHALER</unmanaged-short>
        NativeTypeCustommarshaler = unchecked ((System.Int32)(44)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_ERROR</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_ERROR</unmanaged-short>
        NativeTypeError = unchecked ((System.Int32)(45)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_IINSPECTABLE</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_IINSPECTABLE</unmanaged-short>
        NativeTypeIinspectable = unchecked ((System.Int32)(46)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_HSTRING</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_HSTRING</unmanaged-short>
        NativeTypeHstring = unchecked ((System.Int32)(47)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NATIVE_TYPE_MAX</unmanaged>
        /// <unmanaged-short>NATIVE_TYPE_MAX</unmanaged-short>
        NativeTypeMax = unchecked ((System.Int32)(80))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorNotificationForTokenMovement</unmanaged>
    /// <unmanaged-short>CorNotificationForTokenMovement</unmanaged-short>
    public enum CorNotificationForTokenMovement : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDNotifyDefault</unmanaged>
        /// <unmanaged-short>MDNotifyDefault</unmanaged-short>
        MDNotifyDefault = unchecked ((System.Int32)(15)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDNotifyAll</unmanaged>
        /// <unmanaged-short>MDNotifyAll</unmanaged-short>
        MDNotifyAll = unchecked ((System.Int32)(-1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDNotifyNone</unmanaged>
        /// <unmanaged-short>MDNotifyNone</unmanaged-short>
        MDNotifyNone = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDNotifyMethodDef</unmanaged>
        /// <unmanaged-short>MDNotifyMethodDef</unmanaged-short>
        MDNotifyMethodDef = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDNotifyMemberRef</unmanaged>
        /// <unmanaged-short>MDNotifyMemberRef</unmanaged-short>
        MDNotifyMemberRef = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDNotifyFieldDef</unmanaged>
        /// <unmanaged-short>MDNotifyFieldDef</unmanaged-short>
        MDNotifyFieldDef = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDNotifyTypeRef</unmanaged>
        /// <unmanaged-short>MDNotifyTypeRef</unmanaged-short>
        MDNotifyTypeRef = unchecked ((System.Int32)(8)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDNotifyTypeDef</unmanaged>
        /// <unmanaged-short>MDNotifyTypeDef</unmanaged-short>
        MDNotifyTypeDef = unchecked ((System.Int32)(16)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDNotifyParamDef</unmanaged>
        /// <unmanaged-short>MDNotifyParamDef</unmanaged-short>
        MDNotifyParamDef = unchecked ((System.Int32)(32)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDNotifyInterfaceImpl</unmanaged>
        /// <unmanaged-short>MDNotifyInterfaceImpl</unmanaged-short>
        MDNotifyInterfaceImpl = unchecked ((System.Int32)(64)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDNotifyProperty</unmanaged>
        /// <unmanaged-short>MDNotifyProperty</unmanaged-short>
        MDNotifyProperty = unchecked ((System.Int32)(128)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDNotifyEvent</unmanaged>
        /// <unmanaged-short>MDNotifyEvent</unmanaged-short>
        MDNotifyEvent = unchecked ((System.Int32)(256)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDNotifySignature</unmanaged>
        /// <unmanaged-short>MDNotifySignature</unmanaged-short>
        MDNotifySignature = unchecked ((System.Int32)(512)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDNotifyTypeSpec</unmanaged>
        /// <unmanaged-short>MDNotifyTypeSpec</unmanaged-short>
        MDNotifyTypeSpec = unchecked ((System.Int32)(1024)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDNotifyCustomAttribute</unmanaged>
        /// <unmanaged-short>MDNotifyCustomAttribute</unmanaged-short>
        MDNotifyCustomAttribute = unchecked ((System.Int32)(2048)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDNotifySecurityValue</unmanaged>
        /// <unmanaged-short>MDNotifySecurityValue</unmanaged-short>
        MDNotifySecurityValue = unchecked ((System.Int32)(4096)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDNotifyPermission</unmanaged>
        /// <unmanaged-short>MDNotifyPermission</unmanaged-short>
        MDNotifyPermission = unchecked ((System.Int32)(8192)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDNotifyModuleRef</unmanaged>
        /// <unmanaged-short>MDNotifyModuleRef</unmanaged-short>
        MDNotifyModuleRef = unchecked ((System.Int32)(16384)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDNotifyNameSpace</unmanaged>
        /// <unmanaged-short>MDNotifyNameSpace</unmanaged-short>
        MDNotifyNameSpace = unchecked ((System.Int32)(32768)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDNotifyAssemblyRef</unmanaged>
        /// <unmanaged-short>MDNotifyAssemblyRef</unmanaged-short>
        MDNotifyAssemblyRef = unchecked ((System.Int32)(16777216)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDNotifyFile</unmanaged>
        /// <unmanaged-short>MDNotifyFile</unmanaged-short>
        MDNotifyFile = unchecked ((System.Int32)(33554432)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDNotifyExportedType</unmanaged>
        /// <unmanaged-short>MDNotifyExportedType</unmanaged-short>
        MDNotifyExportedType = unchecked ((System.Int32)(67108864)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDNotifyResource</unmanaged>
        /// <unmanaged-short>MDNotifyResource</unmanaged-short>
        MDNotifyResource = unchecked ((System.Int32)(134217728))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorOpenFlags</unmanaged>
    /// <unmanaged-short>CorOpenFlags</unmanaged-short>
    public enum CorOpenFlags : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ofRead</unmanaged>
        /// <unmanaged-short>ofRead</unmanaged-short>
        OfRead = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ofWrite</unmanaged>
        /// <unmanaged-short>ofWrite</unmanaged-short>
        OfWrite = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ofReadWriteMask</unmanaged>
        /// <unmanaged-short>ofReadWriteMask</unmanaged-short>
        OfReadWriteMask = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ofCopyMemory</unmanaged>
        /// <unmanaged-short>ofCopyMemory</unmanaged-short>
        OfCopyMemory = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ofReadOnly</unmanaged>
        /// <unmanaged-short>ofReadOnly</unmanaged-short>
        OfReadOnly = unchecked ((System.Int32)(16)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ofTakeOwnership</unmanaged>
        /// <unmanaged-short>ofTakeOwnership</unmanaged-short>
        OfTakeOwnership = unchecked ((System.Int32)(32)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ofNoTypeLib</unmanaged>
        /// <unmanaged-short>ofNoTypeLib</unmanaged-short>
        OfNoTypeLib = unchecked ((System.Int32)(128)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ofNoTransform</unmanaged>
        /// <unmanaged-short>ofNoTransform</unmanaged-short>
        OfNoTransform = unchecked ((System.Int32)(4096)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ofReserved1</unmanaged>
        /// <unmanaged-short>ofReserved1</unmanaged-short>
        OfReserved1 = unchecked ((System.Int32)(256)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ofReserved2</unmanaged>
        /// <unmanaged-short>ofReserved2</unmanaged-short>
        OfReserved2 = unchecked ((System.Int32)(512)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ofReserved3</unmanaged>
        /// <unmanaged-short>ofReserved3</unmanaged-short>
        OfReserved3 = unchecked ((System.Int32)(1024)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ofReserved</unmanaged>
        /// <unmanaged-short>ofReserved</unmanaged-short>
        OfReserved = unchecked ((System.Int32)(-4288))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorParamAttr</unmanaged>
    /// <unmanaged-short>CorParamAttr</unmanaged-short>
    public enum CorParamAttr : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pdIn</unmanaged>
        /// <unmanaged-short>pdIn</unmanaged-short>
        PdIn = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pdOut</unmanaged>
        /// <unmanaged-short>pdOut</unmanaged-short>
        PdOut = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pdOptional</unmanaged>
        /// <unmanaged-short>pdOptional</unmanaged-short>
        PdOptional = unchecked ((System.Int32)(16)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pdReservedMask</unmanaged>
        /// <unmanaged-short>pdReservedMask</unmanaged-short>
        PdReservedMask = unchecked ((System.Int32)(61440)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pdHasDefault</unmanaged>
        /// <unmanaged-short>pdHasDefault</unmanaged-short>
        PdHasDefault = unchecked ((System.Int32)(4096)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pdHasFieldMarshal</unmanaged>
        /// <unmanaged-short>pdHasFieldMarshal</unmanaged-short>
        PdHasFieldMarshal = unchecked ((System.Int32)(8192)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pdUnused</unmanaged>
        /// <unmanaged-short>pdUnused</unmanaged-short>
        PdUnused = unchecked ((System.Int32)(53216))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorPEKind</unmanaged>
    /// <unmanaged-short>CorPEKind</unmanaged-short>
    public enum CorPEKind : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>peNot</unmanaged>
        /// <unmanaged-short>peNot</unmanaged-short>
        PeNot = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>peILonly</unmanaged>
        /// <unmanaged-short>peILonly</unmanaged-short>
        PeILonly = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pe32BitRequired</unmanaged>
        /// <unmanaged-short>pe32BitRequired</unmanaged-short>
        Pe32BitRequired = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pe32Plus</unmanaged>
        /// <unmanaged-short>pe32Plus</unmanaged-short>
        Pe32Plus = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pe32Unmanaged</unmanaged>
        /// <unmanaged-short>pe32Unmanaged</unmanaged-short>
        Pe32Unmanaged = unchecked ((System.Int32)(8)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pe32BitPreferred</unmanaged>
        /// <unmanaged-short>pe32BitPreferred</unmanaged-short>
        Pe32BitPreferred = unchecked ((System.Int32)(16))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorPinvokeMap</unmanaged>
    /// <unmanaged-short>CorPinvokeMap</unmanaged-short>
    public enum CorPinvokeMap : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pmNoMangle</unmanaged>
        /// <unmanaged-short>pmNoMangle</unmanaged-short>
        PmNoMangle = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pmCharSetMask</unmanaged>
        /// <unmanaged-short>pmCharSetMask</unmanaged-short>
        PmCharSetMask = unchecked ((System.Int32)(6)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pmCharSetNotSpec</unmanaged>
        /// <unmanaged-short>pmCharSetNotSpec</unmanaged-short>
        PmCharSetNotSpec = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pmCharSetAnsi</unmanaged>
        /// <unmanaged-short>pmCharSetAnsi</unmanaged-short>
        PmCharSetAnsi = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pmCharSetUnicode</unmanaged>
        /// <unmanaged-short>pmCharSetUnicode</unmanaged-short>
        PmCharSetUnicode = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pmCharSetAuto</unmanaged>
        /// <unmanaged-short>pmCharSetAuto</unmanaged-short>
        PmCharSetAuto = unchecked ((System.Int32)(6)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pmBestFitUseAssem</unmanaged>
        /// <unmanaged-short>pmBestFitUseAssem</unmanaged-short>
        PmBestFitUseAssem = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pmBestFitEnabled</unmanaged>
        /// <unmanaged-short>pmBestFitEnabled</unmanaged-short>
        PmBestFitEnabled = unchecked ((System.Int32)(16)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pmBestFitDisabled</unmanaged>
        /// <unmanaged-short>pmBestFitDisabled</unmanaged-short>
        PmBestFitDisabled = unchecked ((System.Int32)(32)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pmBestFitMask</unmanaged>
        /// <unmanaged-short>pmBestFitMask</unmanaged-short>
        PmBestFitMask = unchecked ((System.Int32)(48)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pmThrowOnUnmappableCharUseAssem</unmanaged>
        /// <unmanaged-short>pmThrowOnUnmappableCharUseAssem</unmanaged-short>
        PmThrowOnUnmappableCharUseAssem = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pmThrowOnUnmappableCharEnabled</unmanaged>
        /// <unmanaged-short>pmThrowOnUnmappableCharEnabled</unmanaged-short>
        PmThrowOnUnmappableCharEnabled = unchecked ((System.Int32)(4096)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pmThrowOnUnmappableCharDisabled</unmanaged>
        /// <unmanaged-short>pmThrowOnUnmappableCharDisabled</unmanaged-short>
        PmThrowOnUnmappableCharDisabled = unchecked ((System.Int32)(8192)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pmThrowOnUnmappableCharMask</unmanaged>
        /// <unmanaged-short>pmThrowOnUnmappableCharMask</unmanaged-short>
        PmThrowOnUnmappableCharMask = unchecked ((System.Int32)(12288)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pmSupportsLastError</unmanaged>
        /// <unmanaged-short>pmSupportsLastError</unmanaged-short>
        PmSupportsLastError = unchecked ((System.Int32)(64)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pmCallConvMask</unmanaged>
        /// <unmanaged-short>pmCallConvMask</unmanaged-short>
        PmCallConvMask = unchecked ((System.Int32)(1792)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pmCallConvWinapi</unmanaged>
        /// <unmanaged-short>pmCallConvWinapi</unmanaged-short>
        PmCallConvWinapi = unchecked ((System.Int32)(256)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pmCallConvCdecl</unmanaged>
        /// <unmanaged-short>pmCallConvCdecl</unmanaged-short>
        PmCallConvCdecl = unchecked ((System.Int32)(512)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pmCallConvStdcall</unmanaged>
        /// <unmanaged-short>pmCallConvStdcall</unmanaged-short>
        PmCallConvStdcall = unchecked ((System.Int32)(768)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pmCallConvThiscall</unmanaged>
        /// <unmanaged-short>pmCallConvThiscall</unmanaged-short>
        PmCallConvThiscall = unchecked ((System.Int32)(1024)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pmCallConvFastcall</unmanaged>
        /// <unmanaged-short>pmCallConvFastcall</unmanaged-short>
        PmCallConvFastcall = unchecked ((System.Int32)(1280)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pmMaxValue</unmanaged>
        /// <unmanaged-short>pmMaxValue</unmanaged-short>
        PmMaxValue = unchecked ((System.Int32)(65535))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorPropertyAttr</unmanaged>
    /// <unmanaged-short>CorPropertyAttr</unmanaged-short>
    public enum CorPropertyAttr : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>prSpecialName</unmanaged>
        /// <unmanaged-short>prSpecialName</unmanaged-short>
        PrSpecialName = unchecked ((System.Int32)(512)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>prReservedMask</unmanaged>
        /// <unmanaged-short>prReservedMask</unmanaged-short>
        PrReservedMask = unchecked ((System.Int32)(62464)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>prRTSpecialName</unmanaged>
        /// <unmanaged-short>prRTSpecialName</unmanaged-short>
        PrRTSpecialName = unchecked ((System.Int32)(1024)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>prHasDefault</unmanaged>
        /// <unmanaged-short>prHasDefault</unmanaged-short>
        PrHasDefault = unchecked ((System.Int32)(4096)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>prUnused</unmanaged>
        /// <unmanaged-short>prUnused</unmanaged-short>
        PrUnused = unchecked ((System.Int32)(59903))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorRefToDefCheck</unmanaged>
    /// <unmanaged-short>CorRefToDefCheck</unmanaged-short>
    public enum CorRefToDefCheck : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDRefToDefDefault</unmanaged>
        /// <unmanaged-short>MDRefToDefDefault</unmanaged-short>
        MDRefToDefDefault = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDRefToDefAll</unmanaged>
        /// <unmanaged-short>MDRefToDefAll</unmanaged-short>
        MDRefToDefAll = unchecked ((System.Int32)(-1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDRefToDefNone</unmanaged>
        /// <unmanaged-short>MDRefToDefNone</unmanaged-short>
        MDRefToDefNone = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDTypeRefToDef</unmanaged>
        /// <unmanaged-short>MDTypeRefToDef</unmanaged-short>
        MDTypeRefToDef = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDMemberRefToDef</unmanaged>
        /// <unmanaged-short>MDMemberRefToDef</unmanaged-short>
        MDMemberRefToDef = unchecked ((System.Int32)(2))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorRegFlags</unmanaged>
    /// <unmanaged-short>CorRegFlags</unmanaged-short>
    public enum CorRegFlags : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>regNoCopy</unmanaged>
        /// <unmanaged-short>regNoCopy</unmanaged-short>
        RegNoCopy = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>regConfig</unmanaged>
        /// <unmanaged-short>regConfig</unmanaged-short>
        RegConfig = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>regHasRefs</unmanaged>
        /// <unmanaged-short>regHasRefs</unmanaged-short>
        RegHasRefs = unchecked ((System.Int32)(4))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorSaveSize</unmanaged>
    /// <unmanaged-short>CorSaveSize</unmanaged-short>
    public enum CorSaveSize : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>cssAccurate</unmanaged>
        /// <unmanaged-short>cssAccurate</unmanaged-short>
        CssAccurate = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>cssQuick</unmanaged>
        /// <unmanaged-short>cssQuick</unmanaged-short>
        CssQuick = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>cssDiscardTransientCAs</unmanaged>
        /// <unmanaged-short>cssDiscardTransientCAs</unmanaged-short>
        CssDiscardTransientCAs = unchecked ((System.Int32)(2))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorSerializationType</unmanaged>
    /// <unmanaged-short>CorSerializationType</unmanaged-short>
    public enum CorSerializationType : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>SERIALIZATION_TYPE_UNDEFINED</unmanaged>
        /// <unmanaged-short>SERIALIZATION_TYPE_UNDEFINED</unmanaged-short>
        SerializationTypeUndefined = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>SERIALIZATION_TYPE_BOOLEAN</unmanaged>
        /// <unmanaged-short>SERIALIZATION_TYPE_BOOLEAN</unmanaged-short>
        SerializationTypeBoolean = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>SERIALIZATION_TYPE_CHAR</unmanaged>
        /// <unmanaged-short>SERIALIZATION_TYPE_CHAR</unmanaged-short>
        SerializationTypeChar = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>SERIALIZATION_TYPE_I1</unmanaged>
        /// <unmanaged-short>SERIALIZATION_TYPE_I1</unmanaged-short>
        SerializationTypeI1 = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>SERIALIZATION_TYPE_U1</unmanaged>
        /// <unmanaged-short>SERIALIZATION_TYPE_U1</unmanaged-short>
        SerializationTypeU1 = unchecked ((System.Int32)(5)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>SERIALIZATION_TYPE_I2</unmanaged>
        /// <unmanaged-short>SERIALIZATION_TYPE_I2</unmanaged-short>
        SerializationTypeI2 = unchecked ((System.Int32)(6)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>SERIALIZATION_TYPE_U2</unmanaged>
        /// <unmanaged-short>SERIALIZATION_TYPE_U2</unmanaged-short>
        SerializationTypeU2 = unchecked ((System.Int32)(7)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>SERIALIZATION_TYPE_I4</unmanaged>
        /// <unmanaged-short>SERIALIZATION_TYPE_I4</unmanaged-short>
        SerializationTypeI4 = unchecked ((System.Int32)(8)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>SERIALIZATION_TYPE_U4</unmanaged>
        /// <unmanaged-short>SERIALIZATION_TYPE_U4</unmanaged-short>
        SerializationTypeU4 = unchecked ((System.Int32)(9)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>SERIALIZATION_TYPE_I8</unmanaged>
        /// <unmanaged-short>SERIALIZATION_TYPE_I8</unmanaged-short>
        SerializationTypeI8 = unchecked ((System.Int32)(10)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>SERIALIZATION_TYPE_U8</unmanaged>
        /// <unmanaged-short>SERIALIZATION_TYPE_U8</unmanaged-short>
        SerializationTypeU8 = unchecked ((System.Int32)(11)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>SERIALIZATION_TYPE_R4</unmanaged>
        /// <unmanaged-short>SERIALIZATION_TYPE_R4</unmanaged-short>
        SerializationTypeR4 = unchecked ((System.Int32)(12)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>SERIALIZATION_TYPE_R8</unmanaged>
        /// <unmanaged-short>SERIALIZATION_TYPE_R8</unmanaged-short>
        SerializationTypeR8 = unchecked ((System.Int32)(13)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>SERIALIZATION_TYPE_STRING</unmanaged>
        /// <unmanaged-short>SERIALIZATION_TYPE_STRING</unmanaged-short>
        SerializationTypeString = unchecked ((System.Int32)(14)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>SERIALIZATION_TYPE_SZARRAY</unmanaged>
        /// <unmanaged-short>SERIALIZATION_TYPE_SZARRAY</unmanaged-short>
        SerializationTypeSzarray = unchecked ((System.Int32)(29)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>SERIALIZATION_TYPE_TYPE</unmanaged>
        /// <unmanaged-short>SERIALIZATION_TYPE_TYPE</unmanaged-short>
        SerializationTypeType = unchecked ((System.Int32)(80)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>SERIALIZATION_TYPE_TAGGED_OBJECT</unmanaged>
        /// <unmanaged-short>SERIALIZATION_TYPE_TAGGED_OBJECT</unmanaged-short>
        SerializationTypeTaggedObject = unchecked ((System.Int32)(81)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>SERIALIZATION_TYPE_FIELD</unmanaged>
        /// <unmanaged-short>SERIALIZATION_TYPE_FIELD</unmanaged-short>
        SerializationTypeField = unchecked ((System.Int32)(83)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>SERIALIZATION_TYPE_PROPERTY</unmanaged>
        /// <unmanaged-short>SERIALIZATION_TYPE_PROPERTY</unmanaged-short>
        SerializationTypeProperty = unchecked ((System.Int32)(84)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>SERIALIZATION_TYPE_ENUM</unmanaged>
        /// <unmanaged-short>SERIALIZATION_TYPE_ENUM</unmanaged-short>
        SerializationTypeEnum = unchecked ((System.Int32)(85))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorSetENC</unmanaged>
    /// <unmanaged-short>CorSetENC</unmanaged-short>
    public enum CorSetENC : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDSetENCOn</unmanaged>
        /// <unmanaged-short>MDSetENCOn</unmanaged-short>
        MDSetENCOn = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDSetENCOff</unmanaged>
        /// <unmanaged-short>MDSetENCOff</unmanaged-short>
        MDSetENCOff = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDUpdateENC</unmanaged>
        /// <unmanaged-short>MDUpdateENC</unmanaged-short>
        MDUpdateENC = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDUpdateFull</unmanaged>
        /// <unmanaged-short>MDUpdateFull</unmanaged-short>
        MDUpdateFull = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDUpdateExtension</unmanaged>
        /// <unmanaged-short>MDUpdateExtension</unmanaged-short>
        MDUpdateExtension = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDUpdateIncremental</unmanaged>
        /// <unmanaged-short>MDUpdateIncremental</unmanaged-short>
        MDUpdateIncremental = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDUpdateDelta</unmanaged>
        /// <unmanaged-short>MDUpdateDelta</unmanaged-short>
        MDUpdateDelta = unchecked ((System.Int32)(5)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDUpdateMask</unmanaged>
        /// <unmanaged-short>MDUpdateMask</unmanaged-short>
        MDUpdateMask = unchecked ((System.Int32)(7))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorThreadSafetyOptions</unmanaged>
    /// <unmanaged-short>CorThreadSafetyOptions</unmanaged-short>
    public enum CorThreadSafetyOptions : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDThreadSafetyDefault</unmanaged>
        /// <unmanaged-short>MDThreadSafetyDefault</unmanaged-short>
        MDThreadSafetyDefault = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDThreadSafetyOff</unmanaged>
        /// <unmanaged-short>MDThreadSafetyOff</unmanaged-short>
        MDThreadSafetyOff = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MDThreadSafetyOn</unmanaged>
        /// <unmanaged-short>MDThreadSafetyOn</unmanaged-short>
        MDThreadSafetyOn = unchecked ((System.Int32)(1))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorTokenType</unmanaged>
    /// <unmanaged-short>CorTokenType</unmanaged-short>
    public enum CorTokenType : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdtModule</unmanaged>
        /// <unmanaged-short>mdtModule</unmanaged-short>
        MdtModule = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdtTypeRef</unmanaged>
        /// <unmanaged-short>mdtTypeRef</unmanaged-short>
        MdtTypeRef = unchecked ((System.Int32)(16777216)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdtTypeDef</unmanaged>
        /// <unmanaged-short>mdtTypeDef</unmanaged-short>
        MdtTypeDef = unchecked ((System.Int32)(33554432)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdtFieldDef</unmanaged>
        /// <unmanaged-short>mdtFieldDef</unmanaged-short>
        MdtFieldDef = unchecked ((System.Int32)(67108864)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdtMethodDef</unmanaged>
        /// <unmanaged-short>mdtMethodDef</unmanaged-short>
        MdtMethodDef = unchecked ((System.Int32)(100663296)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdtParamDef</unmanaged>
        /// <unmanaged-short>mdtParamDef</unmanaged-short>
        MdtParamDef = unchecked ((System.Int32)(134217728)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdtInterfaceImpl</unmanaged>
        /// <unmanaged-short>mdtInterfaceImpl</unmanaged-short>
        MdtInterfaceImpl = unchecked ((System.Int32)(150994944)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdtMemberRef</unmanaged>
        /// <unmanaged-short>mdtMemberRef</unmanaged-short>
        MdtMemberRef = unchecked ((System.Int32)(167772160)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdtCustomAttribute</unmanaged>
        /// <unmanaged-short>mdtCustomAttribute</unmanaged-short>
        MdtCustomAttribute = unchecked ((System.Int32)(201326592)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdtPermission</unmanaged>
        /// <unmanaged-short>mdtPermission</unmanaged-short>
        MdtPermission = unchecked ((System.Int32)(234881024)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdtSignature</unmanaged>
        /// <unmanaged-short>mdtSignature</unmanaged-short>
        MdtSignature = unchecked ((System.Int32)(285212672)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdtEvent</unmanaged>
        /// <unmanaged-short>mdtEvent</unmanaged-short>
        MdtEvent = unchecked ((System.Int32)(335544320)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdtProperty</unmanaged>
        /// <unmanaged-short>mdtProperty</unmanaged-short>
        MdtProperty = unchecked ((System.Int32)(385875968)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdtMethodImpl</unmanaged>
        /// <unmanaged-short>mdtMethodImpl</unmanaged-short>
        MdtMethodImpl = unchecked ((System.Int32)(419430400)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdtModuleRef</unmanaged>
        /// <unmanaged-short>mdtModuleRef</unmanaged-short>
        MdtModuleRef = unchecked ((System.Int32)(436207616)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdtTypeSpec</unmanaged>
        /// <unmanaged-short>mdtTypeSpec</unmanaged-short>
        MdtTypeSpec = unchecked ((System.Int32)(452984832)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdtAssembly</unmanaged>
        /// <unmanaged-short>mdtAssembly</unmanaged-short>
        MdtAssembly = unchecked ((System.Int32)(536870912)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdtAssemblyRef</unmanaged>
        /// <unmanaged-short>mdtAssemblyRef</unmanaged-short>
        MdtAssemblyRef = unchecked ((System.Int32)(587202560)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdtFile</unmanaged>
        /// <unmanaged-short>mdtFile</unmanaged-short>
        MdtFile = unchecked ((System.Int32)(637534208)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdtExportedType</unmanaged>
        /// <unmanaged-short>mdtExportedType</unmanaged-short>
        MdtExportedType = unchecked ((System.Int32)(654311424)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdtManifestResource</unmanaged>
        /// <unmanaged-short>mdtManifestResource</unmanaged-short>
        MdtManifestResource = unchecked ((System.Int32)(671088640)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdtGenericParam</unmanaged>
        /// <unmanaged-short>mdtGenericParam</unmanaged-short>
        MdtGenericParam = unchecked ((System.Int32)(704643072)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdtMethodSpec</unmanaged>
        /// <unmanaged-short>mdtMethodSpec</unmanaged-short>
        MdtMethodSpec = unchecked ((System.Int32)(721420288)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdtGenericParamConstraint</unmanaged>
        /// <unmanaged-short>mdtGenericParamConstraint</unmanaged-short>
        MdtGenericParamConstraint = unchecked ((System.Int32)(738197504)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdtString</unmanaged>
        /// <unmanaged-short>mdtString</unmanaged-short>
        MdtString = unchecked ((System.Int32)(1879048192)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdtName</unmanaged>
        /// <unmanaged-short>mdtName</unmanaged-short>
        MdtName = unchecked ((System.Int32)(1895825408)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>mdtBaseType</unmanaged>
        /// <unmanaged-short>mdtBaseType</unmanaged-short>
        MdtBaseType = unchecked ((System.Int32)(1912602624))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorTypeAttr</unmanaged>
    /// <unmanaged-short>CorTypeAttr</unmanaged-short>
    public enum CorTypeAttr : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>tdVisibilityMask</unmanaged>
        /// <unmanaged-short>tdVisibilityMask</unmanaged-short>
        TdVisibilityMask = unchecked ((System.Int32)(7)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>tdNotPublic</unmanaged>
        /// <unmanaged-short>tdNotPublic</unmanaged-short>
        TdNotPublic = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>tdPublic</unmanaged>
        /// <unmanaged-short>tdPublic</unmanaged-short>
        TdPublic = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>tdNestedPublic</unmanaged>
        /// <unmanaged-short>tdNestedPublic</unmanaged-short>
        TdNestedPublic = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>tdNestedPrivate</unmanaged>
        /// <unmanaged-short>tdNestedPrivate</unmanaged-short>
        TdNestedPrivate = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>tdNestedFamily</unmanaged>
        /// <unmanaged-short>tdNestedFamily</unmanaged-short>
        TdNestedFamily = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>tdNestedAssembly</unmanaged>
        /// <unmanaged-short>tdNestedAssembly</unmanaged-short>
        TdNestedAssembly = unchecked ((System.Int32)(5)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>tdNestedFamANDAssem</unmanaged>
        /// <unmanaged-short>tdNestedFamANDAssem</unmanaged-short>
        TdNestedFamANDAssem = unchecked ((System.Int32)(6)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>tdNestedFamORAssem</unmanaged>
        /// <unmanaged-short>tdNestedFamORAssem</unmanaged-short>
        TdNestedFamORAssem = unchecked ((System.Int32)(7)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>tdLayoutMask</unmanaged>
        /// <unmanaged-short>tdLayoutMask</unmanaged-short>
        TdLayoutMask = unchecked ((System.Int32)(24)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>tdAutoLayout</unmanaged>
        /// <unmanaged-short>tdAutoLayout</unmanaged-short>
        TdAutoLayout = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>tdSequentialLayout</unmanaged>
        /// <unmanaged-short>tdSequentialLayout</unmanaged-short>
        TdSequentialLayout = unchecked ((System.Int32)(8)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>tdExplicitLayout</unmanaged>
        /// <unmanaged-short>tdExplicitLayout</unmanaged-short>
        TdExplicitLayout = unchecked ((System.Int32)(16)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>tdClassSemanticsMask</unmanaged>
        /// <unmanaged-short>tdClassSemanticsMask</unmanaged-short>
        TdClassSemanticsMask = unchecked ((System.Int32)(32)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>tdClass</unmanaged>
        /// <unmanaged-short>tdClass</unmanaged-short>
        TdClass = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>tdInterface</unmanaged>
        /// <unmanaged-short>tdInterface</unmanaged-short>
        TdInterface = unchecked ((System.Int32)(32)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>tdAbstract</unmanaged>
        /// <unmanaged-short>tdAbstract</unmanaged-short>
        TdAbstract = unchecked ((System.Int32)(128)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>tdSealed</unmanaged>
        /// <unmanaged-short>tdSealed</unmanaged-short>
        TdSealed = unchecked ((System.Int32)(256)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>tdSpecialName</unmanaged>
        /// <unmanaged-short>tdSpecialName</unmanaged-short>
        TdSpecialName = unchecked ((System.Int32)(1024)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>tdImport</unmanaged>
        /// <unmanaged-short>tdImport</unmanaged-short>
        TdImport = unchecked ((System.Int32)(4096)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>tdSerializable</unmanaged>
        /// <unmanaged-short>tdSerializable</unmanaged-short>
        TdSerializable = unchecked ((System.Int32)(8192)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>tdWindowsRuntime</unmanaged>
        /// <unmanaged-short>tdWindowsRuntime</unmanaged-short>
        TdWindowsRuntime = unchecked ((System.Int32)(16384)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>tdStringFormatMask</unmanaged>
        /// <unmanaged-short>tdStringFormatMask</unmanaged-short>
        TdStringFormatMask = unchecked ((System.Int32)(196608)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>tdAnsiClass</unmanaged>
        /// <unmanaged-short>tdAnsiClass</unmanaged-short>
        TdAnsiClass = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>tdUnicodeClass</unmanaged>
        /// <unmanaged-short>tdUnicodeClass</unmanaged-short>
        TdUnicodeClass = unchecked ((System.Int32)(65536)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>tdAutoClass</unmanaged>
        /// <unmanaged-short>tdAutoClass</unmanaged-short>
        TdAutoClass = unchecked ((System.Int32)(131072)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>tdCustomFormatClass</unmanaged>
        /// <unmanaged-short>tdCustomFormatClass</unmanaged-short>
        TdCustomFormatClass = unchecked ((System.Int32)(196608)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>tdCustomFormatMask</unmanaged>
        /// <unmanaged-short>tdCustomFormatMask</unmanaged-short>
        TdCustomFormatMask = unchecked ((System.Int32)(12582912)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>tdBeforeFieldInit</unmanaged>
        /// <unmanaged-short>tdBeforeFieldInit</unmanaged-short>
        TdBeforeFieldInit = unchecked ((System.Int32)(1048576)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>tdForwarder</unmanaged>
        /// <unmanaged-short>tdForwarder</unmanaged-short>
        TdForwarder = unchecked ((System.Int32)(2097152)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>tdReservedMask</unmanaged>
        /// <unmanaged-short>tdReservedMask</unmanaged-short>
        TdReservedMask = unchecked ((System.Int32)(264192)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>tdRTSpecialName</unmanaged>
        /// <unmanaged-short>tdRTSpecialName</unmanaged-short>
        TdRTSpecialName = unchecked ((System.Int32)(2048)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>tdHasSecurity</unmanaged>
        /// <unmanaged-short>tdHasSecurity</unmanaged-short>
        TdHasSecurity = unchecked ((System.Int32)(262144))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorUnmanagedCallingConvention</unmanaged>
    /// <unmanaged-short>CorUnmanagedCallingConvention</unmanaged-short>
    public enum CorUnmanagedCallingConvention : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IMAGE_CEE_UNMANAGED_CALLCONV_C</unmanaged>
        /// <unmanaged-short>IMAGE_CEE_UNMANAGED_CALLCONV_C</unmanaged-short>
        ImageCeeUnmanagedCallconvC = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IMAGE_CEE_UNMANAGED_CALLCONV_STDCALL</unmanaged>
        /// <unmanaged-short>IMAGE_CEE_UNMANAGED_CALLCONV_STDCALL</unmanaged-short>
        ImageCeeUnmanagedCallconvStdcall = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IMAGE_CEE_UNMANAGED_CALLCONV_THISCALL</unmanaged>
        /// <unmanaged-short>IMAGE_CEE_UNMANAGED_CALLCONV_THISCALL</unmanaged-short>
        ImageCeeUnmanagedCallconvThiscall = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IMAGE_CEE_UNMANAGED_CALLCONV_FASTCALL</unmanaged>
        /// <unmanaged-short>IMAGE_CEE_UNMANAGED_CALLCONV_FASTCALL</unmanaged-short>
        ImageCeeUnmanagedCallconvFastcall = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IMAGE_CEE_CS_CALLCONV_C</unmanaged>
        /// <unmanaged-short>IMAGE_CEE_CS_CALLCONV_C</unmanaged-short>
        ImageCeeCsCallconvC = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IMAGE_CEE_CS_CALLCONV_STDCALL</unmanaged>
        /// <unmanaged-short>IMAGE_CEE_CS_CALLCONV_STDCALL</unmanaged-short>
        ImageCeeCsCallconvStdcall = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IMAGE_CEE_CS_CALLCONV_THISCALL</unmanaged>
        /// <unmanaged-short>IMAGE_CEE_CS_CALLCONV_THISCALL</unmanaged-short>
        ImageCeeCsCallconvThiscall = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IMAGE_CEE_CS_CALLCONV_FASTCALL</unmanaged>
        /// <unmanaged-short>IMAGE_CEE_CS_CALLCONV_FASTCALL</unmanaged-short>
        ImageCeeCsCallconvFastcall = unchecked ((System.Int32)(4))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorValidatorModuleType</unmanaged>
    /// <unmanaged-short>CorValidatorModuleType</unmanaged-short>
    public enum CorValidatorModuleType : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ValidatorModuleTypeInvalid</unmanaged>
        /// <unmanaged-short>ValidatorModuleTypeInvalid</unmanaged-short>
        ValidatorModuleTypeInvalid = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ValidatorModuleTypeMin</unmanaged>
        /// <unmanaged-short>ValidatorModuleTypeMin</unmanaged-short>
        ValidatorModuleTypeMin = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ValidatorModuleTypePE</unmanaged>
        /// <unmanaged-short>ValidatorModuleTypePE</unmanaged-short>
        ValidatorModuleTypePE = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ValidatorModuleTypeObj</unmanaged>
        /// <unmanaged-short>ValidatorModuleTypeObj</unmanaged-short>
        ValidatorModuleTypeObj = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ValidatorModuleTypeEnc</unmanaged>
        /// <unmanaged-short>ValidatorModuleTypeEnc</unmanaged-short>
        ValidatorModuleTypeEnc = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ValidatorModuleTypeIncr</unmanaged>
        /// <unmanaged-short>ValidatorModuleTypeIncr</unmanaged-short>
        ValidatorModuleTypeIncr = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ValidatorModuleTypeMax</unmanaged>
        /// <unmanaged-short>ValidatorModuleTypeMax</unmanaged-short>
        ValidatorModuleTypeMax = unchecked ((System.Int32)(4))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>COUNINITIEE</unmanaged>
    /// <unmanaged-short>COUNINITIEE</unmanaged-short>
    public enum Couninitiee : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>COUNINITEE_DEFAULT</unmanaged>
        /// <unmanaged-short>COUNINITEE_DEFAULT</unmanaged-short>
        EeDefault = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>COUNINITEE_DLL</unmanaged>
        /// <unmanaged-short>COUNINITEE_DLL</unmanaged-short>
        EeDll = unchecked ((System.Int32)(1))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>ILCodeKind</unmanaged>
    /// <unmanaged-short>ILCodeKind</unmanaged-short>
    public enum ILCodeKind : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ILCODE_ORIGINAL_IL</unmanaged>
        /// <unmanaged-short>ILCODE_ORIGINAL_IL</unmanaged-short>
        IlcodeOriginalIl = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ILCODE_REJIT_IL</unmanaged>
        /// <unmanaged-short>ILCODE_REJIT_IL</unmanaged-short>
        IlcodeRejitIl = unchecked ((System.Int32)(2))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>LoadHintEnum</unmanaged>
    /// <unmanaged-short>LoadHintEnum</unmanaged-short>
    public enum LoadHintEnum : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>LoadDefault</unmanaged>
        /// <unmanaged-short>LoadDefault</unmanaged-short>
        LoadDefault = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>LoadAlways</unmanaged>
        /// <unmanaged-short>LoadAlways</unmanaged-short>
        LoadAlways = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>LoadSometimes</unmanaged>
        /// <unmanaged-short>LoadSometimes</unmanaged-short>
        LoadSometimes = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>LoadNever</unmanaged>
        /// <unmanaged-short>LoadNever</unmanaged-short>
        LoadNever = unchecked ((System.Int32)(3))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>LoggingLevelEnum</unmanaged>
    /// <unmanaged-short>LoggingLevelEnum</unmanaged-short>
    public enum LoggingLevelEnum : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>LTraceLevel0</unmanaged>
        /// <unmanaged-short>LTraceLevel0</unmanaged-short>
        LTraceLevel0 = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>LTraceLevel1</unmanaged>
        /// <unmanaged-short>LTraceLevel1</unmanaged-short>
        LTraceLevel1 = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>LTraceLevel2</unmanaged>
        /// <unmanaged-short>LTraceLevel2</unmanaged-short>
        LTraceLevel2 = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>LTraceLevel3</unmanaged>
        /// <unmanaged-short>LTraceLevel3</unmanaged-short>
        LTraceLevel3 = unchecked ((System.Int32)(3)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>LTraceLevel4</unmanaged>
        /// <unmanaged-short>LTraceLevel4</unmanaged-short>
        LTraceLevel4 = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>LStatusLevel0</unmanaged>
        /// <unmanaged-short>LStatusLevel0</unmanaged-short>
        LStatusLevel0 = unchecked ((System.Int32)(20)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>LStatusLevel1</unmanaged>
        /// <unmanaged-short>LStatusLevel1</unmanaged-short>
        LStatusLevel1 = unchecked ((System.Int32)(21)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>LStatusLevel2</unmanaged>
        /// <unmanaged-short>LStatusLevel2</unmanaged-short>
        LStatusLevel2 = unchecked ((System.Int32)(22)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>LStatusLevel3</unmanaged>
        /// <unmanaged-short>LStatusLevel3</unmanaged-short>
        LStatusLevel3 = unchecked ((System.Int32)(23)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>LStatusLevel4</unmanaged>
        /// <unmanaged-short>LStatusLevel4</unmanaged-short>
        LStatusLevel4 = unchecked ((System.Int32)(24)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>LWarningLevel</unmanaged>
        /// <unmanaged-short>LWarningLevel</unmanaged-short>
        LWarningLevel = unchecked ((System.Int32)(40)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>LErrorLevel</unmanaged>
        /// <unmanaged-short>LErrorLevel</unmanaged-short>
        LErrorLevel = unchecked ((System.Int32)(50)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>LPanicLevel</unmanaged>
        /// <unmanaged-short>LPanicLevel</unmanaged-short>
        LPanicLevel = unchecked ((System.Int32)(100))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>LogSwitchCallReason</unmanaged>
    /// <unmanaged-short>LogSwitchCallReason</unmanaged-short>
    public enum LogSwitchCallReason : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>SWITCH_CREATE</unmanaged>
        /// <unmanaged-short>SWITCH_CREATE</unmanaged-short>
        SwitchCreate = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>SWITCH_MODIFY</unmanaged>
        /// <unmanaged-short>SWITCH_MODIFY</unmanaged-short>
        SwitchModify = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>SWITCH_DELETE</unmanaged>
        /// <unmanaged-short>SWITCH_DELETE</unmanaged-short>
        SwitchDelete = unchecked ((System.Int32)(2))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>MergeFlags</unmanaged>
    /// <unmanaged-short>MergeFlags</unmanaged-short>
    public enum MergeFlags : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MergeFlagsNone</unmanaged>
        /// <unmanaged-short>MergeFlagsNone</unmanaged-short>
        MergeFlagsNone = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MergeManifest</unmanaged>
        /// <unmanaged-short>MergeManifest</unmanaged-short>
        MergeManifest = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>DropMemberRefCAs</unmanaged>
        /// <unmanaged-short>DropMemberRefCAs</unmanaged-short>
        DropMemberRefCAs = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NoDupCheck</unmanaged>
        /// <unmanaged-short>NoDupCheck</unmanaged-short>
        NoDupCheck = unchecked ((System.Int32)(4)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MergeExportedTypes</unmanaged>
        /// <unmanaged-short>MergeExportedTypes</unmanaged-short>
        MergeExportedTypes = unchecked ((System.Int32)(8))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>NativeTypeArrayFlags</unmanaged>
    /// <unmanaged-short>NativeTypeArrayFlags</unmanaged-short>
    public enum NativeTypeArrayFlags : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ntaSizeParamIndexSpecified</unmanaged>
        /// <unmanaged-short>ntaSizeParamIndexSpecified</unmanaged-short>
        NtaSizeParamIndexSpecified = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ntaReserved</unmanaged>
        /// <unmanaged-short>ntaReserved</unmanaged-short>
        NtaReserved = unchecked ((System.Int32)(65534))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>NGenHintEnum</unmanaged>
    /// <unmanaged-short>NGenHintEnum</unmanaged-short>
    public enum NGenHintEnum : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NGenDefault</unmanaged>
        /// <unmanaged-short>NGenDefault</unmanaged-short>
        NGenDefault = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NGenEager</unmanaged>
        /// <unmanaged-short>NGenEager</unmanaged-short>
        NGenEager = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NGenLazy</unmanaged>
        /// <unmanaged-short>NGenLazy</unmanaged-short>
        NGenLazy = unchecked ((System.Int32)(2)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>NGenNever</unmanaged>
        /// <unmanaged-short>NGenNever</unmanaged-short>
        NGenNever = unchecked ((System.Int32)(3))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>ReplacesGeneralNumericDefines</unmanaged>
    /// <unmanaged-short>ReplacesGeneralNumericDefines</unmanaged-short>
    public enum ReplacesGeneralNumericDefines : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IMAGE_DIRECTORY_ENTRY_COMHEADER</unmanaged>
        /// <unmanaged-short>IMAGE_DIRECTORY_ENTRY_COMHEADER</unmanaged-short>
        ImageDirectoryEntryComheader = unchecked ((System.Int32)(14))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>VariableLocationType</unmanaged>
    /// <unmanaged-short>VariableLocationType</unmanaged-short>
    public enum VariableLocationType : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>VLT_REGISTER</unmanaged>
        /// <unmanaged-short>VLT_REGISTER</unmanaged-short>
        VltRegister = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>VLT_REGISTER_RELATIVE</unmanaged>
        /// <unmanaged-short>VLT_REGISTER_RELATIVE</unmanaged-short>
        VltRegisterRelative = unchecked ((System.Int32)(1)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>VLT_INVALID</unmanaged>
        /// <unmanaged-short>VLT_INVALID</unmanaged-short>
        VltInvalid = unchecked ((System.Int32)(2))}

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>WriteableMetadataUpdateMode</unmanaged>
    /// <unmanaged-short>WriteableMetadataUpdateMode</unmanaged-short>
    public enum WriteableMetadataUpdateMode : System.Int32
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>LegacyCompatPolicy</unmanaged>
        /// <unmanaged-short>LegacyCompatPolicy</unmanaged-short>
        LegacyCompatPolicy = unchecked ((System.Int32)(0)),
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>AlwaysShowUpdates</unmanaged>
        /// <unmanaged-short>AlwaysShowUpdates</unmanaged-short>
        AlwaysShowUpdates = unchecked ((System.Int32)(1))}
}