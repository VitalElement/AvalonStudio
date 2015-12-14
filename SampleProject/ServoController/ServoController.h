/******************************************************************************
*       Description:
*
*       Author:
*         Date: 15 October 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _SERVOCONTROLLER_H_
#define _SERVOCONTROLLER_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>
#include "SignalGenerator/IPwmChannel.h"
#include "StraightLineFormula.h"

class ServoController
{
#pragma mark Public Members
  public:
    ServoController (IPwmChannel& pwmChannel, float min, float max,
                     float angularRange);
    ~ServoController ();

    void SetAngle (float angle);

    void Start ();
    void Stop ();

#pragma mark Private Members
  private:
    IPwmChannel& pwmchannel;
    StraightLineFormula& scaleFactor;
};

#endif
