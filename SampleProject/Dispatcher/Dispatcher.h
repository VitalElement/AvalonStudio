


#ifndef _DISPATCHER_H_
#define _DISPATCHER_H_

#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>

#include "Action.h"
#include "SimpleList.h"
#include "Event.h"

const uint8_t QueueSize = 255U;

namespace VitalElement
{
    namespace Threading
    {
        /**
         * Dispatcher Actions, holds platform dependent actions required for
         * correct operation
         * of dispatcher.
         */
        struct DispatcherActions
        {
            Action EnterCriticalSection; /**< Disables all interrupts and
                                            context switching that may endanger
                                            the dispatchers queue. */

            Action ExitCriticalSection; /**< Re-enables all interrupts and
                                           context switching. */
        };

        /**
         * Dispatcher, allows the transfer of work from one thread or context to
         * another.
         */
        class Dispatcher
        {
            /**
             * JobList, a list of jobs or tasks to be performed by the
             * dispatcher.
             */
            class JobList : public List
            {
              public:
                Action action; /**< Action to be performed to complete the job. */
            };

          public:
            /**
             * Constructs a Dispatcher instance.
             * @param dispatcherActions - actions required for operation of the
             * dispatcher.
             * The actions will need to disable context switching and
             * interrupts.
             */
            Dispatcher (DispatcherActions& dispatcherActions);            

            /**
             * Runs a single job on the dispatchers queue. This method would
             * normally be called in a loop.
             * @return true indicates a job was run, false indicates the queue was empty.
             */
            bool RunSingle ();

            /**
             * Queues work to be invoked asynchronously on the dispatcher.
             * Work is queued and this method immediately returns.
             * @param action the action to run asynchronously.
             * @return true indicates success, false indicates failure, i.e.
             * processing overload (queue full).
             */
            virtual bool BeginInvoke (Action action);

          private:
            JobList jobs[QueueSize];    /**< Reserved memory to hold all jobs on the dispatcher. */
            List available;             /**< List of available jobs. */
            List active;                /**< List of active jobs. */
            DispatcherActions& actions; /**< Reference to the dispatcher Actions. */
        };
    }
}


#endif
