/**
  ******************************************************************************
  * @file    stm32f429i_discovery_ts.h
  * @author  MCD Application Team
  * @version V2.0.1
  * @date    26-February-2014
  * @brief   This file contains all the functions prototypes for the
  *          stm32f429i_discovery_ts.c driver.
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

/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __STM32F429I_DISCOVERY_TS_H
#define __STM32F429I_DISCOVERY_TS_H

#ifdef __cplusplus
 extern "C" {
#endif   
   
/* Includes ------------------------------------------------------------------*/
#include "stm32f429i_discovery.h"
/* Include TouchScreen component driver */
//#include "..\BSP\Components\stmpe811\stmpe811.h"
   
/** @addtogroup BSP
  * @{
  */

/** @addtogroup STM32F429I_DISCOVERY
  * @{
  */ 

/** @defgroup STM32F429I_DISCOVERY_TS STM32F429I_DISCOVERY_TS
  * @{
  */

/* Exported types ------------------------------------------------------------*/

/** @defgroup STM32F429I_DISCOVERY_TS_Exported_Types STM32F429I_DISCOVERY_TS_Exported_Types
  * @{
  */ 
typedef struct
{
  uint16_t TouchDetected;
  uint16_t X;
  uint16_t Y;
  uint16_t Z;

}TS_StateTypeDef;

/**
  * @}
  */

/* Exported constants --------------------------------------------------------*/

/** @defgroup STM32F429I_DISCOVERY_TS_Exported_Constants STM32F429I_DISCOVERY_TS_Exported_Constants
  * @{
  */ 
#define TS_SWAP_NONE                    0x00
#define TS_SWAP_X                       0x01
#define TS_SWAP_Y                       0x02
#define TS_SWAP_XY                      0x04

typedef enum 
{
  TS_OK       = 0x00,
  TS_ERROR    = 0x01,
  TS_TIMEOUT  = 0x02

}TS_StatusTypeDef;

/* Interrupt sources pins definition */
#define TS_INT_PIN                      0x0010

/**
  * @}
  */

/* Exported macro ------------------------------------------------------------*/

/** @defgroup STM32F429I_DISCOVERY_TS_Exported_Macros STM32F429I_DISCOVERY_TS_Exported_Macros
  * @{
  */
/**
  * @}
  */
/* Exported functions --------------------------------------------------------*/

/** @defgroup STM32F429I_DISCOVERY_TS_Exported_Functions STM32F429I_DISCOVERY_TS_Exported_Functions
  * @{
  */

uint8_t     BSP_TS_Init(uint16_t XSize, uint16_t YSize);
void        BSP_TS_GetState(TS_StateTypeDef *TsState);
uint8_t     BSP_TS_ITConfig(void);
uint8_t     BSP_TS_ITGetStatus(void);
void        BSP_TS_ITClear(void);

#ifdef __cplusplus
}
#endif
#endif /* __STM32F429I_DISCOVERY_TS_H */

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
