//
// Copyright (c) Microsoft Corporation.    All rights reserved.
//

#include "mbed_helpers.h" 

//--//

extern "C"
{

    //
    // We want to run ISRs in privileged Handler mode using the Main Stack Pointer and all the other tasks 
    // in privileged Thread mode using the Process Stack Pointer. 
    //
    // We will assume that native context switching is possible when processor initialization is carried out in
    // Handler/Privileged mode. Of course that is not a complete guarantee. After carrying out the initialization of the 
    // idle thread task, we will let the initialization thread return to thread mode upon first context switch as per classic 
    // technique mentioned below, from ARM reference manual. As switching the mode is carried out naturally by 
    // setting the appropriate flag, there is nothing else we need to do at initialization time. See context switch code 
    // in thread manager as well.

    //
    // From ARMv-7M Architecture Reference Manual. 
    // 
    // 
    // By default, Thread mode uses the MSP. To switch the stack pointer used in Thread mode to the
    // PSP, either:
    // - use the MSR instruction to set the Active stack pointer bit to 1
    // - perform an exception return to Thread mode with the appropriate EXC_RETURN value
    //

    ////// 
    //////  
    ////// 
    //////  Table 2-17 Exception return behavior
    //////
    //////  EXC_RETURN      Description
    //////  =========================================================================
    //////  0xFFFFFF[F|E]1  Return to Handler mode.
    //////                  Exception return gets state [and FP state] from the main 
    //////                  stack (MSP).
    //////                  Execution uses MSP after return.
    ////// 
    //////  0xFFFFFF[F|E]9  Return to Thread mode.
    //////                  Exception Return get state [and FP state] from the main 
    //////                  stack (MSP).
    //////                  Execution uses MSP after return.
    ////// 
    //////  0xFFFFFF[F|E]D  Return to Thread mode.
    //////                  Exception return gets state [and FP state] from the process 
    //////                  stack (PSP).
    //////                  Execution uses PSP after return.
    ////// 
    //////  All other values Reserved.
    ////// 

    //
    // System Control Block 
    //

    /*__STATIC_INLINE*/ uint32_t CMSIS_STUB_SCB__get_CONTROL()
    {
        return __get_CONTROL();
    }

    /*__STATIC_INLINE*/ void CMSIS_STUB_SCB__set_CONTROL(uint32_t control)
    {
        __set_CONTROL(control);
        __ISB(); // always emit a barrier 
    }

    /*__STATIC_INLINE*/ uint32_t CMSIS_STUB_SCB__get_IPSR()
    {
        return __get_IPSR();
    }

    /*__STATIC_INLINE*/ uint32_t CMSIS_STUB_SCB__get_APSR()
    {
        return __get_APSR();
    }

    /*__STATIC_INLINE*/ uint32_t CMSIS_STUB_SCB__get_xPSR()
    {
        return __get_xPSR();
    }

    /*__STATIC_INLINE*/ uint32_t CMSIS_STUB_SCB__get_PSP()
    {
        return __get_PSP();
    }

    /*__STATIC_INLINE*/ void CMSIS_STUB_SCB__set_PSP(uint32_t topOfProcStack)
    {
        __set_PSP(topOfProcStack);
        __ISB(); // always emit a barrier 
    }

    /*__STATIC_INLINE*/ uint32_t CMSIS_STUB_SCB__get_MSP()
    {
        return __get_MSP();
    }

    /*__STATIC_INLINE*/ void CMSIS_STUB_SCB__set_MSP(uint32_t topOfMainStack)
    {
        __set_MSP(topOfMainStack);
        __ISB(); // always emit a barrier 
    }

    /*__STATIC_INLINE*/ uint32_t CMSIS_STUB_SCB__get_PRIMASK()
    {
        return __get_PRIMASK();
    }

    /*__STATIC_INLINE*/ void CMSIS_STUB_SCB__set_PRIMASK(uint32_t priMask)
    {
        __set_PRIMASK(priMask);
    }

    /*__STATIC_INLINE*/ void CMSIS_STUB_SCB__Enable_Irq()
    {
        __enable_irq();
        __ISB();
    }

    /*__STATIC_INLINE*/ void CMSIS_STUB_SCB__Disable_Irq()
    {
        __disable_irq();
        __ISB();
    }

    //
    // vvvvv !!! Cortex M3 only !!! vvvvv
    //
#if       (__CORTEX_M >= 0x03)
//
//#define __enable_fault_irq                __enable_fiq
//
//#define __disable_fault_irq               __disable_fiq

    /*__STATIC_INLINE*/ void CMSIS_STUB_SCB__Enable_Fault_Irq()
    {
        __enable_fault_irq();
        __ISB();
    }

    /*__STATIC_INLINE*/ void CMSIS_STUB_SCB__Disable_Fault_Irq()
    {
        __disable_fault_irq();
        __ISB();
    }

    /*__STATIC_INLINE*/ uint32_t  CMSIS_STUB_SCB__get_BASEPRI()
    {
        return __get_BASEPRI() >> (8 - __NVIC_PRIO_BITS);
    }

    /*__STATIC_INLINE*/ uint32_t CMSIS_STUB_SCB__set_BASEPRI(uint32_t basePri)
    {
        __set_PRIMASK(1);
        __ISB();
        register uint32_t prev = __get_BASEPRI() >> (8 - __NVIC_PRIO_BITS);
        __set_BASEPRI(basePri << (8 - __NVIC_PRIO_BITS));
        __set_PRIMASK(0);
        __ISB();
        return prev;
    }

    /*__STATIC_INLINE*/ uint32_t CMSIS_STUB_SCB__get_FAULTMASK()
    {
        return __get_FAULTMASK();
    }

    /*__STATIC_INLINE*/ void CMSIS_STUB_SCB__set_FAULTMASK(uint32_t faultMask)
    {
        __set_FAULTMASK(faultMask);
    }

#endif /* (__CORTEX_M >= 0x03) */

    //
    // vvvvv !!! Cortex M4 only !!! vvvvv
    //
#if       (__CORTEX_M == 0x04)

    /*__STATIC_INLINE*/ uint32_t CMSIS_STUB_SCB__get_FPSCR()
    {
        return __get_FPSCR();
    }

    /*__STATIC_INLINE*/ void CMSIS_STUB_SCB__set_FPSCR(uint32_t fpscr)
    {
        return __set_FPSCR(fpscr);
    }

#endif /* (__CORTEX_M == 0x04) */


    /*__STATIC_INLINE*/ uint32_t CUSTOM_STUB_SCB__get_FPCCR()
    {
        return *((uint32_t volatile *)0xE000EF34);
    }

    /*__STATIC_INLINE*/ void CUSTOM_STUB_SCB__set_FPCCR(uint32_t fpscr)
    {
        *((uint32_t volatile *)0xE000EF34) = fpscr;
    }

    //
    // CCR
    //

    /*__STATIC_INLINE*/ void CUSTOM_STUB_SCB_set_CCR(uint32_t value)
    {
        *((uint32_t volatile *)0xE000ED14) = value;
    }

    //
    // System Handlers
    // 

    /*__STATIC_INLINE*/ void CUSTOM_STUB_SCB_SHCRS_EnableSystemHandler(uint32_t ex)
    {
        uint32_t SHCRS = *((uint32_t volatile *)0xE000ED24);

        SHCRS |= ex;

        *((uint32_t volatile *)0xE000ED24) = SHCRS;

    }

    /*__STATIC_INLINE*/ void CUSTOM_STUB_SCB_SHCRS_DisableSystemHandler(uint32_t ex)
    {
        uint32_t SHCRS = *((uint32_t volatile *)0xE000ED24);

        SHCRS &= ~ex;

        *((uint32_t volatile *)0xE000ED24) = SHCRS;
    }

    /*__STATIC_INLINE*/ void CUSTOM_STUB_SCB_ICSR_RaiseSystemException(uint32_t ex)
    {
        //
        // Set/Clears system handlers in ICSR register of SCB. 
        // CMSIS does not allow using NVIC api to set/clear System Handlers. 
        //
        uint32_t ICSR = *((uint32_t volatile *)0xE000ED04);

        ICSR |= ex;

        *((uint32_t volatile *)0xE000ED04) = ICSR;
    }

    /*__STATIC_INLINE*/ uint32_t CUSTOM_STUB_SCB_IPSR_GetCurrentISRNumber()
    {
        return __get_IPSR() & 0x000000FF;
    }

    /*__STATIC_INLINE*/ void CUSTOM_STUB_SCB_SCR_SetSystemControlRegister(uint32_t scr)
    {
        *((uint32_t volatile *)0xE000ED10) = scr;
    }

    /*__STATIC_INLINE*/ uint32_t CUSTOM_STUB_SCB_SCR_GetSystemControlRegister( )
    {
        return *((uint32_t volatile *)0xE000ED10);
    }

    /*__STATIC_INLINE*/ void CMSIS_STUB_POWER_WaitForEvent()
    {
        __WFE();
    }

    /*__STATIC_INLINE*/ void CMSIS_STUB_POWER_SendEvent()
    {
        __SEV();
    }

    /*__STATIC_INLINE*/ void CMSIS_STUB_POWER_WaitForInterrupt()
    {
        __WFI();
    }
    
    /*__STATIC_INLINE*/ 
    __attribute__((naked)) 
    __attribute__((aligned(8))) 
    void CUSTOM_STUB_RaiseSupervisorCallForLongJump()
    {
        __ASM volatile ("svc #17");
    }

    /*__STATIC_INLINE*/
    __attribute__((naked))
    __attribute__((aligned(8)))
    void CUSTOM_STUB_RaiseSupervisorCallForStartThreads()
    {
        __ASM volatile ("svc #18");
    }

    /*__STATIC_INLINE*/ 
    __attribute__((naked))
    __attribute__((aligned(8)))
    void CUSTOM_STUB_RaiseSupervisorCallForRetireThread()
    {
        __ASM volatile ("svc #19");
    }


    /*__STATIC_INLINE*/
    /*__attribute__((naked))*/
    __attribute__((aligned(8)))
        void CUSTOM_STUB_RaiseSupervisorCallForSnapshotProcessModeRegisters()
    {
        __ASM volatile ("svc #20");
    }

    //--//

    //
    // The managed portion of the SVC_Handler
    //
    extern void SVC_Handler_Zelig_VFP_NoFPContext();
    extern void SVC_Handler_Zelig();
    extern void CUSTOM_STUB_NotifySoftwareFrameSnapshot( void* frame, int size ); 

    //
    // A convenience storage space to signal what mode to return too
    // Initialized to crash, as the Thread Manager needs to set it right
    //
    uint32_t svc_exc_return = 0xDEADBEEF;

    void CUSTOM_STUB_SetExcReturn(uint32_t ret)
    {
        svc_exc_return = ret; 
    }

    //--//
    //--//
    //--//

    // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    // !!! KEEP IN SYNC WITH ProcessorARMv7M.Context.SoftwareFrame & HardwareFrame !!!
    // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

    #define SW_FRAME_SIZE   10
    #define HW_FRAME_SIZE   8     
    #define FRAME_SIZE      SW_FRAME_SIZE + HW_FRAME_SIZE

    //
    // A convenience storage space to snapshot all registers for Mark & Sweep GC
    //
    uint32_t sw_hw__frame[FRAME_SIZE];

    //
    // Pull
    //
    /*__STATIC_INLINE*/
    __attribute__((aligned(8)))
    uint32_t* CUSTOM_STUB_FetchSoftwareFrameSnapshot()
    {
        //
        // Return the snapshot from the storage area at sw_hw__frame
        //
        return &sw_hw__frame[0];
    }


    //
    // Push
    // 
    extern void CUSTOM_STUB_NotifySoftwareFrameSnapshot(void* frame, int size);


    /*__STATIC_INLINE*/
    __attribute__((aligned(8)))
        void NotifySoftwareFrameSnapshot()
    {
        //
        // Grab the snapshot from the storage area at sw_hw__frame
        //
        CUSTOM_STUB_NotifySoftwareFrameSnapshot(sw_hw__frame, FRAME_SIZE);
    }

    
    __attribute__((naked)) 
    void SVC_Handler(void)
    {    
        __ASM volatile ("TST    LR, #0x4");                 // Test bit 3 to use decide which stack pointer we are coming from 
        __ASM volatile ("ITE    EQ");        
        __ASM volatile ("MRSEQ  R0, msp");
        __ASM volatile ("MRSNE  R0, psp");

        //
        // Test for SupervisorCall__SnapshotProcessModeRegisters (0x14) and snapshot 
        // all remaining registers onto the stack first and into the convenience space
        // right after
        // TODO: move to managed layer
        //
        {
            __ASM volatile ("LDR    R12, [R0 , #24]");          // load the SVC number
            __ASM volatile ("LDRH   R12, [R12, #-2]");
            __ASM volatile ("BICS   R12, R12, #0xFF00");
            __ASM volatile ("CMP    R12, #20");                 // check if we are serving a frame snapshot (SVC_Code.SupervisorCall__SnapshotProcessModeRegisters) ...
            __ASM volatile ("BNE    __SVCCALL");                // ... skip if not

            {
                //
                // Snapshot
                //
                __ASM volatile ("MOV    R2 , LR");              // Save LR and CONTROL, to save the status and privilege/stack mode
                __ASM volatile ("MRS    R3 , CONTROL");

                __ASM volatile ("STMDB  R0!, {R2-R11}");        // Push the SW stack frame, a total of 10 registers, including R2/3

                //
                // Now the stack has the full frame of 18 registers (RegistersOnStack)
                // We need to move the frame to the convenience space and then pop the 
                // Software frame before return
                //
                __ASM volatile ("MOV    R1 , %0" : /*output*/ : "r"(&sw_hw__frame[0]));
                __ASM volatile ("MOV    R2 , R0");              // R0 contains the beginning of the frame
                __ASM volatile ("MOV    R12, #18");

                __ASM volatile ("__COPY_TO_FRAME:");

                __ASM volatile ("LDR    R3, [R2]");
                __ASM volatile ("STR    R3, [R1]");

                __ASM volatile ("ADD    R2 , #4");
                __ASM volatile ("ADD    R1 , #4");
                __ASM volatile ("SUBS   R12, #1");
                __ASM volatile ("CMP    R12, #0"); 
                __ASM volatile ("BNE   __COPY_TO_FRAME");

                __ASM volatile ("LDMIA    R0!, {R2-R11}");      // Unstack the SW frame (10 registers) before proceeding as usual
            }
        }
        //
        // End of snapshot
        //


        //
        // Normal service call
        //
        __ASM volatile ("__SVCCALL:");

        __ASM volatile ("MOV    R1, %0" : /*output*/ : "r"(&svc_exc_return) );
        __ASM volatile ("STR    LR, [R1]");


#if __FPU_USED != 0
        SVC_Handler_Zelig_VFP_NoFPContext();
#else
        SVC_Handler_Zelig();
#endif

        //
        // Push
        //
        //NotifySoftwareFrameSnapshot(); 

        __ASM volatile ("MOV    R1, %0" : /*output*/ : "r"(&svc_exc_return));
        __ASM volatile ("LDR    LR, [R1]");

        __ASM volatile ("BX     LR");
    }

    //--//
    //--//
    //--//

    //
    // The managed portion of the PendSV_Handler
    //
    extern void PendSV_Handler_Zelig_VFP();
    extern void PendSV_Handler_Zelig();

    //
    // A convenience storage space to signal when the floating
    // point context is active 
    //

    __attribute__((naked)) void PendSV_Handler(void)
    {
        __ASM volatile ("MRS      R0, PSP");                 // Save current process stack pointer value into R0

#if __FPU_USED != 0
        __ASM volatile ("ANDS     R1, LR, #0x10");           // When bit 4 in LR is zero (0), we need to stack FP registers as well 
        __ASM volatile ("CMP      R1, #0");                  // R1 is the second parameter of PendSV_Handler_Zelig_VFP_FullFPContext
        __ASM volatile ("IT       EQ");
        __ASM volatile ("VSTMDBEQ R0!, {S16-S31}");
#endif

        __ASM volatile ("MOV      R2, LR");                  // Save LR and CONTROL, to save the status and privilege/stack mode
        __ASM volatile ("MRS      R3, CONTROL");

        __ASM volatile ("STMDB    R0!, {R2-R11}");           // Stack the SW stack frame, a total of 10 registers, including R2/3

#if __FPU_USED != 0
        PendSV_Handler_Zelig_VFP();
#else
        PendSV_Handler_Zelig();                              // Perform context switch, practically setting the stack pointer for the next task
#endif

        __ASM volatile ("LDMIA    R0!, {R2-R11}");           // Unstack the next tasks state

        __ASM volatile ("MOV      LR, R2");                  // Restore LR and CONTROL, to restore the status and privilege/stack mode
        __ASM volatile ("MSR      CONTROL, R3");
        __ASM volatile ("ISB");                              // architectural recommendation, always use ISB after updating control register

#if __FPU_USED != 0
        __ASM volatile ("ANDS     R1, LR, #0x10");               // When bit 4 in LR is zero (0), we need to stack FP registers as well 
        __ASM volatile ("CMP      R1, #0");
        __ASM volatile ("IT       EQ");
        __ASM volatile ("VLDMIAEQ R0!, {S16-S31}");
#endif

        __ASM volatile ("MSR      PSP, R0");                 // update stack pointer to correct location after unstacking 

        __ASM volatile ("BX       LR");
    }

    //--//
    //--//
    //--//

    void CUSTOM_STUB_K64F_DisableMPU()
    {
        uint32_t CESR = *((uint32_t volatile *)0x4000D000);

        CESR &= ~(0x00000001);

        *((uint32_t volatile *)0x4000D000) = CESR;
    }

    // According to http://www.st.com/st-web-ui/static/active/en/resource/technical/document/programming_manual/DM00046982.pdf
    // pg. 186 the MPU is off by default
    // TODO: Someone should double-check I got the address right
    void CUSTOM_STUB_STM32F411_DisableMPU()
    {
        /*uint32_t CESR = *((uint32_t volatile *)0xE000ED94);

        CESR &= ~(0x00000001);

        *((uint32_t volatile *)0xE000ED94) = CESR;*/
    }
}
