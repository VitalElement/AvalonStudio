/******************************************************************************
*       Description:
*
*       Author:
*         Date: 08 April 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#pragma mark Includes
#include <math.h>
#include "DCMotor.h"

#pragma mark Definitions and Constants
static const float PidMinimum = 0.0f;
static const float PidMaximum = 1.0f;
static const float PwmMinimum = 0.0f;
static const float PwmMaximum = 100.0f;
static const float FreqMinimum = 0.0f;
static const float FreqMaximum = 61000.0f;
static const float PositionMinimum = 0.0f;


#pragma mark Static Data


#pragma mark Static Functions


#pragma mark Member Implementations
DCMotorControlLoop::DCMotorControlLoop (
IPwmChannel& pwmChannel, DCMotorActions& actions,
IInputCaptureChannel& inputCaptureChannel)
    : DCMotor (pwmChannel, actions),
      pidController (*new PidController (0.02f, 0.11f, 0.2f, 1.0f, 0, 1.0)),
      inputCaptureChannel (inputCaptureChannel),
      inputScaler (*new StraightLineFormula ()),
      outputScaler (*new StraightLineFormula ())
{
    dutyCycle = 0;
    speed = 0;

    inputScaler.CalculateFrom (FreqMinimum, FreqMaximum, PidMinimum,
                               PidMaximum);

    outputScaler.CalculateFrom (PidMinimum, PidMaximum, PwmMinimum, PwmMaximum);
}

void DCMotorControlLoop::SetSpeed (float countsPerSecond)
{
    pidController.SetSetpoint (inputScaler.GetYForX (countsPerSecond));
}

float DCMotorControlLoop::GetSpeed ()
{
    return inputCaptureChannel.GetFrequency ();
}

void DCMotorControlLoop::Invalidate ()
{
    speed = inputCaptureChannel.GetFrequency ();

    auto gain = pidController.CalculateGain (inputScaler.GetYForX (speed));

    auto output = outputScaler.GetYForX (gain);

    SetPower (output);
}


DCMotorPositionControlLoop::DCMotorPositionControlLoop (
IPwmChannel& pwmChannel, const DCMotorActions& actions,
IInputCaptureChannel& inputCaptureChannel)
    : DCMotor (pwmChannel, actions), pidController (*new PidController (
                                     75.0f, 0.000125, 25.5f, 0.45f, 0, 0.25)),
      inputCaptureChannel (inputCaptureChannel)
{
    inputScaler.CalculateFrom (PositionMinimum, FreqMaximum, PidMinimum,
                               PidMaximum);

    outputScaler.CalculateFrom (PidMinimum, PidMaximum, PwmMinimum, PwmMaximum);

    this->inputCaptureChannel.GetCapture ();

    position = 0;
    enabled = false;
    EnablePidControl = true;

    inputCaptureChannel.Captured += [this](auto sender, auto e)
    {
        switch (direction)
        {
        case Direction::Forward:
            position++;
            break;

        case Direction::Reverse:
            position--;
            break;
        }
    };
}

void DCMotorPositionControlLoop::SetPosition (int32_t position)
{
    TargetPosition = position;
    pidController.SetSetpoint (inputScaler.GetYForX (position));
}

int32_t DCMotorPositionControlLoop::GetPosition ()
{
    return position;
}

void DCMotorPositionControlLoop::Invalidate ()
{
    if (enabled && EnablePidControl)
    {
        auto position = GetPosition ();
        auto input = inputScaler.GetYForX (position);

        auto gain = pidController.CalculateGain (input);

        power = outputScaler.GetYForX (gain);

        if (power >= 0)
        {
            SetDirection (DCMotor::Direction::Forward);
        }
        else
        {
            SetDirection (DCMotor::Direction::Reverse);
        }

        SetPower (fabs (power));

        if (position != lastPosition && PositionChanged != nullptr)
        {
            lastPosition = position;
            EventArgs args;
            PositionChanged (this, args);
        }
    }
}


DCMotor::DCMotor (IPwmChannel& pwmChannel, const DCMotorActions& actions)
    : pwmChannel (pwmChannel), actions (actions)
{
    pwmChannel.SetDutyCycle (0);

    SetDirection (Direction::Forward);
}

DCMotor::~DCMotor ()
{
}

void DCMotor::Start ()
{
    pwmChannel.Start ();

    if (actions.Enable != nullptr)
    {
        actions.Enable ();
    }

    enabled = true;
}

void DCMotor::Stop ()
{
    pwmChannel.Stop ();

    if (actions.Disable != nullptr)
    {
        actions.Disable ();
    }
    
    enabled = false;
}

void DCMotor::SetPower (float powerPc)
{
    pwmChannel.SetDutyCycle (powerPc);
}

void DCMotor::SetDirection (DCMotor::Direction direction)
{
    switch (direction)
    {
    case Direction::Forward:
        actions.Forward ();
        break;

    case Direction::Reverse:
        actions.Reverse ();
        break;
    }

    this->direction = direction;
}
