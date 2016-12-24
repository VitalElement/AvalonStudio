/******************************************************************************
*       Description:
*
*       Author:
*         Date: 13 June 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#pragma mark Includes
#include "StateEngine.h"

using namespace VitalElement::Control;

#pragma mark Definitions and Constants


#pragma mark Static Data


#pragma mark Static Functions


#pragma mark Member Implementations
StateEngine::StateEngine (Dispatcher& eventDispatcher)
    : eventDispatcher (eventDispatcher)
{
    currentState = nullptr;
}

void StateEngine::SetState (State* state)
{
    SetState (*state);
}

void StateEngine::SetState (State& state)
{
    eventDispatcher.BeginInvoke ([&]
                                 {
                                     if (currentState != nullptr)
                                     {
                                         ExitState ();
                                     }

                                     currentState = &state;

                                     if (currentState != nullptr)
                                     {
                                         EnterState (state);
                                     }
                                 });
}

State* StateEngine::GetCurrentState ()
{
    return currentState;
}


void StateEngine::EnterState (State& state)
{    
    for (auto pos = state.eventVectors.next; pos != &state.eventVectors;
         pos = pos->next)
    {
        auto vector = (EventVector*)pos;

        // Subscribes to event.
        vector->interceptor = vector->event +=
        [=](void* sender, EventArgs& args)
        {
            // Events can be raised on any thread... State must be made thread
            // safe...
            // Dispatch events to main context.
            eventDispatcher.BeginInvoke (
            [=]
            {
                if (vector->interceptor != nullptr) // This would indicate state
                                                    // has changed and the event
                                                    // will be lost.
                {
                    vector->handler ((void*)sender, (EventArgs&)args);
                }
            });
        };
    }

    state.OnEnter ();
}


void StateEngine::ExitState ()
{
    for (auto pos = currentState->eventVectors.next;
         pos != &currentState->eventVectors; pos = pos->next)
    {
        auto vector = (EventVector*)pos;

        vector->event -= vector->interceptor;
        vector->interceptor = nullptr;
    }

    currentState->OnExit ();
}

EventVector::EventVector (Event<EventArgs>& event, EventHandler handler)
    : event (event), handler (handler)
{
}

State::State ()
{
}
