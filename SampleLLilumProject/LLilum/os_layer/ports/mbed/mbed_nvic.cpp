//
// Copyright (c) Microsoft Corporation.    All rights reserved.
//

#include "mbed_helpers.h" 

//--//

extern "C"
{

	//
	// NVIC
	//

	/*__STATIC_INLINE*/ void CMSIS_STUB_NVIC_SetPriorityGrouping(uint32_t PriorityGroup)
	{
		NVIC_SetPriorityGrouping(PriorityGroup);
		__ISB(); // always emit a barrier 
	}

	/*__STATIC_INLINE*/ uint32_t CMSIS_STUB_NVIC_GetPriorityGrouping(void)
	{
		return NVIC_GetPriorityGrouping();
	}

	/*__STATIC_INLINE*/ void CMSIS_STUB_NVIC_EnableIRQ(IRQn_Type IRQn)
	{
		NVIC_EnableIRQ(IRQn);
	}

	/*__STATIC_INLINE*/ void CMSIS_STUB_NVIC_DisableIRQ(IRQn_Type IRQn)
	{
		NVIC_DisableIRQ(IRQn);
	}

	/*__STATIC_INLINE*/ uint32_t CMSIS_STUB_NVIC_GetPendingIRQ(IRQn_Type IRQn)
	{
		return NVIC_GetPendingIRQ(IRQn);
	}

	/*__STATIC_INLINE*/ void CMSIS_STUB_NVIC_SetPendingIRQ(IRQn_Type IRQn)
	{
		NVIC_SetPendingIRQ(IRQn);
	}

	/*__STATIC_INLINE*/ void CMSIS_STUB_NVIC_ClearPendingIRQ(IRQn_Type IRQn)
	{
		NVIC_ClearPendingIRQ(IRQn);
	}

	/*__STATIC_INLINE*/ uint32_t CMSIS_STUB_NVIC_GetActive(IRQn_Type IRQn)
	{
		return NVIC_GetActive(IRQn);
	}

	/*__STATIC_INLINE*/ void CMSIS_STUB_NVIC_SetPriority(IRQn_Type IRQn, uint32_t priority)
	{
		NVIC_SetPriority(IRQn, priority);
		__ISB(); // always emit a barrier 
	}

	/*__STATIC_INLINE*/ uint32_t CMSIS_STUB_NVIC_GetPriority(IRQn_Type IRQn)
	{
		return NVIC_GetPriority(IRQn);
	}

	/*__STATIC_INLINE*/ uint32_t CMSIS_STUB_NVIC_EncodePriority(uint32_t PriorityGroup, uint32_t PreemptPriority, uint32_t SubPriority)
	{
		return NVIC_EncodePriority(PriorityGroup, PreemptPriority, SubPriority);
	}

	/*__STATIC_INLINE*/ void CMSIS_STUB_NVIC_DecodePriority(uint32_t Priority, uint32_t PriorityGroup, uint32_t* pPreemptPriority, uint32_t* pSubPriority)
	{
		NVIC_DecodePriority(Priority, PriorityGroup, pPreemptPriority, pSubPriority);
	}

	/*__STATIC_INLINE*/ void CMSIS_STUB_NVIC_SystemReset(void)
	{
		NVIC_SystemReset();
	}

	/*__STATIC_INLINE*/ void CUSTOM_STUB_NVIC_RaisePendSV()
	{
		*((uint32_t volatile *)0xE000ED04) = 0x10000000; // trigger PendSV
	}
}
