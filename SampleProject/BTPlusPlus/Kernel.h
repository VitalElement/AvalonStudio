/******************************************************************************
*       Description:
*
*       Author:
*         Date: 23 July 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _KERNEL_H_
#define _KERNEL_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>

class Kernel
{
  public:
    static void Start ();
    static void Shutdown ();
    static bool IsRunning ();
    static uint32_t GetSystemTime ();

  private:
    static bool isRunning;
};

#endif
