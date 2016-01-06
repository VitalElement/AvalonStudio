/******************************************************************************
*       Description:
*
*       Author:
*         Date: January 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _ISPI_H_
#define _ISPI_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>
#include "../Utils/Event.h"
#include "../Utils/CircularBuffer.h"

class ISpi
{
#pragma mark Public Members
  public:
    enum SpiMode
    {
        Mode0,
        Mode1,
        Mode2,
        Mode3,
        ModeNone
    };

    virtual ~ISpi ()
    {
    }

    virtual uint8_t Tranceive (uint8_t data) = 0;
    virtual void Send (uint8_t data) = 0;
    virtual uint8_t Receive (void) = 0;

    Event<EventArgs> DataRecieved;
    Event<EventArgs> DataSent;

    virtual void SetMode (SpiMode mode) = 0;

    SpiMode CurrentMode;

#pragma mark Private Members
  private:
};

#endif
