/******************************************************************************
*       Description:
*
*       Author:
*         Date: 14 December 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#pragma mark Includes
#include "stm32f4xx_hal.h"
#include "STM32F4Graphic.h"
#include "sdram.h"
#include "gpiof4.h"
#define LCD_WIDTH 240
#define LCD_HEIGHT 320

#pragma mark Definitions and Constants


#pragma mark Static Data
STM32F4Graphic graphic;

#pragma mark Static Functions
static void InitGPIO (GPIO_TypeDef* port, uint32_t pin, uint32_t mode,
                      uint32_t pull, uint32_t speed)
{
    GPIO_InitTypeDef gpioInit;

    gpioInit.Pin = pin;
    gpioInit.Mode = mode;
    gpioInit.Pull = pull;
    gpioInit.Speed = speed;

    HAL_GPIO_Init (port, &gpioInit);
}

static void InitGPIOAlternate (GPIO_TypeDef* port, uint32_t pin, uint32_t mode,
                               uint32_t pull, uint32_t speed,
                               uint32_t alternate)
{
    GPIO_InitTypeDef gpioInit;

    gpioInit.Alternate = alternate;
    gpioInit.Mode = mode;
    gpioInit.Pin = pin;
    gpioInit.Pull = pull;
    gpioInit.Speed = speed;

    HAL_GPIO_Init (port, &gpioInit);
}

extern "C" {
void SDRAM_Init (void);
void TFTLCD_Init (void);
void PutPixel (uint16_t x, uint16_t y, uint16_t color);

void FSMC_IRQHandler (void)
{
}
}

static void SystemClock_Config (void)
{
    RCC_OscInitTypeDef RCC_OscInitStruct;
    RCC_ClkInitTypeDef RCC_ClkInitStruct;

    __PWR_CLK_ENABLE ();

    __HAL_PWR_VOLTAGESCALING_CONFIG (PWR_REGULATOR_VOLTAGE_SCALE1);

    RCC_OscInitStruct.OscillatorType = RCC_OSCILLATORTYPE_HSE;
    RCC_OscInitStruct.HSEState = RCC_HSE_ON;
    RCC_OscInitStruct.PLL.PLLState = RCC_PLL_ON;
    RCC_OscInitStruct.PLL.PLLSource = RCC_PLLSOURCE_HSE;
    RCC_OscInitStruct.PLL.PLLM = 8;
    RCC_OscInitStruct.PLL.PLLN = 336;
    RCC_OscInitStruct.PLL.PLLP = RCC_PLLP_DIV2;
    RCC_OscInitStruct.PLL.PLLQ = 7;
    HAL_RCC_OscConfig (&RCC_OscInitStruct);

    RCC_ClkInitStruct.ClockType =
    RCC_CLOCKTYPE_SYSCLK | RCC_CLOCKTYPE_PCLK1 | RCC_CLOCKTYPE_PCLK2;
    RCC_ClkInitStruct.SYSCLKSource = RCC_SYSCLKSOURCE_PLLCLK;
    RCC_ClkInitStruct.AHBCLKDivider = RCC_SYSCLK_DIV1;
    RCC_ClkInitStruct.APB1CLKDivider = RCC_HCLK_DIV4;
    RCC_ClkInitStruct.APB2CLKDivider = RCC_HCLK_DIV2;
    HAL_RCC_ClockConfig (&RCC_ClkInitStruct, FLASH_LATENCY_5);
}

#pragma mark Member Implementations
int main (void)
{
    SystemInit ();

    SystemClock_Config ();

    HAL_Init ();

    RCC->AHB1ENR |= RCC_AHB1ENR_GPIOAEN | RCC_AHB1ENR_GPIOBEN |
                    RCC_AHB1ENR_GPIOCEN | RCC_AHB1ENR_GPIODEN |
                    RCC_AHB1ENR_GPIOEEN | RCC_AHB1ENR_GPIOFEN |
                    RCC_AHB1ENR_GPIOGEN;

    __disable_irq ();
    SDRAM_Init ();
    TFTLCD_Init ();

    __enable_irq ();
    uint32_t* ptr = (uint32_t*)SDRAM_BASE;

    *ptr = 12;

    if (*ptr == 12)
    {
        PutPixel (10, 10, 0xAAAA);
    }

    for (uint32_t x = 0; x < LCD_WIDTH; x++)
    {
        for (uint32_t y = 0; y < 50; y++)
        {
            PutPixel (x, y, 500);
        }
    }

    while (1)
        ;


    return 0;
}
