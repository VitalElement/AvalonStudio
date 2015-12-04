/******************************************************************************
*       Description:
*
*       Author:
*         Date: 13 November 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _STM32BOOTLOADERSERVICE_H_
#define _STM32BOOTLOADERSERVICE_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>
#include "IBootloaderService.h"

class STM32BootloaderService : public IBootloaderService
{
#pragma mark Public Members
  public:
    STM32BootloaderService ();
    ~STM32BootloaderService ();

    void SetBootloaderFlag ();
    float GetVersion ();
    bool BootloaderPresent ();

    void EraseFirmware ();
    void FlashData (uint32_t address, uint64_t data);
    void FlashData (uint32_t address, uint32_t data);
    void FlashData (uint32_t address, uint16_t data);
    void FlashData (uint32_t address, uint8_t data);

    /**
     * Jumps to application code and never returns unless there is a failure.
     */
    void JumpToApplication ();

    /**
     * Resets the system.
     */
    void SystemReset ();

    void WriteFlags (BootloaderFlags* flags);
    BootloaderFlags* ReadFlags ();


#pragma mark Private Members
  private:  
    void EraseFlags ();
};

#endif
