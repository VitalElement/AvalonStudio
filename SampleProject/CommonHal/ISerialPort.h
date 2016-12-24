/******************************************************************************
*       Description: 
*
*       Author: 
*         Date: 26 May 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _ISERIALPORT_H_
#define _ISERIALPORT_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>
#include "IUart.h"

class ISerialPort : public IUart
{
#pragma mark Public Members
public:
    void Send (const char* string);    
    void Send (uint8_t* data, uint32_t length);
    void Send (uint8_t data);
    
    uint8_t Receive (void);

#pragma mark Private Members
private:

};

#endif
