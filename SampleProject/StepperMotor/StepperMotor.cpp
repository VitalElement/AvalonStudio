/******************************************************************************
*       Description:
*
*       Author:
*         Date: 25 March 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#pragma mark Includes
#include <math.h>
#include <cstdlib>
#include "StepperMotor.h"

#pragma mark Definitions and Constants


#pragma mark Static Data


#pragma mark Static Functions


#pragma mark Member Implementations
StepperMotor::StepperMotor (IFrequencyChannel& frequencyChannel,
                            const StepperMotorActions& actions,
                            int32_t maximumPosition)
    : frequencyChannel (frequencyChannel), actions (actions)
{
    this->frequencyChannel.CycleCompleted += [&](auto sender, auto e)
    {
        this->OnStepped ();
    };

    actions.Disable ();
    actions.Forward ();

    this->position = 0;
    maximum = maximumPosition;
}

StepperMotor::~StepperMotor ()
{
}

void StepperMotor::SetNotificationPosition (int32_t position)
{
    notificationPosition = position;
    notificationsEnabled = true;
}

void StepperMotor::StopNotifications ()
{
    notificationsEnabled = false;
}


void StepperMotor::SetSpeed (float maxFrequency, float accelerationTime,
                             float decelerationTime)
{
    this->accelerationGradient = (maxFrequency) / accelerationTime;
    this->accelerationOffset = 0;

    this->decelerationGradient = ((0 - maxFrequency) / decelerationTime);
    this->decelerationOffset = maxFrequency;

    this->accelerationTime = accelerationTime;

    this->decelerationTime = decelerationTime;

    this->maxFrequency = maxFrequency;
}

void StepperMotor::ResetPosition ()
{
    this->position = 0;
}

void StepperMotor::Goto (int32_t destination)
{
    Direction direction;
    uint32_t steps;

    if (destination == position)
    {
        if (MoveCompleted != nullptr)
        {
            EventArgs args;
            MoveCompleted (this, args);
        }

        return;
    }

    if (destination > position)
    {
        direction = Direction::Forward;
        steps = destination - position;
    }
    else
    {
        direction = Direction::Reverse;
        steps = position - destination;
    }

    Move (direction, steps);
}

void StepperMotor::Move (int32_t steps)
{
    Goto (position + steps);
}

void StepperMotor::Move (Direction direction, uint32_t steps)
{
    if (steps == 0)
    {
        if (MoveCompleted != nullptr)
        {
            EventArgs args;
            MoveCompleted (this, args);
        }

        return; // TODO fire move completed event.
    }

    uint32_t accelerationLimit;

    accelerationSteps = (uint32_t) (0.5f * (maxFrequency * accelerationTime));

    uint32_t decelerationSteps =
    (uint32_t) (0.5f * (maxFrequency * decelerationTime));

    if (accelerationGradient == fabs (decelerationGradient))
    {
        accelerationLimit = steps / 2;
    }
    else
    {
        accelerationLimit =
        (uint32_t) ((steps * fabs (decelerationGradient)) /
                    (accelerationGradient + fabs (decelerationGradient)));
    }

    if (accelerationSteps <= accelerationLimit)
    {
        fullSpeed = true;
        decelerationLimit = steps - decelerationSteps;
    }
    else
    {
        fullSpeed = false;
        // check this.
        decelerationLimit = steps + -(steps - accelerationLimit);
    }

    stepLimit = steps;

    this->direction = direction;
    BeginMove ();
}

float StepperMotor::GetFrequency (float time, float* gradient, float* offset)
{
    return ((*gradient) * time) + *offset;
}

float StepperMotor::GetTime (float frequency, float* gradient, float* offset)
{
    return (frequency - *offset) / *gradient;
}

void StepperMotor::Stop ()
{
    decelerationLimit = stepCount;

    uint32_t decelerationSteps =
    (uint32_t) (0.5f * (maxFrequency * decelerationTime)) + stepCount;

    stepLimit = stepCount + decelerationSteps;
}

void StepperMotor::BeginMove ()
{
    time = sqrtf ((2.0f / accelerationGradient));
    decelTime = 0.0f;

    frequencyChannel.SetFrequency (
    GetFrequency (time, &accelerationGradient, &accelerationOffset));

    stepCount = 0;

    time +=
    1.0f / GetFrequency (time, &accelerationGradient, &accelerationOffset);

    state = State::Accelerating;

    switch (direction)
    {
    case Direction::Forward:
        actions.Forward ();
        break;

    case Direction::Reverse:
        actions.Reverse ();
        break;
    }

    actions.Enable ();

    frequencyChannel.Start ();
}

void StepperMotor::Start (Direction direction)
{
    this->direction = direction;

    accelerationSteps = (uint32_t) (0.5f * (maxFrequency * accelerationTime));

    decelerationLimit = -1;
    stepLimit = -1;

    BeginMove ();
}

void StepperMotor::OnStepped ()
{
    stepCount++;

    if (stepCount > decelerationLimit)
    {
        if (state != State::Decelerating)
        {
            state = State::Decelerating;

            if (!fullSpeed)
            {
                decelTime =
                GetTime (frequency, &decelerationGradient, &decelerationOffset);
            }
        }

        // we are decelerating
        frequency =
        GetFrequency (decelTime, &decelerationGradient, &decelerationOffset);

        if (stepCount == stepLimit || frequency <= 0)
        {
            frequencyChannel.Stop ();

            actions.Disable ();
            state = State::Stopped;

            if (MoveCompleted != nullptr)
            {
                EventArgs args;
                MoveCompleted (this, args);
            }
        }

        decelTime += 1.0f / frequency;
    }
    else if (stepCount < accelerationSteps)
    {
        // we are accelerating.
        frequency =
        GetFrequency (time, &accelerationGradient, &accelerationOffset);

        time += 1.0f / frequency;
    }
    else
    {
        if (state != State::Running)
        {
            state = State::Running;
        }

        // we are at constant speed.
        frequency =
        GetFrequency (time, &accelerationGradient, &accelerationOffset);
    }

    this->frequencyChannel.SetFrequency (frequency);

    switch (direction)
    {
    case Direction::Forward:
        if (position == maximum)
        {
            position = 0;
        }

        position++;
        break;

    case Direction::Reverse:
        position--;

        if (position == -1)
        {
            position = maximum;
        }
        break;
    }

    if (notificationsEnabled && position == notificationPosition &&
        NotifyPosition != nullptr)
    {
        EventArgs args;

        NotifyPosition (this, args);
    }
}
