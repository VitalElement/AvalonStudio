#include "mbed_helpers.h"
#include "llos_pwm.h"
#include "llos_memory.h"

extern "C"
{
    typedef struct LLOS_MbedPwm
    {
        uint32_t PinName;
        uint32_t Period;
        uint32_t PulseWidth;
        float DutyCycle;
        pwmout_t Pwm;
    } LLOS_MbedPwm;

    HRESULT LLOS_PWM_Initialize(uint32_t pinName, LLOS_Context* pChannel)
    {
        LLOS_MbedPwm *pPwm;

        if (pChannel == NULL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        pPwm = (LLOS_MbedPwm*)AllocateFromManagedHeap(sizeof(LLOS_MbedPwm));

        if (pPwm == NULL)
        {
            return LLOS_E_OUT_OF_MEMORY;
        }

        pPwm->PinName = pinName;
        pPwm->Period = 20000; // 20ms
        pPwm->DutyCycle = 0.5;
        pPwm->PulseWidth = 0;
        pwmout_init(&pPwm->Pwm, (PinName)pinName);

        *pChannel = pPwm;

        return S_OK;
    }

    VOID LLOS_PWM_Uninitialize(LLOS_Context channel)
    {
        LLOS_MbedPwm *pPwm = (LLOS_MbedPwm*)channel;
        pwmout_free(&pPwm->Pwm);
        FreeFromManagedHeap(channel);
    }

    HRESULT LLOS_PWM_SetDutyCycle(LLOS_Context channel, uint32_t dutyCycleNumerator, uint32_t dutyCycleDenominator)
    {
        LLOS_MbedPwm *pPwm = (LLOS_MbedPwm*)channel;

        if (pPwm == NULL || dutyCycleDenominator == 0)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        pPwm->DutyCycle = (float)dutyCycleNumerator/(float)dutyCycleDenominator;
        pwmout_write(&pPwm->Pwm, pPwm->DutyCycle);

        return S_OK;
    }

    HRESULT LLOS_PWM_SetPeriod(LLOS_Context channel, uint32_t periodMicroSeconds)
    {
        LLOS_MbedPwm *pPwm = (LLOS_MbedPwm*)channel;

        if (pPwm == NULL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        pPwm->Period = periodMicroSeconds;
        pwmout_period_us(&pPwm->Pwm, periodMicroSeconds);

        return S_OK;
    }

    HRESULT LLOS_PWM_SetPulseWidth(LLOS_Context channel, uint32_t widthMicroSeconds)
    {
        LLOS_MbedPwm *pPwm = (LLOS_MbedPwm*)channel;

        if (pPwm == NULL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        pPwm->PulseWidth = widthMicroSeconds;
        pwmout_pulsewidth_us(&pPwm->Pwm, widthMicroSeconds);

        return S_OK;
    }

    HRESULT LLOS_PWM_SetPolarity(LLOS_Context channel, LLOS_PWM_Polarity polarity)
    {
        return LLOS_E_NOT_SUPPORTED;
    }

    HRESULT LLOS_PWM_SetPrescaler(LLOS_Context channel, LLOS_PWM_Prescaler prescaler)
    {
        return LLOS_E_NOT_SUPPORTED;
    }

    HRESULT LLOS_PWM_Start(LLOS_Context channel)
    {
        LLOS_MbedPwm *pPwm = (LLOS_MbedPwm*)channel;

        if (pPwm == NULL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        pwmout_write(&pPwm->Pwm, pPwm->DutyCycle);

        return S_OK;
    }

    HRESULT LLOS_PWM_Stop(LLOS_Context channel)
    {
        LLOS_MbedPwm *pPwm = (LLOS_MbedPwm*)channel;

        if (pPwm == NULL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        pwmout_write(&pPwm->Pwm, 0.0);

        return S_OK;
    }
}
