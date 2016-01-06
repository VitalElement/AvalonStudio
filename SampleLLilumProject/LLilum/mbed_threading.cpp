//
// Copyright (c) Microsoft Corporation.    All rights reserved.
//

#include "mbed_helpers.h" 

//--//

extern "C"
{

	//
	// Threading 
	//

	void* CreateNativeContext(void* entryPoint, void* stack, int32_t stackSize)
	{
		return NULL;
	}

	void Yield(void* nativeContext)
	{
	}

	void Retire(void* nativeContext)
	{
	}

	void SwitchToContext(void* nativeContext)
	{
	}

	void* GetPriority(void* nativeContext)
	{
		return 0;
	}

	void SetPriority(void* nativeContext, void* priority)
	{
	}
}