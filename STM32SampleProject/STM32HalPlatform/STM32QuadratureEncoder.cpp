/******************************************************************************
*       Description:
*
*       Author:
*         Date: 23 July 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#pragma mark Includes
#include "STM32QuadratureEncoder.h"

#include <cstring>

#pragma mark Definitions and Constants


#pragma mark Static Data


#pragma mark Static Functions


#pragma mark Member Implementations
STM32QuadratureEncoderInterrupt::STM32QuadratureEncoderInterrupt (
STM32QuadratureEncoder* owner, uint32_t interruptNumber)
{
    this->owner = owner;
    Register (interruptNumber, this);
}

void STM32QuadratureEncoderInterrupt::ISR ()
{
    uint16_t count = owner->GetCount ();

    if (owner->lastCount != count)
    {
        owner->overFlow = true;

        if (count > owner->lastCount)
        {
            owner->overFlowDirection = false;
        }
        else
        {
            owner->overFlowDirection = true;
        }
    }
}

void STM32QuadratureEncoder::Update ()
{
    if (reset)
    {
        overFlows = 0;
        overFlow = false;
        overFlowDirection = false;
        lastCount = 0;
        currentPosition = 0;

        __HAL_TIM_SET_COUNTER (timer.handle, 0);

        reset = false;
    }
    else
    {
        uint16_t count = __HAL_TIM_GetCounter (timer.handle);

        if (overFlow)
        {
            overFlow = false;

            if (overFlowDirection)
            {
                currentPosition += ((UINT16_MAX - lastCount) + count);
            }
            else
            {
                currentPosition += ((UINT16_MAX - count) + lastCount);
            }
        }
        else
        {
            currentPosition += count - lastCount;
        }

        lastCount = count;
    }
}

uint16_t STM32QuadratureEncoder::GetCount ()
{
    return __HAL_TIM_GetCounter (timer.handle);
}


STM32QuadratureEncoder::STM32QuadratureEncoder (const STM32Timer& timer,
                                                uint32_t channelA,
                                                uint32_t channelB,
                                                uint32_t interruptNumber)
    : interrupt (this, interruptNumber), timer (timer)
{
    TIM_Encoder_InitTypeDef config;
    memset (&config, 0, sizeof (TIM_Encoder_InitTypeDef));

    this->channelA = channelA;
    this->channelB = channelB;

    config.EncoderMode = TIM_ENCODERMODE_TI12;
    config.IC1Filter = 0x00;
    config.IC1Polarity = TIM_ICPOLARITY_RISING;
    config.IC1Selection = TIM_ICSELECTION_DIRECTTI;
    config.IC1Prescaler = TIM_ICPSC_DIV4;

    config.IC2Filter = 0x00;
    config.IC2Polarity = TIM_ICPOLARITY_RISING;
    config.IC2Selection = TIM_ICSELECTION_DIRECTTI;
    config.IC1Prescaler = TIM_ICPSC_DIV4;

    HAL_TIM_Encoder_Init (timer.handle, &config);

    HAL_TIM_Encoder_Start (timer.handle, TIM_CHANNEL_ALL);

    __HAL_TIM_ENABLE_IT (timer.handle, TIM_IT_UPDATE);

    overFlows = 0;
    overFlow = false;
    overFlowDirection = false;
    lastCount = 0;
    currentPosition = 0;

    __HAL_TIM_SET_COUNTER (timer.handle, 0);
}

void STM32QuadratureEncoder::ResetCounter ()
{
    __HAL_TIM_SET_COUNTER (timer.handle, 0);
}

STM32QuadratureEncoder::~STM32QuadratureEncoder ()
{
}

int32_t STM32QuadratureEncoder::GetCounter ()
{
    // return ((uint16_t)(__HAL_TIM_GetCounter (timer.handle))) + (overFlows *
    // (int32_t)4096U);
    return (int32_t)currentPosition;
}

void STM32QuadratureEncoder::Reset ()
{
    reset = true;

    while (reset)
    {
        
    }
}
