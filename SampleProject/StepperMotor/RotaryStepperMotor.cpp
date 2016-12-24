/******************************************************************************
*       Description:
*
*       Author:
*         Date: 30 March 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#pragma mark Includes
#include <math.h>
#include <cstdlib>
#include "RotaryStepperMotor.h"

#pragma mark Definitions and Constants


#pragma mark Static Data


#pragma mark Static Functions


#pragma mark Member Implementations
RotaryStepperMotor::RotaryStepperMotor (IFrequencyChannel& pwmChannel,
                                        const StepperMotorActions& actions,
                                        int32_t maximumPosition)
    : StepperMotor (pwmChannel, actions, maximumPosition)
{
}

RotaryStepperMotor::~RotaryStepperMotor ()
{
}

void RotaryStepperMotor::Goto (int32_t destination)
{
    Direction direction;
    int32_t route;

    if (destination == position)
    {
        return;
    }

    route = GetShortestPath (position, destination);

    if (route > 0)
    {
        direction = Direction::Forward;
    }
    else
    {
        direction = Direction::Reverse;
    }

    Move (direction, abs (route));
}

int32_t RotaryStepperMotor::Wrap (int32_t value, int32_t lower, int32_t upper)
{
    int32_t distance = upper - lower;

    return value - (((value - lower) / distance) * distance);
}

int32_t RotaryStepperMotor::GetShortestPath (uint32_t start,
                                             uint32_t destination)
{
    int32_t diff = 0;

    int32_t midPoint = maximum / 2;

    diff = (int32_t)destination - (int32_t)start;

    if (destination > start)
    {
        return Wrap (diff, -midPoint, midPoint);
    }
    else
    {
        return Wrap (diff, midPoint, -midPoint);
    }
}
