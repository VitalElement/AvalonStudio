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
File        : FRAMEWIN_Private.h
Purpose     : FRAMEWIN private header file
--------------------END-OF-HEADER-------------------------------------
*/

#ifndef FRAMEWIN_PRIVATE_H
#define FRAMEWIN_PRIVATE_H

#include "WM.h"
#include "FRAMEWIN.h"
#include "WIDGET.h"
#include "GUI_HOOK.h"

#if GUI_WINSUPPORT

/******************************************************************
*
*       Config defaults
*
*******************************************************************
*/
//
//Support for 3D effects
//
#ifndef FRAMEWIN_CLIENTCOLOR_DEFAULT
  #if WIDGET_USE_FLEX_SKIN
    #define FRAMEWIN_CLIENTCOLOR_DEFAULT GUI_WHITE
  #else
    #define FRAMEWIN_CLIENTCOLOR_DEFAULT 0xc0c0c0
  #endif
#endif

//
// Default for top frame size
//
#ifndef FRAMEWIN_TITLEHEIGHT_DEFAULT
  #define FRAMEWIN_TITLEHEIGHT_DEFAULT 0
#endif

//
// Default for left/right/top/bottom frame size
//
#ifndef FRAMEWIN_BORDER_DEFAULT
  #define FRAMEWIN_BORDER_DEFAULT 3
#endif

//
// Default for inner frame size
//
#ifndef FRAMEWIN_IBORDER_DEFAULT
  #define FRAMEWIN_IBORDER_DEFAULT 1
#endif

//
// Default font
//
#ifndef FRAMEWIN_DEFAULT_FONT
  #if WIDGET_USE_FLEX_SKIN
    #if   WIDGET_USE_SCHEME_SMALL
      #define FRAMEWIN_DEFAULT_FONT &GUI_Font13_1
    #elif WIDGET_USE_SCHEME_MEDIUM
      #define FRAMEWIN_DEFAULT_FONT &GUI_Font16_1
    #elif WIDGET_USE_SCHEME_LARGE
      #define FRAMEWIN_DEFAULT_FONT &GUI_Font24_1
    #endif
  #else
    #if   WIDGET_USE_SCHEME_SMALL
      #define FRAMEWIN_DEFAULT_FONT &GUI_Font8_1
    #elif WIDGET_USE_SCHEME_MEDIUM
      #define FRAMEWIN_DEFAULT_FONT &GUI_Font13_1
    #elif WIDGET_USE_SCHEME_LARGE
      #define FRAMEWIN_DEFAULT_FONT &GUI_Font16_1
    #endif
  #endif
#endif

//
// Default bar color when framewin is active
//
#ifndef FRAMEWIN_BARCOLOR_ACTIVE_DEFAULT
  #define FRAMEWIN_BARCOLOR_ACTIVE_DEFAULT 0xFF0000
#endif

//
// Default bar color when framewin is inactive
//
#ifndef FRAMEWIN_BARCOLOR_INACTIVE_DEFAULT
  #define FRAMEWIN_BARCOLOR_INACTIVE_DEFAULT 0x404040
#endif

//
// Default frame color
//
#ifndef FRAMEWIN_FRAMECOLOR_DEFAULT
  #define FRAMEWIN_FRAMECOLOR_DEFAULT 0xAAAAAA
#endif

//
// Default text color when framewin is active
//
#ifndef FRAMEWIN_TEXTCOLOR_INACTIVE_DEFAULT
  #if WIDGET_USE_FLEX_SKIN
    #define FRAMEWIN_TEXTCOLOR_INACTIVE_DEFAULT GUI_BLACK
  #else
    #define FRAMEWIN_TEXTCOLOR_INACTIVE_DEFAULT GUI_WHITE
  #endif
#endif

//
// Default text color when framewin is inactive
//
#ifndef FRAMEWIN_TEXTCOLOR_ACTIVE_DEFAULT
  #if WIDGET_USE_FLEX_SKIN
    #define FRAMEWIN_TEXTCOLOR_ACTIVE_DEFAULT GUI_BLACK
  #else
    #define FRAMEWIN_TEXTCOLOR_ACTIVE_DEFAULT GUI_WHITE
  #endif
