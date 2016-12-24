/******************************************************************************
*       Description:
*
*       Author:
*         Date: 16 April 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _SERIALPORT_H_
#define _SERIALPORT_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>
#include "Event.h"
#include "ISerialPort.h"
#include "IPrinter.h"
#include "CircularBuffer.h"

class SerialPort : public ISerialPort, public IPrinter
{
#pragma mark Public Members
  public:
    SerialPort (uint8_t* transmitData, uint32_t transmitDataLength,
                uint8_t* receiveData, uint32_t receiveDataLength);

    ~SerialPort ();

    void Send (const char* string);    
    void Send (uint8_t* data, uint32_t length);
    void Send (uint8_t data);
    
    void Print (const char* string, uint32_t length);
    void Print (const char data);

    uint8_t Receive (void);
    bool HasBytes (void);

    Event<EventArgs> DataReceived;

#pragma mark Protected Members
  protected:
    virtual void ReceiveData (void);
    virtual void TransmitData (void) = 0;
    volatile bool isTransmitting;
    CircularBuffer<uint8_t>* receiveBuffer;
    CircularBuffer<uint8_t>* transmitBuffer;

#pragma mark Private Members
};

#endif
