/******************************************************************************
*       Description:
*
*       Author:
*         Date: 20 April 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _IPRINTER_H_
#define _IPRINTER_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>

class IPrinter
{
#pragma mark Public Members
  public:
    virtual void Print (const char* string, uint32_t length) = 0;
    virtual void Print (const char data) = 0;


#pragma mark Private Members
  private:
};

#endif
