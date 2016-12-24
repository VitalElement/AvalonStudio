/******************************************************************************
*       Description:
*
*       Author:
*         Date: 18 March 2016
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _STM32SPI_H_
#define _STM32SPI_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>
#include "stm32f4xx_hal.h"
#include "ISpi.h"

class STM32Spi : public ISpi
{
#pragma mark Public Members
  public:
    STM32Spi (SPI_HandleTypeDef* handle);
    ~STM32Spi ();
    
    virtual uint8_t Tranceive (uint8_t data);
    virtual void Send (void* data, uint16_t length);
    virtual void Receive (void* data, uint16_t length);

    void Send (uint16_t data);
    uint16_t Receive16 (void);    

    virtual void SetMode (SpiMode mode);
    virtual void SetDuplex (bool full);
    virtual void SetBiDirectional (bool enable);


#pragma mark Private Members
  private:
    SPI_HandleTypeDef* handle;
};

#endif
