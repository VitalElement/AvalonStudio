/**
  ******************************************************************************
  * @file    stm32469i_eval_qspi.h
  * @author  MCD Application Team
  * @version V1.0.2
  * @date    12-January-2016
  * @brief   This file contains the common defines and functions prototypes for
  *          the stm32469i_eval_qspi.c driver.
  ******************************************************************************
  * @attention
  *
  * <h2><center>&copy; COPYRIGHT(c) 2015 STMicroelectronics</center></h2>
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

/** @addtogroup BSP
  * @{
  */ 

/** @addtogroup STM32469I_EVAL
  * @{
  */
    
/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __STM32469I_EVAL_QSPI_H
#define __STM32469I_EVAL_QSPI_H

#ifdef __cplusplus
 extern "C" {
#endif 

/* Includes ------------------------------------------------------------------*/
#include "stm32f4xx_hal.h"
#include "../Components/s25fl512s/s25fl512s.h"
#include "../Components/n25q512a/n25q512a.h"


/** @addtogroup STM32469I_EVAL_QSPI
  * @{
  */    

  
/* Exported constants --------------------------------------------------------*/ 
/** @defgroup STM32446E_EVAL_QSPI_Exported_Constants STM32469I EVAL QSPI Exported Constants
  * @{
  */
/* QSPI Error codes */
#define QSPI_OK            ((uint8_t)0x00)
#define QSPI_ERROR         ((uint8_t)0x01)
#define QSPI_BUSY          ((uint8_t)0x02)
#define QSPI_NOT_SUPPORTED ((uint8_t)0x04)
#define QSPI_SUSPENDED     ((uint8_t)0x08)
#define QSPI_PROTECTED     ((uint8_t)0x10)

#define QSPI_S25FL512S     ((uint8_t)0x20)
#define QSPI_N25Q512A      ((uint8_t)0x40)

/* Definition for QSPI clock resources */
#define QSPI_CLK_ENABLE()             __HAL_RCC_QSPI_CLK_ENABLE()
#define QSPI_CLK_DISABLE()            __HAL_RCC_QSPI_CLK_DISABLE()
#define QSPI_CS_GPIO_CLK_ENABLE()     __HAL_RCC_GPIOB_CLK_ENABLE()
#define QSPI_CS_GPIO_CLK_DISABLE()  __HAL_RCC_GPIOB_CLK_DISABLE()
#define QSPI_DX_CLK_GPIO_CLK_ENABLE() __HAL_RCC_GPIOF_CLK_ENABLE()
#define QSPI_DX_CLK_GPIO_CLK_DISABLE()  __HAL_RCC_GPIOF_CLK_DISABLE()

#define QSPI_FORCE_RESET()            __HAL_RCC_QSPI_FORCE_RESET()
#define QSPI_RELEASE_RESET()          __HAL_RCC_QSPI_RELEASE_RESET()

/* Definition for QSPI Pins */
#define QSPI_CS_PIN                GPIO_PIN_6
#define QSPI_CS_GPIO_PORT          GPIOB
#define QSPI_CLK_PIN               GPIO_PIN_10
#define QSPI_CLK_GPIO_PORT         GPIOF
#define QSPI_D0_PIN                GPIO_PIN_8
#define QSPI_D1_PIN                GPIO_PIN_9
#define QSPI_D2_PIN                GPIO_PIN_7
#define QSPI_D3_PIN                GPIO_PIN_6
#define QSPI_DX_GPIO_PORT          GPIOF


/**
  * @}
  */

/* Exported types ------------------------------------------------------------*/
/** @defgroup STM32446E_EVAL_QSPI_Exported_Types STM32469I EVAL QSPI Exported Types
  * @{
  */
/**
 * @brief QSPI Info
 * */
typedef struct {
  uint32_t FlashSize;          /*!< Size of the flash                         */
  uint32_t EraseSectorSize;    /*!< Size of sectors for the erase operation   */
  uint32_t EraseSectorsNumber; /*!< Number of sectors for the erase operation */
  uint32_t ProgPageSize;       /*!< Size of pages for the program operation   */
  uint32_t ProgPagesNumber;    /*!< Number of pages for the program operation */
  uint32_t DummyCyclesRead;    /*!< Number of dummy read cycles               */
  uint8_t  ManufID;            /*!< Manufacturor value                        */
  uint32_t SectorEraseMaxTime; /*!< Sector Erase max value                    */
  uint32_t BulkEraseMaxTime;   /*!< Bulk Erase Max Time                       */
} QSPI_InfoTypeDef;

/**
  * @}
  */

  
/* Exported functions --------------------------------------------------------*/
/** @defgroup STM32469I_EVAL_QSPI_Exported_Functions STM32469I EVAL QSPI Exported Functions
  * @{
  */  
uint8_t BSP_QSPI_Init       (void);
uint8_t BSP_QSPI_DeInit     (void);
uint8_t BSP_QSPI_Read       (uint8_t* pData, uint32_t ReadAddr, uint32_t Size);
uint8_t BSP_QSPI_Write      (uint8_t* pData, uint32_t WriteAddr, uint32_t Size);
uint8_t BSP_QSPI_Erase_Block(uint32_t BlockAddress);
uint8_t BSP_QSPI_Erase_Chip (void);
uint8_t BSP_QSPI_GetStatus  (void);
uint8_t BSP_QSPI_GetInfo    (QSPI_InfoTypeDef* pInfo);
uint8_t BSP_QSPI_EnableMemoryMappedMode(void);
/* BSP Aliased function maintained for legacy purpose */
#define BSP_QSPI_MemoryMappedMode      BSP_QSPI_EnableMemoryMappedMode

/* These function can be modified in case the current settings (e.g. DMA stream)
   need to be changed for specific application needs */
void BSP_QSPI_MspInit(QSPI_HandleTypeDef *hqspi, void *Params);
void BSP_QSPI_MspDeInit(QSPI_HandleTypeDef *hqspi, void *Params);

/**
  * @}
  */ 

/**
  * @}
  */ 

#ifdef __cplusplus
}
#endif

#endif /* __STM32446E_EVAL_QSPI_H */
/**
  * @}
  */ 

/**
  * @}
  */ 

/************************ (C) COPYRIGHT STMicroelectronics *****END OF FILE****/
