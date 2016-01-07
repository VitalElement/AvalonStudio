/******************************************************************************
*       Description:
*
*       Author:
*         Date: January 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#pragma mark Includes
#include "startup.h"


#pragma mark Definitions and Constants
typedef void (*func_ptr) (void);

extern func_ptr __init_array_start[0], __init_array_end[0];
extern func_ptr __fini_array_start[0], __fini_array_end[0];

extern unsigned long __exidx_start;
extern unsigned long __data_start__;
extern unsigned long __data_end__;
extern unsigned long __bss_start__;
extern unsigned long __bss_end__;

#pragma mark Static Data


#pragma mark Static Functions


#pragma mark Function Implementations
void system_startup (void)
{
    volatile unsigned long* source;
    volatile unsigned long* destination;

    // Zero bss segment.
    for (destination = &__bss_start__; destination < &__bss_end__;)
    {
        *(destination++) = 0;
    }

    source = &__exidx_start;

    for (destination = &__data_start__; destination < &__data_end__;)
    {
        *(destination++) = *(source++);
    } 
}

void system_init (void)
{
    // Call C++ static constructors.
    func_ptr* func;

    for (func = __init_array_start; func != __init_array_end; func++)
    {
        (*func) ();
    }
}

void system_cleanup (void)
{
    func_ptr* func;

    for (func = __fini_array_start; func != __fini_array_end; func++)
    {
        (*func) ();
    }
}
