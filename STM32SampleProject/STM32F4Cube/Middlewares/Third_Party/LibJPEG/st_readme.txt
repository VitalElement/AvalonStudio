
  @verbatim
  ******************************************************************************
  *  
  *           Portions COPYRIGHT 2014 STMicroelectronics                       
  *           Portions Copyright (C) 1994-2011, Thomas G. Lane, Guido Vollbeding          
  *  
  * @file    st_readme.txt 
  * @author  MCD Application Team
  * @brief   This file lists the main modification done by STMicroelectronics on
  *          LibJPEG for integration with STM32Cube solution.  
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

### V1.0.1/23-December-2014 ###
===========================
   + jinclude.h: add newline at end of file to fix compilation warning.


### V1.0.0/19-June-2014 ###
========================
   + First customized version of LibJPEG V8d for STM32Cube solution.
   + LibJPEG is preconfigured to support by default the FatFs file system when
     media are processed from a FAT format media
   + The original “jmorecfg.h” and “jconfig.h” files was renamed into “jmorecfg_template.h”
     and “jconfig_template.h”, respectively. Two macros has been added to specify the memory
     allocation/Freeing methods.
     These two last files need to be copied to the application folder and customized
     depending on the application requirements.

 * <h3><center>&copy; COPYRIGHT STMicroelectronics</center></h3>
 */
