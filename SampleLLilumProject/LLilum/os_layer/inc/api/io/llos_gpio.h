//
//    LLILUM OS Abstraction Layer - GPIO
//

#ifndef __LLOS_GPIO_H__
#define __LLOS_GPIO_H__

#ifdef __cplusplus
extern "C" {
#endif

#include "llos_types.h"

//
//  GP I/O
//

typedef enum LLOS_GPIO_Resistor
{
    LLOS_GPIO_ResistorDefault = 0,
    LLOS_GPIO_ResistorPullNone,
    LLOS_GPIO_ResistorPullup,
    LLOS_GPIO_ResistorPulldown, 
    LLOS_GPIO_ResistorOpenDrain,
    LLOS_GPIO_ResistorRepeater,
} LLOS_GPIO_Resistor;

typedef enum LLOS_GPIO_Edge
{
    LLOS_GPIO_EdgeNone = 0,
    LLOS_GPIO_EdgeRising,
    LLOS_GPIO_EdgeFalling,
    LLOS_GPIO_EdgeBoth,
    LLOS_GPIO_EdgeLevelLow,
    LLOS_GPIO_EdgeLevelHigh,
} LLOS_GPIO_Edge;

typedef enum LLOS_GPIO_Polarity
{
    LLOS_GPIO_polarity_normal = 0,
    LLOS_GPIO_polarity_inverted,
} LLOS_GPIO_Polarity;

typedef enum LLOS_GPIO_Direction
{
    LLOS_GPIO_Input = 0,
    LLOS_GPIO_Output
} LLOS_GPIO_Direction;

typedef VOID(*LLOS_GPIO_InterruptCallback)(LLOS_Context pin, LLOS_Context callbackCtx, LLOS_GPIO_Edge edge);

HRESULT LLOS_GPIO_AllocatePin ( uint32_t pin_number, LLOS_Context* pin                                                                     );
VOID    LLOS_GPIO_FreePin     ( LLOS_Context pin                                                                                           );
HRESULT LLOS_GPIO_EnablePin   ( LLOS_Context pin, LLOS_GPIO_Edge edge, LLOS_GPIO_InterruptCallback callback, LLOS_Context callback_context );
HRESULT LLOS_GPIO_DisablePin  ( LLOS_Context pin                                                                                           );
HRESULT LLOS_GPIO_SetPolarity ( LLOS_Context pin, LLOS_GPIO_Polarity polarity                                                              );
HRESULT LLOS_GPIO_SetMode     ( LLOS_Context pin, LLOS_GPIO_Resistor resistor                                                              );
HRESULT LLOS_GPIO_SetDirection( LLOS_Context pin, LLOS_GPIO_Direction direction                                                            );
HRESULT LLOS_GPIO_SetDebounce ( LLOS_Context pin, LLOS_TimeSpan debounce_time                                                              );
// TOOD: Documentation: Make sure to note that the managed code layer does not restrict input pins from being written to.
HRESULT LLOS_GPIO_Write       ( LLOS_Context pin, int32_t value                                                                            );
int32_t LLOS_GPIO_Read        ( LLOS_Context pin                                                                                           );

#ifdef __cplusplus
}
#endif

#endif // __LLOS_GPIO_H__