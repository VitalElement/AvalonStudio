/******************************************************************************
*       Description:
*
*       Author:
*         Date: 14 December 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _STM32F4GRAPHIC_H_
#define _STM32F4GRAPHIC_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>
#include "stm32f4xx_hal_dma2d.h"



class STM32F4Graphic
{
#pragma mark Public Members
  public:
    STM32F4Graphic ();
    ~STM32F4Graphic ();

    void Init ();
#pragma mark Private Members
  private:
    DMA2D_InitTypeDef graphicInit;
    
    uint16_t Width;
	uint16_t Height;
	uint16_t CurrentWidth;
	uint16_t CurrentHeight;
	uint32_t StartAddress;
	uint32_t LayerOffset;
	uint32_t Offset;
	uint32_t Pixels;
	uint8_t Initialized;
	uint8_t Orientation;
	uint8_t PixelSize;    
};

#endif
