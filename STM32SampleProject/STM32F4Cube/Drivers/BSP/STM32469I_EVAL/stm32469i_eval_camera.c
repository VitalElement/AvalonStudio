/**
  ******************************************************************************
  * @file    stm32469i_eval_camera.c
  * @author  MCD Application Team
  * @version V1.0.2
  * @date    12-January-2016
  * @brief   This file includes the driver for Camera modules mounted on
  *          STM32469I-EVAL evaluation board.
  ******************************************************************************
  * @attention
  *
  * <h2><center>&copy; COPYRIGHT(c) 2016 STMicroelectronics</center></h2>
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

/* File Info: ------------------------------------------------------------------
                                   User NOTES
1. How to use this driver:
--------------------------
   - This driver is used to drive the camera.
   - The S5K5CAG component driver MUST be included with this driver.

2. Driver description:
---------------------
  + Initialization steps:
     o Initialize the camera using the BSP_CAMERA_Init() function.
     o Start the camera capture/snapshot using the CAMERA_Start() function.
     o Suspend, resume or stop the camera capture using the following functions:
      - BSP_CAMERA_Suspend()
      - BSP_CAMERA_Resume()
      - BSP_CAMERA_Stop()

  + Options
     o Increase or decrease on the fly the brightness and/or contrast
       using the following function:
       - BSP_CAMERA_ContrastBrightnessConfig
     o Add a special effect on the fly using the following functions:
       - BSP_CAMERA_BlackWhiteConfig()
       - BSP_CAMERA_ColorEffectConfig()

------------------------------------------------------------------------------*/

/* Includes ------------------------------------------------------------------*/
#include "stm32469i_eval_camera.h"

/** @addtogroup BSP
  * @{
  */

/** @addtogroup STM32469I_EVAL
  * @{
  */

/** @defgroup STM32469I-EVAL_CAMERA STM32469I EVAL CAMERA
  * @{
  */

/** @defgroup STM32469I-EVAL_CAMERA_Private_TypesDefinitions STM32469I EVAL CAMERA Private TypesDefinitions
  * @{
  */
/**
  * @}
  */

/** @defgroup STM32469I-EVAL_CAMERA_Private_Defines STM32469I EVAL CAMERA Private Defines
  * @{
  */
/**
  * @}
  */

/** @defgroup STM32469I-EVAL_CAMERA_Private_Macros STM32469I EVAL CAMERA Private Macros
  * @{
  */
/**
  * @}
  */

/** @defgroup STM32469I-EVAL_CAMERA_Imported_Variables STM32469I EVAL CAMERA Imported Variables
  * @{
  */
/**
  * @brief  DMA2D handle variable
  */
extern DMA2D_HandleTypeDef hdma2d_eval;
/**
  * @}
  */

/** @defgroup STM32469I-EVAL_CAMERA_Private_Variables STM32469I EVAL CAMERA Private Variables
  * @{
  */
static DCMI_HandleTypeDef  hDcmiEval;
CAMERA_DrvTypeDef   *CameraDrv;

/* Camera current resolution naming (QQVGA, VGA, ...) */
uint32_t CameraCurrentResolution;

/* Camera image rotation on LCD Displayed frame buffer */
uint32_t CameraRotation = CAMERA_ROTATION_INVALID;

static uint32_t  CameraHwAddress;

/**
  * @}
  */

/** @defgroup STM32469I-EVAL_CAMERA_Private_FunctionPrototypes STM32469I EVAL CAMERA Private FunctionPrototypes
  * @{
  */
static uint32_t GetSize(uint32_t Resolution);
/**
  * @}
  */

/** @defgroup STM32469I-EVAL_CAMERA_Public_Functions STM32469I EVAL CAMERA Public Functions
  * @{
  */

/**
  * @brief  Set Camera image rotation on LCD Displayed frame buffer.
  * @param  rotation : uint32_t rotation of camera image in preview buffer sent to LCD
  *         need to be of type Camera_ImageRotationTypeDef
  * @retval Camera status
  */
