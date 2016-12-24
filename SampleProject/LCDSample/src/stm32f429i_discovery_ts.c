/**
  ******************************************************************************
  * @file    stm32f429i_discovery_ts.c
  * @author  MCD Application Team
  * @version V2.0.1
  * @date    26-February-2014
  * @brief   This file provides a set of functions needed to manage Touch 
  * screen available with STMPE811 IO Expander device mounted on STM32F429I-DISCO Kit.
  ******************************************************************************
  * @attention
  *
  * <h2><center>&copy; COPYRIGHT(c) 2014 STMicroelectronics</center></h2>
  *
  * Redistribution and use in source and binary forms, with or without modification,
  * are permitted provided that the following conditions are met:
  *   1. Redistributions of source code must retain the above copyright notice,
  *      this list of conditions and the following disclaimer.
  *   2. Redistributions in binary form must reproduce the above copyright notice,
  *      this list of conditions and the following disclaimer in the documentation
  *      and/or other materials provided with the distribution.
  *   3. Neither the name of STMicroelectronics nor the names of its contributors
  *      may be used to endorse or promote products derived from this software
  *      without specific prior written permission.
  *
  * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
  * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
  * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
  * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
  * FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
  * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
  * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
  * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
  * OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
  * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
  *
  ******************************************************************************
  */ 
  
/* Includes ------------------------------------------------------------------*/
#include "stm32f429i_discovery_ts.h"
#include "stm32f429i_discovery_io.h"

/** @addtogroup BSP
  * @{
  */

/** @addtogroup STM32F429I_DISCOVERY
  * @{
  */ 
  
/** @defgroup STM32F429I_DISCOVERY_TS STM32F429I_DISCOVERY_TS
  * @{
  */ 

/* Private typedef -----------------------------------------------------------*/

/** @defgroup STM32F429I_DISCOVERY_ts_Private_Types_Definitions STM32F429I_DISCOVERY_ts_Private_Types_Definitions
  * @{
  */ 
/**
  * @}
  */ 

/* Private define ------------------------------------------------------------*/

/** @defgroup STM32F429I_DISCOVERY_ts_Private_Defines STM32F429I_DISCOVERY_ts_Private_Defines
  * @{
  */ 

/**
  * @}
  */ 

/* Private macro -------------------------------------------------------------*/

/** @defgroup STM32F429I_DISCOVERY_ts_Private_Macros STM32F429I_DISCOVERY_ts_Private_Macros
  * @{
  */ 
/**
  * @}
  */ 

/* Private variables ---------------------------------------------------------*/

/** @defgroup STM32F429I_DISCOVERY_ts_Private_Variables STM32F429I_DISCOVERY_ts_Private_Variables
  * @{
  */
static TS_DrvTypeDef     *TsDrv;
static uint16_t          TsXBoundary, TsYBoundary; 

/**
  * @}
  */

/* Private function prototypes -----------------------------------------------*/

/** @defgroup STM32F429I_DISCOVERY_ts_Private_Function_Prototypes STM32F429I_DISCOVERY_ts_Private_Function_Prototypes
  * @{
  */

/**
  * @}
  */
  
/* Private functions ---------------------------------------------------------*/

/** @defgroup STM32F429I_DISCOVERY_ts_Private_Functions STM32F429I_DISCOVERY_ts_Private_Functions
  * @{
  */

/**
  * @brief  Initializes and configures the touch screen functionalities and 
  *         configures all necessary hardware resources (GPIOs, clocks..).
  * @param  XSize: The maximum X size of the TS area on LCD
  * @param  YSize: The maximum Y size of the TS area on LCD  
  * @retval TS_OK: if all initializations are OK. Other value if error.
  */
uint8_t BSP_TS_Init(uint16_t XSize, uint16_t YSize)
{
  uint8_t ret = TS_ERROR;

  /* Initialize x and y positions boundaries */
  TsXBoundary = XSize;
  TsYBoundary = YSize;

  /* Read ID and verify if the IO expander is ready */
  if(stmpe811_ts_drv.ReadID(TS_I2C_ADDRESS) == STMPE811_ID)
  {
    /* Initialize the TS driver structure */
    TsDrv = &stmpe811_ts_drv;

    ret = TS_OK;
  }

  if(ret == TS_OK)
  {
    /* Initialize the LL TS Driver */
    TsDrv->Init(TS_I2C_ADDRESS);
    TsDrv->Start(TS_I2C_ADDRESS);
  }

  return ret;
}


/**
  * @brief  Configures and enables the touch screen interrupts.
  * @param  None
  * @retval TS_OK: if ITconfig is OK. Other value if error.
  */
uint8_t BSP_TS_ITConfig(void)
{
  uint8_t ret = TS_ERROR;

  /* Initialize the IO */
  ret = BSP_IO_Init();
  
  /* Configure TS IT line IO */
  BSP_IO_ConfigPin(TS_INT_PIN, IO_MODE_IT_FALLING_EDGE);
  
  /* Enable the TS ITs */
  TsDrv->EnableIT(TS_I2C_ADDRESS);

  return ret;  
}

/**
  * @brief  Gets the TS IT status.
  * @param  None
  * @retval Interrupt status.
  */  
uint8_t BSP_TS_ITGetStatus(void)
{
  /* Return the TS IT status */
  return (TsDrv->GetITStatus(TS_I2C_ADDRESS));
}

/**
  * @brief  Returns status and positions of the touch screen.
  * @param  TsState: Pointer to touch screen current state structure
  * @retval None.
  */
void BSP_TS_GetState(TS_StateTypeDef* TsState)
{
  static uint32_t _x = 0, _y = 0;
  uint16_t xDiff, yDiff , x , y, xr, yr;
  
  TsState->TouchDetected = TsDrv->DetectTouch(TS_I2C_ADDRESS);
  
  if(TsState->TouchDetected)
  {
    TsDrv->GetXY(TS_I2C_ADDRESS, &x, &y);
    
    /* y value first correction */
    y -= 360;  
    
    /* y value second correction */
    yr = y / 11;
    
    /* return y position value */
    if(yr <= 0)
    {
      yr = 0;
    }
    else if (yr > TsYBoundary)
    {
      yr = TsYBoundary - 1;
    }
    else
    {}
    y = yr;

    /* x value first correction */
    if(x <= 3000)
    {
    x = 3870 - x;
    }
    else
    {
     x = 3800 - x;
    }

    /* x value second correction */  
    xr = x / 15;

    /* return x position value */
    if(xr <= 0)
    {
      xr = 0;
    }
    else if (xr > TsXBoundary)
    {
      xr = TsXBoundary - 1;
    }
    else 
    {}

    x = xr;
    xDiff = x > _x? (x - _x): (_x - x);
    yDiff = y > _y? (y - _y): (_y - y); 

    if (xDiff + yDiff > 5)
    {
      _x = x;
      _y = y; 
    }
    
    /* Update the X position */
    TsState->X = _x;
    
    /* Update the Y position */  
    TsState->Y = _y;
    }
}

/**
  * @brief  Clears all touch screen interrupts.
  * @param  None
  * @retval None
  */  
void BSP_TS_ITClear(void)
{
  /* Clear all IO IT pin */
  BSP_IO_ITClear();
  
  /* Clear TS IT pending bits */
  TsDrv->ClearIT(TS_I2C_ADDRESS); 
}

/**
  * @}
  */ 

/**
  * @}
  */ 

/**
  * @}
  */ 

/**
  * @}
  */      
/************************ (C) COPYRIGHT STMicroelectronics *****END OF FILE****/
