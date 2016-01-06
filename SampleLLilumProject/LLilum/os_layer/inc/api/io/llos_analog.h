//
//    LLILUM OS Abstraction Layer - Analog
// 

#ifndef __LLOS_ANALOG_H__
#define __LLOS_ANALOG_H__

#ifdef __cplusplus
extern "C" {
#endif

#include "llos_types.h"

// 
//  Analog In/Out
//

typedef enum LLOS_ADC_Direction
{
    LLOS_ADC_Input = 0,
    LLOS_ADC_Output,
} LLOS_ADC_Direction;

HRESULT LLOS_ADC_Initialize(uint32_t pinName, LLOS_ADC_Direction direction, LLOS_Context* channel);
VOID    LLOS_ADC_Uninitialize(LLOS_Context channel);
HRESULT LLOS_ADC_ReadRaw(LLOS_Context channel, int32_t* value);
HRESULT LLOS_ADC_WriteRaw(LLOS_Context channel, int32_t value);
HRESULT LLOS_ADC_Read(LLOS_Context channel, float* value);
HRESULT LLOS_ADC_Write(LLOS_Context channel, float value);
HRESULT LLOS_ADC_GetPrecisionBits(LLOS_Context channel, uint32_t* precisionInBits);

#ifdef __cplusplus
}
#endif

#endif // __LLOS_ANALOG_H__
