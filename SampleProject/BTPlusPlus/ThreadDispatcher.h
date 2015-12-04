/******************************************************************************
*       Description:
*
*       Author:
*         Date: 28 July 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _THREADDISPATCHER_H_
#define _THREADDISPATCHER_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>
#include "Dispatcher.h"
#include "Thread.h"

namespace VitalElement
{
namespace Threading
{
/**
 * A version of the dispatcher that is tied to Thread. Can suspend and resume
 * its owning thread
 * when the queue is empty.
 */
class ThreadDispatcher : public Dispatcher
{
#pragma mark Public Members
  public:
    ThreadDispatcher (DispatcherActions& dispatcherActions);
    ~ThreadDispatcher ();

    void SetOwner (Thread* owner);

    /**
     * Runs forever, suspending the current thread when there is no work todo.
     */
    void Run ();

    /**
     * Queues work to be invoked asynchronously on the dispatcher.
     * Work is queued and this method immediately returns.
     * @param action the action to run asynchronously.
     * @return true indicates success, false indicates failure, i.e.
     * processing overload (queue full).
     */
            virtual bool BeginInvoke (Action action);
            
#pragma mark Private Members
          private:
            Thread* ownedThread;
        };
    }
}

#endif
