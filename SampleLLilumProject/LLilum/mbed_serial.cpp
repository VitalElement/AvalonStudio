#include "mbed_helpers.h"
#include "llos_serial.h"
#include "llos_memory.h"

//--//

extern "C"
{
    typedef struct LLOS_MbedSerial
    {
        serial_t                      Port;
        LLOS_SERIAL_InterruptCallback Callback;
        LLOS_Context                  Context;
        LLOS_SERIAL_Config            Config;
    } LLOS_MbedSerial;

    HRESULT LLOS_SERIAL_Open(int32_t rxPin, int32_t txPin, LLOS_SERIAL_Config** ppConfig, LLOS_Context* pChannel)
    {
        LLOS_MbedSerial *pSerial;

        if (pChannel == NULL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        pSerial = (LLOS_MbedSerial*)AllocateFromManagedHeap(sizeof(LLOS_MbedSerial));

        if (pSerial == NULL)
        {
            return LLOS_E_OUT_OF_MEMORY;
        }

        serial_init(&pSerial->Port, (PinName)txPin, (PinName)rxPin);

        *pChannel = (LLOS_Context)pSerial;
        *ppConfig = &pSerial->Config;

        return S_OK;
    }

    VOID LLOS_SERIAL_Close(LLOS_Context channel)
    {
        LLOS_MbedSerial *pSerial = (LLOS_MbedSerial*)channel;

        if (pSerial != NULL)
        {
            serial_free(&pSerial->Port);
        }

        FreeFromManagedHeap(pSerial);
    }

    HRESULT InternalSerialEnable(LLOS_Context channel, LLOS_SERIAL_Irq irq, BOOL fEnable)
    {
        LLOS_MbedSerial *pSerial = (LLOS_MbedSerial*)channel;

        if (pSerial == NULL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        switch (irq)
        {
        case LLOS_SERIAL_IrqRx:
            serial_irq_set(&pSerial->Port, RxIrq, fEnable);
            break;
        case LLOS_SERIAL_IrqTx:
            serial_irq_set(&pSerial->Port, TxIrq, fEnable);
            break;
        case LLOS_SERIAL_IrqBoth:
            serial_irq_set(&pSerial->Port, TxIrq, fEnable);
            serial_irq_set(&pSerial->Port, RxIrq, fEnable);
            break;
        default:
            return LLOS_E_INVALID_PARAMETER;
        }

        return S_OK;
    }

    HRESULT LLOS_SERIAL_Enable(LLOS_Context channel, LLOS_SERIAL_Irq irq)
    {
        return InternalSerialEnable(channel, irq, 1);
    }

    HRESULT LLOS_SERIAL_Disable(LLOS_Context channel, LLOS_SERIAL_Irq irq)
    {
        return InternalSerialEnable(channel, irq, 0);
    }

    HRESULT LLOS_SERIAL_SetFlowControl(LLOS_Context channel, int32_t rtsPin, int32_t ctsPin)
    {
#if DEVICE_SERIAL_FC
        LLOS_MbedSerial *pSerial = (LLOS_MbedSerial*)channel;
        FlowControl fc = FlowControlRTSCTS;

        if (pSerial == NULL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        if (rtsPin == -1)
        {
            if (ctsPin == -1)
            {
                fc = FlowControlNone;
            }
            else
            {
                fc = FlowControlCTS;
            }
        }
        else if (ctsPin == -1)
        {
            fc = FlowControlRTS;
        }

        serial_set_flow_control(&pSerial->Port, fc, (PinName)rtsPin, (PinName)ctsPin);         

        return S_OK;
#else
        return LLOS_E_NOTIMPL;
#endif
    }

    HRESULT LLOS_SERIAL_Configure(LLOS_Context channel, LLOS_SERIAL_Config* pConfig)
    {
        LLOS_MbedSerial *pSerial = (LLOS_MbedSerial*)channel;

        if (pSerial == NULL || pConfig == NULL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        serial_format(&pSerial->Port, pConfig->DataBits, (SerialParity)pConfig->Parity, pConfig->StopBits);
        serial_baud(&pSerial->Port, pConfig->BaudRate);

        return S_OK;
    }

    HRESULT LLOS_SERIAL_Read(LLOS_Context channel, uint8_t* pBuffer, int32_t offset, int32_t* pLength)
    {
        LLOS_MbedSerial *pSerial   = (LLOS_MbedSerial*)channel;
        int              bytesRead = 0;
        char            *pcNext    = NULL;

        if (pSerial == NULL || pBuffer == NULL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        pcNext = (char*)&pBuffer[offset];

        while (serial_readable(&pSerial->Port) && bytesRead < *pLength)
        {
            *pcNext++ = serial_getc(&pSerial->Port);
            bytesRead++;
        }

        *pLength = bytesRead;

        return S_OK;
    }

    HRESULT LLOS_SERIAL_Write(LLOS_Context channel, uint8_t* pBuffer, int32_t offset, int32_t length)
    {
        LLOS_MbedSerial *pSerial      = (LLOS_MbedSerial*)channel;
        char            *pcNext       = NULL;

        if (pSerial == NULL || pBuffer == NULL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        pcNext = (char*)&pBuffer[offset];

        while (length-- > 0)
        {
            serial_putc(&pSerial->Port, *pcNext++);
        }

        return S_OK;
    }

    HRESULT LLOS_SERIAL_Flush(LLOS_Context channel)
    {
        return S_OK;
    }

    HRESULT LLOS_SERIAL_Clear(LLOS_Context channel)
    {
        LLOS_MbedSerial *pSerial = (LLOS_MbedSerial*)channel;

        if (pSerial == NULL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        serial_clear(&pSerial->Port);

        return S_OK;
    }

    static void HandleInternalSerialPortInterrupt(uint32_t id, SerialIrq data)
    {
        LLOS_MbedSerial *pSerial = (LLOS_MbedSerial*)id;

        if (pSerial != NULL && pSerial->Callback != NULL)
        {
            pSerial->Callback(pSerial, pSerial->Context, (LLOS_SERIAL_Event)data);
        }
    }

    HRESULT LLOS_SERIAL_SetCallback(LLOS_Context channel, LLOS_SERIAL_InterruptCallback callback, LLOS_Context callbackContext)
    {
        LLOS_MbedSerial *pSerial = (LLOS_MbedSerial*)channel;

        if (pSerial == NULL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        pSerial->Callback = callback;
        pSerial->Context  = callbackContext;

        serial_irq_handler(&pSerial->Port, (uart_irq_handler)HandleInternalSerialPortInterrupt, (uint32_t)channel);

        return S_OK;
    }

    HRESULT LLOS_SERIAL_CanRead(LLOS_Context channel, BOOL *pCanRead)
    {
        LLOS_MbedSerial *pSerial = (LLOS_MbedSerial*)channel;

        if (pSerial == NULL || pCanRead == NULL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        *pCanRead = serial_readable(&pSerial->Port);

        return S_OK;
    }

    HRESULT LLOS_SERIAL_CanWrite(LLOS_Context channel, BOOL *pCanWrite)
    {
        LLOS_MbedSerial *pSerial = (LLOS_MbedSerial*)channel;

        if (pSerial == NULL || pCanWrite == NULL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        *pCanWrite = serial_writable(&pSerial->Port);

        return S_OK;
    }
}