uint8_t BSP_CAMERA_SetRotation(uint32_t rotation)
{
  uint8_t status = CAMERA_ERROR;

  if(rotation < CAMERA_ROTATION_INVALID)
  {
    /* Set Camera image rotation on LCD Displayed frame buffer */
    CameraRotation = rotation;
    status = CAMERA_OK;
  }

  return status;
}

/**
  * @brief  Get Camera image rotation on LCD Displayed frame buffer.
  * @retval rotation : uint32_t value of type Camera_ImageRotationTypeDef
  */
uint32_t BSP_CAMERA_GetRotation(void)
{
  return(CameraRotation);
}

/**
  * @brief  Initializes the camera.
  * @param  Resolution : camera sensor requested resolution (x, y) : standard resolution
  *         naming QQVGA, QVGA, VGA ...
  * @retval Camera status
  */
uint8_t BSP_CAMERA_Init(uint32_t Resolution)
{
  DCMI_HandleTypeDef *phdcmi;
  uint8_t status = CAMERA_ERROR;

  /* Get the DCMI handle structure */
  phdcmi = &hDcmiEval;

  /*** Configures the DCMI to interface with the camera module ***/
  /* DCMI configuration */
  phdcmi->Init.CaptureRate      = DCMI_CR_ALL_FRAME;
  phdcmi->Init.HSPolarity       = DCMI_HSPOLARITY_HIGH;
  phdcmi->Init.SynchroMode      = DCMI_SYNCHRO_HARDWARE;
  phdcmi->Init.VSPolarity       = DCMI_VSPOLARITY_HIGH;
  phdcmi->Init.ExtendedDataMode = DCMI_EXTEND_DATA_8B;
  phdcmi->Init.PCKPolarity      = DCMI_PCKPOLARITY_RISING;

  phdcmi->Instance              = DCMI;

  /* Configure IO functionalities for CAMERA detect pin */
  BSP_IO_Init();
  /* Apply Camera Module hardware reset */
  BSP_CAMERA_HwReset();

  /* Check if the CAMERA Module is plugged on board */
  if(BSP_IO_ReadPin(CAM_PLUG_PIN) == BSP_IO_PIN_SET)
  {
    status = CAMERA_NOT_DETECTED;
    return status; /* Exit with error */
  }

  /* Read ID of Camera module via I2C */
  if (s5k5cag_ReadID(CAMERA_I2C_ADDRESS) == S5K5CAG_ID)
  {
    /* Initialize the camera driver structure */
    CameraDrv = &s5k5cag_drv;
    CameraHwAddress = CAMERA_I2C_ADDRESS;

    /* DCMI Initialization */
    BSP_CAMERA_MspInit(&hDcmiEval, NULL);
    HAL_DCMI_Init(phdcmi);

    /* Camera Module Initialization via I2C to the wanted 'Resolution' */
    CameraDrv->Init(CameraHwAddress, Resolution);

    CameraCurrentResolution = Resolution;

    /* Return CAMERA_OK status */
    status = CAMERA_OK;
  }
  else
  {
    /* Return CAMERA_NOT_SUPPORTED status */
    status = CAMERA_NOT_SUPPORTED;
  }

  return status;
}


/**
  * @brief  DeInitializes the camera.
  * @retval Camera status
  */
uint8_t BSP_CAMERA_DeInit(void)
{
  hDcmiEval.Instance              = DCMI;

  HAL_DCMI_DeInit(&hDcmiEval);
  BSP_CAMERA_MspDeInit(&hDcmiEval, NULL);
  return CAMERA_OK;
}

/**
  * @brief  Starts the camera capture in continuous mode.
  * @param  buff: pointer to the camera output buffer
  */
void BSP_CAMERA_ContinuousStart(uint8_t *buff)
{
  /* Start the camera capture */
  HAL_DCMI_Start_DMA(&hDcmiEval, DCMI_MODE_CONTINUOUS, (uint32_t)buff, GetSize(CameraCurrentResolution));
}

