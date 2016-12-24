/******************************************************************************
*       Description:
*
*       Author:
*         Date: January 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _IUART_H_
#define _IUART_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>
#include "../Utils/Event.h"

class IUart
{
#pragma mark Public Members
  public:
    virtual ~IUart ()
    {
    }

    virtual void Send (uint8_t data) = 0;
    virtual uint8_t Receive (void) = 0;    

#pragma mark Private Members
  private:
};

#endif
