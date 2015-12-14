/******************************************************************************
*       Description:
*
*       Author:
*         Date: 23 July 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _STM32QUADRATUREENCODER_H_
#define _STM32QUADRATUREENCODER_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>
#include "IQuadratureEncoder.h"
#include "STM32Timer.h"

class STM32Timer;

class STM32QuadratureEncoder : public IQuadratureEncoder
{
#pragma mark Public Members
  public:
    STM32QuadratureEncoder (const STM32Timer& timer, uint32_t channelA,
                            uint32_t channelB);
    ~STM32QuadratureEncoder ();

    int16_t GetCounter ();


#pragma mark Private Members
  private:
    const STM32Timer& timer;
    uint32_t channelA;
    uint32_t channelB;
};

#endif
