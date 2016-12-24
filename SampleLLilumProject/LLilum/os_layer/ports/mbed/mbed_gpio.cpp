//
// Copyright (c) Microsoft Corporation.    All rights reserved.
//

#include "mbed_helpers.h" 
#include "llos_gpio.h"
#include "llos_memory.h"


//--//

extern "C"
{
    typedef struct LLOS_MbedGpio
    {
        gpio_t                      Pin;
        gpio_irq_t                  Irq;
        LLOS_GPIO_InterruptCallback Callback;
        LLOS_Context                Context;
    } LLOS_MbedGpio;

    HRESULT LLOS_GPIO_AllocatePin(uint32_t pin_number, LLOS_Context* pPin)
    {
        LLOS_MbedGpio *pGpio;
        
        if (pPin == NULL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        pGpio = (LLOS_MbedGpio*)AllocateFromManagedHeap(sizeof(LLOS_MbedGpio));

        if (pGpio == NULL)
        {
            return LLOS_E_OUT_OF_MEMORY;
        }

        gpio_init( &pGpio->Pin, (PinName)pin_number);
        pGpio->Callback = NULL;
        pGpio->Context  = NULL;

        *pPin = (LLOS_Context)pGpio;

        return S_OK;
    }

    void LLOS_GPIO_FreePin(LLOS_Context pin)
    {
        LLOS_MbedGpio *pGpio = (LLOS_MbedGpio*)pin;

        if (pGpio != NULL)
        {
            gpio_irq_disable(&pGpio->Irq);
            gpio_irq_free(&pGpio->Irq);
            FreeFromManagedHeap(pin);
        }
    }

    static void HandleGpioInterrupt(uint32_t id, gpio_irq_event evt)
    {
        LLOS_MbedGpio   *pGpio = (LLOS_MbedGpio*)id;
        LLOS_GPIO_Edge edge = evt == IRQ_RISE ? LLOS_GPIO_EdgeRising : LLOS_GPIO_EdgeFalling;

        pGpio->Callback((LLOS_Context)&pGpio->Pin, pGpio->Context, edge);
    }

    HRESULT LLOS_GPIO_EnablePin(LLOS_Context pin, LLOS_GPIO_Edge edge, LLOS_GPIO_InterruptCallback callback, LLOS_Context callback_context)
    {
        LLOS_MbedGpio *pGpio = (LLOS_MbedGpio*)pin;
        int edgeRiseEnable = 0;
        int edgeFallEnable = 0;

        if (pGpio == NULL || callback == NULL || edge == LLOS_GPIO_EdgeNone)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        pGpio->Callback = callback;
        pGpio->Context = callback_context;

        if (0 != gpio_irq_init(&pGpio->Irq, pGpio->Pin.pin, (gpio_irq_handler)HandleGpioInterrupt, (uint32_t)pGpio))
        {
            return LLOS_E_PIN_UNAVAILABLE;
        }

        switch (edge)
        {
        case LLOS_GPIO_EdgeBoth:
            edgeRiseEnable = 1;
            edgeFallEnable = 1;
            break;
        case LLOS_GPIO_EdgeFalling:
            edgeFallEnable = 1;
            break;
        case LLOS_GPIO_EdgeRising:
            edgeRiseEnable = 1;
            break;
        default:
            return LLOS_E_NOT_SUPPORTED;
        }

        gpio_irq_set(&pGpio->Irq, IRQ_RISE, edgeRiseEnable);
        gpio_irq_set(&pGpio->Irq, IRQ_FALL, edgeFallEnable);
        gpio_irq_enable(&pGpio->Irq);

        return S_OK;
    }

    HRESULT LLOS_GPIO_DisablePin(LLOS_Context pin)
    {
        LLOS_MbedGpio *pGpio = (LLOS_MbedGpio*)pin;

        if (pGpio != NULL)
        {
            gpio_irq_disable( &pGpio->Irq );
        }

        return S_OK;
    }

    HRESULT LLOS_GPIO_SetPolarity(LLOS_Context pin, LLOS_GPIO_Polarity polarity)
    {
        return LLOS_E_NOT_SUPPORTED;
    }

    HRESULT LLOS_GPIO_SetMode(LLOS_Context pin, LLOS_GPIO_Resistor resistor)
    {
        LLOS_MbedGpio *pGpio = (LLOS_MbedGpio*)pin;
        PinMode mode;

        switch (resistor)
        {
        case LLOS_GPIO_ResistorDefault:
            mode = PullDefault;
            break;
        case LLOS_GPIO_ResistorPullNone:
            mode = PullNone;
            break;
        case LLOS_GPIO_ResistorPullup:
            mode = PullUp;
            break;
        case LLOS_GPIO_ResistorPulldown:
            mode = PullDown;
            break;
        default:
            return LLOS_E_NOT_SUPPORTED;
        }

        gpio_mode(&pGpio->Pin, mode);

        return S_OK;
    }

    HRESULT LLOS_GPIO_SetDirection(LLOS_Context pin, LLOS_GPIO_Direction direction)
    {
        LLOS_MbedGpio *pGpio = (LLOS_MbedGpio*)pin;

        if (pGpio == NULL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        if (direction == LLOS_GPIO_Output)
        {
            gpio_dir(&pGpio->Pin, PIN_OUTPUT);
        }
        else
        {
            gpio_dir(&pGpio->Pin, PIN_INPUT);
        }

        return S_OK;
    }

    HRESULT LLOS_GPIO_SetDebounce(LLOS_Context pin, LLOS_TimeSpan debounceTime)
    {
        return LLOS_E_NOT_SUPPORTED;
    }

    HRESULT LLOS_GPIO_Write(LLOS_Context pin, int32_t value)
    {
        LLOS_MbedGpio *pGpio = (LLOS_MbedGpio*)pin;

        if (pGpio == NULL)
        {
            return LLOS_E_INVALID_PARAMETER;
        }

        gpio_write( &pGpio->Pin, value );

        return S_OK;
    }

    int32_t LLOS_GPIO_Read(LLOS_Context pin)
    {
        LLOS_MbedGpio *pGpio = (LLOS_MbedGpio*)pin;

        if (pGpio == NULL)
        {
            return -1;
        }

        return gpio_read( &pGpio->Pin );
    }

    HRESULT LLOS_GPIO_GetConfig(LLOS_Context pin, uint32_t* pin_number, LLOS_GPIO_Edge* edge, LLOS_GPIO_Resistor* resistor, LLOS_GPIO_Polarity* polarity)
    {
        return LLOS_E_NOT_SUPPORTED;
    }
}
