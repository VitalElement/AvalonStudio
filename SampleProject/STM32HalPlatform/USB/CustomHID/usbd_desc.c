/**
  ******************************************************************************
  * @file           : usbd_desc.c
  * @date           : 24/04/2015 11:59:31
  * @version        : v1.0_Cube
  * @brief          : This file implements the USB Device descriptors
  ******************************************************************************
  *
  * COPYRIGHT(c) 2015 STMicroelectronics
  *
  * Redistribution and use in source and binary forms, with or without
  *modification,
  * are permitted provided that the following conditions are met:
  * 1. Redistributions of source code must retain the above copyright notice,
  * this list of conditions and the following disclaimer.
  * 2. Redistributions in binary form must reproduce the above copyright notice,
  * this list of conditions and the following disclaimer in the documentation
  * and/or other materials provided with the distribution.
  * 3. Neither the name of STMicroelectronics nor the names of its contributors
  * may be used to endorse or promote products derived from this software
  * without specific prior written permission.
  *
  * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
  * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
  * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
  *ARE
  * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
  * FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
  * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
  * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
  * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
  *LIABILITY,
  * OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE
  *USE
  * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
  *
  ******************************************************************************
*/

/* Includes ------------------------------------------------------------------*/
#include "usbd_core.h"
#include "usbd_desc.h"
#include "usbd_conf.h"
#include "usb_device.h"
/** @addtogroup STM32_USB_OTG_DEVICE_LIBRARY
  * @{
  */

/** @defgroup USBD_DESC
  * @brief USBD descriptors module
  * @{
  */

/** @defgroup USBD_DESC_Private_TypesDefinitions
  * @{
  */
/**
  * @}
  */

/** @defgroup USBD_DESC_Private_Defines
  * @{
  */

#define USBD_LANGID_STRING 1033
#define USBD_CONFIGURATION_STRING_FS "Custom HID Config"
#define USBD_INTERFACE_STRING_FS "Custom HID Interface"
#define USB_SIZ_BOS_DESC 0x0C

/**
  * @}
  */

/** @defgroup USBD_DESC_Private_Macros
  * @{
  */
/**
  * @}
  */

/** @defgroup USBD_DESC_Private_Variables
  * @{
  */
uint8_t* USBD_FS_DeviceDescriptor (USBD_SpeedTypeDef speed, uint16_t* length);
uint8_t* USBD_FS_LangIDStrDescriptor (USBD_SpeedTypeDef speed,
                                      uint16_t* length);
uint8_t* USBD_FS_ManufacturerStrDescriptor (USBD_SpeedTypeDef speed,
                                            uint16_t* length);
uint8_t* USBD_FS_ProductStrDescriptor (USBD_SpeedTypeDef speed,
                                       uint16_t* length);
uint8_t* USBD_FS_SerialStrDescriptor (USBD_SpeedTypeDef speed,
                                      uint16_t* length);
uint8_t* USBD_FS_ConfigStrDescriptor (USBD_SpeedTypeDef speed,
                                      uint16_t* length);
uint8_t* USBD_FS_InterfaceStrDescriptor (USBD_SpeedTypeDef speed,
                                         uint16_t* length);

#ifdef USB_SUPPORT_USER_STRING_DESC
uint8_t* USBD_FS_USRStringDesc (USBD_SpeedTypeDef speed, uint8_t idx,
                                uint16_t* length);
#endif /* USB_SUPPORT_USER_STRING_DESC */

#if (USBD_LPM_ENABLED == 1)
uint8_t* USBD_FS_USR_BOSDescriptor (USBD_SpeedTypeDef speed, uint16_t* length);
#endif

USBD_DescriptorsTypeDef FS_Desc = {
    USBD_FS_DeviceDescriptor,          USBD_FS_LangIDStrDescriptor,
    USBD_FS_ManufacturerStrDescriptor, USBD_FS_ProductStrDescriptor,
    USBD_FS_SerialStrDescriptor,       USBD_FS_ConfigStrDescriptor,
    USBD_FS_InterfaceStrDescriptor,
#if (USBD_LPM_ENABLED == 1)
    USBD_FS_USR_BOSDescriptor,
#endif
};


/* BOS descriptor */
#if (USBD_LPM_ENABLED == 1)
#if defined(__ICCARM__) /*!< IAR Compiler */
#pragma data_alignment = 4
#endif
__ALIGN_BEGIN uint8_t USBD_FS_BOSDesc[USB_SIZ_BOS_DESC] __ALIGN_END = {
    0x5, USB_DESC_TYPE_BOS, 0xC, 0x0, 0x1,   /* 1 device capability */
                                             /* device capability*/
    0x7, USB_DEVICE_CAPABITY_TYPE, 0x2, 0x2, /*LPM capability bit set */
    0x0, 0x0, 0x0
};
#endif

#if defined(__ICCARM__) /*!< IAR Compiler */
#pragma data_alignment = 4
#endif

