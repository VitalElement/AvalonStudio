#include "mbed_helpers.h"
#include "llos_i2c.h"
#include "llos_memory.h"

//--//

extern "C"
{
    HRESULT LLOS_I2C_Initialize(int32_t sdaPin, int32_t sclPin, LLOS_Context* pChannel)
    {
        i2c_t *pI2C;

        if (pChannel == NULL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        pI2C = (i2c_t*)AllocateFromManagedHeap(sizeof(i2c_t));

        if (pI2C == NULL)
        {
            return LLOS_E_OUT_OF_MEMORY;
        }

        i2c_init(pI2C, (PinName)sdaPin, (PinName)sclPin);

        *pChannel = (LLOS_Context*)pI2C;

        return S_OK;
    }

    VOID LLOS_I2C_Uninitialize(LLOS_Context channel)
    {
        FreeFromManagedHeap(channel);
    }

    HRESULT LLOS_I2C_SetFrequency(LLOS_Context channel, uint32_t frequencyHz)
    {
        i2c_t  *pI2C = (i2c_t*)channel;

        if (pI2C == NULL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        i2c_frequency(pI2C, frequencyHz);

        return S_OK;
    }

    HRESULT LLOS_I2C_Enable(LLOS_Context channel)
    {
        return LLOS_E_NOT_SUPPORTED;
    }

    HRESULT LLOS_I2C_Disable(LLOS_Context channel)
    {
        return LLOS_E_NOT_SUPPORTED;
    }

    HRESULT LLOS_I2C_Write(LLOS_Context channel, uint32_t address, uint8_t* pBuffer, int32_t offset, int32_t *pLength, BOOL stop)
    {
        i2c_t  *pI2C = (i2c_t*)channel;

        if (pI2C == NULL || pBuffer == NULL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        *pLength = i2c_write(pI2C, address, (const char *)&pBuffer[offset], *pLength, stop);

        return S_OK;
    }

    HRESULT LLOS_I2C_Read(LLOS_Context channel, uint32_t address, uint8_t* pBuffer, int32_t offset, int32_t* pLength, BOOL stop)
    {
        i2c_t  *pI2C = (i2c_t*)channel;

        if (pI2C == NULL || pBuffer == NULL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        *pLength = i2c_read(pI2C, address, (char *)&pBuffer[offset], *pLength, stop);

        return S_OK;
    }

    HRESULT LLOS_I2C_Reset(LLOS_Context channel)
    {
        i2c_t  *pI2C = (i2c_t*)channel;

        if (pI2C == NULL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        i2c_reset(pI2C);

        return S_OK;
    }
}
