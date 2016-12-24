//
// Copyright (c) Microsoft Corporation.    All rights reserved.
//

#include "mbed_helpers.h"

// Enable this macro to shift object pointers from the beginning of the header to the beginning of the payload.
#define CANONICAL_OBJECT_POINTERS

struct Object;

struct ObjectHeader
{
    int32_t MultiUseWord;
    void* VTable;

    inline Object* get_Object()
    {
#ifdef CANONICAL_OBJECT_POINTERS
        return reinterpret_cast<Object*>(this + 1);
#else // CANONICAL_OBJECT_POINTERS
        return reinterpret_cast<Object*>(this);
#endif // CANONICAL_OBJECT_POINTERS
    }
};

struct Object
{
    inline ObjectHeader* get_Header()
    {
#ifdef CANONICAL_OBJECT_POINTERS
        return reinterpret_cast<ObjectHeader*>(this) - 1;
#else // CANONICAL_OBJECT_POINTERS
        return reinterpret_cast<ObjectHeader*>(this);
#endif // CANONICAL_OBJECT_POINTERS
    }
};

extern "C"
{

    // Must match consts defined in ObjectHeader.cs
    #define REFERENCE_COUNT_MASK  0xFF000000
    #define REFERENCE_COUNT_SHIFT 24

    // Helpers for starting / ending section of code that needs to be atomic
    __attribute__((always_inline)) __STATIC_INLINE void StartAtomicOperations(void)
    {
        __set_PRIMASK(1);
    }

    __attribute__((always_inline)) __STATIC_INLINE void EndAtomicOperations(void)
    {
        __set_PRIMASK(0);
    }

    __attribute__((always_inline)) __STATIC_INLINE void AddReferenceFast(Object* target)
    {
        if (target != NULL)
        {
            ObjectHeader* header = target->get_Header();
            if (header->MultiUseWord & REFERENCE_COUNT_MASK)
            {
                header->MultiUseWord += (1 << REFERENCE_COUNT_SHIFT);
            }
        }
    }

    void AddReference(Object* target)
    {
        if (target != NULL)
        {
            ObjectHeader* header = target->get_Header();

            StartAtomicOperations();

            if (header->MultiUseWord & REFERENCE_COUNT_MASK)
            {
                header->MultiUseWord += (1 << REFERENCE_COUNT_SHIFT);
            }

            EndAtomicOperations();
        }
    }

    // Return zero when target is dead after the call
    int ReleaseReferenceNative(Object* target)
    {
        int ret = 1;
        if (target != NULL)
        {
            ObjectHeader* header = target->get_Header();

            StartAtomicOperations();

            int32_t value = header->MultiUseWord;
            if (value & REFERENCE_COUNT_MASK)
            {
                value -= (1 << REFERENCE_COUNT_SHIFT);
                header->MultiUseWord = value;
                ret = value & REFERENCE_COUNT_MASK;
            }

            EndAtomicOperations();
        }

        return ret;
    }

    Object* LoadAndAddReferenceNative(Object** target)
    {
        StartAtomicOperations();

        Object* value = *target;
        AddReferenceFast(value);

        EndAtomicOperations();

        return value;
    }

    Object* ReferenceCountingExchange(Object** target, Object* value)
    {
        StartAtomicOperations();

        Object* oldValue = *target;
        *target = value;
        AddReferenceFast(value);

        EndAtomicOperations();

        return oldValue;
    }

    Object* ReferenceCountingCompareExchange(Object** target, Object* value, Object* comparand)
    {
        StartAtomicOperations();

        Object* oldValue = *target;
        Object* addRefTarget;
        if (oldValue == comparand)
        {
            *target = value;

            // Compare exchange succeeded, we need to add ref the new value
            // The old value's ref count will be passed back to the caller on return.
            addRefTarget = value;
        }
        else
        {
            // Target is not changed, we need to add ref the old value so it has
            // a ref count to pass back to caller on return
            addRefTarget = oldValue;
        }

        AddReferenceFast(addRefTarget);

        EndAtomicOperations();

        return oldValue;
    }
}
