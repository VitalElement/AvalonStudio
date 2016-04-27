
  @verbatim
  ******************************************************************************
  *
  *           Portions COPYRIGHT 2015 STMicroelectronics
  *           Portions Copyright (C) 2006-2013, Brainspark B.V.
  *
  * @file    st_readme.txt 
  * @author  MCD Application Team
  * @brief   This file lists the main modification done by STMicroelectronics on
  *          PolarSSL for integration with STM32Cube solution.
  *          For more details on PolarSSL implementation on STM32Cube, please refer 
  *          to UM1723 "STM32CubeF4 PolarSSL example".  
  ******************************************************************************
  *
  * Licensed under MCD-ST Liberty SW License Agreement V2, (the "License");
  * You may not use this file except in compliance with the License.
  * You may obtain a copy of the License at:
  *
  *        http://www.st.com/software_license_agreement_liberty_v2
  *
  * Unless required by applicable law or agreed to in writing, software 
  * distributed under the License is distributed on an "AS IS" BASIS, 
  * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
  * See the License for the specific language governing permissions and
  * limitations under the License.
  *
  ******************************************************************************
  @endverbatim

### 27-March-2015 ###
=====================
   + Add support of the hardware Cryptographic and Hash processors embedded in STM32F756xx devices.
     This support is activated by defining "USE_STM32F7XX_HW_CRYPTO" macro in PolarSSL config.h file.
   + Fix some compilation warnings 


### 13-March-2015 ###
=====================
   + Align to latest version of STM32Cube CRYP HAL driver: add initialization of Crypto handler's Instance field


### 18-February-2014 ###
========================
   + PolarSSL V1.2.8 customized version for STM32Cube solution.
   + Source files modified to support the hardware Cryptographic and Hash processors embedded in
     STM32F415xx/417xx/437xx/439xx devices. This support is activated by defining
     "USE_STM32F4XX_HW_CRYPTO" macro in PolarSSL config.h file.


 * <h3><center>&copy; COPYRIGHT STMicroelectronics</center></h3>
 */
