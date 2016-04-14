/******************************************************************************
*       Description:
*
*       Author:
*         Date: 28 July 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#pragma mark Includes
#include "Kernel.h"
#include "Thread.h"
#include "Event.h"

#pragma mark Definitions and Constants
extern "C" void system_init (void);
extern int main (void);
#pragma mark Static Data


#pragma mark Static Functions


#pragma mark Member Implementations
extern "C" void Startup ()
{
    static Thread mainThread = Thread (
    [&]
    {
        main ();
    },
    2000);

    GlobalEventHandlers::Initialise ();
    Kernel::Start ();
}
