/******************************************************************************
*       Description:
*
*       Author:
*         Date: January 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _II2C_H_
#define _II2C_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>

class II2C
{
#pragma mark Public Members
  public:
    virtual ~II2C ()
    {
    }

    virtual void Send (uint8_t address, uint8_t data) = 0;
    virtual uint8_t Receive (uint8_t address) = 0;


#pragma mark Private Members
  private:
};

#endif
