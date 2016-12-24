/******************************************************************************
*       Description:
*
*       Author:
*         Date: 01 June 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _TEMPERATURECONTROLLER_H_
#define _TEMPERATURECONTROLLER_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>
#include "SignalGenerator/IPwmChannel.h"
#include "IAdc.h"
#include "PidController.h"
#include "StraightLineFormula.h"

class TemperatureController
{
#pragma mark Public Members
  public:
    TemperatureController (IPwmChannel& pwmChannel, IAdcChannel& adcChannel,
                           float beta, float r25, float r2, float vsupply);
    ~TemperatureController ();

    void SetTemperature (float value);
    float GetTemperature ();
    float GetResistance ();
    void Invalidate ();
    void InvalidateAdc ();
    void Start ();
    void Stop ();
    void BeginPreheat (float powerPc);
    void EndPreheat ();
    bool IsActive ();

    /**
     * Disables the temperature output permanently.
     */
    void Shutdown ();

    Event<EventArgs> TargetReached;

    PidController& pidController;
#pragma mark Private Members
    // private:
    float ResistanceToTemperature (float resistance);
    float TemperatureToResistance (float temperature);
    float VoltageToResistance (float voltage);
    IPwmChannel& pwmChannel;
    IAdcChannel& adcChannel;

    StraightLineFormula& temperatureVoltageScaler;
    StraightLineFormula& inputScaler;
    StraightLineFormula& outputScaler;
    float R25;
    float Beta;
    float R2;
    float Vin;

    bool isActive;
    bool enabled;
    bool hasReportedTarget;
};

#endif