/**
  * @brief  Starts the camera capture in snapshot mode.
  * @param  buff: pointer to the camera output buffer
  */
void BSP_CAMERA_SnapshotStart(uint8_t *buff)
{
  /* Start the camera capture */
  HAL_DCMI_Start_DMA(&hDcmiEval, DCMI_MODE_SNAPSHOT, (uint32_t)buff, GetSize(CameraCurrentResolution));
}

/**
  * @brief Suspend the CAMERA capture
  */
void BSP_CAMERA_Suspend(void)
{
  /* Disable the DMA */
  __HAL_DMA_DISABLE(hDcmiEval.DMA_Handle);
  /* Disable the DCMI */
  __HAL_DCMI_DISABLE(&hDcmiEval);

}

/**
  * @brief Resume the CAMERA capture
  */
void BSP_CAMERA_Resume(void)
{
  /* Enable the DCMI */
  __HAL_DCMI_ENABLE(&hDcmiEval);
  /* Enable the DMA */
  __HAL_DMA_ENABLE(hDcmiEval.DMA_Handle);
}

/**
  * @brief  Stop the CAMERA capture
  * @retval Camera status
  */
uint8_t BSP_CAMERA_Stop(void)
{
  uint8_t status = CAMERA_ERROR;

  if(HAL_DCMI_Stop(&hDcmiEval) == HAL_OK)
  {
     status = CAMERA_OK;
  }

  /* Set Camera in Power Down */
  BSP_CAMERA_PwrDown();

  return status;
}

/**
  * @brief  CANERA hardware reset
  */
void BSP_CAMERA_HwReset(void)
{
  /* Camera sensor RESET sequence */
  BSP_IO_ConfigPin(RSTI_PIN, IO_MODE_OUTPUT);
  BSP_IO_ConfigPin(XSDN_PIN, IO_MODE_OUTPUT);

  /* Assert the camera STANDBY pin (active high)  */
  BSP_IO_WritePin(XSDN_PIN, BSP_IO_PIN_SET);

  /* Assert the camera RSTI pin (active low) */
  BSP_IO_WritePin(RSTI_PIN, BSP_IO_PIN_RESET);

  HAL_Delay(100);   /* RST and XSDN signals asserted during 100ms */

  /* De-assert the camera STANDBY pin (active high) */
  BSP_IO_WritePin(XSDN_PIN, BSP_IO_PIN_RESET);

  HAL_Delay(3);     /* RST de-asserted and XSDN asserted during 3ms */

  /* De-assert the camera RSTI pin (active low) */
  BSP_IO_WritePin(RSTI_PIN, BSP_IO_PIN_SET);

  HAL_Delay(6);     /* RST de-asserted during 3ms */
}

/**
  * @brief  CAMERA power down
  */
void BSP_CAMERA_PwrDown(void)
{
  /* Camera power down sequence */
  BSP_IO_ConfigPin(RSTI_PIN, IO_MODE_OUTPUT);
  BSP_IO_ConfigPin(XSDN_PIN, IO_MODE_OUTPUT);

  /* De-assert the camera STANDBY pin (active high) */
  BSP_IO_WritePin(XSDN_PIN, BSP_IO_PIN_RESET);

  /* Assert the camera RSTI pin (active low) */
  BSP_IO_WritePin(RSTI_PIN, BSP_IO_PIN_RESET);
}

/**
  * @brief  Configures the camera contrast and brightness.
  * @param  contrast_level: Contrast level
  *          This parameter can be one of the following values:
  *            @arg  CAMERA_CONTRAST_LEVEL4: for contrast +2
  *            @arg  CAMERA_CONTRAST_LEVEL3: for contrast +1
  *            @arg  CAMERA_CONTRAST_LEVEL2: for contrast  0
  *            @arg  CAMERA_CONTRAST_LEVEL1: for contrast -1
  *            @arg  CAMERA_CONTRAST_LEVEL0: for contrast -2
  * @param  brightness_level: Contrast level
  *          This parameter can be one of the following values:
  *            @arg  CAMERA_BRIGHTNESS_LEVEL4: for brightness +2
  *            @arg  CAMERA_BRIGHTNESS_LEVEL3: for brightness +1
  *            @arg  CAMERA_BRIGHTNESS_LEVEL2: for brightness  0
  *            @arg  CAMERA_BRIGHTNESS_LEVEL1: for brightness -1
  *            @arg  CAMERA_BRIGHTNESS_LEVEL0: for brightness -2
  */
