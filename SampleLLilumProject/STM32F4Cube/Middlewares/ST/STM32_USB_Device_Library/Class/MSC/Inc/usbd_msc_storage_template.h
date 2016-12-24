/**
  ******************************************************************************
  * @file    usbd_msc_storage.h
  * @author  MCD Application Team
  * @version V2.4.1
  * @date    19-June-2015
  * @brief   Header file for the usbd_msc_storage.c file
  ******************************************************************************
  * @attention
  *
  * <h2><center>&copy; COPYRIGHT 2015 STMicroelectronics</center></h2>
  *
  * Licensed under MCD-ST Liberty SW License Agreement V2, (the "License");
  * You may not use this file except in compliance with the License.
  * You may obtain a copy of the License at:
  *
  *        http://www.st.com/software_license_agreement_liberty_v2
  *
  * Unless required by applicable law or agreed to in writing, software 
  * distributed under the License is distributed on an "AS IS" BASIS, 
  * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
  * See the License for the specific language governing permissions and
  * limitations under the License.
  *
  ******************************************************************************
  */ 

/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __USBD_MSC_STORAGE_H
#define __USBD_MSC_STORAGE_H

#ifdef __cplusplus
 extern "C" {
#endif

/* Includes ------------------------------------------------------------------*/
#include "usbd_msc.h"

/** @addtogroup STM32_USB_DEVICE_LIBRARY
  * @{
  */
  
/** @defgroup USBD_STORAGE
  * @brief header file for the usbd_msc_storage.c file
  * @{
  */ 

/** @defgroup USBD_STORAGE_Exported_Defines
  * @{
  */ 
/**
  * @}
  */ 


/** @defgroup USBD_STORAGE_Exported_Types
  * @{
  */


/**
  * @}
  */ 



/** @defgroup USBD_STORAGE_Exported_Macros
  * @{
  */ 

/**
  * @}
  */ 

/** @defgroup USBD_STORAGE_Exported_Variables
  * @{
  */ 
extern USBD_StorageTypeDef  USBD_MSC_Template_fops;
/**
  * @}
  */ 

/** @defgroup USBD_STORAGE_Exported_FunctionsPrototype
  * @{
  */ 


/**
  * @}
  */ 

#ifdef __cplusplus
}
#endif

#endif /* __USBD_MSC_STORAGE_H */

/**
  * @}
  */ 

/**
* @}
*/ 
/************************ (C) COPYRIGHT STMicroelectronics *****END OF FILE****/
