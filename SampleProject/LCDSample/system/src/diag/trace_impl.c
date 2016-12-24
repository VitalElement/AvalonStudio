//
// This file is part of the µOS++ III distribution.
// Copyright (c) 2014 Liviu Ionescu.
//

// ----------------------------------------------------------------------------

#if defined(TRACE)

#include "cmsis_device.h"
#include "diag/Trace.h"

// ----------------------------------------------------------------------------

// One of these definitions must be passed via the compiler command line
// Note: small Cortex-M0/M0+ might implement a simplified debug interface.

//#define OS_USE_TRACE_ITM
//#define OS_USE_TRACE_SEMIHOSTING_DEBUG
//#define OS_USE_TRACE_SEMIHOSTING_STDOUT

#if !(defined(__ARM_ARCH_7M__) || defined(__ARM_ARCH_7EM__))
#if defined(OS_USE_TRACE_ITM)
#undef OS_USE_TRACE_ITM
#warning "ITM unavailable"
#endif // defined(OS_USE_TRACE_ITM)
#endif // !(defined(__ARM_ARCH_7M__) || defined(__ARM_ARCH_7EM__))

// ----------------------------------------------------------------------------

// Forward definitions.

#if defined(OS_USE_TRACE_ITM)
static ssize_t
_trace_write_itm (const char* buf, size_t nbyte);
#endif

#if defined(OS_USE_TRACE_SEMIHOSTING_STDOUT)
static ssize_t
_trace_write_semihosting_stdout(const char* buf, size_t nbyte);
#endif

#if defined(OS_USE_TRACE_SEMIHOSTING_DEBUG)
static ssize_t
_trace_write_semihosting_debug(const char* buf, size_t nbyte);
#endif

// ----------------------------------------------------------------------------

void
trace_initialize(void)
{
  // No initialisations required for ITM / semihosting
}

// ----------------------------------------------------------------------------

// This function is called from _write() for fd==1 or fd==2 and from some
// of the trace_* functions.

ssize_t
trace_write (const char* buf __attribute__((unused)),
	     size_t nbyte __attribute__((unused)))
{
#if defined(OS_USE_TRACE_ITM)
  return _trace_write_itm (buf, nbyte);
#elif defined(OS_USE_TRACE_SEMIHOSTING_STDOUT)
  return _trace_write_semihosting_stdout(buf, nbyte);
#elif defined(OS_USE_TRACE_SEMIHOSTING_DEBUG)
  return _trace_write_semihosting_debug(buf, nbyte);
#endif

  return -1;
}

// ----------------------------------------------------------------------------

#if defined(OS_USE_TRACE_ITM)

#if defined(__ARM_ARCH_7M__) || defined(__ARM_ARCH_7EM__)

// ITM is the ARM standard mechanism, running over SWD/SWO on Cortex-M3/M4
// devices, and is the recommended setting, if available.
//
// The JLink probe and the GDB server fully support SWD/SWO
// and the JLink Debugging plug-in enables it by default.
// The current OpenOCD does not include support to parse the SWO stream,
// so this configuration will not work on OpenOCD (will not crash, but
// nothing will be displayed in the output console).

#if !defined(OS_INTEGER_TRACE_ITM_STIMULUS_PORT)
#define OS_INTEGER_TRACE_ITM_STIMULUS_PORT     (0)
#endif

static ssize_t
_trace_write_itm (const char* buf, size_t nbyte)
{
  for (size_t i = 0; i < nbyte; i++)
    {
      // Check if ITM or the stimulus port are not enabled
      if (((ITM->TCR & ITM_TCR_ITMENA_Msk) == 0)
	  || ((ITM->TER & (1UL << OS_INTEGER_TRACE_ITM_STIMULUS_PORT)) == 0))
	{
	  return (ssize_t)i; // return the number of sent characters (may be 0)
	}

      // Wait until STIMx is ready...
      while (ITM->PORT[OS_INTEGER_TRACE_ITM_STIMULUS_PORT].u32 == 0)
	;
      // then send data, one byte at a time
      ITM->PORT[OS_INTEGER_TRACE_ITM_STIMULUS_PORT].u8 = (uint8_t) (*buf++);
    }

  return (ssize_t)nbyte; // all characters successfully sent
}

#endif // defined(__ARM_ARCH_7M__) || defined(__ARM_ARCH_7EM__)

#endif // OS_USE_TRACE_ITM

// ----------------------------------------------------------------------------

#if defined(OS_USE_TRACE_SEMIHOSTING_DEBUG) || defined(OS_USE_TRACE_SEMIHOSTING_STDOUT)

#include "arm/semihosting.h"

