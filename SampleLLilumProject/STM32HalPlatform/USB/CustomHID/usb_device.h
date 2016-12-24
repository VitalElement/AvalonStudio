/**
  ******************************************************************************
  * @file           : USB_DEVICE
  * @date           : 24/04/2015 11:59:31
  * @version        : v1.0_Cube
  * @brief          : Header for usb_device file.
  ******************************************************************************
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
/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __usb_device_H
#define __usb_device_H
#ifdef __cplusplus
extern "C" {
#endif

/* Includes ------------------------------------------------------------------*/
#include "stm32f4xx.h"
#include "stm32f4xx_hal.h"
#include "usbd_def.h"
#include "usbd_customhid.h"
#include "CBuffer.h"

typedef struct
{
    int8_t (*Init)(void);
    int8_t (*DeInit)(void);
    int8_t (*OutEvent)(uint8_t reportId, Buffer* dataBuffer);
    Buffer* (*DataRequested)(void);
} USBCallbacks;

extern Buffer* DeviceDescriptor;
extern Buffer* ConfigDescriptor;
extern Buffer* ManufacturerString;
extern Buffer* ProductString;
extern Buffer* SerialNumberString;

extern uint8_t InEndPointAddress;
extern uint16_t InEndPointSize;
extern uint8_t OutEndPointAddress;
extern uint16_t OutEndPointSize;

extern USBD_CUSTOM_HID_ItfTypeDef USBD_CustomHID_fops_FS;
extern USBD_HandleTypeDef hUsbDeviceFS;

/* USB_Device init function */
void MX_USB_DEVICE_Init (USBCallbacks* callbacks, Buffer* deviceDescriptor,
                         Buffer* configDescriptor, Buffer* reportDescriptor,
                         Buffer* manufacturerString, Buffer* productString,
                         Buffer* serialNumberString);

void MX_USB_DEVICE_SendReport (Buffer* data);

#ifdef __cplusplus
}
#endif
#endif /*__usb_device_H */

/**
  * @}
  */

/**
  * @}
  */

/************************ (C) COPYRIGHT STMicroelectronics *****END OF FILE****/
