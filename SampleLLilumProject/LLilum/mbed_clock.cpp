//
// Copyright (c) Microsoft Corporation.    All rights reserved.
//

#include "mbed_helpers.h" 
#include "llos_clock.h"

//--//

extern "C"
{

    uint64_t LLOS_CLOCK_GetTicks()
    {
        return us_ticker_read();
    }

    uint64_t LLOS_CLOCK_GetClockFrequency()
    {
        return SystemCoreClock;
    }

    uint64_t LLOS_CLOCK_GetPerformanceCounter()
    {
        return us_ticker_read();
    }

    uint64_t LLOS_CLOCK_GetPerformanceCounterFrequency()
    {
        return SystemCoreClock;
    }

    uint32_t LLOS_CLOCK_Delay(uint32_t microSeconds)
    {
        // LLOS_CLOCK_DelayCycles is defined in mbed_asm.S
        return LLOS_CLOCK_DelayCycles(SystemCoreClock / 1000000 * microSeconds);
    }

}