void BSP_CAMERA_ContrastBrightnessConfig(uint32_t contrast_level, uint32_t brightness_level)
{
  if(CameraDrv->Config != NULL)
  {
    CameraDrv->Config(CameraHwAddress, CAMERA_CONTRAST_BRIGHTNESS, contrast_level, brightness_level);
  }
}

/**
  * @brief  Configures the camera white balance.
  * @param  Mode: black_white mode
  *          This parameter can be one of the following values:
  *            @arg  CAMERA_BLACK_WHITE_BW
  *            @arg  CAMERA_BLACK_WHITE_NEGATIVE
  *            @arg  CAMERA_BLACK_WHITE_BW_NEGATIVE
  *            @arg  CAMERA_BLACK_WHITE_NORMAL
  */
void BSP_CAMERA_BlackWhiteConfig(uint32_t Mode)
{
  if(CameraDrv->Config != NULL)
  {
    CameraDrv->Config(CameraHwAddress, CAMERA_BLACK_WHITE, Mode, 0);
  }
}

/**
  * @brief  Configures the camera color effect.
  * @param  Effect: Color effect
  *          This parameter can be one of the following values:
  *            @arg  CAMERA_COLOR_EFFECT_NONE
  *            @arg  CAMERA_COLOR_EFFECT_BLUE
  *            @arg  CAMERA_COLOR_EFFECT_GREEN
  *            @arg  CAMERA_COLOR_EFFECT_RED
  *            @arg  CAMERA_COLOR_EFFECT_ANTIQUE
  */
void BSP_CAMERA_ColorEffectConfig(uint32_t Effect)
{
  if(CameraDrv->Config != NULL)
  {
    CameraDrv->Config(CameraHwAddress, CAMERA_COLOR_EFFECT, Effect, 0);
  }
}

/**
  * @brief  Handles DCMI interrupt request.
  */
void BSP_CAMERA_IRQHandler(void)
{
  HAL_DCMI_IRQHandler(&hDcmiEval);
}

/**
  * @brief  Handles DMA interrupt request.
  */
void BSP_CAMERA_DMA_IRQHandler(void)
{
  HAL_DMA_IRQHandler(hDcmiEval.DMA_Handle);
}

/**
  * @brief  Get the capture size in pixels unit.
  * @param  Resolution: the current resolution.
  * @retval capture size in pixels unit.
  */
static uint32_t GetSize(uint32_t Resolution)
{
  uint32_t size = 0;

  /* Get capture size */
  switch (Resolution)
  {
  case CAMERA_R160x120:
    {
      size =  0x2580;
    }
    break;
  case CAMERA_R320x240:
    {
      size =  0x9600;
    }
    break;
  case CAMERA_R480x272:
    {
      size =  0xFF00;
    }
    break;
  case CAMERA_R640x480:
    {
      size =  0x25800;
    }
    break;
  default:
    {
      break;
    }
  }

  return size;
}

/**
  * @brief  Initializes the DCMI MSP.
  * @param  hdcmi: HDMI handle
  * @param  Params : pointer on additional configuration parameters, can be NULL.
  */
