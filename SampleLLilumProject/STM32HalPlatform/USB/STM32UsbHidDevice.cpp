/******************************************************************************
*       Description:
*
*       Author:
*         Date: 27 April 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#pragma mark Includes
#include "STM32UsbHidDevice.h"
#include "usb_device.h"
#include "usbd_customhid.h"

#pragma mark Definitions and Constants


#pragma mark Static Data
static STM32UsbHidDevice* hidDevice;


#pragma mark Static Functions


#pragma mark Member Implementations
STM32UsbHidDevice::STM32UsbHidDevice ()
{
}

STM32UsbHidDevice::~STM32UsbHidDevice ()
{
}

void STM32UsbHidDevice::Initialise (
Buffer* deviceDescriptor, Buffer* configDescriptor, Buffer* reportDescriptor,
Buffer* manufacturerString, Buffer* productString, Buffer* serialString,
uint8_t inEndpointAddress, uint16_t inEndpointSize, uint8_t outEndpointAddress,
uint16_t outEndpointSize)
{
    InEndPointAddress = inEndpointAddress;
    OutEndPointAddress = outEndpointAddress;
    InEndPointSize = inEndpointSize;
    OutEndPointSize = outEndpointSize;

    this->deviceDescriptor = *deviceDescriptor;
    this->configDescriptor = *configDescriptor;
    this->reportDescriptor = *reportDescriptor;
    this->manufacturerString = *manufacturerString;
    this->productString = *productString;
    this->serialString = *serialString;
}

void STM32UsbHidDevice::WriteFeatureReport (uint8_t reportId, Buffer* buffer)
{
}

Buffer* STM32UsbHidDevice::ReadFeatureReport (uint8_t reportId)
{
    return nullptr;
}

void STM32UsbHidDevice::SendReport (Buffer* report)
{
    MX_USB_DEVICE_SendReport (report);
}

bool STM32UsbHidDevice::IsConnected ()
{
    return hUsbDeviceFS.dev_state == USBD_STATE_CONFIGURED;
}

static Buffer* OnUsbDataRequested (void)
{
    static UsbTransactionEventArgs eventArgs;
    static Buffer dataBuffer;

    eventArgs.data = &dataBuffer;

    if (hidDevice->OnUsbInput != nullptr)
    {
        hidDevice->OnUsbInput (hidDevice, eventArgs);
    }

    return eventArgs.data;
}

static int8_t OnUsbHidInit (void)
{
    return 0;
}

static int8_t OnUsbHidDeinit (void)
{
    return 0;
}

static int8_t OnUsbHidOutEvent (uint8_t reportId, Buffer* dataBuffer)
{
    UsbTransactionEventArgs eventArgs;
    eventArgs.reportId = reportId;
    eventArgs.data = dataBuffer;

    if (hidDevice->OnUsbOutput != nullptr)
    {
        hidDevice->OnUsbOutput (hidDevice, eventArgs);
    }

    return 0;
}

void STM32UsbHidDevice::InitialiseStack ()
{
    USBCallbacks callBacks;
    hidDevice = this;

    callBacks.Init = OnUsbHidInit;
    callBacks.DeInit = OnUsbHidDeinit;
    callBacks.OutEvent = OnUsbHidOutEvent;
    callBacks.DataRequested = OnUsbDataRequested;

    MX_USB_DEVICE_Init (&callBacks, &this->deviceDescriptor,
                        &this->configDescriptor, &this->reportDescriptor,
                        &this->manufacturerString, &this->productString,
                        &this->serialString);

    HAL_NVIC_SetPriority (OTG_FS_IRQn, 7, 0);
    HAL_NVIC_SetPriority (OTG_FS_WKUP_IRQn, 7, 0);
    HAL_NVIC_SetPriority (OTG_HS_EP1_IN_IRQn, 7, 0);
    HAL_NVIC_SetPriority (OTG_HS_EP1_OUT_IRQn, 7, 0);
    HAL_NVIC_SetPriority (OTG_HS_IRQn, 7, 0);
    HAL_NVIC_SetPriority (OTG_HS_WKUP_IRQn, 7, 0);   
}
