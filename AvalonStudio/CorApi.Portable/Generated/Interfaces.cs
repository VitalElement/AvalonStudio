namespace CorApi.Portable
{
    [System.Runtime.InteropServices.GuidAttribute("3d6f5f63-7538-11d3-8d5b-00104b35e7ef")]
    public partial class AppDomain : CorApi.Portable.Controller
    {
        public AppDomain(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator AppDomain(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new AppDomain(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetObjectW</unmanaged>
        /// <unmanaged-short>GetObjectW</unmanaged-short>
        public CorApi.Portable.Value ObjectW
        {
            get
            {
                GetObjectW(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetID</unmanaged>
        /// <unmanaged-short>GetID</unmanaged-short>
        public System.UInt32 ID
        {
            get
            {
                GetID(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "processOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugAppDomain::GetProcess([In] ICorDebugProcess** ppProcess)</unmanaged>
        /// <unmanaged-short>ICorDebugAppDomain::GetProcess</unmanaged-short>
        public unsafe void GetProcess(out CorApi.Portable.Process processOut)
        {
            System.IntPtr processOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&processOut_), (*(void ***)this._nativePointer)[13]);
            processOut = (processOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Process(processOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "assembliesOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugAppDomain::EnumerateAssemblies([In] ICorDebugAssemblyEnum** ppAssemblies)</unmanaged>
        /// <unmanaged-short>ICorDebugAppDomain::EnumerateAssemblies</unmanaged-short>
        public unsafe void EnumerateAssemblies(out CorApi.Portable.AssemblyEnum assembliesOut)
        {
            System.IntPtr assembliesOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&assembliesOut_), (*(void ***)this._nativePointer)[14]);
            assembliesOut = (assembliesOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.AssemblyEnum(assembliesOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "iMetaDataRef">No documentation.</param>
        /// <param name = "moduleOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugAppDomain::GetModuleFromMetaDataInterface([In] IUnknown* pIMetaData,[In] ICorDebugModule** ppModule)</unmanaged>
        /// <unmanaged-short>ICorDebugAppDomain::GetModuleFromMetaDataInterface</unmanaged-short>
        public unsafe void GetModuleFromMetaDataInterface(SharpGen.Runtime.IUnknown iMetaDataRef, out CorApi.Portable.Module moduleOut)
        {
            System.IntPtr moduleOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<SharpGen.Runtime.IUnknown>(iMetaDataRef))), (void *)(&moduleOut_), (*(void ***)this._nativePointer)[15]);
            moduleOut = (moduleOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Module(moduleOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "breakpointsOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugAppDomain::EnumerateBreakpoints([In] ICorDebugBreakpointEnum** ppBreakpoints)</unmanaged>
        /// <unmanaged-short>ICorDebugAppDomain::EnumerateBreakpoints</unmanaged-short>
        public unsafe void EnumerateBreakpoints(out CorApi.Portable.BreakpointEnum breakpointsOut)
        {
            System.IntPtr breakpointsOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&breakpointsOut_), (*(void ***)this._nativePointer)[16]);
            breakpointsOut = (breakpointsOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.BreakpointEnum(breakpointsOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "steppersOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugAppDomain::EnumerateSteppers([In] ICorDebugStepperEnum** ppSteppers)</unmanaged>
        /// <unmanaged-short>ICorDebugAppDomain::EnumerateSteppers</unmanaged-short>
        public unsafe void EnumerateSteppers(out CorApi.Portable.StepperEnum steppersOut)
        {
            System.IntPtr steppersOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&steppersOut_), (*(void ***)this._nativePointer)[17]);
            steppersOut = (steppersOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.StepperEnum(steppersOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "bAttachedRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugAppDomain::IsAttached([In] BOOL* pbAttached)</unmanaged>
        /// <unmanaged-short>ICorDebugAppDomain::IsAttached</unmanaged-short>
        public unsafe void IsAttached(SharpGen.Runtime.Win32.RawBool bAttachedRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&bAttachedRef), (*(void ***)this._nativePointer)[18]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cchName">No documentation.</param>
        /// <param name = "cchNameRef">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugAppDomain::GetName([In] unsigned int cchName,[Out] unsigned int* pcchName,[In] wchar_t* szName)</unmanaged>
        /// <unmanaged-short>ICorDebugAppDomain::GetName</unmanaged-short>
        public unsafe void GetName(System.UInt32 cchName, out System.UInt32 cchNameRef, System.IntPtr szName)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *cchNameRef_ = &cchNameRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cchName, (void *)(cchNameRef_), (void *)((void *)szName), (*(void ***)this._nativePointer)[19]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "objectOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugAppDomain::GetObjectW([In] ICorDebugValue** ppObject)</unmanaged>
        /// <unmanaged-short>ICorDebugAppDomain::GetObjectW</unmanaged-short>
        internal unsafe void GetObjectW(out CorApi.Portable.Value objectOut)
        {
            System.IntPtr objectOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&objectOut_), (*(void ***)this._nativePointer)[20]);
            objectOut = (objectOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Value(objectOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugAppDomain::Attach()</unmanaged>
        /// <unmanaged-short>ICorDebugAppDomain::Attach</unmanaged-short>
        public unsafe void Attach()
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (*(void ***)this._nativePointer)[21]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "idRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugAppDomain::GetID([Out] unsigned int* pId)</unmanaged>
        /// <unmanaged-short>ICorDebugAppDomain::GetID</unmanaged-short>
        internal unsafe void GetID(out System.UInt32 idRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *idRef_ = &idRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(idRef_), (*(void ***)this._nativePointer)[22]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("096E81D5-ECDA-4202-83F5-C65980A9EF75")]
    public partial class AppDomain2 : SharpGen.Runtime.ComObject
    {
        public AppDomain2(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator AppDomain2(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new AppDomain2(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "elementType">No documentation.</param>
        /// <param name = "nRank">No documentation.</param>
        /// <param name = "typeArgRef">No documentation.</param>
        /// <param name = "typeOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugAppDomain2::GetArrayOrPointerType([In] CorElementType elementType,[In] unsigned int nRank,[In] ICorDebugType* pTypeArg,[In] ICorDebugType** ppType)</unmanaged>
        /// <unmanaged-short>ICorDebugAppDomain2::GetArrayOrPointerType</unmanaged-short>
        public unsafe void GetArrayOrPointerType(CorApi.Portable.CorElementType elementType, System.UInt32 nRank, CorApi.Portable.Type typeArgRef, out CorApi.Portable.Type typeOut)
        {
            System.IntPtr typeOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, unchecked ((System.Int32)elementType), nRank, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.Type>(typeArgRef))), (void *)(&typeOut_), (*(void ***)this._nativePointer)[3]);
            typeOut = (typeOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Type(typeOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "nTypeArgs">No documentation.</param>
        /// <param name = "typeArgsOut">No documentation.</param>
        /// <param name = "typeOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugAppDomain2::GetFunctionPointerType([In] unsigned int nTypeArgs,[In] ICorDebugType** ppTypeArgs,[In] ICorDebugType** ppType)</unmanaged>
        /// <unmanaged-short>ICorDebugAppDomain2::GetFunctionPointerType</unmanaged-short>
        public unsafe void GetFunctionPointerType(System.UInt32 nTypeArgs, out CorApi.Portable.Type typeArgsOut, out CorApi.Portable.Type typeOut)
        {
            System.IntPtr typeArgsOut_ = System.IntPtr.Zero;
            System.IntPtr typeOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, nTypeArgs, (void *)(&typeArgsOut_), (void *)(&typeOut_), (*(void ***)this._nativePointer)[4]);
            typeArgsOut = (typeArgsOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Type(typeArgsOut_);
            typeOut = (typeOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Type(typeOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("8CB96A16-B588-42E2-B71C-DD849FC2ECCC")]
    public partial class AppDomain3 : SharpGen.Runtime.ComObject
    {
        public AppDomain3(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator AppDomain3(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new AppDomain3(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetCachedWinRTTypes</unmanaged>
        /// <unmanaged-short>GetCachedWinRTTypes</unmanaged-short>
        public CorApi.Portable.GuidToTypeEnum CachedWinRTTypes
        {
            get
            {
                GetCachedWinRTTypes(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cReqTypes">No documentation.</param>
        /// <param name = "iidsToResolve">No documentation.</param>
        /// <param name = "typesEnumOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugAppDomain3::GetCachedWinRTTypesForIIDs([In] unsigned int cReqTypes,[In] GUID* iidsToResolve,[In] ICorDebugTypeEnum** ppTypesEnum)</unmanaged>
        /// <unmanaged-short>ICorDebugAppDomain3::GetCachedWinRTTypesForIIDs</unmanaged-short>
        public unsafe void GetCachedWinRTTypesForIIDs(System.UInt32 cReqTypes, System.Guid iidsToResolve, out CorApi.Portable.TypeEnum typesEnumOut)
        {
            System.IntPtr typesEnumOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cReqTypes, (void *)(&iidsToResolve), (void *)(&typesEnumOut_), (*(void ***)this._nativePointer)[3]);
            typesEnumOut = (typesEnumOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.TypeEnum(typesEnumOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "guidToTypeEnumOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugAppDomain3::GetCachedWinRTTypes([In] ICorDebugGuidToTypeEnum** ppGuidToTypeEnum)</unmanaged>
        /// <unmanaged-short>ICorDebugAppDomain3::GetCachedWinRTTypes</unmanaged-short>
        internal unsafe void GetCachedWinRTTypes(out CorApi.Portable.GuidToTypeEnum guidToTypeEnumOut)
        {
            System.IntPtr guidToTypeEnumOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&guidToTypeEnumOut_), (*(void ***)this._nativePointer)[4]);
            guidToTypeEnumOut = (guidToTypeEnumOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.GuidToTypeEnum(guidToTypeEnumOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("FB99CC40-83BE-4724-AB3B-768E796EBAC2")]
    public partial class AppDomain4 : SharpGen.Runtime.ComObject
    {
        public AppDomain4(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator AppDomain4(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new AppDomain4(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "ccwPointer">No documentation.</param>
        /// <param name = "managedObjectOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugAppDomain4::GetObjectForCCW([In] unsigned longlong ccwPointer,[In] ICorDebugValue** ppManagedObject)</unmanaged>
        /// <unmanaged-short>ICorDebugAppDomain4::GetObjectForCCW</unmanaged-short>
        public unsafe void GetObjectForCCW(System.UInt64 ccwPointer, out CorApi.Portable.Value managedObjectOut)
        {
            System.IntPtr managedObjectOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, ccwPointer, (void *)(&managedObjectOut_), (*(void ***)this._nativePointer)[3]);
            managedObjectOut = (managedObjectOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Value(managedObjectOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("63ca1b24-4359-4883-bd57-13f815f58744")]
    public partial class AppDomainEnum : CorApi.Portable.Enum
    {
        public AppDomainEnum(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator AppDomainEnum(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new AppDomainEnum(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "celt">No documentation.</param>
        /// <param name = "values">No documentation.</param>
        /// <param name = "celtFetchedRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugAppDomainEnum::Next([In] unsigned int celt,[In] ICorDebugAppDomain** values,[In] unsigned int* pceltFetched)</unmanaged>
        /// <unmanaged-short>ICorDebugAppDomainEnum::Next</unmanaged-short>
        public unsafe void Next(System.UInt32 celt, out CorApi.Portable.AppDomain values, System.UInt32 celtFetchedRef)
        {
            System.IntPtr values_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, celt, (void *)(&values_), (void *)(&celtFetchedRef), (*(void ***)this._nativePointer)[7]);
            values = (values_ == System.IntPtr.Zero) ? null : new CorApi.Portable.AppDomain(values_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("0405B0DF-A660-11d2-BD02-0000F80849BD")]
    public partial class ArrayValue : CorApi.Portable.HeapValue
    {
        public ArrayValue(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator ArrayValue(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new ArrayValue(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetRank</unmanaged>
        /// <unmanaged-short>GetRank</unmanaged-short>
        public System.UInt32 Rank
        {
            get
            {
                GetRank(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "typeRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugArrayValue::GetElementType([In] CorElementType* pType)</unmanaged>
        /// <unmanaged-short>ICorDebugArrayValue::GetElementType</unmanaged-short>
        public unsafe void GetElementType(CorApi.Portable.CorElementType typeRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&typeRef), (*(void ***)this._nativePointer)[9]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "nRankRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugArrayValue::GetRank([Out] unsigned int* pnRank)</unmanaged>
        /// <unmanaged-short>ICorDebugArrayValue::GetRank</unmanaged-short>
        internal unsafe void GetRank(out System.UInt32 nRankRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *nRankRef_ = &nRankRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(nRankRef_), (*(void ***)this._nativePointer)[10]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "nCountRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugArrayValue::GetCount([In] unsigned int* pnCount)</unmanaged>
        /// <unmanaged-short>ICorDebugArrayValue::GetCount</unmanaged-short>
        public unsafe void GetCount(System.UInt32 nCountRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&nCountRef), (*(void ***)this._nativePointer)[11]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cdim">No documentation.</param>
        /// <param name = "dims">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugArrayValue::GetDimensions([In] unsigned int cdim,[Out, Buffer] unsigned int* dims)</unmanaged>
        /// <unmanaged-short>ICorDebugArrayValue::GetDimensions</unmanaged-short>
        public unsafe void GetDimensions(System.UInt32 cdim, System.UInt32[] dims)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *dims_ = dims)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cdim, (void *)(dims_), (*(void ***)this._nativePointer)[12]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugArrayValue::HasBaseIndicies([Out] BOOL* pbHasBaseIndicies)</unmanaged>
        /// <unmanaged-short>ICorDebugArrayValue::HasBaseIndicies</unmanaged-short>
        public unsafe SharpGen.Runtime.Win32.RawBool HasBaseIndicies()
        {
            SharpGen.Runtime.Win32.RawBool bHasBaseIndiciesRef;
            bHasBaseIndiciesRef = new SharpGen.Runtime.Win32.RawBool();
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&bHasBaseIndiciesRef), (*(void ***)this._nativePointer)[13]);
            __result__.CheckError();
            return bHasBaseIndiciesRef;
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cdim">No documentation.</param>
        /// <param name = "indicies">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugArrayValue::GetBaseIndicies([In] unsigned int cdim,[Out, Buffer] unsigned int* indicies)</unmanaged>
        /// <unmanaged-short>ICorDebugArrayValue::GetBaseIndicies</unmanaged-short>
        public unsafe void GetBaseIndicies(System.UInt32 cdim, System.UInt32[] indicies)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *indicies_ = indicies)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cdim, (void *)(indicies_), (*(void ***)this._nativePointer)[14]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cdim">No documentation.</param>
        /// <param name = "indices">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugArrayValue::GetElement([In] unsigned int cdim,[In, Buffer] unsigned int* indices,[Out] ICorDebugValue** ppValue)</unmanaged>
        /// <unmanaged-short>ICorDebugArrayValue::GetElement</unmanaged-short>
        public unsafe CorApi.Portable.Value GetElement(System.UInt32 cdim, System.UInt32[] indices)
        {
            CorApi.Portable.Value valueOut;
            System.IntPtr valueOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            fixed (void *indices_ = indices)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cdim, (void *)(indices_), (void *)(&valueOut_), (*(void ***)this._nativePointer)[15]);
            valueOut = (valueOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Value(valueOut_);
            __result__.CheckError();
            return valueOut;
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "nPosition">No documentation.</param>
        /// <param name = "valueOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugArrayValue::GetElementAtPosition([In] unsigned int nPosition,[In] ICorDebugValue** ppValue)</unmanaged>
        /// <unmanaged-short>ICorDebugArrayValue::GetElementAtPosition</unmanaged-short>
        public unsafe void GetElementAtPosition(System.UInt32 nPosition, out CorApi.Portable.Value valueOut)
        {
            System.IntPtr valueOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, nPosition, (void *)(&valueOut_), (*(void ***)this._nativePointer)[16]);
            valueOut = (valueOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Value(valueOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("df59507c-d47a-459e-bce2-6427eac8fd06")]
    public partial class Assembly : SharpGen.Runtime.ComObject
    {
        public Assembly(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Assembly(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Assembly(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetProcess</unmanaged>
        /// <unmanaged-short>GetProcess</unmanaged-short>
        public CorApi.Portable.Process Process
        {
            get
            {
                GetProcess(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetAppDomain</unmanaged>
        /// <unmanaged-short>GetAppDomain</unmanaged-short>
        public CorApi.Portable.AppDomain AppDomain
        {
            get
            {
                GetAppDomain(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "processOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugAssembly::GetProcess([In] ICorDebugProcess** ppProcess)</unmanaged>
        /// <unmanaged-short>ICorDebugAssembly::GetProcess</unmanaged-short>
        internal unsafe void GetProcess(out CorApi.Portable.Process processOut)
        {
            System.IntPtr processOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&processOut_), (*(void ***)this._nativePointer)[3]);
            processOut = (processOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Process(processOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "appDomainOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugAssembly::GetAppDomain([In] ICorDebugAppDomain** ppAppDomain)</unmanaged>
        /// <unmanaged-short>ICorDebugAssembly::GetAppDomain</unmanaged-short>
        internal unsafe void GetAppDomain(out CorApi.Portable.AppDomain appDomainOut)
        {
            System.IntPtr appDomainOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&appDomainOut_), (*(void ***)this._nativePointer)[4]);
            appDomainOut = (appDomainOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.AppDomain(appDomainOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "modulesOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugAssembly::EnumerateModules([In] ICorDebugModuleEnum** ppModules)</unmanaged>
        /// <unmanaged-short>ICorDebugAssembly::EnumerateModules</unmanaged-short>
        public unsafe void EnumerateModules(out CorApi.Portable.ModuleEnum modulesOut)
        {
            System.IntPtr modulesOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&modulesOut_), (*(void ***)this._nativePointer)[5]);
            modulesOut = (modulesOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.ModuleEnum(modulesOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cchName">No documentation.</param>
        /// <param name = "cchNameRef">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugAssembly::GetCodeBase([In] unsigned int cchName,[In] unsigned int* pcchName,[In] wchar_t* szName)</unmanaged>
        /// <unmanaged-short>ICorDebugAssembly::GetCodeBase</unmanaged-short>
        public unsafe void GetCodeBase(System.UInt32 cchName, System.UInt32 cchNameRef, System.String szName)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cchName, (void *)(&cchNameRef), (void *)((void *)szName_), (*(void ***)this._nativePointer)[6]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cchName">No documentation.</param>
        /// <param name = "cchNameRef">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugAssembly::GetName([In] unsigned int cchName,[Out] unsigned int* pcchName,[In] wchar_t* szName)</unmanaged>
        /// <unmanaged-short>ICorDebugAssembly::GetName</unmanaged-short>
        public unsafe void GetName(System.UInt32 cchName, out System.UInt32 cchNameRef, System.IntPtr szName)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *cchNameRef_ = &cchNameRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cchName, (void *)(cchNameRef_), (void *)((void *)szName), (*(void ***)this._nativePointer)[7]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("426d1f9e-6dd4-44c8-aec7-26cdbaf4e398")]
    public partial class Assembly2 : SharpGen.Runtime.ComObject
    {
        public Assembly2(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Assembly2(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Assembly2(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "bFullyTrustedRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugAssembly2::IsFullyTrusted([In] BOOL* pbFullyTrusted)</unmanaged>
        /// <unmanaged-short>ICorDebugAssembly2::IsFullyTrusted</unmanaged-short>
        public unsafe void IsFullyTrusted(SharpGen.Runtime.Win32.RawBool bFullyTrustedRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&bFullyTrustedRef), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("76361AB2-8C86-4FE9-96F2-F73D8843570A")]
    public partial class Assembly3 : SharpGen.Runtime.ComObject
    {
        public Assembly3(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Assembly3(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Assembly3(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetContainerAssembly</unmanaged>
        /// <unmanaged-short>GetContainerAssembly</unmanaged-short>
        public CorApi.Portable.Assembly ContainerAssembly
        {
            get
            {
                GetContainerAssembly(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "assemblyOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugAssembly3::GetContainerAssembly([In] ICorDebugAssembly** ppAssembly)</unmanaged>
        /// <unmanaged-short>ICorDebugAssembly3::GetContainerAssembly</unmanaged-short>
        internal unsafe void GetContainerAssembly(out CorApi.Portable.Assembly assemblyOut)
        {
            System.IntPtr assemblyOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&assemblyOut_), (*(void ***)this._nativePointer)[3]);
            assemblyOut = (assemblyOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Assembly(assemblyOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "assembliesOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugAssembly3::EnumerateContainedAssemblies([In] ICorDebugAssemblyEnum** ppAssemblies)</unmanaged>
        /// <unmanaged-short>ICorDebugAssembly3::EnumerateContainedAssemblies</unmanaged-short>
        public unsafe void EnumerateContainedAssemblies(out CorApi.Portable.AssemblyEnum assembliesOut)
        {
            System.IntPtr assembliesOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&assembliesOut_), (*(void ***)this._nativePointer)[4]);
            assembliesOut = (assembliesOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.AssemblyEnum(assembliesOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("4a2a1ec9-85ec-4bfb-9f15-a89fdfe0fe83")]
    public partial class AssemblyEnum : CorApi.Portable.Enum
    {
        public AssemblyEnum(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator AssemblyEnum(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new AssemblyEnum(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "celt">No documentation.</param>
        /// <param name = "values">No documentation.</param>
        /// <param name = "celtFetchedRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugAssemblyEnum::Next([In] unsigned int celt,[In] ICorDebugAssembly** values,[In] unsigned int* pceltFetched)</unmanaged>
        /// <unmanaged-short>ICorDebugAssemblyEnum::Next</unmanaged-short>
        public unsafe void Next(System.UInt32 celt, out CorApi.Portable.Assembly values, System.UInt32 celtFetchedRef)
        {
            System.IntPtr values_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, celt, (void *)(&values_), (void *)(&celtFetchedRef), (*(void ***)this._nativePointer)[7]);
            values = (values_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Assembly(values_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("976A6278-134A-4a81-81A3-8F277943F4C3")]
    public partial class BlockingObjectEnum : CorApi.Portable.Enum
    {
        public BlockingObjectEnum(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator BlockingObjectEnum(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new BlockingObjectEnum(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "celt">No documentation.</param>
        /// <param name = "values">No documentation.</param>
        /// <param name = "celtFetchedRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugBlockingObjectEnum::Next([In] unsigned int celt,[In] CorDebugBlockingObject* values,[In] unsigned int* pceltFetched)</unmanaged>
        /// <unmanaged-short>ICorDebugBlockingObjectEnum::Next</unmanaged-short>
        public unsafe void Next(System.UInt32 celt, ref CorApi.Portable.CorDebugBlockingObject values, System.UInt32 celtFetchedRef)
        {
            var values_ = new CorApi.Portable.CorDebugBlockingObject.__Native();
            values.__MarshalTo(ref values_);
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, celt, (void *)(&values_), (void *)(&celtFetchedRef), (*(void ***)this._nativePointer)[7]);
            values.__MarshalFree(ref values_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("CC7BCAFC-8A68-11d2-983C-0000F808342D")]
    public partial class BoxValue : CorApi.Portable.HeapValue
    {
        public BoxValue(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator BoxValue(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new BoxValue(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetObjectW</unmanaged>
        /// <unmanaged-short>GetObjectW</unmanaged-short>
        public CorApi.Portable.ObjectValue ObjectW
        {
            get
            {
                GetObjectW(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "objectOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugBoxValue::GetObjectW([In] ICorDebugObjectValue** ppObject)</unmanaged>
        /// <unmanaged-short>ICorDebugBoxValue::GetObjectW</unmanaged-short>
        internal unsafe void GetObjectW(out CorApi.Portable.ObjectValue objectOut)
        {
            System.IntPtr objectOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&objectOut_), (*(void ***)this._nativePointer)[9]);
            objectOut = (objectOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.ObjectValue(objectOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("CC7BCAE8-8A68-11d2-983C-0000F808342D")]
    public partial class Breakpoint : SharpGen.Runtime.ComObject
    {
        public Breakpoint(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Breakpoint(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Breakpoint(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "bActive">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugBreakpoint::Activate([In] BOOL bActive)</unmanaged>
        /// <unmanaged-short>ICorDebugBreakpoint::Activate</unmanaged-short>
        public unsafe void Activate(SharpGen.Runtime.Win32.RawBool bActive)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint0(this._nativePointer, bActive, (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "bActiveRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugBreakpoint::IsActive([In] BOOL* pbActive)</unmanaged>
        /// <unmanaged-short>ICorDebugBreakpoint::IsActive</unmanaged-short>
        public unsafe void IsActive(SharpGen.Runtime.Win32.RawBool bActiveRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&bActiveRef), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("CC7BCB03-8A68-11d2-983C-0000F808342D")]
    public partial class BreakpointEnum : CorApi.Portable.Enum
    {
        public BreakpointEnum(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator BreakpointEnum(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new BreakpointEnum(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "celt">No documentation.</param>
        /// <param name = "breakpoints">No documentation.</param>
        /// <param name = "celtFetchedRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugBreakpointEnum::Next([In] unsigned int celt,[In] ICorDebugBreakpoint** breakpoints,[In] unsigned int* pceltFetched)</unmanaged>
        /// <unmanaged-short>ICorDebugBreakpointEnum::Next</unmanaged-short>
        public unsafe void Next(System.UInt32 celt, out CorApi.Portable.Breakpoint breakpoints, System.UInt32 celtFetchedRef)
        {
            System.IntPtr breakpoints_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, celt, (void *)(&breakpoints_), (void *)(&celtFetchedRef), (*(void ***)this._nativePointer)[7]);
            breakpoints = (breakpoints_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Breakpoint(breakpoints_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("CC7BCAEE-8A68-11d2-983C-0000F808342D")]
    public partial class Chain : SharpGen.Runtime.ComObject
    {
        public Chain(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Chain(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Chain(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetThread</unmanaged>
        /// <unmanaged-short>GetThread</unmanaged-short>
        public CorApi.Portable.Thread Thread
        {
            get
            {
                GetThread(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetContext</unmanaged>
        /// <unmanaged-short>GetContext</unmanaged-short>
        public CorApi.Portable.Context Context
        {
            get
            {
                GetContext(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetCaller</unmanaged>
        /// <unmanaged-short>GetCaller</unmanaged-short>
        public CorApi.Portable.Chain Caller
        {
            get
            {
                GetCaller(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetCallee</unmanaged>
        /// <unmanaged-short>GetCallee</unmanaged-short>
        public CorApi.Portable.Chain Callee
        {
            get
            {
                GetCallee(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetPrevious</unmanaged>
        /// <unmanaged-short>GetPrevious</unmanaged-short>
        public CorApi.Portable.Chain Previous
        {
            get
            {
                GetPrevious(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetNext</unmanaged>
        /// <unmanaged-short>GetNext</unmanaged-short>
        public CorApi.Portable.Chain Next
        {
            get
            {
                GetNext(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IsManaged</unmanaged>
        /// <unmanaged-short>IsManaged</unmanaged-short>
        public SharpGen.Runtime.Win32.RawBool IsManaged
        {
            get
            {
                IsManaged_(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetActiveFrame</unmanaged>
        /// <unmanaged-short>GetActiveFrame</unmanaged-short>
        public CorApi.Portable.Frame ActiveFrame
        {
            get
            {
                GetActiveFrame(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetRegisterSet</unmanaged>
        /// <unmanaged-short>GetRegisterSet</unmanaged-short>
        public CorApi.Portable.RegisterSet RegisterSet
        {
            get
            {
                GetRegisterSet(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "threadOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugChain::GetThread([In] ICorDebugThread** ppThread)</unmanaged>
        /// <unmanaged-short>ICorDebugChain::GetThread</unmanaged-short>
        internal unsafe void GetThread(out CorApi.Portable.Thread threadOut)
        {
            System.IntPtr threadOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&threadOut_), (*(void ***)this._nativePointer)[3]);
            threadOut = (threadOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Thread(threadOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "startRef">No documentation.</param>
        /// <param name = "endRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugChain::GetStackRange([In] unsigned longlong* pStart,[In] unsigned longlong* pEnd)</unmanaged>
        /// <unmanaged-short>ICorDebugChain::GetStackRange</unmanaged-short>
        public unsafe void GetStackRange(System.UInt64 startRef, System.UInt64 endRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&startRef), (void *)(&endRef), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "contextOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugChain::GetContext([In] ICorDebugContext** ppContext)</unmanaged>
        /// <unmanaged-short>ICorDebugChain::GetContext</unmanaged-short>
        internal unsafe void GetContext(out CorApi.Portable.Context contextOut)
        {
            System.IntPtr contextOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&contextOut_), (*(void ***)this._nativePointer)[5]);
            contextOut = (contextOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Context(contextOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "chainOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugChain::GetCaller([In] ICorDebugChain** ppChain)</unmanaged>
        /// <unmanaged-short>ICorDebugChain::GetCaller</unmanaged-short>
        internal unsafe void GetCaller(out CorApi.Portable.Chain chainOut)
        {
            System.IntPtr chainOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&chainOut_), (*(void ***)this._nativePointer)[6]);
            chainOut = (chainOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Chain(chainOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "chainOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugChain::GetCallee([In] ICorDebugChain** ppChain)</unmanaged>
        /// <unmanaged-short>ICorDebugChain::GetCallee</unmanaged-short>
        internal unsafe void GetCallee(out CorApi.Portable.Chain chainOut)
        {
            System.IntPtr chainOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&chainOut_), (*(void ***)this._nativePointer)[7]);
            chainOut = (chainOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Chain(chainOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "chainOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugChain::GetPrevious([In] ICorDebugChain** ppChain)</unmanaged>
        /// <unmanaged-short>ICorDebugChain::GetPrevious</unmanaged-short>
        internal unsafe void GetPrevious(out CorApi.Portable.Chain chainOut)
        {
            System.IntPtr chainOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&chainOut_), (*(void ***)this._nativePointer)[8]);
            chainOut = (chainOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Chain(chainOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "chainOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugChain::GetNext([In] ICorDebugChain** ppChain)</unmanaged>
        /// <unmanaged-short>ICorDebugChain::GetNext</unmanaged-short>
        internal unsafe void GetNext(out CorApi.Portable.Chain chainOut)
        {
            System.IntPtr chainOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&chainOut_), (*(void ***)this._nativePointer)[9]);
            chainOut = (chainOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Chain(chainOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "managedRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugChain::IsManaged([Out] BOOL* pManaged)</unmanaged>
        /// <unmanaged-short>ICorDebugChain::IsManaged</unmanaged-short>
        internal unsafe void IsManaged_(out SharpGen.Runtime.Win32.RawBool managedRef)
        {
            managedRef = new SharpGen.Runtime.Win32.RawBool();
            SharpGen.Runtime.Result __result__;
            fixed (void *managedRef_ = &managedRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(managedRef_), (*(void ***)this._nativePointer)[10]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "framesOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugChain::EnumerateFrames([In] ICorDebugFrameEnum** ppFrames)</unmanaged>
        /// <unmanaged-short>ICorDebugChain::EnumerateFrames</unmanaged-short>
        public unsafe void EnumerateFrames(out CorApi.Portable.FrameEnum framesOut)
        {
            System.IntPtr framesOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&framesOut_), (*(void ***)this._nativePointer)[11]);
            framesOut = (framesOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.FrameEnum(framesOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "frameOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugChain::GetActiveFrame([In] ICorDebugFrame** ppFrame)</unmanaged>
        /// <unmanaged-short>ICorDebugChain::GetActiveFrame</unmanaged-short>
        internal unsafe void GetActiveFrame(out CorApi.Portable.Frame frameOut)
        {
            System.IntPtr frameOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&frameOut_), (*(void ***)this._nativePointer)[12]);
            frameOut = (frameOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Frame(frameOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "registersOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugChain::GetRegisterSet([In] ICorDebugRegisterSet** ppRegisters)</unmanaged>
        /// <unmanaged-short>ICorDebugChain::GetRegisterSet</unmanaged-short>
        internal unsafe void GetRegisterSet(out CorApi.Portable.RegisterSet registersOut)
        {
            System.IntPtr registersOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&registersOut_), (*(void ***)this._nativePointer)[13]);
            registersOut = (registersOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.RegisterSet(registersOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "reasonRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugChain::GetReason([In] CorDebugChainReason* pReason)</unmanaged>
        /// <unmanaged-short>ICorDebugChain::GetReason</unmanaged-short>
        public unsafe void GetReason(CorApi.Portable.CorDebugChainReason reasonRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&reasonRef), (*(void ***)this._nativePointer)[14]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("CC7BCB08-8A68-11d2-983C-0000F808342D")]
    public partial class ChainEnum : CorApi.Portable.Enum
    {
        public ChainEnum(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator ChainEnum(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new ChainEnum(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "celt">No documentation.</param>
        /// <param name = "chains">No documentation.</param>
        /// <param name = "celtFetchedRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugChainEnum::Next([In] unsigned int celt,[Out, Buffer] ICorDebugChain** chains,[Out] unsigned int* pceltFetched)</unmanaged>
        /// <unmanaged-short>ICorDebugChainEnum::Next</unmanaged-short>
        public unsafe void Next(System.UInt32 celt, CorApi.Portable.Chain[] chains, out System.UInt32 celtFetchedRef)
        {
            System.IntPtr*chains_ = stackalloc System.IntPtr[chains.Length];
            SharpGen.Runtime.Result __result__;
            fixed (void *celtFetchedRef_ = &celtFetchedRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, celt, (void *)(chains_), (void *)(celtFetchedRef_), (*(void ***)this._nativePointer)[7]);
            for (int i = 0; i < chains.Length; i++)
                chains[i] = (chains_[i] == System.IntPtr.Zero) ? null : new CorApi.Portable.Chain(chains_[i]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("CC7BCAF5-8A68-11d2-983C-0000F808342D")]
    public partial class Class : SharpGen.Runtime.ComObject
    {
        public Class(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Class(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Class(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetModule</unmanaged>
        /// <unmanaged-short>GetModule</unmanaged-short>
        public CorApi.Portable.Module Module
        {
            get
            {
                GetModule(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetToken</unmanaged>
        /// <unmanaged-short>GetToken</unmanaged-short>
        public System.UInt32 Token
        {
            get
            {
                GetToken(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "moduleRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugClass::GetModule([In] ICorDebugModule** pModule)</unmanaged>
        /// <unmanaged-short>ICorDebugClass::GetModule</unmanaged-short>
        internal unsafe void GetModule(out CorApi.Portable.Module moduleRef)
        {
            System.IntPtr moduleRef_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&moduleRef_), (*(void ***)this._nativePointer)[3]);
            moduleRef = (moduleRef_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Module(moduleRef_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "typeDefRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugClass::GetToken([Out] unsigned int* pTypeDef)</unmanaged>
        /// <unmanaged-short>ICorDebugClass::GetToken</unmanaged-short>
        internal unsafe void GetToken(out System.UInt32 typeDefRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *typeDefRef_ = &typeDefRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(typeDefRef_), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "fieldDef">No documentation.</param>
        /// <param name = "frameRef">No documentation.</param>
        /// <param name = "valueOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugClass::GetStaticFieldValue([In] unsigned int fieldDef,[In] ICorDebugFrame* pFrame,[In] ICorDebugValue** ppValue)</unmanaged>
        /// <unmanaged-short>ICorDebugClass::GetStaticFieldValue</unmanaged-short>
        public unsafe void GetStaticFieldValue(System.UInt32 fieldDef, CorApi.Portable.Frame frameRef, out CorApi.Portable.Value valueOut)
        {
            System.IntPtr valueOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, fieldDef, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.Frame>(frameRef))), (void *)(&valueOut_), (*(void ***)this._nativePointer)[5]);
            valueOut = (valueOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Value(valueOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("B008EA8D-7AB1-43f7-BB20-FBB5A04038AE")]
    public partial class Class2 : SharpGen.Runtime.ComObject
    {
        public Class2(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Class2(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Class2(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "elementType">No documentation.</param>
        /// <param name = "nTypeArgs">No documentation.</param>
        /// <param name = "typeArgsOut">No documentation.</param>
        /// <param name = "typeOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugClass2::GetParameterizedType([In] CorElementType elementType,[In] unsigned int nTypeArgs,[In, Buffer] ICorDebugType** ppTypeArgs,[Out] ICorDebugType** ppType)</unmanaged>
        /// <unmanaged-short>ICorDebugClass2::GetParameterizedType</unmanaged-short>
        public unsafe void GetParameterizedType(CorApi.Portable.CorElementType elementType, System.UInt32 nTypeArgs, CorApi.Portable.Type[] typeArgsOut, out CorApi.Portable.Type typeOut)
        {
            System.IntPtr*typeArgsOut_;
            typeArgsOut_ = (System.IntPtr*)0;
            if (typeArgsOut != null)
            {
                System.IntPtr*typeArgsOut__ = stackalloc System.IntPtr[typeArgsOut.Length];
                typeArgsOut_ = typeArgsOut__;
                for (int i = 0; i < typeArgsOut.Length; i++)
                    typeArgsOut_[i] = SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.Type>(typeArgsOut[i]);
            }

            System.IntPtr typeOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, unchecked ((System.Int32)elementType), nTypeArgs, (void *)(typeArgsOut_), (void *)(&typeOut_), (*(void ***)this._nativePointer)[3]);
            typeOut = (typeOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Type(typeOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "elementType">No documentation.</param>
        /// <param name = "nTypeArgs">No documentation.</param>
        /// <param name = "typeArgsOut">No documentation.</param>
        /// <param name = "typeOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugClass2::GetParameterizedType([In] CorElementType elementType,[In] unsigned int nTypeArgs,[In, Buffer] ICorDebugType** ppTypeArgs,[Out] ICorDebugType** ppType)</unmanaged>
        /// <unmanaged-short>ICorDebugClass2::GetParameterizedType</unmanaged-short>
        public unsafe void GetParameterizedType(CorApi.Portable.CorElementType elementType, System.UInt32 nTypeArgs, SharpGen.Runtime.InterfaceArray<CorApi.Portable.Type> typeArgsOut, out CorApi.Portable.Type typeOut)
        {
            System.IntPtr typeOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, unchecked ((System.Int32)elementType), nTypeArgs, (void *)((void *)(typeArgsOut?.NativePointer ?? System.IntPtr.Zero)), (void *)(&typeOut_), (*(void ***)this._nativePointer)[3]);
            typeOut = (typeOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Type(typeOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "elementType">No documentation.</param>
        /// <param name = "nTypeArgs">No documentation.</param>
        /// <param name = "typeArgsOut">No documentation.</param>
        /// <param name = "typeOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugClass2::GetParameterizedType([In] CorElementType elementType,[In] unsigned int nTypeArgs,[In, Buffer] ICorDebugType** ppTypeArgs,[Out] ICorDebugType** ppType)</unmanaged>
        /// <unmanaged-short>ICorDebugClass2::GetParameterizedType</unmanaged-short>
        private unsafe void GetParameterizedType(CorApi.Portable.CorElementType elementType, System.UInt32 nTypeArgs, System.IntPtr typeArgsOut, System.IntPtr typeOut)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, unchecked ((System.Int32)elementType), nTypeArgs, (void *)((void *)typeArgsOut), (void *)((void *)typeOut), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "bIsJustMyCode">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugClass2::SetJMCStatus([In] BOOL bIsJustMyCode)</unmanaged>
        /// <unmanaged-short>ICorDebugClass2::SetJMCStatus</unmanaged-short>
        public unsafe void SetJMCStatus(SharpGen.Runtime.Win32.RawBool bIsJustMyCode)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint0(this._nativePointer, bIsJustMyCode, (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("CC7BCAF4-8A68-11d2-983C-0000F808342D")]
    public partial class Code : SharpGen.Runtime.ComObject
    {
        public Code(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Code(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Code(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetFunction</unmanaged>
        /// <unmanaged-short>GetFunction</unmanaged-short>
        public CorApi.Portable.Function Function
        {
            get
            {
                GetFunction(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "bILRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugCode::IsIL([In] BOOL* pbIL)</unmanaged>
        /// <unmanaged-short>ICorDebugCode::IsIL</unmanaged-short>
        public unsafe void IsIL(SharpGen.Runtime.Win32.RawBool bILRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&bILRef), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "functionOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugCode::GetFunction([In] ICorDebugFunction** ppFunction)</unmanaged>
        /// <unmanaged-short>ICorDebugCode::GetFunction</unmanaged-short>
        internal unsafe void GetFunction(out CorApi.Portable.Function functionOut)
        {
            System.IntPtr functionOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&functionOut_), (*(void ***)this._nativePointer)[4]);
            functionOut = (functionOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Function(functionOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "startRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugCode::GetAddress([In] unsigned longlong* pStart)</unmanaged>
        /// <unmanaged-short>ICorDebugCode::GetAddress</unmanaged-short>
        public unsafe void GetAddress(System.UInt64 startRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&startRef), (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cBytesRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugCode::GetSize([In] unsigned int* pcBytes)</unmanaged>
        /// <unmanaged-short>ICorDebugCode::GetSize</unmanaged-short>
        public unsafe void GetSize(System.UInt32 cBytesRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&cBytesRef), (*(void ***)this._nativePointer)[6]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "offset">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugCode::CreateBreakpoint([In] unsigned int offset,[Out] ICorDebugFunctionBreakpoint** ppBreakpoint)</unmanaged>
        /// <unmanaged-short>ICorDebugCode::CreateBreakpoint</unmanaged-short>
        public unsafe CorApi.Portable.FunctionBreakpoint CreateBreakpoint(System.UInt32 offset)
        {
            CorApi.Portable.FunctionBreakpoint breakpointOut;
            System.IntPtr breakpointOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, offset, (void *)(&breakpointOut_), (*(void ***)this._nativePointer)[7]);
            breakpointOut = (breakpointOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.FunctionBreakpoint(breakpointOut_);
            __result__.CheckError();
            return breakpointOut;
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "startOffset">No documentation.</param>
        /// <param name = "endOffset">No documentation.</param>
        /// <param name = "cBufferAlloc">No documentation.</param>
        /// <param name = "buffer">No documentation.</param>
        /// <param name = "cBufferSizeRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugCode::GetCode([In] unsigned int startOffset,[In] unsigned int endOffset,[In] unsigned int cBufferAlloc,[In] unsigned char* buffer,[In] unsigned int* pcBufferSize)</unmanaged>
        /// <unmanaged-short>ICorDebugCode::GetCode</unmanaged-short>
        public unsafe void GetCode(System.UInt32 startOffset, System.UInt32 endOffset, System.UInt32 cBufferAlloc, System.Byte buffer, System.UInt32 cBufferSizeRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, startOffset, endOffset, cBufferAlloc, (void *)(&buffer), (void *)(&cBufferSizeRef), (*(void ***)this._nativePointer)[8]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "nVersion">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugCode::GetVersionNumber([In] unsigned int* nVersion)</unmanaged>
        /// <unmanaged-short>ICorDebugCode::GetVersionNumber</unmanaged-short>
        public unsafe void GetVersionNumber(System.UInt32 nVersion)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&nVersion), (*(void ***)this._nativePointer)[9]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cMap">No documentation.</param>
        /// <param name = "cMapRef">No documentation.</param>
        /// <param name = "map">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugCode::GetILToNativeMapping([In] unsigned int cMap,[In] unsigned int* pcMap,[In] COR_DEBUG_IL_TO_NATIVE_MAP* map)</unmanaged>
        /// <unmanaged-short>ICorDebugCode::GetILToNativeMapping</unmanaged-short>
        public unsafe void GetILToNativeMapping(System.UInt32 cMap, System.UInt32 cMapRef, CorApi.Portable.CorDebugIlToNativeMap map)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cMap, (void *)(&cMapRef), (void *)(&map), (*(void ***)this._nativePointer)[10]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cMap">No documentation.</param>
        /// <param name = "cMapRef">No documentation.</param>
        /// <param name = "offsets">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugCode::GetEnCRemapSequencePoints([In] unsigned int cMap,[In] unsigned int* pcMap,[In] unsigned int* offsets)</unmanaged>
        /// <unmanaged-short>ICorDebugCode::GetEnCRemapSequencePoints</unmanaged-short>
        public unsafe void GetEnCRemapSequencePoints(System.UInt32 cMap, System.UInt32 cMapRef, System.UInt32 offsets)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cMap, (void *)(&cMapRef), (void *)(&offsets), (*(void ***)this._nativePointer)[11]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("5F696509-452F-4436-A3FE-4D11FE7E2347")]
    public partial class Code2 : SharpGen.Runtime.ComObject
    {
        public Code2(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Code2(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Code2(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cbufSize">No documentation.</param>
        /// <param name = "cnumChunksRef">No documentation.</param>
        /// <param name = "chunks">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugCode2::GetCodeChunks([In] unsigned int cbufSize,[In] unsigned int* pcnumChunks,[In] CodeChunkInfo* chunks)</unmanaged>
        /// <unmanaged-short>ICorDebugCode2::GetCodeChunks</unmanaged-short>
        public unsafe void GetCodeChunks(System.UInt32 cbufSize, System.UInt32 cnumChunksRef, CorApi.Portable.CodeChunkInfo chunks)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cbufSize, (void *)(&cnumChunksRef), (void *)(&chunks), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "dwFlagsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugCode2::GetCompilerFlags([In] unsigned int* pdwFlags)</unmanaged>
        /// <unmanaged-short>ICorDebugCode2::GetCompilerFlags</unmanaged-short>
        public unsafe void GetCompilerFlags(System.UInt32 dwFlagsRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&dwFlagsRef), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("D13D3E88-E1F2-4020-AA1D-3D162DCBE966")]
    public partial class Code3 : SharpGen.Runtime.ComObject
    {
        public Code3(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Code3(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Code3(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "iLoffset">No documentation.</param>
        /// <param name = "bufferSize">No documentation.</param>
        /// <param name = "fetchedRef">No documentation.</param>
        /// <param name = "offsetsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugCode3::GetReturnValueLiveOffset([In] unsigned int ILoffset,[In] unsigned int bufferSize,[In] unsigned int* pFetched,[In] unsigned int* pOffsets)</unmanaged>
        /// <unmanaged-short>ICorDebugCode3::GetReturnValueLiveOffset</unmanaged-short>
        public unsafe void GetReturnValueLiveOffset(System.UInt32 iLoffset, System.UInt32 bufferSize, System.UInt32 fetchedRef, System.UInt32 offsetsRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, iLoffset, bufferSize, (void *)(&fetchedRef), (void *)(&offsetsRef), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("18221fa4-20cb-40fa-b19d-9f91c4fa8c14")]
    public partial class Code4 : SharpGen.Runtime.ComObject
    {
        public Code4(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Code4(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Code4(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "enumOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugCode4::EnumerateVariableHomes([In] ICorDebugVariableHomeEnum** ppEnum)</unmanaged>
        /// <unmanaged-short>ICorDebugCode4::EnumerateVariableHomes</unmanaged-short>
        public unsafe void EnumerateVariableHomes(out CorApi.Portable.VariableHomeEnum enumOut)
        {
            System.IntPtr enumOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&enumOut_), (*(void ***)this._nativePointer)[3]);
            enumOut = (enumOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.VariableHomeEnum(enumOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("55E96461-9645-45e4-A2FF-0367877ABCDE")]
    public partial class CodeEnum : CorApi.Portable.Enum
    {
        public CodeEnum(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator CodeEnum(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new CodeEnum(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "celt">No documentation.</param>
        /// <param name = "values">No documentation.</param>
        /// <param name = "celtFetchedRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugCodeEnum::Next([In] unsigned int celt,[In] ICorDebugCode** values,[In] unsigned int* pceltFetched)</unmanaged>
        /// <unmanaged-short>ICorDebugCodeEnum::Next</unmanaged-short>
        public unsafe void Next(System.UInt32 celt, out CorApi.Portable.Code values, System.UInt32 celtFetchedRef)
        {
            System.IntPtr values_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, celt, (void *)(&values_), (void *)(&celtFetchedRef), (*(void ***)this._nativePointer)[7]);
            values = (values_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Code(values_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("5F69C5E5-3E12-42DF-B371-F9D761D6EE24")]
    public partial class ComObjectValue : SharpGen.Runtime.ComObject
    {
        public ComObjectValue(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator ComObjectValue(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new ComObjectValue(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "bIInspectableOnly">No documentation.</param>
        /// <param name = "interfacesEnumOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugComObjectValue::GetCachedInterfaceTypes([In] BOOL bIInspectableOnly,[In] ICorDebugTypeEnum** ppInterfacesEnum)</unmanaged>
        /// <unmanaged-short>ICorDebugComObjectValue::GetCachedInterfaceTypes</unmanaged-short>
        public unsafe void GetCachedInterfaceTypes(SharpGen.Runtime.Win32.RawBool bIInspectableOnly, out CorApi.Portable.TypeEnum interfacesEnumOut)
        {
            System.IntPtr interfacesEnumOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint0(this._nativePointer, bIInspectableOnly, (void *)(&interfacesEnumOut_), (*(void ***)this._nativePointer)[3]);
            interfacesEnumOut = (interfacesEnumOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.TypeEnum(interfacesEnumOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "bIInspectableOnly">No documentation.</param>
        /// <param name = "celt">No documentation.</param>
        /// <param name = "cEltFetchedRef">No documentation.</param>
        /// <param name = "trsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugComObjectValue::GetCachedInterfacePointers([In] BOOL bIInspectableOnly,[In] unsigned int celt,[In] unsigned int* pcEltFetched,[In] unsigned longlong* ptrs)</unmanaged>
        /// <unmanaged-short>ICorDebugComObjectValue::GetCachedInterfacePointers</unmanaged-short>
        public unsafe void GetCachedInterfacePointers(SharpGen.Runtime.Win32.RawBool bIInspectableOnly, System.UInt32 celt, System.UInt32 cEltFetchedRef, System.UInt64 trsRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint0(this._nativePointer, bIInspectableOnly, celt, (void *)(&cEltFetchedRef), (void *)(&trsRef), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("CC7BCB00-8A68-11d2-983C-0000F808342D")]
    public partial class Context : CorApi.Portable.ObjectValue
    {
        public Context(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Context(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Context(nativePtr);
    }

    [System.Runtime.InteropServices.GuidAttribute("3d6f5f62-7538-11d3-8d5b-00104b35e7ef")]
    public partial class Controller : SharpGen.Runtime.ComObject
    {
        public Controller(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Controller(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Controller(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IsRunning</unmanaged>
        /// <unmanaged-short>IsRunning</unmanaged-short>
        public SharpGen.Runtime.Win32.RawBool IsRunning
        {
            get
            {
                IsRunning_(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "dwTimeoutIgnored">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugController::Stop([In] unsigned int dwTimeoutIgnored)</unmanaged>
        /// <unmanaged-short>ICorDebugController::Stop</unmanaged-short>
        public unsafe void Stop(System.UInt32 dwTimeoutIgnored)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, dwTimeoutIgnored, (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "fIsOutOfBand">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugController::Continue([In] BOOL fIsOutOfBand)</unmanaged>
        /// <unmanaged-short>ICorDebugController::Continue</unmanaged-short>
        public unsafe void ContinueImpl(SharpGen.Runtime.Win32.RawBool fIsOutOfBand)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint0(this._nativePointer, fIsOutOfBand, (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "bRunningRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugController::IsRunning([Out] BOOL* pbRunning)</unmanaged>
        /// <unmanaged-short>ICorDebugController::IsRunning</unmanaged-short>
        internal unsafe void IsRunning_(out SharpGen.Runtime.Win32.RawBool bRunningRef)
        {
            bRunningRef = new SharpGen.Runtime.Win32.RawBool();
            SharpGen.Runtime.Result __result__;
            fixed (void *bRunningRef_ = &bRunningRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(bRunningRef_), (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "threadRef">No documentation.</param>
        /// <param name = "bQueuedRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugController::HasQueuedCallbacks([In] ICorDebugThread* pThread,[Out] BOOL* pbQueued)</unmanaged>
        /// <unmanaged-short>ICorDebugController::HasQueuedCallbacks</unmanaged-short>
        public unsafe void HasQueuedCallbacks(CorApi.Portable.Thread threadRef, out SharpGen.Runtime.Win32.RawBool bQueuedRef)
        {
            bQueuedRef = new SharpGen.Runtime.Win32.RawBool();
            SharpGen.Runtime.Result __result__;
            fixed (void *bQueuedRef_ = &bQueuedRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.Thread>(threadRef))), (void *)(bQueuedRef_), (*(void ***)this._nativePointer)[6]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "threadsOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugController::EnumerateThreads([In] ICorDebugThreadEnum** ppThreads)</unmanaged>
        /// <unmanaged-short>ICorDebugController::EnumerateThreads</unmanaged-short>
        public unsafe void EnumerateThreads(out CorApi.Portable.ThreadEnum threadsOut)
        {
            System.IntPtr threadsOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&threadsOut_), (*(void ***)this._nativePointer)[7]);
            threadsOut = (threadsOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.ThreadEnum(threadsOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "state">No documentation.</param>
        /// <param name = "exceptThisThreadRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugController::SetAllThreadsDebugState([In] CorDebugThreadState state,[In] ICorDebugThread* pExceptThisThread)</unmanaged>
        /// <unmanaged-short>ICorDebugController::SetAllThreadsDebugState</unmanaged-short>
        public unsafe void SetAllThreadsDebugState(CorApi.Portable.CorDebugThreadState state, CorApi.Portable.Thread exceptThisThreadRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, unchecked ((System.Int32)state), (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.Thread>(exceptThisThreadRef))), (*(void ***)this._nativePointer)[8]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugController::Detach()</unmanaged>
        /// <unmanaged-short>ICorDebugController::Detach</unmanaged-short>
        public unsafe void Detach()
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (*(void ***)this._nativePointer)[9]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "exitCode">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugController::Terminate([In] unsigned int exitCode)</unmanaged>
        /// <unmanaged-short>ICorDebugController::Terminate</unmanaged-short>
        public unsafe void Terminate(System.UInt32 exitCode)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, exitCode, (*(void ***)this._nativePointer)[10]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cSnapshots">No documentation.</param>
        /// <param name = "snapshotsRef">No documentation.</param>
        /// <param name = "errorRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugController::CanCommitChanges([In] unsigned int cSnapshots,[In] ICorDebugEditAndContinueSnapshot** pSnapshots,[In] ICorDebugErrorInfoEnum** pError)</unmanaged>
        /// <unmanaged-short>ICorDebugController::CanCommitChanges</unmanaged-short>
        public unsafe void CanCommitChanges(System.UInt32 cSnapshots, out CorApi.Portable.EditAndContinueSnapshot snapshotsRef, out CorApi.Portable.ErrorInfoEnum errorRef)
        {
            System.IntPtr snapshotsRef_ = System.IntPtr.Zero;
            System.IntPtr errorRef_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cSnapshots, (void *)(&snapshotsRef_), (void *)(&errorRef_), (*(void ***)this._nativePointer)[11]);
            snapshotsRef = (snapshotsRef_ == System.IntPtr.Zero) ? null : new CorApi.Portable.EditAndContinueSnapshot(snapshotsRef_);
            errorRef = (errorRef_ == System.IntPtr.Zero) ? null : new CorApi.Portable.ErrorInfoEnum(errorRef_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cSnapshots">No documentation.</param>
        /// <param name = "snapshotsRef">No documentation.</param>
        /// <param name = "errorRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugController::CommitChanges([In] unsigned int cSnapshots,[In] ICorDebugEditAndContinueSnapshot** pSnapshots,[In] ICorDebugErrorInfoEnum** pError)</unmanaged>
        /// <unmanaged-short>ICorDebugController::CommitChanges</unmanaged-short>
        public unsafe void CommitChanges(System.UInt32 cSnapshots, out CorApi.Portable.EditAndContinueSnapshot snapshotsRef, out CorApi.Portable.ErrorInfoEnum errorRef)
        {
            System.IntPtr snapshotsRef_ = System.IntPtr.Zero;
            System.IntPtr errorRef_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cSnapshots, (void *)(&snapshotsRef_), (void *)(&errorRef_), (*(void ***)this._nativePointer)[12]);
            snapshotsRef = (snapshotsRef_ == System.IntPtr.Zero) ? null : new CorApi.Portable.EditAndContinueSnapshot(snapshotsRef_);
            errorRef = (errorRef_ == System.IntPtr.Zero) ? null : new CorApi.Portable.ErrorInfoEnum(errorRef_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("FE06DC28-49FB-4636-A4A3-E80DB4AE116C")]
    public partial class DataTarget : SharpGen.Runtime.ComObject
    {
        public DataTarget(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator DataTarget(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new DataTarget(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "targetPlatformRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugDataTarget::GetPlatform([In] CorDebugPlatform* pTargetPlatform)</unmanaged>
        /// <unmanaged-short>ICorDebugDataTarget::GetPlatform</unmanaged-short>
        public unsafe void GetPlatform(CorApi.Portable.CorDebugPlatform targetPlatformRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&targetPlatformRef), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "address">No documentation.</param>
        /// <param name = "bufferRef">No documentation.</param>
        /// <param name = "bytesRequested">No documentation.</param>
        /// <param name = "bytesReadRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugDataTarget::ReadVirtual([In] unsigned longlong address,[In] unsigned char* pBuffer,[In] unsigned int bytesRequested,[In] unsigned int* pBytesRead)</unmanaged>
        /// <unmanaged-short>ICorDebugDataTarget::ReadVirtual</unmanaged-short>
        public unsafe void ReadVirtual(System.UInt64 address, System.Byte bufferRef, System.UInt32 bytesRequested, System.UInt32 bytesReadRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, address, (void *)(&bufferRef), bytesRequested, (void *)(&bytesReadRef), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "dwThreadID">No documentation.</param>
        /// <param name = "contextFlags">No documentation.</param>
        /// <param name = "contextSize">No documentation.</param>
        /// <param name = "contextRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugDataTarget::GetThreadContext([In] unsigned int dwThreadID,[In] unsigned int contextFlags,[In] unsigned int contextSize,[In] unsigned char* pContext)</unmanaged>
        /// <unmanaged-short>ICorDebugDataTarget::GetThreadContext</unmanaged-short>
        public unsafe void GetThreadContext(System.UInt32 dwThreadID, System.UInt32 contextFlags, System.UInt32 contextSize, System.Byte contextRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, dwThreadID, contextFlags, contextSize, (void *)(&contextRef), (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("2eb364da-605b-4e8d-b333-3394c4828d41")]
    public partial class DataTarget2 : SharpGen.Runtime.ComObject
    {
        public DataTarget2(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator DataTarget2(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new DataTarget2(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "addr">No documentation.</param>
        /// <param name = "imageBaseRef">No documentation.</param>
        /// <param name = "sizeRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugDataTarget2::GetImageFromPointer([In] unsigned longlong addr,[In] unsigned longlong* pImageBase,[In] unsigned int* pSize)</unmanaged>
        /// <unmanaged-short>ICorDebugDataTarget2::GetImageFromPointer</unmanaged-short>
        public unsafe void GetImageFromPointer(System.UInt64 addr, System.UInt64 imageBaseRef, System.UInt32 sizeRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, addr, (void *)(&imageBaseRef), (void *)(&sizeRef), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "baseAddress">No documentation.</param>
        /// <param name = "cchName">No documentation.</param>
        /// <param name = "cchNameRef">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugDataTarget2::GetImageLocation([In] unsigned longlong baseAddress,[In] unsigned int cchName,[In] unsigned int* pcchName,[In] wchar_t* szName)</unmanaged>
        /// <unmanaged-short>ICorDebugDataTarget2::GetImageLocation</unmanaged-short>
        public unsafe void GetImageLocation(System.UInt64 baseAddress, System.UInt32 cchName, System.UInt32 cchNameRef, System.String szName)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, baseAddress, cchName, (void *)(&cchNameRef), (void *)((void *)szName_), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "imageBaseAddress">No documentation.</param>
        /// <param name = "symProviderOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugDataTarget2::GetSymbolProviderForImage([In] unsigned longlong imageBaseAddress,[In] ICorDebugSymbolProvider** ppSymProvider)</unmanaged>
        /// <unmanaged-short>ICorDebugDataTarget2::GetSymbolProviderForImage</unmanaged-short>
        public unsafe void GetSymbolProviderForImage(System.UInt64 imageBaseAddress, out CorApi.Portable.SymbolProvider symProviderOut)
        {
            System.IntPtr symProviderOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, imageBaseAddress, (void *)(&symProviderOut_), (*(void ***)this._nativePointer)[5]);
            symProviderOut = (symProviderOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.SymbolProvider(symProviderOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cThreadIds">No documentation.</param>
        /// <param name = "cThreadIdsRef">No documentation.</param>
        /// <param name = "threadIdsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugDataTarget2::EnumerateThreadIDs([In] unsigned int cThreadIds,[In] unsigned int* pcThreadIds,[In] unsigned int* pThreadIds)</unmanaged>
        /// <unmanaged-short>ICorDebugDataTarget2::EnumerateThreadIDs</unmanaged-short>
        public unsafe void EnumerateThreadIDs(System.UInt32 cThreadIds, System.UInt32 cThreadIdsRef, System.UInt32 threadIdsRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cThreadIds, (void *)(&cThreadIdsRef), (void *)(&threadIdsRef), (*(void ***)this._nativePointer)[6]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "nativeThreadID">No documentation.</param>
        /// <param name = "contextFlags">No documentation.</param>
        /// <param name = "cbContext">No documentation.</param>
        /// <param name = "initialContext">No documentation.</param>
        /// <param name = "unwinderOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugDataTarget2::CreateVirtualUnwinder([In] unsigned int nativeThreadID,[In] unsigned int contextFlags,[In] unsigned int cbContext,[In] unsigned char* initialContext,[In] ICorDebugVirtualUnwinder** ppUnwinder)</unmanaged>
        /// <unmanaged-short>ICorDebugDataTarget2::CreateVirtualUnwinder</unmanaged-short>
        public unsafe void CreateVirtualUnwinder(System.UInt32 nativeThreadID, System.UInt32 contextFlags, System.UInt32 cbContext, System.Byte initialContext, out CorApi.Portable.VirtualUnwinder unwinderOut)
        {
            System.IntPtr unwinderOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, nativeThreadID, contextFlags, cbContext, (void *)(&initialContext), (void *)(&unwinderOut_), (*(void ***)this._nativePointer)[7]);
            unwinderOut = (unwinderOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.VirtualUnwinder(unwinderOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("D05E60C3-848C-4E7D-894E-623320FF6AFA")]
    public partial class DataTarget3 : SharpGen.Runtime.ComObject
    {
        public DataTarget3(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator DataTarget3(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new DataTarget3(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cRequestedModules">No documentation.</param>
        /// <param name = "cFetchedModulesRef">No documentation.</param>
        /// <param name = "loadedModulesRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugDataTarget3::GetLoadedModules([In] unsigned int cRequestedModules,[In] unsigned int* pcFetchedModules,[In] ICorDebugLoadedModule** pLoadedModules)</unmanaged>
        /// <unmanaged-short>ICorDebugDataTarget3::GetLoadedModules</unmanaged-short>
        public unsafe void GetLoadedModules(System.UInt32 cRequestedModules, System.UInt32 cFetchedModulesRef, out CorApi.Portable.LoadedModule loadedModulesRef)
        {
            System.IntPtr loadedModulesRef_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cRequestedModules, (void *)(&cFetchedModulesRef), (void *)(&loadedModulesRef_), (*(void ***)this._nativePointer)[3]);
            loadedModulesRef = (loadedModulesRef_ == System.IntPtr.Zero) ? null : new CorApi.Portable.LoadedModule(loadedModulesRef_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("41BD395D-DE99-48F1-BF7A-CC0F44A6D281")]
    public partial class DebugEvent : SharpGen.Runtime.ComObject
    {
        public DebugEvent(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator DebugEvent(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new DebugEvent(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetThread</unmanaged>
        /// <unmanaged-short>GetThread</unmanaged-short>
        public CorApi.Portable.Thread Thread
        {
            get
            {
                GetThread(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "debugEventKindRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugDebugEvent::GetEventKind([In] CorDebugDebugEventKind* pDebugEventKind)</unmanaged>
        /// <unmanaged-short>ICorDebugDebugEvent::GetEventKind</unmanaged-short>
        public unsafe void GetEventKind(CorApi.Portable.CorDebugDebugEventKind debugEventKindRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&debugEventKindRef), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "threadOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugDebugEvent::GetThread([In] ICorDebugThread** ppThread)</unmanaged>
        /// <unmanaged-short>ICorDebugDebugEvent::GetThread</unmanaged-short>
        internal unsafe void GetThread(out CorApi.Portable.Thread threadOut)
        {
            System.IntPtr threadOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&threadOut_), (*(void ***)this._nativePointer)[4]);
            threadOut = (threadOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Thread(threadOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("8D600D41-F4F6-4cb3-B7EC-7BD164944036")]
    public partial class EditAndContinueErrorInfo : SharpGen.Runtime.ComObject
    {
        public EditAndContinueErrorInfo(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator EditAndContinueErrorInfo(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new EditAndContinueErrorInfo(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetModule</unmanaged>
        /// <unmanaged-short>GetModule</unmanaged-short>
        public CorApi.Portable.Module Module
        {
            get
            {
                GetModule(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "moduleOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEditAndContinueErrorInfo::GetModule([In] ICorDebugModule** ppModule)</unmanaged>
        /// <unmanaged-short>ICorDebugEditAndContinueErrorInfo::GetModule</unmanaged-short>
        internal unsafe void GetModule(out CorApi.Portable.Module moduleOut)
        {
            System.IntPtr moduleOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&moduleOut_), (*(void ***)this._nativePointer)[3]);
            moduleOut = (moduleOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Module(moduleOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tokenRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEditAndContinueErrorInfo::GetToken([In] unsigned int* pToken)</unmanaged>
        /// <unmanaged-short>ICorDebugEditAndContinueErrorInfo::GetToken</unmanaged-short>
        public unsafe void GetToken(System.UInt32 tokenRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&tokenRef), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hrRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEditAndContinueErrorInfo::GetErrorCode([In] HRESULT* pHr)</unmanaged>
        /// <unmanaged-short>ICorDebugEditAndContinueErrorInfo::GetErrorCode</unmanaged-short>
        public unsafe void GetErrorCode(SharpGen.Runtime.Result hrRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&hrRef), (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cchString">No documentation.</param>
        /// <param name = "cchStringRef">No documentation.</param>
        /// <param name = "szString">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEditAndContinueErrorInfo::GetString([In] unsigned int cchString,[In] unsigned int* pcchString,[In] wchar_t* szString)</unmanaged>
        /// <unmanaged-short>ICorDebugEditAndContinueErrorInfo::GetString</unmanaged-short>
        public unsafe void GetString(System.UInt32 cchString, System.UInt32 cchStringRef, System.String szString)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szString_ = szString)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cchString, (void *)(&cchStringRef), (void *)((void *)szString_), (*(void ***)this._nativePointer)[6]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("6DC3FA01-D7CB-11d2-8A95-0080C792E5D8")]
    public partial class EditAndContinueSnapshot : SharpGen.Runtime.ComObject
    {
        public EditAndContinueSnapshot(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator EditAndContinueSnapshot(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new EditAndContinueSnapshot(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "iStreamRef">No documentation.</param>
        /// <param name = "mvidRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEditAndContinueSnapshot::CopyMetaData([In] IStream* pIStream,[In] GUID* pMvid)</unmanaged>
        /// <unmanaged-short>ICorDebugEditAndContinueSnapshot::CopyMetaData</unmanaged-short>
        public unsafe void CopyMetaData(SharpGen.Runtime.Win32.IStream iStreamRef, System.Guid mvidRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<SharpGen.Runtime.Win32.IStream>(iStreamRef))), (void *)(&mvidRef), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "mvidRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEditAndContinueSnapshot::GetMvid([In] GUID* pMvid)</unmanaged>
        /// <unmanaged-short>ICorDebugEditAndContinueSnapshot::GetMvid</unmanaged-short>
        public unsafe void GetMvid(System.Guid mvidRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&mvidRef), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "roDataRVARef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEditAndContinueSnapshot::GetRoDataRVA([In] unsigned int* pRoDataRVA)</unmanaged>
        /// <unmanaged-short>ICorDebugEditAndContinueSnapshot::GetRoDataRVA</unmanaged-short>
        public unsafe void GetRoDataRVA(System.UInt32 roDataRVARef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&roDataRVARef), (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "rwDataRVARef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEditAndContinueSnapshot::GetRwDataRVA([In] unsigned int* pRwDataRVA)</unmanaged>
        /// <unmanaged-short>ICorDebugEditAndContinueSnapshot::GetRwDataRVA</unmanaged-short>
        public unsafe void GetRwDataRVA(System.UInt32 rwDataRVARef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&rwDataRVARef), (*(void ***)this._nativePointer)[6]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "iStreamRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEditAndContinueSnapshot::SetPEBytes([In] IStream* pIStream)</unmanaged>
        /// <unmanaged-short>ICorDebugEditAndContinueSnapshot::SetPEBytes</unmanaged-short>
        public unsafe void SetPEBytes(SharpGen.Runtime.Win32.IStream iStreamRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<SharpGen.Runtime.Win32.IStream>(iStreamRef))), (*(void ***)this._nativePointer)[7]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "mdFunction">No documentation.</param>
        /// <param name = "cMapSize">No documentation.</param>
        /// <param name = "map">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEditAndContinueSnapshot::SetILMap([In] unsigned int mdFunction,[In] unsigned int cMapSize,[In] COR_IL_MAP* map)</unmanaged>
        /// <unmanaged-short>ICorDebugEditAndContinueSnapshot::SetILMap</unmanaged-short>
        public unsafe void SetILMap(System.UInt32 mdFunction, System.UInt32 cMapSize, CorApi.Portable.CorIlMap map)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, mdFunction, cMapSize, (void *)(&map), (*(void ***)this._nativePointer)[8]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "iStreamRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEditAndContinueSnapshot::SetPESymbolBytes([In] IStream* pIStream)</unmanaged>
        /// <unmanaged-short>ICorDebugEditAndContinueSnapshot::SetPESymbolBytes</unmanaged-short>
        public unsafe void SetPESymbolBytes(SharpGen.Runtime.Win32.IStream iStreamRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<SharpGen.Runtime.Win32.IStream>(iStreamRef))), (*(void ***)this._nativePointer)[9]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("CC7BCB01-8A68-11d2-983C-0000F808342D")]
    public partial class Enum : SharpGen.Runtime.ComObject
    {
        public Enum(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Enum(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Enum(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetCount</unmanaged>
        /// <unmanaged-short>GetCount</unmanaged-short>
        public System.UInt32 Count
        {
            get
            {
                GetCount(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "celt">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEnum::Skip([In] unsigned int celt)</unmanaged>
        /// <unmanaged-short>ICorDebugEnum::Skip</unmanaged-short>
        public unsafe void Skip(System.UInt32 celt)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, celt, (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEnum::Reset()</unmanaged>
        /// <unmanaged-short>ICorDebugEnum::Reset</unmanaged-short>
        public unsafe void Reset()
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "enumOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEnum::Clone([In] ICorDebugEnum** ppEnum)</unmanaged>
        /// <unmanaged-short>ICorDebugEnum::Clone</unmanaged-short>
        public unsafe void Clone(out CorApi.Portable.Enum enumOut)
        {
            System.IntPtr enumOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&enumOut_), (*(void ***)this._nativePointer)[5]);
            enumOut = (enumOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Enum(enumOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "celtRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEnum::GetCount([Out] unsigned int* pcelt)</unmanaged>
        /// <unmanaged-short>ICorDebugEnum::GetCount</unmanaged-short>
        internal unsafe void GetCount(out System.UInt32 celtRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *celtRef_ = &celtRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(celtRef_), (*(void ***)this._nativePointer)[6]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("F0E18809-72B5-11d2-976F-00A0C9B4D50C")]
    public partial class ErrorInfoEnum : CorApi.Portable.Enum
    {
        public ErrorInfoEnum(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator ErrorInfoEnum(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new ErrorInfoEnum(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "celt">No documentation.</param>
        /// <param name = "errors">No documentation.</param>
        /// <param name = "celtFetchedRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugErrorInfoEnum::Next([In] unsigned int celt,[In] ICorDebugEditAndContinueErrorInfo** errors,[In] unsigned int* pceltFetched)</unmanaged>
        /// <unmanaged-short>ICorDebugErrorInfoEnum::Next</unmanaged-short>
        public unsafe void Next(System.UInt32 celt, out CorApi.Portable.EditAndContinueErrorInfo errors, System.UInt32 celtFetchedRef)
        {
            System.IntPtr errors_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, celt, (void *)(&errors_), (void *)(&celtFetchedRef), (*(void ***)this._nativePointer)[7]);
            errors = (errors_ == System.IntPtr.Zero) ? null : new CorApi.Portable.EditAndContinueErrorInfo(errors_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("CC7BCAF6-8A68-11d2-983C-0000F808342D")]
    public partial class Eval : SharpGen.Runtime.ComObject
    {
        public Eval(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Eval(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Eval(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetResult</unmanaged>
        /// <unmanaged-short>GetResult</unmanaged-short>
        public CorApi.Portable.Value Result
        {
            get
            {
                GetResult(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetThread</unmanaged>
        /// <unmanaged-short>GetThread</unmanaged-short>
        public CorApi.Portable.Thread Thread
        {
            get
            {
                GetThread(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "functionRef">No documentation.</param>
        /// <param name = "nArgs">No documentation.</param>
        /// <param name = "argsOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEval::CallFunction([In] ICorDebugFunction* pFunction,[In] unsigned int nArgs,[In] ICorDebugValue** ppArgs)</unmanaged>
        /// <unmanaged-short>ICorDebugEval::CallFunction</unmanaged-short>
        public unsafe void CallFunction(CorApi.Portable.Function functionRef, System.UInt32 nArgs, out CorApi.Portable.Value argsOut)
        {
            System.IntPtr argsOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.Function>(functionRef))), nArgs, (void *)(&argsOut_), (*(void ***)this._nativePointer)[3]);
            argsOut = (argsOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Value(argsOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "constructorRef">No documentation.</param>
        /// <param name = "nArgs">No documentation.</param>
        /// <param name = "argsOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEval::NewObject([In] ICorDebugFunction* pConstructor,[In] unsigned int nArgs,[In] ICorDebugValue** ppArgs)</unmanaged>
        /// <unmanaged-short>ICorDebugEval::NewObject</unmanaged-short>
        public unsafe void NewObject(CorApi.Portable.Function constructorRef, System.UInt32 nArgs, out CorApi.Portable.Value argsOut)
        {
            System.IntPtr argsOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.Function>(constructorRef))), nArgs, (void *)(&argsOut_), (*(void ***)this._nativePointer)[4]);
            argsOut = (argsOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Value(argsOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "classRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEval::NewObjectNoConstructor([In] ICorDebugClass* pClass)</unmanaged>
        /// <unmanaged-short>ICorDebugEval::NewObjectNoConstructor</unmanaged-short>
        public unsafe void NewObjectNoConstructor(CorApi.Portable.Class classRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.Class>(classRef))), (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "text">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEval::NewString([In] const wchar_t* string)</unmanaged>
        /// <unmanaged-short>ICorDebugEval::NewString</unmanaged-short>
        public unsafe void NewString(System.String text)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *text_ = text)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)text_), (*(void ***)this._nativePointer)[6]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "elementType">No documentation.</param>
        /// <param name = "elementClassRef">No documentation.</param>
        /// <param name = "rank">No documentation.</param>
        /// <param name = "dims">No documentation.</param>
        /// <param name = "lowBounds">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEval::NewArray([In] CorElementType elementType,[In] ICorDebugClass* pElementClass,[In] unsigned int rank,[In] unsigned int* dims,[In] unsigned int* lowBounds)</unmanaged>
        /// <unmanaged-short>ICorDebugEval::NewArray</unmanaged-short>
        public unsafe void NewArray(CorApi.Portable.CorElementType elementType, CorApi.Portable.Class elementClassRef, System.UInt32 rank, System.UInt32 dims, System.UInt32 lowBounds)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, unchecked ((System.Int32)elementType), (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.Class>(elementClassRef))), rank, (void *)(&dims), (void *)(&lowBounds), (*(void ***)this._nativePointer)[7]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "bActiveRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEval::IsActive([In] BOOL* pbActive)</unmanaged>
        /// <unmanaged-short>ICorDebugEval::IsActive</unmanaged-short>
        public unsafe void IsActive(SharpGen.Runtime.Win32.RawBool bActiveRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&bActiveRef), (*(void ***)this._nativePointer)[8]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEval::Abort()</unmanaged>
        /// <unmanaged-short>ICorDebugEval::Abort</unmanaged-short>
        public unsafe void Abort()
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (*(void ***)this._nativePointer)[9]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "resultOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEval::GetResult([In] ICorDebugValue** ppResult)</unmanaged>
        /// <unmanaged-short>ICorDebugEval::GetResult</unmanaged-short>
        internal unsafe void GetResult(out CorApi.Portable.Value resultOut)
        {
            System.IntPtr resultOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&resultOut_), (*(void ***)this._nativePointer)[10]);
            resultOut = (resultOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Value(resultOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "threadOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEval::GetThread([In] ICorDebugThread** ppThread)</unmanaged>
        /// <unmanaged-short>ICorDebugEval::GetThread</unmanaged-short>
        internal unsafe void GetThread(out CorApi.Portable.Thread threadOut)
        {
            System.IntPtr threadOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&threadOut_), (*(void ***)this._nativePointer)[11]);
            threadOut = (threadOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Thread(threadOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "elementType">No documentation.</param>
        /// <param name = "elementClassRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEval::CreateValue([In] CorElementType elementType,[In] ICorDebugClass* pElementClass,[Out] ICorDebugValue** ppValue)</unmanaged>
        /// <unmanaged-short>ICorDebugEval::CreateValue</unmanaged-short>
        public unsafe CorApi.Portable.Value CreateValue(CorApi.Portable.CorElementType elementType, CorApi.Portable.Class elementClassRef)
        {
            CorApi.Portable.Value valueOut;
            System.IntPtr valueOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, unchecked ((System.Int32)elementType), (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.Class>(elementClassRef))), (void *)(&valueOut_), (*(void ***)this._nativePointer)[12]);
            valueOut = (valueOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Value(valueOut_);
            __result__.CheckError();
            return valueOut;
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("FB0D9CE7-BE66-4683-9D32-A42A04E2FD91")]
    public partial class Eval2 : SharpGen.Runtime.ComObject
    {
        public Eval2(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Eval2(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Eval2(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "functionRef">No documentation.</param>
        /// <param name = "nTypeArgs">No documentation.</param>
        /// <param name = "typeArgsOut">No documentation.</param>
        /// <param name = "nArgs">No documentation.</param>
        /// <param name = "argsOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEval2::CallParameterizedFunction([In] ICorDebugFunction* pFunction,[In] unsigned int nTypeArgs,[In, Buffer] ICorDebugType** ppTypeArgs,[In] unsigned int nArgs,[In, Buffer] ICorDebugValue** ppArgs)</unmanaged>
        /// <unmanaged-short>ICorDebugEval2::CallParameterizedFunction</unmanaged-short>
        public unsafe void CallParameterizedFunction(CorApi.Portable.Function functionRef, System.UInt32 nTypeArgs, CorApi.Portable.Type[] typeArgsOut, System.UInt32 nArgs, CorApi.Portable.Value[] argsOut)
        {
            System.IntPtr*typeArgsOut_;
            typeArgsOut_ = (System.IntPtr*)0;
            if (typeArgsOut != null)
            {
                System.IntPtr*typeArgsOut__ = stackalloc System.IntPtr[typeArgsOut.Length];
                typeArgsOut_ = typeArgsOut__;
                for (int i = 0; i < typeArgsOut.Length; i++)
                    typeArgsOut_[i] = SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.Type>(typeArgsOut[i]);
            }

            System.IntPtr*argsOut_;
            argsOut_ = (System.IntPtr*)0;
            if (argsOut != null)
            {
                System.IntPtr*argsOut__ = stackalloc System.IntPtr[argsOut.Length];
                argsOut_ = argsOut__;
                for (int i = 0; i < argsOut.Length; i++)
                    argsOut_[i] = SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.Value>(argsOut[i]);
            }

            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.Function>(functionRef))), nTypeArgs, (void *)(typeArgsOut_), nArgs, (void *)(argsOut_), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "functionRef">No documentation.</param>
        /// <param name = "nTypeArgs">No documentation.</param>
        /// <param name = "typeArgsOut">No documentation.</param>
        /// <param name = "nArgs">No documentation.</param>
        /// <param name = "argsOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEval2::CallParameterizedFunction([In] ICorDebugFunction* pFunction,[In] unsigned int nTypeArgs,[In, Buffer] ICorDebugType** ppTypeArgs,[In] unsigned int nArgs,[In, Buffer] ICorDebugValue** ppArgs)</unmanaged>
        /// <unmanaged-short>ICorDebugEval2::CallParameterizedFunction</unmanaged-short>
        public unsafe void CallParameterizedFunction(CorApi.Portable.Function functionRef, System.UInt32 nTypeArgs, SharpGen.Runtime.InterfaceArray<CorApi.Portable.Type> typeArgsOut, System.UInt32 nArgs, SharpGen.Runtime.InterfaceArray<CorApi.Portable.Value> argsOut)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.Function>(functionRef))), nTypeArgs, (void *)((void *)(typeArgsOut?.NativePointer ?? System.IntPtr.Zero)), nArgs, (void *)((void *)(argsOut?.NativePointer ?? System.IntPtr.Zero)), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "functionRef">No documentation.</param>
        /// <param name = "nTypeArgs">No documentation.</param>
        /// <param name = "typeArgsOut">No documentation.</param>
        /// <param name = "nArgs">No documentation.</param>
        /// <param name = "argsOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEval2::CallParameterizedFunction([In] ICorDebugFunction* pFunction,[In] unsigned int nTypeArgs,[In, Buffer] ICorDebugType** ppTypeArgs,[In] unsigned int nArgs,[In, Buffer] ICorDebugValue** ppArgs)</unmanaged>
        /// <unmanaged-short>ICorDebugEval2::CallParameterizedFunction</unmanaged-short>
        private unsafe void CallParameterizedFunction(System.IntPtr functionRef, System.UInt32 nTypeArgs, System.IntPtr typeArgsOut, System.UInt32 nArgs, System.IntPtr argsOut)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)functionRef), nTypeArgs, (void *)((void *)typeArgsOut), nArgs, (void *)((void *)argsOut), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "typeRef">No documentation.</param>
        /// <param name = "valueOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEval2::CreateValueForType([In] ICorDebugType* pType,[In] ICorDebugValue** ppValue)</unmanaged>
        /// <unmanaged-short>ICorDebugEval2::CreateValueForType</unmanaged-short>
        public unsafe void CreateValueForType(CorApi.Portable.Type typeRef, out CorApi.Portable.Value valueOut)
        {
            System.IntPtr valueOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.Type>(typeRef))), (void *)(&valueOut_), (*(void ***)this._nativePointer)[4]);
            valueOut = (valueOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Value(valueOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "constructorRef">No documentation.</param>
        /// <param name = "nTypeArgs">No documentation.</param>
        /// <param name = "typeArgsOut">No documentation.</param>
        /// <param name = "nArgs">No documentation.</param>
        /// <param name = "argsOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEval2::NewParameterizedObject([In] ICorDebugFunction* pConstructor,[In] unsigned int nTypeArgs,[In, Buffer] ICorDebugType** ppTypeArgs,[In] unsigned int nArgs,[In, Buffer] ICorDebugValue** ppArgs)</unmanaged>
        /// <unmanaged-short>ICorDebugEval2::NewParameterizedObject</unmanaged-short>
        public unsafe void NewParameterizedObject(CorApi.Portable.Function constructorRef, System.UInt32 nTypeArgs, CorApi.Portable.Type[] typeArgsOut, System.UInt32 nArgs, CorApi.Portable.Value[] argsOut)
        {
            System.IntPtr*typeArgsOut_;
            typeArgsOut_ = (System.IntPtr*)0;
            if (typeArgsOut != null)
            {
                System.IntPtr*typeArgsOut__ = stackalloc System.IntPtr[typeArgsOut.Length];
                typeArgsOut_ = typeArgsOut__;
                for (int i = 0; i < typeArgsOut.Length; i++)
                    typeArgsOut_[i] = SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.Type>(typeArgsOut[i]);
            }

            System.IntPtr*argsOut_;
            argsOut_ = (System.IntPtr*)0;
            if (argsOut != null)
            {
                System.IntPtr*argsOut__ = stackalloc System.IntPtr[argsOut.Length];
                argsOut_ = argsOut__;
                for (int i = 0; i < argsOut.Length; i++)
                    argsOut_[i] = SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.Value>(argsOut[i]);
            }

            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.Function>(constructorRef))), nTypeArgs, (void *)(typeArgsOut_), nArgs, (void *)(argsOut_), (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "constructorRef">No documentation.</param>
        /// <param name = "nTypeArgs">No documentation.</param>
        /// <param name = "typeArgsOut">No documentation.</param>
        /// <param name = "nArgs">No documentation.</param>
        /// <param name = "argsOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEval2::NewParameterizedObject([In] ICorDebugFunction* pConstructor,[In] unsigned int nTypeArgs,[In, Buffer] ICorDebugType** ppTypeArgs,[In] unsigned int nArgs,[In, Buffer] ICorDebugValue** ppArgs)</unmanaged>
        /// <unmanaged-short>ICorDebugEval2::NewParameterizedObject</unmanaged-short>
        public unsafe void NewParameterizedObject(CorApi.Portable.Function constructorRef, System.UInt32 nTypeArgs, SharpGen.Runtime.InterfaceArray<CorApi.Portable.Type> typeArgsOut, System.UInt32 nArgs, SharpGen.Runtime.InterfaceArray<CorApi.Portable.Value> argsOut)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.Function>(constructorRef))), nTypeArgs, (void *)((void *)(typeArgsOut?.NativePointer ?? System.IntPtr.Zero)), nArgs, (void *)((void *)(argsOut?.NativePointer ?? System.IntPtr.Zero)), (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "constructorRef">No documentation.</param>
        /// <param name = "nTypeArgs">No documentation.</param>
        /// <param name = "typeArgsOut">No documentation.</param>
        /// <param name = "nArgs">No documentation.</param>
        /// <param name = "argsOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEval2::NewParameterizedObject([In] ICorDebugFunction* pConstructor,[In] unsigned int nTypeArgs,[In, Buffer] ICorDebugType** ppTypeArgs,[In] unsigned int nArgs,[In, Buffer] ICorDebugValue** ppArgs)</unmanaged>
        /// <unmanaged-short>ICorDebugEval2::NewParameterizedObject</unmanaged-short>
        private unsafe void NewParameterizedObject(System.IntPtr constructorRef, System.UInt32 nTypeArgs, System.IntPtr typeArgsOut, System.UInt32 nArgs, System.IntPtr argsOut)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)constructorRef), nTypeArgs, (void *)((void *)typeArgsOut), nArgs, (void *)((void *)argsOut), (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "classRef">No documentation.</param>
        /// <param name = "nTypeArgs">No documentation.</param>
        /// <param name = "typeArgsOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEval2::NewParameterizedObjectNoConstructor([In] ICorDebugClass* pClass,[In] unsigned int nTypeArgs,[In] ICorDebugType** ppTypeArgs)</unmanaged>
        /// <unmanaged-short>ICorDebugEval2::NewParameterizedObjectNoConstructor</unmanaged-short>
        public unsafe void NewParameterizedObjectNoConstructor(CorApi.Portable.Class classRef, System.UInt32 nTypeArgs, out CorApi.Portable.Type typeArgsOut)
        {
            System.IntPtr typeArgsOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.Class>(classRef))), nTypeArgs, (void *)(&typeArgsOut_), (*(void ***)this._nativePointer)[6]);
            typeArgsOut = (typeArgsOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Type(typeArgsOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "elementTypeRef">No documentation.</param>
        /// <param name = "rank">No documentation.</param>
        /// <param name = "dims">No documentation.</param>
        /// <param name = "lowBounds">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEval2::NewParameterizedArray([In] ICorDebugType* pElementType,[In] unsigned int rank,[In] unsigned int* dims,[In, Optional] unsigned int* lowBounds)</unmanaged>
        /// <unmanaged-short>ICorDebugEval2::NewParameterizedArray</unmanaged-short>
        public unsafe void NewParameterizedArray(CorApi.Portable.Type elementTypeRef, System.UInt32 rank, System.UInt32 dims, System.UInt32? lowBounds)
        {
            System.UInt32 lowBounds_;
            if (lowBounds != null)
                lowBounds_ = lowBounds.Value;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.Type>(elementTypeRef))), rank, (void *)(&dims), (void *)(lowBounds == null ? (void *)0 : &lowBounds_), (*(void ***)this._nativePointer)[7]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "text">No documentation.</param>
        /// <param name = "uiLength">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEval2::NewStringWithLength([In] const wchar_t* string,[In] unsigned int uiLength)</unmanaged>
        /// <unmanaged-short>ICorDebugEval2::NewStringWithLength</unmanaged-short>
        public unsafe void NewStringWithLength(System.String text, System.UInt32 uiLength)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *text_ = text)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)text_), uiLength, (*(void ***)this._nativePointer)[8]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugEval2::RudeAbort()</unmanaged>
        /// <unmanaged-short>ICorDebugEval2::RudeAbort</unmanaged-short>
        public unsafe void RudeAbort()
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (*(void ***)this._nativePointer)[9]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("AF79EC94-4752-419C-A626-5FB1CC1A5AB7")]
    public partial class ExceptionDebugEvent : CorApi.Portable.DebugEvent
    {
        public ExceptionDebugEvent(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator ExceptionDebugEvent(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new ExceptionDebugEvent(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "stackPointerRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugExceptionDebugEvent::GetStackPointer([In] unsigned longlong* pStackPointer)</unmanaged>
        /// <unmanaged-short>ICorDebugExceptionDebugEvent::GetStackPointer</unmanaged-short>
        public unsafe void GetStackPointer(System.UInt64 stackPointerRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&stackPointerRef), (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "iPRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugExceptionDebugEvent::GetNativeIP([In] unsigned longlong* pIP)</unmanaged>
        /// <unmanaged-short>ICorDebugExceptionDebugEvent::GetNativeIP</unmanaged-short>
        public unsafe void GetNativeIP(System.UInt64 iPRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&iPRef), (*(void ***)this._nativePointer)[6]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "dwFlagsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugExceptionDebugEvent::GetFlags([In] CorDebugExceptionFlags* pdwFlags)</unmanaged>
        /// <unmanaged-short>ICorDebugExceptionDebugEvent::GetFlags</unmanaged-short>
        public unsafe void GetFlags(CorApi.Portable.CorDebugExceptionFlags dwFlagsRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&dwFlagsRef), (*(void ***)this._nativePointer)[7]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("ED775530-4DC4-41F7-86D0-9E2DEF7DFC66")]
    public partial class ExceptionObjectCallStackEnum : CorApi.Portable.Enum
    {
        public ExceptionObjectCallStackEnum(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator ExceptionObjectCallStackEnum(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new ExceptionObjectCallStackEnum(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "celt">No documentation.</param>
        /// <param name = "values">No documentation.</param>
        /// <param name = "celtFetchedRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugExceptionObjectCallStackEnum::Next([In] unsigned int celt,[In] CorDebugExceptionObjectStackFrame* values,[In] unsigned int* pceltFetched)</unmanaged>
        /// <unmanaged-short>ICorDebugExceptionObjectCallStackEnum::Next</unmanaged-short>
        public unsafe void Next(System.UInt32 celt, ref CorApi.Portable.CorDebugExceptionObjectStackFrame values, System.UInt32 celtFetchedRef)
        {
            var values_ = new CorApi.Portable.CorDebugExceptionObjectStackFrame.__Native();
            values.__MarshalTo(ref values_);
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, celt, (void *)(&values_), (void *)(&celtFetchedRef), (*(void ***)this._nativePointer)[7]);
            values.__MarshalFree(ref values_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("AE4CA65D-59DD-42A2-83A5-57E8A08D8719")]
    public partial class ExceptionObjectValue : SharpGen.Runtime.ComObject
    {
        public ExceptionObjectValue(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator ExceptionObjectValue(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new ExceptionObjectValue(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "callStackEnumOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugExceptionObjectValue::EnumerateExceptionCallStack([In] ICorDebugExceptionObjectCallStackEnum** ppCallStackEnum)</unmanaged>
        /// <unmanaged-short>ICorDebugExceptionObjectValue::EnumerateExceptionCallStack</unmanaged-short>
        public unsafe void EnumerateExceptionCallStack(out CorApi.Portable.ExceptionObjectCallStackEnum callStackEnumOut)
        {
            System.IntPtr callStackEnumOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&callStackEnumOut_), (*(void ***)this._nativePointer)[3]);
            callStackEnumOut = (callStackEnumOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.ExceptionObjectCallStackEnum(callStackEnumOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("CC7BCAEF-8A68-11d2-983C-0000F808342D")]
    public partial class Frame : SharpGen.Runtime.ComObject
    {
        public Frame(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Frame(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Frame(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetChain</unmanaged>
        /// <unmanaged-short>GetChain</unmanaged-short>
        public CorApi.Portable.Chain Chain
        {
            get
            {
                GetChain(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetCode</unmanaged>
        /// <unmanaged-short>GetCode</unmanaged-short>
        public CorApi.Portable.Code Code
        {
            get
            {
                GetCode(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetFunction</unmanaged>
        /// <unmanaged-short>GetFunction</unmanaged-short>
        public CorApi.Portable.Function Function
        {
            get
            {
                GetFunction(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetCaller</unmanaged>
        /// <unmanaged-short>GetCaller</unmanaged-short>
        public CorApi.Portable.Frame Caller
        {
            get
            {
                GetCaller(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetCallee</unmanaged>
        /// <unmanaged-short>GetCallee</unmanaged-short>
        public CorApi.Portable.Frame Callee
        {
            get
            {
                GetCallee(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "chainOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugFrame::GetChain([In] ICorDebugChain** ppChain)</unmanaged>
        /// <unmanaged-short>ICorDebugFrame::GetChain</unmanaged-short>
        internal unsafe void GetChain(out CorApi.Portable.Chain chainOut)
        {
            System.IntPtr chainOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&chainOut_), (*(void ***)this._nativePointer)[3]);
            chainOut = (chainOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Chain(chainOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "codeOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugFrame::GetCode([In] ICorDebugCode** ppCode)</unmanaged>
        /// <unmanaged-short>ICorDebugFrame::GetCode</unmanaged-short>
        internal unsafe void GetCode(out CorApi.Portable.Code codeOut)
        {
            System.IntPtr codeOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&codeOut_), (*(void ***)this._nativePointer)[4]);
            codeOut = (codeOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Code(codeOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "functionOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugFrame::GetFunction([In] ICorDebugFunction** ppFunction)</unmanaged>
        /// <unmanaged-short>ICorDebugFrame::GetFunction</unmanaged-short>
        internal unsafe void GetFunction(out CorApi.Portable.Function functionOut)
        {
            System.IntPtr functionOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&functionOut_), (*(void ***)this._nativePointer)[5]);
            functionOut = (functionOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Function(functionOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tokenRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugFrame::GetFunctionToken([In] unsigned int* pToken)</unmanaged>
        /// <unmanaged-short>ICorDebugFrame::GetFunctionToken</unmanaged-short>
        public unsafe void GetFunctionToken(System.UInt32 tokenRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&tokenRef), (*(void ***)this._nativePointer)[6]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "startRef">No documentation.</param>
        /// <param name = "endRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugFrame::GetStackRange([In] unsigned longlong* pStart,[In] unsigned longlong* pEnd)</unmanaged>
        /// <unmanaged-short>ICorDebugFrame::GetStackRange</unmanaged-short>
        public unsafe void GetStackRange(System.UInt64 startRef, System.UInt64 endRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&startRef), (void *)(&endRef), (*(void ***)this._nativePointer)[7]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "frameOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugFrame::GetCaller([In] ICorDebugFrame** ppFrame)</unmanaged>
        /// <unmanaged-short>ICorDebugFrame::GetCaller</unmanaged-short>
        internal unsafe void GetCaller(out CorApi.Portable.Frame frameOut)
        {
            System.IntPtr frameOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&frameOut_), (*(void ***)this._nativePointer)[8]);
            frameOut = (frameOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Frame(frameOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "frameOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugFrame::GetCallee([In] ICorDebugFrame** ppFrame)</unmanaged>
        /// <unmanaged-short>ICorDebugFrame::GetCallee</unmanaged-short>
        internal unsafe void GetCallee(out CorApi.Portable.Frame frameOut)
        {
            System.IntPtr frameOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&frameOut_), (*(void ***)this._nativePointer)[9]);
            frameOut = (frameOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Frame(frameOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "stepperOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugFrame::CreateStepper([In] ICorDebugStepper** ppStepper)</unmanaged>
        /// <unmanaged-short>ICorDebugFrame::CreateStepper</unmanaged-short>
        public unsafe void CreateStepper(out CorApi.Portable.Stepper stepperOut)
        {
            System.IntPtr stepperOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&stepperOut_), (*(void ***)this._nativePointer)[10]);
            stepperOut = (stepperOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Stepper(stepperOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("CC7BCB07-8A68-11d2-983C-0000F808342D")]
    public partial class FrameEnum : CorApi.Portable.Enum
    {
        public FrameEnum(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator FrameEnum(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new FrameEnum(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "celt">No documentation.</param>
        /// <param name = "frames">No documentation.</param>
        /// <param name = "celtFetchedRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugFrameEnum::Next([In] unsigned int celt,[Out, Buffer] ICorDebugFrame** frames,[Out] unsigned int* pceltFetched)</unmanaged>
        /// <unmanaged-short>ICorDebugFrameEnum::Next</unmanaged-short>
        public unsafe void Next(System.UInt32 celt, CorApi.Portable.Frame[] frames, out System.UInt32 celtFetchedRef)
        {
            System.IntPtr*frames_ = stackalloc System.IntPtr[frames.Length];
            SharpGen.Runtime.Result __result__;
            fixed (void *celtFetchedRef_ = &celtFetchedRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, celt, (void *)(frames_), (void *)(celtFetchedRef_), (*(void ***)this._nativePointer)[7]);
            for (int i = 0; i < frames.Length; i++)
                frames[i] = (frames_[i] == System.IntPtr.Zero) ? null : new CorApi.Portable.Frame(frames_[i]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("CC7BCAF3-8A68-11d2-983C-0000F808342D")]
    public partial class Function : SharpGen.Runtime.ComObject
    {
        public Function(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Function(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Function(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetModule</unmanaged>
        /// <unmanaged-short>GetModule</unmanaged-short>
        public CorApi.Portable.Module Module
        {
            get
            {
                GetModule(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetClass</unmanaged>
        /// <unmanaged-short>GetClass</unmanaged-short>
        public CorApi.Portable.Class Class
        {
            get
            {
                GetClass(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetToken</unmanaged>
        /// <unmanaged-short>GetToken</unmanaged-short>
        public System.UInt32 Token
        {
            get
            {
                GetToken(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetILCode</unmanaged>
        /// <unmanaged-short>GetILCode</unmanaged-short>
        public CorApi.Portable.Code ILCode
        {
            get
            {
                GetILCode(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetNativeCode</unmanaged>
        /// <unmanaged-short>GetNativeCode</unmanaged-short>
        public CorApi.Portable.Code NativeCode
        {
            get
            {
                GetNativeCode(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "moduleOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugFunction::GetModule([In] ICorDebugModule** ppModule)</unmanaged>
        /// <unmanaged-short>ICorDebugFunction::GetModule</unmanaged-short>
        internal unsafe void GetModule(out CorApi.Portable.Module moduleOut)
        {
            System.IntPtr moduleOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&moduleOut_), (*(void ***)this._nativePointer)[3]);
            moduleOut = (moduleOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Module(moduleOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "classOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugFunction::GetClass([In] ICorDebugClass** ppClass)</unmanaged>
        /// <unmanaged-short>ICorDebugFunction::GetClass</unmanaged-short>
        internal unsafe void GetClass(out CorApi.Portable.Class classOut)
        {
            System.IntPtr classOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&classOut_), (*(void ***)this._nativePointer)[4]);
            classOut = (classOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Class(classOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "methodDefRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugFunction::GetToken([Out] unsigned int* pMethodDef)</unmanaged>
        /// <unmanaged-short>ICorDebugFunction::GetToken</unmanaged-short>
        internal unsafe void GetToken(out System.UInt32 methodDefRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *methodDefRef_ = &methodDefRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(methodDefRef_), (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "codeOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugFunction::GetILCode([In] ICorDebugCode** ppCode)</unmanaged>
        /// <unmanaged-short>ICorDebugFunction::GetILCode</unmanaged-short>
        internal unsafe void GetILCode(out CorApi.Portable.Code codeOut)
        {
            System.IntPtr codeOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&codeOut_), (*(void ***)this._nativePointer)[6]);
            codeOut = (codeOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Code(codeOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "codeOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugFunction::GetNativeCode([In] ICorDebugCode** ppCode)</unmanaged>
        /// <unmanaged-short>ICorDebugFunction::GetNativeCode</unmanaged-short>
        internal unsafe void GetNativeCode(out CorApi.Portable.Code codeOut)
        {
            System.IntPtr codeOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&codeOut_), (*(void ***)this._nativePointer)[7]);
            codeOut = (codeOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Code(codeOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "breakpointOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugFunction::CreateBreakpoint([In] ICorDebugFunctionBreakpoint** ppBreakpoint)</unmanaged>
        /// <unmanaged-short>ICorDebugFunction::CreateBreakpoint</unmanaged-short>
        public unsafe void CreateBreakpoint(out CorApi.Portable.FunctionBreakpoint breakpointOut)
        {
            System.IntPtr breakpointOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&breakpointOut_), (*(void ***)this._nativePointer)[8]);
            breakpointOut = (breakpointOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.FunctionBreakpoint(breakpointOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "mdSigRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugFunction::GetLocalVarSigToken([In] unsigned int* pmdSig)</unmanaged>
        /// <unmanaged-short>ICorDebugFunction::GetLocalVarSigToken</unmanaged-short>
        public unsafe void GetLocalVarSigToken(System.UInt32 mdSigRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&mdSigRef), (*(void ***)this._nativePointer)[9]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "nCurrentVersionRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugFunction::GetCurrentVersionNumber([In] unsigned int* pnCurrentVersion)</unmanaged>
        /// <unmanaged-short>ICorDebugFunction::GetCurrentVersionNumber</unmanaged-short>
        public unsafe void GetCurrentVersionNumber(System.UInt32 nCurrentVersionRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&nCurrentVersionRef), (*(void ***)this._nativePointer)[10]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("EF0C490B-94C3-4e4d-B629-DDC134C532D8")]
    public partial class Function2 : SharpGen.Runtime.ComObject
    {
        public Function2(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Function2(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Function2(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "bIsJustMyCode">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugFunction2::SetJMCStatus([In] BOOL bIsJustMyCode)</unmanaged>
        /// <unmanaged-short>ICorDebugFunction2::SetJMCStatus</unmanaged-short>
        public unsafe void SetJMCStatus(SharpGen.Runtime.Win32.RawBool bIsJustMyCode)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint0(this._nativePointer, bIsJustMyCode, (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "bIsJustMyCodeRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugFunction2::GetJMCStatus([In] BOOL* pbIsJustMyCode)</unmanaged>
        /// <unmanaged-short>ICorDebugFunction2::GetJMCStatus</unmanaged-short>
        public unsafe void GetJMCStatus(SharpGen.Runtime.Win32.RawBool bIsJustMyCodeRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&bIsJustMyCodeRef), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "codeEnumOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugFunction2::EnumerateNativeCode([In] ICorDebugCodeEnum** ppCodeEnum)</unmanaged>
        /// <unmanaged-short>ICorDebugFunction2::EnumerateNativeCode</unmanaged-short>
        public unsafe void EnumerateNativeCode(out CorApi.Portable.CodeEnum codeEnumOut)
        {
            System.IntPtr codeEnumOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&codeEnumOut_), (*(void ***)this._nativePointer)[5]);
            codeEnumOut = (codeEnumOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.CodeEnum(codeEnumOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "nVersionRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugFunction2::GetVersionNumber([In] unsigned int* pnVersion)</unmanaged>
        /// <unmanaged-short>ICorDebugFunction2::GetVersionNumber</unmanaged-short>
        public unsafe void GetVersionNumber(System.UInt32 nVersionRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&nVersionRef), (*(void ***)this._nativePointer)[6]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("09B70F28-E465-482D-99E0-81A165EB0532")]
    public partial class Function3 : SharpGen.Runtime.ComObject
    {
        public Function3(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Function3(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Function3(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetActiveReJitRequestILCode</unmanaged>
        /// <unmanaged-short>GetActiveReJitRequestILCode</unmanaged-short>
        public CorApi.Portable.ILCode ActiveReJitRequestILCode
        {
            get
            {
                GetActiveReJitRequestILCode(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "reJitedILCodeOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugFunction3::GetActiveReJitRequestILCode([In] ICorDebugILCode** ppReJitedILCode)</unmanaged>
        /// <unmanaged-short>ICorDebugFunction3::GetActiveReJitRequestILCode</unmanaged-short>
        internal unsafe void GetActiveReJitRequestILCode(out CorApi.Portable.ILCode reJitedILCodeOut)
        {
            System.IntPtr reJitedILCodeOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&reJitedILCodeOut_), (*(void ***)this._nativePointer)[3]);
            reJitedILCodeOut = (reJitedILCodeOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.ILCode(reJitedILCodeOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("CC7BCAE9-8A68-11d2-983C-0000F808342D")]
    public partial class FunctionBreakpoint : CorApi.Portable.Breakpoint
    {
        public FunctionBreakpoint(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator FunctionBreakpoint(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new FunctionBreakpoint(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetFunction</unmanaged>
        /// <unmanaged-short>GetFunction</unmanaged-short>
        public CorApi.Portable.Function Function
        {
            get
            {
                GetFunction(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "functionOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugFunctionBreakpoint::GetFunction([In] ICorDebugFunction** ppFunction)</unmanaged>
        /// <unmanaged-short>ICorDebugFunctionBreakpoint::GetFunction</unmanaged-short>
        internal unsafe void GetFunction(out CorApi.Portable.Function functionOut)
        {
            System.IntPtr functionOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&functionOut_), (*(void ***)this._nativePointer)[5]);
            functionOut = (functionOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Function(functionOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "nOffsetRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugFunctionBreakpoint::GetOffset([In] unsigned int* pnOffset)</unmanaged>
        /// <unmanaged-short>ICorDebugFunctionBreakpoint::GetOffset</unmanaged-short>
        public unsafe void GetOffset(System.UInt32 nOffsetRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&nOffsetRef), (*(void ***)this._nativePointer)[6]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("7F3C24D3-7E1D-4245-AC3A-F72F8859C80C")]
    public partial class GCReferenceEnum : CorApi.Portable.Enum
    {
        public GCReferenceEnum(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator GCReferenceEnum(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new GCReferenceEnum(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "celt">No documentation.</param>
        /// <param name = "roots">No documentation.</param>
        /// <param name = "celtFetchedRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugGCReferenceEnum::Next([In] unsigned int celt,[In] COR_GC_REFERENCE* roots,[In] unsigned int* pceltFetched)</unmanaged>
        /// <unmanaged-short>ICorDebugGCReferenceEnum::Next</unmanaged-short>
        public unsafe void Next(System.UInt32 celt, ref CorApi.Portable.CorGcReference roots, System.UInt32 celtFetchedRef)
        {
            var roots_ = new CorApi.Portable.CorGcReference.__Native();
            roots.__MarshalTo(ref roots_);
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, celt, (void *)(&roots_), (void *)(&celtFetchedRef), (*(void ***)this._nativePointer)[7]);
            roots.__MarshalFree(ref roots_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("CC7BCAF8-8A68-11d2-983C-0000F808342D")]
    public partial class GenericValue : CorApi.Portable.Value
    {
        public GenericValue(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator GenericValue(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new GenericValue(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "toRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugGenericValue::GetValue([In] void* pTo)</unmanaged>
        /// <unmanaged-short>ICorDebugGenericValue::GetValue</unmanaged-short>
        public unsafe void GetValue(System.IntPtr toRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)toRef), (*(void ***)this._nativePointer)[7]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "fromRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugGenericValue::SetValue([In] void* pFrom)</unmanaged>
        /// <unmanaged-short>ICorDebugGenericValue::SetValue</unmanaged-short>
        public unsafe void SetValue(System.IntPtr fromRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)fromRef), (*(void ***)this._nativePointer)[8]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("6164D242-1015-4BD6-8CBE-D0DBD4B8275A")]
    public partial class GuidToTypeEnum : CorApi.Portable.Enum
    {
        public GuidToTypeEnum(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator GuidToTypeEnum(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new GuidToTypeEnum(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "celt">No documentation.</param>
        /// <param name = "values">No documentation.</param>
        /// <param name = "celtFetchedRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugGuidToTypeEnum::Next([In] unsigned int celt,[In] CorDebugGuidToTypeMapping* values,[In] unsigned int* pceltFetched)</unmanaged>
        /// <unmanaged-short>ICorDebugGuidToTypeEnum::Next</unmanaged-short>
        public unsafe void Next(System.UInt32 celt, ref CorApi.Portable.CorDebugGuidToTypeMapping values, System.UInt32 celtFetchedRef)
        {
            var values_ = new CorApi.Portable.CorDebugGuidToTypeMapping.__Native();
            values.__MarshalTo(ref values_);
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, celt, (void *)(&values_), (void *)(&celtFetchedRef), (*(void ***)this._nativePointer)[7]);
            values.__MarshalFree(ref values_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("029596E8-276B-46a1-9821-732E96BBB00B")]
    public partial class HandleValue : CorApi.Portable.ReferenceValue
    {
        public HandleValue(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator HandleValue(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new HandleValue(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "typeRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugHandleValue::GetHandleType([In] CorDebugHandleType* pType)</unmanaged>
        /// <unmanaged-short>ICorDebugHandleValue::GetHandleType</unmanaged-short>
        public unsafe void GetHandleType(CorApi.Portable.CorDebugHandleType typeRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&typeRef), (*(void ***)this._nativePointer)[12]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugHandleValue::Dispose()</unmanaged>
        /// <unmanaged-short>ICorDebugHandleValue::Dispose</unmanaged-short>
        public unsafe void Dispose()
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (*(void ***)this._nativePointer)[13]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("76D7DAB8-D044-11DF-9A15-7E29DFD72085")]
    public partial class HeapEnum : CorApi.Portable.Enum
    {
        public HeapEnum(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator HeapEnum(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new HeapEnum(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "celt">No documentation.</param>
        /// <param name = "objects">No documentation.</param>
        /// <param name = "celtFetchedRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugHeapEnum::Next([In] unsigned int celt,[In] COR_HEAPOBJECT* objects,[In] unsigned int* pceltFetched)</unmanaged>
        /// <unmanaged-short>ICorDebugHeapEnum::Next</unmanaged-short>
        public unsafe void Next(System.UInt32 celt, ref CorApi.Portable.CorHeapobject objects, System.UInt32 celtFetchedRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *objects_ = &objects)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, celt, (void *)(objects_), (void *)(&celtFetchedRef), (*(void ***)this._nativePointer)[7]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("A2FA0F8E-D045-11DF-AC8E-CE2ADFD72085")]
    public partial class HeapSegmentEnum : CorApi.Portable.Enum
    {
        public HeapSegmentEnum(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator HeapSegmentEnum(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new HeapSegmentEnum(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "celt">No documentation.</param>
        /// <param name = "segments">No documentation.</param>
        /// <param name = "celtFetchedRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugHeapSegmentEnum::Next([In] unsigned int celt,[In] COR_SEGMENT* segments,[In] unsigned int* pceltFetched)</unmanaged>
        /// <unmanaged-short>ICorDebugHeapSegmentEnum::Next</unmanaged-short>
        public unsafe void Next(System.UInt32 celt, ref CorApi.Portable.CorSegment segments, System.UInt32 celtFetchedRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *segments_ = &segments)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, celt, (void *)(segments_), (void *)(&celtFetchedRef), (*(void ***)this._nativePointer)[7]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("CC7BCAFA-8A68-11d2-983C-0000F808342D")]
    public partial class HeapValue : CorApi.Portable.Value
    {
        public HeapValue(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator HeapValue(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new HeapValue(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "bValidRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugHeapValue::IsValid([In] BOOL* pbValid)</unmanaged>
        /// <unmanaged-short>ICorDebugHeapValue::IsValid</unmanaged-short>
        public unsafe void IsValid(SharpGen.Runtime.Win32.RawBool bValidRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&bValidRef), (*(void ***)this._nativePointer)[7]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "breakpointOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugHeapValue::CreateRelocBreakpoint([In] ICorDebugValueBreakpoint** ppBreakpoint)</unmanaged>
        /// <unmanaged-short>ICorDebugHeapValue::CreateRelocBreakpoint</unmanaged-short>
        public unsafe void CreateRelocBreakpoint(out CorApi.Portable.ValueBreakpoint breakpointOut)
        {
            System.IntPtr breakpointOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&breakpointOut_), (*(void ***)this._nativePointer)[8]);
            breakpointOut = (breakpointOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.ValueBreakpoint(breakpointOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("E3AC4D6C-9CB7-43e6-96CC-B21540E5083C")]
    public partial class HeapValue2 : SharpGen.Runtime.ComObject
    {
        public HeapValue2(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator HeapValue2(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new HeapValue2(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "type">No documentation.</param>
        /// <param name = "handleOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugHeapValue2::CreateHandle([In] CorDebugHandleType type,[In] ICorDebugHandleValue** ppHandle)</unmanaged>
        /// <unmanaged-short>ICorDebugHeapValue2::CreateHandle</unmanaged-short>
        public unsafe void CreateHandle(CorApi.Portable.CorDebugHandleType type, out CorApi.Portable.HandleValue handleOut)
        {
            System.IntPtr handleOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, unchecked ((System.Int32)type), (void *)(&handleOut_), (*(void ***)this._nativePointer)[3]);
            handleOut = (handleOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.HandleValue(handleOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("A69ACAD8-2374-46e9-9FF8-B1F14120D296")]
    public partial class HeapValue3 : SharpGen.Runtime.ComObject
    {
        public HeapValue3(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator HeapValue3(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new HeapValue3(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetMonitorEventWaitList</unmanaged>
        /// <unmanaged-short>GetMonitorEventWaitList</unmanaged-short>
        public CorApi.Portable.ThreadEnum MonitorEventWaitList
        {
            get
            {
                GetMonitorEventWaitList(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "threadOut">No documentation.</param>
        /// <param name = "acquisitionCountRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugHeapValue3::GetThreadOwningMonitorLock([In] ICorDebugThread** ppThread,[In] unsigned int* pAcquisitionCount)</unmanaged>
        /// <unmanaged-short>ICorDebugHeapValue3::GetThreadOwningMonitorLock</unmanaged-short>
        public unsafe void GetThreadOwningMonitorLock(out CorApi.Portable.Thread threadOut, System.UInt32 acquisitionCountRef)
        {
            System.IntPtr threadOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&threadOut_), (void *)(&acquisitionCountRef), (*(void ***)this._nativePointer)[3]);
            threadOut = (threadOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Thread(threadOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "threadEnumOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugHeapValue3::GetMonitorEventWaitList([In] ICorDebugThreadEnum** ppThreadEnum)</unmanaged>
        /// <unmanaged-short>ICorDebugHeapValue3::GetMonitorEventWaitList</unmanaged-short>
        internal unsafe void GetMonitorEventWaitList(out CorApi.Portable.ThreadEnum threadEnumOut)
        {
            System.IntPtr threadEnumOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&threadEnumOut_), (*(void ***)this._nativePointer)[4]);
            threadEnumOut = (threadEnumOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.ThreadEnum(threadEnumOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("7ed1bdff-8e36-11d2-9c56-00a0c9b7cc45")]
    public partial class ICeeGen : SharpGen.Runtime.ComObject
    {
        public ICeeGen(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator ICeeGen(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new ICeeGen(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetIMapTokenIface</unmanaged>
        /// <unmanaged-short>GetIMapTokenIface</unmanaged-short>
        public SharpGen.Runtime.IUnknown IMapTokenIface
        {
            get
            {
                GetIMapTokenIface(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "lpString">No documentation.</param>
        /// <param name = "rva">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICeeGen::EmitString([In] wchar_t* lpString,[In] unsigned int* RVA)</unmanaged>
        /// <unmanaged-short>ICeeGen::EmitString</unmanaged-short>
        public unsafe void EmitString(System.String lpString, System.UInt32 rva)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *lpString_ = lpString)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)lpString_), (void *)(&rva), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "rva">No documentation.</param>
        /// <param name = "lpString">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICeeGen::GetString([In] unsigned int RVA,[Out, Optional] wchar_t** lpString)</unmanaged>
        /// <unmanaged-short>ICeeGen::GetString</unmanaged-short>
        public unsafe void GetString(System.UInt32 rva, System.IntPtr lpString)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, rva, (void *)((void *)lpString), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cchBuffer">No documentation.</param>
        /// <param name = "lpBuffer">No documentation.</param>
        /// <param name = "rva">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICeeGen::AllocateMethodBuffer([In] unsigned int cchBuffer,[In] unsigned char** lpBuffer,[In] unsigned int* RVA)</unmanaged>
        /// <unmanaged-short>ICeeGen::AllocateMethodBuffer</unmanaged-short>
        public unsafe void AllocateMethodBuffer(System.UInt32 cchBuffer, System.Byte lpBuffer, System.UInt32 rva)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cchBuffer, (void *)(&lpBuffer), (void *)(&rva), (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "rva">No documentation.</param>
        /// <param name = "lpBuffer">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICeeGen::GetMethodBuffer([In] unsigned int RVA,[In] unsigned char** lpBuffer)</unmanaged>
        /// <unmanaged-short>ICeeGen::GetMethodBuffer</unmanaged-short>
        public unsafe void GetMethodBuffer(System.UInt32 rva, System.Byte lpBuffer)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, rva, (void *)(&lpBuffer), (*(void ***)this._nativePointer)[6]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "iMapTokenRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICeeGen::GetIMapTokenIface([In] IUnknown** pIMapToken)</unmanaged>
        /// <unmanaged-short>ICeeGen::GetIMapTokenIface</unmanaged-short>
        internal unsafe void GetIMapTokenIface(out SharpGen.Runtime.IUnknown iMapTokenRef)
        {
            System.IntPtr iMapTokenRef_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&iMapTokenRef_), (*(void ***)this._nativePointer)[7]);
            iMapTokenRef = (iMapTokenRef_ == System.IntPtr.Zero) ? null : new SharpGen.Runtime.ComObject(iMapTokenRef_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICeeGen::GenerateCeeFile()</unmanaged>
        /// <unmanaged-short>ICeeGen::GenerateCeeFile</unmanaged-short>
        public unsafe void GenerateCeeFile()
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (*(void ***)this._nativePointer)[8]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "section">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICeeGen::GetIlSection([In] void** section)</unmanaged>
        /// <unmanaged-short>ICeeGen::GetIlSection</unmanaged-short>
        public unsafe void GetIlSection(System.IntPtr section)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)section), (*(void ***)this._nativePointer)[9]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "section">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICeeGen::GetStringSection([In] void** section)</unmanaged>
        /// <unmanaged-short>ICeeGen::GetStringSection</unmanaged-short>
        public unsafe void GetStringSection(System.IntPtr section)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)section), (*(void ***)this._nativePointer)[10]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "section">No documentation.</param>
        /// <param name = "offset">No documentation.</param>
        /// <param name = "relativeTo">No documentation.</param>
        /// <param name = "relocType">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICeeGen::AddSectionReloc([In] void* section,[In] unsigned int offset,[In] void* relativeTo,[In] CeeSectionRelocType relocType)</unmanaged>
        /// <unmanaged-short>ICeeGen::AddSectionReloc</unmanaged-short>
        public unsafe void AddSectionReloc(System.IntPtr section, System.UInt32 offset, System.IntPtr relativeTo, CorApi.Portable.CeeSectionRelocType relocType)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)section), offset, (void *)((void *)relativeTo), unchecked ((System.Int32)relocType), (*(void ***)this._nativePointer)[11]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "name">No documentation.</param>
        /// <param name = "flags">No documentation.</param>
        /// <param name = "section">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICeeGen::GetSectionCreate([In] const char* name,[In] unsigned int flags,[In] void** section)</unmanaged>
        /// <unmanaged-short>ICeeGen::GetSectionCreate</unmanaged-short>
        public unsafe void GetSectionCreate(System.String name, System.UInt32 flags, System.IntPtr section)
        {
            System.IntPtr name_ = System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(name);
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)name_), flags, (void *)((void *)section), (*(void ***)this._nativePointer)[12]);
            System.Runtime.InteropServices.Marshal.FreeHGlobal(name_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "section">No documentation.</param>
        /// <param name = "dataLen">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICeeGen::GetSectionDataLen([In] void* section,[In] unsigned int* dataLen)</unmanaged>
        /// <unmanaged-short>ICeeGen::GetSectionDataLen</unmanaged-short>
        public unsafe void GetSectionDataLen(System.IntPtr section, System.UInt32 dataLen)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)section), (void *)(&dataLen), (*(void ***)this._nativePointer)[13]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "section">No documentation.</param>
        /// <param name = "len">No documentation.</param>
        /// <param name = "align">No documentation.</param>
        /// <param name = "bytesOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICeeGen::GetSectionBlock([In] void* section,[In] unsigned int len,[In] unsigned int align,[In] void** ppBytes)</unmanaged>
        /// <unmanaged-short>ICeeGen::GetSectionBlock</unmanaged-short>
        public unsafe void GetSectionBlock(System.IntPtr section, System.UInt32 len, System.UInt32 align, System.IntPtr bytesOut)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)section), len, align, (void *)((void *)bytesOut), (*(void ***)this._nativePointer)[14]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "section">No documentation.</param>
        /// <param name = "len">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICeeGen::TruncateSection([In] void* section,[In] unsigned int len)</unmanaged>
        /// <unmanaged-short>ICeeGen::TruncateSection</unmanaged-short>
        public unsafe void TruncateSection(System.IntPtr section, System.UInt32 len)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)section), len, (*(void ***)this._nativePointer)[15]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "imageOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICeeGen::GenerateCeeMemoryImage([In] void** ppImage)</unmanaged>
        /// <unmanaged-short>ICeeGen::GenerateCeeMemoryImage</unmanaged-short>
        public unsafe void GenerateCeeMemoryImage(System.IntPtr imageOut)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)imageOut), (*(void ***)this._nativePointer)[16]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "section">No documentation.</param>
        /// <param name = "rva">No documentation.</param>
        /// <param name = "lpBuffer">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICeeGen::ComputePointer([In] void* section,[In] unsigned int RVA,[In] unsigned char** lpBuffer)</unmanaged>
        /// <unmanaged-short>ICeeGen::ComputePointer</unmanaged-short>
        public unsafe void ComputePointer(System.IntPtr section, System.UInt32 rva, System.Byte lpBuffer)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)section), rva, (void *)(&lpBuffer), (*(void ***)this._nativePointer)[17]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("d0e80dd3-12d4-11d3-b39d-00c04ff81795")]
    public partial class IHostFilter : SharpGen.Runtime.ComObject
    {
        public IHostFilter(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator IHostFilter(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new IHostFilter(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tk">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IHostFilter::MarkToken([In] unsigned int tk)</unmanaged>
        /// <unmanaged-short>IHostFilter::MarkToken</unmanaged-short>
        public unsafe void MarkToken(System.UInt32 tk)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, tk, (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("598D46C2-C877-42A7-89D2-3D0C7F1C1264")]
    public partial class ILCode : SharpGen.Runtime.ComObject
    {
        public ILCode(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator ILCode(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new ILCode(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cClauses">No documentation.</param>
        /// <param name = "cClausesRef">No documentation.</param>
        /// <param name = "clauses">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugILCode::GetEHClauses([In] unsigned int cClauses,[In] unsigned int* pcClauses,[In] CorDebugEHClause* clauses)</unmanaged>
        /// <unmanaged-short>ICorDebugILCode::GetEHClauses</unmanaged-short>
        public unsafe void GetEHClauses(System.UInt32 cClauses, System.UInt32 cClausesRef, ref CorApi.Portable.CorDebugEHClause clauses)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *clauses_ = &clauses)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cClauses, (void *)(&cClausesRef), (void *)(clauses_), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("46586093-D3F5-4DB6-ACDB-955BCE228C15")]
    public partial class ILCode2 : SharpGen.Runtime.ComObject
    {
        public ILCode2(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator ILCode2(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new ILCode2(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "mdSigRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugILCode2::GetLocalVarSigToken([In] unsigned int* pmdSig)</unmanaged>
        /// <unmanaged-short>ICorDebugILCode2::GetLocalVarSigToken</unmanaged-short>
        public unsafe void GetLocalVarSigToken(System.UInt32 mdSigRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&mdSigRef), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cMap">No documentation.</param>
        /// <param name = "cMapRef">No documentation.</param>
        /// <param name = "map">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugILCode2::GetInstrumentedILMap([In] unsigned int cMap,[In] unsigned int* pcMap,[In] COR_IL_MAP* map)</unmanaged>
        /// <unmanaged-short>ICorDebugILCode2::GetInstrumentedILMap</unmanaged-short>
        public unsafe void GetInstrumentedILMap(System.UInt32 cMap, System.UInt32 cMapRef, CorApi.Portable.CorIlMap map)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cMap, (void *)(&cMapRef), (void *)(&map), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("03E26311-4F76-11d3-88C6-006097945418")]
    public partial class ILFrame : CorApi.Portable.Frame
    {
        public ILFrame(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator ILFrame(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new ILFrame(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "nOffsetRef">No documentation.</param>
        /// <param name = "mappingResultRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugILFrame::GetIP([Out] unsigned int* pnOffset,[Out] CorDebugMappingResult* pMappingResult)</unmanaged>
        /// <unmanaged-short>ICorDebugILFrame::GetIP</unmanaged-short>
        public unsafe void GetIP(out System.UInt32 nOffsetRef, out CorApi.Portable.CorDebugMappingResult mappingResultRef)
        {
            mappingResultRef = new CorApi.Portable.CorDebugMappingResult();
            SharpGen.Runtime.Result __result__;
            fixed (void *mappingResultRef_ = &mappingResultRef)
                fixed (void *nOffsetRef_ = &nOffsetRef)
                    __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(nOffsetRef_), (void *)(mappingResultRef_), (*(void ***)this._nativePointer)[11]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "nOffset">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugILFrame::SetIP([In] unsigned int nOffset)</unmanaged>
        /// <unmanaged-short>ICorDebugILFrame::SetIP</unmanaged-short>
        public unsafe void SetIP(System.UInt32 nOffset)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, nOffset, (*(void ***)this._nativePointer)[12]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "valueEnumOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugILFrame::EnumerateLocalVariables([In] ICorDebugValueEnum** ppValueEnum)</unmanaged>
        /// <unmanaged-short>ICorDebugILFrame::EnumerateLocalVariables</unmanaged-short>
        public unsafe void EnumerateLocalVariables(out CorApi.Portable.ValueEnum valueEnumOut)
        {
            System.IntPtr valueEnumOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&valueEnumOut_), (*(void ***)this._nativePointer)[13]);
            valueEnumOut = (valueEnumOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.ValueEnum(valueEnumOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "dwIndex">No documentation.</param>
        /// <param name = "valueOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugILFrame::GetLocalVariable([In] unsigned int dwIndex,[In] ICorDebugValue** ppValue)</unmanaged>
        /// <unmanaged-short>ICorDebugILFrame::GetLocalVariable</unmanaged-short>
        public unsafe void GetLocalVariable(System.UInt32 dwIndex, out CorApi.Portable.Value valueOut)
        {
            System.IntPtr valueOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, dwIndex, (void *)(&valueOut_), (*(void ***)this._nativePointer)[14]);
            valueOut = (valueOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Value(valueOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "valueEnumOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugILFrame::EnumerateArguments([In] ICorDebugValueEnum** ppValueEnum)</unmanaged>
        /// <unmanaged-short>ICorDebugILFrame::EnumerateArguments</unmanaged-short>
        public unsafe void EnumerateArguments(out CorApi.Portable.ValueEnum valueEnumOut)
        {
            System.IntPtr valueEnumOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&valueEnumOut_), (*(void ***)this._nativePointer)[15]);
            valueEnumOut = (valueEnumOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.ValueEnum(valueEnumOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "dwIndex">No documentation.</param>
        /// <param name = "valueOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugILFrame::GetArgument([In] unsigned int dwIndex,[In] ICorDebugValue** ppValue)</unmanaged>
        /// <unmanaged-short>ICorDebugILFrame::GetArgument</unmanaged-short>
        public unsafe void GetArgument(System.UInt32 dwIndex, out CorApi.Portable.Value valueOut)
        {
            System.IntPtr valueOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, dwIndex, (void *)(&valueOut_), (*(void ***)this._nativePointer)[16]);
            valueOut = (valueOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Value(valueOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "depthRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugILFrame::GetStackDepth([In] unsigned int* pDepth)</unmanaged>
        /// <unmanaged-short>ICorDebugILFrame::GetStackDepth</unmanaged-short>
        public unsafe void GetStackDepth(System.UInt32 depthRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&depthRef), (*(void ***)this._nativePointer)[17]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "dwIndex">No documentation.</param>
        /// <param name = "valueOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugILFrame::GetStackValue([In] unsigned int dwIndex,[In] ICorDebugValue** ppValue)</unmanaged>
        /// <unmanaged-short>ICorDebugILFrame::GetStackValue</unmanaged-short>
        public unsafe void GetStackValue(System.UInt32 dwIndex, out CorApi.Portable.Value valueOut)
        {
            System.IntPtr valueOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, dwIndex, (void *)(&valueOut_), (*(void ***)this._nativePointer)[18]);
            valueOut = (valueOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Value(valueOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "nOffset">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugILFrame::CanSetIP([Out] unsigned int nOffset)</unmanaged>
        /// <unmanaged-short>ICorDebugILFrame::CanSetIP</unmanaged-short>
        public unsafe void CanSetIP(System.UInt32 nOffset)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, nOffset, (*(void ***)this._nativePointer)[19]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("5D88A994-6C30-479b-890F-BCEF88B129A5")]
    public partial class ILFrame2 : SharpGen.Runtime.ComObject
    {
        public ILFrame2(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator ILFrame2(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new ILFrame2(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "newILOffset">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugILFrame2::RemapFunction([In] unsigned int newILOffset)</unmanaged>
        /// <unmanaged-short>ICorDebugILFrame2::RemapFunction</unmanaged-short>
        public unsafe void RemapFunction(System.UInt32 newILOffset)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, newILOffset, (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tyParEnumOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugILFrame2::EnumerateTypeParameters([In] ICorDebugTypeEnum** ppTyParEnum)</unmanaged>
        /// <unmanaged-short>ICorDebugILFrame2::EnumerateTypeParameters</unmanaged-short>
        public unsafe void EnumerateTypeParameters(out CorApi.Portable.TypeEnum tyParEnumOut)
        {
            System.IntPtr tyParEnumOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&tyParEnumOut_), (*(void ***)this._nativePointer)[4]);
            tyParEnumOut = (tyParEnumOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.TypeEnum(tyParEnumOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("9A9E2ED6-04DF-4FE0-BB50-CAB64126AD24")]
    public partial class ILFrame3 : SharpGen.Runtime.ComObject
    {
        public ILFrame3(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator ILFrame3(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new ILFrame3(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "iLoffset">No documentation.</param>
        /// <param name = "returnValueOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugILFrame3::GetReturnValueForILOffset([In] unsigned int ILoffset,[In] ICorDebugValue** ppReturnValue)</unmanaged>
        /// <unmanaged-short>ICorDebugILFrame3::GetReturnValueForILOffset</unmanaged-short>
        public unsafe void GetReturnValueForILOffset(System.UInt32 iLoffset, out CorApi.Portable.Value returnValueOut)
        {
            System.IntPtr returnValueOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, iLoffset, (void *)(&returnValueOut_), (*(void ***)this._nativePointer)[3]);
            returnValueOut = (returnValueOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Value(returnValueOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("AD914A30-C6D1-4AC5-9C5E-577F3BAA8A45")]
    public partial class ILFrame4 : SharpGen.Runtime.ComObject
    {
        public ILFrame4(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator ILFrame4(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new ILFrame4(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "flags">No documentation.</param>
        /// <param name = "valueEnumOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugILFrame4::EnumerateLocalVariablesEx([In] ILCodeKind flags,[In] ICorDebugValueEnum** ppValueEnum)</unmanaged>
        /// <unmanaged-short>ICorDebugILFrame4::EnumerateLocalVariablesEx</unmanaged-short>
        public unsafe void EnumerateLocalVariablesEx(CorApi.Portable.ILCodeKind flags, out CorApi.Portable.ValueEnum valueEnumOut)
        {
            System.IntPtr valueEnumOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, unchecked ((System.Int32)flags), (void *)(&valueEnumOut_), (*(void ***)this._nativePointer)[3]);
            valueEnumOut = (valueEnumOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.ValueEnum(valueEnumOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "flags">No documentation.</param>
        /// <param name = "dwIndex">No documentation.</param>
        /// <param name = "valueOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugILFrame4::GetLocalVariableEx([In] ILCodeKind flags,[In] unsigned int dwIndex,[In] ICorDebugValue** ppValue)</unmanaged>
        /// <unmanaged-short>ICorDebugILFrame4::GetLocalVariableEx</unmanaged-short>
        public unsafe void GetLocalVariableEx(CorApi.Portable.ILCodeKind flags, System.UInt32 dwIndex, out CorApi.Portable.Value valueOut)
        {
            System.IntPtr valueOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, unchecked ((System.Int32)flags), dwIndex, (void *)(&valueOut_), (*(void ***)this._nativePointer)[4]);
            valueOut = (valueOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Value(valueOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "flags">No documentation.</param>
        /// <param name = "codeOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugILFrame4::GetCodeEx([In] ILCodeKind flags,[In] ICorDebugCode** ppCode)</unmanaged>
        /// <unmanaged-short>ICorDebugILFrame4::GetCodeEx</unmanaged-short>
        public unsafe void GetCodeEx(CorApi.Portable.ILCodeKind flags, out CorApi.Portable.Code codeOut)
        {
            System.IntPtr codeOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, unchecked ((System.Int32)flags), (void *)(&codeOut_), (*(void ***)this._nativePointer)[5]);
            codeOut = (codeOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Code(codeOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("06a3ea8b-0225-11d1-bf72-00c04fc31e12")]
    public partial class IMapToken : SharpGen.Runtime.ComObject
    {
        public IMapToken(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator IMapToken(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new IMapToken(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tkImp">No documentation.</param>
        /// <param name = "tkEmit">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMapToken::Map([In] unsigned int tkImp,[In] unsigned int tkEmit)</unmanaged>
        /// <unmanaged-short>IMapToken::Map</unmanaged-short>
        public unsafe void Map(System.UInt32 tkImp, System.UInt32 tkEmit)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, tkImp, tkEmit, (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("211ef15b-5317-4438-b196-dec87b887693")]
    public partial class IMetaDataAssemblyEmit : SharpGen.Runtime.ComObject
    {
        public IMetaDataAssemblyEmit(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator IMetaDataAssemblyEmit(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new IMetaDataAssemblyEmit(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "bPublicKeyRef">No documentation.</param>
        /// <param name = "cbPublicKey">No documentation.</param>
        /// <param name = "ulHashAlgId">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "metaDataRef">No documentation.</param>
        /// <param name = "dwAssemblyFlags">No documentation.</param>
        /// <param name = "maRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataAssemblyEmit::DefineAssembly([In] const void* pbPublicKey,[In] unsigned int cbPublicKey,[In] unsigned int ulHashAlgId,[In] const wchar_t* szName,[In] const ASSEMBLYMETADATA* pMetaData,[In] unsigned int dwAssemblyFlags,[In] unsigned int* pma)</unmanaged>
        /// <unmanaged-short>IMetaDataAssemblyEmit::DefineAssembly</unmanaged-short>
        public unsafe void DefineAssembly(System.IntPtr bPublicKeyRef, System.UInt32 cbPublicKey, System.UInt32 ulHashAlgId, System.String szName, ref CorApi.Portable.Assemblymetadata metaDataRef, System.UInt32 dwAssemblyFlags, System.UInt32 maRef)
        {
            var metaDataRef_ = new CorApi.Portable.Assemblymetadata.__Native();
            metaDataRef.__MarshalTo(ref metaDataRef_);
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)bPublicKeyRef), cbPublicKey, ulHashAlgId, (void *)((void *)szName_), (void *)(&metaDataRef_), dwAssemblyFlags, (void *)(&maRef), (*(void ***)this._nativePointer)[3]);
            metaDataRef.__MarshalFree(ref metaDataRef_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "bPublicKeyOrTokenRef">No documentation.</param>
        /// <param name = "cbPublicKeyOrToken">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "metaDataRef">No documentation.</param>
        /// <param name = "bHashValueRef">No documentation.</param>
        /// <param name = "cbHashValue">No documentation.</param>
        /// <param name = "dwAssemblyRefFlags">No documentation.</param>
        /// <param name = "mdarRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataAssemblyEmit::DefineAssemblyRef([In] const void* pbPublicKeyOrToken,[In] unsigned int cbPublicKeyOrToken,[In] const wchar_t* szName,[In] const ASSEMBLYMETADATA* pMetaData,[In] const void* pbHashValue,[In] unsigned int cbHashValue,[In] unsigned int dwAssemblyRefFlags,[In] unsigned int* pmdar)</unmanaged>
        /// <unmanaged-short>IMetaDataAssemblyEmit::DefineAssemblyRef</unmanaged-short>
        public unsafe void DefineAssemblyRef(System.IntPtr bPublicKeyOrTokenRef, System.UInt32 cbPublicKeyOrToken, System.String szName, ref CorApi.Portable.Assemblymetadata metaDataRef, System.IntPtr bHashValueRef, System.UInt32 cbHashValue, System.UInt32 dwAssemblyRefFlags, System.UInt32 mdarRef)
        {
            var metaDataRef_ = new CorApi.Portable.Assemblymetadata.__Native();
            metaDataRef.__MarshalTo(ref metaDataRef_);
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)bPublicKeyOrTokenRef), cbPublicKeyOrToken, (void *)((void *)szName_), (void *)(&metaDataRef_), (void *)((void *)bHashValueRef), cbHashValue, dwAssemblyRefFlags, (void *)(&mdarRef), (*(void ***)this._nativePointer)[4]);
            metaDataRef.__MarshalFree(ref metaDataRef_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "bHashValueRef">No documentation.</param>
        /// <param name = "cbHashValue">No documentation.</param>
        /// <param name = "dwFileFlags">No documentation.</param>
        /// <param name = "mdfRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataAssemblyEmit::DefineFile([In] const wchar_t* szName,[In] const void* pbHashValue,[In] unsigned int cbHashValue,[In] unsigned int dwFileFlags,[In] unsigned int* pmdf)</unmanaged>
        /// <unmanaged-short>IMetaDataAssemblyEmit::DefineFile</unmanaged-short>
        public unsafe void DefineFile(System.String szName, System.IntPtr bHashValueRef, System.UInt32 cbHashValue, System.UInt32 dwFileFlags, System.UInt32 mdfRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)szName_), (void *)((void *)bHashValueRef), cbHashValue, dwFileFlags, (void *)(&mdfRef), (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "tkImplementation">No documentation.</param>
        /// <param name = "tkTypeDef">No documentation.</param>
        /// <param name = "dwExportedTypeFlags">No documentation.</param>
        /// <param name = "mdctRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataAssemblyEmit::DefineExportedType([In] const wchar_t* szName,[In] unsigned int tkImplementation,[In] unsigned int tkTypeDef,[In] unsigned int dwExportedTypeFlags,[In] unsigned int* pmdct)</unmanaged>
        /// <unmanaged-short>IMetaDataAssemblyEmit::DefineExportedType</unmanaged-short>
        public unsafe void DefineExportedType(System.String szName, System.UInt32 tkImplementation, System.UInt32 tkTypeDef, System.UInt32 dwExportedTypeFlags, System.UInt32 mdctRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)szName_), tkImplementation, tkTypeDef, dwExportedTypeFlags, (void *)(&mdctRef), (*(void ***)this._nativePointer)[6]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "tkImplementation">No documentation.</param>
        /// <param name = "dwOffset">No documentation.</param>
        /// <param name = "dwResourceFlags">No documentation.</param>
        /// <param name = "mdmrRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataAssemblyEmit::DefineManifestResource([In] const wchar_t* szName,[In] unsigned int tkImplementation,[In] unsigned int dwOffset,[In] unsigned int dwResourceFlags,[In] unsigned int* pmdmr)</unmanaged>
        /// <unmanaged-short>IMetaDataAssemblyEmit::DefineManifestResource</unmanaged-short>
        public unsafe void DefineManifestResource(System.String szName, System.UInt32 tkImplementation, System.UInt32 dwOffset, System.UInt32 dwResourceFlags, System.UInt32 mdmrRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)szName_), tkImplementation, dwOffset, dwResourceFlags, (void *)(&mdmrRef), (*(void ***)this._nativePointer)[7]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "pma">No documentation.</param>
        /// <param name = "bPublicKeyRef">No documentation.</param>
        /// <param name = "cbPublicKey">No documentation.</param>
        /// <param name = "ulHashAlgId">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "metaDataRef">No documentation.</param>
        /// <param name = "dwAssemblyFlags">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataAssemblyEmit::SetAssemblyProps([In] unsigned int pma,[In] const void* pbPublicKey,[In] unsigned int cbPublicKey,[In] unsigned int ulHashAlgId,[In] const wchar_t* szName,[In] const ASSEMBLYMETADATA* pMetaData,[In] unsigned int dwAssemblyFlags)</unmanaged>
        /// <unmanaged-short>IMetaDataAssemblyEmit::SetAssemblyProps</unmanaged-short>
        public unsafe void SetAssemblyProps(System.UInt32 pma, System.IntPtr bPublicKeyRef, System.UInt32 cbPublicKey, System.UInt32 ulHashAlgId, System.String szName, ref CorApi.Portable.Assemblymetadata metaDataRef, System.UInt32 dwAssemblyFlags)
        {
            var metaDataRef_ = new CorApi.Portable.Assemblymetadata.__Native();
            metaDataRef.__MarshalTo(ref metaDataRef_);
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, pma, (void *)((void *)bPublicKeyRef), cbPublicKey, ulHashAlgId, (void *)((void *)szName_), (void *)(&metaDataRef_), dwAssemblyFlags, (*(void ***)this._nativePointer)[8]);
            metaDataRef.__MarshalFree(ref metaDataRef_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "ar">No documentation.</param>
        /// <param name = "bPublicKeyOrTokenRef">No documentation.</param>
        /// <param name = "cbPublicKeyOrToken">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "metaDataRef">No documentation.</param>
        /// <param name = "bHashValueRef">No documentation.</param>
        /// <param name = "cbHashValue">No documentation.</param>
        /// <param name = "dwAssemblyRefFlags">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataAssemblyEmit::SetAssemblyRefProps([In] unsigned int ar,[In] const void* pbPublicKeyOrToken,[In] unsigned int cbPublicKeyOrToken,[In] const wchar_t* szName,[In] const ASSEMBLYMETADATA* pMetaData,[In] const void* pbHashValue,[In] unsigned int cbHashValue,[In] unsigned int dwAssemblyRefFlags)</unmanaged>
        /// <unmanaged-short>IMetaDataAssemblyEmit::SetAssemblyRefProps</unmanaged-short>
        public unsafe void SetAssemblyRefProps(System.UInt32 ar, System.IntPtr bPublicKeyOrTokenRef, System.UInt32 cbPublicKeyOrToken, System.String szName, ref CorApi.Portable.Assemblymetadata metaDataRef, System.IntPtr bHashValueRef, System.UInt32 cbHashValue, System.UInt32 dwAssemblyRefFlags)
        {
            var metaDataRef_ = new CorApi.Portable.Assemblymetadata.__Native();
            metaDataRef.__MarshalTo(ref metaDataRef_);
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, ar, (void *)((void *)bPublicKeyOrTokenRef), cbPublicKeyOrToken, (void *)((void *)szName_), (void *)(&metaDataRef_), (void *)((void *)bHashValueRef), cbHashValue, dwAssemblyRefFlags, (*(void ***)this._nativePointer)[9]);
            metaDataRef.__MarshalFree(ref metaDataRef_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "file">No documentation.</param>
        /// <param name = "bHashValueRef">No documentation.</param>
        /// <param name = "cbHashValue">No documentation.</param>
        /// <param name = "dwFileFlags">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataAssemblyEmit::SetFileProps([In] unsigned int file,[In] const void* pbHashValue,[In] unsigned int cbHashValue,[In] unsigned int dwFileFlags)</unmanaged>
        /// <unmanaged-short>IMetaDataAssemblyEmit::SetFileProps</unmanaged-short>
        public unsafe void SetFileProps(System.UInt32 file, System.IntPtr bHashValueRef, System.UInt32 cbHashValue, System.UInt32 dwFileFlags)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, file, (void *)((void *)bHashValueRef), cbHashValue, dwFileFlags, (*(void ***)this._nativePointer)[10]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "ct">No documentation.</param>
        /// <param name = "tkImplementation">No documentation.</param>
        /// <param name = "tkTypeDef">No documentation.</param>
        /// <param name = "dwExportedTypeFlags">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataAssemblyEmit::SetExportedTypeProps([In] unsigned int ct,[In] unsigned int tkImplementation,[In] unsigned int tkTypeDef,[In] unsigned int dwExportedTypeFlags)</unmanaged>
        /// <unmanaged-short>IMetaDataAssemblyEmit::SetExportedTypeProps</unmanaged-short>
        public unsafe void SetExportedTypeProps(System.UInt32 ct, System.UInt32 tkImplementation, System.UInt32 tkTypeDef, System.UInt32 dwExportedTypeFlags)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, ct, tkImplementation, tkTypeDef, dwExportedTypeFlags, (*(void ***)this._nativePointer)[11]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "mr">No documentation.</param>
        /// <param name = "tkImplementation">No documentation.</param>
        /// <param name = "dwOffset">No documentation.</param>
        /// <param name = "dwResourceFlags">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataAssemblyEmit::SetManifestResourceProps([In] unsigned int mr,[In] unsigned int tkImplementation,[In] unsigned int dwOffset,[In] unsigned int dwResourceFlags)</unmanaged>
        /// <unmanaged-short>IMetaDataAssemblyEmit::SetManifestResourceProps</unmanaged-short>
        public unsafe void SetManifestResourceProps(System.UInt32 mr, System.UInt32 tkImplementation, System.UInt32 dwOffset, System.UInt32 dwResourceFlags)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, mr, tkImplementation, dwOffset, dwResourceFlags, (*(void ***)this._nativePointer)[12]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("ee62470b-e94b-424e-9b7c-2f00c9249f93")]
    public partial class IMetaDataAssemblyImport : SharpGen.Runtime.ComObject
    {
        public IMetaDataAssemblyImport(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator IMetaDataAssemblyImport(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new IMetaDataAssemblyImport(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "mda">No documentation.</param>
        /// <param name = "bPublicKeyOut">No documentation.</param>
        /// <param name = "cbPublicKeyRef">No documentation.</param>
        /// <param name = "ulHashAlgIdRef">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "cchName">No documentation.</param>
        /// <param name = "chNameRef">No documentation.</param>
        /// <param name = "metaDataRef">No documentation.</param>
        /// <param name = "dwAssemblyFlagsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataAssemblyImport::GetAssemblyProps([In] unsigned int mda,[In] const void** ppbPublicKey,[In] unsigned int* pcbPublicKey,[In] unsigned int* pulHashAlgId,[Out, Buffer, Optional] wchar_t* szName,[In] unsigned int cchName,[In] unsigned int* pchName,[In] ASSEMBLYMETADATA* pMetaData,[In] unsigned int* pdwAssemblyFlags)</unmanaged>
        /// <unmanaged-short>IMetaDataAssemblyImport::GetAssemblyProps</unmanaged-short>
        public unsafe void GetAssemblyProps(System.UInt32 mda, System.IntPtr bPublicKeyOut, System.UInt32 cbPublicKeyRef, System.UInt32 ulHashAlgIdRef, System.IntPtr szName, System.UInt32 cchName, System.UInt32 chNameRef, ref CorApi.Portable.Assemblymetadata metaDataRef, System.UInt32 dwAssemblyFlagsRef)
        {
            var metaDataRef_ = new CorApi.Portable.Assemblymetadata.__Native();
            metaDataRef.__MarshalTo(ref metaDataRef_);
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, mda, (void *)((void *)bPublicKeyOut), (void *)(&cbPublicKeyRef), (void *)(&ulHashAlgIdRef), (void *)((void *)szName), cchName, (void *)(&chNameRef), (void *)(&metaDataRef_), (void *)(&dwAssemblyFlagsRef), (*(void ***)this._nativePointer)[3]);
            metaDataRef.__MarshalFree(ref metaDataRef_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "mdar">No documentation.</param>
        /// <param name = "bPublicKeyOrTokenOut">No documentation.</param>
        /// <param name = "cbPublicKeyOrTokenRef">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "cchName">No documentation.</param>
        /// <param name = "chNameRef">No documentation.</param>
        /// <param name = "metaDataRef">No documentation.</param>
        /// <param name = "bHashValueOut">No documentation.</param>
        /// <param name = "cbHashValueRef">No documentation.</param>
        /// <param name = "dwAssemblyRefFlagsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataAssemblyImport::GetAssemblyRefProps([In] unsigned int mdar,[In] const void** ppbPublicKeyOrToken,[In] unsigned int* pcbPublicKeyOrToken,[Out, Buffer, Optional] wchar_t* szName,[In] unsigned int cchName,[In] unsigned int* pchName,[In] ASSEMBLYMETADATA* pMetaData,[In] const void** ppbHashValue,[In] unsigned int* pcbHashValue,[In] unsigned int* pdwAssemblyRefFlags)</unmanaged>
        /// <unmanaged-short>IMetaDataAssemblyImport::GetAssemblyRefProps</unmanaged-short>
        public unsafe void GetAssemblyRefProps(System.UInt32 mdar, System.IntPtr bPublicKeyOrTokenOut, System.UInt32 cbPublicKeyOrTokenRef, System.IntPtr szName, System.UInt32 cchName, System.UInt32 chNameRef, ref CorApi.Portable.Assemblymetadata metaDataRef, System.IntPtr bHashValueOut, System.UInt32 cbHashValueRef, System.UInt32 dwAssemblyRefFlagsRef)
        {
            var metaDataRef_ = new CorApi.Portable.Assemblymetadata.__Native();
            metaDataRef.__MarshalTo(ref metaDataRef_);
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, mdar, (void *)((void *)bPublicKeyOrTokenOut), (void *)(&cbPublicKeyOrTokenRef), (void *)((void *)szName), cchName, (void *)(&chNameRef), (void *)(&metaDataRef_), (void *)((void *)bHashValueOut), (void *)(&cbHashValueRef), (void *)(&dwAssemblyRefFlagsRef), (*(void ***)this._nativePointer)[4]);
            metaDataRef.__MarshalFree(ref metaDataRef_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "mdf">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "cchName">No documentation.</param>
        /// <param name = "chNameRef">No documentation.</param>
        /// <param name = "bHashValueOut">No documentation.</param>
        /// <param name = "cbHashValueRef">No documentation.</param>
        /// <param name = "dwFileFlagsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataAssemblyImport::GetFileProps([In] unsigned int mdf,[Out, Buffer, Optional] wchar_t* szName,[In] unsigned int cchName,[In] unsigned int* pchName,[In] const void** ppbHashValue,[In] unsigned int* pcbHashValue,[In] unsigned int* pdwFileFlags)</unmanaged>
        /// <unmanaged-short>IMetaDataAssemblyImport::GetFileProps</unmanaged-short>
        public unsafe void GetFileProps(System.UInt32 mdf, System.IntPtr szName, System.UInt32 cchName, System.UInt32 chNameRef, System.IntPtr bHashValueOut, System.UInt32 cbHashValueRef, System.UInt32 dwFileFlagsRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, mdf, (void *)((void *)szName), cchName, (void *)(&chNameRef), (void *)((void *)bHashValueOut), (void *)(&cbHashValueRef), (void *)(&dwFileFlagsRef), (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "mdct">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "cchName">No documentation.</param>
        /// <param name = "chNameRef">No documentation.</param>
        /// <param name = "tkImplementationRef">No documentation.</param>
        /// <param name = "tkTypeDefRef">No documentation.</param>
        /// <param name = "dwExportedTypeFlagsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataAssemblyImport::GetExportedTypeProps([In] unsigned int mdct,[Out, Buffer, Optional] wchar_t* szName,[In] unsigned int cchName,[In] unsigned int* pchName,[In] unsigned int* ptkImplementation,[In] unsigned int* ptkTypeDef,[In] unsigned int* pdwExportedTypeFlags)</unmanaged>
        /// <unmanaged-short>IMetaDataAssemblyImport::GetExportedTypeProps</unmanaged-short>
        public unsafe void GetExportedTypeProps(System.UInt32 mdct, System.IntPtr szName, System.UInt32 cchName, System.UInt32 chNameRef, System.UInt32 tkImplementationRef, System.UInt32 tkTypeDefRef, System.UInt32 dwExportedTypeFlagsRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, mdct, (void *)((void *)szName), cchName, (void *)(&chNameRef), (void *)(&tkImplementationRef), (void *)(&tkTypeDefRef), (void *)(&dwExportedTypeFlagsRef), (*(void ***)this._nativePointer)[6]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "mdmr">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "cchName">No documentation.</param>
        /// <param name = "chNameRef">No documentation.</param>
        /// <param name = "tkImplementationRef">No documentation.</param>
        /// <param name = "dwOffsetRef">No documentation.</param>
        /// <param name = "dwResourceFlagsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataAssemblyImport::GetManifestResourceProps([In] unsigned int mdmr,[Out, Buffer, Optional] wchar_t* szName,[In] unsigned int cchName,[In] unsigned int* pchName,[In] unsigned int* ptkImplementation,[In] unsigned int* pdwOffset,[In] unsigned int* pdwResourceFlags)</unmanaged>
        /// <unmanaged-short>IMetaDataAssemblyImport::GetManifestResourceProps</unmanaged-short>
        public unsafe void GetManifestResourceProps(System.UInt32 mdmr, System.IntPtr szName, System.UInt32 cchName, System.UInt32 chNameRef, System.UInt32 tkImplementationRef, System.UInt32 dwOffsetRef, System.UInt32 dwResourceFlagsRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, mdmr, (void *)((void *)szName), cchName, (void *)(&chNameRef), (void *)(&tkImplementationRef), (void *)(&dwOffsetRef), (void *)(&dwResourceFlagsRef), (*(void ***)this._nativePointer)[7]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hEnumRef">No documentation.</param>
        /// <param name = "rAssemblyRefs">No documentation.</param>
        /// <param name = "cMax">No documentation.</param>
        /// <param name = "cTokensRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataAssemblyImport::EnumAssemblyRefs([In] void** phEnum,[In] unsigned int* rAssemblyRefs,[In] unsigned int cMax,[In] unsigned int* pcTokens)</unmanaged>
        /// <unmanaged-short>IMetaDataAssemblyImport::EnumAssemblyRefs</unmanaged-short>
        public unsafe void EnumAssemblyRefs(System.IntPtr hEnumRef, System.UInt32 rAssemblyRefs, System.UInt32 cMax, System.UInt32 cTokensRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)hEnumRef), (void *)(&rAssemblyRefs), cMax, (void *)(&cTokensRef), (*(void ***)this._nativePointer)[8]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hEnumRef">No documentation.</param>
        /// <param name = "rFiles">No documentation.</param>
        /// <param name = "cMax">No documentation.</param>
        /// <param name = "cTokensRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataAssemblyImport::EnumFiles([In] void** phEnum,[In] unsigned int* rFiles,[In] unsigned int cMax,[In] unsigned int* pcTokens)</unmanaged>
        /// <unmanaged-short>IMetaDataAssemblyImport::EnumFiles</unmanaged-short>
        public unsafe void EnumFiles(System.IntPtr hEnumRef, System.UInt32 rFiles, System.UInt32 cMax, System.UInt32 cTokensRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)hEnumRef), (void *)(&rFiles), cMax, (void *)(&cTokensRef), (*(void ***)this._nativePointer)[9]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hEnumRef">No documentation.</param>
        /// <param name = "rExportedTypes">No documentation.</param>
        /// <param name = "cMax">No documentation.</param>
        /// <param name = "cTokensRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataAssemblyImport::EnumExportedTypes([In] void** phEnum,[In] unsigned int* rExportedTypes,[In] unsigned int cMax,[In] unsigned int* pcTokens)</unmanaged>
        /// <unmanaged-short>IMetaDataAssemblyImport::EnumExportedTypes</unmanaged-short>
        public unsafe void EnumExportedTypes(System.IntPtr hEnumRef, System.UInt32 rExportedTypes, System.UInt32 cMax, System.UInt32 cTokensRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)hEnumRef), (void *)(&rExportedTypes), cMax, (void *)(&cTokensRef), (*(void ***)this._nativePointer)[10]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hEnumRef">No documentation.</param>
        /// <param name = "rManifestResources">No documentation.</param>
        /// <param name = "cMax">No documentation.</param>
        /// <param name = "cTokensRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataAssemblyImport::EnumManifestResources([In] void** phEnum,[In] unsigned int* rManifestResources,[In] unsigned int cMax,[In] unsigned int* pcTokens)</unmanaged>
        /// <unmanaged-short>IMetaDataAssemblyImport::EnumManifestResources</unmanaged-short>
        public unsafe void EnumManifestResources(System.IntPtr hEnumRef, System.UInt32 rManifestResources, System.UInt32 cMax, System.UInt32 cTokensRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)hEnumRef), (void *)(&rManifestResources), cMax, (void *)(&cTokensRef), (*(void ***)this._nativePointer)[11]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tkAssemblyRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataAssemblyImport::GetAssemblyFromScope([In] unsigned int* ptkAssembly)</unmanaged>
        /// <unmanaged-short>IMetaDataAssemblyImport::GetAssemblyFromScope</unmanaged-short>
        public unsafe void GetAssemblyFromScope(System.UInt32 tkAssemblyRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&tkAssemblyRef), (*(void ***)this._nativePointer)[12]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "mdtExportedType">No documentation.</param>
        /// <param name = "tkExportedTypeRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataAssemblyImport::FindExportedTypeByName([In] const wchar_t* szName,[In] unsigned int mdtExportedType,[In] unsigned int* ptkExportedType)</unmanaged>
        /// <unmanaged-short>IMetaDataAssemblyImport::FindExportedTypeByName</unmanaged-short>
        public unsafe void FindExportedTypeByName(System.String szName, System.UInt32 mdtExportedType, System.UInt32 tkExportedTypeRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)szName_), mdtExportedType, (void *)(&tkExportedTypeRef), (*(void ***)this._nativePointer)[13]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "tkManifestResourceRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataAssemblyImport::FindManifestResourceByName([In] const wchar_t* szName,[In] unsigned int* ptkManifestResource)</unmanaged>
        /// <unmanaged-short>IMetaDataAssemblyImport::FindManifestResourceByName</unmanaged-short>
        public unsafe void FindManifestResourceByName(System.String szName, System.UInt32 tkManifestResourceRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)szName_), (void *)(&tkManifestResourceRef), (*(void ***)this._nativePointer)[14]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hEnum">No documentation.</param>
        /// <unmanaged>void IMetaDataAssemblyImport::CloseEnum([In] void* hEnum)</unmanaged>
        /// <unmanaged-short>IMetaDataAssemblyImport::CloseEnum</unmanaged-short>
        public unsafe void CloseEnum(System.IntPtr hEnum)
        {
            CorDebug.LocalInterop.Callivoid(this._nativePointer, (void *)((void *)hEnum), (*(void ***)this._nativePointer)[15]);
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "szAppBase">No documentation.</param>
        /// <param name = "szPrivateBin">No documentation.</param>
        /// <param name = "szAssemblyName">No documentation.</param>
        /// <param name = "iUnkOut">No documentation.</param>
        /// <param name = "cMax">No documentation.</param>
        /// <param name = "cAssembliesRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataAssemblyImport::FindAssembliesByName([In] const wchar_t* szAppBase,[In] const wchar_t* szPrivateBin,[In] const wchar_t* szAssemblyName,[In] IUnknown** ppIUnk,[In] unsigned int cMax,[In] unsigned int* pcAssemblies)</unmanaged>
        /// <unmanaged-short>IMetaDataAssemblyImport::FindAssembliesByName</unmanaged-short>
        public unsafe void FindAssembliesByName(System.String szAppBase, System.String szPrivateBin, System.String szAssemblyName, out SharpGen.Runtime.IUnknown iUnkOut, System.UInt32 cMax, System.UInt32 cAssembliesRef)
        {
            System.IntPtr iUnkOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            fixed (char *szAssemblyName_ = szAssemblyName)
                fixed (char *szPrivateBin_ = szPrivateBin)
                    fixed (char *szAppBase_ = szAppBase)
                        __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)szAppBase_), (void *)((void *)szPrivateBin_), (void *)((void *)szAssemblyName_), (void *)(&iUnkOut_), cMax, (void *)(&cAssembliesRef), (*(void ***)this._nativePointer)[16]);
            iUnkOut = (iUnkOut_ == System.IntPtr.Zero) ? null : new SharpGen.Runtime.ComObject(iUnkOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("809c652e-7396-11d2-9771-00a0c9b4d50c")]
    public partial class IMetaDataDispenser : SharpGen.Runtime.ComObject
    {
        public IMetaDataDispenser(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator IMetaDataDispenser(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new IMetaDataDispenser(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "rclsid">No documentation.</param>
        /// <param name = "dwCreateFlags">No documentation.</param>
        /// <param name = "riid">No documentation.</param>
        /// <param name = "iUnkOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataDispenser::DefineScope([In] const GUID&amp; rclsid,[In] unsigned int dwCreateFlags,[In] const GUID&amp; riid,[In] IUnknown** ppIUnk)</unmanaged>
        /// <unmanaged-short>IMetaDataDispenser::DefineScope</unmanaged-short>
        public unsafe void DefineScope(System.Guid rclsid, System.UInt32 dwCreateFlags, System.Guid riid, out SharpGen.Runtime.IUnknown iUnkOut)
        {
            System.IntPtr iUnkOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&rclsid), dwCreateFlags, (void *)(&riid), (void *)(&iUnkOut_), (*(void ***)this._nativePointer)[3]);
            iUnkOut = (iUnkOut_ == System.IntPtr.Zero) ? null : new SharpGen.Runtime.ComObject(iUnkOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "szScope">No documentation.</param>
        /// <param name = "dwOpenFlags">No documentation.</param>
        /// <param name = "riid">No documentation.</param>
        /// <param name = "iUnkOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataDispenser::OpenScope([In] const wchar_t* szScope,[In] unsigned int dwOpenFlags,[In] const GUID&amp; riid,[In] IUnknown** ppIUnk)</unmanaged>
        /// <unmanaged-short>IMetaDataDispenser::OpenScope</unmanaged-short>
        public unsafe void OpenScope(System.String szScope, System.UInt32 dwOpenFlags, System.Guid riid, out SharpGen.Runtime.IUnknown iUnkOut)
        {
            System.IntPtr iUnkOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            fixed (char *szScope_ = szScope)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)szScope_), dwOpenFlags, (void *)(&riid), (void *)(&iUnkOut_), (*(void ***)this._nativePointer)[4]);
            iUnkOut = (iUnkOut_ == System.IntPtr.Zero) ? null : new SharpGen.Runtime.ComObject(iUnkOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "dataRef">No documentation.</param>
        /// <param name = "cbData">No documentation.</param>
        /// <param name = "dwOpenFlags">No documentation.</param>
        /// <param name = "riid">No documentation.</param>
        /// <param name = "iUnkOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataDispenser::OpenScopeOnMemory([In] const void* pData,[In] unsigned int cbData,[In] unsigned int dwOpenFlags,[In] const GUID&amp; riid,[In] IUnknown** ppIUnk)</unmanaged>
        /// <unmanaged-short>IMetaDataDispenser::OpenScopeOnMemory</unmanaged-short>
        public unsafe void OpenScopeOnMemory(System.IntPtr dataRef, System.UInt32 cbData, System.UInt32 dwOpenFlags, System.Guid riid, out SharpGen.Runtime.IUnknown iUnkOut)
        {
            System.IntPtr iUnkOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)dataRef), cbData, dwOpenFlags, (void *)(&riid), (void *)(&iUnkOut_), (*(void ***)this._nativePointer)[5]);
            iUnkOut = (iUnkOut_ == System.IntPtr.Zero) ? null : new SharpGen.Runtime.ComObject(iUnkOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("31bcfce2-dafb-11d2-9f81-00c04f79a0a3")]
    public partial class IMetaDataDispenserEx : CorApi.Portable.IMetaDataDispenser
    {
        public IMetaDataDispenserEx(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator IMetaDataDispenserEx(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new IMetaDataDispenserEx(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "optionid">No documentation.</param>
        /// <param name = "value">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataDispenserEx::SetOption([In] const GUID&amp; optionid,[In] const VARIANT* value)</unmanaged>
        /// <unmanaged-short>IMetaDataDispenserEx::SetOption</unmanaged-short>
        public unsafe void SetOption(System.Guid optionid, SharpGen.Runtime.Win32.Variant value)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&optionid), (void *)(&value), (*(void ***)this._nativePointer)[6]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "optionid">No documentation.</param>
        /// <param name = "valueRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataDispenserEx::GetOption([In] const GUID&amp; optionid,[In] VARIANT* pvalue)</unmanaged>
        /// <unmanaged-short>IMetaDataDispenserEx::GetOption</unmanaged-short>
        public unsafe void GetOption(System.Guid optionid, SharpGen.Runtime.Win32.Variant valueRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&optionid), (void *)(&valueRef), (*(void ***)this._nativePointer)[7]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "iTIRef">No documentation.</param>
        /// <param name = "dwOpenFlags">No documentation.</param>
        /// <param name = "riid">No documentation.</param>
        /// <param name = "iUnkOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataDispenserEx::OpenScopeOnITypeInfo([In] ITypeInfo* pITI,[In] unsigned int dwOpenFlags,[In] const GUID&amp; riid,[In] IUnknown** ppIUnk)</unmanaged>
        /// <unmanaged-short>IMetaDataDispenserEx::OpenScopeOnITypeInfo</unmanaged-short>
        public unsafe void OpenScopeOnITypeInfo(SharpGen.Runtime.ComObject iTIRef, System.UInt32 dwOpenFlags, System.Guid riid, out SharpGen.Runtime.IUnknown iUnkOut)
        {
            System.IntPtr iUnkOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<SharpGen.Runtime.ComObject>(iTIRef))), dwOpenFlags, (void *)(&riid), (void *)(&iUnkOut_), (*(void ***)this._nativePointer)[8]);
            iUnkOut = (iUnkOut_ == System.IntPtr.Zero) ? null : new SharpGen.Runtime.ComObject(iUnkOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "szBuffer">No documentation.</param>
        /// <param name = "cchBuffer">No documentation.</param>
        /// <param name = "chBufferRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataDispenserEx::GetCORSystemDirectory([Out, Buffer, Optional] wchar_t* szBuffer,[In] unsigned int cchBuffer,[In] unsigned int* pchBuffer)</unmanaged>
        /// <unmanaged-short>IMetaDataDispenserEx::GetCORSystemDirectory</unmanaged-short>
        public unsafe void GetCORSystemDirectory(System.IntPtr szBuffer, System.UInt32 cchBuffer, System.UInt32 chBufferRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)szBuffer), cchBuffer, (void *)(&chBufferRef), (*(void ***)this._nativePointer)[9]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "szAppBase">No documentation.</param>
        /// <param name = "szPrivateBin">No documentation.</param>
        /// <param name = "szGlobalBin">No documentation.</param>
        /// <param name = "szAssemblyName">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "cchName">No documentation.</param>
        /// <param name = "cNameRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataDispenserEx::FindAssembly([In] const wchar_t* szAppBase,[In] const wchar_t* szPrivateBin,[In] const wchar_t* szGlobalBin,[In] const wchar_t* szAssemblyName,[In] const wchar_t* szName,[In] unsigned int cchName,[In] unsigned int* pcName)</unmanaged>
        /// <unmanaged-short>IMetaDataDispenserEx::FindAssembly</unmanaged-short>
        public unsafe void FindAssembly(System.String szAppBase, System.String szPrivateBin, System.String szGlobalBin, System.String szAssemblyName, System.String szName, System.UInt32 cchName, System.UInt32 cNameRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                fixed (char *szAssemblyName_ = szAssemblyName)
                    fixed (char *szGlobalBin_ = szGlobalBin)
                        fixed (char *szPrivateBin_ = szPrivateBin)
                            fixed (char *szAppBase_ = szAppBase)
                                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)szAppBase_), (void *)((void *)szPrivateBin_), (void *)((void *)szGlobalBin_), (void *)((void *)szAssemblyName_), (void *)((void *)szName_), cchName, (void *)(&cNameRef), (*(void ***)this._nativePointer)[10]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "szAppBase">No documentation.</param>
        /// <param name = "szPrivateBin">No documentation.</param>
        /// <param name = "szGlobalBin">No documentation.</param>
        /// <param name = "szAssemblyName">No documentation.</param>
        /// <param name = "szModuleName">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "cchName">No documentation.</param>
        /// <param name = "cNameRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataDispenserEx::FindAssemblyModule([In] const wchar_t* szAppBase,[In] const wchar_t* szPrivateBin,[In] const wchar_t* szGlobalBin,[In] const wchar_t* szAssemblyName,[In] const wchar_t* szModuleName,[Out, Buffer, Optional] wchar_t* szName,[In] unsigned int cchName,[In] unsigned int* pcName)</unmanaged>
        /// <unmanaged-short>IMetaDataDispenserEx::FindAssemblyModule</unmanaged-short>
        public unsafe void FindAssemblyModule(System.String szAppBase, System.String szPrivateBin, System.String szGlobalBin, System.String szAssemblyName, System.String szModuleName, System.IntPtr szName, System.UInt32 cchName, System.UInt32 cNameRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szModuleName_ = szModuleName)
                fixed (char *szAssemblyName_ = szAssemblyName)
                    fixed (char *szGlobalBin_ = szGlobalBin)
                        fixed (char *szPrivateBin_ = szPrivateBin)
                            fixed (char *szAppBase_ = szAppBase)
                                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)szAppBase_), (void *)((void *)szPrivateBin_), (void *)((void *)szGlobalBin_), (void *)((void *)szAssemblyName_), (void *)((void *)szModuleName_), (void *)((void *)szName), cchName, (void *)(&cNameRef), (*(void ***)this._nativePointer)[11]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("ba3fee4c-ecb9-4e41-83b7-183fa41cd859")]
    public partial class IMetaDataEmit : SharpGen.Runtime.ComObject
    {
        public IMetaDataEmit(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator IMetaDataEmit(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new IMetaDataEmit(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "szName">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::SetModuleProps([In] const wchar_t* szName)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::SetModuleProps</unmanaged-short>
        public unsafe void SetModuleProps(System.String szName)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)szName_), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "szFile">No documentation.</param>
        /// <param name = "dwSaveFlags">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::Save([In] const wchar_t* szFile,[In] unsigned int dwSaveFlags)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::Save</unmanaged-short>
        public unsafe void Save(System.String szFile, System.UInt32 dwSaveFlags)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szFile_ = szFile)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)szFile_), dwSaveFlags, (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "iStreamRef">No documentation.</param>
        /// <param name = "dwSaveFlags">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::SaveToStream([In] IStream* pIStream,[In] unsigned int dwSaveFlags)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::SaveToStream</unmanaged-short>
        public unsafe void SaveToStream(SharpGen.Runtime.Win32.IStream iStreamRef, System.UInt32 dwSaveFlags)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<SharpGen.Runtime.Win32.IStream>(iStreamRef))), dwSaveFlags, (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "fSave">No documentation.</param>
        /// <param name = "dwSaveSizeRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::GetSaveSize([In] CorSaveSize fSave,[In] unsigned int* pdwSaveSize)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::GetSaveSize</unmanaged-short>
        public unsafe void GetSaveSize(CorApi.Portable.CorSaveSize fSave, System.UInt32 dwSaveSizeRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, unchecked ((System.Int32)fSave), (void *)(&dwSaveSizeRef), (*(void ***)this._nativePointer)[6]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "szTypeDef">No documentation.</param>
        /// <param name = "dwTypeDefFlags">No documentation.</param>
        /// <param name = "tkExtends">No documentation.</param>
        /// <param name = "rtkImplements">No documentation.</param>
        /// <param name = "tdRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::DefineTypeDef([In] const wchar_t* szTypeDef,[In] unsigned int dwTypeDefFlags,[In] unsigned int tkExtends,[In] unsigned int* rtkImplements,[In] unsigned int* ptd)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::DefineTypeDef</unmanaged-short>
        public unsafe void DefineTypeDef(System.String szTypeDef, System.UInt32 dwTypeDefFlags, System.UInt32 tkExtends, System.UInt32 rtkImplements, System.UInt32 tdRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szTypeDef_ = szTypeDef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)szTypeDef_), dwTypeDefFlags, tkExtends, (void *)(&rtkImplements), (void *)(&tdRef), (*(void ***)this._nativePointer)[7]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "szTypeDef">No documentation.</param>
        /// <param name = "dwTypeDefFlags">No documentation.</param>
        /// <param name = "tkExtends">No documentation.</param>
        /// <param name = "rtkImplements">No documentation.</param>
        /// <param name = "tdEncloser">No documentation.</param>
        /// <param name = "tdRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::DefineNestedType([In] const wchar_t* szTypeDef,[In] unsigned int dwTypeDefFlags,[In] unsigned int tkExtends,[In] unsigned int* rtkImplements,[In] unsigned int tdEncloser,[In] unsigned int* ptd)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::DefineNestedType</unmanaged-short>
        public unsafe void DefineNestedType(System.String szTypeDef, System.UInt32 dwTypeDefFlags, System.UInt32 tkExtends, System.UInt32 rtkImplements, System.UInt32 tdEncloser, System.UInt32 tdRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szTypeDef_ = szTypeDef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)szTypeDef_), dwTypeDefFlags, tkExtends, (void *)(&rtkImplements), tdEncloser, (void *)(&tdRef), (*(void ***)this._nativePointer)[8]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "unkRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::SetHandler([In] IUnknown* pUnk)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::SetHandler</unmanaged-short>
        public unsafe void SetHandler(SharpGen.Runtime.IUnknown unkRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<SharpGen.Runtime.IUnknown>(unkRef))), (*(void ***)this._nativePointer)[9]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "td">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "dwMethodFlags">No documentation.</param>
        /// <param name = "vSigBlobRef">No documentation.</param>
        /// <param name = "cbSigBlob">No documentation.</param>
        /// <param name = "ulCodeRVA">No documentation.</param>
        /// <param name = "dwImplFlags">No documentation.</param>
        /// <param name = "mdRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::DefineMethod([In] unsigned int td,[In] const wchar_t* szName,[In] unsigned int dwMethodFlags,[In] const unsigned char* pvSigBlob,[In] unsigned int cbSigBlob,[In] unsigned int ulCodeRVA,[In] unsigned int dwImplFlags,[In] unsigned int* pmd)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::DefineMethod</unmanaged-short>
        public unsafe void DefineMethod(System.UInt32 td, System.String szName, System.UInt32 dwMethodFlags, System.Byte vSigBlobRef, System.UInt32 cbSigBlob, System.UInt32 ulCodeRVA, System.UInt32 dwImplFlags, System.UInt32 mdRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, td, (void *)((void *)szName_), dwMethodFlags, (void *)(&vSigBlobRef), cbSigBlob, ulCodeRVA, dwImplFlags, (void *)(&mdRef), (*(void ***)this._nativePointer)[10]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "td">No documentation.</param>
        /// <param name = "tkBody">No documentation.</param>
        /// <param name = "tkDecl">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::DefineMethodImpl([In] unsigned int td,[In] unsigned int tkBody,[In] unsigned int tkDecl)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::DefineMethodImpl</unmanaged-short>
        public unsafe void DefineMethodImpl(System.UInt32 td, System.UInt32 tkBody, System.UInt32 tkDecl)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, td, tkBody, tkDecl, (*(void ***)this._nativePointer)[11]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tkResolutionScope">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "trRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::DefineTypeRefByName([In] unsigned int tkResolutionScope,[In] const wchar_t* szName,[In] unsigned int* ptr)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::DefineTypeRefByName</unmanaged-short>
        public unsafe void DefineTypeRefByName(System.UInt32 tkResolutionScope, System.String szName, System.UInt32 trRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, tkResolutionScope, (void *)((void *)szName_), (void *)(&trRef), (*(void ***)this._nativePointer)[12]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "assemImportRef">No documentation.</param>
        /// <param name = "bHashValueRef">No documentation.</param>
        /// <param name = "cbHashValue">No documentation.</param>
        /// <param name = "importRef">No documentation.</param>
        /// <param name = "tdImport">No documentation.</param>
        /// <param name = "assemEmitRef">No documentation.</param>
        /// <param name = "trRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::DefineImportType([In] IMetaDataAssemblyImport* pAssemImport,[In] const void* pbHashValue,[In] unsigned int cbHashValue,[In] IMetaDataImport* pImport,[In] unsigned int tdImport,[In] IMetaDataAssemblyEmit* pAssemEmit,[In] unsigned int* ptr)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::DefineImportType</unmanaged-short>
        public unsafe void DefineImportType(CorApi.Portable.IMetaDataAssemblyImport assemImportRef, System.IntPtr bHashValueRef, System.UInt32 cbHashValue, CorApi.Portable.IMetaDataImport importRef, System.UInt32 tdImport, CorApi.Portable.IMetaDataAssemblyEmit assemEmitRef, System.UInt32 trRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.IMetaDataAssemblyImport>(assemImportRef))), (void *)((void *)bHashValueRef), cbHashValue, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.IMetaDataImport>(importRef))), tdImport, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.IMetaDataAssemblyEmit>(assemEmitRef))), (void *)(&trRef), (*(void ***)this._nativePointer)[13]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tkImport">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "vSigBlobRef">No documentation.</param>
        /// <param name = "cbSigBlob">No documentation.</param>
        /// <param name = "mrRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::DefineMemberRef([In] unsigned int tkImport,[In] const wchar_t* szName,[In] const unsigned char* pvSigBlob,[In] unsigned int cbSigBlob,[In] unsigned int* pmr)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::DefineMemberRef</unmanaged-short>
        public unsafe void DefineMemberRef(System.UInt32 tkImport, System.String szName, System.Byte vSigBlobRef, System.UInt32 cbSigBlob, System.UInt32 mrRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, tkImport, (void *)((void *)szName_), (void *)(&vSigBlobRef), cbSigBlob, (void *)(&mrRef), (*(void ***)this._nativePointer)[14]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "assemImportRef">No documentation.</param>
        /// <param name = "bHashValueRef">No documentation.</param>
        /// <param name = "cbHashValue">No documentation.</param>
        /// <param name = "importRef">No documentation.</param>
        /// <param name = "mbMember">No documentation.</param>
        /// <param name = "assemEmitRef">No documentation.</param>
        /// <param name = "tkParent">No documentation.</param>
        /// <param name = "mrRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::DefineImportMember([In] IMetaDataAssemblyImport* pAssemImport,[In] const void* pbHashValue,[In] unsigned int cbHashValue,[In] IMetaDataImport* pImport,[In] unsigned int mbMember,[In] IMetaDataAssemblyEmit* pAssemEmit,[In] unsigned int tkParent,[In] unsigned int* pmr)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::DefineImportMember</unmanaged-short>
        public unsafe void DefineImportMember(CorApi.Portable.IMetaDataAssemblyImport assemImportRef, System.IntPtr bHashValueRef, System.UInt32 cbHashValue, CorApi.Portable.IMetaDataImport importRef, System.UInt32 mbMember, CorApi.Portable.IMetaDataAssemblyEmit assemEmitRef, System.UInt32 tkParent, System.UInt32 mrRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.IMetaDataAssemblyImport>(assemImportRef))), (void *)((void *)bHashValueRef), cbHashValue, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.IMetaDataImport>(importRef))), mbMember, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.IMetaDataAssemblyEmit>(assemEmitRef))), tkParent, (void *)(&mrRef), (*(void ***)this._nativePointer)[15]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "td">No documentation.</param>
        /// <param name = "szEvent">No documentation.</param>
        /// <param name = "dwEventFlags">No documentation.</param>
        /// <param name = "tkEventType">No documentation.</param>
        /// <param name = "mdAddOn">No documentation.</param>
        /// <param name = "mdRemoveOn">No documentation.</param>
        /// <param name = "mdFire">No documentation.</param>
        /// <param name = "rmdOtherMethods">No documentation.</param>
        /// <param name = "mdEventRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::DefineEvent([In] unsigned int td,[In] const wchar_t* szEvent,[In] unsigned int dwEventFlags,[In] unsigned int tkEventType,[In] unsigned int mdAddOn,[In] unsigned int mdRemoveOn,[In] unsigned int mdFire,[In] unsigned int* rmdOtherMethods,[In] unsigned int* pmdEvent)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::DefineEvent</unmanaged-short>
        public unsafe void DefineEvent(System.UInt32 td, System.String szEvent, System.UInt32 dwEventFlags, System.UInt32 tkEventType, System.UInt32 mdAddOn, System.UInt32 mdRemoveOn, System.UInt32 mdFire, System.UInt32 rmdOtherMethods, System.UInt32 mdEventRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szEvent_ = szEvent)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, td, (void *)((void *)szEvent_), dwEventFlags, tkEventType, mdAddOn, mdRemoveOn, mdFire, (void *)(&rmdOtherMethods), (void *)(&mdEventRef), (*(void ***)this._nativePointer)[16]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "td">No documentation.</param>
        /// <param name = "dwPackSize">No documentation.</param>
        /// <param name = "rFieldOffsets">No documentation.</param>
        /// <param name = "ulClassSize">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::SetClassLayout([In] unsigned int td,[In] unsigned int dwPackSize,[In] COR_FIELD_OFFSET* rFieldOffsets,[In] unsigned int ulClassSize)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::SetClassLayout</unmanaged-short>
        public unsafe void SetClassLayout(System.UInt32 td, System.UInt32 dwPackSize, CorApi.Portable.CorFieldOffset rFieldOffsets, System.UInt32 ulClassSize)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, td, dwPackSize, (void *)(&rFieldOffsets), ulClassSize, (*(void ***)this._nativePointer)[17]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "td">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::DeleteClassLayout([In] unsigned int td)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::DeleteClassLayout</unmanaged-short>
        public unsafe void DeleteClassLayout(System.UInt32 td)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, td, (*(void ***)this._nativePointer)[18]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tk">No documentation.</param>
        /// <param name = "vNativeTypeRef">No documentation.</param>
        /// <param name = "cbNativeType">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::SetFieldMarshal([In] unsigned int tk,[In] const unsigned char* pvNativeType,[In] unsigned int cbNativeType)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::SetFieldMarshal</unmanaged-short>
        public unsafe void SetFieldMarshal(System.UInt32 tk, System.Byte vNativeTypeRef, System.UInt32 cbNativeType)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, tk, (void *)(&vNativeTypeRef), cbNativeType, (*(void ***)this._nativePointer)[19]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tk">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::DeleteFieldMarshal([In] unsigned int tk)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::DeleteFieldMarshal</unmanaged-short>
        public unsafe void DeleteFieldMarshal(System.UInt32 tk)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, tk, (*(void ***)this._nativePointer)[20]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tk">No documentation.</param>
        /// <param name = "dwAction">No documentation.</param>
        /// <param name = "vPermissionRef">No documentation.</param>
        /// <param name = "cbPermission">No documentation.</param>
        /// <param name = "mOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::DefinePermissionSet([In] unsigned int tk,[In] unsigned int dwAction,[In] const void* pvPermission,[In] unsigned int cbPermission,[In] unsigned int* ppm)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::DefinePermissionSet</unmanaged-short>
        public unsafe void DefinePermissionSet(System.UInt32 tk, System.UInt32 dwAction, System.IntPtr vPermissionRef, System.UInt32 cbPermission, System.UInt32 mOut)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, tk, dwAction, (void *)((void *)vPermissionRef), cbPermission, (void *)(&mOut), (*(void ***)this._nativePointer)[21]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "md">No documentation.</param>
        /// <param name = "ulRVA">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::SetRVA([In] unsigned int md,[In] unsigned int ulRVA)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::SetRVA</unmanaged-short>
        public unsafe void SetRVA(System.UInt32 md, System.UInt32 ulRVA)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, md, ulRVA, (*(void ***)this._nativePointer)[22]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "vSigRef">No documentation.</param>
        /// <param name = "cbSig">No documentation.</param>
        /// <param name = "msigRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::GetTokenFromSig([In] const unsigned char* pvSig,[In] unsigned int cbSig,[In] unsigned int* pmsig)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::GetTokenFromSig</unmanaged-short>
        public unsafe void GetTokenFromSig(System.Byte vSigRef, System.UInt32 cbSig, System.UInt32 msigRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&vSigRef), cbSig, (void *)(&msigRef), (*(void ***)this._nativePointer)[23]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "murRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::DefineModuleRef([In] const wchar_t* szName,[In] unsigned int* pmur)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::DefineModuleRef</unmanaged-short>
        public unsafe void DefineModuleRef(System.String szName, System.UInt32 murRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)szName_), (void *)(&murRef), (*(void ***)this._nativePointer)[24]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "mr">No documentation.</param>
        /// <param name = "tk">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::SetParent([In] unsigned int mr,[In] unsigned int tk)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::SetParent</unmanaged-short>
        public unsafe void SetParent(System.UInt32 mr, System.UInt32 tk)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, mr, tk, (*(void ***)this._nativePointer)[25]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "vSigRef">No documentation.</param>
        /// <param name = "cbSig">No documentation.</param>
        /// <param name = "typespecRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::GetTokenFromTypeSpec([In] const unsigned char* pvSig,[In] unsigned int cbSig,[In] unsigned int* ptypespec)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::GetTokenFromTypeSpec</unmanaged-short>
        public unsafe void GetTokenFromTypeSpec(System.Byte vSigRef, System.UInt32 cbSig, System.UInt32 typespecRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&vSigRef), cbSig, (void *)(&typespecRef), (*(void ***)this._nativePointer)[26]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "bDataRef">No documentation.</param>
        /// <param name = "cbData">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::SaveToMemory([In] void* pbData,[In] unsigned int cbData)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::SaveToMemory</unmanaged-short>
        public unsafe void SaveToMemory(System.IntPtr bDataRef, System.UInt32 cbData)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)bDataRef), cbData, (*(void ***)this._nativePointer)[27]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "szString">No documentation.</param>
        /// <param name = "cchString">No documentation.</param>
        /// <param name = "stkRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::DefineUserString([In] const wchar_t* szString,[In] unsigned int cchString,[In] unsigned int* pstk)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::DefineUserString</unmanaged-short>
        public unsafe void DefineUserString(System.String szString, System.UInt32 cchString, System.UInt32 stkRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szString_ = szString)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)szString_), cchString, (void *)(&stkRef), (*(void ***)this._nativePointer)[28]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tkObj">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::DeleteToken([In] unsigned int tkObj)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::DeleteToken</unmanaged-short>
        public unsafe void DeleteToken(System.UInt32 tkObj)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, tkObj, (*(void ***)this._nativePointer)[29]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "md">No documentation.</param>
        /// <param name = "dwMethodFlags">No documentation.</param>
        /// <param name = "ulCodeRVA">No documentation.</param>
        /// <param name = "dwImplFlags">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::SetMethodProps([In] unsigned int md,[In] unsigned int dwMethodFlags,[In] unsigned int ulCodeRVA,[In] unsigned int dwImplFlags)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::SetMethodProps</unmanaged-short>
        public unsafe void SetMethodProps(System.UInt32 md, System.UInt32 dwMethodFlags, System.UInt32 ulCodeRVA, System.UInt32 dwImplFlags)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, md, dwMethodFlags, ulCodeRVA, dwImplFlags, (*(void ***)this._nativePointer)[30]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "td">No documentation.</param>
        /// <param name = "dwTypeDefFlags">No documentation.</param>
        /// <param name = "tkExtends">No documentation.</param>
        /// <param name = "rtkImplements">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::SetTypeDefProps([In] unsigned int td,[In] unsigned int dwTypeDefFlags,[In] unsigned int tkExtends,[In] unsigned int* rtkImplements)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::SetTypeDefProps</unmanaged-short>
        public unsafe void SetTypeDefProps(System.UInt32 td, System.UInt32 dwTypeDefFlags, System.UInt32 tkExtends, System.UInt32 rtkImplements)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, td, dwTypeDefFlags, tkExtends, (void *)(&rtkImplements), (*(void ***)this._nativePointer)[31]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "ev">No documentation.</param>
        /// <param name = "dwEventFlags">No documentation.</param>
        /// <param name = "tkEventType">No documentation.</param>
        /// <param name = "mdAddOn">No documentation.</param>
        /// <param name = "mdRemoveOn">No documentation.</param>
        /// <param name = "mdFire">No documentation.</param>
        /// <param name = "rmdOtherMethods">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::SetEventProps([In] unsigned int ev,[In] unsigned int dwEventFlags,[In] unsigned int tkEventType,[In] unsigned int mdAddOn,[In] unsigned int mdRemoveOn,[In] unsigned int mdFire,[In] unsigned int* rmdOtherMethods)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::SetEventProps</unmanaged-short>
        public unsafe void SetEventProps(System.UInt32 ev, System.UInt32 dwEventFlags, System.UInt32 tkEventType, System.UInt32 mdAddOn, System.UInt32 mdRemoveOn, System.UInt32 mdFire, System.UInt32 rmdOtherMethods)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, ev, dwEventFlags, tkEventType, mdAddOn, mdRemoveOn, mdFire, (void *)(&rmdOtherMethods), (*(void ***)this._nativePointer)[32]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tk">No documentation.</param>
        /// <param name = "dwAction">No documentation.</param>
        /// <param name = "vPermissionRef">No documentation.</param>
        /// <param name = "cbPermission">No documentation.</param>
        /// <param name = "mOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::SetPermissionSetProps([In] unsigned int tk,[In] unsigned int dwAction,[In] const void* pvPermission,[In] unsigned int cbPermission,[In] unsigned int* ppm)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::SetPermissionSetProps</unmanaged-short>
        public unsafe void SetPermissionSetProps(System.UInt32 tk, System.UInt32 dwAction, System.IntPtr vPermissionRef, System.UInt32 cbPermission, System.UInt32 mOut)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, tk, dwAction, (void *)((void *)vPermissionRef), cbPermission, (void *)(&mOut), (*(void ***)this._nativePointer)[33]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tk">No documentation.</param>
        /// <param name = "dwMappingFlags">No documentation.</param>
        /// <param name = "szImportName">No documentation.</param>
        /// <param name = "mrImportDLL">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::DefinePinvokeMap([In] unsigned int tk,[In] unsigned int dwMappingFlags,[In] const wchar_t* szImportName,[In] unsigned int mrImportDLL)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::DefinePinvokeMap</unmanaged-short>
        public unsafe void DefinePinvokeMap(System.UInt32 tk, System.UInt32 dwMappingFlags, System.String szImportName, System.UInt32 mrImportDLL)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szImportName_ = szImportName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, tk, dwMappingFlags, (void *)((void *)szImportName_), mrImportDLL, (*(void ***)this._nativePointer)[34]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tk">No documentation.</param>
        /// <param name = "dwMappingFlags">No documentation.</param>
        /// <param name = "szImportName">No documentation.</param>
        /// <param name = "mrImportDLL">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::SetPinvokeMap([In] unsigned int tk,[In] unsigned int dwMappingFlags,[In] const wchar_t* szImportName,[In] unsigned int mrImportDLL)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::SetPinvokeMap</unmanaged-short>
        public unsafe void SetPinvokeMap(System.UInt32 tk, System.UInt32 dwMappingFlags, System.String szImportName, System.UInt32 mrImportDLL)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szImportName_ = szImportName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, tk, dwMappingFlags, (void *)((void *)szImportName_), mrImportDLL, (*(void ***)this._nativePointer)[35]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tk">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::DeletePinvokeMap([In] unsigned int tk)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::DeletePinvokeMap</unmanaged-short>
        public unsafe void DeletePinvokeMap(System.UInt32 tk)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, tk, (*(void ***)this._nativePointer)[36]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tkOwner">No documentation.</param>
        /// <param name = "tkCtor">No documentation.</param>
        /// <param name = "customAttributeRef">No documentation.</param>
        /// <param name = "cbCustomAttribute">No documentation.</param>
        /// <param name = "cvRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::DefineCustomAttribute([In] unsigned int tkOwner,[In] unsigned int tkCtor,[In] const void* pCustomAttribute,[In] unsigned int cbCustomAttribute,[In] unsigned int* pcv)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::DefineCustomAttribute</unmanaged-short>
        public unsafe void DefineCustomAttribute(System.UInt32 tkOwner, System.UInt32 tkCtor, System.IntPtr customAttributeRef, System.UInt32 cbCustomAttribute, System.UInt32 cvRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, tkOwner, tkCtor, (void *)((void *)customAttributeRef), cbCustomAttribute, (void *)(&cvRef), (*(void ***)this._nativePointer)[37]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "pcv">No documentation.</param>
        /// <param name = "customAttributeRef">No documentation.</param>
        /// <param name = "cbCustomAttribute">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::SetCustomAttributeValue([In] unsigned int pcv,[In] const void* pCustomAttribute,[In] unsigned int cbCustomAttribute)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::SetCustomAttributeValue</unmanaged-short>
        public unsafe void SetCustomAttributeValue(System.UInt32 pcv, System.IntPtr customAttributeRef, System.UInt32 cbCustomAttribute)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, pcv, (void *)((void *)customAttributeRef), cbCustomAttribute, (*(void ***)this._nativePointer)[38]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "td">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "dwFieldFlags">No documentation.</param>
        /// <param name = "vSigBlobRef">No documentation.</param>
        /// <param name = "cbSigBlob">No documentation.</param>
        /// <param name = "dwCPlusTypeFlag">No documentation.</param>
        /// <param name = "valueRef">No documentation.</param>
        /// <param name = "cchValue">No documentation.</param>
        /// <param name = "mdRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::DefineField([In] unsigned int td,[In] const wchar_t* szName,[In] unsigned int dwFieldFlags,[In] const unsigned char* pvSigBlob,[In] unsigned int cbSigBlob,[In] unsigned int dwCPlusTypeFlag,[In] const void* pValue,[In] unsigned int cchValue,[In] unsigned int* pmd)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::DefineField</unmanaged-short>
        public unsafe void DefineField(System.UInt32 td, System.String szName, System.UInt32 dwFieldFlags, System.Byte vSigBlobRef, System.UInt32 cbSigBlob, System.UInt32 dwCPlusTypeFlag, System.IntPtr valueRef, System.UInt32 cchValue, System.UInt32 mdRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, td, (void *)((void *)szName_), dwFieldFlags, (void *)(&vSigBlobRef), cbSigBlob, dwCPlusTypeFlag, (void *)((void *)valueRef), cchValue, (void *)(&mdRef), (*(void ***)this._nativePointer)[39]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "td">No documentation.</param>
        /// <param name = "szProperty">No documentation.</param>
        /// <param name = "dwPropFlags">No documentation.</param>
        /// <param name = "vSigRef">No documentation.</param>
        /// <param name = "cbSig">No documentation.</param>
        /// <param name = "dwCPlusTypeFlag">No documentation.</param>
        /// <param name = "valueRef">No documentation.</param>
        /// <param name = "cchValue">No documentation.</param>
        /// <param name = "mdSetter">No documentation.</param>
        /// <param name = "mdGetter">No documentation.</param>
        /// <param name = "rmdOtherMethods">No documentation.</param>
        /// <param name = "mdPropRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::DefineProperty([In] unsigned int td,[In] const wchar_t* szProperty,[In] unsigned int dwPropFlags,[In] const unsigned char* pvSig,[In] unsigned int cbSig,[In] unsigned int dwCPlusTypeFlag,[In] const void* pValue,[In] unsigned int cchValue,[In] unsigned int mdSetter,[In] unsigned int mdGetter,[In] unsigned int* rmdOtherMethods,[In] unsigned int* pmdProp)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::DefineProperty</unmanaged-short>
        public unsafe void DefineProperty(System.UInt32 td, System.String szProperty, System.UInt32 dwPropFlags, System.Byte vSigRef, System.UInt32 cbSig, System.UInt32 dwCPlusTypeFlag, System.IntPtr valueRef, System.UInt32 cchValue, System.UInt32 mdSetter, System.UInt32 mdGetter, System.UInt32 rmdOtherMethods, System.UInt32 mdPropRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szProperty_ = szProperty)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, td, (void *)((void *)szProperty_), dwPropFlags, (void *)(&vSigRef), cbSig, dwCPlusTypeFlag, (void *)((void *)valueRef), cchValue, mdSetter, mdGetter, (void *)(&rmdOtherMethods), (void *)(&mdPropRef), (*(void ***)this._nativePointer)[40]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "md">No documentation.</param>
        /// <param name = "ulParamSeq">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "dwParamFlags">No documentation.</param>
        /// <param name = "dwCPlusTypeFlag">No documentation.</param>
        /// <param name = "valueRef">No documentation.</param>
        /// <param name = "cchValue">No documentation.</param>
        /// <param name = "dOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::DefineParam([In] unsigned int md,[In] unsigned int ulParamSeq,[In] const wchar_t* szName,[In] unsigned int dwParamFlags,[In] unsigned int dwCPlusTypeFlag,[In] const void* pValue,[In] unsigned int cchValue,[In] unsigned int* ppd)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::DefineParam</unmanaged-short>
        public unsafe void DefineParam(System.UInt32 md, System.UInt32 ulParamSeq, System.String szName, System.UInt32 dwParamFlags, System.UInt32 dwCPlusTypeFlag, System.IntPtr valueRef, System.UInt32 cchValue, System.UInt32 dOut)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, md, ulParamSeq, (void *)((void *)szName_), dwParamFlags, dwCPlusTypeFlag, (void *)((void *)valueRef), cchValue, (void *)(&dOut), (*(void ***)this._nativePointer)[41]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "fd">No documentation.</param>
        /// <param name = "dwFieldFlags">No documentation.</param>
        /// <param name = "dwCPlusTypeFlag">No documentation.</param>
        /// <param name = "valueRef">No documentation.</param>
        /// <param name = "cchValue">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::SetFieldProps([In] unsigned int fd,[In] unsigned int dwFieldFlags,[In] unsigned int dwCPlusTypeFlag,[In] const void* pValue,[In] unsigned int cchValue)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::SetFieldProps</unmanaged-short>
        public unsafe void SetFieldProps(System.UInt32 fd, System.UInt32 dwFieldFlags, System.UInt32 dwCPlusTypeFlag, System.IntPtr valueRef, System.UInt32 cchValue)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, fd, dwFieldFlags, dwCPlusTypeFlag, (void *)((void *)valueRef), cchValue, (*(void ***)this._nativePointer)[42]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "pr">No documentation.</param>
        /// <param name = "dwPropFlags">No documentation.</param>
        /// <param name = "dwCPlusTypeFlag">No documentation.</param>
        /// <param name = "valueRef">No documentation.</param>
        /// <param name = "cchValue">No documentation.</param>
        /// <param name = "mdSetter">No documentation.</param>
        /// <param name = "mdGetter">No documentation.</param>
        /// <param name = "rmdOtherMethods">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::SetPropertyProps([In] unsigned int pr,[In] unsigned int dwPropFlags,[In] unsigned int dwCPlusTypeFlag,[In] const void* pValue,[In] unsigned int cchValue,[In] unsigned int mdSetter,[In] unsigned int mdGetter,[In] unsigned int* rmdOtherMethods)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::SetPropertyProps</unmanaged-short>
        public unsafe void SetPropertyProps(System.UInt32 pr, System.UInt32 dwPropFlags, System.UInt32 dwCPlusTypeFlag, System.IntPtr valueRef, System.UInt32 cchValue, System.UInt32 mdSetter, System.UInt32 mdGetter, System.UInt32 rmdOtherMethods)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, pr, dwPropFlags, dwCPlusTypeFlag, (void *)((void *)valueRef), cchValue, mdSetter, mdGetter, (void *)(&rmdOtherMethods), (*(void ***)this._nativePointer)[43]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "pd">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "dwParamFlags">No documentation.</param>
        /// <param name = "dwCPlusTypeFlag">No documentation.</param>
        /// <param name = "valueRef">No documentation.</param>
        /// <param name = "cchValue">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::SetParamProps([In] unsigned int pd,[In] const wchar_t* szName,[In] unsigned int dwParamFlags,[In] unsigned int dwCPlusTypeFlag,[In] const void* pValue,[In] unsigned int cchValue)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::SetParamProps</unmanaged-short>
        public unsafe void SetParamProps(System.UInt32 pd, System.String szName, System.UInt32 dwParamFlags, System.UInt32 dwCPlusTypeFlag, System.IntPtr valueRef, System.UInt32 cchValue)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, pd, (void *)((void *)szName_), dwParamFlags, dwCPlusTypeFlag, (void *)((void *)valueRef), cchValue, (*(void ***)this._nativePointer)[44]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tkObj">No documentation.</param>
        /// <param name = "rSecAttrs">No documentation.</param>
        /// <param name = "cSecAttrs">No documentation.</param>
        /// <param name = "ulErrorAttrRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::DefineSecurityAttributeSet([In] unsigned int tkObj,[In] COR_SECATTR* rSecAttrs,[In] unsigned int cSecAttrs,[In] unsigned int* pulErrorAttr)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::DefineSecurityAttributeSet</unmanaged-short>
        public unsafe void DefineSecurityAttributeSet(System.UInt32 tkObj, ref CorApi.Portable.CorSecattr rSecAttrs, System.UInt32 cSecAttrs, System.UInt32 ulErrorAttrRef)
        {
            var rSecAttrs_ = new CorApi.Portable.CorSecattr.__Native();
            rSecAttrs.__MarshalTo(ref rSecAttrs_);
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, tkObj, (void *)(&rSecAttrs_), cSecAttrs, (void *)(&ulErrorAttrRef), (*(void ***)this._nativePointer)[45]);
            rSecAttrs.__MarshalFree(ref rSecAttrs_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "importRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::ApplyEditAndContinue([In] IUnknown* pImport)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::ApplyEditAndContinue</unmanaged-short>
        public unsafe void ApplyEditAndContinue(SharpGen.Runtime.IUnknown importRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<SharpGen.Runtime.IUnknown>(importRef))), (*(void ***)this._nativePointer)[46]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "assemImportRef">No documentation.</param>
        /// <param name = "bHashValueRef">No documentation.</param>
        /// <param name = "cbHashValue">No documentation.</param>
        /// <param name = "import">No documentation.</param>
        /// <param name = "bSigBlobRef">No documentation.</param>
        /// <param name = "cbSigBlob">No documentation.</param>
        /// <param name = "assemEmitRef">No documentation.</param>
        /// <param name = "emit">No documentation.</param>
        /// <param name = "vTranslatedSigRef">No documentation.</param>
        /// <param name = "cbTranslatedSigMax">No documentation.</param>
        /// <param name = "cbTranslatedSigRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::TranslateSigWithScope([In] IMetaDataAssemblyImport* pAssemImport,[In] const void* pbHashValue,[In] unsigned int cbHashValue,[In] IMetaDataImport* import,[In] const unsigned char* pbSigBlob,[In] unsigned int cbSigBlob,[In] IMetaDataAssemblyEmit* pAssemEmit,[In] IMetaDataEmit* emit,[In] unsigned char* pvTranslatedSig,[In] unsigned int cbTranslatedSigMax,[In] unsigned int* pcbTranslatedSig)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::TranslateSigWithScope</unmanaged-short>
        public unsafe void TranslateSigWithScope(CorApi.Portable.IMetaDataAssemblyImport assemImportRef, System.IntPtr bHashValueRef, System.UInt32 cbHashValue, CorApi.Portable.IMetaDataImport import, System.Byte bSigBlobRef, System.UInt32 cbSigBlob, CorApi.Portable.IMetaDataAssemblyEmit assemEmitRef, CorApi.Portable.IMetaDataEmit emit, System.Byte vTranslatedSigRef, System.UInt32 cbTranslatedSigMax, System.UInt32 cbTranslatedSigRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.IMetaDataAssemblyImport>(assemImportRef))), (void *)((void *)bHashValueRef), cbHashValue, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.IMetaDataImport>(import))), (void *)(&bSigBlobRef), cbSigBlob, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.IMetaDataAssemblyEmit>(assemEmitRef))), (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.IMetaDataEmit>(emit))), (void *)(&vTranslatedSigRef), cbTranslatedSigMax, (void *)(&cbTranslatedSigRef), (*(void ***)this._nativePointer)[47]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "md">No documentation.</param>
        /// <param name = "dwImplFlags">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::SetMethodImplFlags([In] unsigned int md,[In] unsigned int dwImplFlags)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::SetMethodImplFlags</unmanaged-short>
        public unsafe void SetMethodImplFlags(System.UInt32 md, System.UInt32 dwImplFlags)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, md, dwImplFlags, (*(void ***)this._nativePointer)[48]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "fd">No documentation.</param>
        /// <param name = "ulRVA">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::SetFieldRVA([In] unsigned int fd,[In] unsigned int ulRVA)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::SetFieldRVA</unmanaged-short>
        public unsafe void SetFieldRVA(System.UInt32 fd, System.UInt32 ulRVA)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, fd, ulRVA, (*(void ***)this._nativePointer)[49]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "importRef">No documentation.</param>
        /// <param name = "hostMapTokenRef">No documentation.</param>
        /// <param name = "handlerRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::Merge([In] IMetaDataImport* pImport,[In] IMapToken* pHostMapToken,[In] IUnknown* pHandler)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::Merge</unmanaged-short>
        public unsafe void Merge(CorApi.Portable.IMetaDataImport importRef, CorApi.Portable.IMapToken hostMapTokenRef, SharpGen.Runtime.IUnknown handlerRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.IMetaDataImport>(importRef))), (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.IMapToken>(hostMapTokenRef))), (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<SharpGen.Runtime.IUnknown>(handlerRef))), (*(void ***)this._nativePointer)[50]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit::MergeEnd()</unmanaged>
        /// <unmanaged-short>IMetaDataEmit::MergeEnd</unmanaged-short>
        public unsafe void MergeEnd()
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (*(void ***)this._nativePointer)[51]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("f5dd9950-f693-42e6-830e-7b833e8146a9")]
    public partial class IMetaDataEmit2 : CorApi.Portable.IMetaDataEmit
    {
        public IMetaDataEmit2(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator IMetaDataEmit2(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new IMetaDataEmit2(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tkParent">No documentation.</param>
        /// <param name = "vSigBlobRef">No documentation.</param>
        /// <param name = "cbSigBlob">No documentation.</param>
        /// <param name = "miRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit2::DefineMethodSpec([In] unsigned int tkParent,[In] const unsigned char* pvSigBlob,[In] unsigned int cbSigBlob,[In] unsigned int* pmi)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit2::DefineMethodSpec</unmanaged-short>
        public unsafe void DefineMethodSpec(System.UInt32 tkParent, System.Byte vSigBlobRef, System.UInt32 cbSigBlob, System.UInt32 miRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, tkParent, (void *)(&vSigBlobRef), cbSigBlob, (void *)(&miRef), (*(void ***)this._nativePointer)[52]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "fSave">No documentation.</param>
        /// <param name = "dwSaveSizeRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit2::GetDeltaSaveSize([In] CorSaveSize fSave,[In] unsigned int* pdwSaveSize)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit2::GetDeltaSaveSize</unmanaged-short>
        public unsafe void GetDeltaSaveSize(CorApi.Portable.CorSaveSize fSave, System.UInt32 dwSaveSizeRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, unchecked ((System.Int32)fSave), (void *)(&dwSaveSizeRef), (*(void ***)this._nativePointer)[53]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "szFile">No documentation.</param>
        /// <param name = "dwSaveFlags">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit2::SaveDelta([In] const wchar_t* szFile,[In] unsigned int dwSaveFlags)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit2::SaveDelta</unmanaged-short>
        public unsafe void SaveDelta(System.String szFile, System.UInt32 dwSaveFlags)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szFile_ = szFile)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)szFile_), dwSaveFlags, (*(void ***)this._nativePointer)[54]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "iStreamRef">No documentation.</param>
        /// <param name = "dwSaveFlags">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit2::SaveDeltaToStream([In] IStream* pIStream,[In] unsigned int dwSaveFlags)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit2::SaveDeltaToStream</unmanaged-short>
        public unsafe void SaveDeltaToStream(SharpGen.Runtime.Win32.IStream iStreamRef, System.UInt32 dwSaveFlags)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<SharpGen.Runtime.Win32.IStream>(iStreamRef))), dwSaveFlags, (*(void ***)this._nativePointer)[55]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "bDataRef">No documentation.</param>
        /// <param name = "cbData">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit2::SaveDeltaToMemory([In] void* pbData,[In] unsigned int cbData)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit2::SaveDeltaToMemory</unmanaged-short>
        public unsafe void SaveDeltaToMemory(System.IntPtr bDataRef, System.UInt32 cbData)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)bDataRef), cbData, (*(void ***)this._nativePointer)[56]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tk">No documentation.</param>
        /// <param name = "ulParamSeq">No documentation.</param>
        /// <param name = "dwParamFlags">No documentation.</param>
        /// <param name = "szname">No documentation.</param>
        /// <param name = "reserved">No documentation.</param>
        /// <param name = "rtkConstraints">No documentation.</param>
        /// <param name = "gpRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit2::DefineGenericParam([In] unsigned int tk,[In] unsigned int ulParamSeq,[In] unsigned int dwParamFlags,[In] const wchar_t* szname,[In] unsigned int reserved,[In] unsigned int* rtkConstraints,[In] unsigned int* pgp)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit2::DefineGenericParam</unmanaged-short>
        public unsafe void DefineGenericParam(System.UInt32 tk, System.UInt32 ulParamSeq, System.UInt32 dwParamFlags, System.String szname, System.UInt32 reserved, System.UInt32 rtkConstraints, System.UInt32 gpRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szname_ = szname)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, tk, ulParamSeq, dwParamFlags, (void *)((void *)szname_), reserved, (void *)(&rtkConstraints), (void *)(&gpRef), (*(void ***)this._nativePointer)[57]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "gp">No documentation.</param>
        /// <param name = "dwParamFlags">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "reserved">No documentation.</param>
        /// <param name = "rtkConstraints">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit2::SetGenericParamProps([In] unsigned int gp,[In] unsigned int dwParamFlags,[In] const wchar_t* szName,[In] unsigned int reserved,[In] unsigned int* rtkConstraints)</unmanaged>
        /// <unmanaged-short>IMetaDataEmit2::SetGenericParamProps</unmanaged-short>
        public unsafe void SetGenericParamProps(System.UInt32 gp, System.UInt32 dwParamFlags, System.String szName, System.UInt32 reserved, System.UInt32 rtkConstraints)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, gp, dwParamFlags, (void *)((void *)szName_), reserved, (void *)(&rtkConstraints), (*(void ***)this._nativePointer)[58]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataEmit2::ResetENCLog()</unmanaged>
        /// <unmanaged-short>IMetaDataEmit2::ResetENCLog</unmanaged-short>
        public unsafe void ResetENCLog()
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (*(void ***)this._nativePointer)[59]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("b81ff171-20f3-11d2-8dcc-00a0c9b09c19")]
    public partial class IMetaDataError : SharpGen.Runtime.ComObject
    {
        public IMetaDataError(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator IMetaDataError(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new IMetaDataError(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hrError">No documentation.</param>
        /// <param name = "token">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataError::OnError([In] HRESULT hrError,[In] unsigned int token)</unmanaged>
        /// <unmanaged-short>IMetaDataError::OnError</unmanaged-short>
        public unsafe void OnError(SharpGen.Runtime.Result hrError, System.UInt32 token)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint0(this._nativePointer, hrError, token, (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("d0e80dd1-12d4-11d3-b39d-00c04ff81795")]
    public partial class IMetaDataFilter : SharpGen.Runtime.ComObject
    {
        public IMetaDataFilter(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator IMetaDataFilter(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new IMetaDataFilter(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataFilter::UnmarkAll()</unmanaged>
        /// <unmanaged-short>IMetaDataFilter::UnmarkAll</unmanaged-short>
        public unsafe void UnmarkAll()
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tk">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataFilter::MarkToken([In] unsigned int tk)</unmanaged>
        /// <unmanaged-short>IMetaDataFilter::MarkToken</unmanaged-short>
        public unsafe void MarkToken(System.UInt32 tk)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, tk, (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tk">No documentation.</param>
        /// <param name = "isMarkedRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataFilter::IsTokenMarked([In] unsigned int tk,[In] BOOL* pIsMarked)</unmanaged>
        /// <unmanaged-short>IMetaDataFilter::IsTokenMarked</unmanaged-short>
        public unsafe void IsTokenMarked(System.UInt32 tk, SharpGen.Runtime.Win32.RawBool isMarkedRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, tk, (void *)(&isMarkedRef), (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("7dac8207-d3ae-4c75-9b67-92801a497d44")]
    public partial class IMetaDataImport : SharpGen.Runtime.ComObject
    {
        public IMetaDataImport(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator IMetaDataImport(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new IMetaDataImport(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hEnum">No documentation.</param>
        /// <unmanaged>void IMetaDataImport::CloseEnum([In] void* hEnum)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::CloseEnum</unmanaged-short>
        public unsafe void CloseEnum(System.IntPtr hEnum)
        {
            CorDebug.LocalInterop.Callivoid(this._nativePointer, (void *)((void *)hEnum), (*(void ***)this._nativePointer)[3]);
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hEnum">No documentation.</param>
        /// <param name = "ulCountRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::CountEnum([In] void* hEnum,[Out] unsigned int* pulCount)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::CountEnum</unmanaged-short>
        public unsafe void CountEnum(System.IntPtr hEnum, out System.UInt32 ulCountRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *ulCountRef_ = &ulCountRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)hEnum), (void *)(ulCountRef_), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hEnum">No documentation.</param>
        /// <param name = "ulPos">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::ResetEnum([In] void* hEnum,[In] unsigned int ulPos)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::ResetEnum</unmanaged-short>
        public unsafe void ResetEnum(System.IntPtr hEnum, System.UInt32 ulPos)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)hEnum), ulPos, (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hEnumRef">No documentation.</param>
        /// <param name = "rTypeDefs">No documentation.</param>
        /// <param name = "cMax">No documentation.</param>
        /// <param name = "cTypeDefsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::EnumTypeDefs([Out] void** phEnum,[Out, Buffer] unsigned int* rTypeDefs,[In] unsigned int cMax,[Out] unsigned int* pcTypeDefs)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::EnumTypeDefs</unmanaged-short>
        public unsafe void EnumTypeDefs(out System.IntPtr hEnumRef, System.UInt32[] rTypeDefs, System.UInt32 cMax, out System.UInt32 cTypeDefsRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *cTypeDefsRef_ = &cTypeDefsRef)
                fixed (void *rTypeDefs_ = rTypeDefs)
                    fixed (void *hEnumRef_ = &hEnumRef)
                        __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(hEnumRef_), (void *)(rTypeDefs_), cMax, (void *)(cTypeDefsRef_), (*(void ***)this._nativePointer)[6]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hEnumRef">No documentation.</param>
        /// <param name = "td">No documentation.</param>
        /// <param name = "rImpls">No documentation.</param>
        /// <param name = "cMax">No documentation.</param>
        /// <param name = "cImplsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::EnumInterfaceImpls([Out] void** phEnum,[In] unsigned int td,[Out, Buffer] unsigned int* rImpls,[In] unsigned int cMax,[Out] unsigned int* pcImpls)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::EnumInterfaceImpls</unmanaged-short>
        public unsafe void EnumInterfaceImpls(out System.IntPtr hEnumRef, System.UInt32 td, System.UInt32[] rImpls, System.UInt32 cMax, out System.UInt32 cImplsRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *cImplsRef_ = &cImplsRef)
                fixed (void *rImpls_ = rImpls)
                    fixed (void *hEnumRef_ = &hEnumRef)
                        __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(hEnumRef_), td, (void *)(rImpls_), cMax, (void *)(cImplsRef_), (*(void ***)this._nativePointer)[7]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hEnumRef">No documentation.</param>
        /// <param name = "rTypeRefs">No documentation.</param>
        /// <param name = "cMax">No documentation.</param>
        /// <param name = "cTypeRefsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::EnumTypeRefs([In] void** phEnum,[In] unsigned int* rTypeRefs,[In] unsigned int cMax,[In] unsigned int* pcTypeRefs)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::EnumTypeRefs</unmanaged-short>
        public unsafe void EnumTypeRefs(System.IntPtr hEnumRef, System.UInt32 rTypeRefs, System.UInt32 cMax, System.UInt32 cTypeRefsRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)hEnumRef), (void *)(&rTypeRefs), cMax, (void *)(&cTypeRefsRef), (*(void ***)this._nativePointer)[8]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "szTypeDef">No documentation.</param>
        /// <param name = "tkEnclosingClass">No documentation.</param>
        /// <param name = "tdRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::FindTypeDefByName([In] const wchar_t* szTypeDef,[In] unsigned int tkEnclosingClass,[Out] unsigned int* ptd)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::FindTypeDefByName</unmanaged-short>
        public unsafe void FindTypeDefByName(System.String szTypeDef, System.UInt32 tkEnclosingClass, out System.UInt32 tdRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *tdRef_ = &tdRef)
                fixed (char *szTypeDef_ = szTypeDef)
                    __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)szTypeDef_), tkEnclosingClass, (void *)(tdRef_), (*(void ***)this._nativePointer)[9]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "cchName">No documentation.</param>
        /// <param name = "chNameRef">No documentation.</param>
        /// <param name = "mvidRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::GetScopeProps([Out, Buffer, Optional] wchar_t* szName,[In] unsigned int cchName,[Out] unsigned int* pchName,[Out, Optional] GUID* pmvid)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::GetScopeProps</unmanaged-short>
        public unsafe void GetScopeProps(System.IntPtr szName, System.UInt32 cchName, out System.UInt32 chNameRef, out System.Guid mvidRef)
        {
            mvidRef = new System.Guid();
            SharpGen.Runtime.Result __result__;
            fixed (void *mvidRef_ = &mvidRef)
                fixed (void *chNameRef_ = &chNameRef)
                    __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)szName), cchName, (void *)(chNameRef_), (void *)(mvidRef_), (*(void ***)this._nativePointer)[10]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "mdRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::GetModuleFromScope([In] unsigned int* pmd)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::GetModuleFromScope</unmanaged-short>
        public unsafe void GetModuleFromScope(System.UInt32 mdRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&mdRef), (*(void ***)this._nativePointer)[11]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "td">No documentation.</param>
        /// <param name = "szTypeDef">No documentation.</param>
        /// <param name = "cchTypeDef">No documentation.</param>
        /// <param name = "chTypeDefRef">No documentation.</param>
        /// <param name = "dwTypeDefFlagsRef">No documentation.</param>
        /// <param name = "tkExtendsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::GetTypeDefProps([In] unsigned int td,[Out] wchar_t* szTypeDef,[In] unsigned int cchTypeDef,[Out] unsigned int* pchTypeDef,[Out] unsigned int* pdwTypeDefFlags,[Out] unsigned int* ptkExtends)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::GetTypeDefProps</unmanaged-short>
        public unsafe void GetTypeDefProps(System.UInt32 td, System.IntPtr szTypeDef, System.UInt32 cchTypeDef, out System.UInt32 chTypeDefRef, out System.UInt32 dwTypeDefFlagsRef, out System.UInt32 tkExtendsRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *tkExtendsRef_ = &tkExtendsRef)
                fixed (void *dwTypeDefFlagsRef_ = &dwTypeDefFlagsRef)
                    fixed (void *chTypeDefRef_ = &chTypeDefRef)
                        __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, td, (void *)((void *)szTypeDef), cchTypeDef, (void *)(chTypeDefRef_), (void *)(dwTypeDefFlagsRef_), (void *)(tkExtendsRef_), (*(void ***)this._nativePointer)[12]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "iiImpl">No documentation.</param>
        /// <param name = "classRef">No documentation.</param>
        /// <param name = "tkIfaceRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::GetInterfaceImplProps([In] unsigned int iiImpl,[Out] unsigned int* pClass,[Out] unsigned int* ptkIface)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::GetInterfaceImplProps</unmanaged-short>
        public unsafe void GetInterfaceImplProps(System.UInt32 iiImpl, out System.UInt32 classRef, out System.UInt32 tkIfaceRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *tkIfaceRef_ = &tkIfaceRef)
                fixed (void *classRef_ = &classRef)
                    __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, iiImpl, (void *)(classRef_), (void *)(tkIfaceRef_), (*(void ***)this._nativePointer)[13]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tr">No documentation.</param>
        /// <param name = "tkResolutionScopeRef">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "cchName">No documentation.</param>
        /// <param name = "chNameRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::GetTypeRefProps([In] unsigned int tr,[Out] unsigned int* ptkResolutionScope,[Out] wchar_t* szName,[In] unsigned int cchName,[Out] unsigned int* pchName)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::GetTypeRefProps</unmanaged-short>
        public unsafe void GetTypeRefProps(System.UInt32 tr, out System.UInt32 tkResolutionScopeRef, System.IntPtr szName, System.UInt32 cchName, out System.UInt32 chNameRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *chNameRef_ = &chNameRef)
                fixed (void *tkResolutionScopeRef_ = &tkResolutionScopeRef)
                    __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, tr, (void *)(tkResolutionScopeRef_), (void *)((void *)szName), cchName, (void *)(chNameRef_), (*(void ***)this._nativePointer)[14]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tr">No documentation.</param>
        /// <param name = "riid">No documentation.</param>
        /// <param name = "iScopeOut">No documentation.</param>
        /// <param name = "tdRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::ResolveTypeRef([In] unsigned int tr,[In] const GUID&amp; riid,[In] IUnknown** ppIScope,[In] unsigned int* ptd)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::ResolveTypeRef</unmanaged-short>
        public unsafe void ResolveTypeRef(System.UInt32 tr, System.Guid riid, out SharpGen.Runtime.IUnknown iScopeOut, System.UInt32 tdRef)
        {
            System.IntPtr iScopeOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, tr, (void *)(&riid), (void *)(&iScopeOut_), (void *)(&tdRef), (*(void ***)this._nativePointer)[15]);
            iScopeOut = (iScopeOut_ == System.IntPtr.Zero) ? null : new SharpGen.Runtime.ComObject(iScopeOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hEnumRef">No documentation.</param>
        /// <param name = "cl">No documentation.</param>
        /// <param name = "rMembers">No documentation.</param>
        /// <param name = "cMax">No documentation.</param>
        /// <param name = "cTokensRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::EnumMembers([In] void** phEnum,[In] unsigned int cl,[In] unsigned int* rMembers,[In] unsigned int cMax,[In] unsigned int* pcTokens)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::EnumMembers</unmanaged-short>
        public unsafe void EnumMembers(System.IntPtr hEnumRef, System.UInt32 cl, System.UInt32 rMembers, System.UInt32 cMax, System.UInt32 cTokensRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)hEnumRef), cl, (void *)(&rMembers), cMax, (void *)(&cTokensRef), (*(void ***)this._nativePointer)[16]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hEnumRef">No documentation.</param>
        /// <param name = "cl">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "rMembers">No documentation.</param>
        /// <param name = "cMax">No documentation.</param>
        /// <param name = "cTokensRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::EnumMembersWithName([In] void** phEnum,[In] unsigned int cl,[In] const wchar_t* szName,[In] unsigned int* rMembers,[In] unsigned int cMax,[In] unsigned int* pcTokens)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::EnumMembersWithName</unmanaged-short>
        public unsafe void EnumMembersWithName(System.IntPtr hEnumRef, System.UInt32 cl, System.String szName, System.UInt32 rMembers, System.UInt32 cMax, System.UInt32 cTokensRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)hEnumRef), cl, (void *)((void *)szName_), (void *)(&rMembers), cMax, (void *)(&cTokensRef), (*(void ***)this._nativePointer)[17]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hEnumRef">No documentation.</param>
        /// <param name = "cl">No documentation.</param>
        /// <param name = "rMethods">No documentation.</param>
        /// <param name = "cMax">No documentation.</param>
        /// <param name = "cTokensRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::EnumMethods([Out] void** phEnum,[In] unsigned int cl,[Out, Buffer] unsigned int* rMethods,[In] unsigned int cMax,[Out] unsigned int* pcTokens)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::EnumMethods</unmanaged-short>
        public unsafe void EnumMethods(out System.IntPtr hEnumRef, System.UInt32 cl, System.UInt32[] rMethods, System.UInt32 cMax, out System.UInt32 cTokensRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *cTokensRef_ = &cTokensRef)
                fixed (void *rMethods_ = rMethods)
                    fixed (void *hEnumRef_ = &hEnumRef)
                        __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(hEnumRef_), cl, (void *)(rMethods_), cMax, (void *)(cTokensRef_), (*(void ***)this._nativePointer)[18]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hEnumRef">No documentation.</param>
        /// <param name = "cl">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "rMethods">No documentation.</param>
        /// <param name = "cMax">No documentation.</param>
        /// <param name = "cTokensRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::EnumMethodsWithName([In] void** phEnum,[In] unsigned int cl,[In] const wchar_t* szName,[In] unsigned int* rMethods,[In] unsigned int cMax,[In] unsigned int* pcTokens)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::EnumMethodsWithName</unmanaged-short>
        public unsafe void EnumMethodsWithName(System.IntPtr hEnumRef, System.UInt32 cl, System.String szName, System.UInt32 rMethods, System.UInt32 cMax, System.UInt32 cTokensRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)hEnumRef), cl, (void *)((void *)szName_), (void *)(&rMethods), cMax, (void *)(&cTokensRef), (*(void ***)this._nativePointer)[19]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hEnumRef">No documentation.</param>
        /// <param name = "cl">No documentation.</param>
        /// <param name = "rFields">No documentation.</param>
        /// <param name = "cMax">No documentation.</param>
        /// <param name = "cTokensRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::EnumFields([Out] void** phEnum,[In] unsigned int cl,[Out, Buffer] unsigned int* rFields,[In] unsigned int cMax,[Out] unsigned int* pcTokens)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::EnumFields</unmanaged-short>
        public unsafe void EnumFields(out System.IntPtr hEnumRef, System.UInt32 cl, System.UInt32[] rFields, System.UInt32 cMax, out System.UInt32 cTokensRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *cTokensRef_ = &cTokensRef)
                fixed (void *rFields_ = rFields)
                    fixed (void *hEnumRef_ = &hEnumRef)
                        __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(hEnumRef_), cl, (void *)(rFields_), cMax, (void *)(cTokensRef_), (*(void ***)this._nativePointer)[20]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hEnumRef">No documentation.</param>
        /// <param name = "cl">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "rFields">No documentation.</param>
        /// <param name = "cMax">No documentation.</param>
        /// <param name = "cTokensRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::EnumFieldsWithName([In] void** phEnum,[In] unsigned int cl,[In] const wchar_t* szName,[In] unsigned int* rFields,[In] unsigned int cMax,[In] unsigned int* pcTokens)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::EnumFieldsWithName</unmanaged-short>
        public unsafe void EnumFieldsWithName(System.IntPtr hEnumRef, System.UInt32 cl, System.String szName, System.UInt32 rFields, System.UInt32 cMax, System.UInt32 cTokensRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)hEnumRef), cl, (void *)((void *)szName_), (void *)(&rFields), cMax, (void *)(&cTokensRef), (*(void ***)this._nativePointer)[21]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hEnumRef">No documentation.</param>
        /// <param name = "mb">No documentation.</param>
        /// <param name = "rParams">No documentation.</param>
        /// <param name = "cMax">No documentation.</param>
        /// <param name = "cTokensRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::EnumParams([Out] void** phEnum,[In] unsigned int mb,[Out, Buffer] unsigned int* rParams,[In] unsigned int cMax,[Out] unsigned int* pcTokens)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::EnumParams</unmanaged-short>
        public unsafe void EnumParams(out System.IntPtr hEnumRef, System.UInt32 mb, System.UInt32[] rParams, System.UInt32 cMax, out System.UInt32 cTokensRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *cTokensRef_ = &cTokensRef)
                fixed (void *rParams_ = rParams)
                    fixed (void *hEnumRef_ = &hEnumRef)
                        __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(hEnumRef_), mb, (void *)(rParams_), cMax, (void *)(cTokensRef_), (*(void ***)this._nativePointer)[22]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hEnumRef">No documentation.</param>
        /// <param name = "tkParent">No documentation.</param>
        /// <param name = "rMemberRefs">No documentation.</param>
        /// <param name = "cMax">No documentation.</param>
        /// <param name = "cTokensRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::EnumMemberRefs([In] void** phEnum,[In] unsigned int tkParent,[In] unsigned int* rMemberRefs,[In] unsigned int cMax,[In] unsigned int* pcTokens)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::EnumMemberRefs</unmanaged-short>
        public unsafe void EnumMemberRefs(System.IntPtr hEnumRef, System.UInt32 tkParent, System.UInt32 rMemberRefs, System.UInt32 cMax, System.UInt32 cTokensRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)hEnumRef), tkParent, (void *)(&rMemberRefs), cMax, (void *)(&cTokensRef), (*(void ***)this._nativePointer)[23]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hEnumRef">No documentation.</param>
        /// <param name = "td">No documentation.</param>
        /// <param name = "rMethodBody">No documentation.</param>
        /// <param name = "rMethodDecl">No documentation.</param>
        /// <param name = "cMax">No documentation.</param>
        /// <param name = "cTokensRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::EnumMethodImpls([In] void** phEnum,[In] unsigned int td,[In] unsigned int* rMethodBody,[In] unsigned int* rMethodDecl,[In] unsigned int cMax,[In] unsigned int* pcTokens)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::EnumMethodImpls</unmanaged-short>
        public unsafe void EnumMethodImpls(System.IntPtr hEnumRef, System.UInt32 td, System.UInt32 rMethodBody, System.UInt32 rMethodDecl, System.UInt32 cMax, System.UInt32 cTokensRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)hEnumRef), td, (void *)(&rMethodBody), (void *)(&rMethodDecl), cMax, (void *)(&cTokensRef), (*(void ***)this._nativePointer)[24]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hEnumRef">No documentation.</param>
        /// <param name = "tk">No documentation.</param>
        /// <param name = "dwActions">No documentation.</param>
        /// <param name = "rPermission">No documentation.</param>
        /// <param name = "cMax">No documentation.</param>
        /// <param name = "cTokensRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::EnumPermissionSets([In] void** phEnum,[In] unsigned int tk,[In] unsigned int dwActions,[In] unsigned int* rPermission,[In] unsigned int cMax,[In] unsigned int* pcTokens)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::EnumPermissionSets</unmanaged-short>
        public unsafe void EnumPermissionSets(System.IntPtr hEnumRef, System.UInt32 tk, System.UInt32 dwActions, System.UInt32 rPermission, System.UInt32 cMax, System.UInt32 cTokensRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)hEnumRef), tk, dwActions, (void *)(&rPermission), cMax, (void *)(&cTokensRef), (*(void ***)this._nativePointer)[25]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "td">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "vSigBlobRef">No documentation.</param>
        /// <param name = "cbSigBlob">No documentation.</param>
        /// <param name = "mbRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::FindMember([In] unsigned int td,[In] const wchar_t* szName,[In] const unsigned char* pvSigBlob,[In] unsigned int cbSigBlob,[In] unsigned int* pmb)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::FindMember</unmanaged-short>
        public unsafe void FindMember(System.UInt32 td, System.String szName, System.Byte vSigBlobRef, System.UInt32 cbSigBlob, System.UInt32 mbRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, td, (void *)((void *)szName_), (void *)(&vSigBlobRef), cbSigBlob, (void *)(&mbRef), (*(void ***)this._nativePointer)[26]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "td">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "vSigBlobRef">No documentation.</param>
        /// <param name = "cbSigBlob">No documentation.</param>
        /// <param name = "mbRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::FindMethod([In] unsigned int td,[In] const wchar_t* szName,[In] const unsigned char* pvSigBlob,[In] unsigned int cbSigBlob,[In] unsigned int* pmb)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::FindMethod</unmanaged-short>
        public unsafe void FindMethod(System.UInt32 td, System.String szName, System.Byte vSigBlobRef, System.UInt32 cbSigBlob, System.UInt32 mbRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, td, (void *)((void *)szName_), (void *)(&vSigBlobRef), cbSigBlob, (void *)(&mbRef), (*(void ***)this._nativePointer)[27]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "td">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "vSigBlobRef">No documentation.</param>
        /// <param name = "cbSigBlob">No documentation.</param>
        /// <param name = "mbRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::FindField([In] unsigned int td,[In] const wchar_t* szName,[In] const unsigned char* pvSigBlob,[In] unsigned int cbSigBlob,[In] unsigned int* pmb)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::FindField</unmanaged-short>
        public unsafe void FindField(System.UInt32 td, System.String szName, System.Byte vSigBlobRef, System.UInt32 cbSigBlob, System.UInt32 mbRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, td, (void *)((void *)szName_), (void *)(&vSigBlobRef), cbSigBlob, (void *)(&mbRef), (*(void ***)this._nativePointer)[28]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "td">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "vSigBlobRef">No documentation.</param>
        /// <param name = "cbSigBlob">No documentation.</param>
        /// <param name = "mrRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::FindMemberRef([In] unsigned int td,[In] const wchar_t* szName,[In] const unsigned char* pvSigBlob,[In] unsigned int cbSigBlob,[In] unsigned int* pmr)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::FindMemberRef</unmanaged-short>
        public unsafe void FindMemberRef(System.UInt32 td, System.String szName, System.Byte vSigBlobRef, System.UInt32 cbSigBlob, System.UInt32 mrRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, td, (void *)((void *)szName_), (void *)(&vSigBlobRef), cbSigBlob, (void *)(&mrRef), (*(void ***)this._nativePointer)[29]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "mb">No documentation.</param>
        /// <param name = "classRef">No documentation.</param>
        /// <param name = "szMethod">No documentation.</param>
        /// <param name = "cchMethod">No documentation.</param>
        /// <param name = "chMethodRef">No documentation.</param>
        /// <param name = "dwAttrRef">No documentation.</param>
        /// <param name = "vSigBlobOut">No documentation.</param>
        /// <param name = "cbSigBlobRef">No documentation.</param>
        /// <param name = "ulCodeRVARef">No documentation.</param>
        /// <param name = "dwImplFlagsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::GetMethodProps([In] unsigned int mb,[Out] unsigned int* pClass,[Out, Buffer, Optional] wchar_t* szMethod,[In] unsigned int cchMethod,[Out] unsigned int* pchMethod,[Out] unsigned int* pdwAttr,[Out] const unsigned char** ppvSigBlob,[Out] unsigned int* pcbSigBlob,[Out] unsigned int* pulCodeRVA,[Out] unsigned int* pdwImplFlags)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::GetMethodProps</unmanaged-short>
        public unsafe void GetMethodProps(System.UInt32 mb, out System.UInt32 classRef, System.IntPtr szMethod, System.UInt32 cchMethod, out System.UInt32 chMethodRef, out System.UInt32 dwAttrRef, out System.IntPtr vSigBlobOut, out System.UInt32 cbSigBlobRef, out System.UInt32 ulCodeRVARef, out System.UInt32 dwImplFlagsRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *dwImplFlagsRef_ = &dwImplFlagsRef)
                fixed (void *ulCodeRVARef_ = &ulCodeRVARef)
                    fixed (void *cbSigBlobRef_ = &cbSigBlobRef)
                        fixed (void *vSigBlobOut_ = &vSigBlobOut)
                            fixed (void *dwAttrRef_ = &dwAttrRef)
                                fixed (void *chMethodRef_ = &chMethodRef)
                                    fixed (void *classRef_ = &classRef)
                                        __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, mb, (void *)(classRef_), (void *)((void *)szMethod), cchMethod, (void *)(chMethodRef_), (void *)(dwAttrRef_), (void *)(vSigBlobOut_), (void *)(cbSigBlobRef_), (void *)(ulCodeRVARef_), (void *)(dwImplFlagsRef_), (*(void ***)this._nativePointer)[30]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "mr">No documentation.</param>
        /// <param name = "tkRef">No documentation.</param>
        /// <param name = "szMember">No documentation.</param>
        /// <param name = "cchMember">No documentation.</param>
        /// <param name = "chMemberRef">No documentation.</param>
        /// <param name = "vSigBlobOut">No documentation.</param>
        /// <param name = "bSigRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::GetMemberRefProps([In] unsigned int mr,[Out] unsigned int* ptk,[Out] wchar_t* szMember,[In] unsigned int cchMember,[Out] unsigned int* pchMember,[Out] const unsigned char** ppvSigBlob,[Out] unsigned int* pbSig)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::GetMemberRefProps</unmanaged-short>
        public unsafe void GetMemberRefProps(System.UInt32 mr, out System.UInt32 tkRef, System.IntPtr szMember, System.UInt32 cchMember, out System.UInt32 chMemberRef, out System.IntPtr vSigBlobOut, out System.UInt32 bSigRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *bSigRef_ = &bSigRef)
                fixed (void *vSigBlobOut_ = &vSigBlobOut)
                    fixed (void *chMemberRef_ = &chMemberRef)
                        fixed (void *tkRef_ = &tkRef)
                            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, mr, (void *)(tkRef_), (void *)((void *)szMember), cchMember, (void *)(chMemberRef_), (void *)(vSigBlobOut_), (void *)(bSigRef_), (*(void ***)this._nativePointer)[31]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hEnumRef">No documentation.</param>
        /// <param name = "td">No documentation.</param>
        /// <param name = "rProperties">No documentation.</param>
        /// <param name = "cMax">No documentation.</param>
        /// <param name = "cPropertiesRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::EnumProperties([Out] void** phEnum,[In] unsigned int td,[Out, Buffer] unsigned int* rProperties,[In] unsigned int cMax,[Out] unsigned int* pcProperties)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::EnumProperties</unmanaged-short>
        public unsafe void EnumProperties(out System.IntPtr hEnumRef, System.UInt32 td, System.UInt32[] rProperties, System.UInt32 cMax, out System.UInt32 cPropertiesRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *cPropertiesRef_ = &cPropertiesRef)
                fixed (void *rProperties_ = rProperties)
                    fixed (void *hEnumRef_ = &hEnumRef)
                        __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(hEnumRef_), td, (void *)(rProperties_), cMax, (void *)(cPropertiesRef_), (*(void ***)this._nativePointer)[32]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hEnumRef">No documentation.</param>
        /// <param name = "td">No documentation.</param>
        /// <param name = "rEvents">No documentation.</param>
        /// <param name = "cMax">No documentation.</param>
        /// <param name = "cEventsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::EnumEvents([In] void** phEnum,[In] unsigned int td,[In] unsigned int* rEvents,[In] unsigned int cMax,[In] unsigned int* pcEvents)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::EnumEvents</unmanaged-short>
        public unsafe void EnumEvents(System.IntPtr hEnumRef, System.UInt32 td, System.UInt32 rEvents, System.UInt32 cMax, System.UInt32 cEventsRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)hEnumRef), td, (void *)(&rEvents), cMax, (void *)(&cEventsRef), (*(void ***)this._nativePointer)[33]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "ev">No documentation.</param>
        /// <param name = "classRef">No documentation.</param>
        /// <param name = "szEvent">No documentation.</param>
        /// <param name = "cchEvent">No documentation.</param>
        /// <param name = "chEventRef">No documentation.</param>
        /// <param name = "dwEventFlagsRef">No documentation.</param>
        /// <param name = "tkEventTypeRef">No documentation.</param>
        /// <param name = "mdAddOnRef">No documentation.</param>
        /// <param name = "mdRemoveOnRef">No documentation.</param>
        /// <param name = "mdFireRef">No documentation.</param>
        /// <param name = "rmdOtherMethod">No documentation.</param>
        /// <param name = "cMax">No documentation.</param>
        /// <param name = "cOtherMethodRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::GetEventProps([In] unsigned int ev,[In] unsigned int* pClass,[In] const wchar_t* szEvent,[In] unsigned int cchEvent,[In] unsigned int* pchEvent,[In] unsigned int* pdwEventFlags,[In] unsigned int* ptkEventType,[In] unsigned int* pmdAddOn,[In] unsigned int* pmdRemoveOn,[In] unsigned int* pmdFire,[In] unsigned int* rmdOtherMethod,[In] unsigned int cMax,[In] unsigned int* pcOtherMethod)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::GetEventProps</unmanaged-short>
        public unsafe void GetEventProps(System.UInt32 ev, System.UInt32 classRef, System.String szEvent, System.UInt32 cchEvent, System.UInt32 chEventRef, System.UInt32 dwEventFlagsRef, System.UInt32 tkEventTypeRef, System.UInt32 mdAddOnRef, System.UInt32 mdRemoveOnRef, System.UInt32 mdFireRef, System.UInt32 rmdOtherMethod, System.UInt32 cMax, System.UInt32 cOtherMethodRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szEvent_ = szEvent)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, ev, (void *)(&classRef), (void *)((void *)szEvent_), cchEvent, (void *)(&chEventRef), (void *)(&dwEventFlagsRef), (void *)(&tkEventTypeRef), (void *)(&mdAddOnRef), (void *)(&mdRemoveOnRef), (void *)(&mdFireRef), (void *)(&rmdOtherMethod), cMax, (void *)(&cOtherMethodRef), (*(void ***)this._nativePointer)[34]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hEnumRef">No documentation.</param>
        /// <param name = "mb">No documentation.</param>
        /// <param name = "rEventProp">No documentation.</param>
        /// <param name = "cMax">No documentation.</param>
        /// <param name = "cEventPropRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::EnumMethodSemantics([In] void** phEnum,[In] unsigned int mb,[In] unsigned int* rEventProp,[In] unsigned int cMax,[In] unsigned int* pcEventProp)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::EnumMethodSemantics</unmanaged-short>
        public unsafe void EnumMethodSemantics(System.IntPtr hEnumRef, System.UInt32 mb, System.UInt32 rEventProp, System.UInt32 cMax, System.UInt32 cEventPropRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)hEnumRef), mb, (void *)(&rEventProp), cMax, (void *)(&cEventPropRef), (*(void ***)this._nativePointer)[35]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "mb">No documentation.</param>
        /// <param name = "tkEventProp">No documentation.</param>
        /// <param name = "dwSemanticsFlagsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::GetMethodSemantics([In] unsigned int mb,[In] unsigned int tkEventProp,[In] unsigned int* pdwSemanticsFlags)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::GetMethodSemantics</unmanaged-short>
        public unsafe void GetMethodSemantics(System.UInt32 mb, System.UInt32 tkEventProp, System.UInt32 dwSemanticsFlagsRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, mb, tkEventProp, (void *)(&dwSemanticsFlagsRef), (*(void ***)this._nativePointer)[36]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "td">No documentation.</param>
        /// <param name = "dwPackSizeRef">No documentation.</param>
        /// <param name = "rFieldOffset">No documentation.</param>
        /// <param name = "cMax">No documentation.</param>
        /// <param name = "cFieldOffsetRef">No documentation.</param>
        /// <param name = "ulClassSizeRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::GetClassLayout([In] unsigned int td,[In] unsigned int* pdwPackSize,[In] COR_FIELD_OFFSET* rFieldOffset,[In] unsigned int cMax,[In] unsigned int* pcFieldOffset,[In] unsigned int* pulClassSize)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::GetClassLayout</unmanaged-short>
        public unsafe void GetClassLayout(System.UInt32 td, System.UInt32 dwPackSizeRef, CorApi.Portable.CorFieldOffset rFieldOffset, System.UInt32 cMax, System.UInt32 cFieldOffsetRef, System.UInt32 ulClassSizeRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, td, (void *)(&dwPackSizeRef), (void *)(&rFieldOffset), cMax, (void *)(&cFieldOffsetRef), (void *)(&ulClassSizeRef), (*(void ***)this._nativePointer)[37]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tk">No documentation.</param>
        /// <param name = "vNativeTypeOut">No documentation.</param>
        /// <param name = "cbNativeTypeRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::GetFieldMarshal([In] unsigned int tk,[In] const unsigned char** ppvNativeType,[In] unsigned int* pcbNativeType)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::GetFieldMarshal</unmanaged-short>
        public unsafe void GetFieldMarshal(System.UInt32 tk, System.Byte vNativeTypeOut, System.UInt32 cbNativeTypeRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, tk, (void *)(&vNativeTypeOut), (void *)(&cbNativeTypeRef), (*(void ***)this._nativePointer)[38]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tk">No documentation.</param>
        /// <param name = "ulCodeRVARef">No documentation.</param>
        /// <param name = "dwImplFlagsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::GetRVA([In] unsigned int tk,[In] unsigned int* pulCodeRVA,[In] unsigned int* pdwImplFlags)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::GetRVA</unmanaged-short>
        public unsafe void GetRVA(System.UInt32 tk, System.UInt32 ulCodeRVARef, System.UInt32 dwImplFlagsRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, tk, (void *)(&ulCodeRVARef), (void *)(&dwImplFlagsRef), (*(void ***)this._nativePointer)[39]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "pm">No documentation.</param>
        /// <param name = "dwActionRef">No documentation.</param>
        /// <param name = "vPermissionOut">No documentation.</param>
        /// <param name = "cbPermissionRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::GetPermissionSetProps([In] unsigned int pm,[In] unsigned int* pdwAction,[In] const void** ppvPermission,[In] unsigned int* pcbPermission)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::GetPermissionSetProps</unmanaged-short>
        public unsafe void GetPermissionSetProps(System.UInt32 pm, System.UInt32 dwActionRef, System.IntPtr vPermissionOut, System.UInt32 cbPermissionRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, pm, (void *)(&dwActionRef), (void *)((void *)vPermissionOut), (void *)(&cbPermissionRef), (*(void ***)this._nativePointer)[40]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "mdSig">No documentation.</param>
        /// <param name = "vSigOut">No documentation.</param>
        /// <param name = "cbSigRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::GetSigFromToken([In] unsigned int mdSig,[In] const unsigned char** ppvSig,[In] unsigned int* pcbSig)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::GetSigFromToken</unmanaged-short>
        public unsafe void GetSigFromToken(System.UInt32 mdSig, System.Byte vSigOut, System.UInt32 cbSigRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, mdSig, (void *)(&vSigOut), (void *)(&cbSigRef), (*(void ***)this._nativePointer)[41]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "mur">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "cchName">No documentation.</param>
        /// <param name = "chNameRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::GetModuleRefProps([In] unsigned int mur,[Out, Buffer, Optional] wchar_t* szName,[In] unsigned int cchName,[In] unsigned int* pchName)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::GetModuleRefProps</unmanaged-short>
        public unsafe void GetModuleRefProps(System.UInt32 mur, System.IntPtr szName, System.UInt32 cchName, System.UInt32 chNameRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, mur, (void *)((void *)szName), cchName, (void *)(&chNameRef), (*(void ***)this._nativePointer)[42]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hEnumRef">No documentation.</param>
        /// <param name = "rModuleRefs">No documentation.</param>
        /// <param name = "cmax">No documentation.</param>
        /// <param name = "cModuleRefsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::EnumModuleRefs([In] void** phEnum,[In] unsigned int* rModuleRefs,[In] unsigned int cmax,[In] unsigned int* pcModuleRefs)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::EnumModuleRefs</unmanaged-short>
        public unsafe void EnumModuleRefs(System.IntPtr hEnumRef, System.UInt32 rModuleRefs, System.UInt32 cmax, System.UInt32 cModuleRefsRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)hEnumRef), (void *)(&rModuleRefs), cmax, (void *)(&cModuleRefsRef), (*(void ***)this._nativePointer)[43]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "typespec">No documentation.</param>
        /// <param name = "vSigOut">No documentation.</param>
        /// <param name = "cbSigRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::GetTypeSpecFromToken([In] unsigned int typespec,[In] const unsigned char** ppvSig,[In] unsigned int* pcbSig)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::GetTypeSpecFromToken</unmanaged-short>
        public unsafe void GetTypeSpecFromToken(System.UInt32 typespec, System.Byte vSigOut, System.UInt32 cbSigRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, typespec, (void *)(&vSigOut), (void *)(&cbSigRef), (*(void ***)this._nativePointer)[44]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tk">No documentation.</param>
        /// <param name = "szUtf8NamePtrRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::GetNameFromToken([In] unsigned int tk,[In] const char** pszUtf8NamePtr)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::GetNameFromToken</unmanaged-short>
        public unsafe void GetNameFromToken(System.UInt32 tk, System.String szUtf8NamePtrRef)
        {
            System.IntPtr szUtf8NamePtrRef_ = System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(szUtf8NamePtrRef);
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, tk, (void *)((void *)szUtf8NamePtrRef_), (*(void ***)this._nativePointer)[45]);
            System.Runtime.InteropServices.Marshal.FreeHGlobal(szUtf8NamePtrRef_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hEnumRef">No documentation.</param>
        /// <param name = "rMethods">No documentation.</param>
        /// <param name = "cMax">No documentation.</param>
        /// <param name = "cTokensRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::EnumUnresolvedMethods([In] void** phEnum,[In] unsigned int* rMethods,[In] unsigned int cMax,[In] unsigned int* pcTokens)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::EnumUnresolvedMethods</unmanaged-short>
        public unsafe void EnumUnresolvedMethods(System.IntPtr hEnumRef, System.UInt32 rMethods, System.UInt32 cMax, System.UInt32 cTokensRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)hEnumRef), (void *)(&rMethods), cMax, (void *)(&cTokensRef), (*(void ***)this._nativePointer)[46]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "stk">No documentation.</param>
        /// <param name = "szString">No documentation.</param>
        /// <param name = "cchString">No documentation.</param>
        /// <param name = "chStringRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::GetUserString([In] unsigned int stk,[Out] wchar_t* szString,[In] unsigned int cchString,[Out] unsigned int* pchString)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::GetUserString</unmanaged-short>
        public unsafe void GetUserString(System.UInt32 stk, System.IntPtr szString, System.UInt32 cchString, out System.UInt32 chStringRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *chStringRef_ = &chStringRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, stk, (void *)((void *)szString), cchString, (void *)(chStringRef_), (*(void ***)this._nativePointer)[47]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tk">No documentation.</param>
        /// <param name = "dwMappingFlagsRef">No documentation.</param>
        /// <param name = "szImportName">No documentation.</param>
        /// <param name = "cchImportName">No documentation.</param>
        /// <param name = "chImportNameRef">No documentation.</param>
        /// <param name = "mrImportDLLRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::GetPinvokeMap([In] unsigned int tk,[In] unsigned int* pdwMappingFlags,[Out, Buffer, Optional] wchar_t* szImportName,[In] unsigned int cchImportName,[In] unsigned int* pchImportName,[In] unsigned int* pmrImportDLL)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::GetPinvokeMap</unmanaged-short>
        public unsafe void GetPinvokeMap(System.UInt32 tk, System.UInt32 dwMappingFlagsRef, System.IntPtr szImportName, System.UInt32 cchImportName, System.UInt32 chImportNameRef, System.UInt32 mrImportDLLRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, tk, (void *)(&dwMappingFlagsRef), (void *)((void *)szImportName), cchImportName, (void *)(&chImportNameRef), (void *)(&mrImportDLLRef), (*(void ***)this._nativePointer)[48]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hEnumRef">No documentation.</param>
        /// <param name = "rSignatures">No documentation.</param>
        /// <param name = "cmax">No documentation.</param>
        /// <param name = "cSignaturesRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::EnumSignatures([In] void** phEnum,[In] unsigned int* rSignatures,[In] unsigned int cmax,[In] unsigned int* pcSignatures)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::EnumSignatures</unmanaged-short>
        public unsafe void EnumSignatures(System.IntPtr hEnumRef, System.UInt32 rSignatures, System.UInt32 cmax, System.UInt32 cSignaturesRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)hEnumRef), (void *)(&rSignatures), cmax, (void *)(&cSignaturesRef), (*(void ***)this._nativePointer)[49]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hEnumRef">No documentation.</param>
        /// <param name = "rTypeSpecs">No documentation.</param>
        /// <param name = "cmax">No documentation.</param>
        /// <param name = "cTypeSpecsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::EnumTypeSpecs([In] void** phEnum,[In] unsigned int* rTypeSpecs,[In] unsigned int cmax,[In] unsigned int* pcTypeSpecs)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::EnumTypeSpecs</unmanaged-short>
        public unsafe void EnumTypeSpecs(System.IntPtr hEnumRef, System.UInt32 rTypeSpecs, System.UInt32 cmax, System.UInt32 cTypeSpecsRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)hEnumRef), (void *)(&rTypeSpecs), cmax, (void *)(&cTypeSpecsRef), (*(void ***)this._nativePointer)[50]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hEnumRef">No documentation.</param>
        /// <param name = "rStrings">No documentation.</param>
        /// <param name = "cmax">No documentation.</param>
        /// <param name = "cStringsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::EnumUserStrings([In] void** phEnum,[In] unsigned int* rStrings,[In] unsigned int cmax,[In] unsigned int* pcStrings)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::EnumUserStrings</unmanaged-short>
        public unsafe void EnumUserStrings(System.IntPtr hEnumRef, System.UInt32 rStrings, System.UInt32 cmax, System.UInt32 cStringsRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)hEnumRef), (void *)(&rStrings), cmax, (void *)(&cStringsRef), (*(void ***)this._nativePointer)[51]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "md">No documentation.</param>
        /// <param name = "ulParamSeq">No documentation.</param>
        /// <param name = "dOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::GetParamForMethodIndex([In] unsigned int md,[In] unsigned int ulParamSeq,[In] unsigned int* ppd)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::GetParamForMethodIndex</unmanaged-short>
        public unsafe void GetParamForMethodIndex(System.UInt32 md, System.UInt32 ulParamSeq, System.UInt32 dOut)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, md, ulParamSeq, (void *)(&dOut), (*(void ***)this._nativePointer)[52]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hEnumRef">No documentation.</param>
        /// <param name = "tk">No documentation.</param>
        /// <param name = "tkType">No documentation.</param>
        /// <param name = "rCustomAttributes">No documentation.</param>
        /// <param name = "cMax">No documentation.</param>
        /// <param name = "cCustomAttributesRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::EnumCustomAttributes([In] void** phEnum,[In] unsigned int tk,[In] unsigned int tkType,[In] unsigned int* rCustomAttributes,[In] unsigned int cMax,[In] unsigned int* pcCustomAttributes)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::EnumCustomAttributes</unmanaged-short>
        public unsafe void EnumCustomAttributes(System.IntPtr hEnumRef, System.UInt32 tk, System.UInt32 tkType, System.UInt32 rCustomAttributes, System.UInt32 cMax, System.UInt32 cCustomAttributesRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)hEnumRef), tk, tkType, (void *)(&rCustomAttributes), cMax, (void *)(&cCustomAttributesRef), (*(void ***)this._nativePointer)[53]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cv">No documentation.</param>
        /// <param name = "tkObjRef">No documentation.</param>
        /// <param name = "tkTypeRef">No documentation.</param>
        /// <param name = "blobOut">No documentation.</param>
        /// <param name = "cbSizeRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::GetCustomAttributeProps([In] unsigned int cv,[In] unsigned int* ptkObj,[In] unsigned int* ptkType,[In] const void** ppBlob,[In] unsigned int* pcbSize)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::GetCustomAttributeProps</unmanaged-short>
        public unsafe void GetCustomAttributeProps(System.UInt32 cv, System.UInt32 tkObjRef, System.UInt32 tkTypeRef, System.IntPtr blobOut, System.UInt32 cbSizeRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cv, (void *)(&tkObjRef), (void *)(&tkTypeRef), (void *)((void *)blobOut), (void *)(&cbSizeRef), (*(void ***)this._nativePointer)[54]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tkResolutionScope">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "trRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::FindTypeRef([In] unsigned int tkResolutionScope,[In] const wchar_t* szName,[In] unsigned int* ptr)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::FindTypeRef</unmanaged-short>
        public unsafe void FindTypeRef(System.UInt32 tkResolutionScope, System.String szName, System.UInt32 trRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, tkResolutionScope, (void *)((void *)szName_), (void *)(&trRef), (*(void ***)this._nativePointer)[55]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "mb">No documentation.</param>
        /// <param name = "classRef">No documentation.</param>
        /// <param name = "szMember">No documentation.</param>
        /// <param name = "cchMember">No documentation.</param>
        /// <param name = "chMemberRef">No documentation.</param>
        /// <param name = "dwAttrRef">No documentation.</param>
        /// <param name = "vSigBlobOut">No documentation.</param>
        /// <param name = "cbSigBlobRef">No documentation.</param>
        /// <param name = "ulCodeRVARef">No documentation.</param>
        /// <param name = "dwImplFlagsRef">No documentation.</param>
        /// <param name = "dwCPlusTypeFlagRef">No documentation.</param>
        /// <param name = "valueOut">No documentation.</param>
        /// <param name = "cchValueRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::GetMemberProps([In] unsigned int mb,[In] unsigned int* pClass,[Out, Buffer, Optional] wchar_t* szMember,[In] unsigned int cchMember,[In] unsigned int* pchMember,[In] unsigned int* pdwAttr,[In] const unsigned char** ppvSigBlob,[In] unsigned int* pcbSigBlob,[In] unsigned int* pulCodeRVA,[In] unsigned int* pdwImplFlags,[In] unsigned int* pdwCPlusTypeFlag,[In] const void** ppValue,[In] unsigned int* pcchValue)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::GetMemberProps</unmanaged-short>
        public unsafe void GetMemberProps(System.UInt32 mb, System.UInt32 classRef, System.IntPtr szMember, System.UInt32 cchMember, System.UInt32 chMemberRef, System.UInt32 dwAttrRef, System.Byte vSigBlobOut, System.UInt32 cbSigBlobRef, System.UInt32 ulCodeRVARef, System.UInt32 dwImplFlagsRef, System.UInt32 dwCPlusTypeFlagRef, System.IntPtr valueOut, System.UInt32 cchValueRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, mb, (void *)(&classRef), (void *)((void *)szMember), cchMember, (void *)(&chMemberRef), (void *)(&dwAttrRef), (void *)(&vSigBlobOut), (void *)(&cbSigBlobRef), (void *)(&ulCodeRVARef), (void *)(&dwImplFlagsRef), (void *)(&dwCPlusTypeFlagRef), (void *)((void *)valueOut), (void *)(&cchValueRef), (*(void ***)this._nativePointer)[56]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "mb">No documentation.</param>
        /// <param name = "classRef">No documentation.</param>
        /// <param name = "szField">No documentation.</param>
        /// <param name = "cchField">No documentation.</param>
        /// <param name = "chFieldRef">No documentation.</param>
        /// <param name = "dwAttrRef">No documentation.</param>
        /// <param name = "vSigBlobOut">No documentation.</param>
        /// <param name = "cbSigBlobRef">No documentation.</param>
        /// <param name = "dwCPlusTypeFlagRef">No documentation.</param>
        /// <param name = "valueOut">No documentation.</param>
        /// <param name = "cchValueRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::GetFieldProps([In] unsigned int mb,[Out] unsigned int* pClass,[Out] wchar_t* szField,[In] unsigned int cchField,[Out] unsigned int* pchField,[Out] unsigned int* pdwAttr,[Out] const unsigned char** ppvSigBlob,[Out] unsigned int* pcbSigBlob,[Out] unsigned int* pdwCPlusTypeFlag,[Out] const void** ppValue,[Out] unsigned int* pcchValue)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::GetFieldProps</unmanaged-short>
        public unsafe void GetFieldProps(System.UInt32 mb, out System.UInt32 classRef, System.IntPtr szField, System.UInt32 cchField, out System.UInt32 chFieldRef, out System.UInt32 dwAttrRef, out System.IntPtr vSigBlobOut, out System.UInt32 cbSigBlobRef, out System.UInt32 dwCPlusTypeFlagRef, out System.IntPtr valueOut, out System.UInt32 cchValueRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *cchValueRef_ = &cchValueRef)
                fixed (void *valueOut_ = &valueOut)
                    fixed (void *dwCPlusTypeFlagRef_ = &dwCPlusTypeFlagRef)
                        fixed (void *cbSigBlobRef_ = &cbSigBlobRef)
                            fixed (void *vSigBlobOut_ = &vSigBlobOut)
                                fixed (void *dwAttrRef_ = &dwAttrRef)
                                    fixed (void *chFieldRef_ = &chFieldRef)
                                        fixed (void *classRef_ = &classRef)
                                            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, mb, (void *)(classRef_), (void *)((void *)szField), cchField, (void *)(chFieldRef_), (void *)(dwAttrRef_), (void *)(vSigBlobOut_), (void *)(cbSigBlobRef_), (void *)(dwCPlusTypeFlagRef_), (void *)(valueOut_), (void *)(cchValueRef_), (*(void ***)this._nativePointer)[57]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "prop">No documentation.</param>
        /// <param name = "classRef">No documentation.</param>
        /// <param name = "szProperty">No documentation.</param>
        /// <param name = "cchProperty">No documentation.</param>
        /// <param name = "chPropertyRef">No documentation.</param>
        /// <param name = "dwPropFlagsRef">No documentation.</param>
        /// <param name = "vSigOut">No documentation.</param>
        /// <param name = "bSigRef">No documentation.</param>
        /// <param name = "dwCPlusTypeFlagRef">No documentation.</param>
        /// <param name = "defaultValueOut">No documentation.</param>
        /// <param name = "cchDefaultValueRef">No documentation.</param>
        /// <param name = "mdSetterRef">No documentation.</param>
        /// <param name = "mdGetterRef">No documentation.</param>
        /// <param name = "rmdOtherMethod">No documentation.</param>
        /// <param name = "cMax">No documentation.</param>
        /// <param name = "cOtherMethodRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::GetPropertyProps([In] unsigned int prop,[Out] unsigned int* pClass,[Out] const wchar_t* szProperty,[In] unsigned int cchProperty,[Out] unsigned int* pchProperty,[Out] unsigned int* pdwPropFlags,[Out] const unsigned char** ppvSig,[Out] unsigned int* pbSig,[Out] unsigned int* pdwCPlusTypeFlag,[Out] const void** ppDefaultValue,[Out] unsigned int* pcchDefaultValue,[Out] unsigned int* pmdSetter,[Out] unsigned int* pmdGetter,[Out, Buffer] unsigned int* rmdOtherMethod,[In] unsigned int cMax,[Out] unsigned int* pcOtherMethod)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::GetPropertyProps</unmanaged-short>
        public unsafe void GetPropertyProps(System.UInt32 prop, out System.UInt32 classRef, System.IntPtr szProperty, System.UInt32 cchProperty, out System.UInt32 chPropertyRef, out System.UInt32 dwPropFlagsRef, out System.IntPtr vSigOut, out System.UInt32 bSigRef, out System.UInt32 dwCPlusTypeFlagRef, out System.IntPtr defaultValueOut, out System.UInt32 cchDefaultValueRef, out System.UInt32 mdSetterRef, out System.UInt32 mdGetterRef, System.UInt32[] rmdOtherMethod, System.UInt32 cMax, out System.UInt32 cOtherMethodRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *cOtherMethodRef_ = &cOtherMethodRef)
                fixed (void *rmdOtherMethod_ = rmdOtherMethod)
                    fixed (void *mdGetterRef_ = &mdGetterRef)
                        fixed (void *mdSetterRef_ = &mdSetterRef)
                            fixed (void *cchDefaultValueRef_ = &cchDefaultValueRef)
                                fixed (void *defaultValueOut_ = &defaultValueOut)
                                    fixed (void *dwCPlusTypeFlagRef_ = &dwCPlusTypeFlagRef)
                                        fixed (void *bSigRef_ = &bSigRef)
                                            fixed (void *vSigOut_ = &vSigOut)
                                                fixed (void *dwPropFlagsRef_ = &dwPropFlagsRef)
                                                    fixed (void *chPropertyRef_ = &chPropertyRef)
                                                        fixed (void *classRef_ = &classRef)
                                                            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, prop, (void *)(classRef_), (void *)((void *)szProperty), cchProperty, (void *)(chPropertyRef_), (void *)(dwPropFlagsRef_), (void *)(vSigOut_), (void *)(bSigRef_), (void *)(dwCPlusTypeFlagRef_), (void *)(defaultValueOut_), (void *)(cchDefaultValueRef_), (void *)(mdSetterRef_), (void *)(mdGetterRef_), (void *)(rmdOtherMethod_), cMax, (void *)(cOtherMethodRef_), (*(void ***)this._nativePointer)[58]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tk">No documentation.</param>
        /// <param name = "mdRef">No documentation.</param>
        /// <param name = "ulSequenceRef">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "cchName">No documentation.</param>
        /// <param name = "chNameRef">No documentation.</param>
        /// <param name = "dwAttrRef">No documentation.</param>
        /// <param name = "dwCPlusTypeFlagRef">No documentation.</param>
        /// <param name = "valueOut">No documentation.</param>
        /// <param name = "cchValueRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::GetParamProps([In] unsigned int tk,[Out] unsigned int* pmd,[Out] unsigned int* pulSequence,[Out] wchar_t* szName,[In] unsigned int cchName,[Out] unsigned int* pchName,[Out] unsigned int* pdwAttr,[Out] unsigned int* pdwCPlusTypeFlag,[Out] const void** ppValue,[Out] unsigned int* pcchValue)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::GetParamProps</unmanaged-short>
        public unsafe void GetParamProps(System.UInt32 tk, out System.UInt32 mdRef, out System.UInt32 ulSequenceRef, System.IntPtr szName, System.UInt32 cchName, out System.UInt32 chNameRef, out System.UInt32 dwAttrRef, out System.UInt32 dwCPlusTypeFlagRef, out System.IntPtr valueOut, out System.UInt32 cchValueRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *cchValueRef_ = &cchValueRef)
                fixed (void *valueOut_ = &valueOut)
                    fixed (void *dwCPlusTypeFlagRef_ = &dwCPlusTypeFlagRef)
                        fixed (void *dwAttrRef_ = &dwAttrRef)
                            fixed (void *chNameRef_ = &chNameRef)
                                fixed (void *ulSequenceRef_ = &ulSequenceRef)
                                    fixed (void *mdRef_ = &mdRef)
                                        __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, tk, (void *)(mdRef_), (void *)(ulSequenceRef_), (void *)((void *)szName), cchName, (void *)(chNameRef_), (void *)(dwAttrRef_), (void *)(dwCPlusTypeFlagRef_), (void *)(valueOut_), (void *)(cchValueRef_), (*(void ***)this._nativePointer)[59]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tkObj">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "dataOut">No documentation.</param>
        /// <param name = "cbDataRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::GetCustomAttributeByName([In] unsigned int tkObj,[In] const wchar_t* szName,[Out] const void** ppData,[Out] unsigned int* pcbData)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::GetCustomAttributeByName</unmanaged-short>
        public unsafe void GetCustomAttributeByName(System.UInt32 tkObj, System.String szName, out System.IntPtr dataOut, out System.UInt32 cbDataRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *cbDataRef_ = &cbDataRef)
                fixed (void *dataOut_ = &dataOut)
                    fixed (char *szName_ = szName)
                        __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, tkObj, (void *)((void *)szName_), (void *)(dataOut_), (void *)(cbDataRef_), (*(void ***)this._nativePointer)[60]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tk">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>BOOL IMetaDataImport::IsValidToken([In] unsigned int tk)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::IsValidToken</unmanaged-short>
        public unsafe SharpGen.Runtime.Win32.RawBool IsValidToken(System.UInt32 tk)
        {
            SharpGen.Runtime.Win32.RawBool __result__;
            __result__ = CorDebug.LocalInterop.CalliSharpGenRuntimeWin32RawBool(this._nativePointer, tk, (*(void ***)this._nativePointer)[61]);
            return __result__;
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tdNestedClass">No documentation.</param>
        /// <param name = "tdEnclosingClassRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::GetNestedClassProps([In] unsigned int tdNestedClass,[Out] unsigned int* ptdEnclosingClass)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::GetNestedClassProps</unmanaged-short>
        public unsafe void GetNestedClassProps(System.UInt32 tdNestedClass, out System.UInt32 tdEnclosingClassRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *tdEnclosingClassRef_ = &tdEnclosingClassRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, tdNestedClass, (void *)(tdEnclosingClassRef_), (*(void ***)this._nativePointer)[62]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "vSigRef">No documentation.</param>
        /// <param name = "cbSig">No documentation.</param>
        /// <param name = "callConvRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::GetNativeCallConvFromSig([In] const void* pvSig,[In] unsigned int cbSig,[In] unsigned int* pCallConv)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::GetNativeCallConvFromSig</unmanaged-short>
        public unsafe void GetNativeCallConvFromSig(System.IntPtr vSigRef, System.UInt32 cbSig, System.UInt32 callConvRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)vSigRef), cbSig, (void *)(&callConvRef), (*(void ***)this._nativePointer)[63]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "pd">No documentation.</param>
        /// <param name = "bGlobalRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport::IsGlobal([In] unsigned int pd,[In] int* pbGlobal)</unmanaged>
        /// <unmanaged-short>IMetaDataImport::IsGlobal</unmanaged-short>
        public unsafe void IsGlobal(System.UInt32 pd, System.Int32 bGlobalRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, pd, (void *)(&bGlobalRef), (*(void ***)this._nativePointer)[64]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("fce5efa0-8bba-4f8e-a036-8f2022b08466")]
    public partial class IMetaDataImport2 : CorApi.Portable.IMetaDataImport
    {
        public IMetaDataImport2(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator IMetaDataImport2(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new IMetaDataImport2(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hEnumRef">No documentation.</param>
        /// <param name = "tk">No documentation.</param>
        /// <param name = "rGenericParams">No documentation.</param>
        /// <param name = "cMax">No documentation.</param>
        /// <param name = "cGenericParamsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport2::EnumGenericParams([Out] void** phEnum,[In] unsigned int tk,[Out, Buffer] unsigned int* rGenericParams,[In] unsigned int cMax,[Out] unsigned int* pcGenericParams)</unmanaged>
        /// <unmanaged-short>IMetaDataImport2::EnumGenericParams</unmanaged-short>
        public unsafe void EnumGenericParams(out System.IntPtr hEnumRef, System.UInt32 tk, System.UInt32[] rGenericParams, System.UInt32 cMax, out System.UInt32 cGenericParamsRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *cGenericParamsRef_ = &cGenericParamsRef)
                fixed (void *rGenericParams_ = rGenericParams)
                    fixed (void *hEnumRef_ = &hEnumRef)
                        __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(hEnumRef_), tk, (void *)(rGenericParams_), cMax, (void *)(cGenericParamsRef_), (*(void ***)this._nativePointer)[65]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "gp">No documentation.</param>
        /// <param name = "ulParamSeqRef">No documentation.</param>
        /// <param name = "dwParamFlagsRef">No documentation.</param>
        /// <param name = "tOwnerRef">No documentation.</param>
        /// <param name = "reserved">No documentation.</param>
        /// <param name = "wzname">No documentation.</param>
        /// <param name = "cchName">No documentation.</param>
        /// <param name = "chNameRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport2::GetGenericParamProps([In] unsigned int gp,[Out] unsigned int* pulParamSeq,[Out] unsigned int* pdwParamFlags,[Out] unsigned int* ptOwner,[Out] unsigned int* reserved,[Out] wchar_t* wzname,[In] unsigned int cchName,[Out] unsigned int* pchName)</unmanaged>
        /// <unmanaged-short>IMetaDataImport2::GetGenericParamProps</unmanaged-short>
        public unsafe void GetGenericParamProps(System.UInt32 gp, out System.UInt32 ulParamSeqRef, out System.UInt32 dwParamFlagsRef, out System.UInt32 tOwnerRef, out System.UInt32 reserved, System.IntPtr wzname, System.UInt32 cchName, out System.UInt32 chNameRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *chNameRef_ = &chNameRef)
                fixed (void *reserved_ = &reserved)
                    fixed (void *tOwnerRef_ = &tOwnerRef)
                        fixed (void *dwParamFlagsRef_ = &dwParamFlagsRef)
                            fixed (void *ulParamSeqRef_ = &ulParamSeqRef)
                                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, gp, (void *)(ulParamSeqRef_), (void *)(dwParamFlagsRef_), (void *)(tOwnerRef_), (void *)(reserved_), (void *)((void *)wzname), cchName, (void *)(chNameRef_), (*(void ***)this._nativePointer)[66]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "mi">No documentation.</param>
        /// <param name = "tkParent">No documentation.</param>
        /// <param name = "vSigBlobOut">No documentation.</param>
        /// <param name = "cbSigBlobRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport2::GetMethodSpecProps([In] unsigned int mi,[In] unsigned int* tkParent,[In] const unsigned char** ppvSigBlob,[In] unsigned int* pcbSigBlob)</unmanaged>
        /// <unmanaged-short>IMetaDataImport2::GetMethodSpecProps</unmanaged-short>
        public unsafe void GetMethodSpecProps(System.UInt32 mi, System.UInt32 tkParent, System.Byte vSigBlobOut, System.UInt32 cbSigBlobRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, mi, (void *)(&tkParent), (void *)(&vSigBlobOut), (void *)(&cbSigBlobRef), (*(void ***)this._nativePointer)[67]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hEnumRef">No documentation.</param>
        /// <param name = "tk">No documentation.</param>
        /// <param name = "rGenericParamConstraints">No documentation.</param>
        /// <param name = "cMax">No documentation.</param>
        /// <param name = "cGenericParamConstraintsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport2::EnumGenericParamConstraints([In] void** phEnum,[In] unsigned int tk,[In] unsigned int* rGenericParamConstraints,[In] unsigned int cMax,[In] unsigned int* pcGenericParamConstraints)</unmanaged>
        /// <unmanaged-short>IMetaDataImport2::EnumGenericParamConstraints</unmanaged-short>
        public unsafe void EnumGenericParamConstraints(System.IntPtr hEnumRef, System.UInt32 tk, System.UInt32 rGenericParamConstraints, System.UInt32 cMax, System.UInt32 cGenericParamConstraintsRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)hEnumRef), tk, (void *)(&rGenericParamConstraints), cMax, (void *)(&cGenericParamConstraintsRef), (*(void ***)this._nativePointer)[68]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "gpc">No documentation.</param>
        /// <param name = "tGenericParamRef">No documentation.</param>
        /// <param name = "tkConstraintTypeRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport2::GetGenericParamConstraintProps([In] unsigned int gpc,[In] unsigned int* ptGenericParam,[In] unsigned int* ptkConstraintType)</unmanaged>
        /// <unmanaged-short>IMetaDataImport2::GetGenericParamConstraintProps</unmanaged-short>
        public unsafe void GetGenericParamConstraintProps(System.UInt32 gpc, System.UInt32 tGenericParamRef, System.UInt32 tkConstraintTypeRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, gpc, (void *)(&tGenericParamRef), (void *)(&tkConstraintTypeRef), (*(void ***)this._nativePointer)[69]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "dwPEKindRef">No documentation.</param>
        /// <param name = "dwMAchineRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport2::GetPEKind([In] unsigned int* pdwPEKind,[In] unsigned int* pdwMAchine)</unmanaged>
        /// <unmanaged-short>IMetaDataImport2::GetPEKind</unmanaged-short>
        public unsafe void GetPEKind(System.UInt32 dwPEKindRef, System.UInt32 dwMAchineRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&dwPEKindRef), (void *)(&dwMAchineRef), (*(void ***)this._nativePointer)[70]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "wzBufRef">No documentation.</param>
        /// <param name = "ccBufSize">No documentation.</param>
        /// <param name = "ccBufSizeRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport2::GetVersionString([Out, Buffer, Optional] wchar_t* pwzBuf,[In] unsigned int ccBufSize,[In] unsigned int* pccBufSize)</unmanaged>
        /// <unmanaged-short>IMetaDataImport2::GetVersionString</unmanaged-short>
        public unsafe void GetVersionString(System.IntPtr wzBufRef, System.UInt32 ccBufSize, System.UInt32 ccBufSizeRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)wzBufRef), ccBufSize, (void *)(&ccBufSizeRef), (*(void ***)this._nativePointer)[71]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hEnumRef">No documentation.</param>
        /// <param name = "tk">No documentation.</param>
        /// <param name = "rMethodSpecs">No documentation.</param>
        /// <param name = "cMax">No documentation.</param>
        /// <param name = "cMethodSpecsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataImport2::EnumMethodSpecs([In] void** phEnum,[In] unsigned int tk,[In] unsigned int* rMethodSpecs,[In] unsigned int cMax,[In] unsigned int* pcMethodSpecs)</unmanaged>
        /// <unmanaged-short>IMetaDataImport2::EnumMethodSpecs</unmanaged-short>
        public unsafe void EnumMethodSpecs(System.IntPtr hEnumRef, System.UInt32 tk, System.UInt32 rMethodSpecs, System.UInt32 cMax, System.UInt32 cMethodSpecsRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)hEnumRef), tk, (void *)(&rMethodSpecs), cMax, (void *)(&cMethodSpecsRef), (*(void ***)this._nativePointer)[72]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("7998ea64-7f95-48b8-86fc-17caf48bf5cb")]
    public partial class IMetaDataInfo : SharpGen.Runtime.ComObject
    {
        public IMetaDataInfo(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator IMetaDataInfo(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new IMetaDataInfo(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "vDataOut">No documentation.</param>
        /// <param name = "cbDataRef">No documentation.</param>
        /// <param name = "dwMappingTypeRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataInfo::GetFileMapping([In] const void** ppvData,[In] unsigned longlong* pcbData,[In] unsigned int* pdwMappingType)</unmanaged>
        /// <unmanaged-short>IMetaDataInfo::GetFileMapping</unmanaged-short>
        public unsafe void GetFileMapping(System.IntPtr vDataOut, System.UInt64 cbDataRef, System.UInt32 dwMappingTypeRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)vDataOut), (void *)(&cbDataRef), (void *)(&dwMappingTypeRef), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("d8f579ab-402d-4b8e-82d9-5d63b1065c68")]
    public partial class IMetaDataTables : SharpGen.Runtime.ComObject
    {
        public IMetaDataTables(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator IMetaDataTables(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new IMetaDataTables(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cbStringsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataTables::GetStringHeapSize([In] unsigned int* pcbStrings)</unmanaged>
        /// <unmanaged-short>IMetaDataTables::GetStringHeapSize</unmanaged-short>
        public unsafe void GetStringHeapSize(System.UInt32 cbStringsRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&cbStringsRef), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cbBlobsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataTables::GetBlobHeapSize([In] unsigned int* pcbBlobs)</unmanaged>
        /// <unmanaged-short>IMetaDataTables::GetBlobHeapSize</unmanaged-short>
        public unsafe void GetBlobHeapSize(System.UInt32 cbBlobsRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&cbBlobsRef), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cbGuidsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataTables::GetGuidHeapSize([In] unsigned int* pcbGuids)</unmanaged>
        /// <unmanaged-short>IMetaDataTables::GetGuidHeapSize</unmanaged-short>
        public unsafe void GetGuidHeapSize(System.UInt32 cbGuidsRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&cbGuidsRef), (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cbBlobsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataTables::GetUserStringHeapSize([In] unsigned int* pcbBlobs)</unmanaged>
        /// <unmanaged-short>IMetaDataTables::GetUserStringHeapSize</unmanaged-short>
        public unsafe void GetUserStringHeapSize(System.UInt32 cbBlobsRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&cbBlobsRef), (*(void ***)this._nativePointer)[6]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cTablesRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataTables::GetNumTables([In] unsigned int* pcTables)</unmanaged>
        /// <unmanaged-short>IMetaDataTables::GetNumTables</unmanaged-short>
        public unsafe void GetNumTables(System.UInt32 cTablesRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&cTablesRef), (*(void ***)this._nativePointer)[7]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "token">No documentation.</param>
        /// <param name = "ixTblRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataTables::GetTableIndex([In] unsigned int token,[In] unsigned int* pixTbl)</unmanaged>
        /// <unmanaged-short>IMetaDataTables::GetTableIndex</unmanaged-short>
        public unsafe void GetTableIndex(System.UInt32 token, System.UInt32 ixTblRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, token, (void *)(&ixTblRef), (*(void ***)this._nativePointer)[8]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "ixTbl">No documentation.</param>
        /// <param name = "cbRowRef">No documentation.</param>
        /// <param name = "cRowsRef">No documentation.</param>
        /// <param name = "cColsRef">No documentation.</param>
        /// <param name = "iKeyRef">No documentation.</param>
        /// <param name = "nameOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataTables::GetTableInfo([In] unsigned int ixTbl,[In] unsigned int* pcbRow,[In] unsigned int* pcRows,[In] unsigned int* pcCols,[In] unsigned int* piKey,[In] const char** ppName)</unmanaged>
        /// <unmanaged-short>IMetaDataTables::GetTableInfo</unmanaged-short>
        public unsafe void GetTableInfo(System.UInt32 ixTbl, System.UInt32 cbRowRef, System.UInt32 cRowsRef, System.UInt32 cColsRef, System.UInt32 iKeyRef, System.String nameOut)
        {
            System.IntPtr nameOut_ = System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(nameOut);
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, ixTbl, (void *)(&cbRowRef), (void *)(&cRowsRef), (void *)(&cColsRef), (void *)(&iKeyRef), (void *)((void *)nameOut_), (*(void ***)this._nativePointer)[9]);
            System.Runtime.InteropServices.Marshal.FreeHGlobal(nameOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "ixTbl">No documentation.</param>
        /// <param name = "ixCol">No documentation.</param>
        /// <param name = "oColRef">No documentation.</param>
        /// <param name = "cbColRef">No documentation.</param>
        /// <param name = "typeRef">No documentation.</param>
        /// <param name = "nameOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataTables::GetColumnInfo([In] unsigned int ixTbl,[In] unsigned int ixCol,[In] unsigned int* poCol,[In] unsigned int* pcbCol,[In] unsigned int* pType,[In] const char** ppName)</unmanaged>
        /// <unmanaged-short>IMetaDataTables::GetColumnInfo</unmanaged-short>
        public unsafe void GetColumnInfo(System.UInt32 ixTbl, System.UInt32 ixCol, System.UInt32 oColRef, System.UInt32 cbColRef, System.UInt32 typeRef, System.String nameOut)
        {
            System.IntPtr nameOut_ = System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(nameOut);
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, ixTbl, ixCol, (void *)(&oColRef), (void *)(&cbColRef), (void *)(&typeRef), (void *)((void *)nameOut_), (*(void ***)this._nativePointer)[10]);
            System.Runtime.InteropServices.Marshal.FreeHGlobal(nameOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "ixCdTkn">No documentation.</param>
        /// <param name = "cTokensRef">No documentation.</param>
        /// <param name = "tokensOut">No documentation.</param>
        /// <param name = "nameOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataTables::GetCodedTokenInfo([In] unsigned int ixCdTkn,[In] unsigned int* pcTokens,[In] unsigned int** ppTokens,[In] const char** ppName)</unmanaged>
        /// <unmanaged-short>IMetaDataTables::GetCodedTokenInfo</unmanaged-short>
        public unsafe void GetCodedTokenInfo(System.UInt32 ixCdTkn, System.UInt32 cTokensRef, System.UInt32 tokensOut, System.String nameOut)
        {
            System.IntPtr nameOut_ = System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(nameOut);
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, ixCdTkn, (void *)(&cTokensRef), (void *)(&tokensOut), (void *)((void *)nameOut_), (*(void ***)this._nativePointer)[11]);
            System.Runtime.InteropServices.Marshal.FreeHGlobal(nameOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "ixTbl">No documentation.</param>
        /// <param name = "rid">No documentation.</param>
        /// <param name = "rowOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataTables::GetRow([In] unsigned int ixTbl,[In] unsigned int rid,[In] void** ppRow)</unmanaged>
        /// <unmanaged-short>IMetaDataTables::GetRow</unmanaged-short>
        public unsafe void GetRow(System.UInt32 ixTbl, System.UInt32 rid, System.IntPtr rowOut)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, ixTbl, rid, (void *)((void *)rowOut), (*(void ***)this._nativePointer)[12]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "ixTbl">No documentation.</param>
        /// <param name = "ixCol">No documentation.</param>
        /// <param name = "rid">No documentation.</param>
        /// <param name = "valRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataTables::GetColumn([In] unsigned int ixTbl,[In] unsigned int ixCol,[In] unsigned int rid,[In] unsigned int* pVal)</unmanaged>
        /// <unmanaged-short>IMetaDataTables::GetColumn</unmanaged-short>
        public unsafe void GetColumn(System.UInt32 ixTbl, System.UInt32 ixCol, System.UInt32 rid, System.UInt32 valRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, ixTbl, ixCol, rid, (void *)(&valRef), (*(void ***)this._nativePointer)[13]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "ixString">No documentation.</param>
        /// <param name = "stringOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataTables::GetString([In] unsigned int ixString,[In] const char** ppString)</unmanaged>
        /// <unmanaged-short>IMetaDataTables::GetString</unmanaged-short>
        public unsafe void GetString(System.UInt32 ixString, System.String stringOut)
        {
            System.IntPtr stringOut_ = System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(stringOut);
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, ixString, (void *)((void *)stringOut_), (*(void ***)this._nativePointer)[14]);
            System.Runtime.InteropServices.Marshal.FreeHGlobal(stringOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "ixBlob">No documentation.</param>
        /// <param name = "cbDataRef">No documentation.</param>
        /// <param name = "dataOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataTables::GetBlob([In] unsigned int ixBlob,[In] unsigned int* pcbData,[In] const void** ppData)</unmanaged>
        /// <unmanaged-short>IMetaDataTables::GetBlob</unmanaged-short>
        public unsafe void GetBlob(System.UInt32 ixBlob, System.UInt32 cbDataRef, System.IntPtr dataOut)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, ixBlob, (void *)(&cbDataRef), (void *)((void *)dataOut), (*(void ***)this._nativePointer)[15]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "ixGuid">No documentation.</param>
        /// <param name = "gUIDOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataTables::GetGuid([In] unsigned int ixGuid,[In] const GUID** ppGUID)</unmanaged>
        /// <unmanaged-short>IMetaDataTables::GetGuid</unmanaged-short>
        public unsafe void GetGuid(System.UInt32 ixGuid, System.Guid gUIDOut)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, ixGuid, (void *)(&gUIDOut), (*(void ***)this._nativePointer)[16]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "ixUserString">No documentation.</param>
        /// <param name = "cbDataRef">No documentation.</param>
        /// <param name = "dataOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataTables::GetUserString([In] unsigned int ixUserString,[In] unsigned int* pcbData,[In] const void** ppData)</unmanaged>
        /// <unmanaged-short>IMetaDataTables::GetUserString</unmanaged-short>
        public unsafe void GetUserString(System.UInt32 ixUserString, System.UInt32 cbDataRef, System.IntPtr dataOut)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, ixUserString, (void *)(&cbDataRef), (void *)((void *)dataOut), (*(void ***)this._nativePointer)[17]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "ixString">No documentation.</param>
        /// <param name = "nextRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataTables::GetNextString([In] unsigned int ixString,[In] unsigned int* pNext)</unmanaged>
        /// <unmanaged-short>IMetaDataTables::GetNextString</unmanaged-short>
        public unsafe void GetNextString(System.UInt32 ixString, System.UInt32 nextRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, ixString, (void *)(&nextRef), (*(void ***)this._nativePointer)[18]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "ixBlob">No documentation.</param>
        /// <param name = "nextRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataTables::GetNextBlob([In] unsigned int ixBlob,[In] unsigned int* pNext)</unmanaged>
        /// <unmanaged-short>IMetaDataTables::GetNextBlob</unmanaged-short>
        public unsafe void GetNextBlob(System.UInt32 ixBlob, System.UInt32 nextRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, ixBlob, (void *)(&nextRef), (*(void ***)this._nativePointer)[19]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "ixGuid">No documentation.</param>
        /// <param name = "nextRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataTables::GetNextGuid([In] unsigned int ixGuid,[In] unsigned int* pNext)</unmanaged>
        /// <unmanaged-short>IMetaDataTables::GetNextGuid</unmanaged-short>
        public unsafe void GetNextGuid(System.UInt32 ixGuid, System.UInt32 nextRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, ixGuid, (void *)(&nextRef), (*(void ***)this._nativePointer)[20]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "ixUserString">No documentation.</param>
        /// <param name = "nextRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataTables::GetNextUserString([In] unsigned int ixUserString,[In] unsigned int* pNext)</unmanaged>
        /// <unmanaged-short>IMetaDataTables::GetNextUserString</unmanaged-short>
        public unsafe void GetNextUserString(System.UInt32 ixUserString, System.UInt32 nextRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, ixUserString, (void *)(&nextRef), (*(void ***)this._nativePointer)[21]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("badb5f70-58da-43a9-a1c6-d74819f19b15")]
    public partial class IMetaDataTables2 : CorApi.Portable.IMetaDataTables
    {
        public IMetaDataTables2(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator IMetaDataTables2(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new IMetaDataTables2(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "vMdOut">No documentation.</param>
        /// <param name = "cbMdRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataTables2::GetMetaDataStorage([In] const void** ppvMd,[In] unsigned int* pcbMd)</unmanaged>
        /// <unmanaged-short>IMetaDataTables2::GetMetaDataStorage</unmanaged-short>
        public unsafe void GetMetaDataStorage(System.IntPtr vMdOut, System.UInt32 cbMdRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)vMdOut), (void *)(&cbMdRef), (*(void ***)this._nativePointer)[22]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "ix">No documentation.</param>
        /// <param name = "chNameOut">No documentation.</param>
        /// <param name = "vOut">No documentation.</param>
        /// <param name = "cbRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataTables2::GetMetaDataStreamInfo([In] unsigned int ix,[In] const char** ppchName,[In] const void** ppv,[In] unsigned int* pcb)</unmanaged>
        /// <unmanaged-short>IMetaDataTables2::GetMetaDataStreamInfo</unmanaged-short>
        public unsafe void GetMetaDataStreamInfo(System.UInt32 ix, System.String chNameOut, System.IntPtr vOut, System.UInt32 cbRef)
        {
            System.IntPtr chNameOut_ = System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(chNameOut);
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, ix, (void *)((void *)chNameOut_), (void *)((void *)vOut), (void *)(&cbRef), (*(void ***)this._nativePointer)[23]);
            System.Runtime.InteropServices.Marshal.FreeHGlobal(chNameOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("4709c9c6-81ff-11d3-9fc7-00c04f79a0a3")]
    public partial class IMetaDataValidate : SharpGen.Runtime.ComObject
    {
        public IMetaDataValidate(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator IMetaDataValidate(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new IMetaDataValidate(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "dwModuleType">No documentation.</param>
        /// <param name = "unkRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataValidate::ValidatorInit([In] unsigned int dwModuleType,[In] IUnknown* pUnk)</unmanaged>
        /// <unmanaged-short>IMetaDataValidate::ValidatorInit</unmanaged-short>
        public unsafe void ValidatorInit(System.UInt32 dwModuleType, SharpGen.Runtime.IUnknown unkRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, dwModuleType, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<SharpGen.Runtime.IUnknown>(unkRef))), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataValidate::ValidateMetaData()</unmanaged>
        /// <unmanaged-short>IMetaDataValidate::ValidateMetaData</unmanaged-short>
        public unsafe void ValidateMetaData()
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("969ea0c5-964e-411b-a807-b0f3c2dfcbd4")]
    public partial class IMetaDataWinMDImport : SharpGen.Runtime.ComObject
    {
        public IMetaDataWinMDImport(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator IMetaDataWinMDImport(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new IMetaDataWinMDImport(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tr">No documentation.</param>
        /// <param name = "tkResolutionScopeRef">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <param name = "cchName">No documentation.</param>
        /// <param name = "chNameRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT IMetaDataWinMDImport::GetUntransformedTypeRefProps([In] unsigned int tr,[In] unsigned int* ptkResolutionScope,[Out, Buffer, Optional] wchar_t* szName,[In] unsigned int cchName,[In] unsigned int* pchName)</unmanaged>
        /// <unmanaged-short>IMetaDataWinMDImport::GetUntransformedTypeRefProps</unmanaged-short>
        public unsafe void GetUntransformedTypeRefProps(System.UInt32 tr, System.UInt32 tkResolutionScopeRef, System.IntPtr szName, System.UInt32 cchName, System.UInt32 chNameRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, tr, (void *)(&tkResolutionScopeRef), (void *)((void *)szName), cchName, (void *)(&chNameRef), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("A074096B-3ADC-4485-81DA-68C7A4EA52DB")]
    public partial class InstanceFieldSymbol : SharpGen.Runtime.ComObject
    {
        public InstanceFieldSymbol(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator InstanceFieldSymbol(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new InstanceFieldSymbol(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cchName">No documentation.</param>
        /// <param name = "cchNameRef">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugInstanceFieldSymbol::GetName([In] unsigned int cchName,[In] unsigned int* pcchName,[In] wchar_t* szName)</unmanaged>
        /// <unmanaged-short>ICorDebugInstanceFieldSymbol::GetName</unmanaged-short>
        public unsafe void GetName(System.UInt32 cchName, System.UInt32 cchNameRef, System.String szName)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cchName, (void *)(&cchNameRef), (void *)((void *)szName_), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cbSizeRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugInstanceFieldSymbol::GetSize([In] unsigned int* pcbSize)</unmanaged>
        /// <unmanaged-short>ICorDebugInstanceFieldSymbol::GetSize</unmanaged-short>
        public unsafe void GetSize(System.UInt32 cbSizeRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&cbSizeRef), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cbOffsetRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugInstanceFieldSymbol::GetOffset([In] unsigned int* pcbOffset)</unmanaged>
        /// <unmanaged-short>ICorDebugInstanceFieldSymbol::GetOffset</unmanaged-short>
        public unsafe void GetOffset(System.UInt32 cbOffsetRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&cbOffsetRef), (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("B92CC7F7-9D2D-45c4-BC2B-621FCC9DFBF4")]
    public partial class InternalFrame : CorApi.Portable.Frame
    {
        public InternalFrame(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator InternalFrame(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new InternalFrame(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetFrameType</unmanaged>
        /// <unmanaged-short>GetFrameType</unmanaged-short>
        public CorApi.Portable.CorDebugInternalFrameType FrameType
        {
            get
            {
                GetFrameType(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "typeRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugInternalFrame::GetFrameType([Out] CorDebugInternalFrameType* pType)</unmanaged>
        /// <unmanaged-short>ICorDebugInternalFrame::GetFrameType</unmanaged-short>
        internal unsafe void GetFrameType(out CorApi.Portable.CorDebugInternalFrameType typeRef)
        {
            typeRef = new CorApi.Portable.CorDebugInternalFrameType();
            SharpGen.Runtime.Result __result__;
            fixed (void *typeRef_ = &typeRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(typeRef_), (*(void ***)this._nativePointer)[11]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("C0815BDC-CFAB-447e-A779-C116B454EB5B")]
    public partial class InternalFrame2 : SharpGen.Runtime.ComObject
    {
        public InternalFrame2(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator InternalFrame2(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new InternalFrame2(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "addressRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugInternalFrame2::GetAddress([In] unsigned longlong* pAddress)</unmanaged>
        /// <unmanaged-short>ICorDebugInternalFrame2::GetAddress</unmanaged-short>
        public unsafe void GetAddress(System.UInt64 addressRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&addressRef), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "frameToCompareRef">No documentation.</param>
        /// <param name = "isCloserRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugInternalFrame2::IsCloserToLeaf([In] ICorDebugFrame* pFrameToCompare,[In] BOOL* pIsCloser)</unmanaged>
        /// <unmanaged-short>ICorDebugInternalFrame2::IsCloserToLeaf</unmanaged-short>
        public unsafe void IsCloserToLeaf(CorApi.Portable.Frame frameToCompareRef, SharpGen.Runtime.Win32.RawBool isCloserRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.Frame>(frameToCompareRef))), (void *)(&isCloserRef), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("817F343A-6630-4578-96C5-D11BC0EC5EE2")]
    public partial class LoadedModule : SharpGen.Runtime.ComObject
    {
        public LoadedModule(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator LoadedModule(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new LoadedModule(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "addressRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugLoadedModule::GetBaseAddress([In] unsigned longlong* pAddress)</unmanaged>
        /// <unmanaged-short>ICorDebugLoadedModule::GetBaseAddress</unmanaged-short>
        public unsafe void GetBaseAddress(System.UInt64 addressRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&addressRef), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cchName">No documentation.</param>
        /// <param name = "cchNameRef">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugLoadedModule::GetName([In] unsigned int cchName,[In] unsigned int* pcchName,[In] wchar_t* szName)</unmanaged>
        /// <unmanaged-short>ICorDebugLoadedModule::GetName</unmanaged-short>
        public unsafe void GetName(System.UInt32 cchName, System.UInt32 cchNameRef, System.String szName)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cchName, (void *)(&cchNameRef), (void *)((void *)szName_), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cBytesRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugLoadedModule::GetSize([In] unsigned int* pcBytes)</unmanaged>
        /// <unmanaged-short>ICorDebugLoadedModule::GetSize</unmanaged-short>
        public unsafe void GetSize(System.UInt32 cBytesRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&cBytesRef), (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("3d6f5f61-7538-11d3-8d5b-00104b35e7ef")]
    public partial class LocalDebugger : SharpGen.Runtime.ComObject
    {
        public LocalDebugger(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator LocalDebugger(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new LocalDebugger(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebug::Initialize()</unmanaged>
        /// <unmanaged-short>ICorDebug::Initialize</unmanaged-short>
        public unsafe void Initialize()
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebug::Terminate()</unmanaged>
        /// <unmanaged-short>ICorDebug::Terminate</unmanaged-short>
        public unsafe void Terminate()
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "callbackRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebug::SetManagedHandler([In] ICorDebugManagedCallback* pCallback)</unmanaged>
        /// <unmanaged-short>ICorDebug::SetManagedHandler</unmanaged-short>
        public unsafe void SetManagedHandler(CorApi.Portable.ManagedCallback callbackRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.ManagedCallback>(callbackRef))), (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "id">No documentation.</param>
        /// <param name = "win32Attach">No documentation.</param>
        /// <param name = "processOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebug::DebugActiveProcess([In] unsigned int id,[In] BOOL win32Attach,[In] ICorDebugProcess** ppProcess)</unmanaged>
        /// <unmanaged-short>ICorDebug::DebugActiveProcess</unmanaged-short>
        public unsafe void DebugActiveProcess(System.UInt32 id, SharpGen.Runtime.Win32.RawBool win32Attach, out CorApi.Portable.Process processOut)
        {
            System.IntPtr processOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint0(this._nativePointer, id, win32Attach, (void *)(&processOut_), (*(void ***)this._nativePointer)[8]);
            processOut = (processOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Process(processOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "processOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebug::EnumerateProcesses([In] ICorDebugProcessEnum** ppProcess)</unmanaged>
        /// <unmanaged-short>ICorDebug::EnumerateProcesses</unmanaged-short>
        public unsafe void EnumerateProcesses(out CorApi.Portable.ProcessEnum processOut)
        {
            System.IntPtr processOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&processOut_), (*(void ***)this._nativePointer)[9]);
            processOut = (processOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.ProcessEnum(processOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "dwProcessId">No documentation.</param>
        /// <param name = "processOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebug::GetProcess([In] unsigned int dwProcessId,[In] ICorDebugProcess** ppProcess)</unmanaged>
        /// <unmanaged-short>ICorDebug::GetProcess</unmanaged-short>
        public unsafe void GetProcess(System.UInt32 dwProcessId, out CorApi.Portable.Process processOut)
        {
            System.IntPtr processOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, dwProcessId, (void *)(&processOut_), (*(void ***)this._nativePointer)[10]);
            processOut = (processOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Process(processOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "dwProcessId">No documentation.</param>
        /// <param name = "win32DebuggingEnabled">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebug::CanLaunchOrAttach([In] unsigned int dwProcessId,[In] BOOL win32DebuggingEnabled)</unmanaged>
        /// <unmanaged-short>ICorDebug::CanLaunchOrAttach</unmanaged-short>
        public unsafe void CanLaunchOrAttach(System.UInt32 dwProcessId, SharpGen.Runtime.Win32.RawBool win32DebuggingEnabled)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint0(this._nativePointer, dwProcessId, win32DebuggingEnabled, (*(void ***)this._nativePointer)[11]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("ECCCCF2E-B286-4b3e-A983-860A8793D105")]
    public partial class LocalDebugger2 : SharpGen.Runtime.ComObject
    {
        public LocalDebugger2(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator LocalDebugger2(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new LocalDebugger2(nativePtr);
    }

    [System.Runtime.InteropServices.GuidAttribute("3d6f5f60-7538-11d3-8d5b-00104b35e7ef")]
    public partial interface ManagedCallback : SharpGen.Runtime.IUnknown
    {
    }

    [System.Runtime.InteropServices.GuidAttribute("250E5EEA-DB5C-4C76-B6F3-8C46F12E3203")]
    public partial interface ManagedCallback2 : SharpGen.Runtime.IUnknown
    {
    }

    [System.Runtime.InteropServices.GuidAttribute("264EA0FC-2591-49AA-868E-835E6515323F")]
    public partial interface ManagedCallback3 : SharpGen.Runtime.IUnknown
    {
    }

    [System.Runtime.InteropServices.GuidAttribute("CC726F2F-1DB7-459b-B0EC-05F01D841B42")]
    public partial class MDA : SharpGen.Runtime.ComObject
    {
        public MDA(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator MDA(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new MDA(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetFlags</unmanaged>
        /// <unmanaged-short>GetFlags</unmanaged-short>
        public CorApi.Portable.CorDebugMDAFlags Flags
        {
            get
            {
                GetFlags(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cchName">No documentation.</param>
        /// <param name = "cchNameRef">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugMDA::GetName([In] unsigned int cchName,[Out] unsigned int* pcchName,[In] wchar_t* szName)</unmanaged>
        /// <unmanaged-short>ICorDebugMDA::GetName</unmanaged-short>
        public unsafe void GetName(System.UInt32 cchName, out System.UInt32 cchNameRef, System.IntPtr szName)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *cchNameRef_ = &cchNameRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cchName, (void *)(cchNameRef_), (void *)((void *)szName), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cchName">No documentation.</param>
        /// <param name = "cchNameRef">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugMDA::GetDescription([In] unsigned int cchName,[In] unsigned int* pcchName,[In] wchar_t* szName)</unmanaged>
        /// <unmanaged-short>ICorDebugMDA::GetDescription</unmanaged-short>
        public unsafe void GetDescription(System.UInt32 cchName, System.UInt32 cchNameRef, System.String szName)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cchName, (void *)(&cchNameRef), (void *)((void *)szName_), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cchName">No documentation.</param>
        /// <param name = "cchNameRef">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugMDA::GetXML([In] unsigned int cchName,[Out] unsigned int* pcchName,[In] wchar_t* szName)</unmanaged>
        /// <unmanaged-short>ICorDebugMDA::GetXML</unmanaged-short>
        public unsafe void GetXML(System.UInt32 cchName, out System.UInt32 cchNameRef, System.IntPtr szName)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *cchNameRef_ = &cchNameRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cchName, (void *)(cchNameRef_), (void *)((void *)szName), (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "flagsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugMDA::GetFlags([Out] CorDebugMDAFlags* pFlags)</unmanaged>
        /// <unmanaged-short>ICorDebugMDA::GetFlags</unmanaged-short>
        internal unsafe void GetFlags(out CorApi.Portable.CorDebugMDAFlags flagsRef)
        {
            flagsRef = new CorApi.Portable.CorDebugMDAFlags();
            SharpGen.Runtime.Result __result__;
            fixed (void *flagsRef_ = &flagsRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(flagsRef_), (*(void ***)this._nativePointer)[6]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "osTidRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugMDA::GetOSThreadId([In] unsigned int* pOsTid)</unmanaged>
        /// <unmanaged-short>ICorDebugMDA::GetOSThreadId</unmanaged-short>
        public unsafe void GetOSThreadId(System.UInt32 osTidRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&osTidRef), (*(void ***)this._nativePointer)[7]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("677888B3-D160-4B8C-A73B-D79E6AAA1D13")]
    public partial class MemoryBuffer : SharpGen.Runtime.ComObject
    {
        public MemoryBuffer(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator MemoryBuffer(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new MemoryBuffer(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "address">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugMemoryBuffer::GetStartAddress([In] const void** address)</unmanaged>
        /// <unmanaged-short>ICorDebugMemoryBuffer::GetStartAddress</unmanaged-short>
        public unsafe void GetStartAddress(System.IntPtr address)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)address), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cbBufferLengthRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugMemoryBuffer::GetSize([In] unsigned int* pcbBufferLength)</unmanaged>
        /// <unmanaged-short>ICorDebugMemoryBuffer::GetSize</unmanaged-short>
        public unsafe void GetSize(System.UInt32 cbBufferLengthRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&cbBufferLengthRef), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("FAA8637B-3BBE-4671-8E26-3B59875B922A")]
    public partial class MergedAssemblyRecord : SharpGen.Runtime.ComObject
    {
        public MergedAssemblyRecord(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator MergedAssemblyRecord(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new MergedAssemblyRecord(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cchName">No documentation.</param>
        /// <param name = "cchNameRef">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugMergedAssemblyRecord::GetSimpleName([In] unsigned int cchName,[In] unsigned int* pcchName,[In] wchar_t* szName)</unmanaged>
        /// <unmanaged-short>ICorDebugMergedAssemblyRecord::GetSimpleName</unmanaged-short>
        public unsafe void GetSimpleName(System.UInt32 cchName, System.UInt32 cchNameRef, System.String szName)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cchName, (void *)(&cchNameRef), (void *)((void *)szName_), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "majorRef">No documentation.</param>
        /// <param name = "minorRef">No documentation.</param>
        /// <param name = "buildRef">No documentation.</param>
        /// <param name = "revisionRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugMergedAssemblyRecord::GetVersion([In] unsigned short* pMajor,[In] unsigned short* pMinor,[In] unsigned short* pBuild,[In] unsigned short* pRevision)</unmanaged>
        /// <unmanaged-short>ICorDebugMergedAssemblyRecord::GetVersion</unmanaged-short>
        public unsafe void GetVersion(System.UInt16 majorRef, System.UInt16 minorRef, System.UInt16 buildRef, System.UInt16 revisionRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&majorRef), (void *)(&minorRef), (void *)(&buildRef), (void *)(&revisionRef), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cchCulture">No documentation.</param>
        /// <param name = "cchCultureRef">No documentation.</param>
        /// <param name = "szCulture">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugMergedAssemblyRecord::GetCulture([In] unsigned int cchCulture,[In] unsigned int* pcchCulture,[In] wchar_t* szCulture)</unmanaged>
        /// <unmanaged-short>ICorDebugMergedAssemblyRecord::GetCulture</unmanaged-short>
        public unsafe void GetCulture(System.UInt32 cchCulture, System.UInt32 cchCultureRef, System.String szCulture)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szCulture_ = szCulture)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cchCulture, (void *)(&cchCultureRef), (void *)((void *)szCulture_), (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cbPublicKey">No documentation.</param>
        /// <param name = "cbPublicKeyRef">No documentation.</param>
        /// <param name = "bPublicKeyRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugMergedAssemblyRecord::GetPublicKey([In] unsigned int cbPublicKey,[In] unsigned int* pcbPublicKey,[In] unsigned char* pbPublicKey)</unmanaged>
        /// <unmanaged-short>ICorDebugMergedAssemblyRecord::GetPublicKey</unmanaged-short>
        public unsafe void GetPublicKey(System.UInt32 cbPublicKey, System.UInt32 cbPublicKeyRef, System.Byte bPublicKeyRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cbPublicKey, (void *)(&cbPublicKeyRef), (void *)(&bPublicKeyRef), (*(void ***)this._nativePointer)[6]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cbPublicKeyToken">No documentation.</param>
        /// <param name = "cbPublicKeyTokenRef">No documentation.</param>
        /// <param name = "bPublicKeyTokenRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugMergedAssemblyRecord::GetPublicKeyToken([In] unsigned int cbPublicKeyToken,[In] unsigned int* pcbPublicKeyToken,[In] unsigned char* pbPublicKeyToken)</unmanaged>
        /// <unmanaged-short>ICorDebugMergedAssemblyRecord::GetPublicKeyToken</unmanaged-short>
        public unsafe void GetPublicKeyToken(System.UInt32 cbPublicKeyToken, System.UInt32 cbPublicKeyTokenRef, System.Byte bPublicKeyTokenRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cbPublicKeyToken, (void *)(&cbPublicKeyTokenRef), (void *)(&bPublicKeyTokenRef), (*(void ***)this._nativePointer)[7]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "indexRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugMergedAssemblyRecord::GetIndex([In] unsigned int* pIndex)</unmanaged>
        /// <unmanaged-short>ICorDebugMergedAssemblyRecord::GetIndex</unmanaged-short>
        public unsafe void GetIndex(System.UInt32 indexRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&indexRef), (*(void ***)this._nativePointer)[8]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("7cef8ba9-2ef7-42bf-973f-4171474f87d9")]
    public partial class MetaDataLocator : SharpGen.Runtime.ComObject
    {
        public MetaDataLocator(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator MetaDataLocator(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new MetaDataLocator(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "wszImagePath">No documentation.</param>
        /// <param name = "dwImageTimeStamp">No documentation.</param>
        /// <param name = "dwImageSize">No documentation.</param>
        /// <param name = "cchPathBuffer">No documentation.</param>
        /// <param name = "cchPathBufferRef">No documentation.</param>
        /// <param name = "wszPathBuffer">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugMetaDataLocator::GetMetaData([In] const wchar_t* wszImagePath,[In] unsigned int dwImageTimeStamp,[In] unsigned int dwImageSize,[In] unsigned int cchPathBuffer,[Out] unsigned int* pcchPathBuffer,[Out, Buffer] wchar_t* wszPathBuffer)</unmanaged>
        /// <unmanaged-short>ICorDebugMetaDataLocator::GetMetaData</unmanaged-short>
        public unsafe void GetMetaData(System.String wszImagePath, System.UInt32 dwImageTimeStamp, System.UInt32 dwImageSize, System.UInt32 cchPathBuffer, out System.UInt32 cchPathBufferRef, System.IntPtr wszPathBuffer)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *cchPathBufferRef_ = &cchPathBufferRef)
                fixed (char *wszImagePath_ = wszImagePath)
                    __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)wszImagePath_), dwImageTimeStamp, dwImageSize, cchPathBuffer, (void *)(cchPathBufferRef_), (void *)((void *)wszPathBuffer), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("dba2d8c1-e5c5-4069-8c13-10a7c6abf43d")]
    public partial class Module : SharpGen.Runtime.ComObject
    {
        public Module(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Module(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Module(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetAssembly</unmanaged>
        /// <unmanaged-short>GetAssembly</unmanaged-short>
        public CorApi.Portable.Assembly Assembly
        {
            get
            {
                GetAssembly(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetEditAndContinueSnapshot</unmanaged>
        /// <unmanaged-short>GetEditAndContinueSnapshot</unmanaged-short>
        public CorApi.Portable.EditAndContinueSnapshot EditAndContinueSnapshot
        {
            get
            {
                GetEditAndContinueSnapshot(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "processOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugModule::GetProcess([In] ICorDebugProcess** ppProcess)</unmanaged>
        /// <unmanaged-short>ICorDebugModule::GetProcess</unmanaged-short>
        public unsafe void GetProcess(out CorApi.Portable.Process processOut)
        {
            System.IntPtr processOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&processOut_), (*(void ***)this._nativePointer)[3]);
            processOut = (processOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Process(processOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "addressRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugModule::GetBaseAddress([In] unsigned longlong* pAddress)</unmanaged>
        /// <unmanaged-short>ICorDebugModule::GetBaseAddress</unmanaged-short>
        public unsafe void GetBaseAddress(System.UInt64 addressRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&addressRef), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "assemblyOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugModule::GetAssembly([In] ICorDebugAssembly** ppAssembly)</unmanaged>
        /// <unmanaged-short>ICorDebugModule::GetAssembly</unmanaged-short>
        internal unsafe void GetAssembly(out CorApi.Portable.Assembly assemblyOut)
        {
            System.IntPtr assemblyOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&assemblyOut_), (*(void ***)this._nativePointer)[5]);
            assemblyOut = (assemblyOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Assembly(assemblyOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cchName">No documentation.</param>
        /// <param name = "cchNameRef">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugModule::GetName([In] unsigned int cchName,[Out] unsigned int* pcchName,[In] wchar_t* szName)</unmanaged>
        /// <unmanaged-short>ICorDebugModule::GetName</unmanaged-short>
        public unsafe void GetName(System.UInt32 cchName, out System.UInt32 cchNameRef, System.IntPtr szName)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *cchNameRef_ = &cchNameRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cchName, (void *)(cchNameRef_), (void *)((void *)szName), (*(void ***)this._nativePointer)[6]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "bTrackJITInfo">No documentation.</param>
        /// <param name = "bAllowJitOpts">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugModule::EnableJITDebugging([In] BOOL bTrackJITInfo,[In] BOOL bAllowJitOpts)</unmanaged>
        /// <unmanaged-short>ICorDebugModule::EnableJITDebugging</unmanaged-short>
        public unsafe void EnableJITDebugging(SharpGen.Runtime.Win32.RawBool bTrackJITInfo, SharpGen.Runtime.Win32.RawBool bAllowJitOpts)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint0(this._nativePointer, bTrackJITInfo, bAllowJitOpts, (*(void ***)this._nativePointer)[7]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "bClassLoadCallbacks">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugModule::EnableClassLoadCallbacks([In] BOOL bClassLoadCallbacks)</unmanaged>
        /// <unmanaged-short>ICorDebugModule::EnableClassLoadCallbacks</unmanaged-short>
        public unsafe void EnableClassLoadCallbacks(SharpGen.Runtime.Win32.RawBool bClassLoadCallbacks)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint0(this._nativePointer, bClassLoadCallbacks, (*(void ***)this._nativePointer)[8]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "methodDef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugModule::GetFunctionFromToken([In] unsigned int methodDef,[Out] ICorDebugFunction** ppFunction)</unmanaged>
        /// <unmanaged-short>ICorDebugModule::GetFunctionFromToken</unmanaged-short>
        public unsafe CorApi.Portable.Function GetFunctionFromToken(System.UInt32 methodDef)
        {
            CorApi.Portable.Function functionOut;
            System.IntPtr functionOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, methodDef, (void *)(&functionOut_), (*(void ***)this._nativePointer)[9]);
            functionOut = (functionOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Function(functionOut_);
            __result__.CheckError();
            return functionOut;
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "rva">No documentation.</param>
        /// <param name = "functionOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugModule::GetFunctionFromRVA([In] unsigned longlong rva,[In] ICorDebugFunction** ppFunction)</unmanaged>
        /// <unmanaged-short>ICorDebugModule::GetFunctionFromRVA</unmanaged-short>
        public unsafe void GetFunctionFromRVA(System.UInt64 rva, out CorApi.Portable.Function functionOut)
        {
            System.IntPtr functionOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, rva, (void *)(&functionOut_), (*(void ***)this._nativePointer)[10]);
            functionOut = (functionOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Function(functionOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "typeDef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugModule::GetClassFromToken([In] unsigned int typeDef,[Out] ICorDebugClass** ppClass)</unmanaged>
        /// <unmanaged-short>ICorDebugModule::GetClassFromToken</unmanaged-short>
        public unsafe CorApi.Portable.Class GetClassFromToken(System.UInt32 typeDef)
        {
            CorApi.Portable.Class classOut;
            System.IntPtr classOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, typeDef, (void *)(&classOut_), (*(void ***)this._nativePointer)[11]);
            classOut = (classOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Class(classOut_);
            __result__.CheckError();
            return classOut;
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "breakpointOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugModule::CreateBreakpoint([In] ICorDebugModuleBreakpoint** ppBreakpoint)</unmanaged>
        /// <unmanaged-short>ICorDebugModule::CreateBreakpoint</unmanaged-short>
        public unsafe void CreateBreakpoint(out CorApi.Portable.ModuleBreakpoint breakpointOut)
        {
            System.IntPtr breakpointOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&breakpointOut_), (*(void ***)this._nativePointer)[12]);
            breakpointOut = (breakpointOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.ModuleBreakpoint(breakpointOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "editAndContinueSnapshotOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugModule::GetEditAndContinueSnapshot([In] ICorDebugEditAndContinueSnapshot** ppEditAndContinueSnapshot)</unmanaged>
        /// <unmanaged-short>ICorDebugModule::GetEditAndContinueSnapshot</unmanaged-short>
        internal unsafe void GetEditAndContinueSnapshot(out CorApi.Portable.EditAndContinueSnapshot editAndContinueSnapshotOut)
        {
            System.IntPtr editAndContinueSnapshotOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&editAndContinueSnapshotOut_), (*(void ***)this._nativePointer)[13]);
            editAndContinueSnapshotOut = (editAndContinueSnapshotOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.EditAndContinueSnapshot(editAndContinueSnapshotOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "riid">No documentation.</param>
        /// <param name = "objOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugModule::GetMetaDataInterface([In] const GUID&amp; riid,[In] IUnknown** ppObj)</unmanaged>
        /// <unmanaged-short>ICorDebugModule::GetMetaDataInterface</unmanaged-short>
        public unsafe void GetMetaDataInterface(System.Guid riid, out SharpGen.Runtime.IUnknown objOut)
        {
            System.IntPtr objOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&riid), (void *)(&objOut_), (*(void ***)this._nativePointer)[14]);
            objOut = (objOut_ == System.IntPtr.Zero) ? null : new SharpGen.Runtime.ComObject(objOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tokenRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugModule::GetToken([In] unsigned int* pToken)</unmanaged>
        /// <unmanaged-short>ICorDebugModule::GetToken</unmanaged-short>
        public unsafe void GetToken(System.UInt32 tokenRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&tokenRef), (*(void ***)this._nativePointer)[15]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "dynamicRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugModule::IsDynamic([In] BOOL* pDynamic)</unmanaged>
        /// <unmanaged-short>ICorDebugModule::IsDynamic</unmanaged-short>
        public unsafe void IsDynamic(SharpGen.Runtime.Win32.RawBool dynamicRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&dynamicRef), (*(void ***)this._nativePointer)[16]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "fieldDef">No documentation.</param>
        /// <param name = "valueOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugModule::GetGlobalVariableValue([In] unsigned int fieldDef,[In] ICorDebugValue** ppValue)</unmanaged>
        /// <unmanaged-short>ICorDebugModule::GetGlobalVariableValue</unmanaged-short>
        public unsafe void GetGlobalVariableValue(System.UInt32 fieldDef, out CorApi.Portable.Value valueOut)
        {
            System.IntPtr valueOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, fieldDef, (void *)(&valueOut_), (*(void ***)this._nativePointer)[17]);
            valueOut = (valueOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Value(valueOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cBytesRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugModule::GetSize([In] unsigned int* pcBytes)</unmanaged>
        /// <unmanaged-short>ICorDebugModule::GetSize</unmanaged-short>
        public unsafe void GetSize(System.UInt32 cBytesRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&cBytesRef), (*(void ***)this._nativePointer)[18]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "inMemoryRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugModule::IsInMemory([In] BOOL* pInMemory)</unmanaged>
        /// <unmanaged-short>ICorDebugModule::IsInMemory</unmanaged-short>
        public unsafe void IsInMemory(SharpGen.Runtime.Win32.RawBool inMemoryRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&inMemoryRef), (*(void ***)this._nativePointer)[19]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("7FCC5FB5-49C0-41de-9938-3B88B5B9ADD7")]
    public partial class Module2 : SharpGen.Runtime.ComObject
    {
        public Module2(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Module2(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Module2(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetJITCompilerFlags</unmanaged>
        /// <unmanaged-short>GetJITCompilerFlags</unmanaged-short>
        public CorApi.Portable.CorDebugJITCompilerFlags JITCompilerFlags
        {
            get
            {
                GetJITCompilerFlags(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "bIsJustMyCode">No documentation.</param>
        /// <param name = "cTokens">No documentation.</param>
        /// <param name = "tokensRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugModule2::SetJMCStatus([In] BOOL bIsJustMyCode,[In] unsigned int cTokens,[In, Buffer] unsigned int* pTokens)</unmanaged>
        /// <unmanaged-short>ICorDebugModule2::SetJMCStatus</unmanaged-short>
        public unsafe void SetJMCStatus(SharpGen.Runtime.Win32.RawBool bIsJustMyCode, System.UInt32 cTokens, System.UInt32[] tokensRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *tokensRef_ = tokensRef)
                __result__ = CorDebug.LocalInterop.Calliint0(this._nativePointer, bIsJustMyCode, cTokens, (void *)(tokensRef_), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cbMetadata">No documentation.</param>
        /// <param name = "bMetadataRef">No documentation.</param>
        /// <param name = "cbIL">No documentation.</param>
        /// <param name = "bILRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugModule2::ApplyChanges([In] unsigned int cbMetadata,[In] unsigned char* pbMetadata,[In] unsigned int cbIL,[In] unsigned char* pbIL)</unmanaged>
        /// <unmanaged-short>ICorDebugModule2::ApplyChanges</unmanaged-short>
        public unsafe void ApplyChanges(System.UInt32 cbMetadata, System.Byte bMetadataRef, System.UInt32 cbIL, System.Byte bILRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cbMetadata, (void *)(&bMetadataRef), cbIL, (void *)(&bILRef), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "dwFlags">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugModule2::SetJITCompilerFlags([In] unsigned int dwFlags)</unmanaged>
        /// <unmanaged-short>ICorDebugModule2::SetJITCompilerFlags</unmanaged-short>
        public unsafe void SetJITCompilerFlags(CorApi.Portable.CorDebugJITCompilerFlags dwFlags)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, unchecked ((System.Int32)dwFlags), (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "dwFlagsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugModule2::GetJITCompilerFlags([Out] unsigned int* pdwFlags)</unmanaged>
        /// <unmanaged-short>ICorDebugModule2::GetJITCompilerFlags</unmanaged-short>
        internal unsafe void GetJITCompilerFlags(out CorApi.Portable.CorDebugJITCompilerFlags dwFlagsRef)
        {
            dwFlagsRef = new CorApi.Portable.CorDebugJITCompilerFlags();
            SharpGen.Runtime.Result __result__;
            fixed (void *dwFlagsRef_ = &dwFlagsRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(dwFlagsRef_), (*(void ***)this._nativePointer)[6]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tkAssemblyRef">No documentation.</param>
        /// <param name = "assemblyOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugModule2::ResolveAssembly([In] unsigned int tkAssemblyRef,[In] ICorDebugAssembly** ppAssembly)</unmanaged>
        /// <unmanaged-short>ICorDebugModule2::ResolveAssembly</unmanaged-short>
        public unsafe void ResolveAssembly(System.UInt32 tkAssemblyRef, out CorApi.Portable.Assembly assemblyOut)
        {
            System.IntPtr assemblyOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, tkAssemblyRef, (void *)(&assemblyOut_), (*(void ***)this._nativePointer)[7]);
            assemblyOut = (assemblyOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Assembly(assemblyOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("86F012BF-FF15-4372-BD30-B6F11CAAE1DD")]
    public partial class Module3 : SharpGen.Runtime.ComObject
    {
        public Module3(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Module3(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Module3(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "riid">No documentation.</param>
        /// <param name = "objOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugModule3::CreateReaderForInMemorySymbols([In] const GUID&amp; riid,[In] void** ppObj)</unmanaged>
        /// <unmanaged-short>ICorDebugModule3::CreateReaderForInMemorySymbols</unmanaged-short>
        public unsafe void CreateReaderForInMemorySymbols(System.Guid riid, System.IntPtr objOut)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&riid), (void *)((void *)objOut), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("CC7BCAEA-8A68-11d2-983C-0000F808342D")]
    public partial class ModuleBreakpoint : CorApi.Portable.Breakpoint
    {
        public ModuleBreakpoint(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator ModuleBreakpoint(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new ModuleBreakpoint(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetModule</unmanaged>
        /// <unmanaged-short>GetModule</unmanaged-short>
        public CorApi.Portable.Module Module
        {
            get
            {
                GetModule(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "moduleOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugModuleBreakpoint::GetModule([In] ICorDebugModule** ppModule)</unmanaged>
        /// <unmanaged-short>ICorDebugModuleBreakpoint::GetModule</unmanaged-short>
        internal unsafe void GetModule(out CorApi.Portable.Module moduleOut)
        {
            System.IntPtr moduleOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&moduleOut_), (*(void ***)this._nativePointer)[5]);
            moduleOut = (moduleOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Module(moduleOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("51A15E8D-9FFF-4864-9B87-F4FBDEA747A2")]
    public partial class ModuleDebugEvent : CorApi.Portable.DebugEvent
    {
        public ModuleDebugEvent(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator ModuleDebugEvent(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new ModuleDebugEvent(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetModule</unmanaged>
        /// <unmanaged-short>GetModule</unmanaged-short>
        public CorApi.Portable.Module Module
        {
            get
            {
                GetModule(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "moduleOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugModuleDebugEvent::GetModule([In] ICorDebugModule** ppModule)</unmanaged>
        /// <unmanaged-short>ICorDebugModuleDebugEvent::GetModule</unmanaged-short>
        internal unsafe void GetModule(out CorApi.Portable.Module moduleOut)
        {
            System.IntPtr moduleOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&moduleOut_), (*(void ***)this._nativePointer)[5]);
            moduleOut = (moduleOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Module(moduleOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("CC7BCB09-8A68-11d2-983C-0000F808342D")]
    public partial class ModuleEnum : CorApi.Portable.Enum
    {
        public ModuleEnum(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator ModuleEnum(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new ModuleEnum(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "celt">No documentation.</param>
        /// <param name = "modules">No documentation.</param>
        /// <param name = "celtFetchedRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugModuleEnum::Next([In] unsigned int celt,[In] ICorDebugModule** modules,[In] unsigned int* pceltFetched)</unmanaged>
        /// <unmanaged-short>ICorDebugModuleEnum::Next</unmanaged-short>
        public unsafe void Next(System.UInt32 celt, out CorApi.Portable.Module modules, System.UInt32 celtFetchedRef)
        {
            System.IntPtr modules_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, celt, (void *)(&modules_), (void *)(&celtFetchedRef), (*(void ***)this._nativePointer)[7]);
            modules = (modules_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Module(modules_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("A1B8A756-3CB6-4CCB-979F-3DF999673A59")]
    public partial class MutableDataTarget : CorApi.Portable.DataTarget
    {
        public MutableDataTarget(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator MutableDataTarget(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new MutableDataTarget(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "address">No documentation.</param>
        /// <param name = "bufferRef">No documentation.</param>
        /// <param name = "bytesRequested">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugMutableDataTarget::WriteVirtual([In] unsigned longlong address,[In] const unsigned char* pBuffer,[In] unsigned int bytesRequested)</unmanaged>
        /// <unmanaged-short>ICorDebugMutableDataTarget::WriteVirtual</unmanaged-short>
        public unsafe void WriteVirtual(System.UInt64 address, System.Byte bufferRef, System.UInt32 bytesRequested)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, address, (void *)(&bufferRef), bytesRequested, (*(void ***)this._nativePointer)[6]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "dwThreadID">No documentation.</param>
        /// <param name = "contextSize">No documentation.</param>
        /// <param name = "contextRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugMutableDataTarget::SetThreadContext([In] unsigned int dwThreadID,[In] unsigned int contextSize,[In] const unsigned char* pContext)</unmanaged>
        /// <unmanaged-short>ICorDebugMutableDataTarget::SetThreadContext</unmanaged-short>
        public unsafe void SetThreadContext(System.UInt32 dwThreadID, System.UInt32 contextSize, System.Byte contextRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, dwThreadID, contextSize, (void *)(&contextRef), (*(void ***)this._nativePointer)[7]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "dwThreadId">No documentation.</param>
        /// <param name = "continueStatus">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugMutableDataTarget::ContinueStatusChanged([In] unsigned int dwThreadId,[In] unsigned int continueStatus)</unmanaged>
        /// <unmanaged-short>ICorDebugMutableDataTarget::ContinueStatusChanged</unmanaged-short>
        public unsafe void ContinueStatusChanged(System.UInt32 dwThreadId, System.UInt32 continueStatus)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, dwThreadId, continueStatus, (*(void ***)this._nativePointer)[8]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("03E26314-4F76-11d3-88C6-006097945418")]
    public partial class NativeFrame : CorApi.Portable.Frame
    {
        public NativeFrame(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator NativeFrame(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new NativeFrame(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetRegisterSet</unmanaged>
        /// <unmanaged-short>GetRegisterSet</unmanaged-short>
        public CorApi.Portable.RegisterSet RegisterSet
        {
            get
            {
                GetRegisterSet(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "nOffsetRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugNativeFrame::GetIP([Out] unsigned int* pnOffset)</unmanaged>
        /// <unmanaged-short>ICorDebugNativeFrame::GetIP</unmanaged-short>
        public unsafe void GetIP(out System.UInt32 nOffsetRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *nOffsetRef_ = &nOffsetRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(nOffsetRef_), (*(void ***)this._nativePointer)[11]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "nOffset">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugNativeFrame::SetIP([In] unsigned int nOffset)</unmanaged>
        /// <unmanaged-short>ICorDebugNativeFrame::SetIP</unmanaged-short>
        public unsafe void SetIP(System.UInt32 nOffset)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, nOffset, (*(void ***)this._nativePointer)[12]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "registersOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugNativeFrame::GetRegisterSet([In] ICorDebugRegisterSet** ppRegisters)</unmanaged>
        /// <unmanaged-short>ICorDebugNativeFrame::GetRegisterSet</unmanaged-short>
        internal unsafe void GetRegisterSet(out CorApi.Portable.RegisterSet registersOut)
        {
            System.IntPtr registersOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&registersOut_), (*(void ***)this._nativePointer)[13]);
            registersOut = (registersOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.RegisterSet(registersOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "reg">No documentation.</param>
        /// <param name = "cbSigBlob">No documentation.</param>
        /// <param name = "vSigBlobRef">No documentation.</param>
        /// <param name = "valueOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugNativeFrame::GetLocalRegisterValue([In] CorDebugRegister reg,[In] unsigned int cbSigBlob,[In] const unsigned char* pvSigBlob,[In] ICorDebugValue** ppValue)</unmanaged>
        /// <unmanaged-short>ICorDebugNativeFrame::GetLocalRegisterValue</unmanaged-short>
        public unsafe void GetLocalRegisterValue(CorApi.Portable.CorDebugRegister reg, System.UInt32 cbSigBlob, System.Byte vSigBlobRef, out CorApi.Portable.Value valueOut)
        {
            System.IntPtr valueOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, unchecked ((System.Int32)reg), cbSigBlob, (void *)(&vSigBlobRef), (void *)(&valueOut_), (*(void ***)this._nativePointer)[14]);
            valueOut = (valueOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Value(valueOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "highWordReg">No documentation.</param>
        /// <param name = "lowWordReg">No documentation.</param>
        /// <param name = "cbSigBlob">No documentation.</param>
        /// <param name = "vSigBlobRef">No documentation.</param>
        /// <param name = "valueOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugNativeFrame::GetLocalDoubleRegisterValue([In] CorDebugRegister highWordReg,[In] CorDebugRegister lowWordReg,[In] unsigned int cbSigBlob,[In] const unsigned char* pvSigBlob,[In] ICorDebugValue** ppValue)</unmanaged>
        /// <unmanaged-short>ICorDebugNativeFrame::GetLocalDoubleRegisterValue</unmanaged-short>
        public unsafe void GetLocalDoubleRegisterValue(CorApi.Portable.CorDebugRegister highWordReg, CorApi.Portable.CorDebugRegister lowWordReg, System.UInt32 cbSigBlob, System.Byte vSigBlobRef, out CorApi.Portable.Value valueOut)
        {
            System.IntPtr valueOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, unchecked ((System.Int32)highWordReg), unchecked ((System.Int32)lowWordReg), cbSigBlob, (void *)(&vSigBlobRef), (void *)(&valueOut_), (*(void ***)this._nativePointer)[15]);
            valueOut = (valueOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Value(valueOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "address">No documentation.</param>
        /// <param name = "cbSigBlob">No documentation.</param>
        /// <param name = "vSigBlobRef">No documentation.</param>
        /// <param name = "valueOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugNativeFrame::GetLocalMemoryValue([In] unsigned longlong address,[In] unsigned int cbSigBlob,[In] const unsigned char* pvSigBlob,[In] ICorDebugValue** ppValue)</unmanaged>
        /// <unmanaged-short>ICorDebugNativeFrame::GetLocalMemoryValue</unmanaged-short>
        public unsafe void GetLocalMemoryValue(System.UInt64 address, System.UInt32 cbSigBlob, System.Byte vSigBlobRef, out CorApi.Portable.Value valueOut)
        {
            System.IntPtr valueOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, address, cbSigBlob, (void *)(&vSigBlobRef), (void *)(&valueOut_), (*(void ***)this._nativePointer)[16]);
            valueOut = (valueOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Value(valueOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "highWordReg">No documentation.</param>
        /// <param name = "lowWordAddress">No documentation.</param>
        /// <param name = "cbSigBlob">No documentation.</param>
        /// <param name = "vSigBlobRef">No documentation.</param>
        /// <param name = "valueOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugNativeFrame::GetLocalRegisterMemoryValue([In] CorDebugRegister highWordReg,[In] unsigned longlong lowWordAddress,[In] unsigned int cbSigBlob,[In] const unsigned char* pvSigBlob,[In] ICorDebugValue** ppValue)</unmanaged>
        /// <unmanaged-short>ICorDebugNativeFrame::GetLocalRegisterMemoryValue</unmanaged-short>
        public unsafe void GetLocalRegisterMemoryValue(CorApi.Portable.CorDebugRegister highWordReg, System.UInt64 lowWordAddress, System.UInt32 cbSigBlob, System.Byte vSigBlobRef, out CorApi.Portable.Value valueOut)
        {
            System.IntPtr valueOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, unchecked ((System.Int32)highWordReg), lowWordAddress, cbSigBlob, (void *)(&vSigBlobRef), (void *)(&valueOut_), (*(void ***)this._nativePointer)[17]);
            valueOut = (valueOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Value(valueOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "highWordAddress">No documentation.</param>
        /// <param name = "lowWordRegister">No documentation.</param>
        /// <param name = "cbSigBlob">No documentation.</param>
        /// <param name = "vSigBlobRef">No documentation.</param>
        /// <param name = "valueOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugNativeFrame::GetLocalMemoryRegisterValue([In] unsigned longlong highWordAddress,[In] CorDebugRegister lowWordRegister,[In] unsigned int cbSigBlob,[In] const unsigned char* pvSigBlob,[In] ICorDebugValue** ppValue)</unmanaged>
        /// <unmanaged-short>ICorDebugNativeFrame::GetLocalMemoryRegisterValue</unmanaged-short>
        public unsafe void GetLocalMemoryRegisterValue(System.UInt64 highWordAddress, CorApi.Portable.CorDebugRegister lowWordRegister, System.UInt32 cbSigBlob, System.Byte vSigBlobRef, out CorApi.Portable.Value valueOut)
        {
            System.IntPtr valueOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, highWordAddress, unchecked ((System.Int32)lowWordRegister), cbSigBlob, (void *)(&vSigBlobRef), (void *)(&valueOut_), (*(void ***)this._nativePointer)[18]);
            valueOut = (valueOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Value(valueOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "nOffset">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugNativeFrame::CanSetIP([In] unsigned int nOffset)</unmanaged>
        /// <unmanaged-short>ICorDebugNativeFrame::CanSetIP</unmanaged-short>
        public unsafe void CanSetIP(System.UInt32 nOffset)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, nOffset, (*(void ***)this._nativePointer)[19]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("35389FF1-3684-4c55-A2EE-210F26C60E5E")]
    public partial class NativeFrame2 : SharpGen.Runtime.ComObject
    {
        public NativeFrame2(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator NativeFrame2(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new NativeFrame2(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "isChildRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugNativeFrame2::IsChild([In] BOOL* pIsChild)</unmanaged>
        /// <unmanaged-short>ICorDebugNativeFrame2::IsChild</unmanaged-short>
        public unsafe void IsChild(SharpGen.Runtime.Win32.RawBool isChildRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&isChildRef), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "potentialParentFrameRef">No documentation.</param>
        /// <param name = "isParentRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugNativeFrame2::IsMatchingParentFrame([In] ICorDebugNativeFrame2* pPotentialParentFrame,[In] BOOL* pIsParent)</unmanaged>
        /// <unmanaged-short>ICorDebugNativeFrame2::IsMatchingParentFrame</unmanaged-short>
        public unsafe void IsMatchingParentFrame(CorApi.Portable.NativeFrame2 potentialParentFrameRef, SharpGen.Runtime.Win32.RawBool isParentRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.NativeFrame2>(potentialParentFrameRef))), (void *)(&isParentRef), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "sizeRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugNativeFrame2::GetStackParameterSize([In] unsigned int* pSize)</unmanaged>
        /// <unmanaged-short>ICorDebugNativeFrame2::GetStackParameterSize</unmanaged-short>
        public unsafe void GetStackParameterSize(System.UInt32 sizeRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&sizeRef), (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("CC7BCB02-8A68-11d2-983C-0000F808342D")]
    public partial class ObjectEnum : CorApi.Portable.Enum
    {
        public ObjectEnum(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator ObjectEnum(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new ObjectEnum(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "celt">No documentation.</param>
        /// <param name = "objects">No documentation.</param>
        /// <param name = "celtFetchedRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugObjectEnum::Next([In] unsigned int celt,[In] unsigned longlong* objects,[In] unsigned int* pceltFetched)</unmanaged>
        /// <unmanaged-short>ICorDebugObjectEnum::Next</unmanaged-short>
        public unsafe void Next(System.UInt32 celt, System.UInt64 objects, System.UInt32 celtFetchedRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, celt, (void *)(&objects), (void *)(&celtFetchedRef), (*(void ***)this._nativePointer)[7]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("18AD3D6E-B7D2-11d2-BD04-0000F80849BD")]
    public partial class ObjectValue : CorApi.Portable.Value
    {
        public ObjectValue(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator ObjectValue(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new ObjectValue(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetClass</unmanaged>
        /// <unmanaged-short>GetClass</unmanaged-short>
        public CorApi.Portable.Class Class
        {
            get
            {
                GetClass(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetContext</unmanaged>
        /// <unmanaged-short>GetContext</unmanaged-short>
        public CorApi.Portable.Context Context
        {
            get
            {
                GetContext(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IsValueClass</unmanaged>
        /// <unmanaged-short>IsValueClass</unmanaged-short>
        public SharpGen.Runtime.Win32.RawBool IsValueClass
        {
            get
            {
                IsValueClass_(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetManagedCopy</unmanaged>
        /// <unmanaged-short>GetManagedCopy</unmanaged-short>
        public SharpGen.Runtime.IUnknown ManagedCopy
        {
            get
            {
                GetManagedCopy(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "classOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugObjectValue::GetClass([In] ICorDebugClass** ppClass)</unmanaged>
        /// <unmanaged-short>ICorDebugObjectValue::GetClass</unmanaged-short>
        internal unsafe void GetClass(out CorApi.Portable.Class classOut)
        {
            System.IntPtr classOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&classOut_), (*(void ***)this._nativePointer)[7]);
            classOut = (classOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Class(classOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "classRef">No documentation.</param>
        /// <param name = "fieldDef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugObjectValue::GetFieldValue([In] ICorDebugClass* pClass,[In] unsigned int fieldDef,[Out] ICorDebugValue** ppValue)</unmanaged>
        /// <unmanaged-short>ICorDebugObjectValue::GetFieldValue</unmanaged-short>
        public unsafe CorApi.Portable.Value GetFieldValue(CorApi.Portable.Class classRef, System.UInt32 fieldDef)
        {
            CorApi.Portable.Value valueOut;
            System.IntPtr valueOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.Class>(classRef))), fieldDef, (void *)(&valueOut_), (*(void ***)this._nativePointer)[8]);
            valueOut = (valueOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Value(valueOut_);
            __result__.CheckError();
            return valueOut;
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "memberRef">No documentation.</param>
        /// <param name = "functionOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugObjectValue::GetVirtualMethod([In] unsigned int memberRef,[In] ICorDebugFunction** ppFunction)</unmanaged>
        /// <unmanaged-short>ICorDebugObjectValue::GetVirtualMethod</unmanaged-short>
        public unsafe void GetVirtualMethod(System.UInt32 memberRef, out CorApi.Portable.Function functionOut)
        {
            System.IntPtr functionOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, memberRef, (void *)(&functionOut_), (*(void ***)this._nativePointer)[9]);
            functionOut = (functionOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Function(functionOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "contextOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugObjectValue::GetContext([In] ICorDebugContext** ppContext)</unmanaged>
        /// <unmanaged-short>ICorDebugObjectValue::GetContext</unmanaged-short>
        internal unsafe void GetContext(out CorApi.Portable.Context contextOut)
        {
            System.IntPtr contextOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&contextOut_), (*(void ***)this._nativePointer)[10]);
            contextOut = (contextOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Context(contextOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "bIsValueClassRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugObjectValue::IsValueClass([Out] BOOL* pbIsValueClass)</unmanaged>
        /// <unmanaged-short>ICorDebugObjectValue::IsValueClass</unmanaged-short>
        internal unsafe void IsValueClass_(out SharpGen.Runtime.Win32.RawBool bIsValueClassRef)
        {
            bIsValueClassRef = new SharpGen.Runtime.Win32.RawBool();
            SharpGen.Runtime.Result __result__;
            fixed (void *bIsValueClassRef_ = &bIsValueClassRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(bIsValueClassRef_), (*(void ***)this._nativePointer)[11]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "objectOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugObjectValue::GetManagedCopy([In] IUnknown** ppObject)</unmanaged>
        /// <unmanaged-short>ICorDebugObjectValue::GetManagedCopy</unmanaged-short>
        internal unsafe void GetManagedCopy(out SharpGen.Runtime.IUnknown objectOut)
        {
            System.IntPtr objectOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&objectOut_), (*(void ***)this._nativePointer)[12]);
            objectOut = (objectOut_ == System.IntPtr.Zero) ? null : new SharpGen.Runtime.ComObject(objectOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "objectRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugObjectValue::SetFromManagedCopy([In] IUnknown* pObject)</unmanaged>
        /// <unmanaged-short>ICorDebugObjectValue::SetFromManagedCopy</unmanaged-short>
        public unsafe void SetFromManagedCopy(SharpGen.Runtime.IUnknown objectRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<SharpGen.Runtime.IUnknown>(objectRef))), (*(void ***)this._nativePointer)[13]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("49E4A320-4A9B-4eca-B105-229FB7D5009F")]
    public partial class ObjectValue2 : SharpGen.Runtime.ComObject
    {
        public ObjectValue2(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator ObjectValue2(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new ObjectValue2(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "memberRef">No documentation.</param>
        /// <param name = "functionOut">No documentation.</param>
        /// <param name = "typeOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugObjectValue2::GetVirtualMethodAndType([In] unsigned int memberRef,[In] ICorDebugFunction** ppFunction,[In] ICorDebugType** ppType)</unmanaged>
        /// <unmanaged-short>ICorDebugObjectValue2::GetVirtualMethodAndType</unmanaged-short>
        public unsafe void GetVirtualMethodAndType(System.UInt32 memberRef, out CorApi.Portable.Function functionOut, out CorApi.Portable.Type typeOut)
        {
            System.IntPtr functionOut_ = System.IntPtr.Zero;
            System.IntPtr typeOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, memberRef, (void *)(&functionOut_), (void *)(&typeOut_), (*(void ***)this._nativePointer)[3]);
            functionOut = (functionOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Function(functionOut_);
            typeOut = (typeOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Type(typeOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("3d6f5f64-7538-11d3-8d5b-00104b35e7ef")]
    public partial class Process : CorApi.Portable.Controller
    {
        public Process(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Process(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Process(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetID</unmanaged>
        /// <unmanaged-short>GetID</unmanaged-short>
        public System.UInt32 ID
        {
            get
            {
                GetID(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetObjectW</unmanaged>
        /// <unmanaged-short>GetObjectW</unmanaged-short>
        public CorApi.Portable.Value ObjectW
        {
            get
            {
                GetObjectW(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "dwProcessIdRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess::GetID([Out] unsigned int* pdwProcessId)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess::GetID</unmanaged-short>
        internal unsafe void GetID(out System.UInt32 dwProcessIdRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *dwProcessIdRef_ = &dwProcessIdRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(dwProcessIdRef_), (*(void ***)this._nativePointer)[13]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hProcessHandleRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess::GetHandle([In] void** phProcessHandle)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess::GetHandle</unmanaged-short>
        public unsafe void GetHandle(System.IntPtr hProcessHandleRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)hProcessHandleRef), (*(void ***)this._nativePointer)[14]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "dwThreadId">No documentation.</param>
        /// <param name = "threadOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess::GetThread([In] unsigned int dwThreadId,[In] ICorDebugThread** ppThread)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess::GetThread</unmanaged-short>
        public unsafe void GetThread(System.UInt32 dwThreadId, out CorApi.Portable.Thread threadOut)
        {
            System.IntPtr threadOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, dwThreadId, (void *)(&threadOut_), (*(void ***)this._nativePointer)[15]);
            threadOut = (threadOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Thread(threadOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "objectsOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess::EnumerateObjects([In] ICorDebugObjectEnum** ppObjects)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess::EnumerateObjects</unmanaged-short>
        public unsafe void EnumerateObjects(out CorApi.Portable.ObjectEnum objectsOut)
        {
            System.IntPtr objectsOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&objectsOut_), (*(void ***)this._nativePointer)[16]);
            objectsOut = (objectsOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.ObjectEnum(objectsOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "address">No documentation.</param>
        /// <param name = "bTransitionStubRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess::IsTransitionStub([In] unsigned longlong address,[In] BOOL* pbTransitionStub)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess::IsTransitionStub</unmanaged-short>
        public unsafe void IsTransitionStub(System.UInt64 address, SharpGen.Runtime.Win32.RawBool bTransitionStubRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, address, (void *)(&bTransitionStubRef), (*(void ***)this._nativePointer)[17]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "threadID">No documentation.</param>
        /// <param name = "bSuspendedRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess::IsOSSuspended([In] unsigned int threadID,[In] BOOL* pbSuspended)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess::IsOSSuspended</unmanaged-short>
        public unsafe void IsOSSuspended(System.UInt32 threadID, SharpGen.Runtime.Win32.RawBool bSuspendedRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, threadID, (void *)(&bSuspendedRef), (*(void ***)this._nativePointer)[18]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "threadID">No documentation.</param>
        /// <param name = "contextSize">No documentation.</param>
        /// <param name = "context">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess::GetThreadContext([In] unsigned int threadID,[In] unsigned int contextSize,[In] unsigned char* context)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess::GetThreadContext</unmanaged-short>
        public unsafe void GetThreadContext(System.UInt32 threadID, System.UInt32 contextSize, System.Byte context)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, threadID, contextSize, (void *)(&context), (*(void ***)this._nativePointer)[19]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "threadID">No documentation.</param>
        /// <param name = "contextSize">No documentation.</param>
        /// <param name = "context">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess::SetThreadContext([In] unsigned int threadID,[In] unsigned int contextSize,[In] unsigned char* context)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess::SetThreadContext</unmanaged-short>
        public unsafe void SetThreadContext(System.UInt32 threadID, System.UInt32 contextSize, System.Byte context)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, threadID, contextSize, (void *)(&context), (*(void ***)this._nativePointer)[20]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "address">No documentation.</param>
        /// <param name = "size">No documentation.</param>
        /// <param name = "buffer">No documentation.</param>
        /// <param name = "read">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess::ReadMemory([In] unsigned longlong address,[In] unsigned int size,[In] unsigned char* buffer,[In] SIZE_T* read)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess::ReadMemory</unmanaged-short>
        public unsafe void ReadMemory(System.UInt64 address, System.UInt32 size, System.Byte buffer, SharpGen.Runtime.PointerSize read)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, address, size, (void *)(&buffer), (void *)(&read), (*(void ***)this._nativePointer)[21]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "address">No documentation.</param>
        /// <param name = "size">No documentation.</param>
        /// <param name = "buffer">No documentation.</param>
        /// <param name = "written">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess::WriteMemory([In] unsigned longlong address,[In] unsigned int size,[In] unsigned char* buffer,[In] SIZE_T* written)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess::WriteMemory</unmanaged-short>
        public unsafe void WriteMemory(System.UInt64 address, System.UInt32 size, System.Byte buffer, SharpGen.Runtime.PointerSize written)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, address, size, (void *)(&buffer), (void *)(&written), (*(void ***)this._nativePointer)[22]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "threadID">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess::ClearCurrentException([In] unsigned int threadID)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess::ClearCurrentException</unmanaged-short>
        public unsafe void ClearCurrentException(System.UInt32 threadID)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, threadID, (*(void ***)this._nativePointer)[23]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "fOnOff">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess::EnableLogMessages([In] BOOL fOnOff)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess::EnableLogMessages</unmanaged-short>
        public unsafe void EnableLogMessages(SharpGen.Runtime.Win32.RawBool fOnOff)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint0(this._nativePointer, fOnOff, (*(void ***)this._nativePointer)[24]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "logSwitchNameRef">No documentation.</param>
        /// <param name = "lLevel">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess::ModifyLogSwitch([In] wchar_t* pLogSwitchName,[In] int lLevel)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess::ModifyLogSwitch</unmanaged-short>
        public unsafe void ModifyLogSwitch(System.String logSwitchNameRef, System.Int32 lLevel)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *logSwitchNameRef_ = logSwitchNameRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)logSwitchNameRef_), lLevel, (*(void ***)this._nativePointer)[25]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "appDomainsOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess::EnumerateAppDomains([In] ICorDebugAppDomainEnum** ppAppDomains)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess::EnumerateAppDomains</unmanaged-short>
        public unsafe void EnumerateAppDomains(out CorApi.Portable.AppDomainEnum appDomainsOut)
        {
            System.IntPtr appDomainsOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&appDomainsOut_), (*(void ***)this._nativePointer)[26]);
            appDomainsOut = (appDomainsOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.AppDomainEnum(appDomainsOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "objectOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess::GetObjectW([In] ICorDebugValue** ppObject)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess::GetObjectW</unmanaged-short>
        internal unsafe void GetObjectW(out CorApi.Portable.Value objectOut)
        {
            System.IntPtr objectOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&objectOut_), (*(void ***)this._nativePointer)[27]);
            objectOut = (objectOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Value(objectOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "fiberCookie">No documentation.</param>
        /// <param name = "threadOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess::ThreadForFiberCookie([In] unsigned int fiberCookie,[In] ICorDebugThread** ppThread)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess::ThreadForFiberCookie</unmanaged-short>
        public unsafe void ThreadForFiberCookie(System.UInt32 fiberCookie, out CorApi.Portable.Thread threadOut)
        {
            System.IntPtr threadOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, fiberCookie, (void *)(&threadOut_), (*(void ***)this._nativePointer)[28]);
            threadOut = (threadOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Thread(threadOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "threadIDRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess::GetHelperThreadID([In] unsigned int* pThreadID)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess::GetHelperThreadID</unmanaged-short>
        public unsafe void GetHelperThreadID(System.UInt32 threadIDRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&threadIDRef), (*(void ***)this._nativePointer)[29]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("AD1B3588-0EF0-4744-A496-AA09A9F80371")]
    public partial class Process2 : SharpGen.Runtime.ComObject
    {
        public Process2(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Process2(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Process2(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetDesiredNGENCompilerFlags</unmanaged>
        /// <unmanaged-short>GetDesiredNGENCompilerFlags</unmanaged-short>
        public CorApi.Portable.CorDebugJITCompilerFlags DesiredNGENCompilerFlags
        {
            get
            {
                GetDesiredNGENCompilerFlags(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "taskid">No documentation.</param>
        /// <param name = "threadOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess2::GetThreadForTaskID([In] unsigned longlong taskid,[In] ICorDebugThread2** ppThread)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess2::GetThreadForTaskID</unmanaged-short>
        public unsafe void GetThreadForTaskID(System.UInt64 taskid, out CorApi.Portable.Thread2 threadOut)
        {
            System.IntPtr threadOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, taskid, (void *)(&threadOut_), (*(void ***)this._nativePointer)[3]);
            threadOut = (threadOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Thread2(threadOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "version">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess2::GetVersion([In] COR_VERSION* version)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess2::GetVersion</unmanaged-short>
        public unsafe void GetVersion(CorApi.Portable.CorVersion version)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&version), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "address">No documentation.</param>
        /// <param name = "bufsize">No documentation.</param>
        /// <param name = "buffer">No documentation.</param>
        /// <param name = "bufLen">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess2::SetUnmanagedBreakpoint([In] unsigned longlong address,[In] unsigned int bufsize,[In] unsigned char* buffer,[In] unsigned int* bufLen)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess2::SetUnmanagedBreakpoint</unmanaged-short>
        public unsafe void SetUnmanagedBreakpoint(System.UInt64 address, System.UInt32 bufsize, System.Byte buffer, System.UInt32 bufLen)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, address, bufsize, (void *)(&buffer), (void *)(&bufLen), (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "address">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess2::ClearUnmanagedBreakpoint([In] unsigned longlong address)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess2::ClearUnmanagedBreakpoint</unmanaged-short>
        public unsafe void ClearUnmanagedBreakpoint(System.UInt64 address)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, address, (*(void ***)this._nativePointer)[6]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "pdwFlags">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess2::SetDesiredNGENCompilerFlags([In] unsigned int pdwFlags)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess2::SetDesiredNGENCompilerFlags</unmanaged-short>
        public unsafe void SetDesiredNGENCompilerFlags(CorApi.Portable.CorDebugJITCompilerFlags pdwFlags)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, unchecked ((System.Int32)pdwFlags), (*(void ***)this._nativePointer)[7]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "dwFlagsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess2::GetDesiredNGENCompilerFlags([Out] unsigned int* pdwFlags)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess2::GetDesiredNGENCompilerFlags</unmanaged-short>
        internal unsafe void GetDesiredNGENCompilerFlags(out CorApi.Portable.CorDebugJITCompilerFlags dwFlagsRef)
        {
            dwFlagsRef = new CorApi.Portable.CorDebugJITCompilerFlags();
            SharpGen.Runtime.Result __result__;
            fixed (void *dwFlagsRef_ = &dwFlagsRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(dwFlagsRef_), (*(void ***)this._nativePointer)[8]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "handle">No documentation.</param>
        /// <param name = "outValueRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess2::GetReferenceValueFromGCHandle([In] UINT_PTR handle,[In] ICorDebugReferenceValue** pOutValue)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess2::GetReferenceValueFromGCHandle</unmanaged-short>
        public unsafe void GetReferenceValueFromGCHandle(System.UIntPtr handle, out CorApi.Portable.ReferenceValue outValueRef)
        {
            System.IntPtr outValueRef_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, handle, (void *)(&outValueRef_), (*(void ***)this._nativePointer)[9]);
            outValueRef = (outValueRef_ == System.IntPtr.Zero) ? null : new CorApi.Portable.ReferenceValue(outValueRef_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("2EE06488-C0D4-42B1-B26D-F3795EF606FB")]
    public partial class Process3 : SharpGen.Runtime.ComObject
    {
        public Process3(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Process3(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Process3(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "classRef">No documentation.</param>
        /// <param name = "fEnable">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess3::SetEnableCustomNotification([In] ICorDebugClass* pClass,[In] BOOL fEnable)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess3::SetEnableCustomNotification</unmanaged-short>
        public unsafe void SetEnableCustomNotification(CorApi.Portable.Class classRef, SharpGen.Runtime.Win32.RawBool fEnable)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint0(this._nativePointer, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.Class>(classRef))), fEnable, (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("21e9d9c0-fcb8-11df-8cff-0800200c9a66")]
    public partial class Process5 : SharpGen.Runtime.ComObject
    {
        public Process5(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Process5(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Process5(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "heapInfoRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess5::GetGCHeapInformation([In] COR_HEAPINFO* pHeapInfo)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess5::GetGCHeapInformation</unmanaged-short>
        public unsafe void GetGCHeapInformation(ref CorApi.Portable.CorHeapinfo heapInfoRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *heapInfoRef_ = &heapInfoRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(heapInfoRef_), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "objectsOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess5::EnumerateHeap([In] ICorDebugHeapEnum** ppObjects)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess5::EnumerateHeap</unmanaged-short>
        public unsafe void EnumerateHeap(out CorApi.Portable.HeapEnum objectsOut)
        {
            System.IntPtr objectsOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&objectsOut_), (*(void ***)this._nativePointer)[4]);
            objectsOut = (objectsOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.HeapEnum(objectsOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "regionsOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess5::EnumerateHeapRegions([In] ICorDebugHeapSegmentEnum** ppRegions)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess5::EnumerateHeapRegions</unmanaged-short>
        public unsafe void EnumerateHeapRegions(out CorApi.Portable.HeapSegmentEnum regionsOut)
        {
            System.IntPtr regionsOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&regionsOut_), (*(void ***)this._nativePointer)[5]);
            regionsOut = (regionsOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.HeapSegmentEnum(regionsOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "addr">No documentation.</param>
        /// <param name = "objectRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess5::GetObjectW([In] unsigned longlong addr,[In] ICorDebugObjectValue** pObject)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess5::GetObjectW</unmanaged-short>
        public unsafe void GetObjectW(System.UInt64 addr, out CorApi.Portable.ObjectValue objectRef)
        {
            System.IntPtr objectRef_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, addr, (void *)(&objectRef_), (*(void ***)this._nativePointer)[6]);
            objectRef = (objectRef_ == System.IntPtr.Zero) ? null : new CorApi.Portable.ObjectValue(objectRef_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "enumerateWeakReferences">No documentation.</param>
        /// <param name = "enumOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess5::EnumerateGCReferences([In] BOOL enumerateWeakReferences,[In] ICorDebugGCReferenceEnum** ppEnum)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess5::EnumerateGCReferences</unmanaged-short>
        public unsafe void EnumerateGCReferences(SharpGen.Runtime.Win32.RawBool enumerateWeakReferences, out CorApi.Portable.GCReferenceEnum enumOut)
        {
            System.IntPtr enumOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint0(this._nativePointer, enumerateWeakReferences, (void *)(&enumOut_), (*(void ***)this._nativePointer)[7]);
            enumOut = (enumOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.GCReferenceEnum(enumOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "types">No documentation.</param>
        /// <param name = "enumOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess5::EnumerateHandles([In] CorGCReferenceType types,[In] ICorDebugGCReferenceEnum** ppEnum)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess5::EnumerateHandles</unmanaged-short>
        public unsafe void EnumerateHandles(CorApi.Portable.CorGCReferenceType types, out CorApi.Portable.GCReferenceEnum enumOut)
        {
            System.IntPtr enumOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, unchecked ((System.Int32)types), (void *)(&enumOut_), (*(void ***)this._nativePointer)[8]);
            enumOut = (enumOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.GCReferenceEnum(enumOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "obj">No documentation.</param>
        /// <param name = "idRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess5::GetTypeID([In] unsigned longlong obj,[In] COR_TYPEID* pId)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess5::GetTypeID</unmanaged-short>
        public unsafe void GetTypeID(System.UInt64 obj, CorApi.Portable.CorTypeid idRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, obj, (void *)(&idRef), (*(void ***)this._nativePointer)[9]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "id">No documentation.</param>
        /// <param name = "typeOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess5::GetTypeForTypeID([In] COR_TYPEID id,[In] ICorDebugType** ppType)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess5::GetTypeForTypeID</unmanaged-short>
        public unsafe void GetTypeForTypeID(CorApi.Portable.CorTypeid id, out CorApi.Portable.Type typeOut)
        {
            System.IntPtr typeOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint0(this._nativePointer, id, (void *)(&typeOut_), (*(void ***)this._nativePointer)[10]);
            typeOut = (typeOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Type(typeOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "id">No documentation.</param>
        /// <param name = "layoutRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess5::GetArrayLayout([In] COR_TYPEID id,[In] COR_ARRAY_LAYOUT* pLayout)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess5::GetArrayLayout</unmanaged-short>
        public unsafe void GetArrayLayout(CorApi.Portable.CorTypeid id, ref CorApi.Portable.CorArrayLayout layoutRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *layoutRef_ = &layoutRef)
                __result__ = CorDebug.LocalInterop.Calliint0(this._nativePointer, id, (void *)(layoutRef_), (*(void ***)this._nativePointer)[11]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "id">No documentation.</param>
        /// <param name = "layoutRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess5::GetTypeLayout([In] COR_TYPEID id,[In] COR_TYPE_LAYOUT* pLayout)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess5::GetTypeLayout</unmanaged-short>
        public unsafe void GetTypeLayout(CorApi.Portable.CorTypeid id, ref CorApi.Portable.CorTypeLayout layoutRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *layoutRef_ = &layoutRef)
                __result__ = CorDebug.LocalInterop.Calliint0(this._nativePointer, id, (void *)(layoutRef_), (*(void ***)this._nativePointer)[12]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "id">No documentation.</param>
        /// <param name = "celt">No documentation.</param>
        /// <param name = "fields">No documentation.</param>
        /// <param name = "celtNeededRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess5::GetTypeFields([In] COR_TYPEID id,[In] unsigned int celt,[In] COR_FIELD* fields,[In] unsigned int* pceltNeeded)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess5::GetTypeFields</unmanaged-short>
        public unsafe void GetTypeFields(CorApi.Portable.CorTypeid id, System.UInt32 celt, ref CorApi.Portable.CorField fields, System.UInt32 celtNeededRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *fields_ = &fields)
                __result__ = CorDebug.LocalInterop.Calliint0(this._nativePointer, id, celt, (void *)(fields_), (void *)(&celtNeededRef), (*(void ***)this._nativePointer)[13]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "ePolicy">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess5::EnableNGENPolicy([In] CorDebugNGENPolicy ePolicy)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess5::EnableNGENPolicy</unmanaged-short>
        public unsafe void EnableNGENPolicy(CorApi.Portable.CorDebugNGENPolicy ePolicy)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, unchecked ((System.Int32)ePolicy), (*(void ***)this._nativePointer)[14]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("11588775-7205-4CEB-A41A-93753C3153E9")]
    public partial class Process6 : SharpGen.Runtime.ComObject
    {
        public Process6(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Process6(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Process6(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "recordRef">No documentation.</param>
        /// <param name = "countBytes">No documentation.</param>
        /// <param name = "format">No documentation.</param>
        /// <param name = "dwFlags">No documentation.</param>
        /// <param name = "dwThreadId">No documentation.</param>
        /// <param name = "eventOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess6::DecodeEvent([In] const unsigned char* pRecord,[In] unsigned int countBytes,[In] CorDebugRecordFormat format,[In] unsigned int dwFlags,[In] unsigned int dwThreadId,[In] ICorDebugDebugEvent** ppEvent)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess6::DecodeEvent</unmanaged-short>
        public unsafe void DecodeEvent(System.Byte recordRef, System.UInt32 countBytes, CorApi.Portable.CorDebugRecordFormat format, System.UInt32 dwFlags, System.UInt32 dwThreadId, out CorApi.Portable.DebugEvent eventOut)
        {
            System.IntPtr eventOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&recordRef), countBytes, unchecked ((System.Int32)format), dwFlags, dwThreadId, (void *)(&eventOut_), (*(void ***)this._nativePointer)[3]);
            eventOut = (eventOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.DebugEvent(eventOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "change">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess6::ProcessStateChanged([In] CorDebugStateChange change)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess6::ProcessStateChanged</unmanaged-short>
        public unsafe void ProcessStateChanged(CorApi.Portable.CorDebugStateChange change)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, unchecked ((System.Int32)change), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "codeAddress">No documentation.</param>
        /// <param name = "codeOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess6::GetCode([In] unsigned longlong codeAddress,[In] ICorDebugCode** ppCode)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess6::GetCode</unmanaged-short>
        public unsafe void GetCode(System.UInt64 codeAddress, out CorApi.Portable.Code codeOut)
        {
            System.IntPtr codeOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, codeAddress, (void *)(&codeOut_), (*(void ***)this._nativePointer)[5]);
            codeOut = (codeOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Code(codeOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "enableSplitting">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess6::EnableVirtualModuleSplitting([In] BOOL enableSplitting)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess6::EnableVirtualModuleSplitting</unmanaged-short>
        public unsafe void EnableVirtualModuleSplitting(SharpGen.Runtime.Win32.RawBool enableSplitting)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint0(this._nativePointer, enableSplitting, (*(void ***)this._nativePointer)[6]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "fIsAttached">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess6::MarkDebuggerAttached([In] BOOL fIsAttached)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess6::MarkDebuggerAttached</unmanaged-short>
        public unsafe void MarkDebuggerAttached(SharpGen.Runtime.Win32.RawBool fIsAttached)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint0(this._nativePointer, fIsAttached, (*(void ***)this._nativePointer)[7]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "szExportNameRef">No documentation.</param>
        /// <param name = "invokeKindRef">No documentation.</param>
        /// <param name = "invokePurposeRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess6::GetExportStepInfo([In] const wchar_t* pszExportName,[In] CorDebugCodeInvokeKind* pInvokeKind,[In] CorDebugCodeInvokePurpose* pInvokePurpose)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess6::GetExportStepInfo</unmanaged-short>
        public unsafe void GetExportStepInfo(System.String szExportNameRef, CorApi.Portable.CorDebugCodeInvokeKind invokeKindRef, CorApi.Portable.CorDebugCodeInvokePurpose invokePurposeRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szExportNameRef_ = szExportNameRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)szExportNameRef_), (void *)(&invokeKindRef), (void *)(&invokePurposeRef), (*(void ***)this._nativePointer)[8]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("9B2C54E4-119F-4D6F-B402-527603266D69")]
    public partial class Process7 : SharpGen.Runtime.ComObject
    {
        public Process7(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Process7(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Process7(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "flags">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess7::SetWriteableMetadataUpdateMode([In] WriteableMetadataUpdateMode flags)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess7::SetWriteableMetadataUpdateMode</unmanaged-short>
        public unsafe void SetWriteableMetadataUpdateMode(CorApi.Portable.WriteableMetadataUpdateMode flags)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, unchecked ((System.Int32)flags), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("2E6F28C1-85EB-4141-80AD-0A90944B9639")]
    public partial class Process8 : SharpGen.Runtime.ComObject
    {
        public Process8(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Process8(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Process8(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "enableExceptionsOutsideOfJMC">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcess8::EnableExceptionCallbacksOutsideOfMyCode([In] BOOL enableExceptionsOutsideOfJMC)</unmanaged>
        /// <unmanaged-short>ICorDebugProcess8::EnableExceptionCallbacksOutsideOfMyCode</unmanaged-short>
        public unsafe void EnableExceptionCallbacksOutsideOfMyCode(SharpGen.Runtime.Win32.RawBool enableExceptionsOutsideOfJMC)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint0(this._nativePointer, enableExceptionsOutsideOfJMC, (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("CC7BCB05-8A68-11d2-983C-0000F808342D")]
    public partial class ProcessEnum : CorApi.Portable.Enum
    {
        public ProcessEnum(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator ProcessEnum(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new ProcessEnum(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "celt">No documentation.</param>
        /// <param name = "rocessesRef">No documentation.</param>
        /// <param name = "celtFetchedRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugProcessEnum::Next([In] unsigned int celt,[Out, Buffer] ICorDebugProcess** processes,[Out] unsigned int* pceltFetched)</unmanaged>
        /// <unmanaged-short>ICorDebugProcessEnum::Next</unmanaged-short>
        public unsafe void Next(System.UInt32 celt, CorApi.Portable.Process[] rocessesRef, out System.UInt32 celtFetchedRef)
        {
            System.IntPtr*rocessesRef_ = stackalloc System.IntPtr[rocessesRef.Length];
            SharpGen.Runtime.Result __result__;
            fixed (void *celtFetchedRef_ = &celtFetchedRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, celt, (void *)(rocessesRef_), (void *)(celtFetchedRef_), (*(void ***)this._nativePointer)[7]);
            for (int i = 0; i < rocessesRef.Length; i++)
                rocessesRef[i] = (rocessesRef_[i] == System.IntPtr.Zero) ? null : new CorApi.Portable.Process(rocessesRef_[i]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("CC7BCAF9-8A68-11d2-983C-0000F808342D")]
    public partial class ReferenceValue : CorApi.Portable.Value
    {
        public ReferenceValue(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator ReferenceValue(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new ReferenceValue(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IsNull</unmanaged>
        /// <unmanaged-short>IsNull</unmanaged-short>
        public SharpGen.Runtime.Win32.RawBool IsNull
        {
            get
            {
                IsNull_(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "bNullRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugReferenceValue::IsNull([Out] BOOL* pbNull)</unmanaged>
        /// <unmanaged-short>ICorDebugReferenceValue::IsNull</unmanaged-short>
        internal unsafe void IsNull_(out SharpGen.Runtime.Win32.RawBool bNullRef)
        {
            bNullRef = new SharpGen.Runtime.Win32.RawBool();
            SharpGen.Runtime.Result __result__;
            fixed (void *bNullRef_ = &bNullRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(bNullRef_), (*(void ***)this._nativePointer)[7]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "valueRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugReferenceValue::GetValue([In] unsigned longlong* pValue)</unmanaged>
        /// <unmanaged-short>ICorDebugReferenceValue::GetValue</unmanaged-short>
        public unsafe void GetValue(System.UInt64 valueRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&valueRef), (*(void ***)this._nativePointer)[8]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "value">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugReferenceValue::SetValue([In] unsigned longlong value)</unmanaged>
        /// <unmanaged-short>ICorDebugReferenceValue::SetValue</unmanaged-short>
        public unsafe void SetValue(System.UInt64 value)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, value, (*(void ***)this._nativePointer)[9]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugReferenceValue::Dereference([Out] ICorDebugValue** ppValue)</unmanaged>
        /// <unmanaged-short>ICorDebugReferenceValue::Dereference</unmanaged-short>
        public unsafe CorApi.Portable.Value Dereference()
        {
            CorApi.Portable.Value valueOut;
            System.IntPtr valueOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&valueOut_), (*(void ***)this._nativePointer)[10]);
            valueOut = (valueOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Value(valueOut_);
            __result__.CheckError();
            return valueOut;
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "valueOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugReferenceValue::DereferenceStrong([In] ICorDebugValue** ppValue)</unmanaged>
        /// <unmanaged-short>ICorDebugReferenceValue::DereferenceStrong</unmanaged-short>
        public unsafe void DereferenceStrong(out CorApi.Portable.Value valueOut)
        {
            System.IntPtr valueOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&valueOut_), (*(void ***)this._nativePointer)[11]);
            valueOut = (valueOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Value(valueOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("CC7BCB0B-8A68-11d2-983C-0000F808342D")]
    public partial class RegisterSet : SharpGen.Runtime.ComObject
    {
        public RegisterSet(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator RegisterSet(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new RegisterSet(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "availableRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugRegisterSet::GetRegistersAvailable([In] unsigned longlong* pAvailable)</unmanaged>
        /// <unmanaged-short>ICorDebugRegisterSet::GetRegistersAvailable</unmanaged-short>
        public unsafe void GetRegistersAvailable(System.UInt64 availableRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&availableRef), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "mask">No documentation.</param>
        /// <param name = "regCount">No documentation.</param>
        /// <param name = "regBuffer">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugRegisterSet::GetRegisters([In] unsigned longlong mask,[In] unsigned int regCount,[In] unsigned longlong* regBuffer)</unmanaged>
        /// <unmanaged-short>ICorDebugRegisterSet::GetRegisters</unmanaged-short>
        public unsafe void GetRegisters(System.UInt64 mask, System.UInt32 regCount, System.UInt64 regBuffer)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, mask, regCount, (void *)(&regBuffer), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "mask">No documentation.</param>
        /// <param name = "regCount">No documentation.</param>
        /// <param name = "regBuffer">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugRegisterSet::SetRegisters([In] unsigned longlong mask,[In] unsigned int regCount,[In] unsigned longlong* regBuffer)</unmanaged>
        /// <unmanaged-short>ICorDebugRegisterSet::SetRegisters</unmanaged-short>
        public unsafe void SetRegisters(System.UInt64 mask, System.UInt32 regCount, System.UInt64 regBuffer)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, mask, regCount, (void *)(&regBuffer), (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "contextSize">No documentation.</param>
        /// <param name = "context">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugRegisterSet::GetThreadContext([In] unsigned int contextSize,[In] unsigned char* context)</unmanaged>
        /// <unmanaged-short>ICorDebugRegisterSet::GetThreadContext</unmanaged-short>
        public unsafe void GetThreadContext(System.UInt32 contextSize, System.Byte context)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, contextSize, (void *)(&context), (*(void ***)this._nativePointer)[6]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "contextSize">No documentation.</param>
        /// <param name = "context">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugRegisterSet::SetThreadContext([In] unsigned int contextSize,[In] unsigned char* context)</unmanaged>
        /// <unmanaged-short>ICorDebugRegisterSet::SetThreadContext</unmanaged-short>
        public unsafe void SetThreadContext(System.UInt32 contextSize, System.Byte context)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, contextSize, (void *)(&context), (*(void ***)this._nativePointer)[7]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("6DC7BA3F-89BA-4459-9EC1-9D60937B468D")]
    public partial class RegisterSet2 : SharpGen.Runtime.ComObject
    {
        public RegisterSet2(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator RegisterSet2(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new RegisterSet2(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "numChunks">No documentation.</param>
        /// <param name = "availableRegChunks">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugRegisterSet2::GetRegistersAvailable([In] unsigned int numChunks,[In] unsigned char* availableRegChunks)</unmanaged>
        /// <unmanaged-short>ICorDebugRegisterSet2::GetRegistersAvailable</unmanaged-short>
        public unsafe void GetRegistersAvailable(System.UInt32 numChunks, System.Byte availableRegChunks)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, numChunks, (void *)(&availableRegChunks), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "maskCount">No documentation.</param>
        /// <param name = "mask">No documentation.</param>
        /// <param name = "regCount">No documentation.</param>
        /// <param name = "regBuffer">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugRegisterSet2::GetRegisters([In] unsigned int maskCount,[In] unsigned char* mask,[In] unsigned int regCount,[In] unsigned longlong* regBuffer)</unmanaged>
        /// <unmanaged-short>ICorDebugRegisterSet2::GetRegisters</unmanaged-short>
        public unsafe void GetRegisters(System.UInt32 maskCount, System.Byte mask, System.UInt32 regCount, System.UInt64 regBuffer)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, maskCount, (void *)(&mask), regCount, (void *)(&regBuffer), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "maskCount">No documentation.</param>
        /// <param name = "mask">No documentation.</param>
        /// <param name = "regCount">No documentation.</param>
        /// <param name = "regBuffer">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugRegisterSet2::SetRegisters([In] unsigned int maskCount,[In] unsigned char* mask,[In] unsigned int regCount,[In] unsigned longlong* regBuffer)</unmanaged>
        /// <unmanaged-short>ICorDebugRegisterSet2::SetRegisters</unmanaged-short>
        public unsafe void SetRegisters(System.UInt32 maskCount, System.Byte mask, System.UInt32 regCount, System.UInt64 regBuffer)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, maskCount, (void *)(&mask), regCount, (void *)(&regBuffer), (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("D5EBB8E2-7BBE-4c1d-98A6-A3C04CBDEF64")]
    public partial class RemoteDebugger : SharpGen.Runtime.ComObject
    {
        public RemoteDebugger(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator RemoteDebugger(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new RemoteDebugger(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "remoteTargetRef">No documentation.</param>
        /// <param name = "dwProcessId">No documentation.</param>
        /// <param name = "fWin32Attach">No documentation.</param>
        /// <param name = "processOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugRemote::DebugActiveProcessEx([In] ICorDebugRemoteTarget* pRemoteTarget,[In] unsigned int dwProcessId,[In] BOOL fWin32Attach,[In] ICorDebugProcess** ppProcess)</unmanaged>
        /// <unmanaged-short>ICorDebugRemote::DebugActiveProcessEx</unmanaged-short>
        public unsafe void DebugActiveProcessEx(CorApi.Portable.RemoteTarget remoteTargetRef, System.UInt32 dwProcessId, SharpGen.Runtime.Win32.RawBool fWin32Attach, out CorApi.Portable.Process processOut)
        {
            System.IntPtr processOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint0(this._nativePointer, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.RemoteTarget>(remoteTargetRef))), dwProcessId, fWin32Attach, (void *)(&processOut_), (*(void ***)this._nativePointer)[4]);
            processOut = (processOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Process(processOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("C3ED8383-5A49-4cf5-B4B7-01864D9E582D")]
    public partial class RemoteTarget : SharpGen.Runtime.ComObject
    {
        public RemoteTarget(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator RemoteTarget(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new RemoteTarget(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cchHostName">No documentation.</param>
        /// <param name = "cchHostNameRef">No documentation.</param>
        /// <param name = "szHostName">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugRemoteTarget::GetHostName([In] unsigned int cchHostName,[Out] unsigned int* pcchHostName,[Out, Buffer, Optional] wchar_t* szHostName)</unmanaged>
        /// <unmanaged-short>ICorDebugRemoteTarget::GetHostName</unmanaged-short>
        public unsafe void GetHostName(System.UInt32 cchHostName, out System.UInt32 cchHostNameRef, System.IntPtr szHostName)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *cchHostNameRef_ = &cchHostNameRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cchHostName, (void *)(cchHostNameRef_), (void *)((void *)szHostName), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("879CAC0A-4A53-4668-B8E3-CB8473CB187F")]
    public partial class RuntimeUnwindableFrame : CorApi.Portable.Frame
    {
        public RuntimeUnwindableFrame(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator RuntimeUnwindableFrame(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new RuntimeUnwindableFrame(nativePtr);
    }

    [System.Runtime.InteropServices.GuidAttribute("A0647DE9-55DE-4816-929C-385271C64CF7")]
    public partial class StackWalk : SharpGen.Runtime.ComObject
    {
        public StackWalk(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator StackWalk(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new StackWalk(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetFrame</unmanaged>
        /// <unmanaged-short>GetFrame</unmanaged-short>
        public CorApi.Portable.Frame Frame
        {
            get
            {
                GetFrame(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "contextFlags">No documentation.</param>
        /// <param name = "contextBufSize">No documentation.</param>
        /// <param name = "contextSize">No documentation.</param>
        /// <param name = "contextBuf">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugStackWalk::GetContext([In] unsigned int contextFlags,[In] unsigned int contextBufSize,[In] unsigned int* contextSize,[In] unsigned char* contextBuf)</unmanaged>
        /// <unmanaged-short>ICorDebugStackWalk::GetContext</unmanaged-short>
        public unsafe void GetContext(System.UInt32 contextFlags, System.UInt32 contextBufSize, System.UInt32 contextSize, System.Byte contextBuf)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, contextFlags, contextBufSize, (void *)(&contextSize), (void *)(&contextBuf), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "flag">No documentation.</param>
        /// <param name = "contextSize">No documentation.</param>
        /// <param name = "context">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugStackWalk::SetContext([In] CorDebugSetContextFlag flag,[In] unsigned int contextSize,[In] unsigned char* context)</unmanaged>
        /// <unmanaged-short>ICorDebugStackWalk::SetContext</unmanaged-short>
        public unsafe void SetContext(CorApi.Portable.CorDebugSetContextFlag flag, System.UInt32 contextSize, System.Byte context)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, unchecked ((System.Int32)flag), contextSize, (void *)(&context), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugStackWalk::Next()</unmanaged>
        /// <unmanaged-short>ICorDebugStackWalk::Next</unmanaged-short>
        public unsafe void Next()
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "frameRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugStackWalk::GetFrame([In] ICorDebugFrame** pFrame)</unmanaged>
        /// <unmanaged-short>ICorDebugStackWalk::GetFrame</unmanaged-short>
        internal unsafe void GetFrame(out CorApi.Portable.Frame frameRef)
        {
            System.IntPtr frameRef_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&frameRef_), (*(void ***)this._nativePointer)[6]);
            frameRef = (frameRef_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Frame(frameRef_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("CBF9DA63-F68D-4BBB-A21C-15A45EAADF5B")]
    public partial class StaticFieldSymbol : SharpGen.Runtime.ComObject
    {
        public StaticFieldSymbol(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator StaticFieldSymbol(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new StaticFieldSymbol(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cchName">No documentation.</param>
        /// <param name = "cchNameRef">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugStaticFieldSymbol::GetName([In] unsigned int cchName,[In] unsigned int* pcchName,[In] wchar_t* szName)</unmanaged>
        /// <unmanaged-short>ICorDebugStaticFieldSymbol::GetName</unmanaged-short>
        public unsafe void GetName(System.UInt32 cchName, System.UInt32 cchNameRef, System.String szName)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cchName, (void *)(&cchNameRef), (void *)((void *)szName_), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cbSizeRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugStaticFieldSymbol::GetSize([In] unsigned int* pcbSize)</unmanaged>
        /// <unmanaged-short>ICorDebugStaticFieldSymbol::GetSize</unmanaged-short>
        public unsafe void GetSize(System.UInt32 cbSizeRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&cbSizeRef), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "rVARef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugStaticFieldSymbol::GetAddress([In] unsigned longlong* pRVA)</unmanaged>
        /// <unmanaged-short>ICorDebugStaticFieldSymbol::GetAddress</unmanaged-short>
        public unsafe void GetAddress(System.UInt64 rVARef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&rVARef), (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("CC7BCAEC-8A68-11d2-983C-0000F808342D")]
    public partial class Stepper : SharpGen.Runtime.ComObject
    {
        public Stepper(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Stepper(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Stepper(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>IsActive</unmanaged>
        /// <unmanaged-short>IsActive</unmanaged-short>
        public SharpGen.Runtime.Win32.RawBool IsActive
        {
            get
            {
                IsActive_(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "bActiveRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugStepper::IsActive([Out] BOOL* pbActive)</unmanaged>
        /// <unmanaged-short>ICorDebugStepper::IsActive</unmanaged-short>
        internal unsafe void IsActive_(out SharpGen.Runtime.Win32.RawBool bActiveRef)
        {
            bActiveRef = new SharpGen.Runtime.Win32.RawBool();
            SharpGen.Runtime.Result __result__;
            fixed (void *bActiveRef_ = &bActiveRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(bActiveRef_), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugStepper::Deactivate()</unmanaged>
        /// <unmanaged-short>ICorDebugStepper::Deactivate</unmanaged-short>
        public unsafe void Deactivate()
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "mask">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugStepper::SetInterceptMask([In] CorDebugIntercept mask)</unmanaged>
        /// <unmanaged-short>ICorDebugStepper::SetInterceptMask</unmanaged-short>
        public unsafe void SetInterceptMask(CorApi.Portable.CorDebugIntercept mask)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, unchecked ((System.Int32)mask), (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "mask">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugStepper::SetUnmappedStopMask([In] CorDebugUnmappedStop mask)</unmanaged>
        /// <unmanaged-short>ICorDebugStepper::SetUnmappedStopMask</unmanaged-short>
        public unsafe void SetUnmappedStopMask(CorApi.Portable.CorDebugUnmappedStop mask)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, unchecked ((System.Int32)mask), (*(void ***)this._nativePointer)[6]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "bStepIn">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugStepper::Step([In] BOOL bStepIn)</unmanaged>
        /// <unmanaged-short>ICorDebugStepper::Step</unmanaged-short>
        public unsafe void Step(SharpGen.Runtime.Win32.RawBool bStepIn)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint0(this._nativePointer, bStepIn, (*(void ***)this._nativePointer)[7]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "bStepIn">No documentation.</param>
        /// <param name = "ranges">No documentation.</param>
        /// <param name = "cRangeCount">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugStepper::StepRange([In] BOOL bStepIn,[In, Buffer] COR_DEBUG_STEP_RANGE* ranges,[In] unsigned int cRangeCount)</unmanaged>
        /// <unmanaged-short>ICorDebugStepper::StepRange</unmanaged-short>
        public unsafe void StepRange(SharpGen.Runtime.Win32.RawBool bStepIn, CorApi.Portable.CorDebugStepRange[] ranges, System.UInt32 cRangeCount)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *ranges_ = ranges)
                __result__ = CorDebug.LocalInterop.Calliint0(this._nativePointer, bStepIn, (void *)(ranges_), cRangeCount, (*(void ***)this._nativePointer)[8]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugStepper::StepOut()</unmanaged>
        /// <unmanaged-short>ICorDebugStepper::StepOut</unmanaged-short>
        public unsafe void StepOut()
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (*(void ***)this._nativePointer)[9]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "bIL">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugStepper::SetRangeIL([In] BOOL bIL)</unmanaged>
        /// <unmanaged-short>ICorDebugStepper::SetRangeIL</unmanaged-short>
        public unsafe void SetRangeIL(SharpGen.Runtime.Win32.RawBool bIL)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint0(this._nativePointer, bIL, (*(void ***)this._nativePointer)[10]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("C5B6E9C3-E7D1-4a8e-873B-7F047F0706F7")]
    public partial class Stepper2 : SharpGen.Runtime.ComObject
    {
        public Stepper2(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Stepper2(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Stepper2(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "fIsJMCStepper">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugStepper2::SetJMC([In] BOOL fIsJMCStepper)</unmanaged>
        /// <unmanaged-short>ICorDebugStepper2::SetJMC</unmanaged-short>
        public unsafe void SetJMC(SharpGen.Runtime.Win32.RawBool fIsJMCStepper)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint0(this._nativePointer, fIsJMCStepper, (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("CC7BCB04-8A68-11d2-983C-0000F808342D")]
    public partial class StepperEnum : CorApi.Portable.Enum
    {
        public StepperEnum(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator StepperEnum(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new StepperEnum(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "celt">No documentation.</param>
        /// <param name = "steppers">No documentation.</param>
        /// <param name = "celtFetchedRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugStepperEnum::Next([In] unsigned int celt,[In] ICorDebugStepper** steppers,[In] unsigned int* pceltFetched)</unmanaged>
        /// <unmanaged-short>ICorDebugStepperEnum::Next</unmanaged-short>
        public unsafe void Next(System.UInt32 celt, out CorApi.Portable.Stepper steppers, System.UInt32 celtFetchedRef)
        {
            System.IntPtr steppers_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, celt, (void *)(&steppers_), (void *)(&celtFetchedRef), (*(void ***)this._nativePointer)[7]);
            steppers = (steppers_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Stepper(steppers_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("CC7BCAFD-8A68-11d2-983C-0000F808342D")]
    public partial class StringValue : CorApi.Portable.HeapValue
    {
        public StringValue(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator StringValue(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new StringValue(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetLength</unmanaged>
        /// <unmanaged-short>GetLength</unmanaged-short>
        public System.UInt32 Length
        {
            get
            {
                GetLength(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cchStringRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugStringValue::GetLength([Out] unsigned int* pcchString)</unmanaged>
        /// <unmanaged-short>ICorDebugStringValue::GetLength</unmanaged-short>
        internal unsafe void GetLength(out System.UInt32 cchStringRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *cchStringRef_ = &cchStringRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(cchStringRef_), (*(void ***)this._nativePointer)[9]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cchString">No documentation.</param>
        /// <param name = "cchStringRef">No documentation.</param>
        /// <param name = "szString">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugStringValue::GetString([In] unsigned int cchString,[Out] unsigned int* pcchString,[Out] wchar_t* szString)</unmanaged>
        /// <unmanaged-short>ICorDebugStringValue::GetString</unmanaged-short>
        public unsafe void GetString(System.UInt32 cchString, out System.UInt32 cchStringRef, System.IntPtr szString)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *cchStringRef_ = &cchStringRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cchString, (void *)(cchStringRef_), (void *)((void *)szString), (*(void ***)this._nativePointer)[10]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("3948A999-FD8A-4C38-A708-8A71E9B04DBB")]
    public partial class SymbolProvider : SharpGen.Runtime.ComObject
    {
        public SymbolProvider(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator SymbolProvider(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new SymbolProvider(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetAssemblyImageMetadata</unmanaged>
        /// <unmanaged-short>GetAssemblyImageMetadata</unmanaged-short>
        public CorApi.Portable.MemoryBuffer AssemblyImageMetadata
        {
            get
            {
                GetAssemblyImageMetadata(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cbSignature">No documentation.</param>
        /// <param name = "typeSig">No documentation.</param>
        /// <param name = "cRequestedSymbols">No documentation.</param>
        /// <param name = "cFetchedSymbolsRef">No documentation.</param>
        /// <param name = "symbolsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugSymbolProvider::GetStaticFieldSymbols([In] unsigned int cbSignature,[In] unsigned char* typeSig,[In] unsigned int cRequestedSymbols,[In] unsigned int* pcFetchedSymbols,[In] ICorDebugStaticFieldSymbol** pSymbols)</unmanaged>
        /// <unmanaged-short>ICorDebugSymbolProvider::GetStaticFieldSymbols</unmanaged-short>
        public unsafe void GetStaticFieldSymbols(System.UInt32 cbSignature, System.Byte typeSig, System.UInt32 cRequestedSymbols, System.UInt32 cFetchedSymbolsRef, out CorApi.Portable.StaticFieldSymbol symbolsRef)
        {
            System.IntPtr symbolsRef_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cbSignature, (void *)(&typeSig), cRequestedSymbols, (void *)(&cFetchedSymbolsRef), (void *)(&symbolsRef_), (*(void ***)this._nativePointer)[3]);
            symbolsRef = (symbolsRef_ == System.IntPtr.Zero) ? null : new CorApi.Portable.StaticFieldSymbol(symbolsRef_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cbSignature">No documentation.</param>
        /// <param name = "typeSig">No documentation.</param>
        /// <param name = "cRequestedSymbols">No documentation.</param>
        /// <param name = "cFetchedSymbolsRef">No documentation.</param>
        /// <param name = "symbolsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugSymbolProvider::GetInstanceFieldSymbols([In] unsigned int cbSignature,[In] unsigned char* typeSig,[In] unsigned int cRequestedSymbols,[In] unsigned int* pcFetchedSymbols,[In] ICorDebugInstanceFieldSymbol** pSymbols)</unmanaged>
        /// <unmanaged-short>ICorDebugSymbolProvider::GetInstanceFieldSymbols</unmanaged-short>
        public unsafe void GetInstanceFieldSymbols(System.UInt32 cbSignature, System.Byte typeSig, System.UInt32 cRequestedSymbols, System.UInt32 cFetchedSymbolsRef, out CorApi.Portable.InstanceFieldSymbol symbolsRef)
        {
            System.IntPtr symbolsRef_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cbSignature, (void *)(&typeSig), cRequestedSymbols, (void *)(&cFetchedSymbolsRef), (void *)(&symbolsRef_), (*(void ***)this._nativePointer)[4]);
            symbolsRef = (symbolsRef_ == System.IntPtr.Zero) ? null : new CorApi.Portable.InstanceFieldSymbol(symbolsRef_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "nativeRVA">No documentation.</param>
        /// <param name = "cRequestedSymbols">No documentation.</param>
        /// <param name = "cFetchedSymbolsRef">No documentation.</param>
        /// <param name = "symbolsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugSymbolProvider::GetMethodLocalSymbols([In] unsigned int nativeRVA,[In] unsigned int cRequestedSymbols,[In] unsigned int* pcFetchedSymbols,[In] ICorDebugVariableSymbol** pSymbols)</unmanaged>
        /// <unmanaged-short>ICorDebugSymbolProvider::GetMethodLocalSymbols</unmanaged-short>
        public unsafe void GetMethodLocalSymbols(System.UInt32 nativeRVA, System.UInt32 cRequestedSymbols, System.UInt32 cFetchedSymbolsRef, out CorApi.Portable.VariableSymbol symbolsRef)
        {
            System.IntPtr symbolsRef_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, nativeRVA, cRequestedSymbols, (void *)(&cFetchedSymbolsRef), (void *)(&symbolsRef_), (*(void ***)this._nativePointer)[5]);
            symbolsRef = (symbolsRef_ == System.IntPtr.Zero) ? null : new CorApi.Portable.VariableSymbol(symbolsRef_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "nativeRVA">No documentation.</param>
        /// <param name = "cRequestedSymbols">No documentation.</param>
        /// <param name = "cFetchedSymbolsRef">No documentation.</param>
        /// <param name = "symbolsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugSymbolProvider::GetMethodParameterSymbols([In] unsigned int nativeRVA,[In] unsigned int cRequestedSymbols,[In] unsigned int* pcFetchedSymbols,[In] ICorDebugVariableSymbol** pSymbols)</unmanaged>
        /// <unmanaged-short>ICorDebugSymbolProvider::GetMethodParameterSymbols</unmanaged-short>
        public unsafe void GetMethodParameterSymbols(System.UInt32 nativeRVA, System.UInt32 cRequestedSymbols, System.UInt32 cFetchedSymbolsRef, out CorApi.Portable.VariableSymbol symbolsRef)
        {
            System.IntPtr symbolsRef_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, nativeRVA, cRequestedSymbols, (void *)(&cFetchedSymbolsRef), (void *)(&symbolsRef_), (*(void ***)this._nativePointer)[6]);
            symbolsRef = (symbolsRef_ == System.IntPtr.Zero) ? null : new CorApi.Portable.VariableSymbol(symbolsRef_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cRequestedRecords">No documentation.</param>
        /// <param name = "cFetchedRecordsRef">No documentation.</param>
        /// <param name = "recordsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugSymbolProvider::GetMergedAssemblyRecords([In] unsigned int cRequestedRecords,[In] unsigned int* pcFetchedRecords,[In] ICorDebugMergedAssemblyRecord** pRecords)</unmanaged>
        /// <unmanaged-short>ICorDebugSymbolProvider::GetMergedAssemblyRecords</unmanaged-short>
        public unsafe void GetMergedAssemblyRecords(System.UInt32 cRequestedRecords, System.UInt32 cFetchedRecordsRef, out CorApi.Portable.MergedAssemblyRecord recordsRef)
        {
            System.IntPtr recordsRef_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cRequestedRecords, (void *)(&cFetchedRecordsRef), (void *)(&recordsRef_), (*(void ***)this._nativePointer)[7]);
            recordsRef = (recordsRef_ == System.IntPtr.Zero) ? null : new CorApi.Portable.MergedAssemblyRecord(recordsRef_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "codeRva">No documentation.</param>
        /// <param name = "methodTokenRef">No documentation.</param>
        /// <param name = "cGenericParamsRef">No documentation.</param>
        /// <param name = "cbSignature">No documentation.</param>
        /// <param name = "cbSignatureRef">No documentation.</param>
        /// <param name = "signature">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugSymbolProvider::GetMethodProps([In] unsigned int codeRva,[In] unsigned int* pMethodToken,[In] unsigned int* pcGenericParams,[In] unsigned int cbSignature,[In] unsigned int* pcbSignature,[In] unsigned char* signature)</unmanaged>
        /// <unmanaged-short>ICorDebugSymbolProvider::GetMethodProps</unmanaged-short>
        public unsafe void GetMethodProps(System.UInt32 codeRva, System.UInt32 methodTokenRef, System.UInt32 cGenericParamsRef, System.UInt32 cbSignature, System.UInt32 cbSignatureRef, System.Byte signature)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, codeRva, (void *)(&methodTokenRef), (void *)(&cGenericParamsRef), cbSignature, (void *)(&cbSignatureRef), (void *)(&signature), (*(void ***)this._nativePointer)[8]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "vtableRva">No documentation.</param>
        /// <param name = "cbSignature">No documentation.</param>
        /// <param name = "cbSignatureRef">No documentation.</param>
        /// <param name = "signature">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugSymbolProvider::GetTypeProps([In] unsigned int vtableRva,[In] unsigned int cbSignature,[In] unsigned int* pcbSignature,[In] unsigned char* signature)</unmanaged>
        /// <unmanaged-short>ICorDebugSymbolProvider::GetTypeProps</unmanaged-short>
        public unsafe void GetTypeProps(System.UInt32 vtableRva, System.UInt32 cbSignature, System.UInt32 cbSignatureRef, System.Byte signature)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, vtableRva, cbSignature, (void *)(&cbSignatureRef), (void *)(&signature), (*(void ***)this._nativePointer)[9]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "codeRva">No documentation.</param>
        /// <param name = "codeStartAddressRef">No documentation.</param>
        /// <param name = "codeSizeRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugSymbolProvider::GetCodeRange([In] unsigned int codeRva,[In] unsigned int* pCodeStartAddress,[In] unsigned int* pCodeSize)</unmanaged>
        /// <unmanaged-short>ICorDebugSymbolProvider::GetCodeRange</unmanaged-short>
        public unsafe void GetCodeRange(System.UInt32 codeRva, System.UInt32 codeStartAddressRef, System.UInt32 codeSizeRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, codeRva, (void *)(&codeStartAddressRef), (void *)(&codeSizeRef), (*(void ***)this._nativePointer)[10]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "rva">No documentation.</param>
        /// <param name = "length">No documentation.</param>
        /// <param name = "memoryBufferOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugSymbolProvider::GetAssemblyImageBytes([In] unsigned longlong rva,[In] unsigned int length,[In] ICorDebugMemoryBuffer** ppMemoryBuffer)</unmanaged>
        /// <unmanaged-short>ICorDebugSymbolProvider::GetAssemblyImageBytes</unmanaged-short>
        public unsafe void GetAssemblyImageBytes(System.UInt64 rva, System.UInt32 length, out CorApi.Portable.MemoryBuffer memoryBufferOut)
        {
            System.IntPtr memoryBufferOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, rva, length, (void *)(&memoryBufferOut_), (*(void ***)this._nativePointer)[11]);
            memoryBufferOut = (memoryBufferOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.MemoryBuffer(memoryBufferOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cbSignature">No documentation.</param>
        /// <param name = "typeSig">No documentation.</param>
        /// <param name = "objectSizeRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugSymbolProvider::GetObjectSize([In] unsigned int cbSignature,[In] unsigned char* typeSig,[In] unsigned int* pObjectSize)</unmanaged>
        /// <unmanaged-short>ICorDebugSymbolProvider::GetObjectSize</unmanaged-short>
        public unsafe void GetObjectSize(System.UInt32 cbSignature, System.Byte typeSig, System.UInt32 objectSizeRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cbSignature, (void *)(&typeSig), (void *)(&objectSizeRef), (*(void ***)this._nativePointer)[12]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "memoryBufferOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugSymbolProvider::GetAssemblyImageMetadata([In] ICorDebugMemoryBuffer** ppMemoryBuffer)</unmanaged>
        /// <unmanaged-short>ICorDebugSymbolProvider::GetAssemblyImageMetadata</unmanaged-short>
        internal unsafe void GetAssemblyImageMetadata(out CorApi.Portable.MemoryBuffer memoryBufferOut)
        {
            System.IntPtr memoryBufferOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&memoryBufferOut_), (*(void ***)this._nativePointer)[13]);
            memoryBufferOut = (memoryBufferOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.MemoryBuffer(memoryBufferOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("F9801807-4764-4330-9E67-4F685094165E")]
    public partial class SymbolProvider2 : SharpGen.Runtime.ComObject
    {
        public SymbolProvider2(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator SymbolProvider2(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new SymbolProvider2(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetGenericDictionaryInfo</unmanaged>
        /// <unmanaged-short>GetGenericDictionaryInfo</unmanaged-short>
        public CorApi.Portable.MemoryBuffer GenericDictionaryInfo
        {
            get
            {
                GetGenericDictionaryInfo(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "memoryBufferOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugSymbolProvider2::GetGenericDictionaryInfo([In] ICorDebugMemoryBuffer** ppMemoryBuffer)</unmanaged>
        /// <unmanaged-short>ICorDebugSymbolProvider2::GetGenericDictionaryInfo</unmanaged-short>
        internal unsafe void GetGenericDictionaryInfo(out CorApi.Portable.MemoryBuffer memoryBufferOut)
        {
            System.IntPtr memoryBufferOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&memoryBufferOut_), (*(void ***)this._nativePointer)[3]);
            memoryBufferOut = (memoryBufferOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.MemoryBuffer(memoryBufferOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "codeRva">No documentation.</param>
        /// <param name = "codeStartRvaRef">No documentation.</param>
        /// <param name = "parentFrameStartRvaRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugSymbolProvider2::GetFrameProps([In] unsigned int codeRva,[In] unsigned int* pCodeStartRva,[In] unsigned int* pParentFrameStartRva)</unmanaged>
        /// <unmanaged-short>ICorDebugSymbolProvider2::GetFrameProps</unmanaged-short>
        public unsafe void GetFrameProps(System.UInt32 codeRva, System.UInt32 codeStartRvaRef, System.UInt32 parentFrameStartRvaRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, codeRva, (void *)(&codeStartRvaRef), (void *)(&parentFrameStartRvaRef), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("938c6d66-7fb6-4f69-b389-425b8987329b")]
    public partial class Thread : SharpGen.Runtime.ComObject
    {
        public Thread(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Thread(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Thread(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetID</unmanaged>
        /// <unmanaged-short>GetID</unmanaged-short>
        public System.UInt32 ID
        {
            get
            {
                GetID(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetAppDomain</unmanaged>
        /// <unmanaged-short>GetAppDomain</unmanaged-short>
        public CorApi.Portable.AppDomain AppDomain
        {
            get
            {
                GetAppDomain(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetCurrentException</unmanaged>
        /// <unmanaged-short>GetCurrentException</unmanaged-short>
        public CorApi.Portable.Value CurrentException
        {
            get
            {
                GetCurrentException(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetActiveChain</unmanaged>
        /// <unmanaged-short>GetActiveChain</unmanaged-short>
        public CorApi.Portable.Chain ActiveChain
        {
            get
            {
                GetActiveChain(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetActiveFrame</unmanaged>
        /// <unmanaged-short>GetActiveFrame</unmanaged-short>
        public CorApi.Portable.Frame ActiveFrame
        {
            get
            {
                GetActiveFrame(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetRegisterSet</unmanaged>
        /// <unmanaged-short>GetRegisterSet</unmanaged-short>
        public CorApi.Portable.RegisterSet RegisterSet
        {
            get
            {
                GetRegisterSet(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetObjectW</unmanaged>
        /// <unmanaged-short>GetObjectW</unmanaged-short>
        public CorApi.Portable.Value ObjectW
        {
            get
            {
                GetObjectW(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "processOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugThread::GetProcess([In] ICorDebugProcess** ppProcess)</unmanaged>
        /// <unmanaged-short>ICorDebugThread::GetProcess</unmanaged-short>
        public unsafe void GetProcess(out CorApi.Portable.Process processOut)
        {
            System.IntPtr processOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&processOut_), (*(void ***)this._nativePointer)[3]);
            processOut = (processOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Process(processOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "dwThreadIdRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugThread::GetID([Out] unsigned int* pdwThreadId)</unmanaged>
        /// <unmanaged-short>ICorDebugThread::GetID</unmanaged-short>
        internal unsafe void GetID(out System.UInt32 dwThreadIdRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *dwThreadIdRef_ = &dwThreadIdRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(dwThreadIdRef_), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "hThreadHandleRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugThread::GetHandle([In] void** phThreadHandle)</unmanaged>
        /// <unmanaged-short>ICorDebugThread::GetHandle</unmanaged-short>
        public unsafe void GetHandle(System.IntPtr hThreadHandleRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)hThreadHandleRef), (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "appDomainOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugThread::GetAppDomain([In] ICorDebugAppDomain** ppAppDomain)</unmanaged>
        /// <unmanaged-short>ICorDebugThread::GetAppDomain</unmanaged-short>
        internal unsafe void GetAppDomain(out CorApi.Portable.AppDomain appDomainOut)
        {
            System.IntPtr appDomainOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&appDomainOut_), (*(void ***)this._nativePointer)[6]);
            appDomainOut = (appDomainOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.AppDomain(appDomainOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "state">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugThread::SetDebugState([In] CorDebugThreadState state)</unmanaged>
        /// <unmanaged-short>ICorDebugThread::SetDebugState</unmanaged-short>
        public unsafe void SetDebugState(CorApi.Portable.CorDebugThreadState state)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, unchecked ((System.Int32)state), (*(void ***)this._nativePointer)[7]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "stateRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugThread::GetDebugState([In] CorDebugThreadState* pState)</unmanaged>
        /// <unmanaged-short>ICorDebugThread::GetDebugState</unmanaged-short>
        public unsafe void GetDebugState(CorApi.Portable.CorDebugThreadState stateRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&stateRef), (*(void ***)this._nativePointer)[8]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "stateRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugThread::GetUserState([In] CorDebugUserState* pState)</unmanaged>
        /// <unmanaged-short>ICorDebugThread::GetUserState</unmanaged-short>
        public unsafe void GetUserState(CorApi.Portable.CorDebugUserState stateRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&stateRef), (*(void ***)this._nativePointer)[9]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "exceptionObjectOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugThread::GetCurrentException([In] ICorDebugValue** ppExceptionObject)</unmanaged>
        /// <unmanaged-short>ICorDebugThread::GetCurrentException</unmanaged-short>
        internal unsafe void GetCurrentException(out CorApi.Portable.Value exceptionObjectOut)
        {
            System.IntPtr exceptionObjectOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&exceptionObjectOut_), (*(void ***)this._nativePointer)[10]);
            exceptionObjectOut = (exceptionObjectOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Value(exceptionObjectOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugThread::ClearCurrentException()</unmanaged>
        /// <unmanaged-short>ICorDebugThread::ClearCurrentException</unmanaged-short>
        public unsafe void ClearCurrentException()
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (*(void ***)this._nativePointer)[11]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "stepperOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugThread::CreateStepper([In] ICorDebugStepper** ppStepper)</unmanaged>
        /// <unmanaged-short>ICorDebugThread::CreateStepper</unmanaged-short>
        public unsafe void CreateStepper(out CorApi.Portable.Stepper stepperOut)
        {
            System.IntPtr stepperOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&stepperOut_), (*(void ***)this._nativePointer)[12]);
            stepperOut = (stepperOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Stepper(stepperOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "chainsOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugThread::EnumerateChains([In] ICorDebugChainEnum** ppChains)</unmanaged>
        /// <unmanaged-short>ICorDebugThread::EnumerateChains</unmanaged-short>
        public unsafe void EnumerateChains(out CorApi.Portable.ChainEnum chainsOut)
        {
            System.IntPtr chainsOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&chainsOut_), (*(void ***)this._nativePointer)[13]);
            chainsOut = (chainsOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.ChainEnum(chainsOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "chainOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugThread::GetActiveChain([In] ICorDebugChain** ppChain)</unmanaged>
        /// <unmanaged-short>ICorDebugThread::GetActiveChain</unmanaged-short>
        internal unsafe void GetActiveChain(out CorApi.Portable.Chain chainOut)
        {
            System.IntPtr chainOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&chainOut_), (*(void ***)this._nativePointer)[14]);
            chainOut = (chainOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Chain(chainOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "frameOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugThread::GetActiveFrame([In] ICorDebugFrame** ppFrame)</unmanaged>
        /// <unmanaged-short>ICorDebugThread::GetActiveFrame</unmanaged-short>
        internal unsafe void GetActiveFrame(out CorApi.Portable.Frame frameOut)
        {
            System.IntPtr frameOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&frameOut_), (*(void ***)this._nativePointer)[15]);
            frameOut = (frameOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Frame(frameOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "registersOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugThread::GetRegisterSet([In] ICorDebugRegisterSet** ppRegisters)</unmanaged>
        /// <unmanaged-short>ICorDebugThread::GetRegisterSet</unmanaged-short>
        internal unsafe void GetRegisterSet(out CorApi.Portable.RegisterSet registersOut)
        {
            System.IntPtr registersOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&registersOut_), (*(void ***)this._nativePointer)[16]);
            registersOut = (registersOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.RegisterSet(registersOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugThread::CreateEval([Out] ICorDebugEval** ppEval)</unmanaged>
        /// <unmanaged-short>ICorDebugThread::CreateEval</unmanaged-short>
        public unsafe CorApi.Portable.Eval CreateEval()
        {
            CorApi.Portable.Eval evalOut;
            System.IntPtr evalOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&evalOut_), (*(void ***)this._nativePointer)[17]);
            evalOut = (evalOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Eval(evalOut_);
            __result__.CheckError();
            return evalOut;
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "objectOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugThread::GetObjectW([In] ICorDebugValue** ppObject)</unmanaged>
        /// <unmanaged-short>ICorDebugThread::GetObjectW</unmanaged-short>
        internal unsafe void GetObjectW(out CorApi.Portable.Value objectOut)
        {
            System.IntPtr objectOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&objectOut_), (*(void ***)this._nativePointer)[18]);
            objectOut = (objectOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Value(objectOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("2BD956D9-7B07-4bef-8A98-12AA862417C5")]
    public partial class Thread2 : SharpGen.Runtime.ComObject
    {
        public Thread2(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Thread2(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Thread2(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cFunctions">No documentation.</param>
        /// <param name = "cFunctionsRef">No documentation.</param>
        /// <param name = "functionsRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugThread2::GetActiveFunctions([In] unsigned int cFunctions,[In] unsigned int* pcFunctions,[In] COR_ACTIVE_FUNCTION* pFunctions)</unmanaged>
        /// <unmanaged-short>ICorDebugThread2::GetActiveFunctions</unmanaged-short>
        public unsafe void GetActiveFunctions(System.UInt32 cFunctions, System.UInt32 cFunctionsRef, ref CorApi.Portable.CorActiveFunction functionsRef)
        {
            var functionsRef_ = new CorApi.Portable.CorActiveFunction.__Native();
            functionsRef.__MarshalTo(ref functionsRef_);
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cFunctions, (void *)(&cFunctionsRef), (void *)(&functionsRef_), (*(void ***)this._nativePointer)[3]);
            functionsRef.__MarshalFree(ref functionsRef_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "dwConnectionIdRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugThread2::GetConnectionID([In] unsigned int* pdwConnectionId)</unmanaged>
        /// <unmanaged-short>ICorDebugThread2::GetConnectionID</unmanaged-short>
        public unsafe void GetConnectionID(System.UInt32 dwConnectionIdRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&dwConnectionIdRef), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "taskIdRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugThread2::GetTaskID([In] unsigned longlong* pTaskId)</unmanaged>
        /// <unmanaged-short>ICorDebugThread2::GetTaskID</unmanaged-short>
        public unsafe void GetTaskID(System.UInt64 taskIdRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&taskIdRef), (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "dwTidRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugThread2::GetVolatileOSThreadID([In] unsigned int* pdwTid)</unmanaged>
        /// <unmanaged-short>ICorDebugThread2::GetVolatileOSThreadID</unmanaged-short>
        public unsafe void GetVolatileOSThreadID(System.UInt32 dwTidRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&dwTidRef), (*(void ***)this._nativePointer)[6]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "frameRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugThread2::InterceptCurrentException([In] ICorDebugFrame* pFrame)</unmanaged>
        /// <unmanaged-short>ICorDebugThread2::InterceptCurrentException</unmanaged-short>
        public unsafe void InterceptCurrentException(CorApi.Portable.Frame frameRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.Frame>(frameRef))), (*(void ***)this._nativePointer)[7]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("F8544EC3-5E4E-46c7-8D3E-A52B8405B1F5")]
    public partial class Thread3 : SharpGen.Runtime.ComObject
    {
        public Thread3(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Thread3(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Thread3(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "stackWalkOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugThread3::CreateStackWalk([In] ICorDebugStackWalk** ppStackWalk)</unmanaged>
        /// <unmanaged-short>ICorDebugThread3::CreateStackWalk</unmanaged-short>
        public unsafe void CreateStackWalk(out CorApi.Portable.StackWalk stackWalkOut)
        {
            System.IntPtr stackWalkOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&stackWalkOut_), (*(void ***)this._nativePointer)[3]);
            stackWalkOut = (stackWalkOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.StackWalk(stackWalkOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cInternalFrames">No documentation.</param>
        /// <param name = "cInternalFramesRef">No documentation.</param>
        /// <param name = "internalFramesOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugThread3::GetActiveInternalFrames([In] unsigned int cInternalFrames,[In] unsigned int* pcInternalFrames,[In] ICorDebugInternalFrame2** ppInternalFrames)</unmanaged>
        /// <unmanaged-short>ICorDebugThread3::GetActiveInternalFrames</unmanaged-short>
        public unsafe void GetActiveInternalFrames(System.UInt32 cInternalFrames, System.UInt32 cInternalFramesRef, out CorApi.Portable.InternalFrame2 internalFramesOut)
        {
            System.IntPtr internalFramesOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cInternalFrames, (void *)(&cInternalFramesRef), (void *)(&internalFramesOut_), (*(void ***)this._nativePointer)[4]);
            internalFramesOut = (internalFramesOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.InternalFrame2(internalFramesOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("1A1F204B-1C66-4637-823F-3EE6C744A69C")]
    public partial class Thread4 : SharpGen.Runtime.ComObject
    {
        public Thread4(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Thread4(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Thread4(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetBlockingObjects</unmanaged>
        /// <unmanaged-short>GetBlockingObjects</unmanaged-short>
        public CorApi.Portable.BlockingObjectEnum BlockingObjects
        {
            get
            {
                GetBlockingObjects(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetCurrentCustomDebuggerNotification</unmanaged>
        /// <unmanaged-short>GetCurrentCustomDebuggerNotification</unmanaged-short>
        public CorApi.Portable.Value CurrentCustomDebuggerNotification
        {
            get
            {
                GetCurrentCustomDebuggerNotification(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugThread4::HasUnhandledException()</unmanaged>
        /// <unmanaged-short>ICorDebugThread4::HasUnhandledException</unmanaged-short>
        public unsafe void HasUnhandledException()
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "blockingObjectEnumOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugThread4::GetBlockingObjects([In] ICorDebugBlockingObjectEnum** ppBlockingObjectEnum)</unmanaged>
        /// <unmanaged-short>ICorDebugThread4::GetBlockingObjects</unmanaged-short>
        internal unsafe void GetBlockingObjects(out CorApi.Portable.BlockingObjectEnum blockingObjectEnumOut)
        {
            System.IntPtr blockingObjectEnumOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&blockingObjectEnumOut_), (*(void ***)this._nativePointer)[4]);
            blockingObjectEnumOut = (blockingObjectEnumOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.BlockingObjectEnum(blockingObjectEnumOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "notificationObjectOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugThread4::GetCurrentCustomDebuggerNotification([In] ICorDebugValue** ppNotificationObject)</unmanaged>
        /// <unmanaged-short>ICorDebugThread4::GetCurrentCustomDebuggerNotification</unmanaged-short>
        internal unsafe void GetCurrentCustomDebuggerNotification(out CorApi.Portable.Value notificationObjectOut)
        {
            System.IntPtr notificationObjectOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&notificationObjectOut_), (*(void ***)this._nativePointer)[5]);
            notificationObjectOut = (notificationObjectOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Value(notificationObjectOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("CC7BCB06-8A68-11d2-983C-0000F808342D")]
    public partial class ThreadEnum : CorApi.Portable.Enum
    {
        public ThreadEnum(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator ThreadEnum(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new ThreadEnum(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "celt">No documentation.</param>
        /// <param name = "threads">No documentation.</param>
        /// <param name = "celtFetchedRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugThreadEnum::Next([In] unsigned int celt,[Out, Buffer] ICorDebugThread** threads,[Out] unsigned int* pceltFetched)</unmanaged>
        /// <unmanaged-short>ICorDebugThreadEnum::Next</unmanaged-short>
        public unsafe void Next(System.UInt32 celt, CorApi.Portable.Thread[] threads, out System.UInt32 celtFetchedRef)
        {
            System.IntPtr*threads_ = stackalloc System.IntPtr[threads.Length];
            SharpGen.Runtime.Result __result__;
            fixed (void *celtFetchedRef_ = &celtFetchedRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, celt, (void *)(threads_), (void *)(celtFetchedRef_), (*(void ***)this._nativePointer)[7]);
            for (int i = 0; i < threads.Length; i++)
                threads[i] = (threads_[i] == System.IntPtr.Zero) ? null : new CorApi.Portable.Thread(threads_[i]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("D613F0BB-ACE1-4c19-BD72-E4C08D5DA7F5")]
    public partial class Type : SharpGen.Runtime.ComObject
    {
        public Type(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Type(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Type(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetClass</unmanaged>
        /// <unmanaged-short>GetClass</unmanaged-short>
        public CorApi.Portable.Class Class
        {
            get
            {
                GetClass(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetFirstTypeParameter</unmanaged>
        /// <unmanaged-short>GetFirstTypeParameter</unmanaged-short>
        public CorApi.Portable.Type FirstTypeParameter
        {
            get
            {
                GetFirstTypeParameter(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetBase</unmanaged>
        /// <unmanaged-short>GetBase</unmanaged-short>
        public CorApi.Portable.Type Base
        {
            get
            {
                GetBase(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetRank</unmanaged>
        /// <unmanaged-short>GetRank</unmanaged-short>
        public System.UInt32 Rank
        {
            get
            {
                GetRank(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "ty">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugType::GetType([Out] CorElementType* ty)</unmanaged>
        /// <unmanaged-short>ICorDebugType::GetType</unmanaged-short>
        public unsafe void DebugType(out CorApi.Portable.CorElementType ty)
        {
            ty = new CorApi.Portable.CorElementType();
            SharpGen.Runtime.Result __result__;
            fixed (void *ty_ = &ty)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(ty_), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "classOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugType::GetClass([In] ICorDebugClass** ppClass)</unmanaged>
        /// <unmanaged-short>ICorDebugType::GetClass</unmanaged-short>
        internal unsafe void GetClass(out CorApi.Portable.Class classOut)
        {
            System.IntPtr classOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&classOut_), (*(void ***)this._nativePointer)[4]);
            classOut = (classOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Class(classOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "tyParEnumOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugType::EnumerateTypeParameters([In] ICorDebugTypeEnum** ppTyParEnum)</unmanaged>
        /// <unmanaged-short>ICorDebugType::EnumerateTypeParameters</unmanaged-short>
        public unsafe void EnumerateTypeParameters(out CorApi.Portable.TypeEnum tyParEnumOut)
        {
            System.IntPtr tyParEnumOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&tyParEnumOut_), (*(void ***)this._nativePointer)[5]);
            tyParEnumOut = (tyParEnumOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.TypeEnum(tyParEnumOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "value">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugType::GetFirstTypeParameter([In] ICorDebugType** value)</unmanaged>
        /// <unmanaged-short>ICorDebugType::GetFirstTypeParameter</unmanaged-short>
        internal unsafe void GetFirstTypeParameter(out CorApi.Portable.Type value)
        {
            System.IntPtr value_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&value_), (*(void ***)this._nativePointer)[6]);
            value = (value_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Type(value_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "baseRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugType::GetBase([Out] ICorDebugType** pBase)</unmanaged>
        /// <unmanaged-short>ICorDebugType::GetBase</unmanaged-short>
        internal unsafe void GetBase(out CorApi.Portable.Type baseRef)
        {
            System.IntPtr baseRef_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&baseRef_), (*(void ***)this._nativePointer)[7]);
            baseRef = (baseRef_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Type(baseRef_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "fieldDef">No documentation.</param>
        /// <param name = "frameRef">No documentation.</param>
        /// <param name = "valueOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugType::GetStaticFieldValue([In] unsigned int fieldDef,[In] ICorDebugFrame* pFrame,[In] ICorDebugValue** ppValue)</unmanaged>
        /// <unmanaged-short>ICorDebugType::GetStaticFieldValue</unmanaged-short>
        public unsafe void GetStaticFieldValue(System.UInt32 fieldDef, CorApi.Portable.Frame frameRef, out CorApi.Portable.Value valueOut)
        {
            System.IntPtr valueOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, fieldDef, (void *)((void *)(SharpGen.Runtime.CppObject.ToCallbackPtr<CorApi.Portable.Frame>(frameRef))), (void *)(&valueOut_), (*(void ***)this._nativePointer)[8]);
            valueOut = (valueOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Value(valueOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "nRankRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugType::GetRank([Out] unsigned int* pnRank)</unmanaged>
        /// <unmanaged-short>ICorDebugType::GetRank</unmanaged-short>
        internal unsafe void GetRank(out System.UInt32 nRankRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *nRankRef_ = &nRankRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(nRankRef_), (*(void ***)this._nativePointer)[9]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("e6e91d79-693d-48bc-b417-8284b4f10fb5")]
    public partial class Type2 : SharpGen.Runtime.ComObject
    {
        public Type2(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Type2(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Type2(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "id">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugType2::GetTypeID([In] COR_TYPEID* id)</unmanaged>
        /// <unmanaged-short>ICorDebugType2::GetTypeID</unmanaged-short>
        public unsafe void GetTypeID(CorApi.Portable.CorTypeid id)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&id), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("10F27499-9DF2-43ce-8333-A321D7C99CB4")]
    public partial class TypeEnum : CorApi.Portable.Enum
    {
        public TypeEnum(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator TypeEnum(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new TypeEnum(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "celt">No documentation.</param>
        /// <param name = "values">No documentation.</param>
        /// <param name = "celtFetchedRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugTypeEnum::Next([In] unsigned int celt,[Out, Buffer] ICorDebugType** values,[Out] unsigned int* pceltFetched)</unmanaged>
        /// <unmanaged-short>ICorDebugTypeEnum::Next</unmanaged-short>
        public unsafe void Next(System.UInt32 celt, CorApi.Portable.Type[] values, out System.UInt32 celtFetchedRef)
        {
            System.IntPtr*values_ = stackalloc System.IntPtr[values.Length];
            SharpGen.Runtime.Result __result__;
            fixed (void *celtFetchedRef_ = &celtFetchedRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, celt, (void *)(values_), (void *)(celtFetchedRef_), (*(void ***)this._nativePointer)[7]);
            for (int i = 0; i < values.Length; i++)
                values[i] = (values_[i] == System.IntPtr.Zero) ? null : new CorApi.Portable.Type(values_[i]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("CC7BCAF7-8A68-11d2-983C-0000F808342D")]
    public partial class Value : SharpGen.Runtime.ComObject
    {
        public Value(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Value(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Value(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetType</unmanaged>
        /// <unmanaged-short>GetType</unmanaged-short>
        public CorApi.Portable.CorElementType Type
        {
            get
            {
                GetType(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetSize</unmanaged>
        /// <unmanaged-short>GetSize</unmanaged-short>
        public System.UInt32 Size
        {
            get
            {
                GetSize(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetAddress</unmanaged>
        /// <unmanaged-short>GetAddress</unmanaged-short>
        public System.UInt64 Address
        {
            get
            {
                GetAddress(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "typeRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugValue::GetType([Out] CorElementType* pType)</unmanaged>
        /// <unmanaged-short>ICorDebugValue::GetType</unmanaged-short>
        internal unsafe void GetType(out CorApi.Portable.CorElementType typeRef)
        {
            typeRef = new CorApi.Portable.CorElementType();
            SharpGen.Runtime.Result __result__;
            fixed (void *typeRef_ = &typeRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(typeRef_), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "sizeRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugValue::GetSize([Out] unsigned int* pSize)</unmanaged>
        /// <unmanaged-short>ICorDebugValue::GetSize</unmanaged-short>
        internal unsafe void GetSize(out System.UInt32 sizeRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *sizeRef_ = &sizeRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(sizeRef_), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "addressRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugValue::GetAddress([Out] unsigned longlong* pAddress)</unmanaged>
        /// <unmanaged-short>ICorDebugValue::GetAddress</unmanaged-short>
        internal unsafe void GetAddress(out System.UInt64 addressRef)
        {
            SharpGen.Runtime.Result __result__;
            fixed (void *addressRef_ = &addressRef)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(addressRef_), (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "breakpointOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugValue::CreateBreakpoint([In] ICorDebugValueBreakpoint** ppBreakpoint)</unmanaged>
        /// <unmanaged-short>ICorDebugValue::CreateBreakpoint</unmanaged-short>
        public unsafe void CreateBreakpoint(out CorApi.Portable.ValueBreakpoint breakpointOut)
        {
            System.IntPtr breakpointOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&breakpointOut_), (*(void ***)this._nativePointer)[6]);
            breakpointOut = (breakpointOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.ValueBreakpoint(breakpointOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("5E0B54E7-D88A-4626-9420-A691E0A78B49")]
    public partial class Value2 : SharpGen.Runtime.ComObject
    {
        public Value2(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Value2(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Value2(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetExactType</unmanaged>
        /// <unmanaged-short>GetExactType</unmanaged-short>
        public CorApi.Portable.Type ExactType
        {
            get
            {
                GetExactType(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "typeOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugValue2::GetExactType([In] ICorDebugType** ppType)</unmanaged>
        /// <unmanaged-short>ICorDebugValue2::GetExactType</unmanaged-short>
        internal unsafe void GetExactType(out CorApi.Portable.Type typeOut)
        {
            System.IntPtr typeOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&typeOut_), (*(void ***)this._nativePointer)[3]);
            typeOut = (typeOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Type(typeOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("565005FC-0F8A-4F3E-9EDB-83102B156595")]
    public partial class Value3 : SharpGen.Runtime.ComObject
    {
        public Value3(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator Value3(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new Value3(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "sizeRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugValue3::GetSize64([In] unsigned longlong* pSize)</unmanaged>
        /// <unmanaged-short>ICorDebugValue3::GetSize64</unmanaged-short>
        public unsafe void GetSize64(System.UInt64 sizeRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&sizeRef), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("CC7BCAEB-8A68-11d2-983C-0000F808342D")]
    public partial class ValueBreakpoint : CorApi.Portable.Breakpoint
    {
        public ValueBreakpoint(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator ValueBreakpoint(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new ValueBreakpoint(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetValue</unmanaged>
        /// <unmanaged-short>GetValue</unmanaged-short>
        public CorApi.Portable.Value Value
        {
            get
            {
                GetValue(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "valueOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugValueBreakpoint::GetValue([In] ICorDebugValue** ppValue)</unmanaged>
        /// <unmanaged-short>ICorDebugValueBreakpoint::GetValue</unmanaged-short>
        internal unsafe void GetValue(out CorApi.Portable.Value valueOut)
        {
            System.IntPtr valueOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&valueOut_), (*(void ***)this._nativePointer)[5]);
            valueOut = (valueOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Value(valueOut_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("CC7BCB0A-8A68-11d2-983C-0000F808342D")]
    public partial class ValueEnum : CorApi.Portable.Enum
    {
        public ValueEnum(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator ValueEnum(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new ValueEnum(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "celt">No documentation.</param>
        /// <param name = "values">No documentation.</param>
        /// <param name = "celtFetchedRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugValueEnum::Next([In] unsigned int celt,[In] ICorDebugValue** values,[In] unsigned int* pceltFetched)</unmanaged>
        /// <unmanaged-short>ICorDebugValueEnum::Next</unmanaged-short>
        public unsafe void Next(System.UInt32 celt, out CorApi.Portable.Value values, System.UInt32 celtFetchedRef)
        {
            System.IntPtr values_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, celt, (void *)(&values_), (void *)(&celtFetchedRef), (*(void ***)this._nativePointer)[7]);
            values = (values_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Value(values_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("50847b8d-f43f-41b0-924c-6383a5f2278b")]
    public partial class VariableHome : SharpGen.Runtime.ComObject
    {
        public VariableHome(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator VariableHome(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new VariableHome(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <unmanaged>GetCode</unmanaged>
        /// <unmanaged-short>GetCode</unmanaged-short>
        public CorApi.Portable.Code Code
        {
            get
            {
                GetCode(out var __output__);
                return __output__;
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "codeOut">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugVariableHome::GetCode([In] ICorDebugCode** ppCode)</unmanaged>
        /// <unmanaged-short>ICorDebugVariableHome::GetCode</unmanaged-short>
        internal unsafe void GetCode(out CorApi.Portable.Code codeOut)
        {
            System.IntPtr codeOut_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&codeOut_), (*(void ***)this._nativePointer)[3]);
            codeOut = (codeOut_ == System.IntPtr.Zero) ? null : new CorApi.Portable.Code(codeOut_);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "slotIndexRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugVariableHome::GetSlotIndex([In] unsigned int* pSlotIndex)</unmanaged>
        /// <unmanaged-short>ICorDebugVariableHome::GetSlotIndex</unmanaged-short>
        public unsafe void GetSlotIndex(System.UInt32 slotIndexRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&slotIndexRef), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "argumentIndexRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugVariableHome::GetArgumentIndex([In] unsigned int* pArgumentIndex)</unmanaged>
        /// <unmanaged-short>ICorDebugVariableHome::GetArgumentIndex</unmanaged-short>
        public unsafe void GetArgumentIndex(System.UInt32 argumentIndexRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&argumentIndexRef), (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "startOffsetRef">No documentation.</param>
        /// <param name = "endOffsetRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugVariableHome::GetLiveRange([In] unsigned int* pStartOffset,[In] unsigned int* pEndOffset)</unmanaged>
        /// <unmanaged-short>ICorDebugVariableHome::GetLiveRange</unmanaged-short>
        public unsafe void GetLiveRange(System.UInt32 startOffsetRef, System.UInt32 endOffsetRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&startOffsetRef), (void *)(&endOffsetRef), (*(void ***)this._nativePointer)[6]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "locationTypeRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugVariableHome::GetLocationType([In] VariableLocationType* pLocationType)</unmanaged>
        /// <unmanaged-short>ICorDebugVariableHome::GetLocationType</unmanaged-short>
        public unsafe void GetLocationType(CorApi.Portable.VariableLocationType locationTypeRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&locationTypeRef), (*(void ***)this._nativePointer)[7]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "registerRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugVariableHome::GetRegister([In] CorDebugRegister* pRegister)</unmanaged>
        /// <unmanaged-short>ICorDebugVariableHome::GetRegister</unmanaged-short>
        public unsafe void GetRegister(CorApi.Portable.CorDebugRegister registerRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&registerRef), (*(void ***)this._nativePointer)[8]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "offsetRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugVariableHome::GetOffset([In] int* pOffset)</unmanaged>
        /// <unmanaged-short>ICorDebugVariableHome::GetOffset</unmanaged-short>
        public unsafe void GetOffset(System.Int32 offsetRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&offsetRef), (*(void ***)this._nativePointer)[9]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("e76b7a57-4f7a-4309-85a7-5d918c3deaf7")]
    public partial class VariableHomeEnum : CorApi.Portable.Enum
    {
        public VariableHomeEnum(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator VariableHomeEnum(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new VariableHomeEnum(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "celt">No documentation.</param>
        /// <param name = "homes">No documentation.</param>
        /// <param name = "celtFetchedRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugVariableHomeEnum::Next([In] unsigned int celt,[In] ICorDebugVariableHome** homes,[In] unsigned int* pceltFetched)</unmanaged>
        /// <unmanaged-short>ICorDebugVariableHomeEnum::Next</unmanaged-short>
        public unsafe void Next(System.UInt32 celt, out CorApi.Portable.VariableHome homes, System.UInt32 celtFetchedRef)
        {
            System.IntPtr homes_ = System.IntPtr.Zero;
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, celt, (void *)(&homes_), (void *)(&celtFetchedRef), (*(void ***)this._nativePointer)[7]);
            homes = (homes_ == System.IntPtr.Zero) ? null : new CorApi.Portable.VariableHome(homes_);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("707E8932-1163-48D9-8A93-F5B1F480FBB7")]
    public partial class VariableSymbol : SharpGen.Runtime.ComObject
    {
        public VariableSymbol(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator VariableSymbol(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new VariableSymbol(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cchName">No documentation.</param>
        /// <param name = "cchNameRef">No documentation.</param>
        /// <param name = "szName">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugVariableSymbol::GetName([In] unsigned int cchName,[In] unsigned int* pcchName,[In] wchar_t* szName)</unmanaged>
        /// <unmanaged-short>ICorDebugVariableSymbol::GetName</unmanaged-short>
        public unsafe void GetName(System.UInt32 cchName, System.UInt32 cchNameRef, System.String szName)
        {
            SharpGen.Runtime.Result __result__;
            fixed (char *szName_ = szName)
                __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, cchName, (void *)(&cchNameRef), (void *)((void *)szName_), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "cbValueRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugVariableSymbol::GetSize([In] unsigned int* pcbValue)</unmanaged>
        /// <unmanaged-short>ICorDebugVariableSymbol::GetSize</unmanaged-short>
        public unsafe void GetSize(System.UInt32 cbValueRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&cbValueRef), (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "offset">No documentation.</param>
        /// <param name = "cbContext">No documentation.</param>
        /// <param name = "context">No documentation.</param>
        /// <param name = "cbValue">No documentation.</param>
        /// <param name = "cbValueRef">No documentation.</param>
        /// <param name = "valueRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugVariableSymbol::GetValue([In] unsigned int offset,[In] unsigned int cbContext,[In] unsigned char* context,[In] unsigned int cbValue,[In] unsigned int* pcbValue,[In] unsigned char* pValue)</unmanaged>
        /// <unmanaged-short>ICorDebugVariableSymbol::GetValue</unmanaged-short>
        public unsafe void GetValue(System.UInt32 offset, System.UInt32 cbContext, System.Byte context, System.UInt32 cbValue, System.UInt32 cbValueRef, System.Byte valueRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, offset, cbContext, (void *)(&context), cbValue, (void *)(&cbValueRef), (void *)(&valueRef), (*(void ***)this._nativePointer)[5]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "offset">No documentation.</param>
        /// <param name = "threadID">No documentation.</param>
        /// <param name = "cbContext">No documentation.</param>
        /// <param name = "context">No documentation.</param>
        /// <param name = "cbValue">No documentation.</param>
        /// <param name = "valueRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugVariableSymbol::SetValue([In] unsigned int offset,[In] unsigned int threadID,[In] unsigned int cbContext,[In] unsigned char* context,[In] unsigned int cbValue,[In] unsigned char* pValue)</unmanaged>
        /// <unmanaged-short>ICorDebugVariableSymbol::SetValue</unmanaged-short>
        public unsafe void SetValue(System.UInt32 offset, System.UInt32 threadID, System.UInt32 cbContext, System.Byte context, System.UInt32 cbValue, System.Byte valueRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, offset, threadID, cbContext, (void *)(&context), cbValue, (void *)(&valueRef), (*(void ***)this._nativePointer)[6]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "slotIndexRef">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugVariableSymbol::GetSlotIndex([In] unsigned int* pSlotIndex)</unmanaged>
        /// <unmanaged-short>ICorDebugVariableSymbol::GetSlotIndex</unmanaged-short>
        public unsafe void GetSlotIndex(System.UInt32 slotIndexRef)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (void *)(&slotIndexRef), (*(void ***)this._nativePointer)[7]);
            __result__.CheckError();
        }
    }

    [System.Runtime.InteropServices.GuidAttribute("F69126B7-C787-4F6B-AE96-A569786FC670")]
    public partial class VirtualUnwinder : SharpGen.Runtime.ComObject
    {
        public VirtualUnwinder(System.IntPtr nativePtr): base (nativePtr)
        {
        }

        public static explicit operator VirtualUnwinder(System.IntPtr nativePtr) => nativePtr == System.IntPtr.Zero ? null : new VirtualUnwinder(nativePtr);
        /// <summary>
        /// No documentation.
        /// </summary>
        /// <param name = "contextFlags">No documentation.</param>
        /// <param name = "cbContextBuf">No documentation.</param>
        /// <param name = "contextSize">No documentation.</param>
        /// <param name = "contextBuf">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugVirtualUnwinder::GetContext([In] unsigned int contextFlags,[In] unsigned int cbContextBuf,[In] unsigned int* contextSize,[In] unsigned char* contextBuf)</unmanaged>
        /// <unmanaged-short>ICorDebugVirtualUnwinder::GetContext</unmanaged-short>
        public unsafe void GetContext(System.UInt32 contextFlags, System.UInt32 cbContextBuf, System.UInt32 contextSize, System.Byte contextBuf)
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, contextFlags, cbContextBuf, (void *)(&contextSize), (void *)(&contextBuf), (*(void ***)this._nativePointer)[3]);
            __result__.CheckError();
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ICorDebugVirtualUnwinder::Next()</unmanaged>
        /// <unmanaged-short>ICorDebugVirtualUnwinder::Next</unmanaged-short>
        public unsafe void Next()
        {
            SharpGen.Runtime.Result __result__;
            __result__ = CorDebug.LocalInterop.Calliint(this._nativePointer, (*(void ***)this._nativePointer)[4]);
            __result__.CheckError();
        }
    }
}