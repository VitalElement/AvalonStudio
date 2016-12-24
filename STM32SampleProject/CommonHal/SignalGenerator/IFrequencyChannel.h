/******************************************************************************
*       Description: 
*
*       Author: 
*         Date: 01 April 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _IFREQUENCYCHANNEL_H_
#define _IFREQUENCYCHANNEL_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>
#include "Event.h"

class IFrequencyChannel
{
#pragma mark Public Members
public:   
    virtual void SetFrequency (float frequencyHz) = 0;
    // virtual void SetCount (uint32_t count) = 0;

    // virtual float GetMaxTimePeriod () = 0;
    // virtual uint32_t FrequencyToCount (float frequency) = 0;
    virtual void Stop () = 0;
    virtual void Start () = 0;
    
    virtual void ISR () = 0;

    // virtual uint32_t GetTimerFrequency () = 0;
    Event<EventArgs> CycleCompleted;


#pragma mark Private Members
private:

};

#endif
