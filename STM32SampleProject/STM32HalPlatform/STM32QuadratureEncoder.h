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

class STM32QuadratureEncoder;

class STM32QuadratureEncoderInterrupt : public Interrupt
{
  public:
    STM32QuadratureEncoderInterrupt (STM32QuadratureEncoder* owner,
                                     uint32_t interruptNumber);

    void ISR (void);

  private:
    STM32QuadratureEncoder* owner;
};

class STM32QuadratureEncoder : public IQuadratureEncoder
{
    friend STM32QuadratureEncoderInterrupt;

#pragma mark Public Members
  public:
    STM32QuadratureEncoder (const STM32Timer& timer, uint32_t channelA,
                            uint32_t channelB, uint32_t interruptNumber);
    ~STM32QuadratureEncoder ();

    int32_t GetCounter ();
    uint16_t GetCount ();

    void Update ();

    void Reset ();


#pragma mark Private Members
  private:
    // distances
    volatile uint16_t lastCount;
    volatile int32_t currentPosition;
    volatile bool overFlow;
    volatile bool overFlowDirection;
    volatile bool reset;

    const STM32Timer& timer;
    uint32_t channelA;
    uint32_t channelB;
    int32_t overFlows;
    void ResetCounter ();
    STM32QuadratureEncoderInterrupt interrupt;
};

#endif
