/******************************************************************************
*       Description:
*
*       Author:
*         Date: 18 March 2016
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#pragma mark Includes
#include "STM32Spi.h"
#include <string.h>

#pragma mark Definitions and Constants


#pragma mark Static Data


#pragma mark Static Functions


#pragma mark Member Implementations
STM32Spi::STM32Spi (SPI_HandleTypeDef* handle)
{
    this->handle = handle;

    SPI_InitTypeDef initData;
    memset (&initData, 0, sizeof (SPI_InitTypeDef));

    initData.Direction = SPI_DIRECTION_1LINE;
    initData.Mode = SPI_MODE_MASTER;
    initData.DataSize = SPI_DATASIZE_16BIT;
    initData.CLKPolarity = SPI_POLARITY_HIGH;
    initData.CLKPhase = SPI_PHASE_1EDGE;
    initData.NSS = SPI_NSS_SOFT;
    initData.BaudRatePrescaler = SPI_BAUDRATEPRESCALER_64;
    initData.FirstBit = SPI_FIRSTBIT_MSB;
    // initData.CRCPolynomial = 7;

    handle->Init = initData;

    HAL_SPI_Init (handle);

    HAL_GPIO_WritePin (GPIOB, GPIO_PIN_14, GPIO_PinState::GPIO_PIN_SET);
}

STM32Spi::~STM32Spi ()
{
}

uint8_t STM32Spi::Tranceive (uint8_t data)
{
}

void STM32Spi::Send (void* data, uint16_t length)
{
    HAL_SPI_Transmit (handle, (uint8_t*)data, length, 200);
}

void STM32Spi::Send (uint16_t data)
{
    HAL_SPI_Transmit (handle, (uint8_t*)&data, 2, 200);
}

uint16_t STM32Spi::Receive16 (void)
{
    uint16_t result;

    HAL_SPI_Receive (handle, (uint8_t*)&result, 2, 200);

    return result;
}

void STM32Spi::Receive (void* data, uint16_t length)
{
    HAL_SPI_Receive (handle, (uint8_t*)data, length, 200);
}

void STM32Spi::SetMode (SpiMode mode)
{
}

void STM32Spi::SetDuplex (bool full)
{
}

void STM32Spi::SetBiDirectional (bool enable)
{
}
