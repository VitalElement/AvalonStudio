/******************************************************************************
*       Description:
*
*       Author:
*         Date: January 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _ITIMER_H_
#define _ITIMER_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>
#include "Event.h"

class ITimer
{
#pragma mark Public Members
  public:
    virtual uint32_t GetPeriod () = 0;
    virtual uint32_t GetValue () = 0;
    virtual void Start () = 0;
    virtual void Stop () = 0;
    virtual float GetFrequency () = 0;

    Event<EventArgs> Elapsed;
#pragma mark Private Members
  private:
};

#endif
