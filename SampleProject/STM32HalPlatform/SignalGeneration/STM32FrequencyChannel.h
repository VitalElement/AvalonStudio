/******************************************************************************
*       Description:
*
*       Author:
*         Date: 30 April 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _STM32FREQUENCYCHANNEL_H_
#define _STM32FREQUENCYCHANNEL_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>
#include "Interrupt.h"
#include "STM32Timer.h"
#include "SignalGenerator/IFrequencyChannel.h"
#include "PinPacks.h"

class STM32Timer;

class STM32FrequencyChannel;

class STM32FrequencyChannelInterrupt : public Interrupt
{
    public:
    STM32FrequencyChannelInterrupt (STM32FrequencyChannel* owner,
                                    uint32_t interruptNumber);

    void ISR (void);

  private:
    STM32FrequencyChannel* owner;
};


class STM32FrequencyChannel : public IFrequencyChannel
{
    friend class STM32FrequencyChannelInterrupt;

#pragma mark Public Members
  public:
STM32FrequencyChannel (STM32Timer& timer, uint32_t channel,
                           uint32_t interruptNumber);

    ~STM32FrequencyChannel ();

    void SetFrequency (float frequencyHz);

    void SetCount (uint32_t count);

    float GetMaxTimePeriod ();

    void Start ();

    void Stop ();

    uint32_t FrequencyToCount (float frequencyHz);

    void ISR ();


#pragma mark Private Members
  private:
    bool pinState;
    bool stopRequested;
    uint16_t reloadValue;

    STM32Timer& timer;
    uint32_t channel;

    void Enable (bool);

    void Initialise ();

    STM32FrequencyChannelInterrupt interrupt;
};

#endif
