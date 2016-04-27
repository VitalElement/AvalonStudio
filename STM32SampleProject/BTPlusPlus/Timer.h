/******************************************************************************
*       Description:
*
*       Author:
*         Date: 23 July 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _TIMER_H_
#define _TIMER_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>
#include "Event.h"

class Timer
{
#pragma mark Public Members
  public:
    Timer (uint32_t periodMs, bool autoReload);
    ~Timer ();

    void Start ();
    void Stop ();

    static Timer* Create (uint32_t periodMs, bool autoReload);

    Event<EventArgs> Elapsed;


#pragma mark Private Members
  private:
    static void OnTick (void* handle);
    void OnElapsed ();
    void* handle;
};

#endif
