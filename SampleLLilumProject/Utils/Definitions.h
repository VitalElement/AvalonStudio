/******************************************************************************
*       Description: 
*
*       Author: 
*         Date: 27 April 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _DEFINITIONS_H_
#define _DEFINITIONS_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>


#ifdef __cplusplus
extern "C" {
#endif

#pragma mark Module Definitions and Constants
#if defined   (__GNUC__)        /* GNU Compiler */
  #ifndef __ALIGN_END
    #define __ALIGN_END    __attribute__ ((aligned (4)))
  #endif /* __ALIGN_END */
  #ifndef __ALIGN_BEGIN  
    #define __ALIGN_BEGIN
  #endif /* __ALIGN_BEGIN */
#else
  #ifndef __ALIGN_END
    #define __ALIGN_END
  #endif /* __ALIGN_END */
  #ifndef __ALIGN_BEGIN      
    #if defined   (__CC_ARM)      /* ARM Compiler */
      #define __ALIGN_BEGIN    __align(4)  
    #elif defined (__ICCARM__)    /* IAR Compiler */
      #define __ALIGN_BEGIN 
    #endif /* __CC_ARM */
  #endif /* __ALIGN_BEGIN */
#endif /* __GNUC__ */

#pragma mark Module Types


#pragma mark Module Functions


#ifdef __cplusplus
}
#endif

#endif
