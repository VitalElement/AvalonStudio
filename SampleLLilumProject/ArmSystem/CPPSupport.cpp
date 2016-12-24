/******************************************************************************
*       Description:
*
*       Author:
*         Date: January 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#include <cstdlib>
#include <sys/types.h>


/*
 * The default pulls in 70K of garbage
 */

namespace __gnu_cxx
{

    void __verbose_terminate_handler ()
    {
        for (;;)
        {
        }
    }
}


/*
 * The default pulls in about 12K of garbage
 */

extern "C" void __cxa_pure_virtual ()
{
    for (;;)
    {
    }
}


/*
 * Implement C++ new/delete operators using the heap
 */

#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Wimplicit-exception-spec-mismatch"
#pragma clang diagnostic pop