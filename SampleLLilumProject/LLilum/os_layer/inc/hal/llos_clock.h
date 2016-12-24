//
//    LLILUM OS Abstraction Layer - Clock
// 

#ifndef __LLOS_CLOCK_H__
#define __LLOS_CLOCK_H__

#ifdef __cplusplus
extern "C" {
#endif

#include "llos_types.h"

// 
// System clock controller
//

uint64_t LLOS_CLOCK_GetClockTicks();
uint64_t LLOS_CLOCK_GetClockFrequency();
uint64_t LLOS_CLOCK_GetPerformanceCounter();
uint64_t LLOS_CLOCK_GetPerformanceCounterFrequency();

uint32_t LLOS_CLOCK_DelayCycles(uint32_t cycles);
uint32_t LLOS_CLOCK_Delay(uint32_t microSeconds);

#ifdef __cplusplus
}
#endif

#endif // __LLOS_CLOCK_H__
