/******************************************************************************
*       Description:
*
*       Author:
*         Date: 01 April 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _IPWMCHANNEL_H_
#define _IPWMCHANNEL_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>

class IPwmChannel
{
#pragma mark Public Members
  public:
    IPwmChannel ();
    ~IPwmChannel ();

    virtual void Stop () = 0;
    virtual void Start () = 0;
    virtual void SetDutyCycle (float dutyCyclePc) = 0;
    virtual uint32_t GetTimerValue () = 0;

#pragma mark Private Members
  private:
};

#endif
