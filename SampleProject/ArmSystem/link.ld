/**
 *		Vital Element Studios - LinkerScript
 *
 **/
 
OUTPUT_FORMAT ("elf32-littlearm", "elf32-bigarm", "elf32-littlearm")

MEMORY
{
	FLASH (rx)  : ORIGIN = 0x08000000, LENGTH = 0x00100000
	RAM (rwx) : ORIGIN = 0x20000000, LENGTH = 0x00020000
}

_eram = 0x20000000 + 0x00020000;

SECTIONS
{
	.text :
	{
		KEEP(*(.interrupt_vector))
		*(.text*)
		
		KEEP(*(.init))
		KEEP(*(.fini))
		
		/* .ctors */
		*crtbegin.o(.ctors)
		*crtbegin?.o(.ctors)
		*(EXCLUDE_FILE(*crtend?.o *crtend.o) .ctors)
		*(SORT(.ctors.*))
		*(.ctors)
		
		/* .dtors */
		*crtbegin.o(.dtors)
		*crtbegin?.o(.dtors)
		*(EXCLUDE_FILE(*crtend?.o *crtend.o) .dtors)
		*(SORT(.dtors.*))
		*(.dtors)
		
		*(.rodata*)
		
		KEEP(*(.eh_fram e*))
	} > FLASH
    
    	.ARM.extab : 
	{
		*(.ARM.extab* .gnu.linkonce.armextab.*)
	} > FLASH 
	
	__exidx_start = .;
	.ARM.exidx :
	{
		*(.ARM.exidx* .gnu.linkonce.armexidx.*)
	} > FLASH 
	__exidx_end = .;
	__etext = .;
	
    _data_flash = .;
    
    .data : AT ( _data_flash )
	{
		. = ALIGN(4);
		_data_begin = .;
		*(vtable)
		*(.data)
		*(.data*)

		        . = ALIGN(4);
        /* preinit data */
        PROVIDE_HIDDEN (__preinit_array_start = .);
        KEEP(*(.preinit_array))
        PROVIDE_HIDDEN (__preinit_array_end = .);
 
        . = ALIGN(4);
        /* init data */
        PROVIDE_HIDDEN (__init_array_start = .);
        KEEP(*(SORT(.init_array.*)))
        KEEP(*(.init_array))
        PROVIDE_HIDDEN (__init_array_end = .);
 
 
        . = ALIGN(4);
        /* finit data */
        PROVIDE_HIDDEN (__fini_array_start = .);
        KEEP(*(SORT(.fini_array.*)))
        KEEP(*(.fini_array))
        PROVIDE_HIDDEN (__fini_array_end = .);
 
        KEEP(*(.jcr*))

		. = ALIGN(4);
		_data_end = .;
	} > RAM
    
    .bss :
	{
		_bss_begin = .;
		__bss_start__ = _bss_begin;
		*(.bss)
		*(.bss*)
		*(COMMON)
		. = ALIGN(4);
		_bss_end = .;
		__bss_end__ = _bss_end;
	} > RAM
}

/* end of allocated ram _end */
PROVIDE( _HEAP_START = _bss_end );

/* end of the heap -> align 8 byte */ 
PROVIDE ( _HEAP_END = ALIGN(ORIGIN(RAM) + LENGTH(RAM) ,8) );


ENTRY(Default_Reset_Handler);
