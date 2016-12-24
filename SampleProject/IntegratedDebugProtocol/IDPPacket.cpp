/******************************************************************************
*       Description:
*
*       Author:
*         Date: January 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#pragma mark Includes
#include "IDPPacket.h"
#include "CRC.h"

#pragma mark Definitions and Constants


#pragma mark Static Data


#pragma mark Static Functions


#pragma mark Member Implementations
IDPPacket::IDPPacket ()
{
}

IDPPacket::IDPPacket (Buffer* buffer)
{
    this->buffer = buffer;

    Reset ();
}

void IDPPacket::SetBuffer (Buffer* buffer)
{
    this->buffer = buffer;

    Reset ();
}

IDPPacket::~IDPPacket ()
{
}

void IDPPacket::Reset ()
{
    buffer->length = 0;

    Add ((uint8_t)0x02);
    Add ((uint8_t)0x00);
    Add ((uint8_t)0x00);
}

void IDPPacket::Add (uint8_t data)
{
    buffer->elements[buffer->length++] = data;
}

void IDPPacket::Add (bool data)
{
    Add ((uint8_t)data);
}

void IDPPacket::Add (uint16_t data)
{
    Add ((uint8_t) (data >> 8));
    Add ((uint8_t) (data & 0xFF));
}

void IDPPacket::Add (uint32_t data)
{
    Add (&data, sizeof (uint32_t));    
}

void IDPPacket::Add (void* object, uint32_t size)
{
    uint8_t* pData = (uint8_t*)object;

    for (uint32_t i = 0; i < size; i++)
    {
        Add (*pData++);
    }
}

void IDPPacket::Add (float data)
{
    Add (&data, sizeof (float));
}

void IDPPacket::Finalise ()
{
    Add ((uint8_t)0x03);

    uint16_t length = buffer->length + 2;

    buffer->elements[1] = (uint8_t) (length >> 8);
    buffer->elements[2] = (uint8_t) (length & 0xFF);

    uint16_t crc = 0;

    for (uint32_t i = 0; i < buffer->length; i++)
    {
        crc = CRC::Crc16 (crc, buffer->elements[i]);
    }

    Add (crc);
}

Buffer* IDPPacket::GetPacket ()
{
    return buffer;
}
