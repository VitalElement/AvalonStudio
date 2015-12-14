/******************************************************************************
*       Description:
*
*       Author:
*         Date: 25 March 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _STEPPERMOTOR_H_
#define _STEPPERMOTOR_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>
#include "SignalGenerator/IFrequencyChannel.h"
#include "Event.h"
#include "Action.h"

class StepperMotorActions
{
  public:
    Action Forward;
    Action Reverse;
    Action Enable;
    Action Disable;
};

class StepperMotor
{
#pragma mark Public Members
  public:
    enum class State
    {
        Stopped,
        Accelerating,
        Running,
        Decelerating
    };

    enum class Direction
    {
        Forward,
        Reverse
    };


    StepperMotor (IFrequencyChannel& frequencyChannel,
                  const StepperMotorActions& actions, int32_t maximumPosition);

    ~StepperMotor ();

    void SetSpeed (float maxFrequency, float accelerationTime,
                   float decelerationTime);

    void Start (Direction direction);

    void Stop ();

    virtual void Goto (int32_t destination);

    void Move (int32_t steps);
    void Move (Direction direction, uint32_t steps);

    Event<EventArgs> MoveCompleted;
    Event<EventArgs> NotifyPosition;

    void ResetPosition ();
    void SetNotificationPosition (int32_t position);
    void StopNotifications ();

    volatile State state;

    Direction direction;

    int32_t position;
    float frequency;

#pragma mark Private Members
  protected:
    void BeginMove ();
    IFrequencyChannel& frequencyChannel;
    void OnStepped ();

    float GetFrequency (float time, float* gradient, float* offset);
    float GetTime (float frequency, float* gradient, float* offset);


    float time;
    float decelTime;

    float accelerationGradient; // m in y = m.x + c
    float accelerationOffset;   // c ''
    float decelerationGradient; // m in y = m.x + c
    float decelerationOffset;   // c ''

    float accelerationTime;
    float decelerationTime;

    float maxFrequency;

    bool notificationsEnabled;
    int32_t notificationPosition;

    uint32_t accelerationSteps;
    uint32_t decelerationLimit;
    uint32_t stepLimit;
    int32_t maximum;
    bool fullSpeed;
    uint32_t stepCount;
    const StepperMotorActions& actions;
};

#endif
