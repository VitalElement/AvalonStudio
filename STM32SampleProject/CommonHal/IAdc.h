/******************************************************************************
*       Description:
*
*       Author:
*         Date: 29 May 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _IADC_H_
#define _IADC_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>
#include "Event.h"
static const float averageLevel = 1000.0f;

class IAdcChannel
{
#pragma mark Public Members
  public:
    IAdcChannel ()
    {
        voltage = 0;
        averageCount = 0;
        IsInitialised = false;
    }

    /**
     * Gets the current voltage by performing a full measurement.
     */
    virtual float GetVoltage () = 0;

    /**
     * Starts a measurment to be reported via the MeasurementReady event.
     */
    virtual void Start () = 0;

    /**
     * MeansurementReady Event - Triggered when an ADC measurment has completed.
     */
    Event<EventArgs> MeasurementReady;

    float GetAveragedValue ()
    {
        return accumulatedVoltage / averageLevel;
    }

    float GetValue ()
    {
        return voltage;
    }

    void Update ()
    {
        voltage = GetVoltage ();
        
        if (averageCount == 0)
        {
            accumulatedVoltage = voltage * averageLevel;
            averageCount = 1;
            IsInitialised = true;
        }
        else
        {
            accumulatedVoltage -= GetAveragedValue();
            accumulatedVoltage += voltage;
        }
    }

    bool IsInitialised;

#pragma mark Private Members
  private:
    float voltage;
    float accumulatedVoltage;
    uint32_t averageCount;
};

/**
 * IAdc class - ADC Interface
 */
class IAdc
{
#pragma mark Public Members
  public:
    IAdcChannel& GetChannel (uint32_t channel);

#pragma mark Private Members
  private:
};

#endif
