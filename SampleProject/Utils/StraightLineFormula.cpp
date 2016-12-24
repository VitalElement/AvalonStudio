/******************************************************************************
*       Description:
*
*       Author:
*         Date: 01 June 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#pragma mark Includes
#include "StraightLineFormula.h"

#pragma mark Definitions and Constants


#pragma mark Static Data


#pragma mark Static Functions


#pragma mark Member Implementations
StraightLineFormula::StraightLineFormula ()
{
    this->m = 1;
    this->c = 0;
    this->y = 1;
}

StraightLineFormula::~StraightLineFormula ()
{
}

StraightLineFormula::StraightLineFormula (float m, float c)
{
    this->m = m;
    this->c = c;
    this->y = 1;
}

StraightLineFormula::StraightLineFormula (float m, float c, float y)
{
    this->m = m;
    this->c = c;
    this->y = y;
}

void StraightLineFormula::CalculateFrom (float x1, float x2, float y1, float y2,
                                         float x)
{
    CalculateMFrom (x1, x2, y1, y2);
    
    c = y1 - (m * x);
}

void StraightLineFormula::CalculateFrom (float x1, float x2, float y1, float y2)
{
    CalculateFrom (x1, x2, y1, y2, x1);
}

void StraightLineFormula::CalculateMFrom (float x1, float x2, float y1,
                                          float y2)
{
    if ((y2 - y1) != 0 && (x2 - x1) != 0)
    {
        m = (y2 - y1) / (x2 - x1);
    }
}


float StraightLineFormula::GetYForX (float x)
{
    return ((m * x) + c);
}

float StraightLineFormula::GetXForY (float y)
{
    return (y - c) / m;
}