/* USB Standard Device Descriptor */
__ALIGN_BEGIN uint8_t USBD_LangIDDesc[USB_LEN_LANGID_STR_DESC] __ALIGN_END = {
    USB_LEN_LANGID_STR_DESC, USB_DESC_TYPE_STRING, LOBYTE (USBD_LANGID_STRING),
    HIBYTE (USBD_LANGID_STRING),
};


#if defined(__ICCARM__) /*!< IAR Compiler */
#pragma data_alignment = 4
#endif
__ALIGN_BEGIN uint8_t USBD_StrDesc[USBD_MAX_STR_DESC_SIZ] __ALIGN_END;
/**
  * @}
  */

/** @defgroup USBD_DESC_Private_FunctionPrototypes
  * @{
  */
/**
  * @}
  */

/** @defgroup USBD_DESC_Private_Functions
  * @{
  */

/**
* @brief  USBD_FS_DeviceDescriptor
*         return the device descriptor
* @param  speed : current device speed
* @param  length : pointer to data length variable
* @retval pointer to descriptor buffer
*/
uint8_t* USBD_FS_DeviceDescriptor (USBD_SpeedTypeDef speed, uint16_t* length)
{
    *length = DeviceDescriptor->length;

    return DeviceDescriptor->elements;
}

/**
* @brief  USBD_FS_LangIDStrDescriptor
*         return the LangID string descriptor
* @param  speed : current device speed
* @param  length : pointer to data length variable
* @retval pointer to descriptor buffer
*/
uint8_t* USBD_FS_LangIDStrDescriptor (USBD_SpeedTypeDef speed, uint16_t* length)
{
    *length = sizeof (USBD_LangIDDesc);
    return USBD_LangIDDesc;
}

/**
* @brief  USBD_FS_ProductStrDescriptor
*         return the product string descriptor
* @param  speed : current device speed
* @param  length : pointer to data length variable
* @retval pointer to descriptor buffer
*/
uint8_t* USBD_FS_ProductStrDescriptor (USBD_SpeedTypeDef speed,
                                       uint16_t* length)
{
    USBD_GetString (ProductString->elements, USBD_StrDesc, length);
    
    return USBD_StrDesc;
}

/**
* @brief  USBD_FS_ManufacturerStrDescriptor
*         return the manufacturer string descriptor
* @param  speed : current device speed
* @param  length : pointer to data length variable
* @retval pointer to descriptor buffer
*/
uint8_t* USBD_FS_ManufacturerStrDescriptor (USBD_SpeedTypeDef speed,
                                            uint16_t* length)
{
    USBD_GetString (ManufacturerString->elements, USBD_StrDesc, length);
    return USBD_StrDesc;
}

/**
* @brief  USBD_FS_SerialStrDescriptor
*         return the serial number string descriptor
* @param  speed : current device speed
* @param  length : pointer to data length variable
* @retval pointer to descriptor buffer
*/
uint8_t* USBD_FS_SerialStrDescriptor (USBD_SpeedTypeDef speed, uint16_t* length)
{
    USBD_GetString (SerialNumberString->elements, USBD_StrDesc, length);
    return USBD_StrDesc;
}

/**
* @brief  USBD_FS_ConfigStrDescriptor
*         return the configuration string descriptor
* @param  speed : current device speed
* @param  length : pointer to data length variable
* @retval pointer to descriptor buffer
*/
uint8_t* USBD_FS_ConfigStrDescriptor (USBD_SpeedTypeDef speed, uint16_t* length)
{
    if (speed == USBD_SPEED_HIGH)
    {
        USBD_GetString ((uint8_t*)USBD_CONFIGURATION_STRING_FS, USBD_StrDesc, length);
    }
    else
    {
        USBD_GetString ((uint8_t*)USBD_CONFIGURATION_STRING_FS, USBD_StrDesc, length);
    }
    return USBD_StrDesc;
}

/**
* @brief  USBD_HS_InterfaceStrDescriptor
*         return the interface string descriptor
* @param  speed : current device speed
* @param  length : pointer to data length variable
* @retval pointer to descriptor buffer
*/
uint8_t* USBD_FS_InterfaceStrDescriptor (USBD_SpeedTypeDef speed,
                                         uint16_t* length)
{
    if (speed == 0)
    {
        USBD_GetString ((uint8_t*)USBD_INTERFACE_STRING_FS, USBD_StrDesc, length);
    }
    else
    {
        USBD_GetString ((uint8_t*)USBD_INTERFACE_STRING_FS, USBD_StrDesc, length);
    }
    return USBD_StrDesc;
}

#if (USBD_LPM_ENABLED == 1)
/**
  * @brief  USBD_FS_USR_BOSDescriptor
  *         return the BOS descriptor
  * @param  speed : current device speed
  * @param  length : pointer to data length variable
  * @retval pointer to descriptor buffer
  */
uint8_t* USBD_FS_USR_BOSDescriptor (USBD_SpeedTypeDef speed, uint16_t* length)
{
    *length = sizeof (USBD_FS_BOSDesc);
    return (uint8_t*)USBD_FS_BOSDesc;
}
#endif
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
