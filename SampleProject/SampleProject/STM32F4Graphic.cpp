/******************************************************************************
*       Description:
*
*       Author:
*         Date: 14 December 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#pragma mark Includes
#include "STM32F4Graphic.h"
#include "stm32f4xx_hal_rcc.h"


#pragma mark Definitions and Constants
#ifndef DMA2D_GRAPHIC_LCD_WIDTH
#if defined(DMA2D_GRAPHIC_USE_STM324x9_EVAL) || defined(TM_DISCO_STM324x9_EVAL)
#define DMA2D_GRAPHIC_LCD_WIDTH 640 /*!< STM32439-Eval board */
#else
#define DMA2D_GRAPHIC_LCD_WIDTH 240 /*!< STM32F429-Discovery board */
#endif
#endif

/**
 * @brief  Default LCD height in pixels
 */
#ifndef DMA2D_GRAPHIC_LCD_HEIGHT
#if defined(DMA2D_GRAPHIC_USE_STM324x9_EVAL) || defined(TM_DISCO_STM324x9_EVAL)
#define DMA2D_GRAPHIC_LCD_HEIGHT 480 /*!< STM32439-Eval board */
#else
#define DMA2D_GRAPHIC_LCD_HEIGHT 320 /*!< STM32F429-Discovery board */
#endif
#endif

/**
 * @brief  RAM Start address for LCD
 * @note   On STM32F429-Discovery, this is address for SDRAM which operate with
 * LCD and LTDC peripheral
 */
#ifndef DMA2D_GRAPHIC_RAM_ADDR
#if defined(DMA2D_GRAPHIC_USE_STM324x9_EVAL) || defined(TM_DISCO_STM324x9_EVAL)
#define DMA2D_GRAPHIC_RAM_ADDR 0xC0000000 /*!< STM32439-Eval board */
#else
#define DMA2D_GRAPHIC_RAM_ADDR 0xD0000000 /*!< STM32F429-Discovery board */
#endif
#endif

/**
 * @brief  Timeout for DMA2D
 */
#ifndef DMA2D_GRAPHIC_TIMEOUT
#define DMA2D_GRAPHIC_TIMEOUT (uint32_t)10000000
#endif
/**
 * @brief  Number of LCD pixels
 */
#define DMA2D_GRAPHIC_PIXELS DMA2D_GRAPHIC_LCD_WIDTH* DMA2D_GRAPHIC_LCD_HEIGHT

/**
 * @defgroup TM_DMA2D_GRAPHIC_COLORS
 * @brief    Colors for DMA2D graphic library in RGB565 format
 *
 * @{
 */

#define GRAPHIC_COLOR_WHITE 0xFFFF
#define GRAPHIC_COLOR_BLACK 0x0000
#define GRAPHIC_COLOR_RED 0xF800
#define GRAPHIC_COLOR_GREEN 0x07E0
#define GRAPHIC_COLOR_GREEN2 0xB723
#define GRAPHIC_COLOR_BLUE 0x001F
#define GRAPHIC_COLOR_BLUE2 0x051D
#define GRAPHIC_COLOR_YELLOW 0xFFE0
#define GRAPHIC_COLOR_ORANGE 0xFBE4
#define GRAPHIC_COLOR_CYAN 0x07FF
#define GRAPHIC_COLOR_MAGENTA 0xA254
#define GRAPHIC_COLOR_GRAY 0x7BEF
#define GRAPHIC_COLOR_BROWN 0xBBCA

/* Waiting flags */
#define DMA2D_WORKING ((DMA2D->CR & DMA2D_CR_START))
#define DMA2D_WAIT                      \
    do                                  \
    {                                   \
        while (DMA2D_WORKING)           \
            ;                           \
        DMA2D->IFCR = DMA2D_IFSR_CTCIF; \
    } while (0);

#pragma mark Static Data


#pragma mark Static Functions


#pragma mark Member Implementations
STM32F4Graphic::STM32F4Graphic ()
{
}

STM32F4Graphic::~STM32F4Graphic ()
{
}

void STM32F4Graphic::Init ()
{
    StartAddress = DMA2D_GRAPHIC_RAM_ADDR;
    Offset = 0;
    Width = DMA2D_GRAPHIC_LCD_WIDTH;
    Height = DMA2D_GRAPHIC_LCD_HEIGHT;
    Pixels = DMA2D_GRAPHIC_PIXELS;
    CurrentHeight = DMA2D_GRAPHIC_LCD_WIDTH;
    CurrentWidth = DMA2D_GRAPHIC_LCD_HEIGHT;
    Orientation = 0;
    PixelSize = 2;
       
    __DMA2D_CLK_ENABLE ();

    Initialized = 1;
}
