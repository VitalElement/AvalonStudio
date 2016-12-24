//
//    LLILUM OS Abstraction Layer - SERIAL
// 

#ifndef __LLOS_SERIAL_H__
#define __LLOS_SERIAL_H__

#ifdef __cplusplus
extern "C" {
#endif

#include "llos_types.h"

// 
//  Serial
//

typedef enum LLOS_SERIAL_Parity
{
    LLOS_SERIAL_ParityNone = 0,
    LLOS_SERIAL_ParityOdd,
    LLOS_SERIAL_ParityEven,
} LLOS_SERIAL_Parity;

typedef enum LLOS_SERIAL_StopBits
{
    LLOS_SERIAL_StopBitsNone = 0,
    LLOS_SERIAL_StopBitsOne,
    LLOS_SERIAL_StopBitsTwo,
    LLOS_SERIAL_StopBitsOnePointFive,
} LLOS_SERIAL_StopBits;

typedef struct LLOS_SERIAL_Config
{
    uint32_t BaudRate;
    LLOS_SERIAL_Parity Parity;
    uint32_t DataBits;
    uint32_t StopBits;
    uint32_t SoftwareFlowControlValue;
} LLOS_SERIAL_Config;

typedef enum LLOS_SERIAL_Event
{
    LLOS_SERIAL_EventRx = 0,
    LLOS_SERIAL_EventTx,
} LLOS_SERIAL_Event;

typedef enum LLOS_SERIAL_Irq
{
    LLOS_SERIAL_IrqRx = 0,
    LLOS_SERIAL_IrqTx,
    LLOS_SERIAL_IrqBoth
} LLOS_SERIAL_Irq;

typedef VOID(*LLOS_SERIAL_InterruptCallback)(LLOS_Context port, LLOS_Context callbackCtx, LLOS_SERIAL_Event serialEvent);


HRESULT LLOS_SERIAL_Open          (int32_t rxPin, int32_t txPin, LLOS_SERIAL_Config **ppConfig, LLOS_Context* pChannel);
VOID    LLOS_SERIAL_Close         (LLOS_Context channel);
HRESULT LLOS_SERIAL_Enable        (LLOS_Context channel, LLOS_SERIAL_Irq irq);
HRESULT LLOS_SERIAL_Disable       (LLOS_Context channel, LLOS_SERIAL_Irq irq);
HRESULT LLOS_SERIAL_SetFlowControl(LLOS_Context channel, int32_t rtsPin, int32_t ctsPin);
HRESULT LLOS_SERIAL_Configure     (LLOS_Context channel, LLOS_SERIAL_Config* pConfig);
HRESULT LLOS_SERIAL_Read          (LLOS_Context channel, uint8_t* pBuffer, int32_t offset, int32_t* pLength);
HRESULT LLOS_SERIAL_Write         (LLOS_Context channel, uint8_t* pBuffer, int32_t offset, int32_t length);
HRESULT LLOS_SERIAL_Flush         (LLOS_Context channel);
HRESULT LLOS_SERIAL_Clear         (LLOS_Context channel);
HRESULT LLOS_SERIAL_SetCallback   (LLOS_Context channel, LLOS_SERIAL_InterruptCallback callback, LLOS_Context callbackContext);
HRESULT LLOS_SERIAL_CanRead       (LLOS_Context channel, BOOL *pCanRead);
HRESULT LLOS_SERIAL_CanWrite      (LLOS_Context channel, BOOL *pCanWrite);

#ifdef __cplusplus
}
#endif

#endif // __LLOS_SERIAL_H__
