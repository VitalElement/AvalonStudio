/******************************************************************************
*       Description:
*
*       Author:
*         Date: 29 May 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#pragma mark Includes
#include "STM32Adc.h"

#pragma mark Definitions and Constants
static const float scaleFactor = 3.3f / 4096.0f;

#pragma mark Static Data


#pragma mark Static Functions


#pragma mark Member Implementations
STM32Adc::STM32Adc (ADC_HandleTypeDef* adcHandle)
{
    this->adcHandle = adcHandle;

    adcHandle->Init.ClockPrescaler = ADC_CLOCKPRESCALER_PCLK_DIV2;
    adcHandle->Init.Resolution = ADC_RESOLUTION12b;
    adcHandle->Init.ScanConvMode = DISABLE;
    adcHandle->Init.ContinuousConvMode = DISABLE;
    adcHandle->Init.DiscontinuousConvMode = DISABLE;
    adcHandle->Init.ExternalTrigConvEdge = ADC_EXTERNALTRIGCONVEDGE_NONE;
    adcHandle->Init.DataAlign = ADC_DATAALIGN_RIGHT;
    adcHandle->Init.NbrOfConversion = 1;
    adcHandle->Init.DMAContinuousRequests = DISABLE;
    adcHandle->Init.EOCSelection = EOC_SINGLE_CONV;
    HAL_ADC_Init (adcHandle);
}

STM32Adc::~STM32Adc ()
{
}

IAdcChannel& STM32Adc::GetChannel (uint32_t channel)
{
    return *new STM32AdcChannel (*this, channel);
}

STM32AdcChannel::STM32AdcChannel (STM32Adc& adc, uint32_t channel) : adc (adc)
{
    channelConfig.Channel = channel;
    channelConfig.Rank = 1;
    channelConfig.SamplingTime = ADC_SAMPLETIME_3CYCLES;
    channelConfig.Offset = 0;
}

float STM32AdcChannel::GetVoltage ()
{
    float result = 0;

    HAL_ADC_ConfigChannel (adc.adcHandle, &channelConfig);

    HAL_Delay (1);

    HAL_ADC_Start (adc.adcHandle);
    HAL_ADC_PollForConversion (adc.adcHandle, 10);

    if (HAL_ADC_GetState (adc.adcHandle) == HAL_ADC_STATE_EOC_REG)
    {
        result = HAL_ADC_GetValue (adc.adcHandle) * scaleFactor;
    }

    return result;
}

void STM32AdcChannel::Start ()
{
}
