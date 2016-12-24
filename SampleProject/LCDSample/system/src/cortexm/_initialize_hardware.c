//
// This file is part of the µOS++ III distribution.
// Copyright (c) 2014 Liviu Ionescu.
//

// ----------------------------------------------------------------------------

#include "cmsis_device.h"

// ----------------------------------------------------------------------------

// Forward declarations.

void
__initialize_hardware(void);

// ----------------------------------------------------------------------------

// This is the default hardware initialisation routine, it can be
// redefined in the application for more complex applications that
// require early inits (before constructors), otherwise these can
// be done in main().
//
// Called early from _start(), right after data & bss init, before
// constructors.
//
// After Reset the Cortex-M processor is in Thread mode,
// priority is Privileged, and the Stack is set to Main.

void
__attribute__((weak))
__initialize_hardware(void)
{
  // Call the CSMSIS system initialisation routine.
  SystemInit();

#if defined (__VFP_FP__) && !defined (__SOFTFP__)

  // Enable the Cortex-M4 FPU only when -mfloat-abi=hard.
  // Code taken from Section 7.1, Cortex-M4 TRM (DDI0439C)

  // Set bits 20-23 to enable CP10 and CP11 coprocessor
  SCB->CPACR |= (0xF << 20);

#endif // (__VFP_FP__) && !(__SOFTFP__)
}

// ----------------------------------------------------------------------------
