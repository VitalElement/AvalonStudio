/******************************************************************************
*       Description:
*
*       Author:
*         Date: January 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _CRC_H_
#define _CRC_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>

class CRC
{
#pragma mark Public Members
  public:
    static int16_t Crc16 (int16_t old_crc, int8_t data)
    {
        int16_t crc;
        int16_t x;

        x = ((old_crc>>8) ^ data) & 0xff;
        x ^= x >> 4;

        crc = (old_crc << 8) ^ (x << 12) ^ (x << 5) ^ x;

        // crc &= 0xffff;      // enable this line for processors with more than
        // 16 bits

        return crc;
    }


#pragma mark Private Members
  private:
};

#endif
