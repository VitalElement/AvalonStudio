/******************************************************************************
*       Description:
*
*       Author:
*         Date: 15 October 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#pragma mark Includes
#include "ServoController.h"

#pragma mark Definitions and Constants


#pragma mark Static Data


#pragma mark Static Functions


#pragma mark Member Implementations
ServoController::ServoController (IPwmChannel& pwmChannel, float min, float max,
                                  float angularRange)
    : pwmchannel (pwmChannel), scaleFactor (*new StraightLineFormula ())
{
    scaleFactor.CalculateFrom (min, max, 0, angularRange);
}

ServoController::~ServoController ()
{
}

void ServoController::SetAngle (float angle)
{
    pwmchannel.SetDutyCycle (scaleFactor.GetXForY (angle));
}

void ServoController::Start ()
{
    pwmchannel.Start ();
}

void ServoController::Stop ()
{
    pwmchannel.Stop ();
}
