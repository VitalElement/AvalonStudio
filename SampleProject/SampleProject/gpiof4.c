//=================================================================================================
// STM32F4 GPIO Configuration
// Author : Radoslaw Kwiecien
// e-mail : radek@dxp.pl
// http://en.radzio.dxp.pl/
//=================================================================================================
#include <stm32f4xx.h>
#include "gpiof4.h"
//=================================================================================================
// gpio_conf
//=================================================================================================
void gpio_conf(GPIO_TypeDef * GPIO, uint8_t pin, uint8_t mode, uint8_t type, uint8_t speed, uint8_t pullup, uint8_t af)
{
		GPIO->MODER 	= (GPIO->MODER   & MASK2BIT(pin))   | (mode << (pin * 2));
		GPIO->OTYPER 	= (GPIO->OTYPER  & MASK1BIT(pin))   | (type << pin);
		GPIO->OSPEEDR = (GPIO->OSPEEDR & MASK2BIT(pin))   | (speed << (pin * 2));
		GPIO->PUPDR		= (GPIO->PUPDR   & MASK2BIT(pin))		| (pullup << (pin * 2));
		if(pin > 7)
			{
				GPIO->AFR[1] = (GPIO->AFR[1] & AFMASKH(pin)) | (af << ((pin - 8) * 4));
			}
		else
			{
				GPIO->AFR[0] = (GPIO->AFR[0] & AFMASKL(pin)) | (af << ((pin) * 4));
			}
}
//=================================================================================================
// End of file
//=================================================================================================