__weak void BSP_CAMERA_MspInit(DCMI_HandleTypeDef *hdcmi, void *Params)
{
  static DMA_HandleTypeDef hdma_eval;
  GPIO_InitTypeDef gpio_init_structure;

  /*** Enable peripherals and GPIO clocks ***/
  /* Enable DCMI clock */
  __HAL_RCC_DCMI_CLK_ENABLE();

  /* Enable DMA2 clock */
  __HAL_RCC_DMA2_CLK_ENABLE();

  /* Enable GPIO clocks */
  __HAL_RCC_GPIOA_CLK_ENABLE();
  __HAL_RCC_GPIOB_CLK_ENABLE();
  __HAL_RCC_GPIOC_CLK_ENABLE();
  __HAL_RCC_GPIOD_CLK_ENABLE();
  __HAL_RCC_GPIOE_CLK_ENABLE();

  /*** Configure the GPIO ***/
  /* Configure DCMI GPIO as alternate function */
  gpio_init_structure.Pin       = GPIO_PIN_4 | GPIO_PIN_6;
  gpio_init_structure.Mode      = GPIO_MODE_AF_PP;
  gpio_init_structure.Pull      = GPIO_PULLUP;
  gpio_init_structure.Speed     = GPIO_SPEED_HIGH;
  gpio_init_structure.Alternate = GPIO_AF13_DCMI;
  HAL_GPIO_Init(GPIOA, &gpio_init_structure);

  gpio_init_structure.Pin       = GPIO_PIN_7;
  gpio_init_structure.Mode      = GPIO_MODE_AF_PP;
  gpio_init_structure.Pull      = GPIO_PULLUP;
  gpio_init_structure.Speed     = GPIO_SPEED_HIGH;
  gpio_init_structure.Alternate = GPIO_AF13_DCMI;
  HAL_GPIO_Init(GPIOB, &gpio_init_structure);

  gpio_init_structure.Pin       = GPIO_PIN_6 | GPIO_PIN_7  | GPIO_PIN_8  |\
                                  GPIO_PIN_9 | GPIO_PIN_11;
  gpio_init_structure.Mode      = GPIO_MODE_AF_PP;
  gpio_init_structure.Pull      = GPIO_PULLUP;
  gpio_init_structure.Speed     = GPIO_SPEED_HIGH;
  gpio_init_structure.Alternate = GPIO_AF13_DCMI;
  HAL_GPIO_Init(GPIOC, &gpio_init_structure);

  gpio_init_structure.Pin       = GPIO_PIN_3 | GPIO_PIN_6;
  gpio_init_structure.Mode      = GPIO_MODE_AF_PP;
  gpio_init_structure.Pull      = GPIO_PULLUP;
  gpio_init_structure.Speed     = GPIO_SPEED_HIGH;
  gpio_init_structure.Alternate = GPIO_AF13_DCMI;
  HAL_GPIO_Init(GPIOD, &gpio_init_structure);

  gpio_init_structure.Pin       = GPIO_PIN_5 | GPIO_PIN_6;
  gpio_init_structure.Mode      = GPIO_MODE_AF_PP;
  gpio_init_structure.Pull      = GPIO_PULLUP;
  gpio_init_structure.Speed     = GPIO_SPEED_HIGH;
  gpio_init_structure.Alternate = GPIO_AF13_DCMI;
  HAL_GPIO_Init(GPIOE, &gpio_init_structure);

  /*** Configure the DMA ***/
  /* Set the parameters to be configured */
  hdma_eval.Init.Channel             = DMA_CHANNEL_1;
  hdma_eval.Init.Direction           = DMA_PERIPH_TO_MEMORY;
  hdma_eval.Init.PeriphInc           = DMA_PINC_DISABLE;
  hdma_eval.Init.MemInc              = DMA_MINC_ENABLE;
  hdma_eval.Init.PeriphDataAlignment = DMA_PDATAALIGN_WORD;
  hdma_eval.Init.MemDataAlignment    = DMA_MDATAALIGN_WORD;
  hdma_eval.Init.Mode                = DMA_CIRCULAR;
  hdma_eval.Init.Priority            = DMA_PRIORITY_HIGH;
  hdma_eval.Init.FIFOMode            = DMA_FIFOMODE_DISABLE;
  hdma_eval.Init.FIFOThreshold       = DMA_FIFO_THRESHOLD_FULL;
  hdma_eval.Init.MemBurst            = DMA_MBURST_SINGLE;
  hdma_eval.Init.PeriphBurst         = DMA_PBURST_SINGLE;

  hdma_eval.Instance = DMA2_Stream1;

  /* Associate the initialized DMA handle to the DCMI handle */
  __HAL_LINKDMA(hdcmi, DMA_Handle, hdma_eval);

  /*** Configure the NVIC for DCMI and DMA ***/
  /* NVIC configuration for DCMI transfer complete interrupt */
  HAL_NVIC_SetPriority(DCMI_IRQn, 5, 0);
  HAL_NVIC_EnableIRQ(DCMI_IRQn);

  /* NVIC configuration for DMA2D transfer complete interrupt */
  HAL_NVIC_SetPriority(DMA2_Stream1_IRQn, 5, 0);
  HAL_NVIC_EnableIRQ(DMA2_Stream1_IRQn);

  /* Configure the DMA stream */
  HAL_DMA_Init(hdcmi->DMA_Handle);
}

