//
//    LLILUM OS Abstraction Layer - Platform
// 

#ifndef __LLOS_PLATFORM_H__
#define __LLOS_PLATFORM_H__

//
// Platform dependent definitions
//

#ifndef LLOS_PLATFORM_AES_KEY_LENGTH_BYTES
#define LLOS_PLATFORM_AES_KEY_LENGTH_BYTES 16
#endif // LLOS_PLATFORM_AES_KEY_LENGTH_BYTES

#ifndef LLOS_PLATFORM_HASH_LENGTH_BYTES
#define LLOS_PLATFORM_HASH_LENGTH_BYTES 32
#endif // LLOS_PLATFORM_HASH_LENGTH_BYTES

#ifndef LLOS_MALLOC
#define LLOS_MALLOC malloc
#endif // LLOS_MALLOC

#ifndef LLOS_REALLOC
#define LLOS_REALLOC realloc
#endif // LLOS_REALLOC

#ifndef LLOS_CALLOC
#define LLOS_CALLOC calloc
#endif // LLOS_CALLOC

#ifndef LLOS_FREE
#define LLOS_FREE free
#endif // LLOS_FREE

#ifndef LLOS_MEMSET
#ifndef _STRING_H_

#ifndef TARGET_STM32F411RE
extern void memset(void* addr, uint8_t fill, size_t size);
#else
extern void* memset(void* addr, int fill, size_t size);
#endif

#endif
#define LLOS_MEMSET memset
#endif // LLOS_MEMSET

#endif // __LLOS_PLATFORM_H__
