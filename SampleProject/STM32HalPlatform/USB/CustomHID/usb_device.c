/* Includes ------------------------------------------------------------------*/

#include "usb_device.h"
#include "usbd_core.h"
#include "usbd_desc.h"


/* USB Device Core handle declaration */
USBD_HandleTypeDef hUsbDeviceFS;
Buffer* DeviceDescriptor;
Buffer* ConfigDescriptor;
Buffer* ManufacturerString;
Buffer* ProductString;
Buffer* SerialNumberString;

uint8_t InEndPointAddress;
uint16_t InEndPointSize;
uint8_t OutEndPointAddress;
uint16_t OutEndPointSize;


#define USB_CUSTOM_HID_CONFIG_DESC_SIZ 41
#define USB_CUSTOM_HID_DESC_SIZ 9

#define CUSTOM_HID_DESCRIPTOR_TYPE 0x21
#define CUSTOM_HID_REPORT_DESC 0x22


USBD_HandleTypeDef* hUsbDevice_0;
extern USBD_HandleTypeDef hUsbDeviceFS;

USBD_CUSTOM_HID_ItfTypeDef USBD_CustomHID_fops_FS;

/* init function */
void MX_USB_DEVICE_Init (USBCallbacks* callbacks, Buffer* deviceDescriptor,
                         Buffer* configDescriptor, Buffer* reportDescriptor,
                         Buffer* manufacturerString, Buffer* productString,
                         Buffer* serialNumberString)
{
    DeviceDescriptor = deviceDescriptor;
    ConfigDescriptor = configDescriptor;
    ManufacturerString = manufacturerString;
    ProductString = productString;
    SerialNumberString = serialNumberString;

    /* Init Device Library,Add Supported Class and Start the library*/
    USBD_Init (&hUsbDeviceFS, &FS_Desc, DEVICE_FS);

    USBD_RegisterClass (&hUsbDeviceFS, &USBD_CUSTOM_HID);

    // Set the callbacks.
    USBD_CustomHID_fops_FS.DeInit = callbacks->DeInit;
    USBD_CustomHID_fops_FS.Init = callbacks->Init;
    USBD_CustomHID_fops_FS.OutEvent = callbacks->OutEvent;
    USBD_CustomHID_fops_FS.DataRequested = callbacks->DataRequested;

    // Set the report descriptor pointer.
    USBD_CustomHID_fops_FS.pReport = reportDescriptor->elements;

    USBD_CUSTOM_HID_RegisterInterface (&hUsbDeviceFS, &USBD_CustomHID_fops_FS);

    USBD_Start (&hUsbDeviceFS);
}


void MX_USB_DEVICE_SendReport (Buffer* buffer)
{
    USBD_CUSTOM_HID_SendReport (&hUsbDeviceFS, &buffer->elements[0],
                                buffer->length);
}


/************************ (C) COPYRIGHT STMicroelectronics *****END OF FILE****/
