//
// This file is part of the µOS++ III distribution.
// Copyright (c) 2014 Liviu Ionescu.
//

// ----------------------------------------------------------------------------

#include "cortexm/ExceptionHandlers.h"

// ----------------------------------------------------------------------------

void __attribute__((weak))
Default_Handler(void);

// Forward declaration of the specific IRQ handlers. These are aliased
// to the Default_Handler, which is a 'forever' loop. When the application
// defines a handler (with the same name), this will automatically take
// precedence over these weak definitions

void __attribute__ ((weak, alias ("Default_Handler")))
WWDG_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
PVD_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
TAMP_STAMP_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
RTC_WKUP_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
FLASH_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
RCC_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
EXTI0_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
EXTI1_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
EXTI2_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
EXTI3_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
EXTI4_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
DMA1_Stream0_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
DMA1_Stream1_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
DMA1_Stream2_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
DMA1_Stream3_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
DMA1_Stream4_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
DMA1_Stream5_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
DMA1_Stream6_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
ADC_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
CAN1_TX_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
CAN1_RX0_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
CAN1_RX1_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
CAN1_SCE_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
EXTI9_5_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
TIM1_BRK_TIM9_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
TIM1_UP_TIM10_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
TIM1_TRG_COM_TIM11_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
TIM1_CC_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
TIM2_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
TIM3_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
TIM4_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
I2C1_EV_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
I2C1_ER_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
I2C2_EV_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
I2C2_ER_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
SPI1_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
SPI2_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
USART1_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
USART2_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
USART3_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
EXTI15_10_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
RTC_Alarm_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
OTG_FS_WKUP_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
TIM8_BRK_TIM12_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
TIM8_UP_TIM13_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
TIM8_TRG_COM_TIM14_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
TIM8_CC_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
DMA1_Stream7_IRQHandler(void);
//void __attribute__ ((weak, alias ("Default_Handler")))
//FSMC_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
SDIO_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
TIM5_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
SPI3_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
UART4_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
UART5_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
TIM6_DAC_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
TIM7_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
DMA2_Stream0_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
DMA2_Stream1_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
DMA2_Stream2_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
DMA2_Stream3_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
DMA2_Stream4_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
ETH_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
ETH_WKUP_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
CAN2_TX_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
CAN2_RX0_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
CAN2_RX1_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
CAN2_SCE_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
OTG_FS_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
DMA2_Stream5_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
DMA2_Stream6_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
DMA2_Stream7_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
USART6_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
I2C3_EV_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
I2C3_ER_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
OTG_HS_EP1_OUT_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
OTG_HS_EP1_IN_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
OTG_HS_WKUP_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
OTG_HS_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
DCMI_IRQHandler(void);
//void __attribute__ ((weak, alias ("Default_Handler")))
//CRYP_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
HASH_RNG_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
FPU_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
SPI4_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
SPI5_IRQHandler(void);

#if defined(STM32F427xx) || defined(STM32F437xx) || defined(STM32F429xx) || defined(STM32F439xx)

void __attribute__ ((weak, alias ("Default_Handler")))
UART7_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
UART8_IRQHandler(void);
//void __attribute__ ((weak, alias ("Default_Handler")))
//SPI5_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
SPI6_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
SAI1_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
LTDC_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
LTDC_ER_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
DMA2D_IRQHandler(void);
void __attribute__ ((weak, alias ("Default_Handler")))
FMC_IRQHandler(void);

#endif

// ----------------------------------------------------------------------------

extern unsigned int _estack;

typedef void
(* const pHandler)(void);

// ----------------------------------------------------------------------------

// The vector table.
// This relies on the linker script to place at correct location in memory.

