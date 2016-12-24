/******************************************************************************
*       Description:
*
*       Author:
*         Date: 13 June 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _STATEENGINE_H_
#define _STATEENGINE_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>
#include "Dispatcher.h"
#include "Event.h"

using namespace VitalElement::Threading;

namespace VitalElement
{
    namespace Control
    {
        /**
         * A class to manage routing of events from 1 context to another. 
         */
        class EventVector : public List
        {
            friend class StateEngine;

          public:
            EventVector (Event<EventArgs>& event, EventHandler handler);

            Event<EventArgs>& event;
            EventHandler handler;       //!< The event handler to be called following interception.

          private:            
            EventHandle interceptor;  //!< Event subscriber that listens for the event and used to raise the event on the state machine thread.
        };

        /**
         * State - base class all states must inherit from.
         */
        class State
        {
            friend class StateEngine;

          public:
            State ();

            virtual void OnEnter (void) = 0;
            virtual void OnExit (void) = 0;

          protected:
            List eventVectors; //!< list of event vectors that describe how events are routed.
            uint32_t Size;
        };

        /**
         * StateEngine - Handles state machine event routing and state
         * management.
         */
        class StateEngine
        {
          public:
            /**
             * Constructs a StateEngine object.
             * @param eventDispatcher - a dispatcher object belonging to the
             * thread that all events will be routed through.
             */
            StateEngine (Dispatcher& eventDispatcher);

            /**
             * Sets a new state.
             * Calling this method will cleanly exit the current state, void any
             * pending events and set a new state.
             * @param state - the state to change to.
             */
            void SetState (State& state);

            /**
             * Sets a new state.
             * Calling this method will cleanly exit the current state, void any
             * pending events and set a new state.
             * @param state - pointer to the state to change to.
             */
            void SetState (State* state);

            /**
             * Gets a pointer to the current state.
             * This method can be used to compare a state against the current
             * state.
             * @return a pointer to the current state.
             */
            State* GetCurrentState ();

          private:
            /**
             * Subscribes the events in the state to an interceptor, which
             * dispatches the events to the handler. This mechanism ensures
             * thread safety.
             */
            void EnterState (State& state);


            /**
             * Unsubscribes all event interceptors, the state is taken offline.
             */
            void ExitState ();
            Dispatcher& eventDispatcher;
            State* currentState;
        };
    }
}


#endif
