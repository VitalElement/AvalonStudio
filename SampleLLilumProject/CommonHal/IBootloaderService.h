/******************************************************************************
*       Description:
*
*       Author:
*         Date: 13 November 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _IBOOTLOADERSERVICE_H_
#define _IBOOTLOADERSERVICE_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>

const uint32_t BootLoaderSignature = 0x10AD10CC; // LOADLOCK

enum class BootloaderState
{
    Normal,
    Bootloader,
    IntegrityCheck
};

class BootloaderFlags
{
  public:
    BootloaderFlags ()
    {
        Signature = BootLoaderSignature;
        State = BootloaderState::Bootloader;
        ImageLength = 0;
        ImageCRC = 0;
    }

    BootloaderState State;
    uint32_t ImageLength;
    uint32_t ImageCRC;
    float Version;
    uint32_t Signature;

    bool IsBootloaderPresent ()
    {
        return Signature == BootLoaderSignature;
    }
};

class IBootloaderService
{
#pragma mark Public Members
  public:
    virtual float GetVersion () = 0;
    virtual bool BootloaderPresent () = 0;

    virtual void EraseFirmware () = 0;

    virtual void FlashData (uint32_t address, uint64_t data) = 0;
    virtual void FlashData (uint32_t address, uint32_t data) = 0;
    virtual void FlashData (uint32_t address, uint16_t data) = 0;
    virtual void FlashData (uint32_t address, uint8_t data) = 0;


    virtual void WriteFlags (BootloaderFlags* flags) = 0;
    virtual BootloaderFlags* ReadFlags () = 0;

    virtual void JumpToApplication () = 0;
    virtual void SystemReset () = 0;

    void SetBootloaderFlag ()
    {
        auto flags = ReadFlags ();

        flags->State = BootloaderState::Bootloader;

        WriteFlags (flags);        
    }


#pragma mark Private Members
  private:
};

#endif
