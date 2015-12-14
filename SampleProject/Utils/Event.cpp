/******************************************************************************
*       Description:
*
*       Author:
*         Date: 19 November 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#pragma mark Includes
#include "Event.h"

#pragma mark Definitions and Constants


#pragma mark Static Data


#pragma mark Static Functions


#pragma mark Member Implementations
Subscriber GlobalEventHandlers::allocatedHandlers[MaxGlobalSubscribers];

void GlobalEventHandlers::Initialise ()
{
    for (uint32_t i = 0; i < MaxGlobalSubscribers - 1; i++)
    {
        GlobalEventHandlers::Available.Push (&allocatedHandlers[i]);
    }
}

List GlobalEventHandlers::Available = List ();
