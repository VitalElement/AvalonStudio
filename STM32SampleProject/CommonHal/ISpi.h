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
    virtual void Send (void* data, uint16_t length) = 0;
    virtual void Receive (void* buffer, uint16_t length) = 0;

    virtual void SetMode (SpiMode mode) = 0;
    virtual void SetDuplex (bool full) = 0;
    virtual void SetBiDirectional (bool enable) = 0;

    Event<EventArgs> DataRecieved;
    Event<EventArgs> DataSent;
    SpiMode CurrentMode;

#pragma mark Private Members
  private:
};

#endif
