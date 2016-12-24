/******************************************************************************
*       Description:
*
*       Author:
*         Date: 30 March 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _CIPWM_H_
#define _CIPWM_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>

#ifdef __cplusplus
extern "C" {
#endif

#pragma mark Module Definitions and Constants
typedef void (*CAction)(void);
typedef void (*ObjectAction)(void*);
typedef void (*SetCount)(uint32_t count);
typedef void (*SetFrequency)(float frequencyHz);

typedef struct
{
    CAction Start;
    CAction Stop;

    SetCount SetCount;
    SetFrequency SetFrequency;

    ObjectAction CycleCompleted;
} CIPwmChannel;

#pragma mark Module Types


#pragma mark Module Functions


#ifdef __cplusplus
}
#endif

#endif
