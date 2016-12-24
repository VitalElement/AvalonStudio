/******************************************************************************
*       Description:
*
*       Author:
*         Date: 30 March 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _ACTION_H_
#define _ACTION_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>

#include "function.h"
//#include <functional>

typedef func::function<void(void)> Action;
typedef func::function<bool(void)> Predicate;


#endif
