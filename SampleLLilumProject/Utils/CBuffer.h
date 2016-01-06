/******************************************************************************
*       Description:
*
*       Author:
*         Date: 27 April 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _BUFFER_H_
#define _BUFFER_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>


#ifdef __cplusplus
extern "C" {
#endif

#pragma mark Module Definitions and Constants
typedef struct
{
    uint8_t* elements;
    uint32_t length;
} Buffer;

#pragma mark Module Types


#pragma mark Module Functions


#ifdef __cplusplus
}
#endif

#endif