#endif

//
// Default text alignment
//
#ifndef FRAMEWIN_TEXTALIGN_DEFAULT
  #define FRAMEWIN_TEXTALIGN_DEFAULT GUI_TA_LEFT
#endif

/*********************************************************************
*
*       Object definition
*
**********************************************************************
*/
typedef struct {
  unsigned              (* pfGetBordersize)(FRAMEWIN_Handle hObj, unsigned Index);
  WIDGET_DRAW_ITEM_FUNC  * pfDrawSkin;
} FRAMEWIN_SKIN_PRIVATE;

typedef struct {
  const GUI_FONT      * pFont;
  GUI_COLOR             aBarColor[2];
  GUI_COLOR             aTextColor[2];
  GUI_COLOR             ClientColor;
  FRAMEWIN_SKIN_PRIVATE SkinPrivate;
  I16                   TitleHeight;
  I16                   BorderSize;
  I16                   IBorderSize;
  I16                   TextAlign;
} FRAMEWIN_PROPS;

typedef struct {
  WIDGET                  Widget;
  FRAMEWIN_PROPS          Props;
  WIDGET_SKIN const     * pWidgetSkin;
  WIDGET_DRAW_ITEM_FUNC * pfDrawItem;      // Only for drawing the title bar...
  WM_CALLBACK           * cb;
  WM_HWIN                 hClient;
  WM_HWIN                 hMenu;
  WM_HWIN                 hText;
  GUI_RECT                rRestore;
  U16                     Flags;
  WM_HWIN                 hFocussedChild;  // Handle to focussed child .. default none (0)
  WM_DIALOG_STATUS      * pDialogStatus;
  GUI_HOOK              * pFirstHook;
} FRAMEWIN_Obj;

typedef struct {
  I16      TitleHeight;
  I16      MenuHeight;
  GUI_RECT rClient;
  GUI_RECT rTitleText;
} FRAMEWIN_POSITIONS;

/*********************************************************************
*
*       Macros for internal use
*
**********************************************************************
*/
#if GUI_DEBUG_LEVEL >= GUI_DEBUG_LEVEL_CHECK_ALL
  #define FRAMEWIN_INIT_ID(p) (p->Widget.DebugId = FRAMEWIN_ID)
#else
  #define FRAMEWIN_INIT_ID(p)
#endif

#if GUI_DEBUG_LEVEL >= GUI_DEBUG_LEVEL_CHECK_ALL
  FRAMEWIN_Obj * FRAMEWIN_LockH(FRAMEWIN_Handle h);
  #define FRAMEWIN_LOCK_H(h)   FRAMEWIN_LockH(h)
#else
  #define FRAMEWIN_LOCK_H(h)   (FRAMEWIN_Obj *)GUI_LOCK_H(h)
#endif

/*********************************************************************
*
*       Public data (internal defaults)
*
**********************************************************************
*/
extern FRAMEWIN_PROPS      FRAMEWIN__DefaultProps;

extern const WIDGET_SKIN   FRAMEWIN__SkinClassic;
extern       WIDGET_SKIN   FRAMEWIN__Skin;

extern const WIDGET_SKIN * FRAMEWIN__pSkinDefault;

/*********************************************************************
*
*       Private functions
*
**********************************************************************
*/
void     FRAMEWIN__CalcPositions  (FRAMEWIN_Handle hObj, FRAMEWIN_POSITIONS * pPos);
int      FRAMEWIN__CalcTitleHeight(FRAMEWIN_Obj * pObj);
void     FRAMEWIN__UpdatePositions(FRAMEWIN_Handle hObj);
void     FRAMEWIN__UpdateButtons  (FRAMEWIN_Handle hObj, int OldHeight, int OldBorderSizeL, int OldBorderSizeR, int OldBorderSizeT);
void     FRAMEWIN__GetTitleLimits (FRAMEWIN_Handle hObj, int * pxMin, int * pxMax);
unsigned FRAMEWIN__GetBorderSize  (FRAMEWIN_Handle hObj, unsigned Index);

#endif   // GUI_WINSUPPORT
#endif   // FRAMEWIN_PRIVATE_H
