/**
  ******************************************************************************
  * @file    mfxstm32l152.h
  * @author  MCD Application Team
  * @version V1.1.0
  * @date    11-February-2015
  * @brief   This file contains all the functions prototypes for the
  *          mfxstm32l152.c IO expander driver.
  ******************************************************************************
  * @attention
  *
  * <h2><center>&copy; COPYRIGHT(c) 2015 STMicroelectronics</center></h2>
  *
  * Redistribution and use in source and binary forms, with or without modification,
  * are permitted provided that the following conditions are met:
  *   1. Redistributions of source code must retain the above copyright notice,
  *      this list of conditions and the following disclaimer.
  *   2. Redistributions in binary form must reproduce the above copyright notice,
  *      this list of conditions and the following disclaimer in the documentation
  *      and/or other materials provided with the distribution.
  *   3. Neither the name of STMicroelectronics nor the names of its contributors
  *      may be used to endorse or promote products derived from this software
  *      without specific prior written permission.
  *
  * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
  * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
  * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
  * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
  * FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
  * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
  * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
  * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
  * OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
  * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
  *
  ******************************************************************************
  */ 

/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __MFXSTM32L152_H
#define __MFXSTM32L152_H

#ifdef __cplusplus
 extern "C" {
#endif   
   
/* Includes ------------------------------------------------------------------*/
#include "../Common/ts.h"
#include "../Common/io.h"
#include "../Common/idd.h"

/** @addtogroup BSP
  * @{
  */ 

/** @addtogroup Component
  * @{
  */
    
/** @defgroup MFXSTM32L152
  * @{
  */    

/* Exported types ------------------------------------------------------------*/

/** @defgroup MFXSTM32L152_Exported_Types
  * @{
  */ 
typedef struct
{
  uint8_t SYS_CTRL;
  uint8_t ERROR_SRC;
  uint8_t ERROR_MSG;
  uint8_t IRQ_OUT;
  uint8_t IRQ_SRC_EN;
  uint8_t IRQ_PENDING;
  uint8_t IDD_CTRL;
  uint8_t IDD_PRE_DELAY;
  uint8_t IDD_SHUNT0_MSB;
  uint8_t IDD_SHUNT0_LSB;
  uint8_t IDD_SHUNT1_MSB;
  uint8_t IDD_SHUNT1_LSB;
  uint8_t IDD_SHUNT2_MSB;
  uint8_t IDD_SHUNT2_LSB;
  uint8_t IDD_SHUNT3_MSB;
  uint8_t IDD_SHUNT3_LSB;
  uint8_t IDD_SHUNT4_MSB;
  uint8_t IDD_SHUNT4_LSB;
  uint8_t IDD_GAIN_MSB;
  uint8_t IDD_GAIN_LSB;
  uint8_t IDD_VDD_MIN_MSB;
  uint8_t IDD_VDD_MIN_LSB;
  uint8_t IDD_VALUE_MSB;
  uint8_t IDD_VALUE_MID;
  uint8_t IDD_VALUE_LSB;
  uint8_t IDD_CAL_OFFSET_MSB;
  uint8_t IDD_CAL_OFFSET_LSB;
  uint8_t IDD_SHUNT_USED;
}IDD_dbgTypeDef;

/**
  * @}
  */

/* Exported constants --------------------------------------------------------*/
  
/** @defgroup MFXSTM32L152_Exported_Constants
  * @{
  */ 

 /**
  * @brief  MFX COMMON defines
  */
   
 /**
  * @brief  Register address: chip IDs (R)
  */
#define MFXSTM32L152_REG_ADR_ID                 ((uint8_t)0x00)
 /**
  * @brief  Register address: chip FW_VERSION  (R)
  */
#define MFXSTM32L152_REG_ADR_FW_VERSION_MSB     ((uint8_t)0x01)
#define MFXSTM32L152_REG_ADR_FW_VERSION_LSB     ((uint8_t)0x00)
 /**
  * @brief  Register address: System Control Register (R/W)
  */
#define MFXSTM32L152_REG_ADR_SYS_CTRL           ((uint8_t)0x40)
 /**
  * @brief  Register address: Vdd monitoring (R)
  */
#define MFXSTM32L152_REG_ADR_VDD_REF_MSB        ((uint8_t)0x06)
#define MFXSTM32L152_REG_ADR_VDD_REF_LSB        ((uint8_t)0x07)
 /**
  * @brief  Register address: Error source
  */
#define MFXSTM32L152_REG_ADR_ERROR_SRC          ((uint8_t)0x03)
 /**
  * @brief  Register address: Error Message
  */
#define MFXSTM32L152_REG_ADR_ERROR_MSG          ((uint8_t)0x04)

 /**
  * @brief  Reg Addr IRQs: to config the pin that informs Main MCU that MFX events appear
  */
#define MFXSTM32L152_REG_ADR_MFX_IRQ_OUT        ((uint8_t)0x41)
 /**
  * @brief  Reg Addr IRQs: to select the events which activate the MFXSTM32L152_IRQ_OUT signal
  */
#define MFXSTM32L152_REG_ADR_IRQ_SRC_EN         ((uint8_t)0x42)
 /**
  * @brief  Reg Addr IRQs: the Main MCU must read the IRQ_PENDING register to know the interrupt reason
  */
#define MFXSTM32L152_REG_ADR_IRQ_PENDING        ((uint8_t)0x08)
 /**
  * @brief  Reg Addr IRQs: the Main MCU must acknowledge it thanks to a writing access to the IRQ_ACK register
  */
#define MFXSTM32L152_REG_ADR_IRQ_ACK            ((uint8_t)0x44)
   
  /**
  * @brief  MFXSTM32L152_REG_ADR_ID choices
  */
#define MFXSTM32L152_ID_1                    ((uint8_t)0x7B)
#define MFXSTM32L152_ID_2                    ((uint8_t)0x79)
   
  /**
  * @brief  MFXSTM32L152_REG_ADR_SYS_CTRL choices
  */
#define MFXSTM32L152_SWRST                    ((uint8_t)0x80)
#define MFXSTM32L152_STANDBY                  ((uint8_t)0x40)
#define MFXSTM32L152_ALTERNATE_GPIO_EN        ((uint8_t)0x08) /* by the way if IDD and TS are enabled they take automatically the AF pins*/
#define MFXSTM32L152_IDD_EN                   ((uint8_t)0x04)
#define MFXSTM32L152_TS_EN                    ((uint8_t)0x02)
#define MFXSTM32L152_GPIO_EN                  ((uint8_t)0x01)

  /**
  * @brief  MFXSTM32L152_REG_ADR_ERROR_SRC choices
  */
#define MFXSTM32L152_IDD_ERROR_SRC             ((uint8_t)0x04)  /* Error raised by Idd */
#define MFXSTM32L152_TS_ERROR_SRC              ((uint8_t)0x02)  /* Error raised by Touch Screen */
#define MFXSTM32L152_GPIO_ERROR_SRC            ((uint8_t)0x01)  /* Error raised by Gpio */

 /**
  * @brief  MFXSTM32L152_REG_ADR_MFX_IRQ_OUT choices
  */
#define MFXSTM32L152_OUT_PIN_TYPE_OPENDRAIN   ((uint8_t)0x00)
#define MFXSTM32L152_OUT_PIN_TYPE_PUSHPULL    ((uint8_t)0x01)
#define MFXSTM32L152_OUT_PIN_POLARITY_LOW     ((uint8_t)0x00)
#define MFXSTM32L152_OUT_PIN_POLARITY_HIGH    ((uint8_t)0x02)

 /**
   * @brief  REG_ADR_IRQ_SRC_EN, REG_ADR_IRQ_PENDING & REG_ADR_IRQ_ACK choices
  */
#define MFXSTM32L152_IRQ_TS_OVF               ((uint8_t)0x80)  /* TouchScreen FIFO Overflow irq*/
#define MFXSTM32L152_IRQ_TS_FULL              ((uint8_t)0x40)  /* TouchScreen FIFO Full irq*/
#define MFXSTM32L152_IRQ_TS_TH                ((uint8_t)0x20)  /* TouchScreen FIFO threshold triggered irq*/
#define MFXSTM32L152_IRQ_TS_NE                ((uint8_t)0x10)  /* TouchScreen FIFO Not Empty irq*/
#define MFXSTM32L152_IRQ_TS_DET               ((uint8_t)0x08)  /* TouchScreen Detect irq*/
#define MFXSTM32L152_IRQ_ERROR                ((uint8_t)0x04)  /* Error message from MFXSTM32L152 firmware irq */
#define MFXSTM32L152_IRQ_IDD                  ((uint8_t)0x02)  /* IDD function irq */
#define MFXSTM32L152_IRQ_GPIO                 ((uint8_t)0x01)  /* General GPIO irq (only for SRC_EN and PENDING) */
#define MFXSTM32L152_IRQ_ALL                  ((uint8_t)0xFF)  /* All global interrupts          */
#define MFXSTM32L152_IRQ_TS                  (MFXSTM32L152_IRQ_TS_DET | MFXSTM32L152_IRQ_TS_NE |  MFXSTM32L152_IRQ_TS_TH | MFXSTM32L152_IRQ_TS_FULL | MFXSTM32L152_IRQ_TS_OVF ) 

   
 /**
  * @brief  GPIO: 24 programmable input/output called MFXSTM32L152_GPIO[23:0] are provided
  */

 /**
   * @brief  Reg addr: GPIO DIRECTION (R/W): GPIO pins direction: (0) input, (1) output.
  */
#define MFXSTM32L152_REG_ADR_GPIO_DIR1          ((uint8_t)0x60)  /* gpio [0:7] */
#define MFXSTM32L152_REG_ADR_GPIO_DIR2          ((uint8_t)0x61)  /* gpio [8:15] */
#define MFXSTM32L152_REG_ADR_GPIO_DIR3          ((uint8_t)0x62)  /* agpio [0:7] */
 /**
  * @brief  Reg addr: GPIO TYPE (R/W): If GPIO in output: (0) output push pull, (1) output open drain.
  *                          If GPIO in input: (0) input without pull resistor, (1) input with pull resistor.
  */
#define MFXSTM32L152_REG_ADR_GPIO_TYPE1         ((uint8_t)0x64)  /* gpio [0:7] */
#define MFXSTM32L152_REG_ADR_GPIO_TYPE2         ((uint8_t)0x65)  /* gpio [8:15] */
#define MFXSTM32L152_REG_ADR_GPIO_TYPE3         ((uint8_t)0x66)  /* agpio [0:7] */
 /**
  * @brief  Reg addr: GPIO PULL_UP_PULL_DOWN (R/W):  discussion open with Jean Claude
  */
#define MFXSTM32L152_REG_ADR_GPIO_PUPD1         ((uint8_t)0x68)  /* gpio [0:7] */
#define MFXSTM32L152_REG_ADR_GPIO_PUPD2         ((uint8_t)0x69)  /* gpio [8:15] */
#define MFXSTM32L152_REG_ADR_GPIO_PUPD3         ((uint8_t)0x6A)  /* agpio [0:7] */
 /**
  * @brief  Reg addr: GPIO SET (W): When GPIO is in output mode, write (1) puts the corresponding GPO in High level.
  */
#define MFXSTM32L152_REG_ADR_GPO_SET1           ((uint8_t)0x6C)  /* gpio [0:7] */
#define MFXSTM32L152_REG_ADR_GPO_SET2           ((uint8_t)0x6D)  /* gpio [8:15] */
#define MFXSTM32L152_REG_ADR_GPO_SET3           ((uint8_t)0x6E)  /* agpio [0:7] */
 /**
  * @brief  Reg addr: GPIO CLEAR (W): When GPIO is in output mode, write (1) puts the corresponding GPO in Low level.
  */
#define MFXSTM32L152_REG_ADR_GPO_CLR1           ((uint8_t)0x70)  /* gpio [0:7] */
#define MFXSTM32L152_REG_ADR_GPO_CLR2           ((uint8_t)0x71)  /* gpio [8:15] */
#define MFXSTM32L152_REG_ADR_GPO_CLR3           ((uint8_t)0x72)  /* agpio [0:7] */
 /**
  * @brief  Reg addr: GPIO STATE (R): Give state of the GPIO pin.
  */
#define MFXSTM32L152_REG_ADR_GPIO_STATE1         ((uint8_t)0x10)  /* gpio [0:7] */
#define MFXSTM32L152_REG_ADR_GPIO_STATE2         ((uint8_t)0x11)  /* gpio [8:15] */
#define MFXSTM32L152_REG_ADR_GPIO_STATE3         ((uint8_t)0x12)  /* agpio [0:7] */

  /**
  * @brief  GPIO IRQ_GPIs
  */
/* GPIOs can INDIVIDUALLY generate interruption to the Main MCU thanks to the MFXSTM32L152_IRQ_OUT signal */
/* the general MFXSTM32L152_IRQ_GPIO_SRC_EN shall be enabled too          */
  /**
  * @brief  GPIO IRQ_GPI_SRC1/2/3 (R/W): registers enable or not the feature to generate irq
  */
#define MFXSTM32L152_REG_ADR_IRQ_GPI_SRC1       ((uint8_t)0x48)  /* gpio [0:7] */
#define MFXSTM32L152_REG_ADR_IRQ_GPI_SRC2       ((uint8_t)0x49)  /* gpio [8:15] */
#define MFXSTM32L152_REG_ADR_IRQ_GPI_SRC3       ((uint8_t)0x4A)  /* agpio [0:7] */
  /**
  * @brief  GPIO IRQ_GPI_EVT1/2/3 (R/W): Irq generated on level (0) or edge (1).
  */
#define MFXSTM32L152_REG_ADR_IRQ_GPI_EVT1       ((uint8_t)0x4C)  /* gpio [0:7] */
#define MFXSTM32L152_REG_ADR_IRQ_GPI_EVT2       ((uint8_t)0x4D)  /* gpio [8:15] */
#define MFXSTM32L152_REG_ADR_IRQ_GPI_EVT3       ((uint8_t)0x4E)  /* agpio [0:7] */
  /**
  * @brief  GPIO IRQ_GPI_TYPE1/2/3 (R/W): Irq generated on (0) : Low level or Falling edge. (1) : High level or Rising edge.
  */
#define MFXSTM32L152_REG_ADR_IRQ_GPI_TYPE1      ((uint8_t)0x50)  /* gpio [0:7] */
#define MFXSTM32L152_REG_ADR_IRQ_GPI_TYPE2      ((uint8_t)0x51)  /* gpio [8:15] */
#define MFXSTM32L152_REG_ADR_IRQ_GPI_TYPE3      ((uint8_t)0x52)  /* agpio [0:7] */
  /**
  * @brief  GPIO IRQ_GPI_PENDING1/2/3 (R): irq occurs
  */
#define MFXSTM32L152_REG_ADR_IRQ_GPI_PENDING1   ((uint8_t)0x0C)  /* gpio [0:7] */
#define MFXSTM32L152_REG_ADR_IRQ_GPI_PENDING2   ((uint8_t)0x0D)  /* gpio [8:15] */
#define MFXSTM32L152_REG_ADR_IRQ_GPI_PENDING3   ((uint8_t)0x0E)  /* agpio [0:7] */
  /**
  * @brief  GPIO IRQ_GPI_ACK1/2/3 (W): Write (1) to acknowledge IRQ event
  */
#define MFXSTM32L152_REG_ADR_IRQ_GPI_ACK1       ((uint8_t)0x54)  /* gpio [0:7] */
#define MFXSTM32L152_REG_ADR_IRQ_GPI_ACK2       ((uint8_t)0x55)  /* gpio [8:15] */
#define MFXSTM32L152_REG_ADR_IRQ_GPI_ACK3       ((uint8_t)0x56)  /* agpio [0:7] */

   
 /**
  * @brief  GPIO: IO Pins definition
  */
#define MFXSTM32L152_GPIO_PIN_0                  ((uint32_t)0x0001)
#define MFXSTM32L152_GPIO_PIN_1                  ((uint32_t)0x0002)
#define MFXSTM32L152_GPIO_PIN_2                  ((uint32_t)0x0004)
#define MFXSTM32L152_GPIO_PIN_3                  ((uint32_t)0x0008)
#define MFXSTM32L152_GPIO_PIN_4                  ((uint32_t)0x0010)
#define MFXSTM32L152_GPIO_PIN_5                  ((uint32_t)0x0020)
#define MFXSTM32L152_GPIO_PIN_6                  ((uint32_t)0x0040)
#define MFXSTM32L152_GPIO_PIN_7                  ((uint32_t)0x0080)

#define MFXSTM32L152_GPIO_PIN_8                  ((uint32_t)0x0100) 
#define MFXSTM32L152_GPIO_PIN_9                  ((uint32_t)0x0200) 
#define MFXSTM32L152_GPIO_PIN_10                 ((uint32_t)0x0400) 
#define MFXSTM32L152_GPIO_PIN_11                 ((uint32_t)0x0800)
#define MFXSTM32L152_GPIO_PIN_12                 ((uint32_t)0x1000) 
#define MFXSTM32L152_GPIO_PIN_13                 ((uint32_t)0x2000) 
#define MFXSTM32L152_GPIO_PIN_14                 ((uint32_t)0x4000) 
#define MFXSTM32L152_GPIO_PIN_15                 ((uint32_t)0x8000) 

#define MFXSTM32L152_GPIO_PIN_16               ((uint32_t)0x010000)
#define MFXSTM32L152_GPIO_PIN_17               ((uint32_t)0x020000)
#define MFXSTM32L152_GPIO_PIN_18               ((uint32_t)0x040000)
#define MFXSTM32L152_GPIO_PIN_19               ((uint32_t)0x080000)
#define MFXSTM32L152_GPIO_PIN_20               ((uint32_t)0x100000)
#define MFXSTM32L152_GPIO_PIN_21               ((uint32_t)0x200000)
#define MFXSTM32L152_GPIO_PIN_22               ((uint32_t)0x400000)
#define MFXSTM32L152_GPIO_PIN_23               ((uint32_t)0x800000)

#define MFXSTM32L152_AGPIO_PIN_0               MFXSTM32L152_GPIO_PIN_16
#define MFXSTM32L152_AGPIO_PIN_1               MFXSTM32L152_GPIO_PIN_17
#define MFXSTM32L152_AGPIO_PIN_2               MFXSTM32L152_GPIO_PIN_18
#define MFXSTM32L152_AGPIO_PIN_3               MFXSTM32L152_GPIO_PIN_19
#define MFXSTM32L152_AGPIO_PIN_4               MFXSTM32L152_GPIO_PIN_20
#define MFXSTM32L152_AGPIO_PIN_5               MFXSTM32L152_GPIO_PIN_21
#define MFXSTM32L152_AGPIO_PIN_6               MFXSTM32L152_GPIO_PIN_22
#define MFXSTM32L152_AGPIO_PIN_7               MFXSTM32L152_GPIO_PIN_23

#define MFXSTM32L152_GPIO_PINS_ALL             ((uint32_t)0xFFFFFF)

 /**
  * @brief  GPIO: constant
  */
#define MFXSTM32L152_GPIO_DIR_IN                ((uint8_t)0x0)  
#define MFXSTM32L152_GPIO_DIR_OUT               ((uint8_t)0x1)  
#define MFXSTM32L152_IRQ_GPI_EVT_LEVEL          ((uint8_t)0x0)  
#define MFXSTM32L152_IRQ_GPI_EVT_EDGE           ((uint8_t)0x1)  
#define MFXSTM32L152_IRQ_GPI_TYPE_LLFE          ((uint8_t)0x0)  /* Low Level Falling Edge */
#define MFXSTM32L152_IRQ_GPI_TYPE_HLRE          ((uint8_t)0x1)  /*High Level Raising Edge */
#define MFXSTM32L152_GPI_WITHOUT_PULL_RESISTOR  ((uint8_t)0x0)  
#define MFXSTM32L152_GPI_WITH_PULL_RESISTOR     ((uint8_t)0x1)  
#define MFXSTM32L152_GPO_PUSH_PULL              ((uint8_t)0x0)  
#define MFXSTM32L152_GPO_OPEN_DRAIN             ((uint8_t)0x1)  
#define MFXSTM32L152_GPIO_PULL_DOWN             ((uint8_t)0x0)  
#define MFXSTM32L152_GPIO_PULL_UP               ((uint8_t)0x1)   
   
   
  /**
  * @brief  TOUCH SCREEN Registers
  */

  /**
  * @brief  Touch Screen Registers
  */
#define MFXSTM32L152_TS_SETTLING            ((uint8_t)0xA0)
#define MFXSTM32L152_TS_TOUCH_DET_DELAY     ((uint8_t)0xA1)
#define MFXSTM32L152_TS_AVE                 ((uint8_t)0xA2)
#define MFXSTM32L152_TS_TRACK               ((uint8_t)0xA3)
#define MFXSTM32L152_TS_FIFO_TH             ((uint8_t)0xA4)
#define MFXSTM32L152_TS_FIFO_STA            ((uint8_t)0x20)
#define MFXSTM32L152_TS_FIFO_LEVEL          ((uint8_t)0x21)
#define MFXSTM32L152_TS_XY_DATA             ((uint8_t)0x24)

  /**
  * @brief TS registers masks
  */
#define MFXSTM32L152_TS_CTRL_STATUS         ((uint8_t)0x08)
#define MFXSTM32L152_TS_CLEAR_FIFO          ((uint8_t)0x80)


/**
  * @brief  Register address: Idd control register (R/W)
  */
#define MFXSTM32L152_REG_ADR_IDD_CTRL           ((uint8_t)0x80)

/**
  * @brief  Register address: Idd pre delay  register (R/W)
  */
#define MFXSTM32L152_REG_ADR_IDD_PRE_DELAY      ((uint8_t)0x81)

/**
  * @brief  Register address: Idd Shunt registers (R/W)
  */
#define MFXSTM32L152_REG_ADR_IDD_SHUNT0_MSB     ((uint8_t)0x82)
#define MFXSTM32L152_REG_ADR_IDD_SHUNT0_LSB     ((uint8_t)0x83)
#define MFXSTM32L152_REG_ADR_IDD_SHUNT1_MSB     ((uint8_t)0x84)
#define MFXSTM32L152_REG_ADR_IDD_SHUNT1_LSB     ((uint8_t)0x85)
#define MFXSTM32L152_REG_ADR_IDD_SHUNT2_MSB     ((uint8_t)0x86)
#define MFXSTM32L152_REG_ADR_IDD_SHUNT2_LSB     ((uint8_t)0x87)
#define MFXSTM32L152_REG_ADR_IDD_SHUNT3_MSB     ((uint8_t)0x88)
#define MFXSTM32L152_REG_ADR_IDD_SHUNT3_LSB     ((uint8_t)0x89)
#define MFXSTM32L152_REG_ADR_IDD_SHUNT4_MSB     ((uint8_t)0x8A)
#define MFXSTM32L152_REG_ADR_IDD_SHUNT4_LSB     ((uint8_t)0x8B)

/**
  * @brief  Register address: Idd ampli gain register (R/W)
  */
#define MFXSTM32L152_REG_ADR_IDD_GAIN_MSB       ((uint8_t)0x8C)
#define MFXSTM32L152_REG_ADR_IDD_GAIN_LSB       ((uint8_t)0x8D)

/**
  * @brief  Register address: Idd VDD min register (R/W)
  */
#define MFXSTM32L152_REG_ADR_IDD_VDD_MIN_MSB    ((uint8_t)0x8E)
#define MFXSTM32L152_REG_ADR_IDD_VDD_MIN_LSB    ((uint8_t)0x8F)

/**
  * @brief  Register address: Idd value register (R)
  */
#define MFXSTM32L152_REG_ADR_IDD_VALUE_MSB      ((uint8_t)0x14)
#define MFXSTM32L152_REG_ADR_IDD_VALUE_MID      ((uint8_t)0x15)
#define MFXSTM32L152_REG_ADR_IDD_VALUE_LSB      ((uint8_t)0x16)

/**
  * @brief  Register address: Idd calibration offset register (R)
  */
#define MFXSTM32L152_REG_ADR_IDD_CAL_OFFSET_MSB ((uint8_t)0x18)
#define MFXSTM32L152_REG_ADR_IDD_CAL_OFFSET_LSB ((uint8_t)0x19)

/**
  * @brief  Register address: Idd shunt used offset register (R)
  */
#define MFXSTM32L152_REG_ADR_IDD_SHUNT_USED     ((uint8_t)0x1A)

/** @defgroup IDD_Control_Register_Defines  IDD Control Register Defines
  * @{
  */
/**
  * @brief  IDD control register masks
  */
#define MFXSTM32L152_IDD_CTRL_REQ                       ((uint8_t)0x01)
#define MFXSTM32L152_IDD_CTRL_SHUNT_NB                  ((uint8_t)0x0E)
#define MFXSTM32L152_IDD_CTRL_VREF_DIS                  ((uint8_t)0x40)
#define MFXSTM32L152_IDD_CTRL_CAL_DIS                   ((uint8_t)0x80)

/**
  * @brief  IDD Shunt Number
  */
#define MFXSTM32L152_IDD_SHUNT_NB_1                     ((uint8_t) 0x01)
#define MFXSTM32L152_IDD_SHUNT_NB_2                     ((uint8_t) 0x02)
#define MFXSTM32L152_IDD_SHUNT_NB_3                     ((uint8_t) 0x03)
#define MFXSTM32L152_IDD_SHUNT_NB_4                     ((uint8_t) 0x04)
#define MFXSTM32L152_IDD_SHUNT_NB_5                     ((uint8_t) 0x05)

/**
  * @brief  Vref Measurement
  */
#define MFXSTM32L152_IDD_VREF_AUTO_MEASUREMENT_ENABLE   ((uint8_t) 0x00)
#define MFXSTM32L152_IDD_VREF_AUTO_MEASUREMENT_DISABLE  ((uint8_t) 0x70)

/**
  * @brief  IDD Calibration
  */
#define MFXSTM32L152_IDD_AUTO_CALIBRATION_ENABLE        ((uint8_t) 0x00)
#define MFXSTM32L152_IDD_AUTO_CALIBRATION_DISABLE       ((uint8_t) 0x80)
/**
  * @}
  */

/** @defgroup IDD_PreDelay_Defines  IDD PreDelay Defines
  * @{
  */
/**
  * @brief  IDD PreDelay masks
  */
#define MFXSTM32L152_IDD_PREDELAY_UNIT                  ((uint8_t) 0x80)
#define MFXSTM32L152_IDD_PREDELAY_VALUE                 ((uint8_t) 0x7F)


/**
  * @brief  IDD PreDelay unit
  */
#define MFXSTM32L152_IDD_PREDELAY_0_5_MS                ((uint8_t) 0x00)
#define MFXSTM32L152_IDD_PREDELAY_10_MS                 ((uint8_t) 0x80)


/**
  * @}
  */

/**
  * @}
  */

 
/* Exported macro ------------------------------------------------------------*/
   
/** @defgroup MFXSTM32L152_Exported_Macros
  * @{
  */ 

/**
  * @}
  */ 

/* Exported functions --------------------------------------------------------*/
  
/** @defgroup MFXSTM32L152_Exported_Functions
  * @{
  */

/** 
  * @brief MFXSTM32L152 Control functions
  */
void     mfxstm32l152_Init(uint16_t DeviceAddr);
void     mfxstm32l152_DeInit(uint16_t DeviceAddr);
void     mfxstm32l152_Reset(uint16_t DeviceAddr);
uint16_t mfxstm32l152_ReadID(uint16_t DeviceAddr);
uint16_t mfxstm32l152_ReadFwVersion(uint16_t DeviceAddr);
void     mfxstm32l152_LowPower(uint16_t DeviceAddr);
void     mfxstm32l152_WakeUp(uint16_t DeviceAddr);

void     mfxstm32l152_EnableITSource(uint16_t DeviceAddr, uint8_t Source);
void     mfxstm32l152_DisableITSource(uint16_t DeviceAddr, uint8_t Source);
uint8_t  mfxstm32l152_GlobalITStatus(uint16_t DeviceAddr, uint8_t Source);
void     mfxstm32l152_ClearGlobalIT(uint16_t DeviceAddr, uint8_t Source);

void     mfxstm32l152_SetIrqOutPinPolarity(uint16_t DeviceAddr, uint8_t Polarity);
void     mfxstm32l152_SetIrqOutPinType(uint16_t DeviceAddr, uint8_t Type);


/** 
  * @brief MFXSTM32L152 IO functionalities functions
  */
void     mfxstm32l152_IO_Start(uint16_t DeviceAddr, uint32_t IO_Pin);
uint8_t  mfxstm32l152_IO_Config(uint16_t DeviceAddr, uint32_t IO_Pin, IO_ModeTypedef IO_Mode);
void     mfxstm32l152_IO_WritePin(uint16_t DeviceAddr, uint32_t IO_Pin, uint8_t PinState);
uint32_t mfxstm32l152_IO_ReadPin(uint16_t DeviceAddr, uint32_t IO_Pin);
void     mfxstm32l152_IO_EnableIT(uint16_t DeviceAddr);
void     mfxstm32l152_IO_DisableIT(uint16_t DeviceAddr);
uint32_t mfxstm32l152_IO_ITStatus(uint16_t DeviceAddr, uint32_t IO_Pin);
void     mfxstm32l152_IO_ClearIT(uint16_t DeviceAddr, uint32_t IO_Pin);

void     mfxstm32l152_IO_InitPin(uint16_t DeviceAddr, uint32_t IO_Pin, uint8_t Direction);
void     mfxstm32l152_IO_EnableAF(uint16_t DeviceAddr);
void     mfxstm32l152_IO_DisableAF(uint16_t DeviceAddr);
void     mfxstm32l152_IO_SetIrqTypeMode(uint16_t DeviceAddr, uint32_t IO_Pin, uint8_t Type);
void     mfxstm32l152_IO_SetIrqEvtMode(uint16_t DeviceAddr, uint32_t IO_Pin, uint8_t Evt);
void     mfxstm32l152_IO_EnablePinIT(uint16_t DeviceAddr, uint32_t IO_Pin);
void     mfxstm32l152_IO_DisablePinIT(uint16_t DeviceAddr, uint32_t IO_Pin);

/** 
  * @brief MFXSTM32L152 Touch screen functionalities functions
  */
void     mfxstm32l152_TS_Start(uint16_t DeviceAddr);
uint8_t  mfxstm32l152_TS_DetectTouch(uint16_t DeviceAddr);
void     mfxstm32l152_TS_GetXY(uint16_t DeviceAddr, uint16_t *X, uint16_t *Y);
void     mfxstm32l152_TS_EnableIT(uint16_t DeviceAddr);
void     mfxstm32l152_TS_DisableIT(uint16_t DeviceAddr);
uint8_t  mfxstm32l152_TS_ITStatus (uint16_t DeviceAddr);
void     mfxstm32l152_TS_ClearIT (uint16_t DeviceAddr);

/**
  * @brief MFXSTM32L152 IDD current measurement functionalities functions
  */
void     mfxstm32l152_IDD_Start(uint16_t DeviceAddr);
void     mfxstm32l152_IDD_Config(uint16_t DeviceAddr, IDD_ConfigTypeDef MfxIddConfig);
void     mfxstm32l152_IDD_GetValue(uint16_t DeviceAddr, uint32_t *ReadValue);
void     mfxstm32l152_IDD_EnableIT(uint16_t DeviceAddr);
void     mfxstm32l152_IDD_ClearIT(uint16_t DeviceAddr);
uint8_t  mfxstm32l152_IDD_GetITStatus(uint16_t DeviceAddr);
void     mfxstm32l152_IDD_DisableIT(uint16_t DeviceAddr);

/**
  * @brief MFXSTM32L152 Error management functions
  */
uint8_t  mfxstm32l152_Error_ReadSrc(uint16_t DeviceAddr);
uint8_t  mfxstm32l152_Error_ReadMsg(uint16_t DeviceAddr);
void     mfxstm32l152_Error_EnableIT(uint16_t DeviceAddr);
void     mfxstm32l152_Error_ClearIT(uint16_t DeviceAddr);
uint8_t  mfxstm32l152_Error_GetITStatus(uint16_t DeviceAddr);
void     mfxstm32l152_Error_DisableIT(uint16_t DeviceAddr);

uint8_t  mfxstm32l152_ReadReg(uint16_t DeviceAddr, uint8_t RegAddr);


/** 
  * @brief iobus prototypes (they should be defined in common/stm32_iobus.h)
  */
void     MFX_IO_Init(void);
void     MFX_IO_DeInit(void);
void     MFX_IO_ITConfig (void);
void     MFX_IO_EnableWakeupPin(void);
void     MFX_IO_Wakeup(void);
void     MFX_IO_Delay(uint32_t delay);
void     MFX_IO_Write(uint16_t addr, uint8_t reg, uint8_t value);
uint8_t  MFX_IO_Read(uint16_t addr, uint8_t reg);
uint16_t MFX_IO_ReadMultiple(uint16_t addr, uint8_t reg, uint8_t *buffer, uint16_t length);

/**
  * @}
  */ 

/* Touch screen driver structure */
extern TS_DrvTypeDef mfxstm32l152_ts_drv;

/* IO driver structure */
extern IO_DrvTypeDef mfxstm32l152_io_drv;

/* IDD driver structure */
extern IDD_DrvTypeDef mfxstm32l152_idd_drv;


#ifdef __cplusplus
}
#endif
#endif /* __MFXSTM32L152_H */


/**
  * @}
  */ 

/**
  * @}
  */

/**
  * @}
  */ 

/**
  * @}
  */       
/************************ (C) COPYRIGHT STMicroelectronics *****END OF FILE****/
