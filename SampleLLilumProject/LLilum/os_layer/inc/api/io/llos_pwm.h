//
//    LLILUM OS Abstraction Layer - PWM
// 

#ifndef __LLOS_PWM_H__
#define __LLOS_PWM_H__

#ifdef __cplusplus
extern "C" {
#endif

#include "llos_types.h"

typedef enum LLOS_PWM_Polarity
{
    LLOS_PWM_PolarityNormal = 0,
    LLOS_PWM_PolarityInverted,
} LLOS_PWM_Polarity;

typedef enum LLOS_PWM_Prescaler {
    LLOS_PWM_PrescalerDiv1 = 0,
    LLOS_PWM_PrescalerDiv2,
    LLOS_PWM_PrescalerDiv4,
    LLOS_PWM_PrescalerDiv8,
    LLOS_PWM_PrescalerDiv16,
    LLOS_PWM_PrescalerDiv64,
    LLOS_PWM_PrescalerDiv256,
    LLOS_PWM_PrescalerDiv1024
} LLOS_PWM_Prescaler;

// 
//  PWM In/Out
//

HRESULT LLOS_PWM_Initialize   (uint32_t pinName, LLOS_Context* channel);
VOID    LLOS_PWM_Uninitialize (LLOS_Context channel);
HRESULT LLOS_PWM_SetDutyCycle (LLOS_Context channel, uint32_t dutyCycleNumerator, uint32_t dutyCycleDenominator);
HRESULT LLOS_PWM_SetPeriod    (LLOS_Context channel, uint32_t periodMicroSeconds);
HRESULT LLOS_PWM_SetPulseWidth(LLOS_Context channel, uint32_t widthMicroSeconds);
HRESULT LLOS_PWM_SetPolarity  (LLOS_Context channel, LLOS_PWM_Polarity  polarity);
HRESULT LLOS_PWM_SetPrescaler (LLOS_Context channel, LLOS_PWM_Prescaler prescaler);
HRESULT LLOS_PWM_Start        (LLOS_Context channel);
HRESULT LLOS_PWM_Stop         (LLOS_Context channel);

#ifdef __cplusplus
}
#endif

#endif // __LLOS_PWM_H__
