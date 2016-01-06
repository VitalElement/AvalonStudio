//
// Copyright (c) Microsoft Corporation.    All rights reserved.
//

#include "mbed_helpers.h" 

//--//

extern "C"
{

	//
	// SysTick
	//

	//////
	//////	From core_cm3.h in mBed CMSIS support: 
	//////	
	//////	/** \brief  Structure type to access the System Timer (SysTick).
	//////	*/
	//////	typedef struct
	//////	{
	//////		__IO uint32_t CTRL;                    /*!< Offset: 0x000 (R/W)  SysTick Control and Status Register */
	//////		__IO uint32_t LOAD;                    /*!< Offset: 0x004 (R/W)  SysTick Reload Value Register       */
	//////		__IO uint32_t VAL;                     /*!< Offset: 0x008 (R/W)  SysTick Current Value Register      */
	//////		__I  uint32_t CALIB;                   /*!< Offset: 0x00C (R/ )  SysTick Calibration Register        */
	//////	} SysTick_Type;
	//////	
	//////	...
	//////	...
	//////	...
	//////	
	//////	#define SysTick             ((SysTick_Type   *)     SysTick_BASE  )   /*!< SysTick configuration struct       */
	//////

	/*__STATIC_INLINE*/ uint32_t CMSIS_STUB_SysTick_GetCTRL()
	{
		return SysTick->CTRL;
	}

	/*__STATIC_INLINE*/ void CMSIS_STUB_SysTick_SetCTRL(uint32_t value)
	{
		SysTick->CTRL = value;
	}

	/*__STATIC_INLINE*/ uint32_t CMSIS_STUB_SysTick_GetLOAD()
	{
		return SysTick->LOAD & 0x00FFFFFF;
	}

	/*__STATIC_INLINE*/ void CMSIS_STUB_SysTick_SetLOAD(uint32_t value)
	{
		SysTick->LOAD |= value & 0x00FFFFFF;
	}

	/*__STATIC_INLINE*/ uint32_t CMSIS_STUB_SysTick_GetVAL()
	{
		return SysTick->VAL & 0x00FFFFFF;
	}

	/*__STATIC_INLINE*/ void CMSIS_STUB_SysTick_SetVAL(uint32_t value)
	{
		SysTick->VAL |= value & 0x00FFFFFF;
	}

	/*__STATIC_INLINE*/ uint32_t CMSIS_STUB_SysTick_GetCALIB()
	{
		return SysTick->CALIB;
	}

    //
    //
    //

    extern void ContextSwitchTimer_Handler_Zelig();

    __attribute__((naked)) void SysTick_Handler(void)
    {
        __ASM volatile ("STR       LR, [SP, #-4]!");                // Save LR to stack

        ContextSwitchTimer_Handler_Zelig();

        __ASM volatile ("LDR       LR, [SP], #4");                  // Restore LR from stack
        __ASM volatile ("BX        LR");
    }
}
