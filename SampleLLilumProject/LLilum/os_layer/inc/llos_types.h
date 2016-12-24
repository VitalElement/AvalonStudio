//
//    LLILUM OS Abstraction Layer - Types
// 

#ifndef __LLOS_TYPES_H__
#define __LLOS_TYPES_H__

#if defined(WIN32) || defined(_WIN32)
#include <inttypes.h>
#else
#include <stdint.h>
#endif // WIN32 || _WIN32

#include <llos_platform.h>
#include <llos_error.h>

#ifndef OS_TCB_H
typedef uint32_t      BOOL;
#endif

typedef void          VOID;
typedef uint32_t      LLOS_Address;
typedef void*         LLOS_Opaque;
typedef uint64_t      LLOS_Ticks;
typedef uint64_t      LLOS_Time;
typedef uint64_t      LLOS_TimeSpan;
typedef LLOS_Time     LLOS_Timeout;
typedef LLOS_Opaque   LLOS_Context;
typedef int8_t        LLOS_AesKey[ LLOS_PLATFORM_AES_KEY_LENGTH_BYTES ];
typedef int8_t        LLOS_Hash  [ LLOS_PLATFORM_HASH_LENGTH_BYTES    ];
//--//

#define LLOS_Infinite_Timeout ((LLOS_Timespan)(-1ll))

//--//

typedef uint32_t LLOS_Com;

// 
//  Async types
//
typedef enum LLOS_AsyncStatus
{
    LLOS_Created = 0,
    LLOS_Scheduled,
    LLOS_Running,
    LLOS_Cancelling,
    LLOS_Completed,
    LLOS_CompletedWithTimeout,
    LLOS_CompletedWithError,
    LLOS_CompletedWithCancellation,
} LLOS_AsyncStatus;

typedef VOID( *LLOS_Callback )( LLOS_Context );


typedef struct LLOS_AddressRange
{
    LLOS_Address Start;
    LLOS_Address End;
} LLOS_AddressRange;

typedef struct LLOS_BBuffer
{
    uint32_t Length;
    uint8_t  Data[0];
} LLOS_BBuffer;

typedef struct LLOS_WBuffer
{
    uint32_t Length;
    uint16_t Data[0];
} LLOS_WBuffer;

#endif // __LLOS_TYPES_H__
