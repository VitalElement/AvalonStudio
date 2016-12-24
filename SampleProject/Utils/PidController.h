/******************************************************************************
*       Description: PID Controller
*
*       Author: DW
*         Date: 05 May 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _PIDCONTROLLER_H_
#define _PIDCONTROLLER_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>

/**
 * PidController class.
 * Provides standard Proportial Integral Differential closed loop control.
 */
class PidController
{
#pragma mark Public Members
  public:
    /**
     * Contructor
     * Instantiates an instance of PidController.
     * @param proportionalConstant - the value of the proportional constant.
     * @param integralConstant - the value of the integral constant.
     * @param differentialConstant - the value of the differential constant.
     * @param integralLimit - Limits the integral part. Default = 1
     * @param differentialCycle - number of invalidations cycles for
     * differential.
     */
    PidController (float proportionalConstant, float integralConstant,
                   float differentialConstant, float integralLimit,
                   float differentialCycle, float integralWindow);

    /**
     * CalculateGain Method
     * Calculates the gain given a current position.
     * @param position - the current feedback value.
     */
    float CalculateGain (float position);

    /**
     * SetSetPoint Method
     * Sets the target for the PID loop. This method is virtual
     * and can be overidden to scale the values.
     * @param setPoint - the value that the PID loop will target.
     */
    virtual void SetSetpoint (float setPoint);

// private:
#pragma mark Private Members
    float kProportional;
    float kIntegral;
    float kDifferential;
    float integralLimit;
    float integralWindow;
    float proportionalGain;
    float integralGain;
    float differentialGain;
    float setpoint;
    float gain;
    unsigned differentialCycle;
    unsigned differentialCycleCount;
    float errorSum;
    float lastError;
};

#endif