// Semihosting is the other output channel that can be used for the trace
// messages. It comes in two flavours: STDOUT and DEBUG. The STDOUT channel
// is the equivalent of the stdout in POSIX and in most cases it is forwarded
// to the GDB server stdout stream. The debug channel is a separate
// channel. STDOUT is buffered, so nothing is displayed until a \n;
// DEBUG is not buffered, but can be slow.
//
// Choosing between semihosting stdout and debug depends on the capabilities
// of your GDB server, and also on specific needs. It is recommended to test
// DEBUG first, and if too slow, try STDOUT.
//
// The JLink GDB server fully support semihosting, and both configurations
// are available; to activate it, use "monitor semihosting enable" or check
// the corresponding button in the JLink Debugging plug-in.
// In OpenOCD, support for semihosting can be enabled using
// "monitor arm semihosting enable".
//
// Note: Applications built with semihosting output active cannot be
// executed without the debugger connected and active, since they use
// BKPT to communicate with the host. Attempts to run them standalone or
// without semihosting enabled will usually be terminated with hardware faults.

#endif // OS_USE_TRACE_SEMIHOSTING_DEBUG_*

// ----------------------------------------------------------------------------

#if defined(OS_USE_TRACE_SEMIHOSTING_STDOUT)

static ssize_t
_trace_write_semihosting_stdout (const char* buf, size_t nbyte)
{
#if defined(__ARM_ARCH_7M__) || defined(__ARM_ARCH_7EM__)
  // Check if the debugger is enabled. CoreDebug is available only on CM3/CM4.
  // [Contributed by SourceForge user diabolo38]
  if ((CoreDebug->DHCSR & CoreDebug_DHCSR_C_DEBUGEN_Msk) == 0)
    {
      // If not, pretend we wrote all bytes
      return (ssize_t) (nbyte);
    }
#endif // defined(__ARM_ARCH_7M__) || defined(__ARM_ARCH_7EM__)

  static int handle;
  void* block[3];
  int ret;

  if (handle == 0)
    {
      // On the first call get the file handle from the host
      block[0] = ":tt"; // special filename to be used for stdin/out/err
      block[1] = (void*) 4; // mode "w"
      // length of ":tt", except null terminator
      block[2] = (void*) (sizeof(":tt") - 1);

      ret = call_host (SEMIHOSTING_SYS_OPEN, (void*) block);
      if (ret == -1)
        return -1;

      handle = ret;
    }

  block[0] = (void*) handle;
  block[1] = (void*) buf;
  block[2] = (void*) nbyte;
  // send character array to host file/device
  ret = call_host (SEMIHOSTING_SYS_WRITE, (void*) block);
  // this call returns the number of bytes NOT written (0 if all ok)

  // -1 is not a legal value, but SEGGER seems to return it
  if (ret == -1)
    return -1;

  // The compliant way of returning errors
  if (ret == (int) nbyte)
    return -1;

  // Return the number of bytes written
  return (ssize_t) (nbyte) - (ssize_t) ret;
}

#endif // OS_USE_TRACE_SEMIHOSTING_STDOUT

// ----------------------------------------------------------------------------

#if defined(OS_USE_TRACE_SEMIHOSTING_DEBUG)

#define OS_INTEGER_TRACE_TMP_ARRAY_SIZE  (16)

static ssize_t
_trace_write_semihosting_debug (const char* buf, size_t nbyte)
{
#if defined(__ARM_ARCH_7M__) || defined(__ARM_ARCH_7EM__)
  // Check if the debugger is enabled. CoreDebug is available only on CM3/CM4.
  // [Contributed by SourceForge user diabolo38]
  if ((CoreDebug->DHCSR & CoreDebug_DHCSR_C_DEBUGEN_Msk) == 0)
    {
      // If not, pretend we wrote all bytes
      return (ssize_t) (nbyte);
    }
#endif // defined(__ARM_ARCH_7M__) || defined(__ARM_ARCH_7EM__)

  // Since the single character debug channel is quite slow, try to
  // optimise and send a null terminated string, if possible.
  if (buf[nbyte] == '\0')
    {
      // send string
      call_host (SEMIHOSTING_SYS_WRITE0, (void*) buf);
    }
  else
    {
      // If not, use a local buffer to speed things up
      char tmp[OS_INTEGER_TRACE_TMP_ARRAY_SIZE];
      size_t togo = nbyte;
      while (togo > 0)
        {
          unsigned int n = ((togo < sizeof(tmp)) ? togo : sizeof(tmp));
          unsigned int i = 0;
          for (; i < n; ++i, ++buf)
            {
              tmp[i] = *buf;
            }
          tmp[i] = '\0';

          call_host (SEMIHOSTING_SYS_WRITE0, (void*) tmp);

          togo -= n;
        }
    }

  // All bytes written
  return (ssize_t) nbyte;
}

#endif // OS_USE_TRACE_SEMIHOSTING_DEBUG

#endif // TRACE

// ----------------------------------------------------------------------------

