/******************************************************************************
*       Description:
*
*       Author:
*         Date: 27 April 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _STM32USBHIDDEVICE_H_
#define _STM32USBHIDDEVICE_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>
#include "IUsbHidDevice.h"

class STM32UsbHidDevice : public IUsbHidDevice
{
#pragma mark Public Members
  public:
    STM32UsbHidDevice ();

    ~STM32UsbHidDevice ();

    void Initialise (Buffer* deviceDescriptor, Buffer* configDescriptor,
                     Buffer* reportDescriptor, Buffer* manufacturerString,
                     Buffer* productString, Buffer* serialString,
                     uint8_t inEndpointAddress, uint16_t inEndpointSize,
                     uint8_t outEndpointAddress, uint16_t outEndpointSize);

    void WriteFeatureReport (uint8_t reportId, Buffer* buffer);
    Buffer* ReadFeatureReport (uint8_t reportId);
    void SendReport (Buffer* buffer);
    bool IsConnected ();

    void InitialiseStack ();


#pragma mark Private Members
  private:
    Buffer deviceDescriptor;
    Buffer configDescriptor;
    Buffer reportDescriptor;
    Buffer manufacturerString;
    Buffer productString;
    Buffer serialString;
};

#endif
