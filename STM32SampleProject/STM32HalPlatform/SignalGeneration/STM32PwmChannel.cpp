/******************************************************************************
*       Description:
*
*       Author:
*         Date: 05 May 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#pragma mark Includes
#include "STM32PwmChannel.h"

#pragma mark Definitions and Constants


#pragma mark Static Data


#pragma mark Static Functions


#pragma mark Member Implementations
STM32PwmChannel::STM32PwmChannel (STM32Timer& timer, uint32_t channel,
                                  float maxDutyCycle)
    : timer (timer)
{
    this->channel = channel;
    this->maxDutyCycle = maxDutyCycle;

    TIM_OC_InitTypeDef sConfigOC;

    sConfigOC.OCMode = TIM_OCMODE_PWM2;
    sConfigOC.Pulse = 0;
    sConfigOC.OCPolarity = TIM_OCPOLARITY_LOW;
    sConfigOC.OCFastMode = TIM_OCFAST_DISABLE;
    sConfigOC.OCIdleState = TIM_OCIDLESTATE_RESET;

    HAL_TIM_PWM_ConfigChannel (timer.handle, &sConfigOC, channel);

    switch (channel)
    {
    case TIM_CHANNEL_1:
        CCRreg = &timer.handle->Instance->CCR1;
        break;

    case TIM_CHANNEL_2:
        CCRreg = &timer.handle->Instance->CCR2;
        break;

    case TIM_CHANNEL_3:
        CCRreg = &timer.handle->Instance->CCR3;
        break;

    case TIM_CHANNEL_4:
        CCRreg = &timer.handle->Instance->CCR4;
        break;
    }

    Stop ();
        
    SetDutyCycle (0); // Otherwise any random number could be in CCReg.
}

STM32PwmChannel::~STM32PwmChannel ()
{
}

void STM32PwmChannel::SetDutyCycle (float dutyCyclePc)
{
    if (dutyCyclePc > maxDutyCycle)
    {
        dutyCyclePc = maxDutyCycle;
    }
    else if (dutyCyclePc < 0)
    {
        dutyCyclePc = 0;
    }

    uint32_t value =
    (((float)timer.handle->Init.Period) / 100.0f) * dutyCyclePc;

    if (value > UINT16_MAX)
    {
        value = UINT16_MAX;
    }


    *CCRreg = value;
}

void STM32PwmChannel::Start ()
{
    HAL_TIM_PWM_Start (timer.handle, channel);
}

void STM32PwmChannel::Stop ()
{
    HAL_TIM_PWM_Stop (timer.handle, channel);
}

uint32_t STM32PwmChannel::GetTimerValue ()
{
    return timer.handle->Instance->CNT;
}