/**
  * @brief  DeInitializes the DCMI MSP.
  * @param  hdcmi: HDMI handle
  * @param  Params : pointer on additional configuration parameters, can be NULL.
  */
__weak void BSP_CAMERA_MspDeInit(DCMI_HandleTypeDef *hdcmi, void *Params)
{
    /* Disable NVIC  for DCMI transfer complete interrupt */
    HAL_NVIC_DisableIRQ(DCMI_IRQn);

    /* Disable NVIC for DMA2 transfer complete interrupt */
    HAL_NVIC_DisableIRQ(DMA2_Stream1_IRQn);

    /* Configure the DMA stream */
    HAL_DMA_DeInit(hdcmi->DMA_Handle);

    /* Disable DCMI clock */
    __HAL_RCC_DCMI_CLK_DISABLE();

    /* GPIO pins clock and DMA clock can be shut down in the application
       by surcharging this __weak function */
}

/**
  * @brief  Line event callback
  * @param  hdcmi: pointer to the DCMI handle
  */
void HAL_DCMI_LineEventCallback(DCMI_HandleTypeDef *hdcmi)
{
  BSP_CAMERA_LineEventCallback();
}

/**
  * @brief  Line Event callback.
  */
__weak void BSP_CAMERA_LineEventCallback(void)
{
  /* NOTE : This function Should not be modified, when the callback is needed,
            the HAL_DCMI_LineEventCallback could be implemented in the user file
   */
}

/**
  * @brief  VSYNC event callback
  * @param  hdcmi: pointer to the DCMI handle
  */
void HAL_DCMI_VsyncEventCallback(DCMI_HandleTypeDef *hdcmi)
{
  BSP_CAMERA_VsyncEventCallback();
}

/**
  * @brief  VSYNC Event callback.
  */
__weak void BSP_CAMERA_VsyncEventCallback(void)
{
  /* NOTE : This function Should not be modified, when the callback is needed,
            the HAL_DCMI_VsyncEventCallback could be implemented in the user file
   */
}

/**
  * @brief  Frame event callback
  * @param  hdcmi: pointer to the DCMI handle
  */
void HAL_DCMI_FrameEventCallback(DCMI_HandleTypeDef *hdcmi)
{
  BSP_CAMERA_FrameEventCallback();
}

/**
  * @brief  Frame Event callback.
  */
__weak void BSP_CAMERA_FrameEventCallback(void)
{
  /* NOTE : This function Should not be modified, when the callback is needed,
            the HAL_DCMI_FrameEventCallback could be implemented in the user file
   */
}

/**
  * @brief  Error callback
  * @param  hdcmi: pointer to the DCMI handle
  */
void HAL_DCMI_ErrorCallback(DCMI_HandleTypeDef *hdcmi)
{
  BSP_CAMERA_ErrorCallback();
}

/**
  * @brief  Error callback.
  */
__weak void BSP_CAMERA_ErrorCallback(void)
{
  /* NOTE : This function Should not be modified, when the callback is needed,
            the HAL_DCMI_ErrorCallback could be implemented in the user file
   */
}

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
