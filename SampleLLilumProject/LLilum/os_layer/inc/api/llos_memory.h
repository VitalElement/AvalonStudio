//
//    LLILUM OS Abstraction Layer - Heap
// 

#define LLOS_USE_MANAGED_HEAP 1

#ifndef __LLOS_MEMORY_H__
#define __LLOS_MEMORY_H__

#ifdef __cplusplus
extern "C" {
#endif

#include "llos_types.h"

// 
//  General heap
//
#if LLOS_USE_MANAGED_HEAP
extern LLOS_Opaque AllocateFromManagedHeap(uint32_t size);
extern VOID        FreeFromManagedHeap    (LLOS_Opaque address);
#else
inline LLOS_Opaque AllocateFromManagedHeap(uint32_t size) { return LLOS_CALLOC(size, 1); }
inline VOID        FreeFromManagedHeap(LLOS_Opaque address) { LLOS_FREE(address); }
#endif

HRESULT LLOS_MEMORY_GetMaxHeapSize(uint32_t* pMaxHeapSize);
HRESULT LLOS_MEMORY_Allocate      (uint32_t size, uint8_t fill, LLOS_Opaque* pAllocation);
HRESULT LLOS_MEMORY_Reallocate    (LLOS_Opaque* allocation, uint32_t newSize);
VOID    LLOS_MEMORY_Free          (LLOS_Opaque address);

#ifdef __cplusplus
}
#endif

#endif // __LLOS_MEMORY_H__
