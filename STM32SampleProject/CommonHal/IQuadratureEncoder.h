/******************************************************************************
*       Description:
*
*       Author:
*         Date: 08 April 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _IQUADRATUREENCODER_H_
#define _IQUADRATUREENCODER_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>

class IQuadratureEncoder
{
#pragma mark Public Members
  public:

    virtual int32_t GetCounter () = 0;


#pragma mark Private Members
  private:
};

#endif
