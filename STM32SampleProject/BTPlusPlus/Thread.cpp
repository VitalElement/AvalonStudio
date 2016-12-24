/******************************************************************************
*       Description:
*
*       Author:
*         Date: 22 July 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#pragma mark Includes
#include "Thread.h"
#include "FreeRTOS.h"
#include "task.h"
#include "Kernel.h"

#pragma mark Definitions and Constants
static const uint32_t DefaultStackDepth = 200;
static const uint32_t DefaultTaskPriority = tskIDLE_PRIORITY + 1;

#pragma mark Static Data
typedef struct
{
    void* stackPtr;
    void* userData;
} TaskHandle;

#pragma mark Static Functions
void Thread::Execute (void* param)
{
    auto thread = ((Thread*)param);

    thread->isCreated = true;

    if (thread->suspendImmediately)
    {
        Thread::Suspend ();
    }

    thread->action ();

    vTaskDelete (nullptr);
}

void Thread::Yield ()
{
    taskYIELD ();
}

void Thread::Sleep (uint32_t timeMs)
{
    vTaskDelay (timeMs * portTICK_PERIOD_MS);
}

void Thread::Suspend ()
{
    vTaskSuspend (nullptr);
}

void Thread::BeginCriticalRegion ()
{
    portDISABLE_INTERRUPTS ();
}

void Thread::EndCriticalRegion ()
{
    portENABLE_INTERRUPTS ();
}

Thread* Thread::StartNew (Action action)
{
    return Thread::StartNew (action, DefaultStackDepth);
}

Thread* Thread::StartNew (Action action, uint32_t stackDepth)
{
    auto result = new Thread (action, stackDepth);

    result->Start ();

    return result;
}

Thread* Thread::GetCurrentThread ()
{
    return (Thread*)((TaskHandle*)xTaskGetCurrentTaskHandle ())->userData;
}

#pragma mark Member Implementations
Thread::Thread (Action action, uint32_t stackDepth)
{
    suspendImmediately = Kernel::IsRunning ();

    isCreated = false;

    handle = nullptr;

    this->action = action;

    xTaskCreate (Execute, nullptr, stackDepth, this, DefaultTaskPriority,
                 &handle);

    if (suspendImmediately)
    {
        while (!isCreated)
        {
            Yield ();
        }
    }
}

Thread::Thread (Action action) : Thread (action, DefaultStackDepth)
{
}

Thread::~Thread ()
{
    vTaskDelete (handle);
}

void Thread::Start ()
{
    Resume ();
}

void Thread::Resume ()
{
    vTaskResume (handle);
}
