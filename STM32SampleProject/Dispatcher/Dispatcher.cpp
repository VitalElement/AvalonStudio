#include "Dispatcher.h"

using namespace VitalElement::Threading;

Dispatcher::Dispatcher (DispatcherActions& dispatcherActions) : actions (dispatcherActions)
{
    for (uint32_t i = 0; i < QueueSize; i++)
    {
        available.Push (&jobs[i]);
    }
}


bool Dispatcher::RunSingle ()
{
    bool result = false;

    actions.EnterCriticalSection ();
    auto pos = active.Pop ();
    actions.ExitCriticalSection ();

    if (pos != nullptr)
    {
        JobList* job = (JobList*)pos;
        job->action ();

        actions.EnterCriticalSection ();
        available.Push (pos);
        actions.ExitCriticalSection ();

        result = true;
    }

    return result;
}

bool Dispatcher::BeginInvoke (Action action)
{
    bool result = true;

    actions.EnterCriticalSection ();

    auto j = (JobList*)available.Pop ();

    if (!j)
    {
        result = false;
    }
    else
    {
        j->action = action;
        active.Push ((List*)j);
    }

    actions.ExitCriticalSection ();

    return result;
}
