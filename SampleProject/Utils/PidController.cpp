/******************************************************************************
*       Description:
*
*       Author:
*         Date: 05 May 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#pragma mark Includes
#include "PidController.h"
#include <cmath>

#pragma mark Definitions and Constants


#pragma mark Static Data


#pragma mark Static Functions


#pragma mark Member Implementations
PidController::PidController (float kProportional, float kIntegral,
                              float kDifferential, float integralLimit,
                              float differentialCycle, float integralWindow)
{
    this->kProportional = kProportional;
    this->kIntegral = kIntegral;
    this->kDifferential = kDifferential;
    this->integralLimit = integralLimit;
    this->integralWindow = integralWindow;
    this->differentialCycle = differentialCycle;

    this->proportionalGain = 0;
    this->integralGain = 0;
    this->differentialGain = 0;

    this->setpoint = 0;
    this->gain = 0;
    this->differentialCycleCount = 0;
    this->errorSum = 0;
    this->lastError = 0;
}

void PidController::SetSetpoint (float setpoint)
{
    if (this->setpoint != setpoint)
    {
        errorSum = 0;
        this->setpoint = setpoint;
    }
}

float PidController::CalculateGain (float position)
{
    float error = setpoint - position;

    proportionalGain = kProportional * error;

    if (fabs (error) < integralWindow)
    {
        errorSum += error;
        integralGain = kIntegral * errorSum;

        if (fabs (integralGain) > integralLimit)
        {
            if (integralGain > 0)
            {
                integralGain = integralLimit;
            }
            else
            {
                integralGain = -integralLimit;
            }

            errorSum -= error;
        }
    }

    if (++differentialCycleCount >= differentialCycle)
    {
        differentialGain = kDifferential * (error - lastError);
        differentialCycleCount = 0;
    }

    lastError = error;

    gain = proportionalGain + integralGain + differentialGain;

    return gain;
}
