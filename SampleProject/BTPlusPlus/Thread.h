/******************************************************************************
*       Description:
*
*       Author:
*         Date: 22 July 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _THREAD_H_
#define _THREAD_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>
#include "Action.h"

class Thread
{
    friend class Kernel;
#pragma mark Public Members
  public:
    Thread (Action action, uint32_t stackDepth);
    Thread (Action action);
    ~Thread ();

    void Start ();
    static void Yield ();
    static void Sleep (uint32_t timeMs);
    static void Suspend ();
    static void BeginCriticalRegion ();
    static void EndCriticalRegion ();
    static Thread* GetCurrentThread ();
    static Thread* StartNew (Action action);
    static Thread* StartNew (Action action, uint32_t stackDepth);
    void Resume ();


#pragma mark Private Members
  private:
    static void Execute (void* param);

    void* handle;
    Action action;
    volatile bool isCreated;
    bool suspendImmediately;
};


#endif
