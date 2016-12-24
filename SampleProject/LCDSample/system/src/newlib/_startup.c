//
// This file is part of the µOS++ III distribution.
// Copyright (c) 2014 Liviu Ionescu.
//

// ----------------------------------------------------------------------------

// This module contains the startup code for a portable embedded
// C/C++ application, built with newlib.
//
// Control reaches here from the reset handler via jump or call.
//
// The actual steps performed by _start are:
// - clear the BSS area
// - copy the initialised data section
// - initialise the system
// - run the preinit/init array (for the C++ static constructors)
// - initialise the arc/argv
// - branch to main()
// - run the fini array (for the C++ static destructors)
// - call _exit(), directly or via exit()
//
// The normal configuration is standalone, with all support
// functions implemented locally.
//
// For this to be called, the project linker must be configured without
// the startup sequence (-nostartfiles).

// ----------------------------------------------------------------------------

#include <stdint.h>
#include <sys/types.h>

// ----------------------------------------------------------------------------

#define OS_INCLUDE_STARTUP_GUARD_CHECKS (1)

// ----------------------------------------------------------------------------

// Begin address for the initialisation values of the .data section.
// defined in linker script
extern unsigned int _sidata;
// Begin address for the .data section; defined in linker script
extern unsigned int _sdata;
// End address for the .data section; defined in linker script
extern unsigned int _edata;

// Begin address for the .bss section; defined in linker script
extern unsigned int __bss_start__;
// End address for the .bss section; defined in linker script
extern unsigned int __bss_end__;

extern void
__initialize_args(int*, char***);

// main() is the entry point for newlib based applications.
// By default, there are no arguments, but this can be customised
// by redefining __initialize_args(), which is done when the
// semihosting configurations are used.
extern int
main(int argc, char* argv[]);

// The implementation for the exit routine; for embedded
// applications, a system reset will be performed.
extern void
__attribute__((noreturn))
_exit(int);

// ----------------------------------------------------------------------------

// Forward declarations

void
_start(void);

void
__initialize_data(unsigned int* from, unsigned int* section_begin,
    unsigned int* section_end);

void
__initialize_bss(unsigned int* section_begin, unsigned int* section_end);

void
__run_init_array(void);

void
__run_fini_array(void);

void
__initialize_hardware(void);

// ----------------------------------------------------------------------------

inline void
__attribute__((always_inline))
__initialize_data(unsigned int* from, unsigned int* section_begin,
    unsigned int* section_end)
{
  // Iterate and copy word by word.
  // It is assumed that the pointers are word aligned.
  unsigned int *p = section_begin;
  while (p < section_end)
    *p++ = *from++;
}

inline void
__attribute__((always_inline))
__initialize_bss(unsigned int* section_begin, unsigned int* section_end)
{
  // Iterate and clear word by word.
  // It is assumed that the pointers are word aligned.
  unsigned int *p = section_begin;
  while (p < section_end)
    *p++ = 0;
}

// These magic symbols are provided by the linker.
extern void
(*__preinit_array_start[])(void) __attribute__((weak));
extern void
(*__preinit_array_end[])(void) __attribute__((weak));
extern void
(*__init_array_start[])(void) __attribute__((weak));
extern void
(*__init_array_end[])(void) __attribute__((weak));
extern void
(*__fini_array_start[])(void) __attribute__((weak));
extern void
(*__fini_array_end[])(void) __attribute__((weak));

// Iterate over all the preinit/init routines (mainly static constructors).
inline void
__attribute__((always_inline))
__run_init_array(void)
{
  int count;
  int i;

  count = __preinit_array_end - __preinit_array_start;
  for (i = 0; i < count; i++)
    __preinit_array_start[i]();

  // If you need to run the code in the .init section, please use
  // the startup files, since this requires the code in crti.o and crtn.o
  // to add the function prologue/epilogue.
  //_init(); // DO NOT ENABE THIS!

  count = __init_array_end - __init_array_start;
  for (i = 0; i < count; i++)
    __init_array_start[i]();
}

