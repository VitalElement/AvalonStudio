namespace CorApi.Portable
{
    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>ASSEMBLYMETADATA</unmanaged>
    /// <unmanaged-short>ASSEMBLYMETADATA</unmanaged-short>
    public partial struct Assemblymetadata
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>usMajorVersion</unmanaged>
        /// <unmanaged-short>usMajorVersion</unmanaged-short>
        public System.UInt16 UsMajorVersion;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>usMinorVersion</unmanaged>
        /// <unmanaged-short>usMinorVersion</unmanaged-short>
        public System.UInt16 UsMinorVersion;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>usBuildNumber</unmanaged>
        /// <unmanaged-short>usBuildNumber</unmanaged-short>
        public System.UInt16 UsBuildNumber;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>usRevisionNumber</unmanaged>
        /// <unmanaged-short>usRevisionNumber</unmanaged-short>
        public System.UInt16 UsRevisionNumber;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>szLocale</unmanaged>
        /// <unmanaged-short>szLocale</unmanaged-short>
        public System.String SzLocale;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>cbLocale</unmanaged>
        /// <unmanaged-short>cbLocale</unmanaged-short>
        public System.UInt32 CbLocale;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>rProcessor</unmanaged>
        /// <unmanaged-short>rProcessor</unmanaged-short>
        public System.IntPtr RProcessor;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ulProcessor</unmanaged>
        /// <unmanaged-short>ulProcessor</unmanaged-short>
        public System.UInt32 UlProcessor;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>rOS</unmanaged>
        /// <unmanaged-short>rOS</unmanaged-short>
        public System.IntPtr ROS;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ulOS</unmanaged>
        /// <unmanaged-short>ulOS</unmanaged-short>
        public System.UInt32 UlOS;
        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]
        internal partial struct __Native
        {
            public System.UInt16 UsMajorVersion;
            public System.UInt16 UsMinorVersion;
            public System.UInt16 UsBuildNumber;
            public System.UInt16 UsRevisionNumber;
            public System.IntPtr SzLocale;
            public System.UInt32 CbLocale;
            public System.IntPtr RProcessor;
            public System.UInt32 UlProcessor;
            public System.IntPtr ROS;
            public System.UInt32 UlOS;
            internal unsafe void __MarshalFree()
            {
                if (this.SzLocale != System.IntPtr.Zero)
                    System.Runtime.InteropServices.Marshal.FreeHGlobal(this.SzLocale);
            }
        }

        internal void __MarshalFree(ref __Native @ref) => @ref.__MarshalFree();
        internal unsafe void __MarshalFrom(ref __Native @ref)
        {
            this.UsMajorVersion = @ref.UsMajorVersion;
            this.UsMinorVersion = @ref.UsMinorVersion;
            this.UsBuildNumber = @ref.UsBuildNumber;
            this.UsRevisionNumber = @ref.UsRevisionNumber;
            this.SzLocale = @ref.SzLocale == System.IntPtr.Zero ? null : System.Runtime.InteropServices.Marshal.PtrToStringUni(@ref.SzLocale);
            this.CbLocale = @ref.CbLocale;
            this.RProcessor = @ref.RProcessor;
            this.UlProcessor = @ref.UlProcessor;
            this.ROS = @ref.ROS;
            this.UlOS = @ref.UlOS;
        }

        internal unsafe void __MarshalTo(ref __Native @ref)
        {
            @ref.UsMajorVersion = this.UsMajorVersion;
            @ref.UsMinorVersion = this.UsMinorVersion;
            @ref.UsBuildNumber = this.UsBuildNumber;
            @ref.UsRevisionNumber = this.UsRevisionNumber;
            @ref.SzLocale = this.SzLocale == null ? System.IntPtr.Zero : System.Runtime.InteropServices.Marshal.StringToHGlobalUni(this.SzLocale);
            @ref.CbLocale = this.CbLocale;
            @ref.RProcessor = this.RProcessor;
            @ref.UlProcessor = this.UlProcessor;
            @ref.ROS = this.ROS;
            @ref.UlOS = this.UlOS;
        }
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CeeSectionRelocExtra</unmanaged>
    /// <unmanaged-short>CeeSectionRelocExtra</unmanaged-short>
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]
    public partial struct CeeSectionRelocExtra
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>highAdj</unmanaged>
        /// <unmanaged-short>highAdj</unmanaged-short>
        public System.UInt16 HighAdj;
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CodeChunkInfo</unmanaged>
    /// <unmanaged-short>CodeChunkInfo</unmanaged-short>
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]
    public partial struct CodeChunkInfo
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>startAddr</unmanaged>
        /// <unmanaged-short>startAddr</unmanaged-short>
        public System.UInt64 StartAddr;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>length</unmanaged>
        /// <unmanaged-short>length</unmanaged-short>
        public System.UInt32 Length;
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>COR_ACTIVE_FUNCTION</unmanaged>
    /// <unmanaged-short>COR_ACTIVE_FUNCTION</unmanaged-short>
    public partial struct CorActiveFunction
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pAppDomain</unmanaged>
        /// <unmanaged-short>pAppDomain</unmanaged-short>
        public System.IntPtr PAppDomain;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pModule</unmanaged>
        /// <unmanaged-short>pModule</unmanaged-short>
        public System.IntPtr PModule;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pFunction</unmanaged>
        /// <unmanaged-short>pFunction</unmanaged-short>
        public System.IntPtr PFunction;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ilOffset</unmanaged>
        /// <unmanaged-short>ilOffset</unmanaged-short>
        public System.UInt32 IlOffset;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>flags</unmanaged>
        /// <unmanaged-short>flags</unmanaged-short>
        public System.UInt32 Flags;
        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]
        internal partial struct __Native
        {
            public System.IntPtr PAppDomain;
            public System.IntPtr PModule;
            public System.IntPtr PFunction;
            public System.UInt32 IlOffset;
            public System.UInt32 Flags;
            internal unsafe void __MarshalFree()
            {
            }
        }

        internal void __MarshalFree(ref __Native @ref) => @ref.__MarshalFree();
        internal unsafe void __MarshalFrom(ref __Native @ref)
        {
            this.PAppDomain = @ref.PAppDomain;
            this.PModule = @ref.PModule;
            this.PFunction = @ref.PFunction;
            this.IlOffset = @ref.IlOffset;
            this.Flags = @ref.Flags;
        }

        internal unsafe void __MarshalTo(ref __Native @ref)
        {
            @ref.PAppDomain = this.PAppDomain;
            @ref.PModule = this.PModule;
            @ref.PFunction = this.PFunction;
            @ref.IlOffset = this.IlOffset;
            @ref.Flags = this.Flags;
        }
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>COR_ARRAY_LAYOUT</unmanaged>
    /// <unmanaged-short>COR_ARRAY_LAYOUT</unmanaged-short>
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]
    public partial struct CorArrayLayout
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>componentID</unmanaged>
        /// <unmanaged-short>componentID</unmanaged-short>
        public CorApi.Portable.CorTypeid ComponentID;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>componentType</unmanaged>
        /// <unmanaged-short>componentType</unmanaged-short>
        public CorApi.Portable.CorElementType ComponentType;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>firstElementOffset</unmanaged>
        /// <unmanaged-short>firstElementOffset</unmanaged-short>
        public System.UInt32 FirstElementOffset;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>elementSize</unmanaged>
        /// <unmanaged-short>elementSize</unmanaged-short>
        public System.UInt32 ElementSize;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>countOffset</unmanaged>
        /// <unmanaged-short>countOffset</unmanaged-short>
        public System.UInt32 CountOffset;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>rankSize</unmanaged>
        /// <unmanaged-short>rankSize</unmanaged-short>
        public System.UInt32 RankSize;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>numRanks</unmanaged>
        /// <unmanaged-short>numRanks</unmanaged-short>
        public System.UInt32 NumRanks;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>rankOffset</unmanaged>
        /// <unmanaged-short>rankOffset</unmanaged-short>
        public System.UInt32 RankOffset;
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugBlockingObject</unmanaged>
    /// <unmanaged-short>CorDebugBlockingObject</unmanaged-short>
    public partial struct CorDebugBlockingObject
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pBlockingObject</unmanaged>
        /// <unmanaged-short>pBlockingObject</unmanaged-short>
        public System.IntPtr PBlockingObject;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>dwTimeout</unmanaged>
        /// <unmanaged-short>dwTimeout</unmanaged-short>
        public System.UInt32 DwTimeout;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>blockingReason</unmanaged>
        /// <unmanaged-short>blockingReason</unmanaged-short>
        public CorApi.Portable.CorDebugBlockingReason BlockingReason;
        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]
        internal partial struct __Native
        {
            public System.IntPtr PBlockingObject;
            public System.UInt32 DwTimeout;
            public CorApi.Portable.CorDebugBlockingReason BlockingReason;
            internal unsafe void __MarshalFree()
            {
            }
        }

        internal void __MarshalFree(ref __Native @ref) => @ref.__MarshalFree();
        internal unsafe void __MarshalFrom(ref __Native @ref)
        {
            this.PBlockingObject = @ref.PBlockingObject;
            this.DwTimeout = @ref.DwTimeout;
            this.BlockingReason = @ref.BlockingReason;
        }

        internal unsafe void __MarshalTo(ref __Native @ref)
        {
            @ref.PBlockingObject = this.PBlockingObject;
            @ref.DwTimeout = this.DwTimeout;
            @ref.BlockingReason = this.BlockingReason;
        }
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugEHClause</unmanaged>
    /// <unmanaged-short>CorDebugEHClause</unmanaged-short>
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]
    public partial struct CorDebugEHClause
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>Flags</unmanaged>
        /// <unmanaged-short>Flags</unmanaged-short>
        public System.UInt32 Flags;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>TryOffset</unmanaged>
        /// <unmanaged-short>TryOffset</unmanaged-short>
        public System.UInt32 TryOffset;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>TryLength</unmanaged>
        /// <unmanaged-short>TryLength</unmanaged-short>
        public System.UInt32 TryLength;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>HandlerOffset</unmanaged>
        /// <unmanaged-short>HandlerOffset</unmanaged-short>
        public System.UInt32 HandlerOffset;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>HandlerLength</unmanaged>
        /// <unmanaged-short>HandlerLength</unmanaged-short>
        public System.UInt32 HandlerLength;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ClassToken</unmanaged>
        /// <unmanaged-short>ClassToken</unmanaged-short>
        public System.UInt32 ClassToken;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>FilterOffset</unmanaged>
        /// <unmanaged-short>FilterOffset</unmanaged-short>
        public System.UInt32 FilterOffset;
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugExceptionObjectStackFrame</unmanaged>
    /// <unmanaged-short>CorDebugExceptionObjectStackFrame</unmanaged-short>
    public partial struct CorDebugExceptionObjectStackFrame
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pModule</unmanaged>
        /// <unmanaged-short>pModule</unmanaged-short>
        public System.IntPtr PModule;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ip</unmanaged>
        /// <unmanaged-short>ip</unmanaged-short>
        public System.UInt64 Ip;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>methodDef</unmanaged>
        /// <unmanaged-short>methodDef</unmanaged-short>
        public System.UInt32 MethodDef;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>isLastForeignExceptionFrame</unmanaged>
        /// <unmanaged-short>isLastForeignExceptionFrame</unmanaged-short>
        public SharpGen.Runtime.Win32.RawBool IsLastForeignExceptionFrame;
        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]
        internal partial struct __Native
        {
            public System.IntPtr PModule;
            public System.UInt64 Ip;
            public System.UInt32 MethodDef;
            public SharpGen.Runtime.Win32.RawBool IsLastForeignExceptionFrame;
            internal unsafe void __MarshalFree()
            {
            }
        }

        internal void __MarshalFree(ref __Native @ref) => @ref.__MarshalFree();
        internal unsafe void __MarshalFrom(ref __Native @ref)
        {
            this.PModule = @ref.PModule;
            this.Ip = @ref.Ip;
            this.MethodDef = @ref.MethodDef;
            this.IsLastForeignExceptionFrame = @ref.IsLastForeignExceptionFrame;
        }

        internal unsafe void __MarshalTo(ref __Native @ref)
        {
            @ref.PModule = this.PModule;
            @ref.Ip = this.Ip;
            @ref.MethodDef = this.MethodDef;
            @ref.IsLastForeignExceptionFrame = this.IsLastForeignExceptionFrame;
        }
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CorDebugGuidToTypeMapping</unmanaged>
    /// <unmanaged-short>CorDebugGuidToTypeMapping</unmanaged-short>
    public partial struct CorDebugGuidToTypeMapping
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>iid</unmanaged>
        /// <unmanaged-short>iid</unmanaged-short>
        public System.Guid Iid;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pType</unmanaged>
        /// <unmanaged-short>pType</unmanaged-short>
        public System.IntPtr PType;
        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]
        internal partial struct __Native
        {
            public System.Guid Iid;
            public System.IntPtr PType;
            internal unsafe void __MarshalFree()
            {
            }
        }

        internal void __MarshalFree(ref __Native @ref) => @ref.__MarshalFree();
        internal unsafe void __MarshalFrom(ref __Native @ref)
        {
            this.Iid = @ref.Iid;
            this.PType = @ref.PType;
        }

        internal unsafe void __MarshalTo(ref __Native @ref)
        {
            @ref.Iid = this.Iid;
            @ref.PType = this.PType;
        }
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>COR_DEBUG_IL_TO_NATIVE_MAP</unmanaged>
    /// <unmanaged-short>COR_DEBUG_IL_TO_NATIVE_MAP</unmanaged-short>
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]
    public partial struct CorDebugIlToNativeMap
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ilOffset</unmanaged>
        /// <unmanaged-short>ilOffset</unmanaged-short>
        public System.UInt32 IlOffset;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>nativeStartOffset</unmanaged>
        /// <unmanaged-short>nativeStartOffset</unmanaged-short>
        public System.UInt32 NativeStartOffset;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>nativeEndOffset</unmanaged>
        /// <unmanaged-short>nativeEndOffset</unmanaged-short>
        public System.UInt32 NativeEndOffset;
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>COR_DEBUG_STEP_RANGE</unmanaged>
    /// <unmanaged-short>COR_DEBUG_STEP_RANGE</unmanaged-short>
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]
    public partial struct CorDebugStepRange
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>startOffset</unmanaged>
        /// <unmanaged-short>startOffset</unmanaged-short>
        public System.UInt32 StartOffset;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>endOffset</unmanaged>
        /// <unmanaged-short>endOffset</unmanaged-short>
        public System.UInt32 EndOffset;
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>COR_FIELD</unmanaged>
    /// <unmanaged-short>COR_FIELD</unmanaged-short>
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]
    public partial struct CorField
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>token</unmanaged>
        /// <unmanaged-short>token</unmanaged-short>
        public System.UInt32 Token;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>offset</unmanaged>
        /// <unmanaged-short>offset</unmanaged-short>
        public System.UInt32 Offset;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>id</unmanaged>
        /// <unmanaged-short>id</unmanaged-short>
        public CorApi.Portable.CorTypeid Id;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>fieldType</unmanaged>
        /// <unmanaged-short>fieldType</unmanaged-short>
        public CorApi.Portable.CorElementType FieldType;
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>COR_FIELD_OFFSET</unmanaged>
    /// <unmanaged-short>COR_FIELD_OFFSET</unmanaged-short>
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]
    public partial struct CorFieldOffset
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ridOfField</unmanaged>
        /// <unmanaged-short>ridOfField</unmanaged-short>
        public System.UInt32 RidOfField;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ulOffset</unmanaged>
        /// <unmanaged-short>ulOffset</unmanaged-short>
        public System.UInt32 UlOffset;
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>COR_GC_REFERENCE</unmanaged>
    /// <unmanaged-short>COR_GC_REFERENCE</unmanaged-short>
    public partial struct CorGcReference
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>Domain</unmanaged>
        /// <unmanaged-short>Domain</unmanaged-short>
        public System.IntPtr Domain;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>Location</unmanaged>
        /// <unmanaged-short>Location</unmanaged-short>
        public System.IntPtr Location;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>Type</unmanaged>
        /// <unmanaged-short>Type</unmanaged-short>
        public CorApi.Portable.CorGCReferenceType Type;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ExtraData</unmanaged>
        /// <unmanaged-short>ExtraData</unmanaged-short>
        public System.UInt64 ExtraData;
        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]
        internal partial struct __Native
        {
            public System.IntPtr Domain;
            public System.IntPtr Location;
            public CorApi.Portable.CorGCReferenceType Type;
            public System.UInt64 ExtraData;
            internal unsafe void __MarshalFree()
            {
            }
        }

        internal void __MarshalFree(ref __Native @ref) => @ref.__MarshalFree();
        internal unsafe void __MarshalFrom(ref __Native @ref)
        {
            this.Domain = @ref.Domain;
            this.Location = @ref.Location;
            this.Type = @ref.Type;
            this.ExtraData = @ref.ExtraData;
        }

        internal unsafe void __MarshalTo(ref __Native @ref)
        {
            @ref.Domain = this.Domain;
            @ref.Location = this.Location;
            @ref.Type = this.Type;
            @ref.ExtraData = this.ExtraData;
        }
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>COR_HEAPINFO</unmanaged>
    /// <unmanaged-short>COR_HEAPINFO</unmanaged-short>
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]
    public partial struct CorHeapinfo
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>areGCStructuresValid</unmanaged>
        /// <unmanaged-short>areGCStructuresValid</unmanaged-short>
        public SharpGen.Runtime.Win32.RawBool AreGCStructuresValid;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pointerSize</unmanaged>
        /// <unmanaged-short>pointerSize</unmanaged-short>
        public System.UInt32 PointerSize;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>numHeaps</unmanaged>
        /// <unmanaged-short>numHeaps</unmanaged-short>
        public System.UInt32 NumHeaps;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>concurrent</unmanaged>
        /// <unmanaged-short>concurrent</unmanaged-short>
        public SharpGen.Runtime.Win32.RawBool Concurrent;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>gcType</unmanaged>
        /// <unmanaged-short>gcType</unmanaged-short>
        public CorApi.Portable.CorDebugGCType GcType;
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>COR_HEAPOBJECT</unmanaged>
    /// <unmanaged-short>COR_HEAPOBJECT</unmanaged-short>
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]
    public partial struct CorHeapobject
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>address</unmanaged>
        /// <unmanaged-short>address</unmanaged-short>
        public System.UInt64 Address;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>size</unmanaged>
        /// <unmanaged-short>size</unmanaged-short>
        public System.UInt64 Size;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>type</unmanaged>
        /// <unmanaged-short>type</unmanaged-short>
        public CorApi.Portable.CorTypeid Type;
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>COR_IL_MAP</unmanaged>
    /// <unmanaged-short>COR_IL_MAP</unmanaged-short>
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]
    public partial struct CorIlMap
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>oldOffset</unmanaged>
        /// <unmanaged-short>oldOffset</unmanaged-short>
        public System.UInt32 OldOffset;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>newOffset</unmanaged>
        /// <unmanaged-short>newOffset</unmanaged-short>
        public System.UInt32 NewOffset;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>fAccurate</unmanaged>
        /// <unmanaged-short>fAccurate</unmanaged-short>
        public SharpGen.Runtime.Win32.RawBool FAccurate;
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>COR_NATIVE_LINK</unmanaged>
    /// <unmanaged-short>COR_NATIVE_LINK</unmanaged-short>
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]
    public partial struct CorNativeLink
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>m_linkType</unmanaged>
        /// <unmanaged-short>m_linkType</unmanaged-short>
        public System.Byte MLinkType;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>m_flags</unmanaged>
        /// <unmanaged-short>m_flags</unmanaged-short>
        public System.Byte MFlags;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>m_entryPoint</unmanaged>
        /// <unmanaged-short>m_entryPoint</unmanaged-short>
        public System.UInt32 MEntryPoint;
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>COR_SECATTR</unmanaged>
    /// <unmanaged-short>COR_SECATTR</unmanaged-short>
    public partial struct CorSecattr
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>tkCtor</unmanaged>
        /// <unmanaged-short>tkCtor</unmanaged-short>
        public System.UInt32 TkCtor;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>pCustomAttribute</unmanaged>
        /// <unmanaged-short>pCustomAttribute</unmanaged-short>
        public System.IntPtr PCustomAttribute;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>cbCustomAttribute</unmanaged>
        /// <unmanaged-short>cbCustomAttribute</unmanaged-short>
        public System.UInt32 CbCustomAttribute;
        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]
        internal partial struct __Native
        {
            public System.UInt32 TkCtor;
            public System.IntPtr PCustomAttribute;
            public System.UInt32 CbCustomAttribute;
            internal unsafe void __MarshalFree()
            {
            }
        }

        internal void __MarshalFree(ref __Native @ref) => @ref.__MarshalFree();
        internal unsafe void __MarshalFrom(ref __Native @ref)
        {
            this.TkCtor = @ref.TkCtor;
            this.PCustomAttribute = @ref.PCustomAttribute;
            this.CbCustomAttribute = @ref.CbCustomAttribute;
        }

        internal unsafe void __MarshalTo(ref __Native @ref)
        {
            @ref.TkCtor = this.TkCtor;
            @ref.PCustomAttribute = this.PCustomAttribute;
            @ref.CbCustomAttribute = this.CbCustomAttribute;
        }
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>COR_SEGMENT</unmanaged>
    /// <unmanaged-short>COR_SEGMENT</unmanaged-short>
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]
    public partial struct CorSegment
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>start</unmanaged>
        /// <unmanaged-short>start</unmanaged-short>
        public System.UInt64 Start;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>end</unmanaged>
        /// <unmanaged-short>end</unmanaged-short>
        public System.UInt64 End;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>type</unmanaged>
        /// <unmanaged-short>type</unmanaged-short>
        public CorApi.Portable.CorDebugGenerationTypes Type;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>heap</unmanaged>
        /// <unmanaged-short>heap</unmanaged-short>
        public System.UInt32 Heap;
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>COR_TYPEID</unmanaged>
    /// <unmanaged-short>COR_TYPEID</unmanaged-short>
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]
    public partial struct CorTypeid
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>token1</unmanaged>
        /// <unmanaged-short>token1</unmanaged-short>
        public System.UInt64 Token1;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>token2</unmanaged>
        /// <unmanaged-short>token2</unmanaged-short>
        public System.UInt64 Token2;
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>COR_TYPE_LAYOUT</unmanaged>
    /// <unmanaged-short>COR_TYPE_LAYOUT</unmanaged-short>
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]
    public partial struct CorTypeLayout
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>parentID</unmanaged>
        /// <unmanaged-short>parentID</unmanaged-short>
        public CorApi.Portable.CorTypeid ParentID;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>objectSize</unmanaged>
        /// <unmanaged-short>objectSize</unmanaged-short>
        public System.UInt32 ObjectSize;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>numFields</unmanaged>
        /// <unmanaged-short>numFields</unmanaged-short>
        public System.UInt32 NumFields;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>boxOffset</unmanaged>
        /// <unmanaged-short>boxOffset</unmanaged-short>
        public System.UInt32 BoxOffset;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>type</unmanaged>
        /// <unmanaged-short>type</unmanaged-short>
        public CorApi.Portable.CorElementType Type;
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>COR_VERSION</unmanaged>
    /// <unmanaged-short>COR_VERSION</unmanaged-short>
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]
    public partial struct CorVersion
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>dwMajor</unmanaged>
        /// <unmanaged-short>dwMajor</unmanaged-short>
        public System.UInt32 DwMajor;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>dwMinor</unmanaged>
        /// <unmanaged-short>dwMinor</unmanaged-short>
        public System.UInt32 DwMinor;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>dwBuild</unmanaged>
        /// <unmanaged-short>dwBuild</unmanaged-short>
        public System.UInt32 DwBuild;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>dwSubBuild</unmanaged>
        /// <unmanaged-short>dwSubBuild</unmanaged-short>
        public System.UInt32 DwSubBuild;
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>CVStruct</unmanaged>
    /// <unmanaged-short>CVStruct</unmanaged-short>
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]
    public partial struct CVStruct
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>Major</unmanaged>
        /// <unmanaged-short>Major</unmanaged-short>
        public System.Int16 Major;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>Minor</unmanaged>
        /// <unmanaged-short>Minor</unmanaged-short>
        public System.Int16 Minor;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>Sub</unmanaged>
        /// <unmanaged-short>Sub</unmanaged-short>
        public System.Int16 Sub;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>Build</unmanaged>
        /// <unmanaged-short>Build</unmanaged-short>
        public System.Int16 Build;
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>IMAGE_COR_ILMETHOD</unmanaged>
    /// <unmanaged-short>IMAGE_COR_ILMETHOD</unmanaged-short>
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Explicit, Pack = 0)]
    public partial struct ImageCorIlmethod
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>Tiny</unmanaged>
        /// <unmanaged-short>Tiny</unmanaged-short>
        [System.Runtime.InteropServices.FieldOffset(0)]
        public CorApi.Portable.ImageCorIlmethodTiny Tiny;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>Fat</unmanaged>
        /// <unmanaged-short>Fat</unmanaged-short>
        [System.Runtime.InteropServices.FieldOffset(0)]
        public CorApi.Portable.ImageCorIlmethodFat Fat;
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>IMAGE_COR_ILMETHOD_FAT</unmanaged>
    /// <unmanaged-short>IMAGE_COR_ILMETHOD_FAT</unmanaged-short>
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Explicit, Pack = 0)]
    public partial struct ImageCorIlmethodFat
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>Flags</unmanaged>
        /// <unmanaged-short>Flags</unmanaged-short>
        public System.UInt32 Flags
        {
            get => (System.UInt32)((_Flags >> 0) & 4095);
            set => this._Flags = (System.UInt32)((this._Flags & ~(4095 << 0)) | ((value & 4095) << 0));
        }

        [System.Runtime.InteropServices.FieldOffset(0)]
        internal System.UInt32 _Flags;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>Size</unmanaged>
        /// <unmanaged-short>Size</unmanaged-short>
        public System.UInt32 Size
        {
            get => (System.UInt32)((_Size >> 12) & 15);
            set => this._Size = (System.UInt32)((this._Size & ~(15 << 12)) | ((value & 15) << 12));
        }

        [System.Runtime.InteropServices.FieldOffset(0)]
        internal System.UInt32 _Size;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>MaxStack</unmanaged>
        /// <unmanaged-short>MaxStack</unmanaged-short>
        public System.UInt32 MaxStack
        {
            get => (System.UInt32)((_MaxStack >> 16) & 65535);
            set => this._MaxStack = (System.UInt32)((this._MaxStack & ~(65535 << 16)) | ((value & 65535) << 16));
        }

        [System.Runtime.InteropServices.FieldOffset(0)]
        internal System.UInt32 _MaxStack;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>CodeSize</unmanaged>
        /// <unmanaged-short>CodeSize</unmanaged-short>
        [System.Runtime.InteropServices.FieldOffset(0)]
        public System.UInt32 CodeSize;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>LocalVarSigTok</unmanaged>
        /// <unmanaged-short>LocalVarSigTok</unmanaged-short>
        [System.Runtime.InteropServices.FieldOffset(4)]
        public System.UInt32 LocalVarSigTok;
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>IMAGE_COR_ILMETHOD_SECT_EH</unmanaged>
    /// <unmanaged-short>IMAGE_COR_ILMETHOD_SECT_EH</unmanaged-short>
    public partial struct ImageCorIlmethodSectEh
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>Small</unmanaged>
        /// <unmanaged-short>Small</unmanaged-short>
        public CorApi.Portable.ImageCorIlmethodSectEhSmall Small;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>Fat</unmanaged>
        /// <unmanaged-short>Fat</unmanaged-short>
        public CorApi.Portable.ImageCorIlmethodSectEhFat Fat;
        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Explicit, Pack = 0)]
        internal partial struct __Native
        {
            [System.Runtime.InteropServices.FieldOffset(0)]
            public CorApi.Portable.ImageCorIlmethodSectEhSmall.__Native Small;
            [System.Runtime.InteropServices.FieldOffset(0)]
            public CorApi.Portable.ImageCorIlmethodSectEhFat.__Native Fat;
            internal unsafe void __MarshalFree()
            {
                this.Small.__MarshalFree();
                this.Fat.__MarshalFree();
            }
        }

        internal void __MarshalFree(ref __Native @ref) => @ref.__MarshalFree();
        internal unsafe void __MarshalFrom(ref __Native @ref)
        {
            {
                this.Small = new CorApi.Portable.ImageCorIlmethodSectEhSmall();
                this.Small.__MarshalFrom(ref @ref.Small);
            }

            {
                this.Fat = new CorApi.Portable.ImageCorIlmethodSectEhFat();
                this.Fat.__MarshalFrom(ref @ref.Fat);
            }
        }
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_FAT</unmanaged>
    /// <unmanaged-short>IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_FAT</unmanaged-short>
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Explicit, Pack = 0)]
    public partial struct ImageCorIlmethodSectEhClauseFat
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>Flags</unmanaged>
        /// <unmanaged-short>Flags</unmanaged-short>
        [System.Runtime.InteropServices.FieldOffset(0)]
        public CorApi.Portable.CorExceptionFlag Flags;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>TryOffset</unmanaged>
        /// <unmanaged-short>TryOffset</unmanaged-short>
        [System.Runtime.InteropServices.FieldOffset(4)]
        public System.UInt32 TryOffset;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>TryLength</unmanaged>
        /// <unmanaged-short>TryLength</unmanaged-short>
        [System.Runtime.InteropServices.FieldOffset(8)]
        public System.UInt32 TryLength;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>HandlerOffset</unmanaged>
        /// <unmanaged-short>HandlerOffset</unmanaged-short>
        [System.Runtime.InteropServices.FieldOffset(12)]
        public System.UInt32 HandlerOffset;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>HandlerLength</unmanaged>
        /// <unmanaged-short>HandlerLength</unmanaged-short>
        [System.Runtime.InteropServices.FieldOffset(16)]
        public System.UInt32 HandlerLength;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ClassToken</unmanaged>
        /// <unmanaged-short>ClassToken</unmanaged-short>
        [System.Runtime.InteropServices.FieldOffset(20)]
        public System.UInt32 ClassToken;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>FilterOffset</unmanaged>
        /// <unmanaged-short>FilterOffset</unmanaged-short>
        [System.Runtime.InteropServices.FieldOffset(20)]
        public System.UInt32 FilterOffset;
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_SMALL</unmanaged>
    /// <unmanaged-short>IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_SMALL</unmanaged-short>
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Explicit, Pack = 0)]
    public partial struct ImageCorIlmethodSectEhClauseSmall
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>TryOffset</unmanaged>
        /// <unmanaged-short>TryOffset</unmanaged-short>
        public System.UInt32 TryOffset
        {
            get => (System.UInt32)((_TryOffset >> 16) & 65535);
            set => this._TryOffset = (System.UInt32)((this._TryOffset & ~(65535 << 16)) | ((value & 65535) << 16));
        }

        [System.Runtime.InteropServices.FieldOffset(0)]
        internal System.UInt32 _TryOffset;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>TryLength</unmanaged>
        /// <unmanaged-short>TryLength</unmanaged-short>
        public System.UInt32 TryLength
        {
            get => (System.UInt32)((_TryLength >> 32) & 255);
            set => this._TryLength = (System.UInt32)((this._TryLength & ~(255 << 32)) | ((value & 255) << 32));
        }

        [System.Runtime.InteropServices.FieldOffset(0)]
        internal System.UInt32 _TryLength;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>HandlerOffset</unmanaged>
        /// <unmanaged-short>HandlerOffset</unmanaged-short>
        public System.UInt32 HandlerOffset
        {
            get => (System.UInt32)((_HandlerOffset >> 40) & 65535);
            set => this._HandlerOffset = (System.UInt32)((this._HandlerOffset & ~(65535 << 40)) | ((value & 65535) << 40));
        }

        [System.Runtime.InteropServices.FieldOffset(0)]
        internal System.UInt32 _HandlerOffset;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>HandlerLength</unmanaged>
        /// <unmanaged-short>HandlerLength</unmanaged-short>
        public System.UInt32 HandlerLength
        {
            get => (System.UInt32)((_HandlerLength >> 56) & 255);
            set => this._HandlerLength = (System.UInt32)((this._HandlerLength & ~(255 << 56)) | ((value & 255) << 56));
        }

        [System.Runtime.InteropServices.FieldOffset(0)]
        internal System.UInt32 _HandlerLength;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>ClassToken</unmanaged>
        /// <unmanaged-short>ClassToken</unmanaged-short>
        [System.Runtime.InteropServices.FieldOffset(0)]
        public System.UInt32 ClassToken;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>FilterOffset</unmanaged>
        /// <unmanaged-short>FilterOffset</unmanaged-short>
        [System.Runtime.InteropServices.FieldOffset(0)]
        public System.UInt32 FilterOffset;
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>IMAGE_COR_ILMETHOD_SECT_EH_FAT</unmanaged>
    /// <unmanaged-short>IMAGE_COR_ILMETHOD_SECT_EH_FAT</unmanaged-short>
    public partial struct ImageCorIlmethodSectEhFat
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>SectFat</unmanaged>
        /// <unmanaged-short>SectFat</unmanaged-short>
        public CorApi.Portable.ImageCorIlmethodSectFat SectFat;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>Clauses</unmanaged>
        /// <unmanaged-short>Clauses</unmanaged-short>
        public CorApi.Portable.ImageCorIlmethodSectEhClauseFat[] Clauses
        {
            get => _Clauses ?? (_Clauses = new CorApi.Portable.ImageCorIlmethodSectEhClauseFat[1]);
        }

        internal CorApi.Portable.ImageCorIlmethodSectEhClauseFat[] _Clauses;
        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]
        internal partial struct __Native
        {
            public CorApi.Portable.ImageCorIlmethodSectFat SectFat;
            public CorApi.Portable.ImageCorIlmethodSectEhClauseFat Clauses;
            internal unsafe void __MarshalFree()
            {
            }
        }

        internal void __MarshalFree(ref __Native @ref) => @ref.__MarshalFree();
        internal unsafe void __MarshalFrom(ref __Native @ref)
        {
            this.SectFat = @ref.SectFat;
            fixed (void *__to = &this.Clauses[0], __from = &@ref.Clauses)
                SharpGen.Runtime.MemoryHelpers.CopyMemory((System.IntPtr)__to, (System.IntPtr)__from, 1 * sizeof (CorApi.Portable.ImageCorIlmethodSectEhClauseFat));
        }
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>IMAGE_COR_ILMETHOD_SECT_EH_SMALL</unmanaged>
    /// <unmanaged-short>IMAGE_COR_ILMETHOD_SECT_EH_SMALL</unmanaged-short>
    public partial struct ImageCorIlmethodSectEhSmall
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>SectSmall</unmanaged>
        /// <unmanaged-short>SectSmall</unmanaged-short>
        public CorApi.Portable.ImageCorIlmethodSectSmall SectSmall;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>Reserved</unmanaged>
        /// <unmanaged-short>Reserved</unmanaged-short>
        public System.UInt16 Reserved;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>Clauses</unmanaged>
        /// <unmanaged-short>Clauses</unmanaged-short>
        public CorApi.Portable.ImageCorIlmethodSectEhClauseSmall[] Clauses
        {
            get => _Clauses ?? (_Clauses = new CorApi.Portable.ImageCorIlmethodSectEhClauseSmall[1]);
        }

        internal CorApi.Portable.ImageCorIlmethodSectEhClauseSmall[] _Clauses;
        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]
        internal partial struct __Native
        {
            public CorApi.Portable.ImageCorIlmethodSectSmall SectSmall;
            public System.UInt16 Reserved;
            public CorApi.Portable.ImageCorIlmethodSectEhClauseSmall Clauses;
            internal unsafe void __MarshalFree()
            {
            }
        }

        internal void __MarshalFree(ref __Native @ref) => @ref.__MarshalFree();
        internal unsafe void __MarshalFrom(ref __Native @ref)
        {
            this.SectSmall = @ref.SectSmall;
            this.Reserved = @ref.Reserved;
            fixed (void *__to = &this.Clauses[0], __from = &@ref.Clauses)
                SharpGen.Runtime.MemoryHelpers.CopyMemory((System.IntPtr)__to, (System.IntPtr)__from, 1 * sizeof (CorApi.Portable.ImageCorIlmethodSectEhClauseSmall));
        }
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>IMAGE_COR_ILMETHOD_SECT_FAT</unmanaged>
    /// <unmanaged-short>IMAGE_COR_ILMETHOD_SECT_FAT</unmanaged-short>
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Explicit, Pack = 0)]
    public partial struct ImageCorIlmethodSectFat
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>Kind</unmanaged>
        /// <unmanaged-short>Kind</unmanaged-short>
        public System.UInt32 Kind
        {
            get => (System.UInt32)((_Kind >> 0) & 255);
            set => this._Kind = (System.UInt32)((this._Kind & ~(255 << 0)) | ((value & 255) << 0));
        }

        [System.Runtime.InteropServices.FieldOffset(0)]
        internal System.UInt32 _Kind;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>DataSize</unmanaged>
        /// <unmanaged-short>DataSize</unmanaged-short>
        public System.UInt32 DataSize
        {
            get => (System.UInt32)((_DataSize >> 8) & 16777215);
            set => this._DataSize = (System.UInt32)((this._DataSize & ~(16777215 << 8)) | ((value & 16777215) << 8));
        }

        [System.Runtime.InteropServices.FieldOffset(0)]
        internal System.UInt32 _DataSize;
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>IMAGE_COR_ILMETHOD_SECT_SMALL</unmanaged>
    /// <unmanaged-short>IMAGE_COR_ILMETHOD_SECT_SMALL</unmanaged-short>
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]
    public partial struct ImageCorIlmethodSectSmall
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>Kind</unmanaged>
        /// <unmanaged-short>Kind</unmanaged-short>
        public System.Byte Kind;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>DataSize</unmanaged>
        /// <unmanaged-short>DataSize</unmanaged-short>
        public System.Byte DataSize;
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>IMAGE_COR_ILMETHOD_TINY</unmanaged>
    /// <unmanaged-short>IMAGE_COR_ILMETHOD_TINY</unmanaged-short>
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]
    public partial struct ImageCorIlmethodTiny
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>Flags_CodeSize</unmanaged>
        /// <unmanaged-short>Flags_CodeSize</unmanaged-short>
        public System.Byte FlagsCodeSize;
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>IMAGE_COR_VTABLEFIXUP</unmanaged>
    /// <unmanaged-short>IMAGE_COR_VTABLEFIXUP</unmanaged-short>
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]
    public partial struct ImageCorVtablefixup
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>RVA</unmanaged>
        /// <unmanaged-short>RVA</unmanaged-short>
        public System.UInt32 Rva;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>Count</unmanaged>
        /// <unmanaged-short>Count</unmanaged-short>
        public System.UInt16 Count;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>Type</unmanaged>
        /// <unmanaged-short>Type</unmanaged-short>
        public System.UInt16 Type;
    }

    /// <summary>
    /// No documentation.
    /// </summary>
    /// <unmanaged>OSINFO</unmanaged>
    /// <unmanaged-short>OSINFO</unmanaged-short>
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 0)]
    public partial struct Osinfo
    {
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>dwOSPlatformId</unmanaged>
        /// <unmanaged-short>dwOSPlatformId</unmanaged-short>
        public System.UInt32 DwOSPlatformId;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>dwOSMajorVersion</unmanaged>
        /// <unmanaged-short>dwOSMajorVersion</unmanaged-short>
        public System.UInt32 DwOSMajorVersion;
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>dwOSMinorVersion</unmanaged>
        /// <unmanaged-short>dwOSMinorVersion</unmanaged-short>
        public System.UInt32 DwOSMinorVersion;
    }
}