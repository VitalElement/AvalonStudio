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

extern unsigned long _data_flash;
extern unsigned long _data_begin;
extern unsigned long _data_end;
extern unsigned long _bss_begin;
extern unsigned long _bss_end;

#pragma mark Static Data


#pragma mark Static Functions


#pragma mark Function Implementations
void system_startup (void)
{
    volatile unsigned long* source;
    volatile unsigned long* destination;

    // Zero bss segment.
    for (destination = &_bss_begin; destination < &_bss_end;)
    {
        *(destination++) = 0;
    }

    source = &_data_flash;

    for (destination = &_data_begin; destination < &_data_end;)
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
