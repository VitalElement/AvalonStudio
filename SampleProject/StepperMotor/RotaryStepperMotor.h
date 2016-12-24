/******************************************************************************
*       Description: 
*
*       Author: 
*         Date: 30 March 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _ROTARYSTEPPERMOTOR_H_
#define _ROTARYSTEPPERMOTOR_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>
#include "StepperMotor.h"

class RotaryStepperMotor : public StepperMotor
{
#pragma mark Public Members
public:

    RotaryStepperMotor (IFrequencyChannel& pwmChannel, const StepperMotorActions& actions,
                  int32_t maximumPosition);
    
    virtual void Goto (int32_t destination);
    
    ~RotaryStepperMotor ();


#pragma mark Private Members
private:
    int32_t Wrap (int32_t value, int32_t lower, int32_t upper);
    int32_t GetShortestPath (uint32_t start, uint32_t destination);
};

#endif
