/******************************************************************************
*       Description:
*
*       Author:
*         Date: 06 May 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#pragma mark Includes
#include "STM32Timer.h"
#include <cstring>

#pragma mark Definitions and Constants


#pragma mark Static Data


#pragma mark Static Functions


#pragma mark Member Implementations

STM32TimerInterrupt::STM32TimerInterrupt (STM32Timer* owner,
                                          uint32_t interruptNumber)
{
    this->owner = owner;
    Register (interruptNumber, this);
}

void STM32TimerInterrupt::ISR (void)
{
    owner->ISR ();
}


STM32Timer::STM32Timer (TIM_HandleTypeDef* handle, uint32_t interruptNumber,
                        uint32_t clockFrequency, float desiredFrequency,
                        uint32_t period)
    : interrupt (this, interruptNumber)
{
    this->handle = handle;
    this->clockFrequency = clockFrequency;

    TIM_ClockConfigTypeDef sClockSourceConfig;
    TIM_MasterConfigTypeDef sMasterConfig;
    TIM_OC_InitTypeDef sConfigOC;

    memset (&sClockSourceConfig, 0, sizeof (TIM_ClockConfigTypeDef));
    memset (&sMasterConfig, 0, sizeof (TIM_MasterConfigTypeDef));
    memset (&sConfigOC, 0, sizeof (TIM_OC_InitTypeDef));

    sClockSourceConfig.ClockSource = TIM_CLOCKSOURCE_INTERNAL;
    HAL_TIM_ConfigClockSource (handle, &sClockSourceConfig);

    handle->Init.Prescaler =
    (uint32_t) ((clockFrequency / desiredFrequency) - 1);
    handle->Init.CounterMode = TIM_COUNTERMODE_UP;
    handle->Init.Period = period;
    handle->Init.ClockDivision = TIM_CLOCKDIVISION_DIV1;
    __HAL_TIM_CLEAR_IT (handle, TIM_IT_UPDATE);
    __HAL_TIM_CLEAR_FLAG (handle, TIM_FLAG_UPDATE);


    HAL_TIM_Base_Init (handle);
}

STM32Timer::~STM32Timer ()
{
}

void STM32Timer::InitialisePWM ()
{
    HAL_TIM_PWM_Init (handle);
}

void STM32Timer::InitialiseOC ()
{
    HAL_TIM_OC_Init (handle);
}

uint32_t STM32Timer::GetPeriod ()
{
    return handle->Init.Period;
}

float STM32Timer::GetFrequency ()
{
    return (clockFrequency) / (handle->Init.Prescaler + 1);
}

uint32_t STM32Timer::GetValue ()
{
    return handle->Instance->CNT;
}

void STM32Timer::Start ()
{
    HAL_TIM_Base_Start_IT (handle);
}
void STM32Timer::Stop ()
{
    HAL_TIM_Base_Stop_IT (handle);
}

void STM32Timer::ISR ()
{
    if (Elapsed != nullptr)
    {
        EventArgs args;
        Elapsed (this, args);
    }
}
