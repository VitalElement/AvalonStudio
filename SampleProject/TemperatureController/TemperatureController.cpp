/******************************************************************************
*       Description:
*
*       Author:
*         Date: 01 June 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#pragma mark Includes
#include <math.h>
#include "TemperatureController.h"

#pragma mark Definitions and Constants
static const float PidMinimum = -1.0f;
static const float PidMaximum = 1.0f;
static const float PwmMaximum = 100.0f;
static const float PwmMinimum = -100.0f;
static const float TempMin = 10.0f;
static const float TempMax = 210.0f;
uint8_t myVar = 0;


#pragma mark Static Data


#pragma mark Static Functions


#pragma mark Member Implementations
TemperatureController::TemperatureController (IPwmChannel& pwmChannel,
                                              IAdcChannel& adcChannel,
                                              float beta, float r25, float r2,
                                              float vsupply)
    : pidController (*new PidController (7.5f, 0.05f, 0.25f, 0.225, 1, 1)),
      pwmChannel (pwmChannel), adcChannel (adcChannel),
      temperatureVoltageScaler (*new StraightLineFormula ()),
      inputScaler (*new StraightLineFormula ()),
      outputScaler (*new StraightLineFormula ())
{
    inputScaler.CalculateFrom (TempMin, TempMax, PidMinimum, PidMaximum);
    
    outputScaler.CalculateFrom (PidMinimum, PidMaximum, PwmMinimum, PwmMaximum);

    inputScaler.
    
    inputScaler.CalculateFrom (TempMin, TempMax, PidMaximum, PidMinimum, PidMaximum);
    
    hasReportedTarget = false;

    this->R25 = r25;
    this->Beta = beta;
    this->R2 = r2;
    this->Vin = vsupply;
    this->enabled = true;
    this->isActive = false;
}

TemperatureController::~TemperatureController ()
{
}

void TemperatureController::BeginPreheat (float powerPc)
{
    enabled = false;

    Stop ();

    pwmChannel.SetDutyCycle (powerPc);

    pwmChannel.Start ();
}

bool TemperatureController::IsActive ()
{
    return isActive;
}

void TemperatureController::EndPreheat ()
{
    enabled = true;

    pwmChannel.Stop ();

    Stop ();
}

void TemperatureController::SetTemperature (float value)
{
    if (enabled)
    {
        pidController.SetSetpoint (inputScaler.GetYForX (value));
    }
}

float TemperatureController::GetTemperature ()
{
    return ResistanceToTemperature (
    VoltageToResistance (adcChannel.GetAveragedValue ()));
}

float TemperatureController::GetResistance ()
{
    return VoltageToResistance (adcChannel.GetAveragedValue ());
}

void TemperatureController::InvalidateAdc ()
{
    adcChannel.Update ();
}

void TemperatureController::Start ()
{
    if (enabled)
    {
        isActive = true;
        pwmChannel.Start ();
    }
}

void TemperatureController::Stop ()
{
    isActive = false;
    pwmChannel.Stop ();
}

void TemperatureController::Shutdown ()
{
    Stop ();

    enabled = false;
    isActive = false;
}

void TemperatureController::Invalidate ()
{
    if (enabled)
    {
        auto input = inputScaler.GetYForX (ResistanceToTemperature (
        VoltageToResistance (adcChannel.GetAveragedValue ())));

        auto gain = pidController.CalculateGain (input);

        auto output = outputScaler.GetYForX (gain);

        pwmChannel.SetDutyCycle (output);

        if (!hasReportedTarget && input >= pidController.setpoint)
        {
            EventArgs args;
            TargetReached (this, args);
        }
    }
}

float TemperatureController::VoltageToResistance (float voltage)
{
    return R2 * ((Vin / voltage) - 1);
}

float TemperatureController::ResistanceToTemperature (float resistance)
{
    float steinhart;
    steinhart = resistance / R25; // (R/Ro)


    steinhart = logf (steinhart); // ln(R/Ro)

    steinhart /= Beta;                     // 1/B * ln(R/Ro)
    steinhart += 1.0f / (25.0f + 273.15f); // + (1/To)
    steinhart = 1.0f / steinhart;          // Invert
    steinhart -= 273.15f;                  // convert to C

    return (steinhart);
}