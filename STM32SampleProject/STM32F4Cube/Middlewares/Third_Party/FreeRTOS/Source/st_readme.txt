
  @verbatim
  ******************************************************************************
  *
  *           Portions COPYRIGHT 2015 STMicroelectronics
  *           Portions Copyright (C) 2015 Real Time Engineers Ltd, All rights reserved
  *
  * @file    st_readme.txt 
  * @author  MCD Application Team
  * @brief   This file lists the main modification done by STMicroelectronics on
  *          FreeRTOS for integration with STM32Cube solution.
  *          For more details on FreeRTOS implementation on STM32Cube, please refer 
  *          to UM1722 "Developing Applications on STM32Cube with FreeRTOS"  
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
  The purpose of this release is to Upgrade to use FreeRTOS V8.2.1.

  + Major change of the version 8.2.1 is the support of CM7 core. 
  For STM32F746xx/STM32F756xx devices, need to use port files under Source/Portable/XXX/ARM_CM7/r0p1,
  where XXX refers to the compiler used.

  + It also provides implementation of osSignal management APIs, osSignalSet() and osSignalWait(),
  fixes osMassage queue size, osMailQDef macro and osDelayUntil parameters.
  
  + In this release an alignment has been done in ARM_CM4 and ARM_CM3 port.c versus ARM_CM0 port.c 
  regarding the use of macros configPRE_SLEEP_PROCESSING and configPOST_SLEEP_PROCESSING, these tow macros
  are now taking as parameter as pointer to TickType_t.  

  + cmsis_os.c
    - Add implementation of osSignalSet() and osSignalWait() APIs
    - Fix massage queue size in osMessageCreate API
    - osDelayUntil: parameter PreviousWakeTime is now passed as a pointer. 
    - Enabling Mail queue management APIs (temporary removed in previous version).
    - Function "osThreadGetPriority" uses now uxTaskPriorityGetFromISR if called from an interrupt handler, if not use uxTaskPriorityGet.

  + cmsis_os.h
    - osFeature_Wait is defined to 0 to indicate that osWait function is not available (as specified by cmsis_os template by ARM)  
    - Fix compilation issue with osMailQDef macro.
    - Enabling Mail queue management APIs (temporary removed in previous version)

  + freeRTOS sources : 
    - ARM_CM3 port.c and ARM_CM4 port.c:
      function vPortSuppressTicksAndSleep : configPRE_SLEEP_PROCESSING and configPOST_SLEEP_PROCESSING are now taking
      as parameter as pointer to TickType_t.
      The purpose of this change is to align the CM3 and CM4 implementation with CM0 one.       

  + Note :
    - osSignalSet returns an int32_t value which is a a status (osOK or osError) 
      instead of the previous signal value as specified in cmsis_os template by ARM. 
      This is mainly due to freeRTOS implementation, the return value will be aligned (with the cmsis os template by ARM) as soon as the freeRTOS next version will allow it.

    - osThreadDef() macro is defined in the freeRTOS cmsis_os.h wrapper as follow : 
        osThreadDef(name, thread, priority, instances, stacksz)
      the macro osThreadDef() as defined in ARM cmsis_os.h is defined with 4 parameters :
      name : name of the thread function. 
      priority : initial priority of the thread function.
      instances : number of possible thread instances.
      stacksz : stack size (in bytes) requirements for the thread function.
      
    - osThreadDef as defined in the ARM template file cmsis_os.h assumes that the thread name is the same as the thread function name.
      where the freeRTOS implementation gives separate parameters for the thread name and the thread function name.
      
      care must be taken when porting an application from/to another OS to/from freeRTOS cmsis_os regarding this macro.
      
        the macro osThreadDef() as defined in ARM cmsis_os.h template is defined with 4 parameters :
           name : name of the thread function.
           priority : initial priority of the thread function.
           instances : number of possible thread instances.
           stacksz : stack size (in bytes) requirements for the thread function.
        
        the macro osThreadDef() as defined in freeRTOS cmsis_os.h is defined with 5 parameters :
           name : name of the thread (used for debugging and trace).
           thread : name of the thread function
           priority : initial priority of the thread function.
           instances : number of possible thread instances.
           stacksz : stack size (in bytes) requirements for the thread function.


