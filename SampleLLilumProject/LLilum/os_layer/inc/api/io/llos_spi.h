//
//    LLILUM OS Abstraction Layer - SPI
// 

#ifndef __LLOS_SPI_H__
#define __LLOS_SPI_H__

#ifdef __cplusplus
extern "C" {
#endif

#include "llos_types.h"

// 
//  SPI - master and slave mode
//

typedef struct LLOS_SPI_ControllerConfig
{
    uint32_t ChipSelect;
    BOOL     LoopbackMode;
    BOOL     MSBTransferMode;
    BOOL     ActiveLow;
    BOOL     InversePolarity;
    BOOL     ClockIdleLevel;
    BOOL     ClockSamplingEdge;
    BOOL     Master;
    uint32_t PhaseMode;
    uint32_t WordSize;
    uint32_t ClockRateHz;
    uint32_t ChipSelectSetupCycles;
    uint32_t ChipSelectHoldCycles;
    uint32_t BusyPin;
} LLOS_SPI_ControllerConfig;

typedef enum LLOS_SPI_Action
{
    LLOS_SPI_ActionWrite = 0,
    LLOS_SPI_ActionRead,
    LLOS_SPI_ActionTransfer,
    LLOS_SPI_ActionError,
} LLOS_SPI_Action;

typedef VOID(*LLOS_SPI_Callback)(LLOS_Context channel, LLOS_Context callbackCtx, LLOS_SPI_Action edge);

HRESULT LLOS_SPI_Initialize  ( uint32_t mosi, uint32_t miso, uint32_t sclk, uint32_t chipSelect, LLOS_Context* ppChannel, LLOS_SPI_ControllerConfig** ppConfiguration );
VOID    LLOS_SPI_Uninitialize( LLOS_Context channel );
HRESULT LLOS_SPI_Configure   ( LLOS_Context channel, LLOS_SPI_ControllerConfig* pConfig );
HRESULT LLOS_SPI_SetCallback ( LLOS_Context channel, LLOS_SPI_Callback request, LLOS_Context context );
HRESULT LLOS_SPI_SetFrequency( LLOS_Context channel, uint32_t frequencyHz );
HRESULT LLOS_SPI_Transfer    ( LLOS_Context channel, uint8_t* txBuffer, int32_t txOffset, int32_t txCount, uint8_t* rxBuffer, int32_t rxOffset, int32_t rxCount, int32_t rxStartOffset);
HRESULT LLOS_SPI_Write       ( LLOS_Context channel, uint8_t* txBuffer, int32_t txOffset, int32_t txCount ); 
HRESULT LLOS_SPI_Read        ( LLOS_Context channel, uint8_t* rxBuffer, int32_t rxOffset, int32_t rxCount, int32_t rxStartOffset ); 
HRESULT LLOS_SPI_IsBusy      ( LLOS_Context channel, BOOL* isBusy );
HRESULT LLOS_SPI_Suspend     ( LLOS_Context channel );
HRESULT LLOS_SPI_Resume      ( LLOS_Context channel );

#ifdef __cplusplus
}
#endif

#endif // __LLOS_SPI_H__
