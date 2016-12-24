/**
  ******************************************************************************
  * @file    main.c
  * @author  Pierpaolo Bagnasco
  * @version V0.1
  * @date    15-July-2014
  * @brief   This file provides an example of how to use the LCD of the
  * 		 STM32F429-DISCOVERY.
  ******************************************************************************
 */

/* Includes ------------------------------------------------------------------*/
#include "main.h"
#include "stm32f429i_discovery_lcd.h"
#include <stdlib.h>

/* Private function prototypes -----------------------------------------------*/
static void SystemClock_Config (void);

/* Private functions ---------------------------------------------------------*/

/**
 * @brief   Main program
 * @param  None
 * @retval None
 */
int main (void)
{
    HAL_Init ();

    /* Configure the system clock */
    SystemClock_Config ();

    BSP_LCD_Init ();

    BSP_LCD_LayerDefaultInit (0, (uint32_t)LCD_FRAME_BUFFER);
    BSP_LCD_SetLayerVisible (0, ENABLE);
    BSP_LCD_LayerDefaultInit (1, (uint32_t)LCD_FRAME_BUFFER + 76800);
    BSP_LCD_SelectLayer (1);
    BSP_LCD_SetLayerVisible (1, DISABLE);


    BSP_LCD_SelectLayer (0);
    BSP_LCD_Clear (LCD_COLOR_WHITE);

    BSP_LCD_DrawPixel (10, 50, 0);
    BSP_LCD_DrawCircle (50, 100, 50);

    BSP_LCD_SelectLayer (1);
    BSP_LCD_SetBackColor (LCD_COLOR_TRANSPARENT);
    BSP_LCD_SetTextColor (LCD_COLOR_BLACK);
    BSP_LCD_DisplayOn ();

    BSP_LCD_DisplayStringAtLineMode (1, (uint8_t*)"Test display", CENTER_MODE);
    HAL_Delay (500);
    BSP_LCD_DisplayStringAtLineMode (5, (uint8_t*)"ALL CAPS", CENTER_MODE);
    HAL_Delay (500);
    BSP_LCD_DisplayStringAtLineMode (8, (uint8_t*)"all lowercase", CENTER_MODE);

    while (1)
    {
        BSP_LCD_DisplayStringAtLineMode (10, (uint8_t*)"all lowercase",
                                         CENTER_MODE);
        HAL_Delay (100);
    }
}

/**
 * @brief  System Clock Configuration
 *         The system Clock is configured as follow :
 *            System Clock source            = PLL (HSE)
 *            SYSCLK(Hz)                     = 180000000
 *            HCLK(Hz)                       = 180000000
 *            AHB Prescaler                  = 1
 *            APB1 Prescaler                 = 4
 *            APB2 Prescaler                 = 2
 *            HSE Frequency(Hz)              = 8000000
 *            PLL_M                          = 8
 *            PLL_N                          = 360
 *            PLL_P                          = 2
 *            PLL_Q                          = 7
 *            VDD(V)                         = 3.3
 *            Main regulator output voltage  = Scale1 mode
 *            Flash Latency(WS)              = 5
 *         The LTDC Clock is configured as follow :
 *            PLLSAIN                        = 192
 *            PLLSAIR                        = 4
 *            PLLSAIDivR                     = 8
 * @param  None
 * @retval None
 *
 * COPYRIGHT(c) 2014 STMicroelectronics
 */
static void SystemClock_Config (void)
{
    RCC_ClkInitTypeDef RCC_ClkInitStruct;
    RCC_OscInitTypeDef RCC_OscInitStruct;
    RCC_PeriphCLKInitTypeDef PeriphClkInitStruct;

    /* Enable Power Control clock */
    __PWR_CLK_ENABLE ();

    /* The voltage scaling allows optimizing the power consumption when the
     device is
     clocked below the maximum system frequency, to update the voltage scaling
     value
     regarding system frequency refer to product datasheet.  */
    __HAL_PWR_VOLTAGESCALING_CONFIG (PWR_REGULATOR_VOLTAGE_SCALE1);

    /*##-1- System Clock Configuration
     * #########################################*/
    /* Enable HSE Oscillator and activate PLL with HSE as source */
    RCC_OscInitStruct.OscillatorType = RCC_OSCILLATORTYPE_HSE;
    RCC_OscInitStruct.HSEState = RCC_HSE_ON;
    RCC_OscInitStruct.PLL.PLLState = RCC_PLL_ON;
    RCC_OscInitStruct.PLL.PLLSource = RCC_PLLSOURCE_HSE;
    RCC_OscInitStruct.PLL.PLLM = 8;
    RCC_OscInitStruct.PLL.PLLN = 360;
    RCC_OscInitStruct.PLL.PLLP = RCC_PLLP_DIV2;
    RCC_OscInitStruct.PLL.PLLQ = 7;
    HAL_RCC_OscConfig (&RCC_OscInitStruct);

    HAL_PWREx_ActivateOverDrive ();

    /* Select PLL as system clock source and configure the HCLK, PCLK1 and PCLK2
     clocks dividers */
    RCC_ClkInitStruct.ClockType = (RCC_CLOCKTYPE_SYSCLK | RCC_CLOCKTYPE_HCLK |
                                   RCC_CLOCKTYPE_PCLK1 | RCC_CLOCKTYPE_PCLK2);
    RCC_ClkInitStruct.SYSCLKSource = RCC_SYSCLKSOURCE_PLLCLK;
    RCC_ClkInitStruct.AHBCLKDivider = RCC_SYSCLK_DIV1;
    RCC_ClkInitStruct.APB1CLKDivider = RCC_HCLK_DIV4;
    RCC_ClkInitStruct.APB2CLKDivider = RCC_HCLK_DIV2;
    HAL_RCC_ClockConfig (&RCC_ClkInitStruct, FLASH_LATENCY_5);

    /*##-2- LTDC Clock Configuration
     * ###########################################*/
    /* LCD clock configuration */
    /* PLLSAI_VCO Input = HSE_VALUE/PLL_M = 1 Mhz */
    /* PLLSAI_VCO Output = PLLSAI_VCO Input * PLLSAIN = 192 Mhz */
    /* PLLLCDCLK = PLLSAI_VCO Output/PLLSAIR = 192/4 = 48 Mhz */
    /* LTDC clock frequency = PLLLCDCLK / RCC_PLLSAIDIVR_8 = 48/8 = 6 Mhz */
    PeriphClkInitStruct.PeriphClockSelection = RCC_PERIPHCLK_LTDC;
    PeriphClkInitStruct.PLLSAI.PLLSAIN = 192;
    PeriphClkInitStruct.PLLSAI.PLLSAIR = 4;
    PeriphClkInitStruct.PLLSAIDivR = RCC_PLLSAIDIVR_8;
    HAL_RCCEx_PeriphCLKConfig (&PeriphClkInitStruct);
}
