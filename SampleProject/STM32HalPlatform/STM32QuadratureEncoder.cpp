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
STM32QuadratureEncoder::STM32QuadratureEncoder (const STM32Timer& timer,
                                                uint32_t channelA,
                                                uint32_t channelB)
    : timer (timer)
{
    TIM_Encoder_InitTypeDef config;
    memset (&config, 0, sizeof (TIM_Encoder_InitTypeDef));

    this->channelA = channelA;
    this->channelB = channelB;

    config.EncoderMode = TIM_ENCODERMODE_TI12;
    config.IC1Filter = 0x00;
    config.IC1Polarity = TIM_ICPOLARITY_RISING;
    config.IC1Selection = TIM_ICSELECTION_DIRECTTI;
    config.IC1Prescaler = TIM_ICPSC_DIV1;

    config.IC2Filter = 0x00;
    config.IC2Polarity = TIM_ICPOLARITY_RISING;
    config.IC2Selection = TIM_ICSELECTION_DIRECTTI;
    config.IC1Prescaler = TIM_ICPSC_DIV1;

    HAL_TIM_Encoder_Init (timer.handle, &config);

    HAL_TIM_Encoder_Start_IT (timer.handle, TIM_CHANNEL_ALL);
}

STM32QuadratureEncoder::~STM32QuadratureEncoder ()
{
}

int16_t STM32QuadratureEncoder::GetCounter ()
{
    return (int16_t)__HAL_TIM_GetCounter (timer.handle);
}
