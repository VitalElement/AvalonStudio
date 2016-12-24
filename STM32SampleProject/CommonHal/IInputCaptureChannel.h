/******************************************************************************
*       Description:
*
*       Author:
*         Date: 13 May 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _IINPUTCAPTURE_H_
#define _IINPUTCAPTURE_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>
#include "Event.h"

class IInputCaptureChannel
{
#pragma mark Public Members
  public:
    virtual float GetFrequency () = 0;
    virtual uint32_t GetCapture () = 0;

    Event<EventArgs> Captured;
    volatile uint32_t count;

#pragma mark Private Members
  private:
};

#endif
