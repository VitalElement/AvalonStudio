/******************************************************************************
*       Description:
*
*       Author:
*         Date: 13 November 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#pragma mark Includes
#include "STM32BootloaderService.h"
#include "stm32f4xx_hal.h"

#pragma mark Definitions and Constants
static const uint32_t ApplicationAddress = 0x0800C000;
typedef void (*Function) (void);

BootloaderFlags* Flags = (BootloaderFlags*)0x08008000;

#pragma mark Static Data


#pragma mark Static Functions


#pragma mark Member Implementations
STM32BootloaderService::STM32BootloaderService ()
{
}

STM32BootloaderService::~STM32BootloaderService ()
{
}

void STM32BootloaderService::SetBootloaderFlag ()
{
    auto flags = *ReadFlags ();

    flags.State = BootloaderState::Bootloader;

    WriteFlags (&flags);
}

float STM32BootloaderService::GetVersion ()
{
    return Flags->Version;
}

bool STM32BootloaderService::BootloaderPresent ()
{
    return Flags->Signature == BootLoaderSignature;
}

void STM32BootloaderService::SystemReset ()
{
    HAL_RCC_DeInit ();
    SysTick->CTRL = 0;
    SysTick->LOAD = 0;
    SysTick->VAL = 0;

    HAL_NVIC_SystemReset ();
}

void STM32BootloaderService::EraseFirmware ()
{
    HAL_FLASH_Unlock ();

    __HAL_FLASH_CLEAR_FLAG (FLASH_FLAG_EOP | FLASH_FLAG_OPERR |
                            FLASH_FLAG_WRPERR | FLASH_FLAG_PGAERR |
                            FLASH_FLAG_PGSERR | FLASH_FLAG_PGPERR);

    FLASH_Erase_Sector (FLASH_SECTOR_3, FLASH_VOLTAGE_RANGE_3);

    FLASH_WaitForLastOperation (10000);

    FLASH_Erase_Sector (FLASH_SECTOR_4, FLASH_VOLTAGE_RANGE_3);

    FLASH_WaitForLastOperation (10000);

    FLASH_Erase_Sector (FLASH_SECTOR_5, FLASH_VOLTAGE_RANGE_3);

    FLASH_WaitForLastOperation (10000);

    FLASH_Erase_Sector (FLASH_SECTOR_6, FLASH_VOLTAGE_RANGE_3);
    FLASH_WaitForLastOperation (10000);
}

void STM32BootloaderService::FlashData (uint32_t address, uint64_t data)
{
    uint32_t* ptr = (uint32_t*)&data;

    HAL_FLASH_Program (TYPEPROGRAM_WORD, address, *ptr++);
    FLASH_WaitForLastOperation (10000);
    HAL_FLASH_Program (TYPEPROGRAM_WORD, address, *ptr);
    FLASH_WaitForLastOperation (10000);
}

void STM32BootloaderService::FlashData (uint32_t address, uint32_t data)
{
    HAL_FLASH_Program (TYPEPROGRAM_WORD, address, data);
    FLASH_WaitForLastOperation (10000);
}

void STM32BootloaderService::FlashData (uint32_t address, uint16_t data)
{
    HAL_FLASH_Program (TYPEPROGRAM_HALFWORD, address, data);
    FLASH_WaitForLastOperation (10000);
}

void STM32BootloaderService::FlashData (uint32_t address, uint8_t data)
{
    HAL_FLASH_Program (TYPEPROGRAM_BYTE, address, data);
    FLASH_WaitForLastOperation (10000);
}


void STM32BootloaderService::JumpToApplication ()
{
    HAL_RCC_DeInit ();
    SysTick->CTRL = 0;
    SysTick->LOAD = 0;
    SysTick->VAL = 0;

    auto initsp =
    ((*(__IO uint32_t*)ApplicationAddress)); // read stack pointer.

    if (((initsp & 3) == 0) &&
        initsp >= 0x20000000 && initsp <= 0x2FFE0000) // check for a valid stack pointer.
    {
        SCB->VTOR =
        FLASH_BASE | 0xC000; // set vector table offset before jumping.

        /* Jump to user application */
        static auto jumpToApplication =
        (Function) * (__IO uint32_t*)(ApplicationAddress + 4);

        /* Initialize user application's Stack Pointer */
        __set_MSP (*(__IO uint32_t*)ApplicationAddress);

        jumpToApplication ();
    }
}

void STM32BootloaderService::EraseFlags ()
{
    __HAL_FLASH_CLEAR_FLAG (FLASH_FLAG_EOP | FLASH_FLAG_OPERR |
                            FLASH_FLAG_WRPERR | FLASH_FLAG_PGAERR |
                            FLASH_FLAG_PGSERR | FLASH_FLAG_PGPERR);

    FLASH_Erase_Sector (FLASH_SECTOR_2, FLASH_VOLTAGE_RANGE_3);

    FLASH_WaitForLastOperation (10000);
}

void STM32BootloaderService::WriteFlags (BootloaderFlags* flags)
{
    HAL_FLASH_Unlock ();

    __HAL_FLASH_CLEAR_FLAG (FLASH_FLAG_EOP | FLASH_FLAG_OPERR |
                            FLASH_FLAG_WRPERR | FLASH_FLAG_PGAERR |
                            FLASH_FLAG_PGSERR |FLASH_FLAG_PGPERR);

    EraseFlags ();

    __HAL_FLASH_CLEAR_FLAG (FLASH_FLAG_EOP | FLASH_FLAG_OPERR |
                            FLASH_FLAG_WRPERR | FLASH_FLAG_PGAERR |
                            FLASH_FLAG_PGSERR | FLASH_FLAG_PGPERR);

    uint8_t* src = (uint8_t*)flags;
    uint8_t* destination = (uint8_t*)Flags;

    for (uint32_t index = 0; index < sizeof (BootloaderFlags); index++)
    {
        FlashData ((uint32_t)destination++, *src++);
    }

    HAL_FLASH_Lock ();
}

BootloaderFlags* STM32BootloaderService::ReadFlags ()
{
    return Flags;
}
