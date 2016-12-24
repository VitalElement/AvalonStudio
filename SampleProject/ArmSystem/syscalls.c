/******************************************************************************
*       Description:
*
*       Author:
*         Date: January 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#pragma mark Includes


#pragma mark Definitions and Constants


#pragma mark Static Data


#pragma mark Static Functions


#pragma mark Function Implementations
#undef errno
extern int errno;
#include <stdint.h>
int _kill (int pid, int sig)
{
    //    pid = pid; sig = sig; / avoid warnings /
    return -1;
}

void _exit (int status)
{
    //    xprintf("_exit called with parameter %d\n", status);
    while (1)
    {
        ;
    }
}

int _getpid (void)
{
    return 1;
}


char get_heap_end (void)
{
    return (char)0;
}

char get_stack_top (void)
{
    return (char)0;
}


int _close (int file)
{
    //    file = file; / avoid warning /
    return -1;
}

int _open (int file)
{
    return -1;
}


int _isatty (int file)
{
    //    file = file; / avoid warning /
    return 1;
}

int _lseek (int file, int ptr, int dir)
{
    //    file = file; / avoid warning /
    //    ptr = ptr; / avoid warning /
    //    dir = dir; / avoid warning /
    return 0;
}

int _read (int file, char ptr, int len)
{
    //    file = file; / avoid warning /
    //    ptr = ptr; / avoid warning /
    //    len = len; / avoid warning /
    return 0;
}

int _fstat (int file, uint32_t* st)
{
    // st->st_mode = S_IFCHR;
    return 0;
}

/* weak so can be overriden by retargetting.*/
__attribute__ ((weak)) int _write (int file, char ptr, int len)
{
    int todo;
    //    file = file; / avoid warning /
    for (todo = 0; todo < len; todo++)
    {
        //        xputc(*ptr++);
    }
    return len;
}