/******************************************************************************
*       Description:
*
*       Author:
*         Date: January 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _IDPPACKET_H_
#define _IDPPACKET_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>
#include "CBuffer.h"

class IDPPacket
{
#pragma mark Public Members
  public:
    IDPPacket ();
    IDPPacket (Buffer* buffer);
    ~IDPPacket ();

    void SetBuffer (Buffer* buffer);

    void Reset ();
    void Add (bool data);
    void Add (uint8_t data);
    void Add (uint16_t data);
    void Add (uint32_t data);
    void Add (float data);
    void Add (void* object, uint32_t size);

    void Finalise ();

    Buffer* GetPacket ();


#pragma mark Private Members
  private:
    Buffer* buffer;
};

#endif
