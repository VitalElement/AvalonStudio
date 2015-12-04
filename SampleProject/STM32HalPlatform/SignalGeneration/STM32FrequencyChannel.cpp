/******************************************************************************
*       Description:
*
*       Author:
*         Date: 30 April 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#pragma mark Includes
#include "STM32FrequencyChannel.h"
#include <string.h>

#pragma mark Definitions and Constants


#pragma mark Static Data


#pragma mark Static Functions


#pragma mark Member Implementations
STM32FrequencyChannelInterrupt::STM32FrequencyChannelInterrupt (
STM32FrequencyChannel* owner, uint32_t interruptNumber)
{
    this->owner = owner;

    Interrupt::Register (interruptNumber, this);
}

void STM32FrequencyChannelInterrupt::ISR ()
{
    owner->ISR ();
}


STM32FrequencyChannel::STM32FrequencyChannel (STM32Timer& timer,
                                              uint32_t channel,
                                              uint32_t interruptNumber)
    : timer (timer), interrupt (this, interruptNumber)
{
    this->pinState = false;
    this->channel = channel;
    this->stopRequested = false;

    Initialise ();
}

STM32FrequencyChannel::~STM32FrequencyChannel ()
{
}

void STM32FrequencyChannel::SetFrequency (float frequencyHz)
{
    this->SetCount (this->FrequencyToCount (frequencyHz));
}

uint32_t STM32FrequencyChannel::FrequencyToCount (float frequencyHz)
{
    float count = (timer.GetFrequency () / frequencyHz);

    if (count > timer.GetPeriod ())
    {
        count = timer.GetPeriod ();
    }

    return (uint32_t)count;
}

void STM32FrequencyChannel::ISR ()
{
    pinState = !pinState;

    auto capture = __HAL_TIM_GET_COMPARE (timer.handle, channel);

    if (!pinState)
    {
        if (CycleCompleted != nullptr)
        {
            EventArgs args;
            CycleCompleted (this, args);
        }


        if (stopRequested)
        {
            Enable (false);
            stopRequested = false;
        }
    }

    __HAL_TIM_SET_COMPARE (timer.handle, channel, capture + reloadValue);
}

void STM32FrequencyChannel::SetCount (uint32_t count)
{
    reloadValue = count;
}

float STM32FrequencyChannel::GetMaxTimePeriod ()
{
    float result = 0;

    result = ((float)0xFFFF / timer.GetFrequency ());

    return result;
}

void STM32FrequencyChannel::Start ()
{
    Enable (true);
}

void STM32FrequencyChannel::Stop ()
{
    this->stopRequested = true;
}


void STM32FrequencyChannel::Enable (bool enable)
{
    if (enable)
    {
        __HAL_TIM_SET_COMPARE (timer.handle, channel,
                               __HAL_TIM_GetCompare (timer.handle, channel) +
                               reloadValue);

        HAL_TIM_OC_Start_IT (timer.handle, channel);
    }
    else
    {
        HAL_TIM_OC_Stop_IT (timer.handle, channel);
    }
}


void STM32FrequencyChannel::Initialise ()
{
    TIM_OC_InitTypeDef sConfig;

    memset (&sConfig, 0, sizeof (TIM_OC_InitTypeDef));

    sConfig.OCMode = TIM_OCMODE_TOGGLE;
    sConfig.OCPolarity = TIM_OCPOLARITY_LOW;
    sConfig.Pulse = 0xFFFF;

    HAL_TIM_OC_ConfigChannel (timer.handle, &sConfig, channel);
}
