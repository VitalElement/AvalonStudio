/******************************************************************************
*       Description: 
*
*       Author: 
*         Date: 30 April 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _PINPACKS_H_
#define _PINPACKS_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>

enum class TimerPinsPack
{
    PinsPack1,
    PinsPack2,
    PinsPack3
};


enum class SpiPinsPack
{
    PinsPack1,
    PinsPack2,
    PinsPack3,
    PinsPackCustom
};

#endif
