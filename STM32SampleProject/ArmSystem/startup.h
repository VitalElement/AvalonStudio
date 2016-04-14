/******************************************************************************
*       Description:
*
*       Author:
*         Date: January 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _STARTUP_H_
#define _STARTUP_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>


#ifdef __cplusplus
extern "C" {
#endif

#pragma mark Module Definitions and Constants


#pragma mark Module Types


#pragma mark Module Functions
/**
 * Initialises the data sections bss and data.
 */
extern void system_startup (void);


#ifdef __cplusplus
}
#endif

#endif
