/******************************************************************************
*       Description:
*
*       Author:
*         Date: 06 May 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _STM32TIMER_H_
#define _STM32TIMER_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>
#include "ITimer.h"
#include "Interrupt.h"
#include "stm32f4xx_hal.h"
#include "STM32InputCaptureChannel.h"
#include "STM32QuadratureEncoder.h"
#include "SignalGeneration/STM32PwmChannel.h"
#include "SignalGeneration/STM32FrequencyChannel.h"


class STM32Timer;

class STM32TimerInterrupt : Interrupt
{
  public:
    STM32TimerInterrupt (STM32Timer* owner, uint32_t interruptNumber);

    void ISR (void);

  private:
    STM32Timer* owner;
};

class STM32Timer : public ITimer
{
    friend class STM32TimerInterrupt;
    friend class STM32InputCaptureChannel;
    friend class STM32FrequencyChannel;
    friend class STM32PwmChannel;
    friend class STM32QuadratureEncoder;

#pragma mark Public Members
  public:
    /*
     * Instantiates a new instance of STM32Timer.
     * Note assumes that the Clock is enabled before the constructor is called.
     * @param handle - a pointer to a timer handle to use.
     * @param interruptNumber - the interrupt number for overflow interrupts.
     * @param clockFrequency - the clock frequency of the timer.
     * @param frequency - the desired counting frequency <= the timers clocking
     * frequency.
     * @param period - the count period where the timer overflows and interrupts
     * are triggered.
     */
    STM32Timer (TIM_HandleTypeDef* handle, uint32_t interruptNumber, uint32_t clockFrequency, float desiredFrequency, uint32_t period);


    ~STM32Timer ();

    void InitialisePWM ();
    void InitialiseOC ();

    /*
     * Gets the counting period of the timer.
     * @return the value of the counting period.
     */
    uint32_t GetPeriod ();

    /*
     * Gets the actual counting frequency of the timer. This maybe different
     * from the requested.
     *
     * @return the counting frequency of the timer.
     */
    float GetFrequency ();

    /*
     * Gets the value of the timer counter.
     * @return 32bit integer of the count value.
     */
    uint32_t GetValue ();

    void Start ();
    void Stop ();

    void ISR ();

#pragma mark Private Members
  private:
    TIM_HandleTypeDef* handle;
    STM32TimerInterrupt interrupt;
    uint32_t clockFrequency;
};

#endif
