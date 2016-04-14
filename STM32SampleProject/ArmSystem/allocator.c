/******************************************************************************
*       Description:
*
*       Author:
*         Date: January 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#pragma mark Includes
#include <stddef.h>

#pragma mark Definitions and Constants
extern unsigned int _HEAP_START;
extern unsigned int _HEAP_END;

typedef char* caddr_t;

#pragma mark Static Data
static caddr_t heap = NULL;

#pragma mark Static Functions


#pragma mark Function Implementations
caddr_t _sbrk (int increment)
{
    caddr_t prevHeap;
    caddr_t nextHeap;

    if (heap == NULL)
    {
        heap = (caddr_t)&_HEAP_START;
    }

    prevHeap = heap;

    nextHeap = (caddr_t)(((unsigned int)(heap + increment) + 7) & ~7);

    register caddr_t stackPtr asm("sp") = NULL;

    if ((((caddr_t)&_HEAP_START < stackPtr) && (nextHeap > stackPtr)) ||
        (nextHeap > (caddr_t)&_HEAP_END))
    {
        return NULL; // error - no more memory
    }
    else
    {
        heap = nextHeap;
        return (caddr_t)prevHeap;
    }
}