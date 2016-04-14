/**
  ******************************************************************************
  * @file    stm324xg_eval_sd.h
  * @author  MCD Application Team
  * @version V2.2.1
  * @date    15-January-2016
  * @brief   This file contains the common defines and functions prototypes for
  *          the stm324xg_eval_sd.c driver.
  ******************************************************************************
  * @attention
  *
  * <h2><center>&copy; COPYRIGHT(c) 2016 STMicroelectronics</center></h2>
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
#ifndef __STM324xG_EVAL_SD_H
#define __STM324xG_EVAL_SD_H

#ifdef __cplusplus
 extern "C" {
#endif 

/* Includes ------------------------------------------------------------------*/
#include "stm32f4xx_hal.h"

/** @addtogroup BSP
  * @{
  */ 

/** @addtogroup STM324xG_EVAL
  * @{
  */
    
/** @defgroup STM324xG_EVAL_SD STM324xG EVAL SD
  * @{
  */    

/** @defgroup STM324xG_EVAL_SD_Exported_Types STM324xG EVAL SD Exported Types
  * @{
  */

/** 
  * @brief SD Card information structure 
  */   
#define SD_CardInfo HAL_SD_CardInfoTypedef
   
/** 
  * @brief  SD status structure definition  
  */     
#define MSD_OK         0x00
#define MSD_ERROR      0x01

/**
  * @}
  */

/** @defgroup STM324xG_EVAL_SD_Exported_Constants STM324xG EVAL SD Exported Constants
  * @{
  */ 
#define SD_DETECT_PIN                    GPIO_PIN_13
#define SD_DETECT_GPIO_PORT              GPIOH
#define __SD_DETECT_GPIO_CLK_ENABLE()    __GPIOH_CLK_ENABLE()
#define SD_DETECT_IRQn                   EXTI15_10_IRQn

#define SD_DATATIMEOUT           ((uint32_t)100000000)

#define SD_PRESENT               ((uint8_t)0x01)
#define SD_NOT_PRESENT           ((uint8_t)0x00)
   
/* DMA definitions for SD DMA transfer */
#define __DMAx_TxRx_CLK_ENABLE            __DMA2_CLK_ENABLE
#define SD_DMAx_Tx_CHANNEL                DMA_CHANNEL_4
#define SD_DMAx_Rx_CHANNEL                DMA_CHANNEL_4
#define SD_DMAx_Tx_STREAM                 DMA2_Stream6  
#define SD_DMAx_Rx_STREAM                 DMA2_Stream3  
#define SD_DMAx_Tx_IRQn                   DMA2_Stream6_IRQn
#define SD_DMAx_Rx_IRQn                   DMA2_Stream3_IRQn
#define SD_DMAx_Tx_IRQHandler             DMA2_Stream6_IRQHandler
#define SD_DMAx_Rx_IRQHandler             DMA2_Stream3_IRQHandler
#define SD_DetectIRQHandler()             HAL_GPIO_EXTI_IRQHandler(GPIO_PIN_13)

/**
  * @}
  */
  
/** @defgroup STM324xG_EVAL_SD_Exported_Macro STM324xG EVAL SD Exported Macro
  * @{
  */ 

/**
  * @}
  */
  
/** @defgroup STM324xG_EVAL_SD_Exported_Functions STM324xG EVAL SD Exported Functions
  * @{
  */  
uint8_t BSP_SD_Init(void);
uint8_t BSP_SD_ITConfig(void);
void    BSP_SD_DetectIT(void);
void    BSP_SD_DetectCallback(void);
uint8_t BSP_SD_ReadBlocks(uint32_t *pData, uint64_t ReadAddr, uint32_t BlockSize, uint32_t NumOfBlocks);
uint8_t BSP_SD_WriteBlocks(uint32_t *pData, uint64_t WriteAddr, uint32_t BlockSize, uint32_t NumOfBlocks);
uint8_t BSP_SD_ReadBlocks_DMA(uint32_t *pData, uint64_t ReadAddr, uint32_t BlockSize, uint32_t NumOfBlocks);
uint8_t BSP_SD_WriteBlocks_DMA(uint32_t *pData, uint64_t WriteAddr, uint32_t BlockSize, uint32_t NumOfBlocks);
uint8_t BSP_SD_Erase(uint64_t StartAddr, uint64_t EndAddr);
void    BSP_SD_IRQHandler(void);
void    BSP_SD_DMA_Tx_IRQHandler(void);
void    BSP_SD_DMA_Rx_IRQHandler(void);
HAL_SD_TransferStateTypedef BSP_SD_GetStatus(void);
void    BSP_SD_GetCardInfo(HAL_SD_CardInfoTypedef *CardInfo);
uint8_t BSP_SD_IsDetected(void);   

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

#ifdef __cplusplus
}
#endif

#endif /* __STM324xG_EVAL_SD_H */

/************************ (C) COPYRIGHT STMicroelectronics *****END OF FILE****/
