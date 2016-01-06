//
//    LLILUM OS Abstraction Layer - Timer
// 

#ifndef __LLOS_SYSTEM_TIMER_H__
#define __LLOS_SYSTEM_TIMER_H__

#ifdef __cplusplus
extern "C" {
#endif

#include "llos_types.h"

// 
// System timer controller
//

typedef VOID(*LLOS_SYSTEM_TIMER_Callback)(LLOS_Context callbackCtx, uint64_t ticks);

HRESULT  LLOS_SYSTEM_TIMER_Enable  (LLOS_SYSTEM_TIMER_Callback callback);
VOID     LLOS_SYSTEM_TIMER_Disable ();
HRESULT  LLOS_SYSTEM_TIMER_SetTicks(uint64_t value);
uint64_t LLOS_SYSTEM_TIMER_GetTicks();
uint64_t LLOS_SYSTEM_TIMER_GetTimerFrequency();

HRESULT  LLOS_SYSTEM_TIMER_AllocateTimer(LLOS_Context callbackContext, LLOS_Context *pTimer);
VOID     LLOS_SYSTEM_TIMER_FreeTimer    (LLOS_Context pTimer);
HRESULT  LLOS_SYSTEM_TIMER_ScheduleTimer(LLOS_Context pTimer, uint64_t microsecondsFromNow);

#ifdef __cplusplus
}
#endif

#endif // __LLOS_SYSTEM_TIMER_H__
