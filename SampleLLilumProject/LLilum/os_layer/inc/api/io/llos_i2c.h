//
//    LLILUM OS Abstraction Layer - I2C
// 

#ifndef __LLOS_I2C_H__
#define __LLOS_I2C_H__

#ifdef __cplusplus
extern "C" {
#endif

#include "llos_types.h"

// 
// I2C
//

HRESULT LLOS_I2C_Initialize  (int32_t sdaPin, int32_t sclPin, LLOS_Context* pChannel);
VOID    LLOS_I2C_Uninitialize(LLOS_Context channel);
HRESULT LLOS_I2C_SetFrequency(LLOS_Context channel, uint32_t frequencyHz);
HRESULT LLOS_I2C_Enable      (LLOS_Context channel);
HRESULT LLOS_I2C_Disable     (LLOS_Context channel);
HRESULT LLOS_I2C_Write       (LLOS_Context channel, uint32_t address, uint8_t* pBuffer, int32_t offset, int32_t* pLength, BOOL stop);
HRESULT LLOS_I2C_Read        (LLOS_Context channel, uint32_t address, uint8_t* pBuffer, int32_t offset, int32_t* pLength, BOOL stop);
HRESULT LLOS_I2C_Reset       (LLOS_Context channel);

#ifdef __cplusplus
}
#endif

#endif // __LLOS_I2C_H__