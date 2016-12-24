/*********************************************************************
*                SEGGER Microcontroller GmbH & Co. KG                *
*        Solutions for real time microcontroller applications        *
**********************************************************************
*                                                                    *
*        (c) 1996 - 2015  SEGGER Microcontroller GmbH & Co. KG       *
*                                                                    *
*        Internet: www.segger.com    Support:  support@segger.com    *
*                                                                    *
**********************************************************************

** emWin V5.28 - Graphical user interface for embedded applications **
All  Intellectual Property rights  in the Software belongs to  SEGGER.
emWin is protected by  international copyright laws.  Knowledge of the
source code may not be used to write a similar product.  This file may
only be used in accordance with the following terms:

The  software has  been licensed  to STMicroelectronics International
N.V. a Dutch company with a Swiss branch and its headquarters in Plan-
les-Ouates, Geneva, 39 Chemin du Champ des Filles, Switzerland for the
purposes of creating libraries for ARM Cortex-M-based 32-bit microcon_
troller products commercialized by Licensee only, sublicensed and dis_
tributed under the terms and conditions of the End User License Agree_
ment supplied by STMicroelectronics International N.V.
Full source code is available at: www.segger.com

We appreciate your understanding and fairness.
----------------------------------------------------------------------
File        : GUIDRV_Dist.h
Purpose     : Interface definition for GUIDRV_Dist driver
---------------------------END-OF-HEADER------------------------------
*/

#ifndef GUIDRV_DIST_H
#define GUIDRV_DIST_H

#if defined(__cplusplus)
extern "C" {     /* Make sure we have C-declarations in C++ programs */
#endif

/*********************************************************************
*
*       Display driver
*/
//
// Address
//
extern const GUI_DEVICE_API GUIDRV_Dist_API;

//
// Macros to be used in configuration files
//
#if defined(WIN32) && !defined(LCD_SIMCONTROLLER)

  #define GUIDRV_DIST &GUIDRV_Win_API

#else

  #define GUIDRV_DIST &GUIDRV_Dist_API

#endif

/*********************************************************************
*
*       Public routines
*/
#if defined(WIN32) && !defined(LCD_SIMCONTROLLER)

  #define GUIDRV_Dist_AddDriver(pDevice, pDriver, pRect)

#else

  void GUIDRV_Dist_AddDriver(GUI_DEVICE * pDevice, GUI_DEVICE * pDriver, GUI_RECT * pRect);

#endif

#if defined(__cplusplus)
}
#endif

#endif /* GUIDRV_DIST_H */

/*************************** End of file ****************************/
