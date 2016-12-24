/******************************************************************************
*       Description: DC Motor Control
*
*       Author: DW
*         Date: 08 April 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _DCMOTOR_H_
#define _DCMOTOR_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>
#include "SignalGenerator/IPwmChannel.h"
#include "IQuadratureEncoder.h"
#include "IInputCaptureChannel.h"
#include "PidController.h"
#include "Action.h"
#include "StraightLineFormula.h"

/**
 * Actions class for DCMotor support.
 * Provides actions to assert GPIO ton control direction and enable / disable
 * the motor.
 */
class DCMotorActions
{
  public:
    Action Forward;
    Action Reverse;
    Action Enable;
    Action Disable;
};

/**
 * DCMotor class
 * Provides basic control of a DCMotor.
 */
class DCMotor
{
#pragma mark Public Members
  public:
    /**
     * Direction enum
     * Provides enumeration value to control direction of DCMotors.
     */
    enum class Direction
    {
        Forward,
        Reverse
    };

    /**
     * Contructor
     * Instantiates a instance of DCMotor.
     * @param pwmChannel - reference to a pwm channel that controls the DC
     * Motor.
     * @param actions - reference to a DCMotorActions object to provide low
     * level manipulation of hardware.
     */
    DCMotor (IPwmChannel& pwmChannel, const DCMotorActions& actions);

    /**
     * Destructor
     * Cleans up resources used by the instance of DCMotor.
     */
    ~DCMotor ();

    /**
     * Start method
     * Starts the motor at the currently set power level. This will cause the
     * motor to be enabled
     * and for the PWM signal to begin.
     */
    void Start ();

    /**
     * Stop method
     * Stops the motor, by stopping the PWM signal and then disabling the motor.
     */
    void Stop ();

    /**
     * SetDirection method
     * Sets the direction of the motor. If the motor is currently running this
     * will
     * instantly change the direction of the motor.
     * @param direction - the direction to change to.
     */
    void SetDirection (Direction direction);

    /**
     * SetPower method
     * Sets the power to the motor.
     * @param powerPc - the power as a percentage.
     */
    void SetPower (float powerPc);

  protected:
    bool enabled;
    volatile Direction direction;

#pragma mark Private Members
  private:
    IPwmChannel& pwmChannel;
    const DCMotorActions& actions;
};


class DCMotorPositionControlLoop : public DCMotor
{
  public:
    DCMotorPositionControlLoop (IPwmChannel& pwmChannel,
                                const DCMotorActions& actions,
                                IInputCaptureChannel& inputCaptureChannel);

    void SetPosition (int32_t position);

    int32_t GetPosition ();

    void Invalidate ();

    PidController& pidController;

    volatile int32_t position;
    int32_t TargetPosition;
    float count;
    float power;
    bool EnablePidControl;
    int32_t lastPosition;
    Event<EventArgs> PositionChanged;

  private:
    IInputCaptureChannel& inputCaptureChannel;

    StraightLineFormula inputScaler;
    StraightLineFormula outputScaler;
};


/**
 * DCMotor control loop class.
 * A DCMotor with feedback loop to regulate velocity. This requires a clocking
 * signal
 * usually provided via an encoder channel.
 */
class DCMotorControlLoop : public DCMotor
{
  public:
    /**
     * Contructor
     * Instantiates an instance of DCMotorControlLoop.
     * @param pwmChannel - reference to a pwm channel that controls the DC
     * Motor.
     * @param actions - reference to a DCMotorActions object to provide low
     * level manipulation of hardware.
     * @param inputCaptureChannel - a reference to the inputCaptureChannel
     * connected to the
     * motors encoder shaft.
     */
    DCMotorControlLoop (IPwmChannel& pwmChannel, DCMotorActions& actions,
                        IInputCaptureChannel& inputCaptureChannel);

    /**
     * SetSpeed Method
     * Sets the regulated speed of the motor.
     * @param countsPerSecond - the regulated speed in Hz representing the
     * encoder channel frequency.
     */
    void SetSpeed (float countsPerSecond);

    /**
     * GetSpeed Method
     * Gets the latest measued speed.
     */
    float GetSpeed ();

    /**
     * Invalidate
     * Invalidates the PID loop of the DCMotor. This method should be called at
     * equal intervals.
     */
    void Invalidate ();


    PidController& pidController;
    float count;

    // private:
    IInputCaptureChannel& inputCaptureChannel;
    float speed;
    float dutyCycle;
    StraightLineFormula& inputScaler;
    StraightLineFormula& outputScaler;
};

#endif
