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
#include "CStepperMotor.h"


#pragma mark Definitions and Constants


#pragma mark Static Data


#pragma mark Static Functions
float GetFrequency (float time, float* gradient, float* offset)
{
    return ((*gradient) * time) + *offset;
}

float GetTime (float frequency, float* gradient, float* offset)
{
    return (frequency - *offset) / *gradient;
}


static void StepperMotor_OnStepped (void* object)
{
    CStepperMotor* motor = (CStepperMotor*)object;

    motor->stepCount++;

    if (motor->stepCount > motor->decelerationLimit)
    {
        if (motor->state != STEPPER_STATE_DECELERATING)
        {
            motor->state = STEPPER_STATE_DECELERATING;

            if (!motor->fullSpeed)
            {
                motor->decelTime =
                GetTime (motor->frequency, &motor->decelerationGradient,
                         &motor->decelerationOffset);
            }
        }

        // we are decelerating
        motor->frequency =
        GetFrequency (motor->decelTime, &motor->decelerationGradient,
                      &motor->decelerationOffset);

        motor->decelTime += 1.0f / motor->frequency;

        if (motor->stepCount == motor->stepLimit)
        {
            motor->pwmChannel->Stop ();

            motor->actions->Disable ();
            motor->state = STEPPER_STATE_STOPPED;

            motor->MoveCompleted ();
        }
    }
    else if (motor->stepCount < motor->accelerationSteps)
    {
        // we are accelerating.
        motor->frequency = GetFrequency (
        motor->time, &motor->accelerationGradient, &motor->accelerationOffset);

        motor->time += 1.0f / motor->frequency;
    }
    else
    {
        if (motor->state != STEPPER_STATE_RUNNING)
        {
            motor->state = STEPPER_STATE_RUNNING;
        }

        // we are at constant speed.
        motor->frequency =
        GetFrequency (motor->time, &motor->accelerationGradient, &motor->accelerationOffset);
    }

    motor->pwmChannel->SetFrequency (motor->frequency);

    switch (motor->direction)
    {
        case STEPPER_DIRECTION_FORWARD:
        motor->position++;

        if (motor->position == motor->maximum)
        {
            motor->position = 0;
        }
        break;

        case STEPPER_DIRECTION_REVERSE:
        motor->position--;

        if (motor->position == -1)
        {
            motor->position = motor->maximum;
        }
        break;
    }
}


#pragma mark Function Implementations
void StepperMotor_Initialise (CStepperMotor* motor, CIPwmChannel* pwmChannel,
                              CStepperMotorActions* actions,
                              int32_t maximumPosition)
{
    motor->pwmChannel = pwmChannel;
    motor->pwmChannel->CycleCompleted = StepperMotor_OnStepped;

    motor->actions = actions;
    actions->Disable ();
    actions->Forward ();

    motor->position = 0;

    motor->maximum = maximumPosition;
}

void StepperMotor_SetSpeed (CStepperMotor* motor, float maxFrequency,
                            float accelerationTime, float decelerationTime)
{
    motor->accelerationGradient = (maxFrequency) / accelerationTime;
    motor->accelerationOffset = 0;

    motor->decelerationGradient = ((0 - maxFrequency) / decelerationTime);
    motor->decelerationOffset = maxFrequency;

    motor->accelerationTime = accelerationTime;

    motor->decelerationTime = decelerationTime;

    motor->maxFrequency = maxFrequency;
}

void StepperMotor_Start (CStepperMotor* motor, CStepperDirection direction)
{
    motor->direction = direction;

    motor->time = sqrtf ((2.0f / motor->accelerationGradient));
    motor->decelTime = 0.0f;

    motor->pwmChannel->SetFrequency (GetFrequency (
    motor->time, &motor->accelerationGradient, &motor->accelerationOffset));

    motor->stepCount = 0;

    motor->time +=
    1.0f / GetFrequency (motor->time, &motor->accelerationGradient,
                         &motor->accelerationOffset);

    motor->state = STEPPER_STATE_ACCELERATING;

    switch (motor->direction)
    {
    case STEPPER_DIRECTION_FORWARD:
        motor->actions->Forward ();
        break;

    case STEPPER_DIRECTION_REVERSE:
        motor->actions->Reverse ();
        break;
    }

    motor->actions->Enable ();

    motor->pwmChannel->Start ();
}


void StepperMotor_Move (CStepperMotor* motor, CStepperDirection direction,
                        uint32_t steps)
{
    uint32_t accelerationLimit;

    motor->accelerationSteps =
    (uint32_t)(0.5f * (motor->maxFrequency * motor->accelerationTime));

    uint32_t decelerationSteps =
    (uint32_t)(0.5f * (motor->maxFrequency * motor->decelerationTime));

    if (motor->accelerationGradient == fabs (motor->decelerationGradient))
    {
        accelerationLimit = steps / 2;
    }
    else
    {
        accelerationLimit = (uint32_t)(
        (steps * fabs (motor->decelerationGradient)) /
        (motor->accelerationGradient + fabs (motor->decelerationGradient)));
    }

    if (motor->accelerationSteps <= accelerationLimit)
    {
        motor->fullSpeed = true;
        motor->decelerationLimit = steps - decelerationSteps;
    }
    else
    {
        motor->fullSpeed = false;
        // check this.
        motor->decelerationLimit = steps + -(steps - accelerationLimit);
    }

    motor->stepLimit = steps;

    StepperMotor_Start (motor, direction);
}


void StepperMotor_Goto (CStepperMotor* motor, int32_t destination)
{
    CStepperDirection direction;
    uint32_t steps;

    if (destination == motor->position)
    {
        return;
    }

    if (destination > motor->position)
    {
        direction = STEPPER_DIRECTION_FORWARD;
        steps = destination - motor->position;
    }
    else
    {
        direction = STEPPER_DIRECTION_REVERSE;
        steps = motor->position - destination;
    }

    StepperMotor_Move (motor, direction, steps);
}