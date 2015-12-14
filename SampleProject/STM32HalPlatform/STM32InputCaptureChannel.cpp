/******************************************************************************
*       Description:
*
*       Author:
*         Date: 13 May 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#pragma mark Includes
#include "STM32InputCaptureChannel.h"
#include <cstring>

#pragma mark Definitions and Constants


#pragma mark Static Data


#pragma mark Static Functions


#pragma mark Member Implementations

STM32InputCaptureInterrupt::STM32InputCaptureInterrupt (
STM32InputCaptureChannel* owner, uint32_t interruptNumber)
{
    this->owner = owner;
    Register (interruptNumber, this);
}

void STM32InputCaptureInterrupt::ISR ()
{
    owner->OnCapture ();
}


STM32InputCaptureChannel::STM32InputCaptureChannel (STM32Timer& timer,
                                                    uint32_t channel,
                                                    uint32_t interruptNumber)
    : interrupt (this, interruptNumber), timer(timer)
{
    TIM_IC_InitTypeDef sConfig;

    memset (&sConfig, 0, sizeof (TIM_IC_InitTypeDef));

    this->channel = channel;
    lastValue = 0;
    overFlows = 0;

    period = timer.GetPeriod () + 1;

    // Starts overflow notification.
    timer.Start ();

    HAL_TIM_IC_Init (timer.handle);

    sConfig.ICPrescaler = TIM_ICPSC_DIV1;
    sConfig.ICFilter = 0x0F;
    sConfig.ICPolarity = TIM_ICPOLARITY_RISING;
    sConfig.ICSelection = TIM_ICSELECTION_DIRECTTI;
    HAL_TIM_IC_ConfigChannel (timer.handle, &sConfig, channel);

    HAL_TIM_IC_Start_IT (timer.handle, channel);

    this->timer.Elapsed += [this](auto sender, auto e)
    {
        overFlows++;
    };

    count = 0;
}

STM32InputCaptureChannel::~STM32InputCaptureChannel ()
{
}

uint32_t STM32InputCaptureChannel::GetCapture ()
{
    auto result = value;

    // value = 0;

    return result;
}

float STM32InputCaptureChannel::GetFrequency ()
{
    float result = 0;

    if (!hasCaptured)
    {
        result = 0;
    }
    else
    {
        hasCaptured = false;

        if (value != 0)
        {
            result = timer.GetFrequency () / (float)value;
        }
    }

    return result;
}

void STM32InputCaptureChannel::OnCapture ()
{
    uint32_t overFlow = 0;

    uint32_t count = HAL_TIM_ReadCapturedValue (timer.handle, channel);

    if (count > lastValue)
    {
        value = (count - lastValue);

        overFlow = (period * overFlows);
    }
    else
    {
        // TODO this code is broken!
        value = ((period - lastValue) + count);

        overFlow = (period * (overFlows - 1));
    }

    overFlows = 0;

    value += overFlow;

    lastValue = count;

    hasCaptured = true;

    this->count++;

    if (Captured != nullptr)
    {
        static EventArgs args;
        Captured (this, args);
    }
}
