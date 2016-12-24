//
// Copyright (c) Microsoft Corporation.    All rights reserved.
//

#include "mbed_helpers.h" 
#include "llos_memory.h"

//--//

extern "C"
{
    HRESULT LLOS_MEMORY_GetMaxHeapSize(uint32_t* pMaxHeapSize)
    {
        if (pMaxHeapSize == NULL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        *pMaxHeapSize = 0x4000;

#if defined(__HEAP_SIZE) 
#if __HEAP_SIZE > 0
        *pMaxHeapSize = __HEAP_SIZE;
#endif
#endif

        return S_OK;
    }


    uint32_t LLOS_MEMORY_GetDefaultManagedStackSize()
    {
#if defined(__DEFAULT_STACK_SIZE) && __DEFAULT_STACK_SIZE > 0
        return __DEFAULT_STACK_SIZE;
#else
        // The following return value (0) will result in the default stack size defined in managed code being used
        return 0;
#endif
    }

    HRESULT LLOS_MEMORY_Allocate(uint32_t size, uint8_t fill, LLOS_Opaque* pAllocation)
    {
        if (pAllocation == NULL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        if (fill == 0)
        {
            *pAllocation = LLOS_CALLOC(size, 1);
        }
        else
        {
            *pAllocation = LLOS_MALLOC(size);

            if (*pAllocation != NULL)
            {
                LLOS_MEMSET(*pAllocation, fill, size);
            }
        }

        if (*pAllocation == NULL)
        {
            return LLOS_E_OUT_OF_MEMORY;
        }

        return S_OK;
    }

    HRESULT LLOS_MEMORY_Reallocate(LLOS_Opaque* pAllocation, uint32_t newSize)
    {
        LLOS_REALLOC(pAllocation, newSize);
        
        if (pAllocation == NULL && newSize > 0)
        {
            return LLOS_E_OUT_OF_MEMORY;
        }

        return S_OK;
    }

    VOID LLOS_MEMORY_Free(LLOS_Opaque address)
    {
        LLOS_FREE(address);
    }
}
