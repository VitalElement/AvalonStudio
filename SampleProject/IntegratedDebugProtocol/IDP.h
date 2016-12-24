/******************************************************************************
*       Description:
*
*       Author:
*         Date: January 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _IDP_H_
#define _IDP_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>

#include "Event.h"
#include "CircularBuffer.h"

class IDP;

typedef void (IDP::*IDPState)(uint8_t data);

class IDP
{
#pragma mark Public Members
  public:
    IDP ();
    ~IDP ();

    void ProcessByte (uint8_t data);
    Event<EventArgs> PacketReceived;

    uint8_t ReadByte ();
    float ReadFloat ();
    bool ReadBool ();

#pragma mark Private Members
    // private:
    IDPState state;
    void WaitingForStx (uint8_t data);
    void ReadingLengthH (uint8_t data);
    void ReadingLengthL (uint8_t data);
    void WaitingForEtx (uint8_t data);
    void ReadingCRCH (uint8_t data);
    void ReadingCRCL (uint8_t data);


    CircularBuffer<uint8_t>* receiveBuffer;
    uint8_t rxData[128];

    uint32_t bytesRead;
    uint16_t length;
    uint16_t crc;
    uint16_t receivedCrc;

    void Reset (void);
};

#endif
