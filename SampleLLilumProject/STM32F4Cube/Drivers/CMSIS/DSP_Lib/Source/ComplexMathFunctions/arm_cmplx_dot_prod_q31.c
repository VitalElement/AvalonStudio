/* ----------------------------------------------------------------------    
* Copyright (C) 2010-2014 ARM Limited. All rights reserved.    
*    
* $Date:        12. March 2014
* $Revision: 	V1.4.4
*    
* Project: 	    CMSIS DSP Library    
* Title:		arm_cmplx_dot_prod_q31.c    
*    
* Description:	Q31 complex dot product    
*    
* Target Processor: Cortex-M4/Cortex-M3/Cortex-M0
*  
* Redistribution and use in source and binary forms, with or without 
* modification, are permitted provided that the following conditions
* are met:
*   - Redistributions of source code must retain the above copyright
*     notice, this list of conditions and the following disclaimer.
*   - Redistributions in binary form must reproduce the above copyright
*     notice, this list of conditions and the following disclaimer in
*     the documentation and/or other materials provided with the 
*     distribution.
*   - Neither the name of ARM LIMITED nor the names of its contributors
*     may be used to endorse or promote products derived from this
*     software without specific prior written permission.
*
* THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
* "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
* LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
* FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE 
* COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
* INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
* BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
* LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
* CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
* LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN
* ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
* POSSIBILITY OF SUCH DAMAGE.   
* -------------------------------------------------------------------- */

#include "arm_math.h"

/**    
 * @ingroup groupCmplxMath    
 */

/**    
 * @addtogroup cmplx_dot_prod    
 * @{    
 */

/**    
 * @brief  Q31 complex dot product    
 * @param  *pSrcA points to the first input vector    
 * @param  *pSrcB points to the second input vector    
 * @param  numSamples number of complex samples in each vector    
 * @param  *realResult real part of the result returned here    
 * @param  *imagResult imaginary part of the result returned here    
 * @return none.    
 *    
 * <b>Scaling and Overflow Behavior:</b>    
 * \par    
 * The function is implemented using an internal 64-bit accumulator.    
 * The intermediate 1.31 by 1.31 multiplications are performed with 64-bit precision and then shifted to 16.48 format.    
 * The internal real and imaginary accumulators are in 16.48 format and provide 15 guard bits.    
 * Additions are nonsaturating and no overflow will occur as long as <code>numSamples</code> is less than 32768.    
 * The return results <code>realResult</code> and <code>imagResult</code> are in 16.48 format.    
 * Input down scaling is not required.    
 */

void arm_cmplx_dot_prod_q31(
  q31_t * pSrcA,
  q31_t * pSrcB,
  uint32_t numSamples,
  q63_t * realResult,
  q63_t * imagResult)
{
  q63_t real_sum = 0, imag_sum = 0;              /* Temporary result storage */
  q31_t a0,b0,c0,d0;

#ifndef ARM_MATH_CM0_FAMILY

  /* Run the below code for Cortex-M4 and Cortex-M3 */
  uint32_t blkCnt;                               /* loop counter */


  /*loop Unrolling */
  blkCnt = numSamples >> 2u;

  /* First part of the processing with loop unrolling.  Compute 4 outputs at a time.    
   ** a second loop below computes the remaining 1 to 3 samples. */
  while(blkCnt > 0u)
  {
      a0 = *pSrcA++;
      b0 = *pSrcA++;
      c0 = *pSrcB++;
      d0 = *pSrcB++;  
  
      real_sum += ((q63_t)a0 * c0) >> 14;
      imag_sum += ((q63_t)a0 * d0) >> 14;
      real_sum -= ((q63_t)b0 * d0) >> 14;
      imag_sum += ((q63_t)b0 * c0) >> 14;
      
      a0 = *pSrcA++;
      b0 = *pSrcA++;
      c0 = *pSrcB++;
      d0 = *pSrcB++;  
  
      real_sum += ((q63_t)a0 * c0) >> 14;
      imag_sum += ((q63_t)a0 * d0) >> 14;
      real_sum -= ((q63_t)b0 * d0) >> 14;
      imag_sum += ((q63_t)b0 * c0) >> 14;
      
      a0 = *pSrcA++;
      b0 = *pSrcA++;
      c0 = *pSrcB++;
      d0 = *pSrcB++;  
  
      real_sum += ((q63_t)a0 * c0) >> 14;
      imag_sum += ((q63_t)a0 * d0) >> 14;
      real_sum -= ((q63_t)b0 * d0) >> 14;
      imag_sum += ((q63_t)b0 * c0) >> 14;
      
      a0 = *pSrcA++;
      b0 = *pSrcA++;
      c0 = *pSrcB++;
      d0 = *pSrcB++;  
  
      real_sum += ((q63_t)a0 * c0) >> 14;
      imag_sum += ((q63_t)a0 * d0) >> 14;
      real_sum -= ((q63_t)b0 * d0) >> 14;
      imag_sum += ((q63_t)b0 * c0) >> 14;

      /* Decrement the loop counter */
      blkCnt--;
  }

  /* If the numSamples  is not a multiple of 4, compute any remaining output samples here.    
   ** No loop unrolling is used. */
  blkCnt = numSamples % 0x4u;

  while(blkCnt > 0u)
  {
      a0 = *pSrcA++;
      b0 = *pSrcA++;
      c0 = *pSrcB++;
      d0 = *pSrcB++;  
  
      real_sum += ((q63_t)a0 * c0) >> 14;
      imag_sum += ((q63_t)a0 * d0) >> 14;
      real_sum -= ((q63_t)b0 * d0) >> 14;
      imag_sum += ((q63_t)b0 * c0) >> 14;

      /* Decrement the loop counter */
      blkCnt--;
  }

#else

  /* Run the below code for Cortex-M0 */

  while(numSamples > 0u)
  {
      a0 = *pSrcA++;
      b0 = *pSrcA++;
      c0 = *pSrcB++;
      d0 = *pSrcB++;  
  
      real_sum += ((q63_t)a0 * c0) >> 14;
      imag_sum += ((q63_t)a0 * d0) >> 14;
      real_sum -= ((q63_t)b0 * d0) >> 14;
      imag_sum += ((q63_t)b0 * c0) >> 14;

      /* Decrement the loop counter */
      numSamples--;
  }

#endif /* #ifndef ARM_MATH_CM0_FAMILY */

  /* Store the real and imaginary results in 16.48 format  */
  *realResult = real_sum;
  *imagResult = imag_sum;
}

/**    
 * @} end of cmplx_dot_prod group    
 */
