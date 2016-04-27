/******************************************************************************
*       Description:
*
*       Author:
*         Date: 29 May 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _STM32ADCCHANNEL_H_
#define _STM32ADCCHANNEL_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>
#include "stm32f4xx_hal.h"
#include "IAdc.h"

class STM32Adc;

class STM32AdcChannel : public IAdcChannel
{
    friend class STM32Adc;
#pragma mark Public Members
  public:
    ~STM32AdcChannel ();

    float GetVoltage ();
    void Start ();


#pragma mark Private Members
  private:
    STM32AdcChannel (STM32Adc& adc, uint32_t channel);
    STM32Adc& adc;
    ADC_ChannelConfTypeDef channelConfig;
};

class STM32Adc : public IAdc
{
    friend class STM32AdcChannel;

#pragma mark Public Members
  public:
    STM32Adc (ADC_HandleTypeDef* adcHandle);
    ~STM32Adc ();

    IAdcChannel& GetChannel (uint32_t channel);

#pragma mark Private Members
  private:
    ADC_HandleTypeDef* adcHandle;
    ADC_ChannelConfTypeDef channelConfig;       
};

#endif
