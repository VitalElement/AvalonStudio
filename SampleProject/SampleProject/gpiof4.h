//=================================================================================================
// STM32F4 GPIO Configuration
// Author : Radoslaw Kwiecien
// e-mail : radek@dxp.pl
// http://en.radzio.dxp.pl/
//=================================================================================================
#include <stm32f4xx.h>
//=================================================================================================
//
//=================================================================================================
#define MODE_INPUT 	0
#define MODE_GPIO		1
#define MODE_AF			2
#define MODE_ANALOG	3

#define TYPE_PUSHPULL 	0
#define TYPE_OPENDRAIN 	1

#define SPEED_2MHz		0
#define SPEED_25MHz		1
#define SPEED_50MHz		2
#define SPEED_100MHz	3

#define PULLUP_NONE		0
#define PULLUP_UP			1
#define PULLUP_DOWN		2

#define MASK1BIT(pin) ((uint32_t)~(1 << (pin * 1)))
#define MASK2BIT(pin) ((uint32_t)~(3 << (pin * 2)))
#define MASK4BIT(pin) ((uint32_t)~(15 << (pin * 4)))
#define AFMASKL(pin)	((uint32_t)~(15 << (pin * 4)))
#define AFMASKH(pin)	((uint32_t)~(15 << ((pin - 8) * 4)))
//=================================================================================================
//
//=================================================================================================
void gpio_conf(GPIO_TypeDef * GPIO, uint8_t pin, uint8_t mode, uint8_t type, uint8_t speed, uint8_t pullup, uint8_t af);
//=================================================================================================
//
//=================================================================================================
