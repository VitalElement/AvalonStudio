/******************************************************************************
*       Description:
*
*       Author:
*         Date: 17 April 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#pragma mark Includes
#include <stdio.h>
#include "Retarget.h"

#pragma mark Definitions and Constants


#pragma mark Static Data
static IPrinter* _printer;

#pragma mark Static Functions
extern "C" {

void _mon_putc (char c)
{
    _printer->Print (c);
}

int _write (int file, char* ptr, int len)
{
    if (_printer != NULL)
    {
        _printer->Print ((char*)ptr, len);
    }

    return len;
}
}

#pragma mark Member Implementations
void Retarget::SetInterface (IPrinter* setInterface)
{
    setbuf (stdout, NULL);
    _printer = setInterface;
}