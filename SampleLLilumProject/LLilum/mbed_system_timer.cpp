//
// Copyright (c) Microsoft Corporation.    All rights reserved.
//

#include "mbed_helpers.h" 
#include "llos_system_timer.h"
#include "llos_memory.h"
//--//

extern "C"
{
    typedef struct LLOS_MbedTimer
    {
        LLOS_Context   Context;
        ticker_event_t TickerEvent;
    } LLOS_MbedTimer;

    static LLOS_SYSTEM_TIMER_Callback s_TimerCallback = NULL;
    static const ticker_data_t *s_pTickerData = get_us_ticker_data();

    // This is used to call back into the Kernel using a WellKnownMethod
    static void MbedInterruptHandler(uint32_t id)
    {
        if (s_TimerCallback != NULL)
        {
            LLOS_MbedTimer *pCtx = (LLOS_MbedTimer*)id;

            if (pCtx != NULL)
            {
                uint64_t ticks = us_ticker_read();

                s_TimerCallback(pCtx->Context, ticks);
            }
        }
    }

    HRESULT LLOS_SYSTEM_TIMER_Enable(LLOS_SYSTEM_TIMER_Callback callback)
    {
        s_TimerCallback = callback;

        us_ticker_init();

        ticker_set_handler(s_pTickerData, MbedInterruptHandler);

        return S_OK;
    }

    VOID LLOS_SYSTEM_TIMER_Disable()
    {
        s_TimerCallback = NULL;
    }

    HRESULT LLOS_SYSTEM_TIMER_SetTicks(uint64_t value)
    {
        return LLOS_E_NOT_SUPPORTED;
    }

    uint64_t LLOS_SYSTEM_TIMER_GetTicks()
    {
        return us_ticker_read();
    }

    uint64_t LLOS_SYSTEM_TIMER_GetTimerFrequency()
    {
        return 1000000; // 1us tick timer = 1MHz
    }

    HRESULT LLOS_SYSTEM_TIMER_AllocateTimer(LLOS_Context callbackContext, LLOS_Context *pTimer)
    {
        LLOS_MbedTimer *pCtx;

        if (pTimer == NULL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }
        
        pCtx = (LLOS_MbedTimer*)AllocateFromManagedHeap(sizeof(LLOS_MbedTimer));

        if (pCtx == NULL)
        {
            return LLOS_E_OUT_OF_MEMORY;
        }

        pCtx->Context = callbackContext;

        *pTimer = pCtx;

        return S_OK;
    }

    VOID LLOS_SYSTEM_TIMER_FreeTimer(LLOS_Context pTimer)
    {
        LLOS_MbedTimer *pCtx = (LLOS_MbedTimer*)pTimer;

        if (pCtx != NULL)
        {
            ticker_remove_event(s_pTickerData, &pCtx->TickerEvent);
        }
    }

    HRESULT LLOS_SYSTEM_TIMER_ScheduleTimer(LLOS_Context pTimer, uint64_t microsecondsFromNow)
    {
        LLOS_MbedTimer *pCtx = (LLOS_MbedTimer*)pTimer;

        if (pCtx == NULL || microsecondsFromNow > 0xFFFFFFFFuLL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        if (microsecondsFromNow < 2)
        {
            microsecondsFromNow = 2;
        }

        ticker_remove_event(s_pTickerData, &pCtx->TickerEvent);
        ticker_insert_event(s_pTickerData, &pCtx->TickerEvent, us_ticker_read() + (uint32_t)microsecondsFromNow, (uint32_t)pTimer);

        return S_OK;
    }
}
