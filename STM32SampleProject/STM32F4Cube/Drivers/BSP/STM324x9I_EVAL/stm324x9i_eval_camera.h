/**
  ******************************************************************************
  * @file    stm324x9i_eval_camera.h
  * @author  MCD Application Team
  * @version V2.2.2
  * @date    13-January-2016
  * @brief   This file contains the common defines and functions prototypes for
  *          the stm324x9i_eval_camera.c driver.
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
#ifndef __STM324x9I_EVAL_CAMERA_H
#define __STM324x9I_EVAL_CAMERA_H

#ifdef __cplusplus
 extern "C" {
#endif 

/* Includes ------------------------------------------------------------------*/
/* Include Camera component Driver */
#include "../Components/ov2640/ov2640.h"   

/* Include IO Driver */
#include "stm324x9i_eval_io.h"   

/** @addtogroup BSP
  * @{
  */

/** @addtogroup STM324x9I_EVAL
  * @{
  */
    
/** @addtogroup STM324x9I_EVAL_CAMERA
  * @{
  */ 
   
/** @defgroup STM324x9I_EVAL_CAMERA_Exported_Types STM324x9I EVAL CAMERA Exported Types
  * @{
  */
  
/** 
  * @brief  Camera State structures definition  
  */  
typedef enum 
{
  CAMERA_OK       = 0x00,
  CAMERA_ERROR    = 0x01,
  CAMERA_TIMEOUT  = 0x02 
}Camera_StatusTypeDef;

#define RESOLUTION_R160x120      CAMERA_R160x120      /* QQVGA Resolution     */
#define RESOLUTION_R320x240      CAMERA_R320x240      /* QVGA Resolution      */
#define RESOLUTION_R480x272      CAMERA_R480x272      /* 480x272 Resolution   */
#define RESOLUTION_R640x480      CAMERA_R640x480      /* VGA Resolution       */  
/**
  * @}
  */ 
 
/** @defgroup STM324x9I_EVAL_CAMERA_Exported_Constants STM324x9I EVAL CAMERA Exported Constants
  * @{
  */
/**
  * @}
  */

/** @defgroup STM324x9I_EVAL_CAMERA_Exported_Functions STM324x9I EVAL CAMERA Exported Functions
  * @{
  */    
uint8_t BSP_CAMERA_Init(uint32_t Resolution);  
void    BSP_CAMERA_ContinuousStart(uint8_t *buff);
void    BSP_CAMERA_SnapshotStart(uint8_t *buff);
void    BSP_CAMERA_Suspend(void);
void    BSP_CAMERA_Resume(void);
uint8_t BSP_CAMERA_Stop(void); 
void    BSP_CAMERA_LineEventCallback(void);
void    BSP_CAMERA_VsyncEventCallback(void);
void    BSP_CAMERA_FrameEventCallback(void);
void    BSP_CAMERA_ErrorCallback(void);

/* Camera features functions prototype */
void    BSP_CAMERA_ContrastBrightnessConfig(uint32_t contrast_level, uint32_t brightness_level);
void    BSP_CAMERA_BlackWhiteConfig(uint32_t Mode);
void    BSP_CAMERA_ColorEffectConfig(uint32_t Effect);

/* To be called in DCMI_IRQHandler function */
void    BSP_CAMERA_IRQHandler(void);
/* To be called in DMA2_Stream1_IRQHandler function */
void    BSP_CAMERA_DMA_IRQHandler(void);
   
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

#endif /* __STM324x9I_EVAL_CAMERA_H */

/************************ (C) COPYRIGHT STMicroelectronics *****END OF FILE****/
