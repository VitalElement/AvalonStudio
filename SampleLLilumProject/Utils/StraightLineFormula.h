/******************************************************************************
*       Description:
*
*       Author:
*         Date: 01 June 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas
#ifndef _STRAIGHTLINEFORMULA_H_
#define _STRAIGHTLINEFORMULA_H_

#pragma mark Includes
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>

class StraightLineFormula
{
#pragma mark Public Members
  public:
    StraightLineFormula ();

    StraightLineFormula (float m, float c);

    StraightLineFormula (float m, float c, float y);

    void CalculateMFrom (float x1, float x2, float y1, float y2);

    void CalculateFrom (float x1, float x2, float y1, float y2, float x);
    
    void CalculateFrom (float x1, float x2, float y1, float y2);

    float GetYForX (float x);

    float GetXForY (float y);

    ~StraightLineFormula ();


#pragma mark Private Members
  private:
    float m; // Gradient
    float c; // Intercept
    float y;
};

#endif
