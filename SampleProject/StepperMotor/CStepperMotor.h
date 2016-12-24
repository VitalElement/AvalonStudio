/******************************************************************************
*       Description:
*
*       Author:
*         Date: 30 March 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _CSTEPPERMOTOR_H_
#define _CSTEPPERMOTOR_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>
#include "SignalGenerator/CIFrequencyGenerator.h"

#ifdef __cplusplus
extern "C" {
#endif

#pragma mark Module Definitions and Constants
typedef enum
{
    STEPPER_STATE_STOPPED,
    STEPPER_STATE_ACCELERATING,
    STEPPER_STATE_RUNNING,
    STEPPER_STATE_DECELERATING
} CStepperState;

typedef enum
{
    STEPPER_DIRECTION_FORWARD,
    STEPPER_DIRECTION_REVERSE
} CStepperDirection;

typedef struct
{
    CAction Forward;
    CAction Reverse;
    CAction Enable;
    CAction Disable;
} CStepperMotorActions;


typedef struct
{
    CIPwmChannel* pwmChannel;

    CAction MoveCompleted;


    volatile CStepperState state;

    float time;
    float decelTime;
    float frequency;

    float accelerationGradient; // m in y = m.x + c
    float accelerationOffset;   // c ''
    float decelerationGradient; // m in y = m.x + c
    float decelerationOffset;   // c ''

    float accelerationTime;
    float decelerationTime;

    float maxFrequency;

    uint32_t accelerationSteps;
    uint32_t decelerationLimit;
    uint32_t stepLimit;
    int32_t maximum;
    bool fullSpeed;
    uint32_t stepCount;
    int32_t position;

    CStepperDirection direction;

    CStepperMotorActions* actions;
} CStepperMotor;

#pragma mark Module Types


#pragma mark Module Functions
extern void StepperMotor_Initialise (CStepperMotor* motor,
                                     CIPwmChannel* pwmChannel,
                                     CStepperMotorActions* actions,
                                     int32_t maximumPosition);

extern void StepperMotor_SetSpeed (CStepperMotor* motor, float maxFrequency,
                                   float accelerationTime,
                                   float decelerationTime);

extern void StepperMotor_Start (CStepperMotor* motor,
                                CStepperDirection direction);


extern void StepperMotor_Move (CStepperMotor* motor,
                               CStepperDirection direction, uint32_t steps);


extern void StepperMotor_Goto (CStepperMotor* motor, int32_t destination);


#ifdef __cplusplus
}
#endif

#endif
