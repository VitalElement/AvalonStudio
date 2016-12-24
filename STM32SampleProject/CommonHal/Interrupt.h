/******************************************************************************
*       Description:
*
*       Author:
*         Date: January 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _INTERRUPT_H_
#define _INTERRUPT_H_

#pragma mark Includes
#include <functional>
#include <vector>
#include <stdint.h>

#define MAX_INTERRUPTS (50)

class Interrupt;

class Interrupt
{
  public:
    static void Register (int interruptNumber, Interrupt* intThisPtr)
    {
        ISRVectorTable[interruptNumber] = intThisPtr;
    }


    static void Call (uint32_t interrupt)
    {

        if (ISRVectorTable[interrupt] != nullptr)
        {
            ISRVectorTable[interrupt]->ISR ();
        }
    }

    // wrapper functions to ISR()
    static void Interrupt_0 (void)
    {
        ISRVectorTable[0]->ISR ();
    }

    static void Interrupt_1 (void)
    {
        ISRVectorTable[1]->ISR ();
    }

    static void Interrupt_2 (void)
    {
        ISRVectorTable[2]->ISR ();
    }

    /* etc.*/
    virtual void ISR (void) = 0;

  private:
    static Interrupt* ISRVectorTable[];
};
#endif
