#include "mbed_helpers.h"
#include "llos_analog.h"
#include "llos_memory.h"

//--//

extern "C"
{
#if DEVICE_ANALOGOUT
    typedef struct LLOS_MbedAdc
    {
        union
        {
            analogin_t InputChannel;
            dac_t      OutputChannel;
        };

        LLOS_ADC_Direction Direction;
    } LLOS_MbedAdc;

    HRESULT LLOS_ADC_Initialize(uint32_t pinName, LLOS_ADC_Direction direction, LLOS_Context* channel)
    {
        LLOS_MbedAdc *pCtx;

        if (channel == NULL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        pCtx = (LLOS_MbedAdc*)AllocateFromManagedHeap(sizeof(LLOS_MbedAdc));

        if (pCtx == NULL)
        {
            return LLOS_E_OUT_OF_MEMORY;
        }

        pCtx->Direction = direction;

        if (direction == LLOS_ADC_Input)
        {
            analogin_init(&pCtx->InputChannel, (PinName)pinName);
        }
        else
        {
            analogout_init(&pCtx->OutputChannel, (PinName)pinName);
        }

        *channel = pCtx;

        return S_OK;
    }

    VOID LLOS_ADC_Uninitialize(LLOS_Context channel)
    {
        FreeFromManagedHeap(channel);
    }

    HRESULT LLOS_ADC_ReadRaw(LLOS_Context channel, int32_t* value)
    {
        LLOS_MbedAdc *pCtx = (LLOS_MbedAdc*)channel;

        if (value == NULL || pCtx == NULL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        if (pCtx->Direction == LLOS_ADC_Input)
        {
            *value = (uint32_t)analogin_read_u16(&pCtx->InputChannel);
        }
        else
        {
            *value = (uint32_t)analogout_read_u16(&pCtx->OutputChannel);
        }

        return S_OK;
    }
    HRESULT LLOS_ADC_WriteRaw(LLOS_Context channel, int32_t value)
    {
        LLOS_MbedAdc *pCtx = (LLOS_MbedAdc*)channel;

        if (pCtx == NULL || pCtx->Direction == LLOS_ADC_Input || value > 0xFFFF || value < 0)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        analogout_write_u16(&pCtx->OutputChannel, (uint16_t)value);

        return S_OK;
    }

    HRESULT LLOS_ADC_Read(LLOS_Context channel, float* value)
    {
        LLOS_MbedAdc *pCtx = (LLOS_MbedAdc*)channel;

        if (value == NULL || pCtx == NULL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        if (pCtx->Direction == LLOS_ADC_Input)
        {
            *value = analogin_read(&pCtx->InputChannel);
        }
        else
        {
            *value = analogout_read(&pCtx->OutputChannel);
        }

        return S_OK;
    }

    HRESULT LLOS_ADC_Write(LLOS_Context channel, float value)
    {
        LLOS_MbedAdc *pCtx = (LLOS_MbedAdc*)channel;

        if (pCtx == NULL || pCtx->Direction == LLOS_ADC_Input)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        analogout_write(&pCtx->OutputChannel, value);

        return S_OK;
    }

    HRESULT LLOS_ADC_GetPrecisionBits(LLOS_Context channel, uint32_t* precisionInBits)
    {
        return LLOS_E_NOTIMPL;
    }
#endif
}
