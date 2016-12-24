/******************************************************************************
*       Description:
*
*       Author:
*         Date: 23 July 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#pragma mark Includes
#include "Timer.h"
#include "FreeRtos.h"
#include "timers.h"
#pragma mark Definitions and Constants


#pragma mark Static Data
typedef struct
{
    Timer* timer;
} TimerHandle;

#pragma mark Static Functions


#pragma mark Member Implementations
Timer::Timer (uint32_t periodMs, bool autoReload)
{
    handle = xTimerCreate (nullptr, periodMs * portTICK_PERIOD_MS, autoReload,
                           0, OnTick, this);
}

Timer::~Timer ()
{
}

Timer* Timer::Create (uint32_t periodMs, bool autoReload)
{
    Timer* result = new Timer (periodMs, autoReload);

    return result;
}

void Timer::Start ()
{
    xTimerStart (handle, 0);
}

void Timer::Stop ()
{
    xTimerStop (handle, 0);
}

void Timer::OnElapsed ()
{
    Timer* timer = this;

    if (timer->Elapsed != nullptr)
    {
        EventArgs args;
        Elapsed (this, args);
    }
}

void Timer::OnTick (void* handle)
{
    auto timerHandle = ((TimerHandle*)handle)->timer;

    timerHandle->OnElapsed ();
}
