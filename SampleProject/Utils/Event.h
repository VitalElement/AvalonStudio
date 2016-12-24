#ifndef _EVENT_H_
#define _EVENT_H_

#include <vector>
#include "function.h"
#include "SimpleList.h"


class EventArgs
{
  public:
    uint32_t id;
};

typedef void* EventHandle;

class Subscriber;

typedef func::function<void(void*, EventArgs&)> EventHandler;

class Subscriber : public List
{
  public:
    EventHandler fn;
};

class GlobalEventHandlers
{
    static const uint32_t MaxGlobalSubscribers = 200;
    static Subscriber allocatedHandlers[MaxGlobalSubscribers];

  public:
    static void Initialise ();    

    static List Available;
};

template <typename T> class Event
{
  public:
    Event ()
    {
    }

    void operator() (void* sender, T& e)
    {
        for (auto pos = active.next; pos != &active;)
        {
            // This allows the event to be unsubscribed within its own handler.
            auto next = pos->next;

            Subscriber* s = (Subscriber*)pos;

            (s->fn) (sender, e);

            pos = next;
        }
    }

    EventHandle operator+=(EventHandler f)
    {        
        auto s = (Subscriber*)GlobalEventHandlers::Available.Pop ();

        if (!s)
        {
            while (true)
            {
            }
        }

        s->fn = f;

        active.Push ((List*)s);

        return &s->fn;
    }

    void operator-=(void* f)
    {
        Unregister (f);
    }

    bool operator==(void* target)
    {
        return &active == active.next;
    }

    bool operator!=(void* target)
    {
        return &active != active.next;
    }

  private:
    void Unregister (EventHandle f)
    {
        for (auto pos = active.next; pos != &active; pos = pos->next)
        {
            Subscriber* s = (Subscriber*)pos;

            if ((EventHandle)&s->fn == f)
            {
                active.Remove (pos);
                GlobalEventHandlers::Available.Push (pos);
                break;
            }
        }
    }

    List active;
};

// typedef Event<EventArgs>::EventHandler EventHandler;
#endif
