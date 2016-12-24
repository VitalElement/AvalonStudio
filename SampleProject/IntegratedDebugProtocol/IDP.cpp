/******************************************************************************
*       Description:
*
*       Author:
*         Date: January 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#pragma mark Includes
#include "IDP.h"
#include "CRC.h"

#pragma mark Definitions and Constants


#pragma mark Static Data


#pragma mark Static Functions


#pragma mark Member Implementations
IDP::IDP ()
{
    receiveBuffer = new CircularBuffer<uint8_t> (&rxData[0], 128);
    Reset ();
}

IDP::~IDP ()
{
}

float IDP::ReadFloat ()
{
    float result;
    uint8_t* ptr = (uint8_t*)&result;

    *ptr++ = ReadByte ();
    *ptr++ = ReadByte ();
    *ptr++ = ReadByte ();
    *ptr++ = ReadByte ();

    return result;
}

uint8_t IDP::ReadByte ()
{
    uint8_t result = 0;

    result = receiveBuffer->Read ();

    return result;
}

bool IDP::ReadBool ()
{
    return ReadByte ();
}

void IDP::Reset ()
{
    bytesRead = 0;
    receivedCrc = 0;
    receiveBuffer->Clear ();
    state = &IDP::WaitingForStx;
}

void IDP::WaitingForStx (uint8_t data)
{
    if (data == 0x02)
    {
        state = &IDP::ReadingLengthH;
        bytesRead++;
        receivedCrc = CRC::Crc16 (receivedCrc, data);
    }
}

void IDP::ReadingLengthH (uint8_t data)
{
    length = (uint16_t)(data << 8);
    state = &IDP::ReadingLengthL;
    bytesRead++;
    receivedCrc = CRC::Crc16 (receivedCrc, data);
}

void IDP::ReadingLengthL (uint8_t data)
{
    length |= data;

    if (length > 64)
    {
        Reset ();
    }
    else
    {
        state = &IDP::WaitingForEtx;
        bytesRead++;
        receivedCrc = CRC::Crc16 (receivedCrc, data);
    }
}

void IDP::WaitingForEtx (uint8_t data)
{
    bytesRead++;
    receivedCrc = CRC::Crc16 (receivedCrc, data);

    if (bytesRead >= (uint32_t)(length - 2))
    {
        if (data == 0x03)
        {
            state = &IDP::ReadingCRCH;
        }
        else
        {
            Reset ();
        }
    }
    else
    {
        this->receiveBuffer->Write (data);
    }
}

void IDP::ReadingCRCH (uint8_t data)
{
    crc = (uint16_t)(data << 8);
    state = &IDP::ReadingCRCL;
}

void IDP::ReadingCRCL (uint8_t data)
{
    crc |= data;

    if (crc == receivedCrc)
    {
        if (PacketReceived != nullptr)
        {
            EventArgs args;
            PacketReceived (this, args);
        }
    }

    Reset ();
}

void IDP::ProcessByte (uint8_t data)
{
    (this->*state)(data);
}