__attribute__ ((section(".isr_vector")))
pHandler __isr_vectors[] =
  {
  // Core Level - CM4
      (pHandler) &_estack,                      // The initial stack pointer
      Reset_Handler,                            // The reset handler

      NMI_Handler,                              // The NMI handler
      HardFault_Handler,                        // The hard fault handler

#if defined(__ARM_ARCH_7M__) || defined(__ARM_ARCH_7EM__)
      MemManage_Handler,                        // The MPU fault handler
      BusFault_Handler,                        // The bus fault handler
      UsageFault_Handler,                        // The usage fault handler
#else
      0, 0, 0,                                  // Reserved
#endif
      0,                                        // Reserved
      0,                                        // Reserved
      0,                                        // Reserved
      0,                                        // Reserved
      SVC_Handler,                              // SVCall handler
#if defined(__ARM_ARCH_7M__) || defined(__ARM_ARCH_7EM__)
      DebugMon_Handler,                         // Debug monitor handler
#else
      0,                                        // Reserved
#endif
      0,                                        // Reserved
      PendSV_Handler,                           // The PendSV handler
      SysTick_Handler,                          // The SysTick handler

      // ----------------------------------------------------------------------
      // Chip Level - STM32F4xx
      WWDG_IRQHandler,        // Window WatchDog
      PVD_IRQHandler, // PVD through EXTI Line detection
      TAMP_STAMP_IRQHandler,  // Tamper and TimeStamps through the EXTI line
      RTC_WKUP_IRQHandler,    // RTC Wakeup through the EXTI line
      FLASH_IRQHandler,       // FLASH
      RCC_IRQHandler, // RCC
      EXTI0_IRQHandler,       // EXTI Line0
      EXTI1_IRQHandler,       // EXTI Line1
      EXTI2_IRQHandler,       // EXTI Line2
      EXTI3_IRQHandler,       // EXTI Line3
      EXTI4_IRQHandler,       // EXTI Line4
      DMA1_Stream0_IRQHandler,        // DMA1 Stream 0
      DMA1_Stream1_IRQHandler,        // DMA1 Stream 1
      DMA1_Stream2_IRQHandler,        // DMA1 Stream 2
      DMA1_Stream3_IRQHandler,        // DMA1 Stream 3
      DMA1_Stream4_IRQHandler,        // DMA1 Stream 4
      DMA1_Stream5_IRQHandler,        // DMA1 Stream 5
      DMA1_Stream6_IRQHandler,        // DMA1 Stream 6
      ADC_IRQHandler, // ADC1

#if defined(STM32F401xC) || defined(STM32F401xE)

      0,      // Reserved
      0,      // Reserved
      0,      // Reserved
      0,      // Reserved
      EXTI9_5_IRQHandler,     // External Line[9:5]s
      TIM1_BRK_TIM9_IRQHandler,       // TIM1 Break and TIM9
      TIM1_UP_TIM10_IRQHandler,       // TIM1 Update and TIM10
      TIM1_TRG_COM_TIM11_IRQHandler,  // TIM1 Trigger and Commutation and TIM11
      TIM1_CC_IRQHandler,     // TIM1 Capture Compare
      TIM2_IRQHandler,        // TIM2
      TIM3_IRQHandler,        // TIM3
      TIM4_IRQHandler,        // TIM4
      I2C1_EV_IRQHandler,     // I2C1 Event
      I2C1_ER_IRQHandler,     // I2C1 Error
      I2C2_EV_IRQHandler,     // I2C2 Event
      I2C2_ER_IRQHandler,     // I2C2 Error
      SPI1_IRQHandler,        // SPI1
      SPI2_IRQHandler,        // SPI2
      USART1_IRQHandler,      // USART1
      USART2_IRQHandler,      // USART2
      0,      // Reserved
      EXTI15_10_IRQHandler,   // External Line[15:10]s
      RTC_Alarm_IRQHandler,   // RTC Alarm (A and B) through EXTI Line
      OTG_FS_WKUP_IRQHandler, // USB OTG FS Wakeup through EXTI line
      0,      // Reserved
      0,      // Reserved
      0,      // Reserved
      0,      // Reserved
      DMA1_Stream7_IRQHandler,        // DMA1 Stream7
      0,      // Reserved
      SDIO_IRQHandler,        // SDIO
      TIM5_IRQHandler,        // TIM5
      SPI3_IRQHandler,        // SPI3
      0,      // Reserved
      0,      // Reserved
      0,      // Reserved
      0,      // Reserved
      DMA2_Stream0_IRQHandler,        // DMA2 Stream 0
      DMA2_Stream1_IRQHandler,        // DMA2 Stream 1
      DMA2_Stream2_IRQHandler,        // DMA2 Stream 2
      DMA2_Stream3_IRQHandler,        // DMA2 Stream 3
      DMA2_Stream4_IRQHandler,        // DMA2 Stream 4
      0,      // Reserved
      0,      // Reserved
      0,      // Reserved
      0,      // Reserved
      0,      // Reserved
      0,      // Reserved
      OTG_FS_IRQHandler,      // USB OTG FS
      DMA2_Stream5_IRQHandler,        // DMA2 Stream 5
      DMA2_Stream6_IRQHandler,        // DMA2 Stream 6
      DMA2_Stream7_IRQHandler,        // DMA2 Stream 7
      USART6_IRQHandler,      // USART6
      I2C3_EV_IRQHandler,     // I2C3 event
      I2C3_ER_IRQHandler,     // I2C3 error
      0,      // Reserved
      0,      // Reserved
      0,      // Reserved
      0,      // Reserved
      0,      // Reserved
      0,      // Reserved
      0,      // Reserved
      FPU_IRQHandler, // FPU
      0,      // Reserved
      0,      // Reserved
      SPI4_IRQHandler        // SPI4

#elif defined (STM32F405xx)

      CAN1_TX_IRQHandler,     // CAN1 TX
      CAN1_RX0_IRQHandler,    // CAN1 RX0
      CAN1_RX1_IRQHandler,    // CAN1 RX1
      CAN1_SCE_IRQHandler,    // CAN1 SCE
      EXTI9_5_IRQHandler,     // External Line[9:5]s
      TIM1_BRK_TIM9_IRQHandler,       // TIM1 Break and TIM9
      TIM1_UP_TIM10_IRQHandler,       // TIM1 Update and TIM10
      TIM1_TRG_COM_TIM11_IRQHandler,  // TIM1 Trigger and Commutation and TIM11
      TIM1_CC_IRQHandler,     // TIM1 Capture Compare
      TIM2_IRQHandler,        // TIM2
      TIM3_IRQHandler,        // TIM3
      TIM4_IRQHandler,        // TIM4
      I2C1_EV_IRQHandler,     // I2C1 Event
      I2C1_ER_IRQHandler,     // I2C1 Error
      I2C2_EV_IRQHandler,     // I2C2 Event
      I2C2_ER_IRQHandler,     // I2C2 Error
      SPI1_IRQHandler,        // SPI1
      SPI2_IRQHandler,        // SPI2
      USART1_IRQHandler,      // USART1
      USART2_IRQHandler,      // USART2
      USART3_IRQHandler,      // USART3
      EXTI15_10_IRQHandler,   // External Line[15:10]s
      RTC_Alarm_IRQHandler,   // RTC Alarm (A and B) through EXTI Line
      OTG_FS_WKUP_IRQHandler, // USB OTG FS Wakeup through EXTI line
      TIM8_BRK_TIM12_IRQHandler,      // TIM8 Break and TIM12
      TIM8_UP_TIM13_IRQHandler,       // TIM8 Update and TIM13
      TIM8_TRG_COM_TIM14_IRQHandler,  // TIM8 Trigger and Commutation and TIM14
      TIM8_CC_IRQHandler,     // TIM8 Capture Compare
      DMA1_Stream7_IRQHandler,        // DMA1 Stream7
      FSMC_IRQHandler,        // FSMC
      SDIO_IRQHandler,        // SDIO
      TIM5_IRQHandler,        // TIM5
      SPI3_IRQHandler,        // SPI3
      UART4_IRQHandler,       // UART4
      UART5_IRQHandler,       // UART5
      TIM6_DAC_IRQHandler,    // TIM6 and DAC1&2 underrun errors
      TIM7_IRQHandler,        // TIM7
      DMA2_Stream0_IRQHandler,        // DMA2 Stream 0
      DMA2_Stream1_IRQHandler,        // DMA2 Stream 1
      DMA2_Stream2_IRQHandler,        // DMA2 Stream 2
      DMA2_Stream3_IRQHandler,        // DMA2 Stream 3
      DMA2_Stream4_IRQHandler,        // DMA2 Stream 4
      0,      // Reserved
      0,      // Reserved
      CAN2_TX_IRQHandler,     // CAN2 TX
      CAN2_RX0_IRQHandler,    // CAN2 RX0
      CAN2_RX1_IRQHandler,    // CAN2 RX1
      CAN2_SCE_IRQHandler,    // CAN2 SCE
      OTG_FS_IRQHandler,      // USB OTG FS
      DMA2_Stream5_IRQHandler,        // DMA2 Stream 5
      DMA2_Stream6_IRQHandler,        // DMA2 Stream 6
      DMA2_Stream7_IRQHandler,        // DMA2 Stream 7
      USART6_IRQHandler,      // USART6
      I2C3_EV_IRQHandler,     // I2C3 event
      I2C3_ER_IRQHandler,     // I2C3 error
      OTG_HS_EP1_OUT_IRQHandler,      // USB OTG HS End Point 1 Out
      OTG_HS_EP1_IN_IRQHandler,       // USB OTG HS End Point 1 In
      OTG_HS_WKUP_IRQHandler, // USB OTG HS Wakeup through EXTI
      OTG_HS_IRQHandler,      // USB OTG HS
      0,      // Reserved
      0,      // Reserved
      HASH_RNG_IRQHandler,    // Hash and Rng
      FPU_IRQHandler // FPU

#elif defined (STM32F407xx)

      CAN1_TX_IRQHandler,     // CAN1 TX
      CAN1_RX0_IRQHandler,    // CAN1 RX0
      CAN1_RX1_IRQHandler,    // CAN1 RX1
      CAN1_SCE_IRQHandler,    // CAN1 SCE
      EXTI9_5_IRQHandler,     // External Line[9:5]s
      TIM1_BRK_TIM9_IRQHandler,       // TIM1 Break and TIM9
      TIM1_UP_TIM10_IRQHandler,       // TIM1 Update and TIM10
      TIM1_TRG_COM_TIM11_IRQHandler,  // TIM1 Trigger and Commutation and TIM11
      TIM1_CC_IRQHandler,     // TIM1 Capture Compare
      TIM2_IRQHandler,        // TIM2
      TIM3_IRQHandler,        // TIM3
      TIM4_IRQHandler,        // TIM4
      I2C1_EV_IRQHandler,     // I2C1 Event
      I2C1_ER_IRQHandler,     // I2C1 Error
      I2C2_EV_IRQHandler,     // I2C2 Event
      I2C2_ER_IRQHandler,     // I2C2 Error
      SPI1_IRQHandler,        // SPI1
      SPI2_IRQHandler,        // SPI2
      USART1_IRQHandler,      // USART1
      USART2_IRQHandler,      // USART2
      USART3_IRQHandler,      // USART3
      EXTI15_10_IRQHandler,   // External Line[15:10]s
      RTC_Alarm_IRQHandler,   // RTC Alarm (A and B) through EXTI Line
      OTG_FS_WKUP_IRQHandler, // USB OTG FS Wakeup through EXTI line
      TIM8_BRK_TIM12_IRQHandler,      // TIM8 Break and TIM12
      TIM8_UP_TIM13_IRQHandler,       // TIM8 Update and TIM13
      TIM8_TRG_COM_TIM14_IRQHandler,  // TIM8 Trigger and Commutation and TIM14
      TIM8_CC_IRQHandler,     // TIM8 Capture Compare
      DMA1_Stream7_IRQHandler,        // DMA1 Stream7
      FSMC_IRQHandler,        // FSMC
      SDIO_IRQHandler,        // SDIO
      TIM5_IRQHandler,        // TIM5
      SPI3_IRQHandler,        // SPI3
      UART4_IRQHandler,       // UART4
      UART5_IRQHandler,       // UART5
      TIM6_DAC_IRQHandler,    // TIM6 and DAC1&2 underrun errors
      TIM7_IRQHandler,        // TIM7
      DMA2_Stream0_IRQHandler,        // DMA2 Stream 0
      DMA2_Stream1_IRQHandler,        // DMA2 Stream 1
      DMA2_Stream2_IRQHandler,        // DMA2 Stream 2
      DMA2_Stream3_IRQHandler,        // DMA2 Stream 3
      DMA2_Stream4_IRQHandler,        // DMA2 Stream 4
      ETH_IRQHandler, // Ethernet
      ETH_WKUP_IRQHandler,    // Ethernet Wakeup through EXTI line
      CAN2_TX_IRQHandler,     // CAN2 TX
      CAN2_RX0_IRQHandler,    // CAN2 RX0
      CAN2_RX1_IRQHandler,    // CAN2 RX1
      CAN2_SCE_IRQHandler,    // CAN2 SCE
      OTG_FS_IRQHandler,      // USB OTG FS
      DMA2_Stream5_IRQHandler,        // DMA2 Stream 5
      DMA2_Stream6_IRQHandler,        // DMA2 Stream 6
      DMA2_Stream7_IRQHandler,        // DMA2 Stream 7
      USART6_IRQHandler,      // USART6
      I2C3_EV_IRQHandler,     // I2C3 event
      I2C3_ER_IRQHandler,     // I2C3 error
      OTG_HS_EP1_OUT_IRQHandler,      // USB OTG HS End Point 1 Out
      OTG_HS_EP1_IN_IRQHandler,       // USB OTG HS End Point 1 In
      OTG_HS_WKUP_IRQHandler, // USB OTG HS Wakeup through EXTI
      OTG_HS_IRQHandler,      // USB OTG HS
      DCMI_IRQHandler,        // DCMI
      0,      // CRYP crypto
      HASH_RNG_IRQHandler,    // Hash and Rng
      FPU_IRQHandler // FPU

#elif defined(STM32F411xE)

      WWDG_IRQHandler,        // Window WatchDog
      PVD_IRQHandler, // PVD through EXTI Line detection
      TAMP_STAMP_IRQHandler,  // Tamper and TimeStamps through the EXTI line
      RTC_WKUP_IRQHandler,    // RTC Wakeup through the EXTI line
      FLASH_IRQHandler,       // FLASH
      RCC_IRQHandler, // RCC
      EXTI0_IRQHandler,       // EXTI Line0
      EXTI1_IRQHandler,       // EXTI Line1
      EXTI2_IRQHandler,       // EXTI Line2
      EXTI3_IRQHandler,       // EXTI Line3
      EXTI4_IRQHandler,       // EXTI Line4
      DMA1_Stream0_IRQHandler,        // DMA1 Stream 0
      DMA1_Stream1_IRQHandler,        // DMA1 Stream 1
      DMA1_Stream2_IRQHandler,        // DMA1 Stream 2
      DMA1_Stream3_IRQHandler,        // DMA1 Stream 3
      DMA1_Stream4_IRQHandler,        // DMA1 Stream 4
      DMA1_Stream5_IRQHandler,        // DMA1 Stream 5
      DMA1_Stream6_IRQHandler,        // DMA1 Stream 6
      ADC_IRQHandler, // ADC1, ADC2 and ADC3s
      0,      // Reserved
      0,      // Reserved
      0,      // Reserved
      0,      // Reserved
      EXTI9_5_IRQHandler,     // External Line[9:5]s
      TIM1_BRK_TIM9_IRQHandler,       // TIM1 Break and TIM9
      TIM1_UP_TIM10_IRQHandler,       // TIM1 Update and TIM10
      TIM1_TRG_COM_TIM11_IRQHandler,  // TIM1 Trigger and Commutation and TIM11
      TIM1_CC_IRQHandler,     // TIM1 Capture Compare
      TIM2_IRQHandler,        // TIM2
      TIM3_IRQHandler,        // TIM3
      TIM4_IRQHandler,        // TIM4
      I2C1_EV_IRQHandler,     // I2C1 Event
      I2C1_ER_IRQHandler,     // I2C1 Error
      I2C2_EV_IRQHandler,     // I2C2 Event
      I2C2_ER_IRQHandler,     // I2C2 Error
      SPI1_IRQHandler,        // SPI1
      SPI2_IRQHandler,        // SPI2
      USART1_IRQHandler,      // USART1
      USART2_IRQHandler,      // USART2
      0,      // Reserved
      EXTI15_10_IRQHandler,   // External Line[15:10]s
      RTC_Alarm_IRQHandler,   // RTC Alarm (A and B) through EXTI Line
      OTG_FS_WKUP_IRQHandler, // USB OTG FS Wakeup through EXTI line
      0,      // Reserved
      0,      // Reserved
      0,      // Reserved
      0,      // Reserved
      DMA1_Stream7_IRQHandler,        // DMA1 Stream7
      0,      // Reserved
      SDIO_IRQHandler,        // SDIO
      TIM5_IRQHandler,        // TIM5
      SPI3_IRQHandler,        // SPI3
      0,      // Reserved
      0,      // Reserved
      0,      // Reserved
      0,      // Reserved
      DMA2_Stream0_IRQHandler,        // DMA2 Stream 0
      DMA2_Stream1_IRQHandler,        // DMA2 Stream 1
      DMA2_Stream2_IRQHandler,        // DMA2 Stream 2
      DMA2_Stream3_IRQHandler,        // DMA2 Stream 3
      DMA2_Stream4_IRQHandler,        // DMA2 Stream 4
      0,      // Reserved
      0,      // Reserved
      0,      // Reserved
      0,      // Reserved
      0,      // Reserved
      0,      // Reserved
      OTG_FS_IRQHandler,      // USB OTG FS
      DMA2_Stream5_IRQHandler,        // DMA2 Stream 5
      DMA2_Stream6_IRQHandler,        // DMA2 Stream 6
      DMA2_Stream7_IRQHandler,        // DMA2 Stream 7
      USART6_IRQHandler,      // USART6
      I2C3_EV_IRQHandler,     // I2C3 event
      I2C3_ER_IRQHandler,     // I2C3 error
      0,      // Reserved
      0,      // Reserved
      0,      // Reserved
      0,      // Reserved
      0,      // Reserved
      0,      // Reserved
      0,      // Reserved
      FPU_IRQHandler, // FPU
      0,      // Reserved
      0,      // Reserved
      SPI4_IRQHandler,        // SPI4
      SPI5_IRQHandler,        // SPI5

#elif defined (STM32F415xx)

      CAN1_TX_IRQHandler,     // CAN1 TX
      CAN1_RX0_IRQHandler,    // CAN1 RX0
      CAN1_RX1_IRQHandler,    // CAN1 RX1
      CAN1_SCE_IRQHandler,    // CAN1 SCE
      EXTI9_5_IRQHandler,     // External Line[9:5]s
      TIM1_BRK_TIM9_IRQHandler,       // TIM1 Break and TIM9
      TIM1_UP_TIM10_IRQHandler,       // TIM1 Update and TIM10
      TIM1_TRG_COM_TIM11_IRQHandler,  // TIM1 Trigger and Commutation and TIM11
      TIM1_CC_IRQHandler,     // TIM1 Capture Compare
      TIM2_IRQHandler,        // TIM2
      TIM3_IRQHandler,        // TIM3
      TIM4_IRQHandler,        // TIM4
      I2C1_EV_IRQHandler,     // I2C1 Event
      I2C1_ER_IRQHandler,     // I2C1 Error
      I2C2_EV_IRQHandler,     // I2C2 Event
      I2C2_ER_IRQHandler,     // I2C2 Error
      SPI1_IRQHandler,        // SPI1
      SPI2_IRQHandler,        // SPI2
      USART1_IRQHandler,      // USART1
      USART2_IRQHandler,      // USART2
      USART3_IRQHandler,      // USART3
      EXTI15_10_IRQHandler,   // External Line[15:10]s
      RTC_Alarm_IRQHandler,   // RTC Alarm (A and B) through EXTI Line
      OTG_FS_WKUP_IRQHandler, // USB OTG FS Wakeup through EXTI line
      TIM8_BRK_TIM12_IRQHandler,      // TIM8 Break and TIM12
      TIM8_UP_TIM13_IRQHandler,       // TIM8 Update and TIM13
      TIM8_TRG_COM_TIM14_IRQHandler,  // TIM8 Trigger and Commutation and TIM14
      TIM8_CC_IRQHandler,     // TIM8 Capture Compare
      DMA1_Stream7_IRQHandler,        // DMA1 Stream7
      FSMC_IRQHandler,        // FSMC
      SDIO_IRQHandler,        // SDIO
      TIM5_IRQHandler,        // TIM5
      SPI3_IRQHandler,        // SPI3
      UART4_IRQHandler,       // UART4
      UART5_IRQHandler,       // UART5
      TIM6_DAC_IRQHandler,    // TIM6 and DAC1&2 underrun errors
      TIM7_IRQHandler,        // TIM7
      DMA2_Stream0_IRQHandler,        // DMA2 Stream 0
      DMA2_Stream1_IRQHandler,        // DMA2 Stream 1
      DMA2_Stream2_IRQHandler,        // DMA2 Stream 2
      DMA2_Stream3_IRQHandler,        // DMA2 Stream 3
      DMA2_Stream4_IRQHandler,        // DMA2 Stream 4
      0,      // Reserved
      0,      // Reserved
      CAN2_TX_IRQHandler,     // CAN2 TX
      CAN2_RX0_IRQHandler,    // CAN2 RX0
      CAN2_RX1_IRQHandler,    // CAN2 RX1
      CAN2_SCE_IRQHandler,    // CAN2 SCE
      OTG_FS_IRQHandler,      // USB OTG FS
      DMA2_Stream5_IRQHandler,        // DMA2 Stream 5
      DMA2_Stream6_IRQHandler,        // DMA2 Stream 6
      DMA2_Stream7_IRQHandler,        // DMA2 Stream 7
      USART6_IRQHandler,      // USART6
      I2C3_EV_IRQHandler,     // I2C3 event
      I2C3_ER_IRQHandler,     // I2C3 error
      OTG_HS_EP1_OUT_IRQHandler,      // USB OTG HS End Point 1 Out
      OTG_HS_EP1_IN_IRQHandler,       // USB OTG HS End Point 1 In
      OTG_HS_WKUP_IRQHandler, // USB OTG HS Wakeup through EXTI
      OTG_HS_IRQHandler,      // USB OTG HS
      0,      // Reserved
      CRYP_IRQHandler,        // CRYP crypto
      HASH_RNG_IRQHandler,    // Hash and Rng
      FPU_IRQHandler // FPU

#elif defined (STM32F417xx)

      CAN1_TX_IRQHandler,     // CAN1 TX
      CAN1_RX0_IRQHandler,    // CAN1 RX0
      CAN1_RX1_IRQHandler,    // CAN1 RX1
      CAN1_SCE_IRQHandler,    // CAN1 SCE
      EXTI9_5_IRQHandler,     // External Line[9:5]s
      TIM1_BRK_TIM9_IRQHandler,       // TIM1 Break and TIM9
      TIM1_UP_TIM10_IRQHandler,       // TIM1 Update and TIM10
      TIM1_TRG_COM_TIM11_IRQHandler,  // TIM1 Trigger and Commutation and TIM11
      TIM1_CC_IRQHandler,     // TIM1 Capture Compare
      TIM2_IRQHandler,        // TIM2
      TIM3_IRQHandler,        // TIM3
      TIM4_IRQHandler,        // TIM4
      I2C1_EV_IRQHandler,     // I2C1 Event
      I2C1_ER_IRQHandler,     // I2C1 Error
      I2C2_EV_IRQHandler,     // I2C2 Event
      I2C2_ER_IRQHandler,     // I2C2 Error
      SPI1_IRQHandler,        // SPI1
      SPI2_IRQHandler,        // SPI2
      USART1_IRQHandler,      // USART1
      USART2_IRQHandler,      // USART2
      USART3_IRQHandler,      // USART3
      EXTI15_10_IRQHandler,   // External Line[15:10]s
      RTC_Alarm_IRQHandler,   // RTC Alarm (A and B) through EXTI Line
      OTG_FS_WKUP_IRQHandler, // USB OTG FS Wakeup through EXTI line
      TIM8_BRK_TIM12_IRQHandler,      // TIM8 Break and TIM12
      TIM8_UP_TIM13_IRQHandler,       // TIM8 Update and TIM13
      TIM8_TRG_COM_TIM14_IRQHandler,  // TIM8 Trigger and Commutation and TIM14
      TIM8_CC_IRQHandler,     // TIM8 Capture Compare
      DMA1_Stream7_IRQHandler,        // DMA1 Stream7
      FSMC_IRQHandler,        // FSMC
      SDIO_IRQHandler,        // SDIO
      TIM5_IRQHandler,        // TIM5
      SPI3_IRQHandler,        // SPI3
      UART4_IRQHandler,       // UART4
      UART5_IRQHandler,       // UART5
      TIM6_DAC_IRQHandler,    // TIM6 and DAC1&2 underrun errors
      TIM7_IRQHandler,        // TIM7
      DMA2_Stream0_IRQHandler,        // DMA2 Stream 0
      DMA2_Stream1_IRQHandler,        // DMA2 Stream 1
      DMA2_Stream2_IRQHandler,        // DMA2 Stream 2
      DMA2_Stream3_IRQHandler,        // DMA2 Stream 3
      DMA2_Stream4_IRQHandler,        // DMA2 Stream 4
      ETH_IRQHandler, // Ethernet
      ETH_WKUP_IRQHandler,    // Ethernet Wakeup through EXTI line
      CAN2_TX_IRQHandler,     // CAN2 TX
      CAN2_RX0_IRQHandler,    // CAN2 RX0
      CAN2_RX1_IRQHandler,    // CAN2 RX1
      CAN2_SCE_IRQHandler,    // CAN2 SCE
      OTG_FS_IRQHandler,      // USB OTG FS
      DMA2_Stream5_IRQHandler,        // DMA2 Stream 5
      DMA2_Stream6_IRQHandler,        // DMA2 Stream 6
      DMA2_Stream7_IRQHandler,        // DMA2 Stream 7
      USART6_IRQHandler,      // USART6
      I2C3_EV_IRQHandler,     // I2C3 event
      I2C3_ER_IRQHandler,     // I2C3 error
      OTG_HS_EP1_OUT_IRQHandler,      // USB OTG HS End Point 1 Out
      OTG_HS_EP1_IN_IRQHandler,       // USB OTG HS End Point 1 In
      OTG_HS_WKUP_IRQHandler, // USB OTG HS Wakeup through EXTI
      OTG_HS_IRQHandler,      // USB OTG HS
      DCMI_IRQHandler,        // DCMI
      CRYP_IRQHandler,        // CRYP crypto
      HASH_RNG_IRQHandler,    // Hash and Rng
      FPU_IRQHandler // FPU

#elif defined(STM32F427xx)

      CAN1_TX_IRQHandler,     // CAN1 TX
      CAN1_RX0_IRQHandler,    // CAN1 RX0
      CAN1_RX1_IRQHandler,    // CAN1 RX1
      CAN1_SCE_IRQHandler,    // CAN1 SCE
      EXTI9_5_IRQHandler,     // External Line[9:5]s
      TIM1_BRK_TIM9_IRQHandler,       // TIM1 Break and TIM9
      TIM1_UP_TIM10_IRQHandler,       // TIM1 Update and TIM10
      TIM1_TRG_COM_TIM11_IRQHandler,  // TIM1 Trigger and Commutation and TIM11
      TIM1_CC_IRQHandler,     // TIM1 Capture Compare
      TIM2_IRQHandler,        // TIM2
      TIM3_IRQHandler,        // TIM3
      TIM4_IRQHandler,        // TIM4
      I2C1_EV_IRQHandler,     // I2C1 Event
      I2C1_ER_IRQHandler,     // I2C1 Error
      I2C2_EV_IRQHandler,     // I2C2 Event
      I2C2_ER_IRQHandler,     // I2C2 Error
      SPI1_IRQHandler,        // SPI1
      SPI2_IRQHandler,        // SPI2
      USART1_IRQHandler,      // USART1
      USART2_IRQHandler,      // USART2
      USART3_IRQHandler,      // USART3
      EXTI15_10_IRQHandler,   // External Line[15:10]s
      RTC_Alarm_IRQHandler,   // RTC Alarm (A and B) through EXTI Line
      OTG_FS_WKUP_IRQHandler, // USB OTG FS Wakeup through EXTI line
      TIM8_BRK_TIM12_IRQHandler,      // TIM8 Break and TIM12
      TIM8_UP_TIM13_IRQHandler,       // TIM8 Update and TIM13
      TIM8_TRG_COM_TIM14_IRQHandler,  // TIM8 Trigger and Commutation and TIM14
      TIM8_CC_IRQHandler,     // TIM8 Capture Compare
      DMA1_Stream7_IRQHandler,        // DMA1 Stream7
      FMC_IRQHandler, // FMC
      SDIO_IRQHandler,        // SDIO
      TIM5_IRQHandler,        // TIM5
      SPI3_IRQHandler,        // SPI3
      UART4_IRQHandler,       // UART4
      UART5_IRQHandler,       // UART5
      TIM6_DAC_IRQHandler,    // TIM6 and DAC1&2 underrun errors
      TIM7_IRQHandler,        // TIM7
      DMA2_Stream0_IRQHandler,        // DMA2 Stream 0
      DMA2_Stream1_IRQHandler,        // DMA2 Stream 1
      DMA2_Stream2_IRQHandler,        // DMA2 Stream 2
      DMA2_Stream3_IRQHandler,        // DMA2 Stream 3
      DMA2_Stream4_IRQHandler,        // DMA2 Stream 4
      ETH_IRQHandler, // Ethernet
      ETH_WKUP_IRQHandler,    // Ethernet Wakeup through EXTI line
      CAN2_TX_IRQHandler,     // CAN2 TX
      CAN2_RX0_IRQHandler,    // CAN2 RX0
      CAN2_RX1_IRQHandler,    // CAN2 RX1
      CAN2_SCE_IRQHandler,    // CAN2 SCE
      OTG_FS_IRQHandler,      // USB OTG FS
      DMA2_Stream5_IRQHandler,        // DMA2 Stream 5
      DMA2_Stream6_IRQHandler,        // DMA2 Stream 6
      DMA2_Stream7_IRQHandler,        // DMA2 Stream 7
      USART6_IRQHandler,      // USART6
      I2C3_EV_IRQHandler,     // I2C3 event
      I2C3_ER_IRQHandler,     // I2C3 error
      OTG_HS_EP1_OUT_IRQHandler,      // USB OTG HS End Point 1 Out
      OTG_HS_EP1_IN_IRQHandler,       // USB OTG HS End Point 1 In
      OTG_HS_WKUP_IRQHandler, // USB OTG HS Wakeup through EXTI
      OTG_HS_IRQHandler,      // USB OTG HS
      DCMI_IRQHandler,        // DCMI
      0,      // Reserved
      HASH_RNG_IRQHandler,    // Hash and Rng
      FPU_IRQHandler, // FPU
      UART7_IRQHandler,       // UART7
      UART8_IRQHandler,       // UART8
      SPI4_IRQHandler,        // SPI4
      SPI5_IRQHandler,        // SPI5
      SPI6_IRQHandler,        // SPI6
      SAI1_IRQHandler,        // SAI1
      0,      // Reserved
      0,      // Reserved
      DMA2D_IRQHandler       // DMA2D

#elif defined(STM32F429xx)

      CAN1_TX_IRQHandler,     // CAN1 TX
      CAN1_RX0_IRQHandler,    // CAN1 RX0
      CAN1_RX1_IRQHandler,    // CAN1 RX1
      CAN1_SCE_IRQHandler,    // CAN1 SCE
      EXTI9_5_IRQHandler,     // External Line[9:5]s
      TIM1_BRK_TIM9_IRQHandler,       // TIM1 Break and TIM9
      TIM1_UP_TIM10_IRQHandler,       // TIM1 Update and TIM10
      TIM1_TRG_COM_TIM11_IRQHandler,  // TIM1 Trigger and Commutation and TIM11
      TIM1_CC_IRQHandler,     // TIM1 Capture Compare
      TIM2_IRQHandler,        // TIM2
      TIM3_IRQHandler,        // TIM3
      TIM4_IRQHandler,        // TIM4
      I2C1_EV_IRQHandler,     // I2C1 Event
      I2C1_ER_IRQHandler,     // I2C1 Error
      I2C2_EV_IRQHandler,     // I2C2 Event
      I2C2_ER_IRQHandler,     // I2C2 Error
      SPI1_IRQHandler,        // SPI1
      SPI2_IRQHandler,        // SPI2
      USART1_IRQHandler,      // USART1
      USART2_IRQHandler,      // USART2
      USART3_IRQHandler,      // USART3
      EXTI15_10_IRQHandler,   // External Line[15:10]s
      RTC_Alarm_IRQHandler,   // RTC Alarm (A and B) through EXTI Line
      OTG_FS_WKUP_IRQHandler, // USB OTG FS Wakeup through EXTI line
      TIM8_BRK_TIM12_IRQHandler,      // TIM8 Break and TIM12
      TIM8_UP_TIM13_IRQHandler,       // TIM8 Update and TIM13
      TIM8_TRG_COM_TIM14_IRQHandler,  // TIM8 Trigger and Commutation and TIM14
      TIM8_CC_IRQHandler,     // TIM8 Capture Compare
      DMA1_Stream7_IRQHandler,        // DMA1 Stream7
      FMC_IRQHandler, // FMC
      SDIO_IRQHandler,        // SDIO
      TIM5_IRQHandler,        // TIM5
      SPI3_IRQHandler,        // SPI3
      UART4_IRQHandler,       // UART4
      UART5_IRQHandler,       // UART5
      TIM6_DAC_IRQHandler,    // TIM6 and DAC1&2 underrun errors
      TIM7_IRQHandler,        // TIM7
      DMA2_Stream0_IRQHandler,        // DMA2 Stream 0
      DMA2_Stream1_IRQHandler,        // DMA2 Stream 1
      DMA2_Stream2_IRQHandler,        // DMA2 Stream 2
      DMA2_Stream3_IRQHandler,        // DMA2 Stream 3
      DMA2_Stream4_IRQHandler,        // DMA2 Stream 4
      ETH_IRQHandler, // Ethernet
      ETH_WKUP_IRQHandler,    // Ethernet Wakeup through EXTI line
      CAN2_TX_IRQHandler,     // CAN2 TX
      CAN2_RX0_IRQHandler,    // CAN2 RX0
      CAN2_RX1_IRQHandler,    // CAN2 RX1
      CAN2_SCE_IRQHandler,    // CAN2 SCE
      OTG_FS_IRQHandler,      // USB OTG FS
      DMA2_Stream5_IRQHandler,        // DMA2 Stream 5
      DMA2_Stream6_IRQHandler,        // DMA2 Stream 6
      DMA2_Stream7_IRQHandler,        // DMA2 Stream 7
      USART6_IRQHandler,      // USART6
      I2C3_EV_IRQHandler,     // I2C3 event
      I2C3_ER_IRQHandler,     // I2C3 error
      OTG_HS_EP1_OUT_IRQHandler,      // USB OTG HS End Point 1 Out
      OTG_HS_EP1_IN_IRQHandler,       // USB OTG HS End Point 1 In
      OTG_HS_WKUP_IRQHandler, // USB OTG HS Wakeup through EXTI
      OTG_HS_IRQHandler,      // USB OTG HS
      DCMI_IRQHandler,        // DCMI
      0,      // Reserved
      HASH_RNG_IRQHandler,    // Hash and Rng
      FPU_IRQHandler, // FPU
      UART7_IRQHandler,       // UART7
      UART8_IRQHandler,       // UART8
      SPI4_IRQHandler,        // SPI4
      SPI5_IRQHandler,        // SPI5
      SPI6_IRQHandler,        // SPI6
      SAI1_IRQHandler,        // SAI1
      LTDC_IRQHandler,        // LTDC_IRQHandler
      LTDC_ER_IRQHandler,     // LTDC_ER_IRQHandler
      DMA2D_IRQHandler       // DMA2D

#elif defined(STM32F437xx)

      CAN1_TX_IRQHandler,     // CAN1 TX
      CAN1_RX0_IRQHandler,    // CAN1 RX0
      CAN1_RX1_IRQHandler,    // CAN1 RX1
      CAN1_SCE_IRQHandler,    // CAN1 SCE
      EXTI9_5_IRQHandler,     // External Line[9:5]s
      TIM1_BRK_TIM9_IRQHandler,       // TIM1 Break and TIM9
      TIM1_UP_TIM10_IRQHandler,       // TIM1 Update and TIM10
      TIM1_TRG_COM_TIM11_IRQHandler,  // TIM1 Trigger and Commutation and TIM11
      TIM1_CC_IRQHandler,     // TIM1 Capture Compare
      TIM2_IRQHandler,        // TIM2
      TIM3_IRQHandler,        // TIM3
      TIM4_IRQHandler,        // TIM4
      I2C1_EV_IRQHandler,     // I2C1 Event
      I2C1_ER_IRQHandler,     // I2C1 Error
      I2C2_EV_IRQHandler,     // I2C2 Event
      I2C2_ER_IRQHandler,     // I2C2 Error
      SPI1_IRQHandler,        // SPI1
      SPI2_IRQHandler,        // SPI2
      USART1_IRQHandler,      // USART1
      USART2_IRQHandler,      // USART2
      USART3_IRQHandler,      // USART3
      EXTI15_10_IRQHandler,   // External Line[15:10]s
      RTC_Alarm_IRQHandler,   // RTC Alarm (A and B) through EXTI Line
      OTG_FS_WKUP_IRQHandler, // USB OTG FS Wakeup through EXTI line
      TIM8_BRK_TIM12_IRQHandler,      // TIM8 Break and TIM12
      TIM8_UP_TIM13_IRQHandler,       // TIM8 Update and TIM13
      TIM8_TRG_COM_TIM14_IRQHandler,  // TIM8 Trigger and Commutation and TIM14
      TIM8_CC_IRQHandler,     // TIM8 Capture Compare
      DMA1_Stream7_IRQHandler,        // DMA1 Stream7
      FMC_IRQHandler, // FMC
      SDIO_IRQHandler,        // SDIO
      TIM5_IRQHandler,        // TIM5
      SPI3_IRQHandler,        // SPI3
      UART4_IRQHandler,       // UART4
      UART5_IRQHandler,       // UART5
      TIM6_DAC_IRQHandler,    // TIM6 and DAC1&2 underrun errors
      TIM7_IRQHandler,        // TIM7
      DMA2_Stream0_IRQHandler,        // DMA2 Stream 0
      DMA2_Stream1_IRQHandler,        // DMA2 Stream 1
      DMA2_Stream2_IRQHandler,        // DMA2 Stream 2
      DMA2_Stream3_IRQHandler,        // DMA2 Stream 3
      DMA2_Stream4_IRQHandler,        // DMA2 Stream 4
      ETH_IRQHandler, // Ethernet
      ETH_WKUP_IRQHandler,    // Ethernet Wakeup through EXTI line
      CAN2_TX_IRQHandler,     // CAN2 TX
      CAN2_RX0_IRQHandler,    // CAN2 RX0
      CAN2_RX1_IRQHandler,    // CAN2 RX1
      CAN2_SCE_IRQHandler,    // CAN2 SCE
      OTG_FS_IRQHandler,      // USB OTG FS
      DMA2_Stream5_IRQHandler,        // DMA2 Stream 5
      DMA2_Stream6_IRQHandler,        // DMA2 Stream 6
      DMA2_Stream7_IRQHandler,        // DMA2 Stream 7
      USART6_IRQHandler,      // USART6
      I2C3_EV_IRQHandler,     // I2C3 event
      I2C3_ER_IRQHandler,     // I2C3 error
      OTG_HS_EP1_OUT_IRQHandler,      // USB OTG HS End Point 1 Out
      OTG_HS_EP1_IN_IRQHandler,       // USB OTG HS End Point 1 In
      OTG_HS_WKUP_IRQHandler, // USB OTG HS Wakeup through EXTI
      OTG_HS_IRQHandler,      // USB OTG HS
      DCMI_IRQHandler,        // DCMI
      CRYP_IRQHandler,        // CRYP crypto
      HASH_RNG_IRQHandler,    // Hash and Rng
      FPU_IRQHandler, // FPU
      UART7_IRQHandler,       // UART7
      UART8_IRQHandler,       // UART8
      SPI4_IRQHandler,        // SPI4
      SPI5_IRQHandler,        // SPI5
      SPI6_IRQHandler,        // SPI6
      SAI1_IRQHandler,        // SAI1
      0,      // Reserved
      0,      // Reserved
      DMA2D_IRQHandler       // DMA2D

#elif defined(STM32F439xx)

      CAN1_TX_IRQHandler,     // CAN1 TX
      CAN1_RX0_IRQHandler,    // CAN1 RX0
      CAN1_RX1_IRQHandler,    // CAN1 RX1
      CAN1_SCE_IRQHandler,    // CAN1 SCE
      EXTI9_5_IRQHandler,     // External Line[9:5]s
      TIM1_BRK_TIM9_IRQHandler,       // TIM1 Break and TIM9
      TIM1_UP_TIM10_IRQHandler,       // TIM1 Update and TIM10
      TIM1_TRG_COM_TIM11_IRQHandler,  // TIM1 Trigger and Commutation and TIM11
      TIM1_CC_IRQHandler,     // TIM1 Capture Compare
      TIM2_IRQHandler,        // TIM2
      TIM3_IRQHandler,        // TIM3
      TIM4_IRQHandler,        // TIM4
      I2C1_EV_IRQHandler,     // I2C1 Event
      I2C1_ER_IRQHandler,     // I2C1 Error
      I2C2_EV_IRQHandler,     // I2C2 Event
      I2C2_ER_IRQHandler,     // I2C2 Error
      SPI1_IRQHandler,        // SPI1
      SPI2_IRQHandler,        // SPI2
      USART1_IRQHandler,      // USART1
      USART2_IRQHandler,      // USART2
      USART3_IRQHandler,      // USART3
      EXTI15_10_IRQHandler,   // External Line[15:10]s
      RTC_Alarm_IRQHandler,   // RTC Alarm (A and B) through EXTI Line
      OTG_FS_WKUP_IRQHandler, // USB OTG FS Wakeup through EXTI line
      TIM8_BRK_TIM12_IRQHandler,      // TIM8 Break and TIM12
      TIM8_UP_TIM13_IRQHandler,       // TIM8 Update and TIM13
      TIM8_TRG_COM_TIM14_IRQHandler,  // TIM8 Trigger and Commutation and TIM14
      TIM8_CC_IRQHandler,     // TIM8 Capture Compare
      DMA1_Stream7_IRQHandler,        // DMA1 Stream7
      FMC_IRQHandler, // FMC
      SDIO_IRQHandler,        // SDIO
      TIM5_IRQHandler,        // TIM5
      SPI3_IRQHandler,        // SPI3
      UART4_IRQHandler,       // UART4
      UART5_IRQHandler,       // UART5
      TIM6_DAC_IRQHandler,    // TIM6 and DAC1&2 underrun errors
      TIM7_IRQHandler,        // TIM7
      DMA2_Stream0_IRQHandler,        // DMA2 Stream 0
      DMA2_Stream1_IRQHandler,        // DMA2 Stream 1
      DMA2_Stream2_IRQHandler,        // DMA2 Stream 2
      DMA2_Stream3_IRQHandler,        // DMA2 Stream 3
      DMA2_Stream4_IRQHandler,        // DMA2 Stream 4
      ETH_IRQHandler, // Ethernet
      ETH_WKUP_IRQHandler,    // Ethernet Wakeup through EXTI line
      CAN2_TX_IRQHandler,     // CAN2 TX
      CAN2_RX0_IRQHandler,    // CAN2 RX0
      CAN2_RX1_IRQHandler,    // CAN2 RX1
      CAN2_SCE_IRQHandler,    // CAN2 SCE
      OTG_FS_IRQHandler,      // USB OTG FS
      DMA2_Stream5_IRQHandler,        // DMA2 Stream 5
      DMA2_Stream6_IRQHandler,        // DMA2 Stream 6
      DMA2_Stream7_IRQHandler,        // DMA2 Stream 7
      USART6_IRQHandler,      // USART6
      I2C3_EV_IRQHandler,     // I2C3 event
      I2C3_ER_IRQHandler,     // I2C3 error
      OTG_HS_EP1_OUT_IRQHandler,      // USB OTG HS End Point 1 Out
      OTG_HS_EP1_IN_IRQHandler,       // USB OTG HS End Point 1 In
      OTG_HS_WKUP_IRQHandler, // USB OTG HS Wakeup through EXTI
      OTG_HS_IRQHandler,      // USB OTG HS
      DCMI_IRQHandler,        // DCMI
      CRYP_IRQHandler,        // CRYP crypto
      HASH_RNG_IRQHandler,    // Hash and Rng
      FPU_IRQHandler, // FPU
      UART7_IRQHandler,       // UART7
      UART8_IRQHandler,       // UART8
      SPI4_IRQHandler,        // SPI4
      SPI5_IRQHandler,        // SPI5
      SPI6_IRQHandler,        // SPI6
      SAI1_IRQHandler,        // SAI1
      LTDC_IRQHandler,        // LTDC
      LTDC_ER_IRQHandler,     // LTDC error
      DMA2D_IRQHandler       // DMA2D

#else
#error "missing vectors"
#endif

  };

// ----------------------------------------------------------------------------

// Processor ends up here if an unexpected interrupt occurs or a specific
// handler is not present in the application code.

void __attribute__ ((section(".after_vectors")))
Default_Handler(void)
{
  while (1)
    {
    }
}

// ----------------------------------------------------------------------------
