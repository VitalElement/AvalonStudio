/******************************************************************************
*       Description:
*
*       Author:
*         Date: 16 April 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#pragma mark Includes
#include "SerialPort.h"

#pragma mark Definitions and Constants


#pragma mark Static Data


#pragma mark Static Functions


#pragma mark Member Implementations
SerialPort::SerialPort (uint8_t* transmitData, uint32_t transmitDataLength,
                        uint8_t* receiveData, uint32_t receiveDataLength)
{
    this->receiveBuffer =
    new CircularBuffer<uint8_t> (receiveData, receiveDataLength);

    this->transmitBuffer =
    new CircularBuffer<uint8_t> (transmitData, transmitDataLength);
}

void SerialPort::Print (const char* string, uint32_t length)
{
    Send ((uint8_t*)string, length);
}

void SerialPort::Print (const char data)
{
    Send (data);
}

void SerialPort::Send (const char* string)
{
    while (*string != '\0')
    {
        this->transmitBuffer->Write (*string++);
    }

    if (!this->isTransmitting)
    {
        isTransmitting = true;
        TransmitData ();
    }
}

void SerialPort::Send (uint8_t data)
{
    this->transmitBuffer->Write (data);

    if (!this->isTransmitting)
    {
        isTransmitting = true;
        TransmitData ();
    }
}

void SerialPort::Send (uint8_t* data, uint32_t length)
{
    while (length--)
    {
        this->transmitBuffer->Write (*data++);
    }

    if (!this->isTransmitting)
    {
        isTransmitting = true;
        TransmitData ();
    }
}

uint8_t SerialPort::Receive (void)
{
    return this->receiveBuffer->Read ();
}

void SerialPort::ReceiveData (void)
{
    if (DataReceived != nullptr)
    {
        EventArgs args;
        DataReceived (this, args);
    }
}

bool SerialPort::HasBytes (void)
{
    return !this->receiveBuffer->IsEmpty ();
}

SerialPort::~SerialPort ()
{
}
