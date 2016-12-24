/******************************************************************************
*       Description:
*
*       Author:
*         Date: 28 July 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#pragma mark Includes
#include "ThreadDispatcher.h"

#pragma mark Definitions and Constants
using namespace VitalElement::Threading;

#pragma mark Static Data


#pragma mark Static Functions


#pragma mark Member Implementations
ThreadDispatcher::ThreadDispatcher (DispatcherActions& dispatcherActions)
    : Dispatcher (dispatcherActions)
{
    ownedThread = Thread::GetCurrentThread ();
}

ThreadDispatcher::~ThreadDispatcher ()
{
}

void ThreadDispatcher::Run ()
{
    while (true)
    {
        if (!RunSingle ())
        {
            Thread::Yield ();
        }
    }
}

bool ThreadDispatcher::BeginInvoke (Action action)
{
    bool result = Dispatcher::BeginInvoke (action);

    if (result)
    {
        //ownedThread->Resume ();
    }

    return result;
}

void ThreadDispatcher::SetOwner (Thread* owner)
{
    Thread::BeginCriticalRegion ();
    ownedThread = owner;
    Thread::EndCriticalRegion ();
}