// Run all the cleanup routines (mainly static destructors).
inline void
__attribute__((always_inline))
__run_fini_array(void)
{
  int count;
  int i;

  count = __fini_array_end - __fini_array_start;
  for (i = count; i > 0; i--)
    __fini_array_start[i - 1]();

  // If you need to run the code in the .fini section, please use
  // the startup files, since this requires the code in crti.o and crtn.o
  // to add the function prologue/epilogue.
  //_fini(); // DO NOT ENABE THIS!
}

#if defined(DEBUG) && defined(OS_INCLUDE_STARTUP_GUARD_CHECKS)

// These definitions are used to check if the routines used to
// clear the BSS and to copy the initialised DATA perform correctly.

#define BSS_GUARD_BAD_VALUE (0xCADEBABA)

static uint32_t volatile __attribute__ ((section(".bss_begin")))
__bss_begin_guard;
static uint32_t volatile __attribute__ ((section(".bss_end")))
__bss_end_guard;

#define DATA_GUARD_BAD_VALUE (0xCADEBABA)
#define DATA_BEGIN_GUARD_VALUE (0x12345678)
#define DATA_END_GUARD_VALUE (0x98765432)

static uint32_t volatile __attribute__ ((section(".data_begin")))
__data_begin_guard = DATA_BEGIN_GUARD_VALUE;

static uint32_t volatile __attribute__ ((section(".data_end")))
__data_end_guard = DATA_END_GUARD_VALUE;

#endif // defined(DEBUG) && defined(OS_INCLUDE_STARTUP_GUARD_CHECKS)

// This is the place where Cortex-M core will go immediately after reset,
// via a call or jump from the Reset_Handler
void __attribute__ ((section(".after_vectors"),noreturn))
_start(void)
{

  // Use Old Style Data and BSS section initialisation,
  // that will initialise a single BSS sections.

#if defined(DEBUG) && defined(OS_INCLUDE_STARTUP_GUARD_CHECKS)
  __bss_begin_guard = BSS_GUARD_BAD_VALUE;
  __bss_end_guard = BSS_GUARD_BAD_VALUE;
#endif

  // Zero fill the bss segment
  __initialize_bss(&__bss_start__, &__bss_end__);

#if defined(DEBUG) && defined(OS_INCLUDE_STARTUP_GUARD_CHECKS)
  if ((__bss_begin_guard != 0) || (__bss_end_guard != 0))
    {
      for (;;)
        ;
    }
#endif

#if defined(DEBUG) && defined(OS_INCLUDE_STARTUP_GUARD_CHECKS)
  __data_begin_guard = DATA_GUARD_BAD_VALUE;
  __data_end_guard = DATA_GUARD_BAD_VALUE;
#endif

  // Copy the data segment from Flash to RAM.
  // When using startup files, this code is executed via the preinit array.
  __initialize_data(&_sidata, &_sdata, &_edata);

#if defined(DEBUG) && defined(OS_INCLUDE_STARTUP_GUARD_CHECKS)
  if ((__data_begin_guard != DATA_BEGIN_GUARD_VALUE) || (__data_end_guard != __data_end_guard))
    {
      for (;;)
        ;
    }
#endif

  __initialize_hardware();

  // Get the args (useful in semihosting configurations).
  int argc;
  char** argv;
  __initialize_args(&argc, &argv);

  // Call the standard library initialisation (mandatory for C++ to
  // execute the constructors for the static objects).
  __run_init_array();

  // Call the main entry point, and save the exit code.
  int code = main(argc, argv);

  // Run the C++ static destructors.
  __run_fini_array();

  _exit(code);

  // Should never reach this, _exit() should have already
  // performed a reset.
  for (;;)
    ;
}

// ----------------------------------------------------------------------------
