/******************************************************************************
*       Description:
*
*       Author:
*         Date: 23 July 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#pragma mark Includes
#include "Kernel.h"
#include "FreeRTOS.h"
#include "task.h"

#pragma mark Definitions and Constants


#pragma mark Static Data
bool Kernel::isRunning = false;

#pragma mark Static Functions
extern "C" void vApplicationTickHook ()
{
}

extern "C" void vApplicationIdleHook ()
{
}

extern "C" void vApplicationMallocFailedHook ()
{
}

extern "C" void vApplicationStackOverflowHook (xTaskHandle* pxTask,
                                               signed portCHAR* pcTaskName)
{
    while (true)
    {
    }
}


#pragma mark Member Implementations
void Kernel::Start ()
{
    isRunning = true;
    vTaskStartScheduler ();
}

bool Kernel::IsRunning ()
{
    return isRunning;
}

void Kernel::Shutdown ()
{
    vTaskEndScheduler ();
}

uint32_t Kernel::GetSystemTime ()
{
    TickType_t result;

    // Note this may not be portable (we may need a compatibility function ...
    // IsInterruptContext)
    if (portNVIC_INT_CTRL_REG != 0)
    {
        result = xTaskGetTickCountFromISR ();
    }
    else
    {
        result = xTaskGetTickCount ();
    }

    return result;
}
