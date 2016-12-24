/******************************************************************************
*       Description:
*
*       Author:
*         Date: 27 April 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _IUSBHIDDEVICE_H_
#define _IUSBHIDDEVICE_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>
#include "CBuffer.h"
#include "Event.h"

#define LO_BYTE(x) ((uint8_t)(x & 0x00FF))
#define HI_BYTE(x) ((uint8_t)((x & 0xFF00) >> 8))

class UsbTransactionEventArgs : public EventArgs
{
  public:
    uint8_t reportId;
    Buffer* data;
};


class IUsbHidDevice
{
#pragma mark Public Members
  public:
    virtual void Initialise (Buffer* deviceDescriptor, Buffer* configDescriptor,
                             Buffer* reportDescriptor,
                             Buffer* manufacturerString, Buffer* productString,
                             Buffer* serialString, uint8_t inEndpointAddress,
                             uint16_t inEndpointSize,
                             uint8_t outEndpointAddress,
                             uint16_t outEndpointSize) = 0;
                             
    virtual void WriteFeatureReport (uint8_t reportId, Buffer* buffer) = 0;
    virtual Buffer* ReadFeatureReport (uint8_t reportId) = 0;
    virtual void SendReport (Buffer* buffer) = 0;
    virtual bool IsConnected () = 0;

    virtual void InitialiseStack () = 0;

    Event<UsbTransactionEventArgs> OnUsbOutput;
    Event<UsbTransactionEventArgs> OnUsbInput;


#pragma mark Private Members
  private:
};

#endif