### 25-December-2014 ###
========================
  The purpose of this release is to remove compilation errors and warning. It also reintroduces
  the function osThreadIsSuspended() which has been removed in the version V1.2.0.

  + cmsis_os.c
      - osThreadGetPriority() and makeCmsisPriority(): replace INCLUDE_vTaskPriorityGet by the correct
         freeRTOS constant uxTaskPriorityGet.
         The version 1.2.2 is using a wrong constant INCLUDE_vTaskPriorityGet, while the correct freeRTOS
         constant is uxTaskPriorityGet.
         This fix ensure a safe use of osThreadGetPriority() function.

      - osThreadIsSuspended(): this function has been removed in version V1.2.0, it is now available gain.
         User can either use this function to check if a Thread is suspended either use function osThreadGetState,
         which is more generic, to check the exact state of a thread.
                                               
      - osThreadList(): this function is now taking as argument a pointer to uint8_t instead of a pointer to int8_t.
         The change is made to remove a compilation warning.

      - osRecursiveMutexCreate(): the prototype has been changed to osMutexId osRecursiveMutexCreate (const osMutexDef_t *mutex_def)
          This change is made to make osRecursiveMutexCreate() compatible with function MutexCreate().
          It also allow the better use of the function in conjunction with the macro osMutex, note that osMutex return a
          "const osMutexDef_t *mutex_def".
          example : osMutex1Id = osRecursiveMutexCreate (osMutex(Mutex1));

      - Fix implementation of functions osSemaphoreWait(), osMutexRelease() and osMutexWait() by using the appropriate
         freeRTOS “FromISR” APIs when called from an interrupt.

      - Fix compilation warning when the constant INCLUDE_eTaskGetState is not defined

  + cmsis_os.h
      - osThreadIsSuspended(): add function prototype.
      - osThreadList(): function prototype modified as described in cmsis_os.c section.
      - osRecursiveMutexCreate(): function modified as described in cmsis_os.c section.

  + Important note:
    Mail Queue Management Functions are not supported in this cmsis_os version, will be added in the next release.


### 04-December-2014 ###
========================
  + cmsis_os.c, osSemaphoreCreate(): use vSemaphoreCreateBinary() instead of xSemaphoreCreateBinary(),
    to keep compatibility with application code developed on FreeRTOS V7.6.0.


### 07-November-2014 ###
========================
  + cmsis_os.h: modify the osThreadState enum to fix warning generated by ARMCC compiler
  + task.c: add preprocessor compilation condition for prvTaskIsTaskSuspended() function 
            (it's build only when INCLUDE_vTaskSuspend option is enabled in FreeRTOSConfig.h file)


### 04-November-2014 ###
========================
  + Upgrade to use FreeRTOS V8.1.2 and CMSIS-RTOS V1.02.
  + cmsis_os.c
      - Almost of CMSIS-RTOS APIs are implemented for FreeRTOS
      - Additional wrapper APIs created for FreeRTOS

  + Important note:
    When upgrading existing application code to use this last version, the following 
    update should be considered:
      - osThreadIsSuspended() is no longer public API in FreeRTOS and it should
        be replaced by the wrapping of eTaskGetState()
      - osKernelStart() API changed, must be updated
      - update FreeRTOSConfig.h file, taking FreeRTOSConfig_template.h file as reference


### 13-June-2014 ###
====================
  + FreeRTOSConfig_template.h: add this definition #define INCLUDE_xTaskGetSchedulerState 1
                               to enable the use of xTaskGetSchedulerState() API in the 
                               application code. 


### 30-April-2014 ###
=====================
  + cmsis_os.c: add preprocessor compilation condition when calling some FreeRTOS APIs, to avoid link
                errors with MDK-ARM when some FreeRTOS features are not enabled in FreeRTOSConfig.h


### 22-April-2014 ###
=====================
  + Add Tickles mode for CM0 port (IAR, GCC, RVDS).


### 18-February-2014 ###
========================
   + FreeRTOS V7.6.0 customized version for STM32Cube solution.


 * <h3><center>&copy; COPYRIGHT STMicroelectronics</center></h3>
 */
