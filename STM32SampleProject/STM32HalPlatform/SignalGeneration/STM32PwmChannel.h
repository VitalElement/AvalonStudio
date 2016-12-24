/******************************************************************************
*       Description:
*
*       Author:
*         Date: 05 May 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _STM32PWMCHANNEL_H_
#define _STM32PWMCHANNEL_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>
#include "SignalGenerator/IPwmChannel.h"
#include "STM32Timer.h"

class STM32Timer;

class STM32PwmChannel : public IPwmChannel
{
#pragma mark Public Members
  public:
    STM32PwmChannel (STM32Timer& timer, uint32_t channel, float maxDutyCycle = 100);

    ~STM32PwmChannel ();

    void SetDutyCycle (float dutyCyclePc);

    void Start ();

    void Stop ();

    uint32_t GetTimerValue ();


#pragma mark Private Members
  private:
    float maxDutyCycle;
    uint16_t channel;
    STM32Timer& timer;
    volatile uint32_t* CCRreg;
};

#endif
