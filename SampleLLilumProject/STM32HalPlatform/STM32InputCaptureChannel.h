/******************************************************************************
*       Description:
*
*       Author:
*         Date: 13 May 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _STM32INPUTCAPTURE_H_
#define _STM32INPUTCAPTURE_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>
#include "IInputCaptureChannel.h"
#include "Interrupt.h"
#include "STM32Timer.h"

class STM32Timer;

class STM32InputCaptureChannel;

class STM32InputCaptureInterrupt : public Interrupt
{
  public:
    STM32InputCaptureInterrupt (STM32InputCaptureChannel* owner,
                                uint32_t interruptNumber);

    void ISR (void);

  private:
    STM32InputCaptureChannel* owner;
};

class STM32InputCaptureChannel : public IInputCaptureChannel
{
    friend STM32InputCaptureInterrupt;

#pragma mark Public Members
  public:
    STM32InputCaptureChannel (STM32Timer& timer, uint32_t channel,
                              uint32_t interruptNumber);
    ~STM32InputCaptureChannel ();

    uint32_t GetCapture ();

    float GetFrequency ();

#pragma mark Private Members
  private:
    STM32InputCaptureInterrupt interrupt;
    STM32Timer& timer;
    uint32_t channel;
    volatile uint32_t overFlows;
    uint32_t period;
    uint32_t lastValue;
    volatile uint32_t value;
    volatile bool hasCaptured;
    
    
    void OnCapture ();
};

#endif
